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

internal static class ColorUtils
{

#if MONOGAME || SASOGINE

    public static byte[] AdaptFrom(Color color)
    {
        return new byte[] { color.R, color.G, color.B, color.A };
    }

    public static Color AdaptTo(byte r, byte g, byte b, byte a = 255)
    {
        return new Color(r, g, b, a);
    }

#elif SKIA

    public static byte[] AdaptFrom(Color color)
    {
        return new byte[] { color.Red, color.Green, color.Blue, color.Alpha };
    }

    public static Color AdaptTo(byte r, byte g, byte b, byte a = 255)
    {
        return new Color(r, g, b, a);
    }

#elif WPF || DRAWING || AVALONIA

    public static byte[] AdaptFrom(Color color)
    {
        return new byte[] { color.R, color.G, color.B, color.A };
    }

    public static Color AdaptTo(byte r, byte g, byte b, byte a = 255)
    {
        return Color.FromArgb(a, r, g, b);
    }

#else
    public static byte[] AdaptFrom(Color color)
    {
        return new byte[] { color.Red, color.Green, color.Blue, color.Alpha };
    }

    public static Color AdaptTo(byte r, byte g, byte b, byte a = 255)
    {
        return new Color(a, r, g, b);
    }

#endif

    public static byte Clamp(int value, int min = 0, int max = 255)
    {
        return (byte)Math.Min(max, Math.Max(min, value));
    }

    public static byte Clamp(float value, float min = 0f, float max = 1f)
    {
        // Clamp den Wert zwischen min und max
        float clamped = Math.Min(max, Math.Max(min, value));

        // Skaliere den Wert auf den Bereich [0, 255] und runde ihn
        return (byte)Math.Round(clamped * 255.0f);
    }

    public static byte Clamp(double value, double min = 0.0, double max = 1.0)
    {
        // Clamp den Wert zwischen min und max
        double clamped = Math.Min(max, Math.Max(min, value));

        // Skaliere den Wert auf den Bereich [0, 255] und runde ihn
        return (byte)Math.Round(clamped * 255.0);
    }

    public static float[] AdaptToSingleFrom(Color color, bool alpha = false)
    {
        // Channels holen (entsprechend der Plattform)
        var channels = AdaptFrom(color);

        // Erstelle das Ergebnis, wobei der Alpha-Kanal optional berücksichtigt wird
        var result = new List<float>
        {
            channels[0] / 255f, // Red: Wert von 0 bis 1
            channels[1] / 255f, // Green: Wert von 0 bis 1
            channels[2] / 255f  // Blue: Wert von 0 bis 1
        };

        // Wenn der Alpha-Kanal gewünscht wird, füge ihn hinzu
        if (alpha)
        {
            result.Add(channels[3] / 255f); // Alpha: Wert von 0 bis 1
        }

        return result.ToArray();
    }

    public static double[] AdaptToDoubleFrom(Color color, bool alpha = false)
    {
        // Channels holen (entsprechend der Plattform)
        var channels = AdaptFrom(color);

        // Erstelle das Ergebnis, wobei der Alpha-Kanal optional berücksichtigt wird
        var result = new List<double>
        {
            channels[0] / 255f, // Red: Wert von 0 bis 1
            channels[1] / 255f, // Green: Wert von 0 bis 1
            channels[2] / 255f  // Blue: Wert von 0 bis 1
        };

        // Wenn der Alpha-Kanal gewünscht wird, füge ihn hinzu
        if (alpha)
        {
            result.Add(channels[3] / 255f); // Alpha: Wert von 0 bis 1
        }

        return result.ToArray();
    }

    public static Color AdaptFromSingleTo(float r, float g, float b, float a = 1f)
    {
        // Clamp the float values to the range of 0 to 1
        r = Clamp(r, 0f, 1f);
        g = Clamp(g, 0f, 1f);
        b = Clamp(b, 0f, 1f);
        a = Clamp(a, 0f, 1f);

        // Convert the float values (0 to 1) to byte values (0 to 255)
        byte red = (byte)(r * 255);
        byte green = (byte)(g * 255);
        byte blue = (byte)(b * 255);
        byte alpha = (byte)(a * 255);

        // Return the color using the appropriate platform color constructor
        return AdaptTo(red, green, blue, alpha);
    }

    public static Color AdaptFromDoubleTo(double r, double g, double b, double a = 1.0)
    {
        // Clamp the float values to the range of 0 to 1
        r = Clamp(r, 0f, 1f);
        g = Clamp(g, 0f, 1f);
        b = Clamp(b, 0f, 1f);
        a = Clamp(a, 0f, 1f);

        // Convert the float values (0 to 1) to byte values (0 to 255)
        byte red = (byte)(r * 255);
        byte green = (byte)(g * 255);
        byte blue = (byte)(b * 255);
        byte alpha = (byte)(a * 255);

        // Return the color using the appropriate platform color constructor
        return AdaptTo(red, green, blue, alpha);
    }
}