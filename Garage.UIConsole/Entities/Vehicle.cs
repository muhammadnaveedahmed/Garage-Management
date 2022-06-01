namespace Garage.UIConsole.Entities;

public abstract class Vehicle : IVehicle
{
    public string RegisterNumber { get; init; }
    public string Color { get; init; }
    public uint NumberOfWheels { get; init; }

    public Vehicle(string registerNumber, string color, uint numberOfWheels)
    {
        RegisterNumber = registerNumber;
        Color = color;
        NumberOfWheels = numberOfWheels;
    }

    public virtual string Stats() => $"Vehicle:{GetType().Name}\n\rRegistreringsnummer:{RegisterNumber}\n\rFärg:{Color}\n\rAntal hjul:{NumberOfWheels}\n\r";
        public virtual bool Matches(ref string keyword)
    {
        keyword = keyword.Trim().ToLower();
        return RegisterNumber == keyword || Color == keyword || NumberOfWheels.ToString() == keyword;
    }
}