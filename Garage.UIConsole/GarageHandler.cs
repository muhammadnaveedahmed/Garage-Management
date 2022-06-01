using Garage.Common;
using Garage.UIConsole.Entities;

namespace Garage.UIConsole;

public class GarageHandler
{
    private IGarage<IVehicle> garage;
    public GarageHandler(uint capacity, IEnumerable<IVehicle>? vehicles = null)
    {
        if (vehicles is not null && vehicles.Count() > capacity)
            throw new ArgumentException();

        garage = new Garage<IVehicle>(capacity);

        if (vehicles is not null)
            foreach (var vehicle in vehicles)
            {
                AddVehicle(vehicle);
            }

    }

    private static IVehicle CreateAirplane(dynamic vehicle,
                                   uint wingSpan) => new Airplane(vehicle.RegisterNumber, vehicle.Color, vehicle.NumberOfWheels, wingSpan);

    private static IVehicle CreateBoat(dynamic vehicle,
                               uint hullType) => new Boat(vehicle.RegisterNumber, vehicle.Color, vehicle.NumberOfWheels, hullType);

    private static IVehicle CreateBus(dynamic vehicle,
                              uint busType) => new Bus(vehicle.RegisterNumber, vehicle.Color, vehicle.NumberOfWheels, busType);

    private static IVehicle CreateCar(dynamic vehicle,
                              bool hasOneLessWheelSuspension) => new Car(vehicle.RegisterNumber, vehicle.Color, vehicle.NumberOfWheels, hasOneLessWheelSuspension);

    private static IVehicle CreateMotorcycle(dynamic vehicle,
                                     uint topBoxCapacity) => new Motorcycle(vehicle.RegisterNumber, vehicle.Color, vehicle.NumberOfWheels, topBoxCapacity);

    private static IVehicle CreateVehicle(object vehicle,
                               VehicleType vehicleType,
                               uint? wingSpan = null,
                               uint? hullType = null,
                               uint? busType = null,
                               bool? hasOneLessWheelSuspension = null,
                               uint? topBoxCapacity = null)
    {
        var result = vehicleType switch
        {
            VehicleType.Airplain => CreateAirplane(vehicle, wingSpan!.Value),
            VehicleType.Boat => CreateBoat(vehicle, hullType!.Value),
            VehicleType.Bus => CreateBus(vehicle, busType!.Value),
            VehicleType.Car => CreateCar(vehicle, hasOneLessWheelSuspension!.Value),
            VehicleType.Motorcycle => CreateMotorcycle(vehicle, topBoxCapacity!.Value),
            _ => throw new NotImplementedException(),
        };

        return result;
    }
    public static IVehicle CreateVehicle(string registerNumber,
                               string color,
                               uint numberOfWheels,
                               VehicleType vehicleType,
                               uint? wingSpan = null,
                               uint? hullType = null,
                               uint? busType = null,
                               bool? hasOneLessWheelSuspension = null,
                               uint? topBoxCapacity = null)
    {
        dynamic vehicle = new System.Dynamic.ExpandoObject();

        vehicle.RegisterNumber = registerNumber;
        vehicle.Color = color;
        vehicle.NumberOfWheels = numberOfWheels;



        return CreateVehicle(vehicle, vehicleType, wingSpan, hullType, busType, hasOneLessWheelSuspension, topBoxCapacity);
    }

    public bool AddVehicle(IVehicle vehicle) => garage.Add(vehicle);

    public bool RemoveVehicle(IVehicle vehicle) => garage.Remove(vehicle);

    public IEnumerable<IVehicle> GetVehicles() => garage.Where(x => x is not null);

    public IVehicle? GetVehicleByRegisterNumber(string registerNumber) => garage.FirstOrDefault(x => x is not null && x.RegisterNumber == registerNumber!);
    public IEnumerable<IVehicle> FilterVehiclesByKeyword(string keyword) => garage.Where(x => x is not null && x.Matches(ref keyword));







}
