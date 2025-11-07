using AirportSystem.Application.Interfaces;
using AirportSystem.Application.Services;
using AirportSystem.Domain.Aggregates;
using AirportSystem.Domain.Enums;
using AirportSystem.Domain.Interfaces;
using AirportSystem.Domain.Repositories;
using AirportSystem.Domain.ValueObjects;
using AirportSystem.Infrastructure.Repositories;

namespace AirportSystem;

internal abstract class Program
{
    static void Main()
    {
        Console.WriteLine("--- Симуляция работы Аэропорта ---");

        IAirplaneRepository airplaneRepo = new InMemoryAirplaneRepository();
        IFlightRepository flightRepo = new InMemoryFlightRepository();
        IPassengerRepository passengerRepo = new InMemoryPassengerRepository();
        IAirportCompanyRepository companyRepo = new InMemoryAirportCompanyRepository();
        IPilotRepository pilotRepo = new InMemoryPilotRepository();
        IFlightAttendantRepository attendantRepo = new InMemoryFlightAttendantRepository();
        IMaintenanceTechnicianRepository techRepo = new InMemoryMaintenanceTechnicianRepository();

        IFleetManagementService fleetService = new FleetManagementService(airplaneRepo);
        IFlightManagementService flightService = new FlightManagementService(flightRepo);
        IAirplaneMaintenanceService maintenanceService = new AirplaneMaintenanceService();
        IFinancialService financialService = new FinancialService(companyRepo);
        IHumanResourcesService hrService = new HumanResourcesService(attendantRepo, techRepo, pilotRepo);
        IBookingService bookingService = new BookingService(flightRepo, passengerRepo, financialService);
        IFlightOperationsService operationsService = new FlightOperationsService(flightRepo);


        Console.WriteLine("\n--- 1. Отдел кадров ---");

        var pilotMoney = new Money(15000, Currency.Eur);
        IPilot pilot = hrService.HirePilot("Серега Пилот", 48, Gender.Male, pilotMoney);

        var techMoney = new Money(5000, Currency.Eur);
        IMaintenanceTechnician technician = hrService.HireMaintenanceTechnician(
            "Виктор Механик", 35, Gender.Male, techMoney, MaintenanceCertification.Avionics, 10);

        Console.WriteLine($"Нанят пилот: {pilot.Name}");
        Console.WriteLine($"Нанят техник: {technician.Name}");


        Console.WriteLine("\n--- 2. Управление флотом ---");

        var planeSpecs = new AirplaneSpecs("Airbus", 2023, "Twin-Aisle");
        var planePrice = new Money(150_000_000, Currency.Eur);

        IAirplane airplane = fleetService.PurchaseAirplaneWithSpecs(
            "A330", 2, 5000, planePrice, planeSpecs);

        Console.WriteLine($"Куплен самолет: {airplane.Model}");


        Console.WriteLine("\n--- 3. Управление рейсами ---");

        var crew = new List<IPilot> { pilot };
        var destinationCountry = new Country("Япония");
        var route = new Route(destinationCountry, 9000);
        var ticketPrice = new Money(800, Currency.Eur);

        IFlight flight = flightService.CreateFlight(airplane, crew, route, ticketPrice);

        Console.WriteLine($"Создан рейс [ID: {flight.FlightId}] в {destinationCountry.Name}");
        Console.WriteLine($"Статус рейса: {flight.Status}");


        Console.WriteLine("\n--- 4. Бронирование и Финансы ---");

        var passengerMoney = new Money(1000, Currency.Eur);
        var passenger = new Passenger("Анна Пассажир", 29, Gender.Female, passengerMoney, null, null);
        passengerRepo.Add(passenger);

        var company = companyRepo.Get();

        Console.WriteLine($"Баланс компании (до): {company.Balance.Amount} {company.Balance.Currency}");
        Console.WriteLine($"Баланс пассажира (до): {passenger.Money.Amount} {passenger.Money.Currency}");

        bool purchaseSuccess = bookingService.BuyTicket(passenger.Id, flight.FlightId);

        Console.WriteLine($"Покупка билета успешна: {purchaseSuccess}");

        Console.WriteLine($"Баланс компании (после): {company.Balance.Amount} {company.Balance.Currency}");
        Console.WriteLine($"Баланс пассажира (после): {passenger.Money.Amount} {passenger.Money.Currency}");

        var updatedPassenger = passengerRepo.GetById(passenger.Id);
        var ticket = updatedPassenger?.Tickets.FirstOrDefault();

        Console.WriteLine($"У пассажира {updatedPassenger?.Tickets.Count} билет. Статус билета: {ticket?.Status}");


        Console.WriteLine("\n--- 5. Обслуживание ---");

        var maintenanceCost = new Money(5500, Currency.Eur);

        MaintenanceRecord record = maintenanceService.LogMaintenanceRecord(
            airplane, technician, "Плановая проверка", maintenanceCost);

        Console.WriteLine($"Добавлена запись об обслуживании: {record.Description}");
        Console.WriteLine($"Количество записей в истории самолета: {airplane.MaintenanceHistory.Count}");


        Console.WriteLine("\n--- 6. Симуляция Полета ---");

        Console.WriteLine($"Вылет рейса: {flight.FlightId}...");
        var departed = operationsService.DepartFlight(flight.FlightId);

        Console.WriteLine($"Вылет успешен: {departed}");
        Console.WriteLine($"Самолет {airplane.Model} в полете. Статус: {airplane.Status}");
        Console.WriteLine($"Рейс {flight.FlightId} в полете. Статус: {flight.Status}");

        Console.WriteLine("...самолет летит...");

        Console.WriteLine($"Прибытие рейса: {flight.FlightId}...");
        bool arrived = operationsService.ArriveFlight(flight.FlightId);

        Console.WriteLine($"Прибытие успешно: {arrived}");
        Console.WriteLine($"Билет пассажира использован. Статус: {ticket?.Status}");
        Console.WriteLine($"Самолет {airplane.Model} приземлился. Статус: {airplane.Status}");
        Console.WriteLine($"Рейс {flight.FlightId} завершен. Статус: {flight.Status}");
    }
}