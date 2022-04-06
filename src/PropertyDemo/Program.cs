using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using System.ComponentModel;

namespace PropertyDemo;
class Program
{

    static void Main()
    { 
    }
}

public class MyModel: INotifyPropertyChanged
{
    [NotifyPropertyChanged]
    public int Id { get; set; }
    [NotifyPropertyChanged]
    public string Name { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}
public class NotifyPropertyChangedAttribute : OverrideFieldOrPropertyAspect
{
    public override dynamic OverrideProperty
    {
        // 保留原本get的逻辑
        get => meta.Proceed();
        set
        {
            // 判断当前属性的Value与传入value是否相等
            if (meta.Target.Property.Value != value)
            {
                // 原本set的逻辑
                meta.Proceed();
                // 这里的This等同于调用类的This
                meta.This.PropertyChanged?.Invoke(meta.This, new PropertyChangedEventArgs(meta.Target.Property.Name));
            }
        }
    }
}