namespace Garage.UIConsole.Entities;

public class Car : Vehicle
{
    public bool HasOneLessWheelSuspension { get; init; }

    public Car(string registerNumber,
                    string color,
                    uint numberOfWheels,
                    bool hasOneLessWheelSuspension) : base(registerNumber, color, numberOfWheels) => HasOneLessWheelSuspension = hasOneLessWheelSuspension;

    public override string Stats() => $"{base.Stats()}Has a smaller wheel suspension?{(HasOneLessWheelSuspension ? "Ja" : "Nej")}";
    public override bool Matches(ref string keyword) => base.Matches(ref keyword) || HasOneLessWheelSuspension.ToString() == keyword;

}