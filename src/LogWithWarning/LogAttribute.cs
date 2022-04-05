using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Eligibility;

namespace LogWithWarning;

// 这里是增加的 Attribute
public class LogAttribute : OverrideMethodAspect
{
    static DiagnosticDefinition<(INamedType DeclaringType, IMethod Method)> _loggerFieldNotFoundError = new(
    "DEMO01",
    Severity.Error,
    "类型'{0}'必须包含ILogger类型的字段 '_logger'因为使用了[Log]Aspect在'{1}'上.");

    // Entry point of the aspect.
    public override void BuildAspect(IAspectBuilder<IMethod> builder)
    {
        // 此处必须调用，否则目标方法将不会被覆盖，因为这里Override与BuildAspect共同使用了
        base.BuildAspect(builder);
      
        // 验证字段
        var loggerField = builder.Target.DeclaringType.Fields.OfName("_logger").SingleOrDefault();
        if (loggerField == null || !loggerField.Type.Is(typeof(ILogger)) || loggerField.IsStatic)
        {
            // 报告异常
            builder.Diagnostics.Report(_loggerFieldNotFoundError.WithArguments((builder.Target.DeclaringType, builder.Target)), builder.Target.DeclaringType);
            // 不执行Aspect
            builder.SkipAspect();
            return;
        }
    }
    public override void BuildEligibility(IEligibilityBuilder<IMethod> builder)
    {
        base.BuildEligibility(builder);
        builder.MustBeNonStatic();
    }
    public override dynamic? OverrideMethod()
    {
        meta.This._logger.Info(meta.Target.Method.ToDisplayString() + " 开始运行.");
        var result = meta.Proceed();
        meta.This._logger.Info(meta.Target.Method.ToDisplayString() + " 结束运行.");
        return result;
    }
}