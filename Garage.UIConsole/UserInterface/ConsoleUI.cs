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
    //infoPrompt is used for the first prompt. errorPrompt is used for invalid input.
    public static T AskForAnInput(string infoPrompt, string? errorPrompt = null)
    {
        if (errorPrompt is null) errorPrompt = infoPrompt;
        T result;

        //It is special for string type otherwise we got runtime error.
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

                //If the app can not change the desired type, it gives an exception. So we have to deal with it with try-catch.
                result = (T)Convert.ChangeType(answer, typeof(T))!;
                //If the user does not enter an entry, we make an exception to do the same thing because it is an invalid entry.


                success = true;
            }
            catch
            {
                Console.WriteLine($"We Should write up {errorPrompt}");
            }

        } while (!success);

        return result;

    }
}