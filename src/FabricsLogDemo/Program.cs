using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;
using System.Linq;
namespace FabricLogDemo;

public class Program
{
    public static void Main(string[] args)
    {
        var r = Add(1, 2);
        Console.WriteLine(r);
    }
    private static int Add(int a, int b)
    {
        var result = a + b;
        Console.WriteLine("Add" + result);
        return result;
    }
}

public class LogAttribute : OverrideMethodAspect
{
    public override dynamic? OverrideMethod()
    {
        Console.WriteLine(meta.Target.Method.ToDisplayString() + " 开始运行.");
        var result = meta.Proceed();
        Console.WriteLine(meta.Target.Method.ToDisplayString() + " 结束运行.");
        return result;
    }
}
internal class Fabric : ProjectFabric
{
    public override void AmendProject(IProjectAmender amender)
    {
        // 添加 LogAttribute 到符合规则的方法上
        // 为名为 Add 且 private 的方法添加 LogAttribute
        amender.With(c =>
                c.Types.SelectMany(t => t.Methods)
                       .Where(t => 
                              t.Name == "Add" && 
                              t.Accessibility == Metalama.Framework.Code.Accessibility.Private)
            ).AddAspect(t => new LogAttribute());
    }
}