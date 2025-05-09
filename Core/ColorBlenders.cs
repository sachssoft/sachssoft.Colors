using System;

#if SASOGINE
using Microsoft.Xna.Framework;
namespace sachssoft.Sasogine.Graphics.Colors.Blenders;
#elif MONOGAME
using Microsoft.Xna.Framework;
namespace sachssoft.Monogame.Colors.Blenders;
#elif AVALONIA
using Ava = Avalonia;
namespace sachssoft.Avalonia.Colors.Blenders;
using Color = Ava.Media.Color;
#elif DRAWING
namespace sachssoft.Drawing.Colors.Blenders;
using Color = System.Drawing.Color;
#elif WPF
namespace sachssoft.WPF.Colors.Blenders;
using Color = System.Windows.Media.Color;
#elif SKIA
namespace sachssoft.Skia.Colors.Blenders;
using Color = SkiaSharp.SKColor;
#else
namespace sachssoft.Colors.Blenders;
using Color = sachssoft.Colors.ColorCode;
#endif

// OK
public sealed class MixColorBlender : IColorBlender
{
    // = Lerp, Tint
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        // Konvertiere beide Farben in die gleiche Farbrepräsentation (z. B. RGBA)
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Berechne die Mischung der Farbkomponenten (r, g, b) unter Berücksichtigung des "amount"
        byte r = ColorUtils.Clamp((int)(a1[0] + (a2[0] - a1[0]) * (float)amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (a2[1] - a1[1]) * (float)amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (a2[2] - a1[2]) * (float)amount));
        byte a = (byte)(a1[3] + (a2[3] - a1[3]) * (float)amount); // Auch Alpha-Wert berücksichtigen

        // Konvertiere zurück und gebe das Ergebnis zurück
        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class DarkenColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Berechne den minimalen Wert der beiden Farben
        byte r = ColorUtils.Clamp(Math.Min(a1[0], a2[0]));
        byte g = ColorUtils.Clamp(Math.Min(a1[1], a2[1]));
        byte b = ColorUtils.Clamp(Math.Min(a1[2], a2[2]));
        byte a = a1[3];

        // Mische die Farben basierend auf `amount`
        r = (byte)(a1[0] + (r - a1[0]) * (float)amount);
        g = (byte)(a1[1] + (g - a1[1]) * (float)amount);
        b = (byte)(a1[2] + (b - a1[2]) * (float)amount);

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class LightenColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Berechne den maximalen Wert der beiden Farben
        byte r = ColorUtils.Clamp(Math.Max(a1[0], a2[0]));
        byte g = ColorUtils.Clamp(Math.Max(a1[1], a2[1]));
        byte b = ColorUtils.Clamp(Math.Max(a1[2], a2[2]));
        byte a = a1[3];

        // Mische die Farben basierend auf `amount`
        r = (byte)(a1[0] + (r - a1[0]) * (float)amount);
        g = (byte)(a1[1] + (g - a1[1]) * (float)amount);
        b = (byte)(a1[2] + (b - a1[2]) * (float)amount);

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class DifferenceColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Berechne den Farbunterschied
        byte r = ColorUtils.Clamp(Math.Abs(a1[0] - a2[0]));
        byte g = ColorUtils.Clamp(Math.Abs(a1[1] - a2[1]));
        byte b = ColorUtils.Clamp(Math.Abs(a1[2] - a2[2]));
        byte a = a1[3];

        // Mische die Farben basierend auf `amount`
        r = (byte)(a1[0] + (r - a1[0]) * (float)amount);
        g = (byte)(a1[1] + (g - a1[1]) * (float)amount);
        b = (byte)(a1[2] + (b - a1[2]) * (float)amount);

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

public sealed class ExponentialColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        float exp_amount = (float)Math.Pow((double)amount, 2);  // Exponentielle Gewichtung

        byte r = ColorUtils.Clamp((int)(a1[0] + (a2[0] - a1[0]) * exp_amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (a2[1] - a1[1]) * exp_amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (a2[2] - a1[2]) * exp_amount));
        byte a = a1[3];

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class SaturationColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var output = c1.Adjust<Transformers.SaturationColorTransformer>(amount);
        return output.Blend<MixColorBlender>(c2, amount);
    }
}

// OK
public sealed class DesaturationColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var output = c1.Adjust<Transformers.DesaturationColorTransformer>(amount);
        return output.Blend<MixColorBlender>(c2, amount);
    }
}

