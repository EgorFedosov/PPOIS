**Интерфейсы (Interfaces)**

- `IAnimal` 4 2 &rarr; Product, Place
- `IAttachableMachine` 4 1 &rarr; Tractor
- `IMachine` 4 2 &rarr; IWorker, Place
- `IWorker` 8 2 &rarr; EmployeeLevel, Place

---

**Абстрактные классы (Abstract Classes)**

- `Animal` 7 8 &rarr; IAnimal, AnimalConfig, Place, Product, AnimalAlreadyDeadException, InvalidMoveException, AnimalMissingProductException, AnimalInvalidPlaceStateException
- `Employee` 2 4 &rarr; EmployeeConfig, IWorker, Place, InvalidMoveException
- `EmployeeWithWarehouse` 4 1 &rarr; Warehouse
- `Field` 5 4 &rarr; FieldConfig, Place, Warehouse, FieldSeedLimitExceededException
- `Machine` 3 3 &rarr; IMachine, Place, IWorker
- `AttachableMachine` 1 2 &rarr; Machine, IAttachableMachine
- `Place` 4 3 &rarr; IAnimal, IMachine, IWorker
- `Product` 12 1 &rarr; ProductConfig

---

**Сущности: Животные (Entities: Animals)**

- `Chicken` 1 1 &rarr; Animal
- `Cow` 1 1 &rarr; Animal
- `Duck` 1 1 &rarr; Animal
- `Goat` 2 1 &rarr; Animal
- `Pig` 2 2 &rarr; Animal, AnimalInvalidPlaceStateException
- `Rabbit` 2 1 &rarr; Animal
- `Sheep` 1 1 &rarr; Animal

---

**Сущности: Работники (Entities: Employees)**

- `Accountant` 2 5 &rarr; Employee, IWorker, EmployeeConfig, SalaryConfig, EmployeeLevel
- `EquipmentOperator` 4 11 &rarr; EmployeeWithWarehouse, EmployeeConfig, IMachine, MachineAlreadyOccupiedException, InvalidEmployeeLocationException, MachineLocationMismatchException, DriverNotAssignedException, EquipmentOrFieldNotAssignedException, MachineNotOnException, InvalidWorkLocationException, InsufficientSeedsException
- `Farmer` 2 11 &rarr; EmployeeWithWarehouse, EmployeeConfig, Warehouse, FarmerLocationNotAssignedException, NoAnimalsOnLocationException, IAnimal, Product, AnimalHasNoProductException, Pig, Rabbit, EmployeeLevel
- `FieldWorker` 2 7 &rarr; EmployeeWithWarehouse, EmployeeConfig, Field, FieldWorkerNotOnFieldException, EquipmentOperator, Tractor, UnknownAttachmentException
- `SalesManager` 2 3 &rarr; EmployeeWithWarehouse, EmployeeConfig, Product

---

**Сущности: Техника (Entities: Machines)**

- `CropSprayer` 1 2 &rarr; Machine, IAttachableMachine
- `Plow` 2 2 &rarr; Machine, IAttachableMachine
- `Seeder` 1 2 &rarr; Machine, IAttachableMachine
- `Harvester` 2 3 &rarr; Machine, IWorker, InvalidHarvesterDriverException, HarvesterCannotBeTurnedOnException
- `Tractor` 3 4 &rarr; Machine, IAttachableMachine, AttachmentAlreadyConnectedException, AttachmentNotConnectedException

---

**Сущности: Места (Entities: Places)**

- `Barn` 2 1 &rarr; Place
- `Warehouse` 11 3 &rarr; Place, CropSeed, Product
- `CabbageField` 1 1 &rarr; Field
- `CornField` 1 1 &rarr; Field
- `FruitField` 1 1 &rarr; Field
- `PotatoField` 2 1 &rarr; Field
- `WheatField` 1 1 &rarr; Field

---

**Сущности: Продукты (Entities: Products)**

- `Cabbage` 1 1 &rarr; Product
- `Corn` 1 1 &rarr; Product
- `CropSeed` 2 1 &rarr; Product
- `Egg` 1 1 &rarr; Product
- `Fruit` 1 1 &rarr; Product
- `Meat` 1 1 &rarr; Product
- `Milk` 1 1 &rarr; Product
- `Potato` 1 1 &rarr; Product
- `Wheat` 1 1 &rarr; Product
- `Wool` 1 1 &rarr; Product

---

**Конфигурации (Configurations)**

- `AnimalConfig` 2 1 &rarr; Product
- `EmployeeConfig` 5 2 &rarr; EmployeeLevel, Place
- `FieldConfig` 2 1 &rarr; Product
- `ProductConfig` 2 0
- `SalaryConfig` 1 3 &rarr; EmployeeSalaryData, IWorker, EmployeeLevel
- `EmployeeSalaryData` 1 1 &rarr; EmployeeLevel

---

**Перечисления (Enums)**

- `EmployeeLevel` 5 0

---

**Исключения (Exceptions)**

- `AnimalAlreadyDeadException` 1 0
- `AnimalHasNoProductException` 1 0
- `AnimalInvalidPlaceStateException` 2 0
- `AnimalMissingProductException` 1 0
- `AttachmentAlreadyConnectedException` 1 0
- `AttachmentNotConnectedException` 1 0
- `DriverNotAssignedException` 1 0
- `EquipmentOrFieldNotAssignedException` 1 0
- `FarmerLocationNotAssignedException` 1 0
- `FieldFullException` 0 0
- `FieldSeedLimitExceededException` 1 0
- `FieldWorkerNotOnFieldException` 1 0
- `HarvesterCannotBeTurnedOnException` 1 0
- `InsufficientSeedsException` 1 0
- `InvalidEmployeeLocationException` 1 0
- `InvalidHarvesterDriverException` 1 0
- `InvalidMoveException` 2 0
- `InvalidWorkLocationException` 1 0
- `MachineAlreadyOccupiedException` 1 0
- `MachineLocationMismatchException` 1 0
- `MachineNotOnException` 2 0
- `NoAnimalsOnLocationException` 1 0
- `NoDriverAssignedException` 1 0
- `OperatorHasNoMachineException` 0 0
- `UnknownAttachmentException` 1 0
- `WarehouseNotAssignedException` 0 0

---

- **Классы:** 77
- **Поля:** 142
- **Поведения:** 209
- **Ассоциации:** 171
