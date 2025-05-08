
using System;
using System.Collections.Generic;

#if SASOGINE
using Microsoft.Xna.Framework;
namespace sachssoft.Sasogine.Graphics.Colors;
#elif MONOGAME
using Microsoft.Xna.Framework;
namespace sachssoft.Monogame.Colors;
#elif AVALONIA
using Ava = Avalonia;
namespace sachssoft.Avalonia.Colors;
using Color = Ava.Media.Color;
#elif DRAWING
namespace sachssoft.Drawing.Colors;
using Color = System.Drawing.Color;
#elif WPF
namespace sachssoft.WPF.Colors;
using Color = System.Windows.Media.Color;
#elif SKIA
namespace sachssoft.Skia.Colors;
using Color = SkiaSharp.SKColor;
#else
namespace sachssoft.Colors;
using Color = sachssoft.Colors.ColorCode;
#endif

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
