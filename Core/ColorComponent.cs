using System;
using System.Globalization;

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

public readonly struct ColorComponent
{
    private readonly string _name;
    private readonly string _abbreviation;
    private readonly double _value;
    private readonly string _value_format;
    private readonly ColorRange _level;
    private readonly string _unit;

    public ColorComponent(string name, string abbreviation, double value, double max_value, string value_format, string unit)
    {
        _name = name;
        _abbreviation = abbreviation;
        _value = value;
        _value_format = value_format;
        _level = new ColorRange(value, max_value);
        _unit = unit;
    }

    public string Name
    {
        get => _name;
    }

    public string Abbreviation
    {
        get => _abbreviation;
    }

    public double Value
    {
        get => _value;
    }

    public string ValueFormat
    {
        get => _value_format;
    }

    /// </summary>
    public ColorRange Level
    {
        get => _level;
    }

    public string Unit
    {
        get => _unit;
    }

    public string ToString(ColorComponentFormats format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    public string ToString(ColorComponentFormats format, CultureInfo culture)
    {
        switch (format)
        {
            case ColorComponentFormats.Level:
                return Level.ToString(2, culture);
            case ColorComponentFormats.Percent:
                return Level.ToPercent(0, culture);
            default:
                return Value.ToString(ValueFormat, culture) + " " + Unit;
        }
    }

    public override string ToString()
    {
        return $"{Level.ToString(2)} [{Value} {Unit}]";
    }
}
