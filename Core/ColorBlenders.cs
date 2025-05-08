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

public sealed class AddColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte r = ColorUtils.Clamp(a1[0] + a2[0]);
        byte g = ColorUtils.Clamp(a1[1] + a2[1]);
        byte b = ColorUtils.Clamp(a1[2] + a2[2]);
        byte ar = ColorUtils.AdaptFrom(c1)[3];
        // mix with original by amount
        byte rr = ColorUtils.Clamp(r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(b * amount + a1[2] * (1 - amount));
        return ColorUtils.AdaptTo(rr, gg, bb, ar);
    }
}

public sealed class AndColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte r = (byte)(a1[0] & a2[0]);
        byte g = (byte)(a1[1] & a2[1]);
        byte b = (byte)(a1[2] & a2[2]);
        byte ar = a1[3];
        byte rr = ColorUtils.Clamp(r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(b * amount + a1[2] * (1 - amount));
        return ColorUtils.AdaptTo(rr, gg, bb, ar);
    }
}

public sealed class BurnColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            float baseVal = a1[i] / 255f;
            float blend = a2[i] / 255f;
            float burn = blend == 0f ? 0f : 1f - (1f - baseVal) / blend;
            float mixed = baseVal * (1 - amount) + burn * amount;
            result[i] = ColorUtils.Clamp(mixed);
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}

public sealed class DarkenColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte r = (byte)Math.Min(a1[0], a2[0]);
        byte g = (byte)Math.Min(a1[1], a2[1]);
        byte b = (byte)Math.Min(a1[2], a2[2]);
        byte ar = a1[3];
        byte rr = ColorUtils.Clamp(r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(b * amount + a1[2] * (1 - amount));
        return ColorUtils.AdaptTo(rr, gg, bb, ar);
    }
}

public sealed class DifferenceColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte r = (byte)Math.Abs(a1[0] - a2[0]);
        byte g = (byte)Math.Abs(a1[1] - a2[1]);
        byte b = (byte)Math.Abs(a1[2] - a2[2]);
        byte ar = a1[3];
        byte rr = ColorUtils.Clamp(r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(b * amount + a1[2] * (1 - amount));
        return ColorUtils.AdaptTo(rr, gg, bb, ar);
    }
}

public sealed class DivideColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte r = a2[0] == 0 ? (byte)255 : ColorUtils.Clamp(a1[0] * 255 / a2[0]);
        byte g = a2[1] == 0 ? (byte)255 : ColorUtils.Clamp(a1[1] * 255 / a2[1]);
        byte b = a2[2] == 0 ? (byte)255 : ColorUtils.Clamp(a1[2] * 255 / a2[2]);
        byte ar = a1[3];
        byte rr = ColorUtils.Clamp(r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(b * amount + a1[2] * (1 - amount));
        return ColorUtils.AdaptTo(rr, gg, bb, ar);
    }
}

// ColorDodge blend: brightens to reflect light source
public sealed class DodgeColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            float baseVal = a1[i] / 255f;
            float blend = a2[i] / 255f;
            float dodge = baseVal + blend > 1f ? 1f : baseVal / (1f - blend);
            float mixed = baseVal * (1 - amount) + dodge * amount;
            result[i] = ColorUtils.Clamp(mixed);
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}

public sealed class LerpColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte rr = ColorUtils.Clamp(a1[0] * (1 - amount) + a2[0] * amount);
        byte gg = ColorUtils.Clamp(a1[1] * (1 - amount) + a2[1] * amount);
        byte bb = ColorUtils.Clamp(a1[2] * (1 - amount) + a2[2] * amount);
        return ColorUtils.AdaptTo(rr, gg, bb, ar);
    }
}

public sealed class LightenColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte r = (byte)Math.Max(a1[0], a2[0]);
        byte g = (byte)Math.Max(a1[1], a2[1]);
        byte b = (byte)Math.Max(a1[2], a2[2]);
        byte ar = a1[3];
        byte rr = ColorUtils.Clamp(r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(b * amount + a1[2] * (1 - amount));
        return ColorUtils.AdaptTo(rr, gg, bb, ar);
    }
}

public sealed class ModuloColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            int divisor = a2[i] + 1;
            byte mod = (byte)(a1[i] % divisor);
            result[i] = ColorUtils.Clamp(mod * amount + a1[i] * (1 - amount));
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}

