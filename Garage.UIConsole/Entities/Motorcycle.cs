namespace Garage.UIConsole.Entities;

public class Motorcycle : Vehicle
{
    public uint TopBoxCapacity { get; init; }

    public Motorcycle(string registerNumber,
                    string color,
                    uint numberOfWheels,
                    uint topBoxCapacity) : base(registerNumber, color, topBoxCapacity) => TopBoxCapacity = topBoxCapacity;
    public override string Stats() => $"{base.Stats()}Top Box Capacity:{TopBoxCapacity}";
    public override bool Matches(ref string keyword) => base.Matches(ref keyword) || TopBoxCapacity.ToString() == keyword;

}