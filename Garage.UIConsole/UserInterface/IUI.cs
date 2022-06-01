namespace Garage.UIConsole.UserInterface;

public interface IUI
{
    void Clear();
    ConsoleKey GetKey();
    void AddMessage(string message);
}
