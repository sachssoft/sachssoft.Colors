/// © Tobias Sachs
/// Colors/HSV.cs
/// 22.01.2023

using System;

namespace tcs.Colors;

public struct HSV : IColorSpace
{
    public HSV(ColorRange h, ColorRange s, ColorRange v)
    {
        this.HueComponent = h;
        this.SaturationComponent = s;
        this.ValueComponent = v;
    }

    public HSV(ColorCode code) : this()
    {
        this.ConvertFrom(code);
    }

    public ColorRange HueComponent { get; set; }

    public ColorRange SaturationComponent { get; set; }

    public ColorRange ValueComponent { get; set; }

    public double Hue
    {
        get => this.HueComponent * 360.0;
    }

    public double H
    {
        get => this.Hue;
    }

    public double Saturation
    {
        get => this.SaturationComponent * 100.0;
    }

    public double S
    {
        get => this.Saturation;
    }

    public double Value
    {
        get => this.ValueComponent * 100.0;
    }

    public double Brightness
    {
        get => this.Value;
    }

    public double V
    {
        get => this.Value;
    }

    public int ComponentCount
    {
        get => 3;
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length == 1)
        {
            this.HueComponent = values[0];
        }
        else if (values.Length == 2)
        {
            this.HueComponent = values[0];
            this.SaturationComponent = values[1];
        }
        else if (values.Length == 2)
        {
            this.HueComponent = values[0];
            this.SaturationComponent = values[1];
            this.ValueComponent = values[2];
        }
    }

    public void ConvertFrom(ColorCode code)
    {
        // https://www.rapidtables.com/convert/color/rgb-to-hsv.html

        var r = code.Red / 255.0;
        var g = code.Green / 255.0;
        var b = code.Blue / 255.0;

        var cmax = Math.Max(r, Math.Max(g, b));
        var cmin = Math.Min(r, Math.Min(g, b));
        var delta = cmax - cmin;

        // Hue

        var h = 0.0;

        if (delta < 0.0001 /* delta == 0.0 */)
        {
            h = 0.0;
        }
        else if (r >= cmax)
        {
            h = 60.0 * ((g - b) / delta % 6);
        }
        else if (g >= cmax)
        {
            h = 60.0 * ((b - r) / delta + 2.0);
        }
        else if (b >= cmax)
        {
            h = 60.0 * ((r - g) / delta + 4.0);
        }

        h = h / 360.0;

        if (h < 0.0)
        {
            h = 1.0 + h;
        }

        // Saturation
        var s = (cmax == 0.0) ? 0.0 : (delta / cmax);

        // Value
        var v = cmax;

        // Output
        this.HueComponent = new ColorRange(h);
        this.SaturationComponent = new ColorRange(s);
        this.ValueComponent = new ColorRange(v);
    }

    public ColorCode ConvertTo()
    {
        // https://www.rapidtables.com/convert/color/hsv-to-rgb.html

        var h = (double)this.HueComponent * 360.0; // 0...360
        var s = (double)this.SaturationComponent; // 0...1
        var v = (double)this.ValueComponent; // 0...1

        var c = v * s;
        var x = c * (1.0 - Math.Abs((h / 60.0) % 2.0 - 1.0));
        var m = v - c;

        double r1 = 0.0, g1 = 0.0, b1 = 0.0;

        if (h <= 60.0)
        {
            r1 = c;
            g1 = x;
            b1 = 0.0;
        }
        else if (h >= 60.0 && h <= 120.0)
        {
            r1 = x;
            g1 = c;
            b1 = 0.0;
        }
        else if (h >= 120.0 && h <= 180.0)
        {
            r1 = 0;
            g1 = c;
            b1 = x;
        }
        else if (h >= 180.0 && h <= 240.0)
        {
            r1 = 0;
            g1 = x;
            b1 = c;
        }
        else if (h >= 240.0 && h <= 300.0)
        {
            r1 = x;
            g1 = 0.0;
            b1 = c;
        }
        else if (h >= 300.0 && h <= 360.0)
        {
            r1 = c;
            g1 = 0.0;
            b1 = x;
        }

        return new ColorCode(Convert.ToByte((r1 + m) * 255.0), Convert.ToByte((g1 + m) * 255.0), Convert.ToByte((b1 + m) * 255.0));
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("Hue", "H", this.Hue, 360.0, "0.0", "°"),
            new ColorComponent("Saturation", "S", this.Saturation, 100.0, "0.0", "%"),
            new ColorComponent("Value", "V", this.Value, 100.0, "0.0", "%")
        };
    }
}
