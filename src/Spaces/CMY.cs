/// © Tobias Sachs
/// Colors/CMY.cs
/// 22.01.2023

using System;

namespace tcs.Colors;

public struct CMY : IColorSpace
{
    public CMY(ColorRange c, ColorRange m, ColorRange y)
    {
        CyanComponent = c;
        MagentaComponent = m;
        YellowComponent = y;
    }

    public ColorRange CyanComponent { get; set; }

    public ColorRange MagentaComponent { get; set; }

    public ColorRange YellowComponent { get; set; }

    public byte Cyan
    {
        get => Convert.ToByte(CyanComponent * 255.0);
    }

    public byte C
    {
        get => Cyan;
    }

    public byte Magenta
    {
        get => Convert.ToByte(MagentaComponent * 255.0);
    }

    public byte M
    {
        get => Magenta;
    }

    public byte Yellow
    {
        get => Convert.ToByte(YellowComponent * 255.0);
    }

    public byte Y
    {
        get => Yellow;
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length == 1)
        {
            CyanComponent = values[0];
        }
        else if (values.Length == 2)
        {
            CyanComponent = values[0];
            MagentaComponent = values[1];
        }
        else if (values.Length == 2)
        {
            CyanComponent = values[0];
            MagentaComponent = values[1];
            YellowComponent = values[2];
        }
    }

    public int ComponentCount
    {
        get => 3;
    }

    public void ConvertFrom(ColorCode code)
    {
        CyanComponent = new ColorRange((255 - code.Red) / 255.0);
        MagentaComponent = new ColorRange((255 - code.Green) / 255.0);
        YellowComponent = new ColorRange((255 - code.Blue) / 255.0);
    }

    public ColorCode ConvertTo()
    {
        var r = (byte)(255 - Cyan);
        var g = (byte)(255 - Magenta);
        var b = (byte)(255 - Yellow);

        return new ColorCode(r, g, b);
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("Cyan", "C", Cyan,  byte.MaxValue, "0", ""),
            new ColorComponent("Magenta", "M", Magenta,  byte.MaxValue, "0", ""),
            new ColorComponent("Yellow", "Y", Yellow, byte.MaxValue, "0", "")
        };
    }
}
