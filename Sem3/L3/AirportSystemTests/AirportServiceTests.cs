using AirportSystem.Application.Interfaces;
using AirportSystem.Application.Services;
using AirportSystem.Domain.Aggregates;
using AirportSystem.Domain.Entities;
using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Exceptions.Country;
using AirportSystem.Domain.Exceptions.Money;
using AirportSystem.Domain.Exceptions.Route;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;
using AirportSystem.Domain.ValueObjects;
using AirportSystem.Infrastructure.Repositories;

namespace AirportSystemTests;

public class AirportServiceTests
{
    private const Currency TestCurrency = Currency.Eur;
    private readonly Money _testPrice = new(100, TestCurrency);
    private readonly Money _passengerStartMoney = new(1000, TestCurrency);

    private (IAirplaneRepository, IFlightRepository, IPassengerRepository, IAirportCompanyRepository, 
        IPilotRepository, IFlightAttendantRepository, IMaintenanceTechnicianRepository, 
        IFleetManagementService, IFlightManagementService, IAirplaneMaintenanceService, 
        IFinancialService, IHumanResourcesService, IBookingService, IFlightOperationsService) 
        SetupFullSystem()
    {
        var airplaneRepo = new InMemoryAirplaneRepository();
        var flightRepo = new InMemoryFlightRepository();
        var passengerRepo = new InMemoryPassengerRepository();
        var companyRepo = new InMemoryAirportCompanyRepository();
        var pilotRepo = new InMemoryPilotRepository();
        var attendantRepo = new InMemoryFlightAttendantRepository();
        var techRepo = new InMemoryMaintenanceTechnicianRepository();

        var fleetService = new FleetManagementService(airplaneRepo);
        var flightService = new FlightManagementService(flightRepo);
        var maintenanceService = new AirplaneMaintenanceService();
        var financialService = new FinancialService(companyRepo);
        var hrService = new HumanResourcesService(attendantRepo, techRepo, pilotRepo);
        var bookingService = new BookingService(flightRepo, passengerRepo, financialService);
        var operationsService = new FlightOperationsService(flightRepo);

        return (airplaneRepo, flightRepo, passengerRepo, companyRepo, pilotRepo, attendantRepo, techRepo,
            fleetService, flightService, maintenanceService, financialService, hrService, bookingService, operationsService);
    }

    private IAirplane CreateTestAirplane(
        IFleetManagementService fleetService, 
        uint capacity = 100, 
        uint maxBaggage = 5000)
    {
        var specs = new AirplaneSpecs("Test", 2020, "TestEngine");
        var plane = fleetService.PurchaseAirplaneWithSpecs(
            "TestPlane", capacity, maxBaggage, _testPrice, specs);
        return plane;
    }

    private IFlight CreateTestFlight(
        IFlightManagementService flightService, 
        IHumanResourcesService hrService, 
        IAirplane airplane)
    {
        var pilot = hrService.HirePilot("TestPilot", 40, Gender.Male, _testPrice);
        var route = new Route(new Country("TestCountry"), 1000);
        var flight = flightService.CreateFlight(airplane, [pilot], route, _testPrice);
        return flight;
    }

    private IPassenger CreateTestPassenger(
        IPassengerRepository passengerRepo, 
        Money? money = null)
    {
        var passenger = new Passenger(
            "TestPassenger", 30, Gender.Female, money ?? _passengerStartMoney, null, null);
        passengerRepo.Add(passenger);
        return passenger;
    }


    [Fact]
    [Trait("Category", "Domain")]
    public void Money_Subtract_ShouldThrowNotEnoughMoneyException_WhenAmountIsInsufficient()
    {
        var m1 = new Money(100, TestCurrency);
        var m2 = new Money(200, TestCurrency);
        
        Assert.Throws<NotEnoughMoneyException>(() => m1.Subtract(m2));
    }

