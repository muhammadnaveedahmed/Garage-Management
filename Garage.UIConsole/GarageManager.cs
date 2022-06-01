using Garage.Common;
using Garage.UIConsole.Entities;
using Garage.UIConsole.UserInterface;

namespace Garage.UIConsole;

public class GarageManager
{

    private IUI consoleUI;
    private GarageHandler? garageHandler = default!;

    public GarageManager()
    {
        consoleUI = new ConsoleUI();

    }

    public void Run()
    {
        do
        {
            // Show Main Menue of Garage Management
            ShowMainMenu();
            GetUserInput();
        } while (true);
    }

    void ShowMainMenu()
    {
        consoleUI.Clear();
        consoleUI.AddMessage("Show Main Menue by using following Numbers \n(1, 2, 3, ..., 8, 0) som du väljer med hjälp av numeriska tangenter"
                + "\n1.The Installation of The New Garage"
                + "\n2.The Installation of the new garage with the number of Vehicles"
                + "\n3.The List of All Parked Vehicles"
                + "\n4.The List of Vehicle types and how many of each are in the Garage"
                + "\n5.Add The Vehicle Garage"
                + "\n6.Remove the Vehicles from Garage"
                + "\n7.Find the specific Vehicle"
                + "\n8.Filter the Vehicles"
                + "\n0.Close the Garage Management Application");
    }

    void GetUserInput()
    {
        var keyPressed = consoleUI.GetKey();//Creates the Key input to be used with the switch-case below.

        var actionMeny = new Dictionary<ConsoleKey, Action>()
                {
                    {ConsoleKey.NumPad1,CreateGarage },
                    {ConsoleKey.NumPad2,CreateGarageWithVehicles },
                    {ConsoleKey.NumPad3,ListOfVehicles },
                    {ConsoleKey.NumPad4,ListOfVehicleTypes},
                    {ConsoleKey.NumPad5,AddVehicle },
                    {ConsoleKey.NumPad6,RemoveVehicle},
                    {ConsoleKey.NumPad7,FindVehicle},
                    {ConsoleKey.NumPad8,FilterVehicles },
                    {ConsoleKey.NumPad0,ClosePrograme },
                };

        if (actionMeny.ContainsKey(keyPressed))
            actionMeny[keyPressed]?.Invoke();

    }

    void CreateGarage()
    {
        CreateGarage(vehicles: null);
    }

    void CreateGarage(IEnumerable<IVehicle>? vehicles = null)
    {
        consoleUI.Clear();
        consoleUI.AddMessage("Instansieringen av ett nytt garage");

        uint capacity = ConsoleUI<uint>.AskForAnInput("Kapacitet?");

        garageHandler = new(capacity, vehicles);

        var remaindeCapacity = capacity - (vehicles is null ? 0 : vehicles.Count());

        consoleUI.AddMessage($"Garaget har {remaindeCapacity} kapacitet kvar");

        consoleUI.AddMessage("Något att gå tillbaka till huvudmeny");
        consoleUI.GetKey();

    }

    void CreateGarageWithVehicles()
    {
        consoleUI.Clear();
        consoleUI.AddMessage("Instansieringen av ett nytt garage med ett antal fordon");

        var vehicles = this.CreateVehicles();
        CreateGarage(vehicles);
    }

    void ListOfVehicles()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("Lista samtliga parkerade fordon");

