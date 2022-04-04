﻿using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace LogDemo {
    public class Program
    {
        public static void Main(string[] args)
        {
            var r = Add(1, 2);
            Console.WriteLine(r);
        }
        // 在这个方法中使用了下面的Attribute
        [LogAttribute]
        private static int Add(int a, int b)
        {
            var result = a + b;
            Console.WriteLine("Add" + result);
            return result;
        }
    }
    // 这里是增加的 Attribute
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
    public class Log2Attribute : MethodAspect
    {
        public override void BuildAspect(IAspectBuilder<IMethod> builder)
        {
           builder.Advices.OverrideMethod(builder.Target,nameof(this.MethodLog));
        }
        [Template]
        public dynamic MethodLog()
        {
            Console.WriteLine(meta.Target.Method.ToDisplayString() + " 开始运行.");
            var result = meta.Proceed();
            Console.WriteLine(meta.Target.Method.ToDisplayString() + " 结束运行.");
            return result;

        }
    }
}
