// See https://aka.ms/new-console-template for more information

using FabricDynamicMethod;

Console.WriteLine("Hello, World!");


foreach (var methodInfo in typeof(AddUtils).GetMethods())
{
    Console.WriteLine($"{methodInfo.Name}({string.Join(",", methodInfo.GetParameters().Select(t => t.Name))})");
}