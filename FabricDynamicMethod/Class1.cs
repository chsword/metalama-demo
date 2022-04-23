using System.Reflection.Emit;
using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;

namespace FabricDynamicMethod;

public class AddUtils
{
    private class Fabric : TypeFabric
    {
        // 实现的方法体
        [Template]
        public int MethodTemplate()
        {
            var num = (int) meta.Tags["nums"]!;
            var result = 0;
            foreach (var targetParameter in meta.Target.Parameters)
            {
                result += targetParameter.Value;
            }

            return num;
        }

        public override void AmendType(ITypeAmender amender)
        {
            for (var i = 2; i < 15; i++)
            {
                // 生成一个方法
                var methodBuilder = amender.Advices.IntroduceMethod(
                    amender.Type,
                    nameof(this.MethodTemplate),
                    tags: new TagDictionary { ["nums"] = i });
                // 方法名
                methodBuilder.Name = "Add" + i;
                // 添加参数
                for (int parameterIndex = 1; parameterIndex <= i; parameterIndex++)
                {
                    methodBuilder.AddParameter($"x{parameterIndex}", typeof(int));
                }
                
            }
        }
    }
}