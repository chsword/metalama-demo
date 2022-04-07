
using System.Linq;
using System.Threading.Tasks;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.CodeFixes;
namespace ToStringWithCodeFix;
[ToString]
internal class Point
{
    public double X;

    public double Y;
}

internal class Program
{
    private static void Main()
    {
        var point = new Point { X = 5, Y = 3 };

        Console.WriteLine($"point = {point}");
    }
}
 