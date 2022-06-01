namespace Garage.UIConsole.Entities;

public class Airplane : Vehicle
{
    public uint WingSpan { get; init; }

    public Airplane(string registerNumber,
                    string color,
                    uint numberOfWheels,
                    uint wingSpan) : base(registerNumber, color, numberOfWheels) => WingSpan = wingSpan;

    public override string Stats() => $"{base.Stats()}Wing Span:{WingSpan}";
        public override bool Matches(ref string keyword) => base.Matches(ref keyword) || WingSpan.ToString() == keyword;
}