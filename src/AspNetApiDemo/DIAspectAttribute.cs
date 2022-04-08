using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApiDemo
{
    public class DIAspectAttribute : TypeAspect
    {
        //public override void BuildAspect(IAspectBuilder<IConstructor> builder)
        //{
        //    base.BuildAspect(builder);
            
        //}

        private static readonly DiagnosticDefinition<int> _error = new(
            "DEMO02",
            Severity.Warning,
            "[DIAspect]只能定义在ControllerBase子类上,但当前应用在了{0}上。");

        public override void BuildAspect(IAspectBuilder<INamedType> builder)
        {
            base.BuildAspect(builder);
            //if (builder.Target.BaseType?.Name != nameof(ControllerBase))
            //{
            //    builder.Diagnostics.Report(_error.WithArguments(builder.Target.FullName));
            //}

            var ctor = builder.Target.Methods.Count;
            builder.Diagnostics.Report(_error.WithArguments(ctor));
            //builder.Advices.OverrideMethod(builder.Target.Methods,
            //    nameof(MethodLog));
        }
        [Template]
        public dynamic MethodLog()
        {
            Console.WriteLine(meta.Target.Method.ToDisplayString() + " 开始运行.");
            System.Diagnostics.Debugger.Break();
            meta.DebugBreak();
            var result = meta.Proceed();
            Console.WriteLine(meta.Target.Method.ToDisplayString() + " 结束运行.");
            return result;

        }
    }

    public class X : ConstructorAspect
    {

    }
}
