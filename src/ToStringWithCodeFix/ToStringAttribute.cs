using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.CodeFixes;
namespace ToStringWithCodeFix;

public class ToStringAttribute : TypeAspect
{
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        base.BuildAspect(builder);
        // 添加一个建议手动实现的重构提示
        if (builder.AspectInstance.Predecessors[0].Instance is IAttribute attribute)
        {
            builder.Diagnostics.Suggest(
                new CodeFix("将 [ToString] 切换至手动实现",
                    async (codeFixBuilder) =>
                    {
                       // TODO  await codeFixBuilder.ApplyAspectAsync(codeFixBuilder,builder.Target);
                        await codeFixBuilder.RemoveAttributesAsync(builder.Target, typeof(ToStringAttribute));
                    }),
                builder.Target);
        }
    }

    ///// <summary>
    ///// 当点击手动实现时的操作
    ///// </summary>
 
    //private async Task ImplementManually(ICodeActionBuilder builder, INamedType targetType)
    //{
    //    await builder.ApplyAspectAsync(targetType, this);
    //    await builder.RemoveAttributesAsync(targetType, typeof(ToStringAttribute));
    //}

    [Introduce(WhenExists = OverrideStrategy.Override, Name = "ToString")]
    public string IntroducedToString()
    {
        // 获取非静态字段
        var fields = meta.Target.Type.FieldsAndProperties.Where(f => !f.IsStatic).ToList();

        // 构建一个$""字符串
        var stringBuilder = new InterpolatedStringBuilder();
        stringBuilder.AddText("{ ");
        stringBuilder.AddText(meta.Target.Type.Name);
        stringBuilder.AddText(" ");

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
