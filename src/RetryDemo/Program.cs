using Metalama.Framework.Aspects;

namespace RetryDemo;
class Program
{
    static int _callCount;

    [Retry(RetryCount = 5)]
    static void MyMethod()
    {
        _callCount++;
        Console.WriteLine($"当前是第{_callCount}次调用.");
        if (_callCount <= 2)
        {
            Console.WriteLine("前两次直接抛异常:-(");
            throw new TimeoutException();
        }
        else
        {
            Console.WriteLine("成功 :-)");
        }
    }


    static void Main()
    {
        MyMethod();
        //for (int i = 0; i < 3; i++)
        //{
        //    try
        //    {
        //        MyMethod();
        //        break;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Console.WriteLine(ex);
        //    }
        //}
        
    }
}
internal class RetryAttribute : OverrideMethodAspect
{
    public int RetryCount { get; set; } = 3;
    // 应用到方法的切面模板
    public override dynamic? OverrideMethod()
    {
        for (var i = 0; ; i++)
        {
            try
            {
                return meta.Proceed(); // 这是实际调用方法的位置
            }
            catch (Exception e) when (i < this.RetryCount)
            {
                Console.WriteLine($"发生异常 {e.Message.GetType().Name}. 1秒后重试.");
                Thread.Sleep(1000);
            }
        }
    }
}