    [Fact]
    [Trait("Category", "Domain")]
    public void Money_Add_ShouldThrowCurrencyMismatchException_WhenCurrenciesDiffer()
    {
        var m1 = new Money(100, Currency.Eur);
        var m2 = new Money(100, Currency.Usd);
        
        Assert.Throws<CurrencyMismatchException>(() => m1.Add(m2));
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Money_OperatorAdd_ShouldThrowInvalidOperationException_WhenCurrenciesDiffer()
    {
        var m1 = new Money(100, Currency.Eur);
        var m2 = new Money(100, Currency.Usd);
        
        Assert.Throws<InvalidOperationException>(() => m1 + m2);
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Money_OperatorSubtract_ShouldThrowInvalidOperationException_WhenCurrenciesDiffer()
    {
        var m1 = new Money(100, Currency.Eur);
        var m2 = new Money(100, Currency.Usd);
        
        Assert.Throws<InvalidOperationException>(() => m1 - m2);
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Money_Constructor_ShouldThrowNegativeMoneyAmountException_WhenAmountIsNegative()
    {
        Assert.Throws<NegativeMoneyAmountException>(() => new Money(-100, TestCurrency));
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Money_EqualsAndOperators_ShouldWork()
    {
        var m1 = new Money(100, Currency.Eur);
        var m2 = new Money(100, Currency.Eur);
        var m3 = new Money(200, Currency.Eur);
        var m4 = new Money(100, Currency.Usd);

        Assert.True(m1.Equals(m2));
        Assert.True(m1 == m2);
        Assert.False(m1 != m2);
        Assert.False(m1.Equals(m3));
        Assert.False(m1.Equals(m4));
        Assert.False(m1.Equals(null));
        Assert.False(m1.Equals(new object()));
        Assert.Equal(m1.GetHashCode(), m2.GetHashCode());
    }

    [Fact]
    [Trait("Category", "Domain")]
    public void Country_Constructor_ShouldThrowInvalidCountryNameException_WhenNameIsNull()
    {
        Assert.Throws<InvalidCountryNameException>(() => new Country(" "));
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Country_EqualsAndOperators_ShouldWork()
    {
        var c1 = new Country("Test");
        var c2 = new Country("Test");
        var c3 = new Country("Test2");
        
        Assert.True(c1.Equals(c2));
        Assert.True(c1 == c2);
        Assert.False(c1 != c2);
        Assert.False(c1.Equals(c3));
        Assert.False(c1.Equals(null));
        Assert.False(c1.Equals(new object()));
        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
    }

    [Fact]
    [Trait("Category", "Domain")]
    public void Route_Constructor_ShouldThrowInvalidDistanceException_WhenDistanceIsZero()
    {
        Assert.Throws<InvalidDistanceException>(() => new Route(new Country("Test"), 0));
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Route_EqualsAndOperators_ShouldWork()
    {
        var r1 = new Route(new Country("Test"), 100);
        var r2 = new Route(new Country("Test"), 100);
        var r3 = new Route(new Country("Test2"), 100);
        
        Assert.True(r1.Equals(r2));
        Assert.True(r1 == r2);
        Assert.False(r1 != r2);
        Assert.False(r1.Equals(r3));
        Assert.False(r1.Equals(null));
        Assert.False(r1.Equals(new object()));
        Assert.Equal(r1.GetHashCode(), r2.GetHashCode());
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Address_Constructor_ShouldThrowArgumentException_WhenInvalid()
    {
        var country = new Country("Test");
        Assert.Throws<ArgumentException>("street", () => new Address("", "City", "123", country));
        Assert.Throws<ArgumentException>("city", () => new Address("Street", " ", "123", country));
        Assert.Throws<ArgumentException>("zipCode", () => new Address("Street", "City", null!, country));
        Assert.Throws<ArgumentNullException>("country", () => new Address("Street", "City", "123", null!));
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Address_Equals_ShouldWork()
    {
        var country = new Country("Test");
        var a1 = new Address("Street", "City", "123", country);
        var a2 = new Address("Street", "City", "123", country);
        var a3 = new Address("Street2", "City", "123", country);
        
        Assert.True(a1.Equals(a2));
        Assert.False(a1.Equals(a3));
        Assert.False(a1.Equals(null));
        Assert.False(a1.Equals(new object()));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void AirplaneSpecs_Equals_ShouldWork()
    {
        var s1 = new AirplaneSpecs("M", 2000, "E");
        var s2 = new AirplaneSpecs("M", 2000, "E");
        var s3 = new AirplaneSpecs("M2", 2000, "E");
        
        Assert.True(s1.Equals(s2));
        Assert.False(s1.Equals(s3));
        Assert.False(s1.Equals(null));
        Assert.False(s1.Equals(new object()));
        Assert.Equal(s1.GetHashCode(), s2.GetHashCode());
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void ContactDetails_Equals_ShouldWork()
    {
        var c1 = new ContactDetails("a@b.com", "123");
        var c2 = new ContactDetails("a@b.com", "123");
        var c3 = new ContactDetails("a@b.com", "456");
        
        Assert.True(c1.Equals(c2));
        Assert.False(c1.Equals(c3));
        Assert.False(c1.Equals(null));
        Assert.False(c1.Equals(new object()));
        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Passport_Constructor_ShouldThrowArgumentException_WhenNumberIsEmpty()
    {
        Assert.Throws<ArgumentException>("passportNumber", () => new Passport(" ", new Country("T"), DateTime.Now));
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Passport_IsValid_ShouldReturnCorrectStatus()
    {
        var p1 = new Passport("123", new Country("T"), DateTime.Now.AddDays(1));
        var p2 = new Passport("123", new Country("T"), DateTime.Now.AddDays(-1));
        
        Assert.True(p1.IsValid(DateTime.Now));
        Assert.False(p2.IsValid(DateTime.Now));
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Passport_Equals_ShouldWork()
    {
        var p1 = new Passport("123", new Country("T"), DateTime.Now.AddDays(1));
        var p2 = new Passport("123", new Country("T"), DateTime.Now.AddDays(2));
        var p3 = new Passport("456", new Country("T"), DateTime.Now.AddDays(1));
        
        Assert.True(p1.Equals(p2));
        Assert.False(p1.Equals(p3));
        Assert.False(p1.Equals(null));
        Assert.False(p1.Equals(new object()));
        Assert.Equal(p1.GetHashCode(), p2.GetHashCode());
    }

    [Fact]
    [Trait("Category", "Domain")]
    public void Passenger_Pay_ShouldReturnFalse_WhenCurrencyMismatches()
    {
        var (_, _, passengerRepo, _, _, _, _, _, _, _, _, _, _, _) = SetupFullSystem();
        var passenger = CreateTestPassenger(passengerRepo, new Money(100, Currency.Eur));
        var payment = new Money(50, Currency.Usd);
        
        var result = passenger.Pay(payment);
        
        Assert.False(result);
        Assert.Equal(100, passenger.Money.Amount);
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Passenger_Pay_ShouldReturnFalse_WhenAmountIsInsufficient()
    {
        var (_, _, passengerRepo, _, _, _, _, _, _, _, _, _, _, _) = SetupFullSystem();
        var passenger = CreateTestPassenger(passengerRepo, new Money(100, Currency.Eur));
        var payment = new Money(150, Currency.Eur);
        
        var result = passenger.Pay(payment);
        
        Assert.False(result);
        Assert.Equal(100, passenger.Money.Amount);
    }

    [Fact]
    [Trait("Category", "Domain")]
    public void Passenger_RemoveTicket_ShouldRemoveTicketFromList()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService, 1);
        var flight = CreateTestFlight(flightService, hrService, plane);
        var passenger = new Passenger("p1", 20, Gender.Male, _testPrice, null, null);
        var ticket = new Ticket(passenger, flight, TicketStatus.Paid, _testPrice);
        passenger.AddTicket(ticket);
        
        Assert.Single(passenger.Tickets);
        
        passenger.RemoveTicket(ticket);
        
        Assert.Empty(passenger.Tickets);
    }

    [Fact]
    [Trait("Category", "Domain")]
    public void Flight_AddPassenger_ShouldReturnFalse_WhenFlightIsAtCapacity()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService, 1);
        var flight = CreateTestFlight(flightService, hrService, plane);
        var p1 = new Passenger("p1", 20, Gender.Male, _testPrice, null, null);
        var p2 = new Passenger("p2", 20, Gender.Male, _testPrice, null, null);
        
        flight.AddPassenger(p1);
        var result = flight.AddPassenger(p2);
        
        Assert.False(result);
    }
    

    [Fact]
    [Trait("Category", "Domain")]
    public void AirportCompany_SubtractBalance_ShouldThrowNotEnoughMoneyException()
    {
        var company = new AirportCompany(new Money(100, TestCurrency));
        
        Assert.Throws<NotEnoughMoneyException>(() => company.SubtractBalance(new Money(200, TestCurrency)));
    }
    
    [Fact]
    [Trait("Category", "Domain")]
    public void Ticket_BookingReference_ShouldBeSet()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService, 1);
        var flight = CreateTestFlight(flightService, hrService, plane);
        var passenger = new Passenger("p1", 20, Gender.Male, _testPrice, null, null);
        
        var ticket = new Ticket(passenger, flight, TicketStatus.Paid, _testPrice);
        
        Assert.NotNull(ticket.BookingReference);
        Assert.Equal(6, ticket.BookingReference.Length);
    }


    [Fact]
    [Trait("Category", "Service")]
    public void FinancialService_ProcessPayment_ShouldSucceed_WhenPassengerHasEnoughMoney()
    {
        var (_, _, passengerRepo, companyRepo, _, _, _, _, _, _, financialService, _, _, _) = SetupFullSystem();
        var passenger = CreateTestPassenger(passengerRepo, new Money(100, Currency.Eur));
        var company = companyRepo.Get();
        var companyStartBalance = company.Balance.Amount;
        var payment = new Money(50, Currency.Eur);
        
        var result = financialService.ProcessPayment(passenger, payment);
        
        Assert.True(result);
        Assert.Equal(50, passenger.Money.Amount);
        Assert.Equal(companyStartBalance + 50, company.Balance.Amount);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FinancialService_ProcessPayment_ShouldFail_WhenPassengerHasInsufficientMoney()
    {
        var (_, _, passengerRepo, companyRepo, _, _, _, _, _, _, financialService, _, _, _) = SetupFullSystem();
        var passenger = CreateTestPassenger(passengerRepo, new Money(100, Currency.Eur));
        var company = companyRepo.Get();
        var companyStartBalance = company.Balance.Amount;
        var payment = new Money(150, Currency.Eur);
        
        var result = financialService.ProcessPayment(passenger, payment);
        
        Assert.False(result);
        Assert.Equal(100, passenger.Money.Amount);
        Assert.Equal(companyStartBalance, company.Balance.Amount);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FinancialService_ProcessRefund_ShouldSucceed_AndRestoreBalance()
    {
        var (_, _, passengerRepo, companyRepo, _, _, _, _, _, _, financialService, _, _, _) = SetupFullSystem();
        var passenger = CreateTestPassenger(passengerRepo, new Money(100, Currency.Eur));
        var company = companyRepo.Get();
        var refundAmount = new Money(50, Currency.Eur);
        company.AddBalance(refundAmount);
        var companyStartBalance = company.Balance.Amount;

        var result = financialService.ProcessRefund(passenger, refundAmount);
        
        Assert.True(result);
        Assert.Equal(150, passenger.Money.Amount);
        Assert.Equal(companyStartBalance - 50, company.Balance.Amount);
    }

    [Fact]
    [Trait("Category", "Service")]
    public void FinancialService_ProcessRefund_ShouldFail_WhenCompanyHasInsufficientFunds()
    {
        var (_, _, passengerRepo, companyRepo, _, _, _, _, _, _, financialService, _, _, _) = SetupFullSystem();
        var passenger = CreateTestPassenger(passengerRepo, new Money(100, Currency.Eur));
        var company = companyRepo.Get();
        var refundAmount = new Money(5_000_000, Currency.Eur);
        var companyStartBalance = company.Balance.Amount;
        var passengerStartBalance = passenger.Money.Amount;
    
        var result = financialService.ProcessRefund(passenger, refundAmount);
        
        Assert.False(result);
        Assert.Equal(passengerStartBalance, passenger.Money.Amount);
        Assert.Equal(companyStartBalance, company.Balance.Amount);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void BookingService_BuyTicket_ShouldSucceed_WhenAllConditionsMet()
    {
        var (_, _, passengerRepo, _, _, _, _, fleetService, flightService, _, _, hrService, bookingService, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService, 2);
        var flight = CreateTestFlight(flightService, hrService, plane);
        var passenger = CreateTestPassenger(passengerRepo);
        
        var result = bookingService.BuyTicket(passenger.Id, flight.FlightId);
        
        Assert.True(result);
        Assert.Equal(_passengerStartMoney.Amount - flight.TicketPrice.Amount, passenger.Money.Amount);
        Assert.Single(passenger.Tickets);
        Assert.Single(flight.Passengers);
    }

    [Fact]
    [Trait("Category", "Service")]
    public void BookingService_BuyTicket_ShouldFail_WhenFlightIsFull()
    {
        var (_, _, passengerRepo, _, _, _, _, fleetService, flightService, _, _, hrService, bookingService, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService, 1);
        var flight = CreateTestFlight(flightService, hrService, plane);
        var p1 = CreateTestPassenger(passengerRepo);
        var p2 = CreateTestPassenger(passengerRepo);
        bookingService.BuyTicket(p1.Id, flight.FlightId);
        
        var result = bookingService.BuyTicket(p2.Id, flight.FlightId);
        
        Assert.False(result);
        Assert.Equal(_passengerStartMoney.Amount, p2.Money.Amount);
        Assert.Empty(p2.Tickets);
        Assert.Single(flight.Passengers);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void BookingService_BuyTicket_ShouldFail_WhenPassengerNotFound()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, bookingService, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService, 1);
        var flight = CreateTestFlight(flightService, hrService, plane);
        
        var result = bookingService.BuyTicket(Guid.NewGuid(), flight.FlightId);
        
        Assert.False(result);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void BookingService_BuyTicket_ShouldFail_WhenFlightNotFound()
    {
        var (_, _, passengerRepo, _, _, _, _, _, _, _, _, _, bookingService, _) = SetupFullSystem();
        var passenger = CreateTestPassenger(passengerRepo);
        
        var result = bookingService.BuyTicket(passenger.Id, Guid.NewGuid());
        
        Assert.False(result);
    }

    [Fact]
    [Trait("Category", "Service")]
    public void BookingService_BuyTicket_ShouldFail_WhenPassengerHasInsufficientFunds()
    {
        var (_, _, passengerRepo, _, _, _, _, fleetService, flightService, _, _, hrService, bookingService, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService, 2);
        var flight = CreateTestFlight(flightService, hrService, plane);
        var passenger = CreateTestPassenger(passengerRepo, new Money(50, TestCurrency));
        
        var result = bookingService.BuyTicket(passenger.Id, flight.FlightId);
        
        Assert.False(result);
        Assert.Equal(50, passenger.Money.Amount);
        Assert.Empty(passenger.Tickets);
        Assert.Empty(flight.Passengers);
    }

    [Fact]
    [Trait("Category", "Service")]
    public void BookingService_BuyTicket_ShouldFailAndRefund_WhenAddPassengerFails()
    {
        var setupFullSystem = SetupFullSystem();
        var plane = CreateTestAirplane(setupFullSystem.Item8, 0);
        var flight = CreateTestFlight(setupFullSystem.Item9, setupFullSystem.Item12, plane);
        var passenger = CreateTestPassenger(setupFullSystem.Item3);
        var startMoney = passenger.Money.Amount;
        var companyStartMoney = setupFullSystem.Item4.Get().Balance.Amount;
        
        var result = setupFullSystem.Item13.BuyTicket(passenger.Id, flight.FlightId);
        
        Assert.False(result);
        Assert.Equal(startMoney, passenger.Money.Amount);
        Assert.Empty(passenger.Tickets);
        Assert.Equal(companyStartMoney, setupFullSystem.Item4.Get().Balance.Amount);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void BookingService_PrintAllFlights_ShouldNotThrow()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, bookingService, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        CreateTestFlight(flightService, hrService, plane);
        
        bookingService.PrintAllFlights();
        
        Assert.True(true);
    }

    [Fact]
    [Trait("Category", "Service")]
    public void FlightOperationsService_DepartFlight_ShouldSucceed_WhenFlightIsScheduledAndPlaneIsAvailable()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, operationsService) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var flight = CreateTestFlight(flightService, hrService, plane);
        
        var result = operationsService.DepartFlight(flight.FlightId);
        
        Assert.True(result);
        Assert.Equal(FlightStatus.InFlight, flight.Status);
        Assert.Equal(AirplaneStatus.InFlight, plane.Status);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightOperationsService_DepartFlight_ShouldFail_WhenFlightNotFound()
    {
        var (_, _, _, _, _, _, _, _, _, _, _, _, _, operationsService) = SetupFullSystem();
        var result = operationsService.DepartFlight(Guid.NewGuid());
        Assert.False(result);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightOperationsService_DepartFlight_ShouldFail_WhenFlightIsAlreadyInFlight()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, operationsService) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var flight = CreateTestFlight(flightService, hrService, plane);
        operationsService.DepartFlight(flight.FlightId);
        
        var result = operationsService.DepartFlight(flight.FlightId);
        
        Assert.False(result);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightOperationsService_DepartFlight_ShouldFail_WhenPlaneIsInMaintenance()
    {
        var setupFullSystem = SetupFullSystem();
        var plane = CreateTestAirplane(setupFullSystem.Item8);
        var flight = CreateTestFlight(setupFullSystem.Item9, setupFullSystem.Item12, plane);
        setupFullSystem.Item8.ScheduleMaintenance(plane);
        
        var result = setupFullSystem.Item14.DepartFlight(flight.FlightId);
        
        Assert.False(result);
        Assert.Equal(AirplaneStatus.InMaintenance, plane.Status);
    }

    [Fact]
    [Trait("Category", "Service")]
    public void FlightOperationsService_ArriveFlight_ShouldSucceed_WhenFlightIsInFlight()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, operationsService) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var flight = CreateTestFlight(flightService, hrService, plane);
        operationsService.DepartFlight(flight.FlightId);
        
        var result = operationsService.ArriveFlight(flight.FlightId);
        
        Assert.True(result);
        Assert.Equal(FlightStatus.Arrived, flight.Status);
        Assert.Equal(AirplaneStatus.Available, plane.Status);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightOperationsService_ArriveFlight_ShouldFail_WhenFlightIsScheduled()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, operationsService) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var flight = CreateTestFlight(flightService, hrService, plane);
        
        var result = operationsService.ArriveFlight(flight.FlightId);
        
        Assert.False(result);
        Assert.Equal(FlightStatus.Scheduled, flight.Status);
        Assert.Equal(AirplaneStatus.Available, plane.Status);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightOperationsService_ArriveFlight_ShouldFail_WhenFlightNotFound()
    {
        var (_, _, _, _, _, _, _, _, _, _, _, _, _, operationsService) = SetupFullSystem();
        var result = operationsService.ArriveFlight(Guid.NewGuid());
        Assert.False(result);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightOperationsService_ArriveFlight_ShouldSetTicketStatusToUsed()
    {
        var (_, _, passengerRepo, _, _, _, _, fleetService, flightService, _, _, hrService, bookingService, operationsService) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var flight = CreateTestFlight(flightService, hrService, plane);
        var passenger = CreateTestPassenger(passengerRepo);
        bookingService.BuyTicket(passenger.Id, flight.FlightId);
        operationsService.DepartFlight(flight.FlightId);
        
        operationsService.ArriveFlight(flight.FlightId);
        
        var ticket = passenger.Tickets.First();
        Assert.Equal(TicketStatus.Used, ticket.Status);
    }

    [Fact]
    [Trait("Category", "Service")]
    public void FleetManagementService_PurchaseAirplaneWithSpecs_ShouldAddAirplaneToRepository()
    {
        var (airplaneRepo, _, _, _, _, _, _, fleetService, _, _, _, _, _, _) = SetupFullSystem();
        var planeCountBefore = airplaneRepo.GetAll().Count();
        
        var plane = CreateTestAirplane(fleetService);
        
        var planeCountAfter = airplaneRepo.GetAll().Count();
        Assert.NotNull(plane);
        Assert.Equal(planeCountBefore + 1, planeCountAfter);
        Assert.NotNull(airplaneRepo.GetById(plane.Id));
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FleetManagementService_ScheduleMaintenance_ShouldSetStatusToInMaintenance()
    {
        var setupFullSystem = SetupFullSystem();
        var plane = CreateTestAirplane(setupFullSystem.Item8);
        
        setupFullSystem.Item8.ScheduleMaintenance(plane);
        
        Assert.Equal(AirplaneStatus.InMaintenance, plane.Status);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FleetManagementService_ScheduleMaintenance_ShouldThrowInvalidOperationException_WhenPlaneIsInFlight()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, operationsService) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var flight = CreateTestFlight(flightService, hrService, plane);
        operationsService.DepartFlight(flight.FlightId);
        
        Assert.Throws<InvalidOperationException>(() => fleetService.ScheduleMaintenance(plane));
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FleetManagementService_CompleteMaintenance_ShouldSetStatusToAvailable()
    {
        var (_, _, _, _, _, _, _, fleetService, _, _, _, _, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        fleetService.ScheduleMaintenance(plane);
        
        fleetService.CompleteMaintenance(plane);
        
        Assert.Equal(AirplaneStatus.Available, plane.Status);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FleetManagementService_CompleteMaintenance_ShouldDoNothing_WhenPlaneIsAvailable()
    {
        var (_, _, _, _, _, _, _, fleetService, _, _, _, _, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        
        fleetService.CompleteMaintenance(plane);
        
        Assert.Equal(AirplaneStatus.Available, plane.Status);
    }

    [Fact]
    [Trait("Category", "Service")]
    public void HumanResourcesService_HirePilot_ShouldAddPilotToRepository()
    {
        var (_, _, _, _, pilotRepo, _, _, _, _, _, _, hrService, _, _) = SetupFullSystem();
        
        var pilot = hrService.HirePilot("Test", 30, Gender.Male, _testPrice);
        
        Assert.NotNull(pilot);
        Assert.NotNull(pilotRepo.GetById(pilot.Id));
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void HumanResourcesService_HireMaintenanceTechnician_ShouldAddTechnicianToRepository()
    {
        var (_, _, _, _, _, _, techRepo, _, _, _, _, hrService, _, _) = SetupFullSystem();
        
        var tech = hrService.HireMaintenanceTechnician(
            "Test", 30, Gender.Male, _testPrice, MaintenanceCertification.Basic, 5);
        
        Assert.NotNull(tech);
        Assert.NotNull(techRepo.GetById(tech.Id));
    }

    [Fact]
    [Trait("Category", "Service")]
    public void HumanResourcesService_HireFlightAttendant_ShouldAddAttendantToRepository()
    {
        var (_, _, _, _, _, attendantRepo, _, _, _, _, _, hrService, _, _) = SetupFullSystem();
        
        var attendant = hrService.HireFlightAttendant(
            "Test", 30, Gender.Female, _testPrice, ["English"], DateTime.Now.AddYears(1));
        
        Assert.NotNull(attendant);
        Assert.NotNull(attendantRepo.GetById(attendant.Id));
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightManagementService_CreateFlight_ShouldAddFlightToRepository()
    {
        var (_, flightRepo, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        
        var flight = CreateTestFlight(flightService, hrService, plane);
        
        Assert.NotNull(flight);
        Assert.NotNull(flightRepo.GetById(flight.FlightId));
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightManagementService_CreateFlight_ShouldThrowArgumentException_WhenCrewIsEmpty()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, _, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var route = new Route(new Country("Test"), 100);
        
        Assert.Throws<ArgumentException>("crew", () => flightService.CreateFlight(plane, [], route, _testPrice));
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightManagementService_CreateFlight_ShouldThrowArgumentException_WhenCrewIsNull()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, _, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var route = new Route(new Country("Test"), 100);
        
        Assert.Throws<ArgumentException>("crew", () => flightService.CreateFlight(plane, null!, route, _testPrice));
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightManagementService_UpdateTicketPrice_ShouldChangePrice()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var flight = CreateTestFlight(flightService, hrService, plane);
        var newPrice = new Money(999, TestCurrency);
        
        flightService.UpdateTicketPrice(flight, newPrice);
        
        Assert.Equal(newPrice, flight.TicketPrice);
        Assert.Equal(999, flight.TicketPrice.Amount);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void FlightManagementService_UpdateTicketPrice_ShouldDoNothing_WhenPriceIsTheSame()
    {
        var (_, _, _, _, _, _, _, fleetService, flightService, _, _, hrService, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var flight = CreateTestFlight(flightService, hrService, plane);
        var samePrice = new Money(flight.TicketPrice.Amount, flight.TicketPrice.Currency);
        
        flightService.UpdateTicketPrice(flight, samePrice);
        
        Assert.Equal(samePrice, flight.TicketPrice);
    }
    
    [Fact]
    [Trait("Category", "Service")]
    public void AirplaneMaintenanceService_LogMaintenanceRecord_ShouldAddRecordToAirplaneHistory()
    {
        var (_, _, _, _, _, _, _, fleetService, _, maintenanceService, _, hrService, _, _) = SetupFullSystem();
        var plane = CreateTestAirplane(fleetService);
        var tech = hrService.HireMaintenanceTechnician(
            "Test", 30, Gender.Male, _testPrice, MaintenanceCertification.Basic, 5);
        var cost = new Money(1000, TestCurrency);
        
        var record = maintenanceService.LogMaintenanceRecord(plane, tech, "Test", cost);
        
        Assert.NotNull(record);
        Assert.Single(plane.MaintenanceHistory);
        Assert.Equal("Test", plane.MaintenanceHistory.First().Description);
        Assert.Equal(tech.StaffId, record.TechnicianId);
    }
}