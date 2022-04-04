namespace LogWithWarning;

public class ConsoleLogger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine(message);
    }
}
