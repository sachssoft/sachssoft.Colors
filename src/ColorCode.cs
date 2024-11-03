/// © Tobias Sachs
/// tcs_CoreVisuals
/// Colors/ColorCode.cs
/// 22.01.2023

using System;
using System.Text.RegularExpressions;

namespace tcs.Colors;

public struct ColorCode
{
    public const double WebSafe = 5;

    public ColorCode(uint value)
    {
        Value = value;
    }

    public ColorCode(uint value, byte alpha)
    {
        Value = value | (uint)(alpha << 24);
    }

    public ColorCode(int value)
    {
        Value = (uint)value;
    }

    public ColorCode(int value, byte alpha)
    {
        Value = (uint)(value | (alpha << 24));
    }

    public ColorCode(byte alpha, byte red, byte green, byte blue)
    {
        unchecked
        {
            Value = (uint)((alpha << 32) | (red << 24) | (green << 16) | (blue << 8));
        }
    }

    public ColorCode(byte red, byte green, byte blue) : this(255, red, green, blue)
    {
    }

    public static ColorCode Opacity(ColorCode code, ColorRange value)
    {
        var ca = value * 255.0;

        return new ColorCode((byte)ca, code.Red, code.Green, code.Blue);
    }

    public static ColorCode Opacity(ColorCode code, byte value)
    {
        return new ColorCode(value, code.Red, code.Green, code.Blue);
    }

    public ColorCode Opacity(ColorRange value)
    {
        return ColorCode.Opacity(this, value);
    }

    public ColorCode Opacity(byte value)
    {
        return ColorCode.Opacity(this, value);
    }

    public static ColorCode Darker(ColorCode base_color, ColorRange value)
    {
        var xmax = (double)ColorRange.MaxValue;
        var x = (double)value.Value;
        var s = 1.0 - (1.0 / xmax) * x;
        var cr = base_color.Red * s;
        var cg = base_color.Green * s;
        var cb = base_color.Blue * s;

        return new ColorCode(base_color.Alpha, (byte)cr, (byte)cg, (byte)cb);
    }

    public ColorCode Darker(ColorRange value)
    {
        return ColorCode.Darker(this, value);
    }

    public static ColorCode Lighter(ColorCode base_color, ColorRange value)
    {
        var xmax = (double)ColorRange.MaxValue;
        var x = (double)value.Value;
        var cr = base_color.Red + ((255 - base_color.Red) * (x / xmax));
        var cg = base_color.Green + ((255 - base_color.Green) * (x / xmax));
        var cb = base_color.Blue + ((255 - base_color.Blue) * (x / xmax));

        return new ColorCode(base_color.Alpha, (byte)cr, (byte)cg, (byte)cb);
    }

    public ColorCode Lighter(ColorRange value)
    {
        return ColorCode.Lighter(this, value);
    }

    public static ColorCode Multiply(ColorCode base_color, ColorRange value)
    {
        var cr = (double)base_color.Red * value;
        var cg = (double)base_color.Green * value;
        var cb = (double)base_color.Blue * value;

        return new ColorCode(base_color.Alpha, (byte)cr, (byte)cg, (byte)cb);
    }

    public ColorCode Multiply(ColorRange value)
    {
        return ColorCode.Multiply(this, value);
    }

    public static ColorCode Add(ColorCode base_color, ColorRange value)
    {
        var cr = Math.Min(base_color.Red + (value * 255.0), 255.0);
        var cg = Math.Min(base_color.Green + (value * 255.0), 255.0);
        var cb = Math.Min(base_color.Blue + (value * 255.0), 255.0);

        return new ColorCode(base_color.Alpha, (byte)cr, (byte)cg, (byte)cb);
    }

    public ColorCode Add(ColorRange value)
    {
        return ColorCode.Add(this, value);
    }

    public static ColorCode Substract(ColorCode base_color, ColorRange value)
    {
        var cr = Math.Max(base_color.Red - (value * 255.0), 0.0);
        var cg = Math.Max(base_color.Green - (value * 255.0), 0.0);
        var cb = Math.Max(base_color.Blue - (value * 255.0), 0.0);

        return new ColorCode(base_color.Alpha, (byte)cr, (byte)cg, (byte)cb);
    }

    public ColorCode Substract(ColorRange value)
    {
        return ColorCode.Substract(this, value);
    }

    public static ColorCode Difference(ColorCode base_color, ColorRange value)
    {
        var cr = Math.Abs(base_color.Red - (value * 255.0));
        var cg = Math.Abs(base_color.Green - (value * 255.0));
        var cb = Math.Abs(base_color.Blue - (value * 255.0));

        return new ColorCode(base_color.Alpha, (byte)cr, (byte)cg, (byte)cb);
    }

    public ColorCode Difference(ColorRange value)
    {
        return ColorCode.Difference(this, value);
    }

    public static ColorCode And(ColorCode base_color, ColorRange value)
    {
        var cr = base_color.Red & (byte)(value * 255.0);
        var cg = base_color.Green & (byte)(value * 255.0);
        var cb = base_color.Blue & (byte)(value * 255.0);

        return new ColorCode(base_color.Alpha, (byte)cr, (byte)cg, (byte)cb);
    }
    public ColorCode And(ColorRange value)
    {
        return ColorCode.And(this, value);
    }

    public static ColorCode Or(ColorCode base_color, ColorRange value)
    {
        var cr = base_color.Red | (byte)(value * 255.0);
        var cg = base_color.Green | (byte)(value * 255.0);
        var cb = base_color.Blue | (byte)(value * 255.0);

        return new ColorCode(base_color.Alpha, (byte)cr, (byte)cg, (byte)cb);
    }
    public ColorCode Or(ColorRange value)
    {
        return ColorCode.Or(this, value);
    }

