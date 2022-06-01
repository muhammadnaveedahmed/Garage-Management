namespace Garage.UIConsole.Entities;

public class Bus : Vehicle
{
    public uint BusType { get; init; }

    public Bus(string registerNumber,
                    string color,
                    uint numberOfWheels,
                    uint busType) : base(registerNumber, color, busType) => BusType = busType;
    public override string Stats() => $"{base.Stats()}Bus Type:{BusType}";
    public override bool Matches(ref string keyword) => base.Matches(ref keyword) || BusType.ToString() == keyword;

}