namespace LogWithWarning;
class Program
{
    ILogger _logger = new ConsoleLogger();
    public static void Main(string[] args)
    {
        var r = new Program().Add(1, 2);
        Console.WriteLine(r);
    }
    // 在这个方法中使用了下面的Attribute
    [LogAttribute]
    private int Add(int a, int b)
    {
        var result = a + b;
        // _logger.Info("Add" + result);
        return result;
    }
}
