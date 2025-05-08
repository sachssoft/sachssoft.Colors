using System;
using System.Linq;
using System.Xml.Linq;

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

public static class ColorExtensions
{

    public static Color Adjust<T>(this Color color, float amount) where T : IColorTransformer, new()
    {
        var transformer = new T();
        return transformer.Transform(color, amount);
    }

    public static Color Adjust<T>(this Color color, float amount, float factor) where T : IFactorColorTransformer, new()
    {
        var transformer = new T();
        return transformer.Transform(color, amount);
    }

    public static Color Blend<T>(this Color color, Color other, float amount) where T : IColorBlender, new()
    {
        var blender = new T();
        return blender.Blend(color, other, amount);
    }

    public static Color FromSpace<T>(this T space) where T : struct, IColorSpace
    {
        return space.ConvertTo();
    }

    public static T ToSpace<T>(this Color color) where T : struct, IColorSpace
    {
        var space = new T();
        space.ConvertFrom(color);
        return space;
    }

    public static TOutput Transform<TInput, TOutput>(this TInput space) where TInput : struct, IColorSpace where TOutput : struct, IColorSpace
    {
        var output = new TOutput();
        output.ConvertFrom(space.ConvertTo());
        return output;
    }

    public static string ToHex(this Color color, bool alpha = true, bool upper_case = false)
    {
        var channels = ColorUtils.AdaptFrom(color);

        var r = channels[0];
        var g = channels[1];
        var b = channels[2];
        var a = channels[3];

        string hexValue;

        // Wenn Alpha einbezogen werden soll
        if (alpha)
        {
            hexValue = $"#{a:X2}{r:X2}{g:X2}{b:X2}";  // Mit Alpha-Kanal
        }
        else
        {
            hexValue = $"#{r:X2}{g:X2}{b:X2}";  // Ohne Alpha-Kanal
        }

        // Falls upper_case true ist, den gesamten Hex-Wert in Großbuchstaben umwandeln
        return upper_case ? hexValue.ToUpper() : hexValue.ToLower();
    }

    public static Color FromHexToColor(this string hex)
        => FromHexToColor(hex, false);

    public static Color FromHexToColor(this string hex, bool alpha)
    {
        // Entferne das führende "#" falls vorhanden
        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1);
        }

        // Prüfen, ob die Länge des Hex-Strings korrekt ist
        int expectedLength = alpha ? 8 : 6;
        if (hex.Length != expectedLength)
        {
            throw new ArgumentException("Hex string must be 6 or 8 characters in length.");
        }

        // Extrahieren der Farbkanäle aus dem Hex-String
        int r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        int b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        // Wenn Alpha gewünscht ist, extrahiere auch den Alpha-Wert
        int a = alpha ? int.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : 255;

        // Rückgabe der Color
        return ColorUtils.AdaptTo((byte)r, (byte)g, (byte)b, (byte)a);
    }

    public static string? ToKnownName<T>(this Color color) where T : ColorNameEnumerator
    {
        foreach (var name in ColorNameEnumerator.Find<T>(color))
        {
            return name.Name;
        }

        return null;
    }

    public static Color? FromKnownNameToColor<T>(this string name, StringComparison comparison = StringComparison.Ordinal) where T : ColorNameEnumerator
    {
        var found = ColorNameEnumerator.Find<T>(name, comparison).FirstOrDefault(x => x.Name == name);
        return found.Equals(default(ColorName)) ? (Color?)null : found.Color;
    }
}