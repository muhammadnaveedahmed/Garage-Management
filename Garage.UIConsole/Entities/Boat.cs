namespace Garage.UIConsole.Entities;

public class Boat : Vehicle
{
    public uint HullType { get; init; }

    public Boat(string registerNumber,
                    string color,
                    uint numberOfWheels,
                    uint hullType) : base(registerNumber, color, hullType) => HullType = hullType;

    public override string Stats() => $"{base.Stats()}Hull Type:{HullType}";
    public override bool Matches(ref string keyword) => base.Matches(ref keyword) || HullType.ToString() == keyword;
}