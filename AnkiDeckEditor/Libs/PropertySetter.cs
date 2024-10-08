using System;
using System.Linq;
using System.Reflection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AnkiDeckEditor.Libs;

public static class PropertySetter
{
    public static void Set<TSource, TTarget>(ref TSource source, ref TTarget target)
    {
        var sourceType = source?.GetType();
        var targetType = target?.GetType();
        var sourceProperties = sourceType!.GetRuntimeProperties();

        foreach (var property in sourceProperties)
            try
            {
                var targetProperty = targetType!.GetRuntimeProperties().First(p => p.Name.Equals(property.Name));

                // todo: error: no attributes
                targetProperty.SetValue(target, property.GetValue(source));
            }
            catch (Exception e) when (e is InvalidOperationException or ArgumentNullException)
            {
            }
    }

    public static void SetReactive<TSource, TTarget>(ref TSource source, ref TTarget target)
        where TTarget : IReactiveObject
    {
        var sourceType = source?.GetType();
        var targetType = target.GetType();
        var sourceProperties = sourceType!.GetRuntimeProperties();

        foreach (var property in sourceProperties)
            try
            {
                var targetProperty = targetType.GetRuntimeProperties().First(p => p.Name.Equals(property.Name));
                var condition = (
                    targetProperty.GetCustomAttributes().ToList().FirstOrDefault() != null,
                    targetProperty.GetCustomAttributes().ToList().FirstOrDefault() is ReactiveAttribute);

                if (condition is not { Item1: true, Item2: true }) continue;
                targetProperty.SetValue(target, property.GetValue(source));
            }
            catch (Exception e) when (e is InvalidOperationException or ArgumentNullException)
            {
            }
    }
}