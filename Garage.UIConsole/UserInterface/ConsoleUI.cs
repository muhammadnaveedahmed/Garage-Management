namespace Garage.UIConsole.UserInterface;

public class ConsoleUI : IUI
{
    public void Clear()
    {
        //Console.CursorVisible = false;
        Console.Clear();
    }
    public ConsoleKey GetKey() => Console.ReadKey(intercept: true).Key;
    public void AddMessage(string message) => Console.WriteLine(message);


}
public class ConsoleUI<T>
{
    //infoPrompt används för den första prompten. errorPrompt används för ogiltig inmatning.
    public static T AskForAnInput(string infoPrompt, string? errorPrompt = null)
    {
        if (errorPrompt is null) errorPrompt = infoPrompt;
        T result;

        //Det är specialt för string type annars fick vi runtime fel.
        if (typeof(T) == typeof(string)) result = (T)Activator.CreateInstance(typeof(T), string.Empty.ToCharArray())!;
        else result = Activator.CreateInstance<T>();

        bool success = false;
        string? answer;

        Console.WriteLine(infoPrompt);
        do
        {
            answer = Console.ReadLine();

            try
            {
                ArgumentNullException.ThrowIfNull(answer);
                answer = answer.Trim();
                if (string.IsNullOrWhiteSpace(answer.Trim())) throw new Exception();
                if (typeof(T) == typeof(bool))
                    if (answer.ToLower()[0] == 'j') answer = "true";
                    else if (answer.ToLower()[0] == 'n') answer = "false";

                //Om appen inte kan ändra den önskade typen ger den ett exception. Så vi måste hantera det med try-catch.
                result = (T)Convert.ChangeType(answer, typeof(T))!;
                //Om användaren inte anger en ingång gör vi ett exception för att göra samma sak eftersom det är en ogiltig inmatning.


                success = true;
            }
            catch
            {
                Console.WriteLine($"Du bör skriva upp {errorPrompt}");
            }

        } while (!success);

        return result;

    }
}