// OK
public sealed class ShadowColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        float inv_amount = 1f - (float)amount;

        // Berechne gemischte Farbe in Richtung Schattenfarbe (c2), aber gedämpft
        byte r = ColorUtils.Clamp((int)((a1[0] * inv_amount) + (a2[0] * (float)amount * 0.5f)));
        byte g = ColorUtils.Clamp((int)((a1[1] * inv_amount) + (a2[1] * (float)amount * 0.5f)));
        byte b = ColorUtils.Clamp((int)((a1[2] * inv_amount) + (a2[2] * (float)amount * 0.5f)));
        byte a = a1[3]; // Original Alpha bleibt

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class ExposureColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var output = c1.Adjust<Transformers.ExposureColorTransformer>(amount);
        return output.Blend<MixColorBlender>(c2, amount);
    }
}

// OK
public sealed class AddColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        byte r = ColorUtils.Clamp((int)(a1[0] + (a2[0] * (float)amount)));
        byte g = ColorUtils.Clamp((int)(a1[1] + (a2[1] * (float)amount)));
        byte b = ColorUtils.Clamp((int)(a1[2] + (a2[2] * (float)amount)));
        byte a = a1[3];

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class SubtractColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        byte r = ColorUtils.Clamp((int)(a1[0] - (a2[0] * (float)amount)));
        byte g = ColorUtils.Clamp((int)(a1[1] - (a2[1] * (float)amount)));
        byte b = ColorUtils.Clamp((int)(a1[2] - (a2[2] * (float)amount)));
        byte a = a1[3];

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class MultiplyColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Normierte Multiplikation der Farbkanäle
        byte r_mul = (byte)((a1[0] * a2[0]) / 255);
        byte g_mul = (byte)((a1[1] * a2[1]) / 255);
        byte b_mul = (byte)((a1[2] * a2[2]) / 255);
        byte a = a1[3]; // Alpha beibehalten

        // Interpolation zwischen Originalfarbe und multiplizierter Farbe
        byte r = ColorUtils.Clamp((int)(a1[0] + (r_mul - a1[0]) * (float)amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (g_mul - a1[1]) * (float)amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (b_mul - a1[2]) * (float)amount));

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}