public sealed class MultiplyColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte r = ColorUtils.Clamp(a1[0] * a2[0] / 255);
        byte g = ColorUtils.Clamp(a1[1] * a2[1] / 255);
        byte b = ColorUtils.Clamp(a1[2] * a2[2] / 255);
        byte ar = a1[3];
        byte rr = ColorUtils.Clamp(r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(b * amount + a1[2] * (1 - amount));
        return ColorUtils.AdaptTo(rr, gg, bb, ar);
    }
}

// Negate (Invert) blend: inverts the base color channels, ignores second color, then mixes
public sealed class NegateColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        byte inv_r = (byte)(255 - a1[0]);
        byte inv_g = (byte)(255 - a1[1]);
        byte inv_b = (byte)(255 - a1[2]);
        byte alpha = a1[3];
        byte rr = ColorUtils.Clamp(inv_r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(inv_g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(inv_b * amount + a1[2] * (1 - amount));
        return ColorUtils.AdaptTo(rr, gg, bb, alpha);
    }
}

public sealed class OrColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte r = (byte)(a1[0] | a2[0]);
        byte g = (byte)(a1[1] | a2[1]);
        byte b = (byte)(a1[2] | a2[2]);
        byte ar = a1[3];
        byte rr = ColorUtils.Clamp(r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(b * amount + a1[2] * (1 - amount));
        return ColorUtils.AdaptTo(rr, gg, bb, ar);
    }
}

public sealed class OverlayColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            float b = a1[i] / 255f;
            float s = a2[i] / 255f;
            float op = b < 0.5f ? 2f * b * s : 1f - 2f * (1f - b) * (1f - s);
            float mixed = b * (1 - amount) + op * amount;
            result[i] = ColorUtils.Clamp(mixed);
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}

public sealed class PowerColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            float baseVal = a1[i] / 255f;
            float exponent = (a2[i] / 255f) * amount + (1 - amount);
            float powered = MathF.Pow(baseVal, exponent) * 255f;
            result[i] = ColorUtils.Clamp(powered);
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}

public sealed class RootColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            float val = a1[i] / 255f;
            float degree = (a2[i] / 255f) * amount + 1f;
            float rooted = MathF.Pow(val, 1f / degree) * 255f;
            result[i] = ColorUtils.Clamp(rooted);
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}

public sealed class ScreenColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte ar = a1[3];
        byte[] result = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            float inv1 = 1f - a1[i] / 255f;
            float inv2 = 1f - a2[i] / 255f;
            float screen = 1f - inv1 * inv2;
            float mixed = (a1[i] / 255f) * (1 - amount) + screen * amount;
            result[i] = ColorUtils.Clamp(mixed);
        }
        result[3] = ar;
        return ColorUtils.AdaptTo(result[0], result[1], result[2], result[3]);
    }
}

public sealed class SubtractColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        amount = Math.Clamp(amount, 0f, 1f);
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);
        byte r = ColorUtils.Clamp(a1[0] - a2[0]);
        byte g = ColorUtils.Clamp(a1[1] - a2[1]);
        byte b = ColorUtils.Clamp(a1[2] - a2[2]);
        byte ar = a1[3];
        byte rr = ColorUtils.Clamp(r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(b * amount + a1[2] * (1 - amount));
        return ColorUtils.AdaptTo(rr, gg, bb, ar);
    }
}

public sealed class XorColorBlender : IColorBlender
{
    public Color Blend(Color c1, Color c2, float amount)
    {
        // amount in [0,1] beschränken
        amount = Math.Clamp(amount, 0f, 1f);

        // Farbkanäle extrahieren (RGBA)
        var a1 = ColorUtils.AdaptFrom(c1);
        var a2 = ColorUtils.AdaptFrom(c2);

        // bitweises XOR auf jedem Kanal
        byte xor_r = (byte)(a1[0] ^ a2[0]);
        byte xor_g = (byte)(a1[1] ^ a2[1]);
        byte xor_b = (byte)(a1[2] ^ a2[2]);
        byte alpha = a1[3];  // Alphakanal beibehalten

        // mit amount gegen das Original mischen
        byte rr = ColorUtils.Clamp(xor_r * amount + a1[0] * (1 - amount));
        byte gg = ColorUtils.Clamp(xor_g * amount + a1[1] * (1 - amount));
        byte bb = ColorUtils.Clamp(xor_b * amount + a1[2] * (1 - amount));

        return ColorUtils.AdaptTo(rr, gg, bb, alpha);
    }
}