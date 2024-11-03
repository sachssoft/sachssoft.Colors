/// © Tobias Sachs
/// tcs_CoreVisuals
/// Colors/ColorBlends.cs
/// 22.01.2023

using System;

namespace tcs.Colors;

public static class ColorBlends
{
    public static ColorCode Add(ColorCode color1, ColorCode color2, bool alpha = false)
    {
        var ca = Math.Min(color1.Alpha + color2.Alpha, 255.0);
        var cr = Math.Min(color1.Red + color2.Red, 255.0);
        var cg = Math.Min(color1.Green + color2.Green, 255.0);
        var cb = Math.Min(color1.Blue + color2.Blue, 255.0);

        return new ColorCode((alpha == true) ? (byte)ca : (byte)255, (byte)cr, (byte)cg, (byte)cb);
    }

    public static ColorCode Substract(ColorCode color1, ColorCode color2, bool alpha = false)
    {
        var ca = Math.Max(color1.Alpha - color2.Alpha, 0);
        var cr = Math.Max(color1.Red - color2.Red, 0);
        var cg = Math.Max(color1.Green - color2.Green, 0);
        var cb = Math.Max(color1.Blue - color2.Blue, 0);

        return new ColorCode((alpha == true) ? (byte)ca : (byte)255, (byte)cr, (byte)cg, (byte)cb);
    }

    public static ColorCode And(ColorCode color1, ColorCode color2, bool alpha = false)
    {
        var ca = color1.Alpha & color2.Alpha;
        var cr = color1.Red & color2.Red;
        var cg = color1.Green & color2.Green;
        var cb = color1.Blue & color2.Blue;

        return new ColorCode((alpha == true) ? (byte)ca : (byte)255, (byte)cr, (byte)cg, (byte)cb);
    }

    public static ColorCode Or(ColorCode color1, ColorCode color2, bool alpha = false)
    {
        var ca = color1.Alpha | color2.Alpha;
        var cr = color1.Red | color2.Red;
        var cg = color1.Green | color2.Green;
        var cb = color1.Blue | color2.Blue;

        return new ColorCode((alpha == true) ? (byte)ca : (byte)255, (byte)cr, (byte)cg, (byte)cb);
    }

    public static ColorCode Xor(ColorCode color1, ColorCode color2, bool alpha = false)
    {
        var ca = color1.Alpha ^ color2.Alpha;
        var cr = color1.Red ^ color2.Red;
        var cg = color1.Green ^ color2.Green;
        var cb = color1.Blue ^ color2.Blue;

        return new ColorCode((alpha == true) ? (byte)ca : (byte)255, (byte)cr, (byte)cg, (byte)cb);
    }

    public static ColorCode Mix(ColorCode color1, ColorCode color2, bool alpha = false)
    {
        var ca = (color1.Alpha + color2.Alpha) / 2.0;
        var cr = (color1.Red + color2.Red) / 2.0;
        var cg = (color1.Green + color2.Green) / 2.0;
        var cb = (color1.Blue + color2.Blue) / 2.0;

        return new ColorCode((alpha == true) ? (byte)ca : (byte)255, (byte)cr, (byte)cg, (byte)cb);
    }

    public static ColorCode Gray(ColorCode color)
    {
        var g = (color.Red + color.Green + color.Blue) / 3.0;

        return new ColorCode(color.Alpha, (byte)g, (byte)g, (byte)g);
    }
}
