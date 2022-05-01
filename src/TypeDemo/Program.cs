using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using System.ComponentModel;

namespace TypeDemo;
class Program
{

    static void Main()
    {
      
    }
}

//[Inherited]
internal class TypeNotifyPropertyChangedAttribute : TypeAspect
{
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        // 当前类实现一个接口
        builder.Advice.ImplementInterface(builder.Target, typeof(INotifyPropertyChanged));
        // 获取所有符合要求的属性
        var props = builder.Target.Properties.Where(p => !p.IsAbstract && p.Writeability == Writeability.All);
        foreach (var property in props)
        {
            //用OverridePropertySetter重写属性或字段
            //参数1 要重写的属性 参数2 新的get实现 参数3 新的set实现
            builder.Advice.OverrideAccessors(property, null, nameof(this.OverridePropertySetter));
        }
    }
    // Interface 要实现什么成员
    [InterfaceMember]
    public event PropertyChangedEventHandler? PropertyChanged;

    // 也可以没有这个方法，直接调用 meta.This 这里只是展示另一种调用方式，更加直观
    [Introduce(WhenExists = OverrideStrategy.Ignore)]
    protected void OnPropertyChanged(string name)
    {
        this.PropertyChanged?.Invoke(meta.This, new PropertyChangedEventArgs(name));
    }

    // 重写set的模板
    [Template]
    private dynamic OverridePropertySetter(dynamic value)
    {
        if (value != meta.Target.Property.Value)
        {
            meta.Proceed();
            this.OnPropertyChanged(meta.Target.Property.Name);
        }

        return value;
    }
}