        var vehicles = garageHandler.GetVehicles();
        foreach (var vehicle in vehicles)
        {
            consoleUI.AddMessage(vehicle.Stats() + " \n\r");
        }
        consoleUI.AddMessage("Något att gå tillbaka till huvudmeny");
        consoleUI.GetKey();
    }

    void ListOfVehicleTypes()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("Lista fordonstyper och hur många av varje som står i garaget");

        var vehicleGroups = garageHandler.GetVehicles().GroupBy(x => x.GetType().Name).Select(x => new { Type = x.Key, Count = x.Count() });
        foreach (var vehicleGroup in vehicleGroups)
        {
            consoleUI.AddMessage($"Fordonstyp: {vehicleGroup.Type}, {vehicleGroup.Count} st\n\r");
        }

        consoleUI.AddMessage("Något att gå tillbaka till huvudmeny");
        consoleUI.GetKey();
    }

    void AddVehicle()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("Lägga till fordon");

        var vehicle = this.CreateVehicle();
        if (vehicle is not null)
        {
            if (garageHandler.AddVehicle(vehicle))
                consoleUI.AddMessage("Fordonet läggs till garaget");

            consoleUI.AddMessage("Något att gå tillbaka till huvudmeny");
            consoleUI.GetKey();
        }
    }

    IEnumerable<IVehicle> CreateVehicles()
    {
        var result = new List<IVehicle>();
        do
        {
            var vehicle = this.CreateVehicle();

            if (vehicle is null)
                break;

            result.Add(vehicle);
            //yield return vehicle;

        } while (true);

        return result;
    }

    IVehicle? CreateVehicle()
    {
        var vehicle = AskForVehicle();

        if (vehicle is null)
            return null;

        var result = GarageHandler.CreateVehicle(registerNumber: vehicle.Value.Item1,
                       color: vehicle.Value.Item2,
                       numberOfWheels: vehicle.Value.Item3,
                       vehicleType: vehicle.Value.Item4,
                       wingSpan: vehicle.Value.Item5,
                       hullType: vehicle.Value.Item6,
                       busType: vehicle.Value.Item7,
                       hasOneLessWheelSuspension: vehicle.Value.Item8,
                       topBoxCapacity: vehicle.Value.Item9);

        return result;

    }

    (string, string, uint, VehicleType, uint?, uint?, uint?, bool?, uint?)? AskForVehicle()
    {
        consoleUI.AddMessage("Vilket typ av fordon?"
                    + "\n1.Airplain"
                    + "\n2.Boat"
                    + "\n3.Bus"
                    + "\n4.Car"
                    + "\n5.MotorCycle"
                    + "\nTomt att klara");

        string? input = Console.ReadLine();
        ArgumentNullException.ThrowIfNull(input);

        if (string.IsNullOrEmpty(input))
            return null;

        // ToDo: out of range VehicleType
        var vehicleType = (VehicleType)(int.Parse(input) - 1);

        string registerNumber = ConsoleUI<string>.AskForAnInput("Registreringsnummer?");

        string color = ConsoleUI<string>.AskForAnInput("Färg?");

        uint numberOfWheels = ConsoleUI<uint>.AskForAnInput("Antal hjul?");


        uint? wingSpan = null;
        uint? hullType = null;
        uint? busType = null;
        bool? hasOneLessWheelSuspension = null;
        uint? topBoxCapacity = null;
        // ToDo: ArgumentException
        if (vehicleType == VehicleType.Airplain) wingSpan = ConsoleUI<uint>.AskForAnInput("Ving spann?");
        else if (vehicleType == VehicleType.Boat) hullType = ConsoleUI<uint>.AskForAnInput("Skrov typ?");
        else if (vehicleType == VehicleType.Bus) busType = ConsoleUI<uint>.AskForAnInput("Buss typ?");
        else if (vehicleType == VehicleType.Car) hasOneLessWheelSuspension = ConsoleUI<bool>.AskForAnInput("Har en hjulupphängning mindre? Ja=j, Nej=n", "Ja=j, Nej=n");
        else if (vehicleType == VehicleType.Motorcycle) topBoxCapacity = ConsoleUI<uint>.AskForAnInput("Toppboxens kapacitet?");
        else throw new NotImplementedException();

        // ToDo: fix converting
        (string registerNumber, string color, uint, VehicleType vehicleType, uint? wingSpan, uint? hullType, uint? busType, bool? hasOneLessWheelSuspension, uint? topBoxCapacity) result =
            (registerNumber, color, numberOfWheels, vehicleType, wingSpan, hullType, busType, hasOneLessWheelSuspension, topBoxCapacity);

        return result;
    }

    void RemoveVehicle()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("Ta bort fordon ur garaget");

        string registerNumber = ConsoleUI<string>.AskForAnInput("Registreringsnummer?");

        // ToDo: Could not find the vehicle
        var vehicle = garageHandler.GetVehicleByRegisterNumber(registerNumber);
        ArgumentNullException.ThrowIfNull(vehicle);

        garageHandler.RemoveVehicle(vehicle);

        consoleUI.AddMessage("Fordonet tas bort ur garaget");

        consoleUI.AddMessage("Något att gå tillbaka till huvudmeny");
        consoleUI.GetKey();
    }

    void FindVehicle()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("Hitta ett specifikt fordon");

        string registerNumber = ConsoleUI<string>.AskForAnInput("Registreringsnummer?");

        // ToDo: Could not find the vehicle
        var vehicle = garageHandler.GetVehicleByRegisterNumber(registerNumber);
        ArgumentNullException.ThrowIfNull(vehicle);

        consoleUI.AddMessage(vehicle.Stats());
        consoleUI.AddMessage("Något att gå tillbaka till huvudmeny");
        consoleUI.GetKey();
    }

    void FilterVehicles()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("Filtrera fordon");

        string keyword = ConsoleUI<string>.AskForAnInput("Sökord?");

        // ToDo: Could not find the vehicle
        var vehicles = garageHandler.FilterVehiclesByKeyword(keyword);


        vehicles.ToList<IVehicle>().ForEach(x => consoleUI.AddMessage(x.Stats() + "\n\r"));

        consoleUI.AddMessage("Något att gå tillbaka till huvudmeny");
        consoleUI.GetKey();
    }

    void ClosePrograme()
    {
        consoleUI.Clear();
        consoleUI.AddMessage("Stängs");
        Environment.Exit(0);
    }


}
