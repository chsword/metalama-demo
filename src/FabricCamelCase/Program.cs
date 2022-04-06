using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Fabrics;
using Metalama.Framework.Validation;
using System.Linq;
namespace FabricCamelCaseDemo;

public class Program
{
    int count = 0;
    int _total = 0;
    public static void Main(string[] args)
    {
        new Program().Run();
    }
    void Run()
    {
        Console.WriteLine(count);
        Console.WriteLine(_total);
    }
}
class Fabric : NamespaceFabric
{
    private static readonly DiagnosticDefinition<string> _warning = new(
 "DEMO04",
 Severity.Warning,
 "'{0}'必须使用驼峰命名法并以'_'开头");

    public override void AmendNamespace(INamespaceAmender amender)
    {
        amender.WithTargetMembers(c => 
                                    c.AllTypes.SelectMany(t=>t.Fields)
                                    .Where(t => t.Accessibility == Accessibility.Private && !t.IsStatic
                                    )
                                 )
            .RegisterFinalValidator(this.FinalValidator);
    }

    private void FinalValidator(in DeclarationValidationContext context)
    {
        var fullname = context.Declaration.ToDisplayString();
        var fieldName = fullname.Split('.').LastOrDefault();
        if (fieldName!=null && (!fieldName.StartsWith("_") || !char.IsLower(fieldName[1])))
        {
            context.Diagnostics.Report(_warning.WithArguments(fieldName));
        }
    }
}