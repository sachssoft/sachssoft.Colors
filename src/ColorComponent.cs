/// © Tobias Sachs
/// Colors/ColorComponent.cs
/// 15.03.2024

using System.Globalization;

namespace tcs.Colors;

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

    /// <summary>
    /// 
    /// </summary>
    public string Name
    {
        get => _name;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Abbreviation
    {
        get => _abbreviation;
    }

    /// <summary>
    /// 
    /// </summary>
    public double Value
    {
        get => _value;
    }

    /// <summary>
    /// 
    /// </summary>
    public string ValueFormat
    {
        get => _value_format;
    }

    /// <summary>
    /// 
    /// </summary>
    public ColorRange Level
    {
        get => _level;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Unit
    {
        get => _unit;
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public string ToString(ColorComponentFormats format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{Level.ToString(2)} [{Value} {Unit}]";
    }
}
