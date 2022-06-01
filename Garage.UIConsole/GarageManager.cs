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
        consoleUI.AddMessage("Show Main Menue by using following Numbers \n(1, 2, 3, ..., 8, 0) which you select using numeric keys"
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
                    {ConsoleKey.D1,CreateGarage },
                    {ConsoleKey.D2,CreateGarageWithVehicles },
                    {ConsoleKey.D3,ListOfVehicles },
                    {ConsoleKey.D4,ListOfVehicleTypes},
                    {ConsoleKey.D5,AddVehicle },
                    {ConsoleKey.D6,RemoveVehicle},
                    {ConsoleKey.D7,FindVehicle},
                    {ConsoleKey.D8,FilterVehicles },
                    {ConsoleKey.D0,ClosePrograme },
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
        consoleUI.AddMessage("The Installation of The New Garage");

        uint capacity = ConsoleUI<uint>.AskForAnInput("Capacity?");

        garageHandler = new(capacity, vehicles);

        var remaindeCapacity = capacity - (vehicles is null ? 0 : vehicles.Count());

        consoleUI.AddMessage($"Garage has {remaindeCapacity} capacity left");

        consoleUI.AddMessage("Someone Going back to the Main Menue");
        consoleUI.GetKey();

    }

    void CreateGarageWithVehicles()
    {
        consoleUI.Clear();
        consoleUI.AddMessage("The Installation of the new garage with the number of Vehicles");

        var vehicles = this.CreateVehicles();
        CreateGarage(vehicles);
    }

    void ListOfVehicles()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("The List of All Parked Vehicles");

        var vehicles = garageHandler.GetVehicles();
        foreach (var vehicle in vehicles)
        {
            consoleUI.AddMessage(vehicle.Stats() + " \n\r");
        }
        consoleUI.AddMessage("Someone Going back to the Main Menue");
        consoleUI.GetKey();
    }

    void ListOfVehicleTypes()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("The List of Vehicle types and how many of each are in the Garage");

        var vehicleGroups = garageHandler.GetVehicles().GroupBy(x => x.GetType().Name).Select(x => new { Type = x.Key, Count = x.Count() });
        foreach (var vehicleGroup in vehicleGroups)
        {
            consoleUI.AddMessage($"Vehicle Type: {vehicleGroup.Type}, {vehicleGroup.Count} st\n\r");
        }

        consoleUI.AddMessage("Someone Going back to the Main Menue");
        consoleUI.GetKey();
    }

    void AddVehicle()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("Add The Vehicle");

        var vehicle = this.CreateVehicle();
        if (vehicle is not null)
        {
            if (garageHandler.AddVehicle(vehicle))
                consoleUI.AddMessage("The Vehicle is added to the Garage");

            consoleUI.AddMessage("Someone Going back to the Main Menue");
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
        consoleUI.AddMessage("Which Type of Vehicle?"
                    + "\n1.Airplane"
                    + "\n2.Boat"
                    + "\n3.Bus"
                    + "\n4.Car"
                    + "\n5.MotorCycle"
                    + "\nEmpty to manage");

        string? input = Console.ReadLine();
        ArgumentNullException.ThrowIfNull(input);

        if (string.IsNullOrEmpty(input))
            return null;

        // ToDo: out of range VehicleType
        var vehicleType = (VehicleType)(int.Parse(input) - 1);

        string registerNumber = ConsoleUI<string>.AskForAnInput("Registrationsnumber?");

        string color = ConsoleUI<string>.AskForAnInput("Color?");

        uint numberOfWheels = ConsoleUI<uint>.AskForAnInput("Number of Wheels?");


        uint? wingSpan = null;
        uint? hullType = null;
        uint? busType = null;
        bool? hasOneLessWheelSuspension = null;
        uint? topBoxCapacity = null;
        // ToDo: ArgumentException
        if (vehicleType == VehicleType.Airplane) wingSpan = ConsoleUI<uint>.AskForAnInput("Wing Span?");
        else if (vehicleType == VehicleType.Boat) hullType = ConsoleUI<uint>.AskForAnInput("Hull Type?");
        else if (vehicleType == VehicleType.Bus) busType = ConsoleUI<uint>.AskForAnInput("Buss Type?");
        else if (vehicleType == VehicleType.Car) hasOneLessWheelSuspension = ConsoleUI<bool>.AskForAnInput("Has a smaller wheel suspension? Ja=j, Nej=n", "Ja=j, Nej=n");
        else if (vehicleType == VehicleType.Motorcycle) topBoxCapacity = ConsoleUI<uint>.AskForAnInput("The capacity of the Top Box?");
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
        consoleUI.AddMessage("Remove Vehicle from the Garage");

        string registerNumber = ConsoleUI<string>.AskForAnInput("RegistrationsNumber?");

        // ToDo: Could not find the vehicle
        var vehicle = garageHandler.GetVehicleByRegisterNumber(registerNumber);
        ArgumentNullException.ThrowIfNull(vehicle);

        garageHandler.RemoveVehicle(vehicle);

        consoleUI.AddMessage("The Vehicle is removed from the Garage");

        consoleUI.AddMessage("Someone going back to Main Menue");
        consoleUI.GetKey();
    }

    void FindVehicle()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("Search a specific Vehicle");

        string registerNumber = ConsoleUI<string>.AskForAnInput("Registrationsnumber?");

        // ToDo: Could not find the vehicle
        var vehicle = garageHandler.GetVehicleByRegisterNumber(registerNumber);
        ArgumentNullException.ThrowIfNull(vehicle);

        consoleUI.AddMessage(vehicle.Stats());
        consoleUI.AddMessage("Someone going back to Main Menue");
        consoleUI.GetKey();
    }

    void FilterVehicles()
    {
        ArgumentNullException.ThrowIfNull(garageHandler);

        consoleUI.Clear();
        consoleUI.AddMessage("Filter Vehicle");

        string keyword = ConsoleUI<string>.AskForAnInput("Keywords?");

        // ToDo: Could not find the vehicle
        var vehicles = garageHandler.FilterVehiclesByKeyword(keyword);


        vehicles.ToList<IVehicle>().ForEach(x => consoleUI.AddMessage(x.Stats() + "\n\r"));

        consoleUI.AddMessage("Someone going back to Main Menue");
        consoleUI.GetKey();
    }

    void ClosePrograme()
    {
        consoleUI.Clear();
        consoleUI.AddMessage("Closed");
        Environment.Exit(0);
    }


}
