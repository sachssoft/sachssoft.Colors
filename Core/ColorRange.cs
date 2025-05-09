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
    public const float MinValue = 0.0f;
    public const float MaxValue = 1.0f;

    private float _value;

    public ColorRange(float value)
    {
        _value = MathF.Max(MinValue, MathF.Min(value, MaxValue));
    }

    public ColorRange(float value, float max_value) : this(value / max_value) { }

    public ColorRange(Half value)
    {
        _value = MathF.Max(MinValue, MathF.Min((float)value, MaxValue));
    }

    public ColorRange(Half value, Half max_value) : this((float)value / (float)max_value) { }

    public ColorRange(double value)
    {
        _value = MathF.Max(MinValue, MathF.Min((float)value, MaxValue));
    }

    public ColorRange(double value, double max_value) : this((float)value / (float)max_value) { }

    public bool IsBelowMin => _value <= MinValue;
    public bool IsAboveMax => _value >= MaxValue;

    public float Value
    {
        get => _value;
        set => _value = MathF.Max(MinValue, MathF.Min(value, MaxValue));
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            ColorRange other => _value == other._value,
            float f => _value == f,
            double d => _value == (float)d,
            decimal m => _value == (float)m,
            _ => false
        };
    }

    public override int GetHashCode() => _value.GetHashCode();

    public static ColorRange Parse(string? s, CultureInfo culture)
    {
        if (TryParse(s, culture, out var d))
            return d;

        throw new FormatException();
    }

    public static bool TryParse(string? s, CultureInfo culture, out ColorRange result)
    {
        var percent = culture.NumberFormat.PercentSymbol;
        var permille = culture.NumberFormat.PerMilleSymbol;

        s ??= "0.0";
        result = new ColorRange(0.0f);

        if (Regex.IsMatch(s, @"^\d+[.]?\d*" + Regex.Escape(percent) + "?$"))
        {
            if (float.TryParse(s.Replace(percent, ""), NumberStyles.Number, culture, out var f))
            {
                result = new ColorRange(f / 100f);
                return true;
            }
        }
        else if (Regex.IsMatch(s, @"^\d+[.]?\d*" + Regex.Escape(permille) + "?$"))
        {
            if (float.TryParse(s.Replace(permille, ""), NumberStyles.Number, culture, out var f))
            {
                result = new ColorRange(f / 1000f);
                return true;
            }
        }
        else
        {
            if (float.TryParse(s, NumberStyles.Number, culture, out var f))
            {
                result = new ColorRange(f);
                return true;
            }
        }

        return false;
    }

    public string ToPercent() => ToPercent(0, CultureInfo.CurrentCulture);

    public string ToPercent(int decimal_places, CultureInfo culture)
    {
        var format = "0." + new string('0', decimal_places);
        var amount = _value * 100f;
        return amount.ToString(format, culture) + " " + culture.NumberFormat.PercentSymbol;
    }

    public override string ToString() => _value.ToString("0.00");

    public string ToString(IFormatProvider provider) => _value.ToString("0.00", provider);

    public string ToString(int decimal_places) => ToString(decimal_places, NumberFormatInfo.CurrentInfo);

    public string ToString(int decimal_places, IFormatProvider provider)
    {
        var format = "0." + new string('0', decimal_places);
        return _value.ToString(format, provider);
    }

    public string ToString(string? format, IFormatProvider provider) => _value.ToString(format, provider);

    // --- ColorRange mit ColorRange ---
    public static bool operator ==(ColorRange a, ColorRange b) => a._value == b._value;
    public static bool operator !=(ColorRange a, ColorRange b) => a._value != b._value;
    public static bool operator <(ColorRange a, ColorRange b) => a._value < b._value;
    public static bool operator >(ColorRange a, ColorRange b) => a._value > b._value;
    public static bool operator <=(ColorRange a, ColorRange b) => a._value <= b._value;
    public static bool operator >=(ColorRange a, ColorRange b) => a._value >= b._value;

    // --- ColorRange mit float ---
    public static bool operator ==(ColorRange a, float b) => a._value == b;
    public static bool operator !=(ColorRange a, float b) => a._value != b;
    public static bool operator <(ColorRange a, float b) => a._value < b;
    public static bool operator >(ColorRange a, float b) => a._value > b;
    public static bool operator <=(ColorRange a, float b) => a._value <= b;
    public static bool operator >=(ColorRange a, float b) => a._value >= b;

    public static bool operator ==(float a, ColorRange b) => a == b._value;
    public static bool operator !=(float a, ColorRange b) => a != b._value;
    public static bool operator <(float a, ColorRange b) => a < b._value;
    public static bool operator >(float a, ColorRange b) => a > b._value;
    public static bool operator <=(float a, ColorRange b) => a <= b._value;
    public static bool operator >=(float a, ColorRange b) => a >= b._value;

    // --- ColorRange mit double ---
    public static bool operator ==(ColorRange a, double b) => a._value == (float)b;
    public static bool operator !=(ColorRange a, double b) => a._value != (float)b;
    public static bool operator <(ColorRange a, double b) => a._value < (float)b;
    public static bool operator >(ColorRange a, double b) => a._value > (float)b;
    public static bool operator <=(ColorRange a, double b) => a._value <= (float)b;
    public static bool operator >=(ColorRange a, double b) => a._value >= (float)b;

    public static bool operator ==(double a, ColorRange b) => a == (double)b._value;
    public static bool operator !=(double a, ColorRange b) => a != (double)b._value;
    public static bool operator <(double a, ColorRange b) => a < (double)b._value;
    public static bool operator >(double a, ColorRange b) => a > (double)b._value;
    public static bool operator <=(double a, ColorRange b) => a <= (double)b._value;
    public static bool operator >=(double a, ColorRange b) => a >= (double)b._value;

    // --- ColorRange mit decimal ---
    public static bool operator ==(ColorRange a, decimal b) => (decimal)a._value == b;
    public static bool operator !=(ColorRange a, decimal b) => (decimal)a._value != b;
    public static bool operator <(ColorRange a, decimal b) => (decimal)a._value < b;
    public static bool operator >(ColorRange a, decimal b) => (decimal)a._value > b;
    public static bool operator <=(ColorRange a, decimal b) => (decimal)a._value <= b;
    public static bool operator >=(ColorRange a, decimal b) => (decimal)a._value >= b;

    public static bool operator ==(decimal a, ColorRange b) => a == (decimal)b._value;
    public static bool operator !=(decimal a, ColorRange b) => a != (decimal)b._value;
    public static bool operator <(decimal a, ColorRange b) => a < (decimal)b._value;
    public static bool operator >(decimal a, ColorRange b) => a > (decimal)b._value;
    public static bool operator <=(decimal a, ColorRange b) => a <= (decimal)b._value;
    public static bool operator >=(decimal a, ColorRange b) => a >= (decimal)b._value;

    // float
    public static implicit operator float(ColorRange v) => v._value;
    public static explicit operator ColorRange(float v) => new(v);

    // Half
    public static implicit operator Half(ColorRange v) => (Half)v._value;
    public static explicit operator ColorRange(Half v) => new((float)v);

    // double
    public static implicit operator double(ColorRange v) => v._value;
    public static explicit operator ColorRange(double v) => new((float)v);

    // decimal
    public static implicit operator decimal(ColorRange v) => (decimal)v._value;
    public static explicit operator ColorRange(decimal v) => new((float)v);

}
