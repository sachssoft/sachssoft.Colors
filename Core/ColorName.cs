using System;

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

public readonly struct ColorName
{
    public ColorName(string name, Color color, string context)
    {
        Name = name;
        Color = color;
        Context = context;
        Channels = ColorUtils.AdaptFrom(color);
    }

    public ColorName(string name, Color color)
    {
        Name = name;
        Color = color;
        Context = string.Empty;
        Channels = ColorUtils.AdaptFrom(color);
    }

    public Color Color { get; }

    public string Name { get; }

    public string Context { get; }

    internal byte[] Channels { get; }

    public override bool Equals(object? obj)
    {
        return obj is ColorName other && this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Context,
            Channels[0], Channels[1], Channels[2], Channels[3]);
    }

    private static bool ChannelsEqual(byte[] a, byte[] b)
    {
        if (a == null || b == null) return false;
        if (a.Length != b.Length) return false;

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i]) return false;
        }

        return true;
    }

    public static bool operator ==(ColorName left, ColorName right)
    {
        return left.Name == right.Name &&
               left.Context == right.Context &&
               ChannelsEqual(left.Channels, right.Channels);
    }

    public static bool operator !=(ColorName left, ColorName right)
    {
        return !(left == right);
    }

    public static bool operator ==(ColorName colorName, Color color)
    {
        return ChannelsEqual(colorName.Channels, ColorUtils.AdaptFrom(color));
    }

    public static bool operator !=(ColorName colorName, Color color)
    {
        return !(colorName == color);
    }

    public static bool operator ==(Color color, ColorName colorName)
    {
        return colorName == color;
    }

    public static bool operator !=(Color color, ColorName colorName)
    {
        return !(color == colorName);
    }
}
