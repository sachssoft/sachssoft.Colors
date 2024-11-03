/// © Tobias Sachs
/// Colors/CMYK.cs
/// 22.01.2023

using System;

namespace tcs.Colors;

public struct CMYK : IColorSpace
{
    public CMYK(ColorRange c, ColorRange m, ColorRange y, ColorRange k)
    {
        this.CyanComponent = c;
        this.MagentaComponent = m;
        this.YellowComponent = y;
        this.BlackComponent = k;
    }

    public ColorRange CyanComponent { get; set; }

    public ColorRange MagentaComponent { get; set; }

    public ColorRange YellowComponent { get; set; }

    public ColorRange BlackComponent { get; set; }

    public double Cyan
    {
        get => this.CyanComponent * 100.0;
    }

    public double C
    {
        get => this.Cyan;
    }

    public double Magenta
    {
        get => this.MagentaComponent * 100.0;
    }

    public double M
    {
        get => this.Magenta;
    }

    public double Yellow
    {
        get => this.YellowComponent * 100.0;
    }

    public double Y
    {
        get => this.Yellow;
    }

    public double Black
    {
        get => this.BlackComponent * 100.0;
    }

    public double K
    {
        get => this.Black;
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length == 1)
        {
            this.CyanComponent = values[0];
        }
        else if (values.Length == 2)
        {
            this.CyanComponent = values[0];
            this.MagentaComponent = values[1];
        }
        else if (values.Length == 2)
        {
            this.CyanComponent = values[0];
            this.MagentaComponent = values[1];
            this.YellowComponent = values[2];
        }
        else if (values.Length == 3)
        {
            this.CyanComponent = values[0];
            this.MagentaComponent = values[1];
            this.YellowComponent = values[2];
            this.BlackComponent = values[3];
        }
    }

    public int ComponentCount
    {
        get => 4;
    }

    public void ConvertFrom(ColorCode code)
    {
        // https://www.rapidtables.com/convert/color/rgb-to-cmyk.html

        static double Clamp(double v)
        {
            if (v <= 0.0 || double.IsNaN(v) == true)
            {
                v = 0.0;
            }

            return v;
        }

        var r = code.Red / 255.0;
        var g = code.Green / 255.0;
        var b = code.Blue / 255.0;

        var k = Clamp(1.0 - Math.Max(r, Math.Max(g, b)));
        var c = Clamp((1.0 - r - k) / (1.0 - k));
        var m = Clamp((1.0 - g - k) / (1.0 - k));
        var y = Clamp((1.0 - b - k) / (1.0 - k));

        this.CyanComponent = new ColorRange(c);
        this.MagentaComponent = new ColorRange(m);
        this.YellowComponent = new ColorRange(y);
        this.BlackComponent = new ColorRange(k);
    }

    public ColorCode ConvertTo()
    {
        var k = this.BlackComponent;
        var r = (1.0 - this.CyanComponent) * (1.0 - k) * 255.0;
        var g = (1.0 - this.MagentaComponent) * (1.0 - k) * 255.0;
        var b = (1.0 - this.YellowComponent) * (1.0 - k) * 255.0;

        return new ColorCode((byte)r, (byte)g, (byte)b);
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("Cyan", "C", this.Cyan, 100.0, "0.0", "%"),
            new ColorComponent("Magenta", "M", this.Magenta, 100.0, "0.0", "%"),
            new ColorComponent("Yellow", "Y", this.Yellow, 100.0, "0.0", "%"),
            new ColorComponent("Black", "K", this.Black, 100.0, "0.0", "%")
        };
    }
}
