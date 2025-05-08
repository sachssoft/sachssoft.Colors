using System;
using System.Collections.Generic;
using System.Reflection;

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
    internal protected ColorNameEnumerator() { }

    public static IEnumerable<ColorName> GetNames<T>() where T : ColorNameEnumerator
    {
        var properties = typeof(T).GetProperties(BindingFlags.Static | BindingFlags.Public);
        foreach (var property in properties)
        {
            if (property.CanRead &&
                property.GetMethod?.ReturnType == typeof(ColorName))
            {
                yield return (ColorName)property.GetValue(null)!;
            }
        }
    }

    public static IEnumerable<ColorName> Find<T>(Color color) where T : ColorNameEnumerator
        => Find<T>(n => n == color);

    public static IEnumerable<ColorName> Find<T>(ColorName name) where T : ColorNameEnumerator
        => Find<T>(n => n == name);

    public static IEnumerable<ColorName> Find<T>(string name, StringComparison comparison = StringComparison.Ordinal) where T : ColorNameEnumerator
    {
        return Find<T>(n => string.Equals(n.Name, name, comparison));
    }

    public static IEnumerable<ColorName> Find<T>(Func<ColorName, bool> match) where T : ColorNameEnumerator
    {
        foreach (var name in GetNames<T>())
        {
            if (match(name))
                yield return name;
        }
    }
}
