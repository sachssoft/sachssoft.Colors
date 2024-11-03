/// © Tobias Sachs
/// tcs_CoreVisuals
/// Colors/ColorEnumerator.cs
/// 22.01.2023

using System;
using System.Collections.Generic;

namespace tcs.Colors;

public abstract class ColorNameEnumerator
{
    protected ColorNameEnumerator() { }

    public static IEnumerable<ColorName> GetNames<T>() where T : ColorNameEnumerator
    {
        var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);

        foreach (var property in properties)
        {
            if (property.CanRead == true && property.GetMethod != null)
            {
                if (property.GetMethod.ReturnType == typeof(ColorName))
                {
                    yield return (ColorName)property.GetValue(null)!;
                }
            }
        }
    }

    public static IEnumerable<ColorName> Find<T>(Func<ColorName, bool> filter) where T : ColorNameEnumerator
    {
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);

        foreach (var property in properties)
        {
            if (property.CanRead == true && property.GetMethod != null)
            {
                if (property.GetMethod.ReturnType == typeof(ColorName))
                {
                    var name = (ColorName)property.GetValue(null)!;

                    if (filter.Invoke(name) == false)
                    {
                        yield return name;
                    }
                }
            }
        }
    }
}