// OK
public sealed class DivideColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        byte r_div = (byte)((a1[0] != 0) ? Math.Min(255f, (a2[0] / Math.Max(1f, a1[0])) * 255f) : 255f);
        byte g_div = (byte)((a1[1] != 0) ? Math.Min(255f, (a2[1] / Math.Max(1f, a1[1])) * 255f) : 255f);
        byte b_div = (byte)((a1[2] != 0) ? Math.Min(255f, (a2[2] / Math.Max(1f, a1[2])) * 255f) : 255f);
        byte a = a1[3];

        byte r = ColorUtils.Clamp((int)(a1[0] + (r_div - a1[0]) * (float)amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (g_div - a1[1]) * (float)amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (b_div - a1[2]) * (float)amount));

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class ModuloColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        byte r_mod = (byte)(a1[0] != 0 ? a2[0] % a1[0] : a2[0]);
        byte g_mod = (byte)(a1[1] != 0 ? a2[1] % a1[1] : a2[1]);
        byte b_mod = (byte)(a1[2] != 0 ? a2[2] % a1[2] : a2[2]);
        byte a = a1[3];

        byte r = ColorUtils.Clamp((int)(a1[0] + (r_mod - a1[0]) * (float)amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (g_mod - a1[1]) * (float)amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (b_mod - a1[2]) * (float)amount));

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class AndColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Bitweises AND pro Kanal
        byte r_and = (byte)(a1[0] & a2[0]);
        byte g_and = (byte)(a1[1] & a2[1]);
        byte b_and = (byte)(a1[2] & a2[2]);
        byte a = a1[3];

        // Interpolation: Original + (AND - Original) * amount
        byte r = ColorUtils.Clamp((int)(a1[0] + (r_and - a1[0]) * (float)amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (g_and - a1[1]) * (float)amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (b_and - a1[2]) * (float)amount));

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class OrColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Bitweises AND pro Kanal
        byte r_and = (byte)(a1[0] | a2[0]);
        byte g_and = (byte)(a1[1] | a2[1]);
        byte b_and = (byte)(a1[2] | a2[2]);
        byte a = a1[3];

        // Interpolation: Original + (AND - Original) * amount
        byte r = ColorUtils.Clamp((int)(a1[0] + (r_and - a1[0]) * (float)amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (g_and - a1[1]) * (float)amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (b_and - a1[2]) * (float)amount));

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class XorColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Bitweises AND pro Kanal
        byte r_and = (byte)(a1[0] ^ a2[0]);
        byte g_and = (byte)(a1[1] ^ a2[1]);
        byte b_and = (byte)(a1[2] ^ a2[2]);
        byte a = a1[3];

        // Interpolation: Original + (AND - Original) * amount
        byte r = ColorUtils.Clamp((int)(a1[0] + (r_and - a1[0]) * (float)amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (g_and - a1[1]) * (float)amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (b_and - a1[2]) * (float)amount));

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class NandColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Bitweises AND pro Kanal
        byte r_and = (byte)~(a1[0] & a2[0]);
        byte g_and = (byte)~(a1[1] & a2[1]);
        byte b_and = (byte)~(a1[2] & a2[2]);
        byte a = a1[3];

        // Interpolation: Original + (AND - Original) * amount
        byte r = ColorUtils.Clamp((int)(a1[0] + (r_and - a1[0]) * (float)amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (g_and - a1[1]) * (float)amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (b_and - a1[2]) * (float)amount));

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class NorColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Bitweises AND pro Kanal
        byte r_and = (byte)~(a1[0] | a2[0]);
        byte g_and = (byte)~(a1[1] | a2[1]);
        byte b_and = (byte)~(a1[2] | a2[2]);
        byte a = a1[3];

        // Interpolation: Original + (AND - Original) * amount
        byte r = ColorUtils.Clamp((int)(a1[0] + (r_and - a1[0]) * (float)amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (g_and - a1[1]) * (float)amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (b_and - a1[2]) * (float)amount));

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class NxorColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // Bitweises AND pro Kanal
        byte r_and = (byte)~(a1[0] ^ a2[0]);
        byte g_and = (byte)~(a1[1] ^ a2[1]);
        byte b_and = (byte)~(a1[2] ^ a2[2]);
        byte a = a1[3];

        // Interpolation: Original + (AND - Original) * amount
        byte r = ColorUtils.Clamp((int)(a1[0] + (r_and - a1[0]) * (float)amount));
        byte g = ColorUtils.Clamp((int)(a1[1] + (g_and - a1[1]) * (float)amount));
        byte b = ColorUtils.Clamp((int)(a1[2] + (b_and - a1[2]) * (float)amount));

        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class BurnColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            float baseVal = a1[i] / 255f;
            float blend = a2[i] / 255f;
            float burn = blend == 0f ? 0f : 1f - (1f - baseVal) / blend;
            float mixed = baseVal * (1f - (float)amount) + burn * (float)amount;
            result[i] = ColorUtils.Clamp(mixed);
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}

// OK
public sealed class DodgeColorBlender : IColorBlender
{
    // ColorDodge blend: brightens to reflect light source
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            float baseVal = a1[i] / 255f;
            float blend = a2[i] / 255f;
            float dodge = baseVal + blend > 1f ? 1f : baseVal / (1f - blend);
            float mixed = baseVal * (1f - (float)amount) + dodge * (float)amount;
            result[i] = ColorUtils.Clamp(mixed);
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}

// OK
public sealed class InterpolateColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var channels1 = ColorUtils.AdaptFrom(c1);
        var channels2 = ColorUtils.AdaptFrom(c2);

        // Lineare Interpolation der Farbkanäle
        byte r = (byte)(channels1[0] + (channels2[0] - channels1[0]) * (float)amount);
        byte g = (byte)(channels1[1] + (channels2[2] - channels1[1]) * (float)amount);
        byte b = (byte)(channels1[2] + (channels2[3] - channels1[2]) * (float)amount);
        byte a = (byte)(channels1[3] + (channels2[3] - channels1[3]) * (float)amount);

        // Rückgabe der interpolierten Farbe
        return ColorUtils.AdaptTo(r, g, b, a);
    }
}

// OK
public sealed class OverlayColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            float b = a1[i] / 255f;
            float s = a2[i] / 255f;
            float op = b < 0.5f ? 2f * b * s : 1f - 2f * (1f - b) * (1f - s);
            float mixed = b * (1f - (float)amount) + op * (float)amount;
            result[i] = ColorUtils.Clamp(mixed);
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}

// OK
public sealed class ScreenColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, ColorRange amount)
    {
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            float inv1 = 1f - a1[i] / 255f;
            float inv2 = 1f - a2[i] / 255f;
            float screen = 1f - inv1 * inv2;
            float mixed = (a1[i] / 255f) * (1f - (float)amount) + screen * (float)amount;
            result[i] = ColorUtils.Clamp(mixed);
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}