    public static ColorCode Xor(ColorCode base_color, ColorRange value)
    {
        var cr = base_color.Red ^ (byte)(value * 255.0);
        var cg = base_color.Green ^ (byte)(value * 255.0);
        var cb = base_color.Blue ^ (byte)(value * 255.0);

        return new ColorCode(base_color.Alpha, (byte)cr, (byte)cg, (byte)cb);
    }

    public ColorCode Xor(ColorRange value)
    {
        return ColorCode.Xor(this, value);
    }

    public static ColorCode Invert(ColorCode base_color, ColorRange level)
    {
        return new ColorCode(base_color.Alpha,
            (byte)(255.0 - (base_color.Red * (double)level.Value)),
            (byte)(255.0 - (base_color.Green * (double)level.Value)),
            (byte)(255.0 - (base_color.Blue * (double)level.Value)));
    }

    public ColorCode Invert(ColorRange value)
    {
        return ColorCode.Invert(this, value);
    }

    public static ColorCode Swap(ColorCode color, ColorComponentSwap swap)
    {
        switch (swap)
        {
            case ColorComponentSwap.GRB:
                return new ColorCode(color.Alpha, color.Green, color.Red, color.Blue);
            case ColorComponentSwap.GBR:
                return new ColorCode(color.Alpha, color.Green, color.Blue, color.Red);
            case ColorComponentSwap.BRG:
                return new ColorCode(color.Alpha, color.Blue, color.Red, color.Green);
            case ColorComponentSwap.BGR:
                return new ColorCode(color.Alpha, color.Blue, color.Green, color.Red);
            case ColorComponentSwap.RBG:
            default:
                return new ColorCode(color.Alpha, color.Red, color.Blue, color.Green);
        }
    }

    public ColorCode Swap(ColorComponentSwap swap)
    {
        return ColorCode.Swap(this, swap);
    }

    public uint Value
    {
        get;
        set;
    }

    public byte Alpha
    {
        get => (byte)((Value >> 32) & 0xFF);
    }

    public byte Red
    {
        get => (byte)((Value >> 24) & 0xFF);
    }

    public byte Green
    {
        get => (byte)((Value >> 16) & 0xFF);
    }

    public byte Blue
    {
        get => (byte)((Value >> 8) & 0xFF);
    }

    public bool IsSafe(double shades = ColorCode.WebSafe)
    {
        // https://cloford.com/resources/colours/websafe1.htm

        // Websafe Shades = 5 (255 / (5))
        //    // 0 = 00
        //    // 51 = 33
        //    // 102 = 66
        //    // 153 = 99
        //    // 204 = CC
        //    // 255 = FF

        double shade_key = 255.0 / shades;

        var sr = Red % shade_key == 0;
        var sg = Green % shade_key == 0;
        var sb = Blue % shade_key == 0;

        return sr & sg & sb;
    }

    public bool IsWebSafe() => IsSafe(ColorCode.WebSafe);

    // Näherung: Tontrennung / Farbtiefe !!
    // Selbst erfunden
    public static ColorCode Approach(ColorCode base_color, ColorRange value)
    {
        // https://en.wikipedia.org/wiki/Web_colors#Safest_web_colors

        double shade_key = 255.0 / (255.0 * value);

        var nr = Math.Round(base_color.Red / shade_key) * shade_key;
        var ng = Math.Round(base_color.Green / shade_key) * shade_key;
        var nb = Math.Round(base_color.Blue / shade_key) * shade_key;

        return new ColorCode(base_color.Alpha, (byte)nr, (byte)ng, (byte)nb);
    }

    public ColorCode Approach(ColorRange value)
    {
        return ColorCode.Approach(this, value);
    }

    public string ToHexString(bool include_alpha = true)
    {
        return ((include_alpha == true) ? Alpha.ToString("X2") : "") +
            Red.ToString("X2") +
            Blue.ToString("X2") +
            Green.ToString("X2");
    }

    public static string ToHexString(byte alpha, byte red, byte green, byte blue)
    {
        return (new ColorCode(alpha, red, green, blue)).ToHexString(true);
    }

    public static string ToHexString(byte red, byte green, byte blue)
    {
        return (new ColorCode(red, green, blue)).ToHexString(false);
    }

    public TSpace ToSpace<TSpace>() where TSpace : IColorSpace, new()
    {
        var space = new TSpace();
        space.ConvertFrom(this);
        return space;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return $"[A:{Alpha}, R:{Red}, G:{Green}, B:{Blue}]";
    }

    public static ColorCode FromHexString(string hex)
    {
        if (hex.StartsWith("#") == true)
        {
            hex = hex.Substring(1);
        }
        else if (hex.StartsWith("&H") == true || hex.StartsWith("&h") == true)
        {
            hex = hex.Substring(2);
        }

        if (new Regex("^([0-9A-Fa-f]{6}|[0-9A-Fa-f]{8})\\b$").IsMatch(hex) == false)
        {
            throw new FormatException("Invalid hex code format");
        }

        // With alpha
        if (hex.Length == 8)
        {
            var a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            var r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            var g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            var b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));

            return new ColorCode(a, r, g, b);
        }
        // Without alpha
        else if (hex.Length == 6)
        {
            var r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            var g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            var b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));

            return new ColorCode(255, r, g, b);
        }

        throw new FormatException("Invalid hex code");
    }
}
