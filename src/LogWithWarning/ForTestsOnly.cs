using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Validation;
namespace LogWithWarning.Tests
{
    public class MyClass2
    {
        public void Call()
        {
            new MyClass1().Run();
        }
    }
}
namespace LogWithWarning
{
    public class MyClass1
    {
        [ForTestOnlyAttribute]
        public void Run()
        {

        }
        public void Call()
        {
            this.Run();
        }
    }

    class ForTestOnlyAttribute : Aspect, IAspect<IDeclaration>
    {
        private static readonly DiagnosticDefinition<IDeclaration> _warning = new(
            "DEMO02",
            Severity.Warning,
            "'{0}' 只能在一个以 '.Tests'结尾的命名空间中使用");

        public void BuildAspect(IAspectBuilder<IDeclaration> builder)
        {
            builder.WithTarget().ValidateReferences(this.ValidateReference, ReferenceKinds.All);
        }

        private void ValidateReference(in ReferenceValidationContext context)
        {
            if (!context.ReferencingType.Namespace.FullName.EndsWith(".Tests"))
            {
                context.Diagnostics.Report(_warning.WithArguments(context.ReferencedDeclaration));
            }
        }
    }
}
