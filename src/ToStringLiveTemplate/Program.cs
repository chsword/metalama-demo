using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;

namespace ToStringLiveTemplate;
internal class Program
{
    private static void Main()
    {
        var point = new Point { X = 5, Y = 3 };
        Console.WriteLine($"point = {point}");
    }
}
internal class Point
{
    public double X;

    public double Y;

}






























[LiveTemplate] // 表示当前Aspect为VS添加LiveTempate
internal class ToStringAttribute : TypeAspect
{
    [Introduce(WhenExists = OverrideStrategy.Override, Name = "ToString")]
    public string IntroducedToString()
    {
        var stringBuilder = new InterpolatedStringBuilder();
        stringBuilder.AddText("{ ");
        stringBuilder.AddText(meta.Target.Type.Name);
        stringBuilder.AddText(" ");

        var fields = meta.Target.Type.FieldsAndProperties.Where(f => !f.IsStatic).ToList();

        var i = meta.CompileTime(0);

        foreach (var field in fields)
        {
            if (i > 0)
            {
                stringBuilder.AddText(", ");
            }

            stringBuilder.AddText(field.Name);
            stringBuilder.AddText("=");
            stringBuilder.AddExpression(field.Invokers.Final.GetValue(meta.This));

            i++;
        }

        stringBuilder.AddText(" }");

        return stringBuilder.ToValue();
    }
}