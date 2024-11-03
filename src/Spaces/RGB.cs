/// © Tobias Sachs
/// Colors/RGB.cs
/// 22.01.2023

using System;

namespace tcs.Colors;

public struct RGB : IColorSpace
{
    public RGB(ColorRange r, ColorRange g, ColorRange b)
    {
        this.RedComponent = r;
        this.GreenComponent = g;
        this.BlueComponent = b;
    }

    public RGB(ColorCode code) : this()
    {
        this.ConvertFrom(code);
    }

    public ColorRange RedComponent { get; set; }

    public ColorRange GreenComponent { get; set; }

    public ColorRange BlueComponent { get; set; }

    public byte Red
    {
        get => Convert.ToByte(this.RedComponent * 255.0);
    }

    public byte R
    {
        get => this.Red;
    }

    public byte Green
    {
        get => Convert.ToByte(this.GreenComponent * 255.0);
    }

    public byte G
    {
        get => this.Green;
    }

    public byte Blue
    {
        get => Convert.ToByte(this.BlueComponent * 255.0);
    }

    public byte B
    {
        get => this.Blue;
    }

    public int ComponentCount
    {
        get => 3;
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length == 1)
        {
            this.RedComponent = values[0];
        }
        else if (values.Length == 2)
        {
            this.RedComponent = values[0];
            this.GreenComponent = values[1];
        }
        else if (values.Length == 2)
        {
            this.RedComponent = values[0];
            this.GreenComponent = values[1];
            this.BlueComponent = values[2];
        }
    }

    public void ConvertFrom(ColorCode code)
    {
        this.RedComponent = new ColorRange(code.Red / 255.0);
        this.GreenComponent = new ColorRange(code.Green / 255.0);
        this.BlueComponent = new ColorRange(code.Blue / 255.0);
    }

    public ColorCode ConvertTo()
    {
        return new ColorCode(this.Red, this.Green, this.Blue);
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("Red", "R", this.Red,  byte.MaxValue, "0", ""),
            new ColorComponent("Green", "G", this.Green,  byte.MaxValue, "0", ""),
            new ColorComponent("Blue", "B", this.Blue, byte.MaxValue, "0", "")
        };
    }
}
