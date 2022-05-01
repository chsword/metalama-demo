using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Eligibility;
using Metalama.Framework.Fabrics;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApiDemo;

public class DIAspectAttribute : OverrideFieldOrPropertyAspect
{
    public override dynamic? OverrideProperty
    {
        get
        {
            // Get the property value.
            var value = meta.Proceed();

            if (value == null)
            {
             
                // Call the service locator.
                value = meta.Cast(
                    meta.Target.FieldOrProperty.Type,
                    (meta.This.HttpContext.RequestServices as IServiceProvider).GetService(meta.Target.FieldOrProperty.Type.ToType()));

                // Set the field/property to the new value.
                meta.Target.Property.Value = value
                                             ?? throw new InvalidOperationException(
                                                 $"Cannot get a service of type {meta.Target.FieldOrProperty.Type}.");
            }

            return value;
        }
        set => throw new NotImplementedException();
    }
}

public class AspNetProjectFabric : ProjectFabric
{
    public override void AmendProject(IProjectAmender amender)
    {
        // 取 Controller结尾类中的private 且非static的field
        amender.With(c => c.Types.Where(t =>
                t.Name.EndsWith("Controller")
                )
                .SelectMany(t=>t.Fields)
                .Where(f=>f.Accessibility == Accessibility.Private
                && !f.IsStatic )

                
            )
            .AddAspect<DIAspectAttribute>();
    }
}