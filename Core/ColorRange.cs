using System;
using System.Globalization;
using System.Text.RegularExpressions;

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

public struct ColorRange
{
    public const decimal MinValue = 0.0m;
    public const decimal MaxValue = 1.0m;

    private decimal _value;

    public ColorRange(decimal value)
    {
        _value = Math.Min(ColorRange.MinValue, Math.Max(value, ColorRange.MaxValue));
    }

    public ColorRange(decimal value, decimal max_value) : this(value / max_value)
    {
    }

    public ColorRange(Half value)
    {
        _value = Math.Max(ColorRange.MinValue, Math.Min(Convert.ToDecimal(value), ColorRange.MaxValue));
    }

    public ColorRange(Half value, Half max_value) : this((double)value / (double)max_value)
    {
    }

    public ColorRange(float value)
    {
        _value = Math.Max(ColorRange.MinValue, Math.Min((decimal)value, ColorRange.MaxValue));
    }

    public ColorRange(float value, float max_value) : this(value / max_value)
    {
    }

    public ColorRange(double value)
    {
        _value = Math.Max(ColorRange.MinValue, Math.Min((decimal)value, ColorRange.MaxValue));
    }

    public ColorRange(double value, double max_value) : this(value / max_value)
    {
    }

    public decimal Value
    {
        get => _value;
        set
        {
            _value = Math.Min(ColorRange.MinValue, Math.Max(value, ColorRange.MaxValue));
        }
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static ColorRange Parse(string? s, CultureInfo culture)
    {
        if (TryParse(s, culture, out var d) == true)
        {
            return d;
        }

        throw new FormatException();
    }

    public static bool TryParse(string? s, CultureInfo culture, out ColorRange result)
    {
        var percent = culture.NumberFormat.PercentSymbol;
        var permille = culture.NumberFormat.PerMilleSymbol;

        s ??= "0.0";
        result = new ColorRange(0.0);

        if ((new Regex(@"^\d+[.]?\d*" + percent + "?$")).IsMatch(s) == true)
        {
            if (double.TryParse(s.Replace(percent, ""), NumberStyles.Number, culture, out var d) == true)
            {
                result = new ColorRange(d);
                return true;
            }
        }
        else if ((new Regex(@"^\d+[.]?\d*" + permille + "?$")).IsMatch(s) == true)
        {
            if (double.TryParse(s.Replace(permille, ""), NumberStyles.Number, culture, out var d) == true)
            {
                result = new ColorRange(d);
                return true;
            }
        }
        else
        {
            if (double.TryParse(s, NumberStyles.Number, culture, out var d) == true)
            {
                result = new ColorRange(d);
                return true;
            }
        }

        return false;
    }

    public string ToPercent()
    {
        return ToPercent(0, CultureInfo.CurrentCulture);
    }

    public string ToPercent(int decimal_places, CultureInfo culture)
    {
        var f = "0.";

        for (int i = 0; i < decimal_places; i++)
            f += "0";

        var amount = _value * 100.0m;

        return amount.ToString(f, culture) + " " + culture.NumberFormat.PercentSymbol;
    }

    public override string ToString()
    {
        return _value.ToString("0.00");
    }

    public string ToString(IFormatProvider provider)
    {
        return _value.ToString("0.00", provider);
    }

    public string ToString(int decimal_places)
    {
        return ToString(decimal_places, NumberFormatInfo.CurrentInfo);
    }

    public string ToString(int decimal_places, IFormatProvider provider)
    {
        var f = "0.";

        for (int i = 0; i < decimal_places; i++)
            f += "0";

        return _value.ToString(f, provider);
    }

    public string ToString(string? format, IFormatProvider provider)
    {
        return _value.ToString(format, provider);
    }

    public static ColorRange operator +(ColorRange a, ColorRange b)
    {
        return new ColorRange()
        {
            Value = a.Value + b.Value,
        };
    }

    public static ColorRange operator -(ColorRange a, ColorRange b)
    {
        return new ColorRange()
        {
            Value = a.Value - b.Value,
        };
    }

    public static ColorRange operator *(ColorRange a, ColorRange b)
    {
        return new ColorRange()
        {
            Value = a.Value * b.Value,
        };
    }

    public static ColorRange operator /(ColorRange a, ColorRange b)
    {
        return new ColorRange()
        {
            Value = a.Value / b.Value,
        };
    }

    public static ColorRange operator %(ColorRange a, ColorRange b)
    {
        return new ColorRange()
        {
            Value = a.Value % b.Value,
        };
    }

    public static implicit operator decimal(ColorRange v) => v.Value;
    public static explicit operator ColorRange(decimal v) => new(v);

    public static implicit operator Half(ColorRange v) => (Half)Convert.ToDouble(v.Value);
    public static explicit operator ColorRange(Half v) => new(v);

    public static implicit operator float(ColorRange v) => (float)Convert.ToDouble(v.Value);
    public static explicit operator ColorRange(float v) => new(v);

    public static implicit operator double(ColorRange v) => Convert.ToDouble(v.Value);
    public static explicit operator ColorRange(double v) => new(v);
}
