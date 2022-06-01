namespace Garage.UIConsole.Entities
{
    public interface IVehicle
    {
        string Color { get; init; }
        uint NumberOfWheels { get; init; }
        string RegisterNumber { get; init; }

        string Stats();
        bool Matches(ref string keyword);
    }
}