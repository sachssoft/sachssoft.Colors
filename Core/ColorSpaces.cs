using System;

#if SASOGINE
using Microsoft.Xna.Framework;
namespace sachssoft.Sasogine.Graphics.Colors.Spaces;
#elif MONOGAME
using Microsoft.Xna.Framework;
namespace sachssoft.Monogame.Colors.Spaces;
#elif AVALONIA
using Ava = Avalonia;
namespace sachssoft.Avalonia.Colors.Spaces;
using Color = Ava.Media.Color;
#elif DRAWING
namespace sachssoft.Drawing.Colors.Spaces;
using Color = System.Drawing.Color;
#elif WPF
namespace sachssoft.WPF.Colors.Spaces;
using Color = System.Windows.Media.Color;
#elif SKIA
namespace sachssoft.Skia.Colors.Spaces;
using Color = SkiaSharp.SKColor;
#else
namespace sachssoft.Colors.Spaces;
using Color = sachssoft.Colors.ColorCode;
#endif

public struct ACEScg : IColorSpace
{
    public ColorRange RedComponent { get; set; }
    public ColorRange GreenComponent { get; set; }
    public ColorRange BlueComponent { get; set; }

    public ACEScg(ColorRange r, ColorRange g, ColorRange b)
    {
        RedComponent = r;
        GreenComponent = g;
        BlueComponent = b;
    }

    public double R => RedComponent * 100.0;
    public double G => GreenComponent * 100.0;
    public double B => BlueComponent * 100.0;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] { RedComponent, GreenComponent, BlueComponent };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length > 0) RedComponent = values[0];
        if (values.Length > 1) GreenComponent = values[1];
        if (values.Length > 2) BlueComponent = values[2];
    }

    public int ComponentCount => 3;

    public void ConvertFrom(Color color)
    {
        // Convert sRGB to linear double RGB (0.0 to 1.0)
        var channels = ColorUtils.AdaptToDoubleFrom(color);

        // Assume input is in sRGB space; convert to ACEScg using matrix
        // sRGB D65 -> ACEScg D60 (approximation)
        double r_aces = 1.45143932 * channels[0] - 0.23651075 * channels[1] - 0.21492857 * channels[2];
        double g_aces = -0.07655377 * channels[0] + 1.1762297 * channels[1] - 0.09967593 * channels[2];
        double b_aces = 0.00831615 * channels[0] - 0.00603245 * channels[1] + 0.9977163 * channels[2];

        RedComponent = new ColorRange(r_aces);
        GreenComponent = new ColorRange(g_aces);
        BlueComponent = new ColorRange(b_aces);
    }

    public Color ConvertTo()
    {
        double r = RedComponent;
        double g = GreenComponent;
        double b = BlueComponent;

        // ACEScg -> sRGB (approximate inverse matrix)
        double r_srgb = 0.695452 * r + 0.140678 * g + 0.163869 * b;
        double g_srgb = 0.044794 * r + 0.859671 * g + 0.095535 * b;
        double b_srgb = -0.005525 * r + 0.004025 * g + 1.0015 * b;

        return ColorUtils.AdaptFromDoubleTo(r_srgb, g_srgb, b_srgb);
    }

    public ColorComponent[] GetComponents()
    {
        return new[]
        {
            new ColorComponent("RedComponent", "R", R, 100.0, "0.0", "%"),
            new ColorComponent("GreenComponent", "G", G, 100.0, "0.0", "%"),
            new ColorComponent("BlueComponent", "B", B, 100.0, "0.0", "%")
        };
    }
}

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

    public byte Cyan => Convert.ToByte(CyanComponent * 255.0);

    public byte C => Cyan;

    public byte Magenta => Convert.ToByte(MagentaComponent * 255.0);

    public byte M => Magenta;

    public byte Yellow => Convert.ToByte(YellowComponent * 255.0);

    public byte Y => Yellow;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] { CyanComponent, MagentaComponent, YellowComponent };
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
        else if (values.Length >= 3)
        {
            CyanComponent = values[0];
            MagentaComponent = values[1];
            YellowComponent = values[2];
        }
    }

    public int ComponentCount => 3;

    public void ConvertFrom(Color color)
    {
        var channels = ColorUtils.AdaptFrom(color);

        CyanComponent = new ColorRange((255 - channels[0]) / 255.0);
        MagentaComponent = new ColorRange((255 - channels[1]) / 255.0);
        YellowComponent = new ColorRange((255 - channels[2]) / 255.0);
    }

    public Color ConvertTo()
    {
        var r = (byte)(255 - Cyan);
        var g = (byte)(255 - Magenta);
        var b = (byte)(255 - Yellow);

        return ColorUtils.AdaptTo(r, g, b, 255);
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("Cyan", "C", Cyan, 255, "0", ""),
            new ColorComponent("Magenta", "M", Magenta, 255, "0", ""),
            new ColorComponent("Yellow", "Y", Yellow, 255, "0", "")
        };
    }

    public override string ToString()
    {
        return $"CMY({CyanComponent}, {MagentaComponent}, {YellowComponent})";
    }
}

public struct CMYK : IColorSpace
{
    public CMYK(ColorRange c, ColorRange m, ColorRange y, ColorRange k)
    {
        CyanComponent = c;
        MagentaComponent = m;
        YellowComponent = y;
        BlackComponent = k;
    }

    public ColorRange CyanComponent { get; set; }
    public ColorRange MagentaComponent { get; set; }
    public ColorRange YellowComponent { get; set; }
    public ColorRange BlackComponent { get; set; }

    public double Cyan => CyanComponent * 100.0;
    public double C => Cyan;

    public double Magenta => MagentaComponent * 100.0;
    public double M => Magenta;

    public double Yellow => YellowComponent * 100.0;
    public double Y => Yellow;

    public double Black => BlackComponent * 100.0;
    public double K => Black;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] { CyanComponent, MagentaComponent, YellowComponent, BlackComponent };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length >= 1) CyanComponent = values[0];
        if (values.Length >= 2) MagentaComponent = values[1];
        if (values.Length >= 3) YellowComponent = values[2];
        if (values.Length >= 4) BlackComponent = values[3];
    }

    public int ComponentCount => 4;

    public void ConvertFrom(Color color)
    {
        var channels = ColorUtils.AdaptFrom(color);

        static double Clamp(double v) => v <= 0.0 || double.IsNaN(v) ? 0.0 : v;

        var r = channels[0] / 255.0;
        var g = channels[1] / 255.0;
        var b = channels[2] / 255.0;

        var k = Clamp(1.0 - Math.Max(r, Math.Max(g, b)));
        var c = Clamp((1.0 - r - k) / (1.0 - k));
        var m = Clamp((1.0 - g - k) / (1.0 - k));
        var y = Clamp((1.0 - b - k) / (1.0 - k));

        CyanComponent = new ColorRange(c);
        MagentaComponent = new ColorRange(m);
        YellowComponent = new ColorRange(y);
        BlackComponent = new ColorRange(k);
    }

    public Color ConvertTo()
    {
        var k = BlackComponent;
        var r = (1.0 - CyanComponent) * (1.0 - k) * 255.0;
        var g = (1.0 - MagentaComponent) * (1.0 - k) * 255.0;
        var b = (1.0 - YellowComponent) * (1.0 - k) * 255.0;

        return ColorUtils.AdaptTo((byte)r, (byte)g, (byte)b, 255);
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("Cyan", "C", Cyan, 100.0, "0.0", "%"),
            new ColorComponent("Magenta", "M", Magenta, 100.0, "0.0", "%"),
            new ColorComponent("Yellow", "Y", Yellow, 100.0, "0.0", "%"),
            new ColorComponent("Black", "K", Black, 100.0, "0.0", "%")
        };
    }

    public override string ToString()
    {
        return $"CMYK({C:0.##}%, {M:0.##}%, {Y:0.##}%, {K:0.##}%)";
    }
}

public struct CSP
{
    public double C { get; set; }
    public double S { get; set; }
    public double P { get; set; }

    public CSP(double C, double S, double P)
    {
        this.C = C;
        this.S = S;
        this.P = P;
    }

    // Umrechnung von RGB in CSP (dies ist eine Beispielumrechnung, die für andere Umrechnungen angepasst werden kann)
    public static CSP FromRGB(double R, double G, double B)
    {
        double C = Math.Sqrt(R * G);
        double S = Math.Abs(R - G);
        double P = (R + G + B) / 3.0;

        return new CSP(C, S, P);
    }

    // Umrechnung von CSP in RGB (dies ist eine Beispielumrechnung, die für andere Umrechnungen angepasst werden kann)
    public static (double R, double G, double B) ToRGB(CSP csp)
    {
        double R = csp.C * 1.0;
        double G = csp.S * 1.0;
        double B = csp.P * 1.0;

        return (R, G, B);
    }
}

public struct HCL : IColorSpace
{
    public ColorRange HueComponent { get; set; }
    public ColorRange ChromaComponent { get; set; }
    public ColorRange LuminanceComponent { get; set; }

    public HCL(ColorRange h, ColorRange c, ColorRange l)
    {
        HueComponent = h;
        ChromaComponent = c;
        LuminanceComponent = l;
    }

    public double H => HueComponent * 360.0;
    public double C => ChromaComponent * 100.0;
    public double L => LuminanceComponent * 100.0;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            HueComponent,
            ChromaComponent,
            LuminanceComponent
        };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length > 0) HueComponent = values[0];
        if (values.Length > 1) ChromaComponent = values[1];
        if (values.Length > 2) LuminanceComponent = values[2];
    }

    public int ComponentCount => 3;

    public void ConvertFrom(Color color)
    {
        // Konvertiere RGB zu CIELUV und dann zu HCL
        // Dies erfordert komplexe Berechnungen und ist hier nicht vollständig implementiert
        throw new NotImplementedException("Konvertierung von RGB zu HCL ist nicht implementiert.");
    }

    public Color ConvertTo()
    {
        // Konvertiere HCL zu CIELUV und dann zu RGB
        // Dies erfordert komplexe Berechnungen und ist hier nicht vollständig implementiert
        throw new NotImplementedException("Konvertierung von HCL zu RGB ist nicht implementiert.");
    }

    public ColorComponent[] GetComponents()
    {
        return new[]
        {
            new ColorComponent("Hue", "H", H, 360.0, "0.0", "°"),
            new ColorComponent("Chroma", "C", C, 100.0, "0.0", "%"),
            new ColorComponent("LuminanceComponent", "L", L, 100.0, "0.0", "%")
        };
    }
}

public struct HSI : IColorSpace
{
    public ColorRange HueComponent { get; set; }
    public ColorRange SaturationComponent { get; set; }
    public ColorRange IntensityComponent { get; set; }

    public HSI(ColorRange h, ColorRange s, ColorRange i)
    {
        HueComponent = h;
        SaturationComponent = s;
        IntensityComponent = i;
    }

    public double H => HueComponent * 360.0;
    public double S => SaturationComponent * 100.0;
    public double I => IntensityComponent * 255.0;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            HueComponent,
            SaturationComponent,
            IntensityComponent
        };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length > 0) HueComponent = values[0];
        if (values.Length > 1) SaturationComponent = values[1];
        if (values.Length > 2) IntensityComponent = values[2];
    }

    public int ComponentCount => 3;

    public void ConvertFrom(Color color)
    {
        // Convert sRGB to linear double RGB (0.0 to 1.0)
        var channels = ColorUtils.AdaptToDoubleFrom(color);
        var r = channels[0];
        var g = channels[1];
        var b = channels[2];

        double min = Math.Min(r, Math.Min(g, b));
        double sum = r + g + b;
        double i = sum / 3.0;

        double s = 1.0 - (min / i);
        double h = 0.0;

        if (s != 0)
        {
            double num = 0.5 * ((r - g) + (r - b));
            double den = Math.Sqrt((r - g) * (r - g) + (r - b) * (g - b));
            h = Math.Acos(num / den);
            if (b > g)
                h = 2 * Math.PI - h;
            h = h / (2 * Math.PI);
        }

        HueComponent = new ColorRange(h);
        SaturationComponent = new ColorRange(s);
        IntensityComponent = new ColorRange(i);
    }

    public Color ConvertTo()
    {
        double h = HueComponent;
        double s = SaturationComponent;
        double i = IntensityComponent;

        double r, g, b;

        double z = 1 - Math.Abs((h * 6) % 2 - 1);
        double c = (3 * i * s) / (1 + z);
        double x = c * z;
        double m = i * (1 - s);

        if (h < 1.0 / 6.0)
        {
            r = c; g = x; b = 0;
        }
        else if (h < 2.0 / 6.0)
        {
            r = x; g = c; b = 0;
        }
        else if (h < 3.0 / 6.0)
        {
            r = 0; g = c; b = x;
        }
        else if (h < 4.0 / 6.0)
        {
            r = 0; g = x; b = c;
        }
        else if (h < 5.0 / 6.0)
        {
            r = x; g = 0; b = c;
        }
        else
        {
            r = c; g = 0; b = x;
        }

        r += m;
        g += m;
        b += m;

        return ColorUtils.AdaptFromDoubleTo(r, g, b);
    }

    public ColorComponent[] GetComponents()
    {
        return new[]
        {
            new ColorComponent("Hue", "H", H, 360.0, "0.0", "°"),
            new ColorComponent("Saturation", "S", S, 100.0, "0.0", "%"),
            new ColorComponent("Intensity", "I", I, 255.0, "0.0", "")
        };
    }
}

public struct HSL : IColorSpace
{
    public HSL(ColorRange h, ColorRange s, ColorRange l)
    {
        HueComponent = h;
        SaturationComponent = s;
        LightnessComponent = l;
    }

    public ColorRange HueComponent { get; set; }
    public ColorRange SaturationComponent { get; set; }
    public ColorRange LightnessComponent { get; set; }

    public double Hue => HueComponent * 360.0;
    public double H => Hue;

    public double Saturation => SaturationComponent * 100.0;
    public double S => Saturation;

    public double Lightness => LightnessComponent * 100.0;
    public double L => Lightness;

    public int ComponentCount => 3;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            HueComponent,
            SaturationComponent,
            LightnessComponent
        };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length == 1)
        {
            HueComponent = values[0];
        }
        else if (values.Length == 2)
        {
            HueComponent = values[0];
            SaturationComponent = values[1];
        }
        else if (values.Length == 3)
        {
            HueComponent = values[0];
            SaturationComponent = values[1];
            LightnessComponent = values[2];
        }
        else
        {
            throw new ArgumentException("SetValues requires 1 to 3 values.");
        }
    }

    public void ConvertFrom(Color color)
    {
        var channels = ColorUtils.AdaptFrom(color);
        var r = channels[0] / 255.0;
        var g = channels[1] / 255.0;
        var b = channels[2] / 255.0;

        var cmax = Math.Max(r, Math.Max(g, b));
        var cmin = Math.Min(r, Math.Min(g, b));
        var delta = cmax - cmin;

        double h = 0.0;
        if (delta < 0.0001)
        {
            h = 0.0;
        }
        else if (cmax == r)
        {
            h = 60.0 * (((g - b) / delta) % 6);
        }
        else if (cmax == g)
        {
            h = 60.0 * (((b - r) / delta) + 2);
        }
        else if (cmax == b)
        {
            h = 60.0 * (((r - g) / delta) + 4);
        }

        h = h / 360.0;
        if (h < 0.0)
            h += 1.0;

        double l = (cmax + cmin) / 2.0;
        double s = (delta == 0.0) ? 0.0 : delta / (1.0 - Math.Abs(2.0 * l - 1.0));

        HueComponent = new ColorRange(h);
        SaturationComponent = new ColorRange(s);
        LightnessComponent = new ColorRange(l);
    }

    public Color ConvertTo()
    {
        var h = (double)HueComponent * 360.0;
        var s = (double)SaturationComponent;
        var l = (double)LightnessComponent;

        double c = (1.0 - Math.Abs(2.0 * l - 1.0)) * s;
        double x = c * (1.0 - Math.Abs((h / 60.0) % 2.0 - 1.0));
        double m = l - c / 2.0;

        double r1 = 0, g1 = 0, b1 = 0;

        if (h < 60)
        {
            r1 = c; g1 = x; b1 = 0;
        }
        else if (h < 120)
        {
            r1 = x; g1 = c; b1 = 0;
        }
        else if (h < 180)
        {
            r1 = 0; g1 = c; b1 = x;
        }
        else if (h < 240)
        {
            r1 = 0; g1 = x; b1 = c;
        }
        else if (h < 300)
        {
            r1 = x; g1 = 0; b1 = c;
        }
        else
        {
            r1 = c; g1 = 0; b1 = x;
        }

        byte r = (byte)Math.Round((r1 + m) * 255);
        byte g = (byte)Math.Round((g1 + m) * 255);
        byte b = (byte)Math.Round((b1 + m) * 255);

        return ColorUtils.AdaptTo(r, g, b);
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("Hue", "H", Hue, 360.0, "°", ""),
            new ColorComponent("Saturation", "S", Saturation, 100.0, "%", ""),
            new ColorComponent("Lightness", "L", Lightness, 100.0, "%", "")
        };
    }

    public override string ToString()
    {
        return $"HSL({Hue:F1}°, {Saturation:F1}%, {Lightness:F1}%)";
    }
}

public struct HSV : IColorSpace
{
    public HSV(ColorRange h, ColorRange s, ColorRange v)
    {
        this.HueComponent = h;
        this.SaturationComponent = s;
        this.ValueComponent = v;
    }

    public HSV(Color color) : this()
    {
        this.ConvertFrom(color);
    }

    public ColorRange HueComponent { get; set; }
    public ColorRange SaturationComponent { get; set; }
    public ColorRange ValueComponent { get; set; }

    public double Hue => this.HueComponent * 360.0;
    public double H => this.Hue;

    public double Saturation => this.SaturationComponent * 100.0;
    public double S => this.Saturation;

    public double Value => this.ValueComponent * 100.0;
    public double Brightness => this.Value;
    public double V => this.Value;

    public int ComponentCount => 3;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            HueComponent,
            SaturationComponent,
            ValueComponent
        };
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
        else if (values.Length == 3)
        {
            this.HueComponent = values[0];
            this.SaturationComponent = values[1];
            this.ValueComponent = values[2];
        }
        else
        {
            throw new ArgumentException("HSV requires 1 to 3 components.");
        }
    }

    public void ConvertFrom(Color color)
    {
        var channels = ColorUtils.AdaptFrom(color);

        double r = channels[0] / 255.0;
        double g = channels[1] / 255.0;
        double b = channels[2] / 255.0;

        double cmax = Math.Max(r, Math.Max(g, b));
        double cmin = Math.Min(r, Math.Min(g, b));
        double delta = cmax - cmin;

        double h;
        if (delta < 0.0001)
        {
            h = 0.0;
        }
        else if (cmax == r)
        {
            h = 60.0 * (((g - b) / delta) % 6.0);
        }
        else if (cmax == g)
        {
            h = 60.0 * (((b - r) / delta) + 2.0);
        }
        else // cmax == b
        {
            h = 60.0 * (((r - g) / delta) + 4.0);
        }

        // Normalize hue to [0, 1)
        h = ((h % 360.0) + 360.0) % 360.0 / 360.0;

        double s = (cmax == 0.0) ? 0.0 : (delta / cmax);
        double v = cmax;

        this.HueComponent = new ColorRange(h);
        this.SaturationComponent = new ColorRange(s);
        this.ValueComponent = new ColorRange(v);
    }

    public Color ConvertTo()
    {
        double h = (double)this.HueComponent * 360.0;
        double s = (double)this.SaturationComponent;
        double v = (double)this.ValueComponent;

        double c = v * s;
        double x = c * (1.0 - Math.Abs((h / 60.0) % 2.0 - 1.0));
        double m = v - c;

        double r1 = 0.0, g1 = 0.0, b1 = 0.0;

        if (h < 60.0)
        {
            r1 = c; g1 = x; b1 = 0.0;
        }
        else if (h < 120.0)
        {
            r1 = x; g1 = c; b1 = 0.0;
        }
        else if (h < 180.0)
        {
            r1 = 0.0; g1 = c; b1 = x;
        }
        else if (h < 240.0)
        {
            r1 = 0.0; g1 = x; b1 = c;
        }
        else if (h < 300.0)
        {
            r1 = x; g1 = 0.0; b1 = c;
        }
        else // h < 360.0
        {
            r1 = c; g1 = 0.0; b1 = x;
        }

        return ColorUtils.AdaptTo(
            Convert.ToByte((r1 + m) * 255.0),
            Convert.ToByte((g1 + m) * 255.0),
            Convert.ToByte((b1 + m) * 255.0),
            255);
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

    public override string ToString()
    {
        return $"HSV({Hue:F1}°, {Saturation:F1}%, {Value:F1}%)";
    }
}

public struct HWB : IColorSpace
{
    public HWB(ColorRange h, ColorRange w, ColorRange b)
    {
        this.HueComponent = h;
        this.WhiteComponent = w;
        this.BlackComponent = b;
    }

    public ColorRange HueComponent { get; set; }     // 0–1 (mapped to 0–360°)
    public ColorRange WhiteComponent { get; set; }   // 0–1
    public ColorRange BlackComponent { get; set; }   // 0–1

    public double Hue => this.HueComponent * 360.0;
    public double Whiteness => this.WhiteComponent * 100.0;
    public double Blackness => this.BlackComponent * 100.0;

    public double H => this.Hue;
    public double W => this.Whiteness;
    public double B => this.Blackness;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            HueComponent,
            WhiteComponent,
            BlackComponent
        };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length > 0) this.HueComponent = values[0];
        if (values.Length > 1) this.WhiteComponent = values[1];
        if (values.Length > 2) this.BlackComponent = values[2];
    }

    public int ComponentCount => 3;

    public void ConvertFrom(Color color)
    {
        // RGB → HSV → HWB
        var hsv = new HSV();
        hsv.ConvertFrom(color);

        var h = hsv.HueComponent;
        var v = hsv.ValueComponent;
        var s = hsv.SaturationComponent;

        var w = (1.0 - s) * v;
        var b = 1.0 - v;

        this.HueComponent = h;
        this.WhiteComponent = new ColorRange(w);
        this.BlackComponent = new ColorRange(b);
    }

    public Color ConvertTo()
    {
        double h = this.Hue;               // 0–360
        double w = this.WhiteComponent;    // 0–1
        double b = this.BlackComponent;    // 0–1
        double sum = w + b;

        if (sum >= 1.0)
        {
            double gray = w / sum;
            byte grayByte = (byte)(gray * 255.0);
            return ColorUtils.AdaptTo(grayByte, grayByte, grayByte, 255);
        }

        double v = 1.0 - b;
        double s = (v == 0.0) ? 0.0 : 1.0 - (w / v);

        // HSV → RGB
        var hsv = new HSV(new ColorRange(h / 360.0), new ColorRange(s), new ColorRange(v));
        return hsv.ConvertTo();
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("Hue", "H", this.Hue, 360.0, "0.0", "°"),
            new ColorComponent("Whiteness", "W", this.Whiteness, 100.0, "0.0", "%"),
            new ColorComponent("Blackness", "B", this.Blackness, 100.0, "0.0", "%")
        };
    }
}

public struct ICtCp : IColorSpace
{
    public ColorRange IComponent { get; set; }
    public ColorRange CtComponent { get; set; }
    public ColorRange CpComponent { get; set; }

    public ICtCp(ColorRange i, ColorRange ct, ColorRange cp)
    {
        IComponent = i;
        CtComponent = ct;
        CpComponent = cp;
    }

    public double I => IComponent * 100.0;
    public double Ct => CtComponent * 100.0;
    public double Cp => CpComponent * 100.0;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            IComponent,
            CtComponent,
            CpComponent
        };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length > 0) IComponent = values[0];
        if (values.Length > 1) CtComponent = values[1];
        if (values.Length > 2) CpComponent = values[2];
    }

    public int ComponentCount => 3;

    public void ConvertFrom(Color color)
    {
        // Convert sRGB to linear double RGB (0.0 to 1.0)
        var channels = ColorUtils.AdaptToDoubleFrom(color);

        // Linearize sRGB
        var r = Linearize(channels[0]);
        var g = Linearize(channels[1]);
        var b = Linearize(channels[2]);

        // RGB to LMS
        double l = 0.4002 * r + 0.7076 * g + -0.0808 * b;
        double m = -0.2263 * r + 1.1653 * g + 0.0457 * b;
        double s = 0.0 * r + 0.0 * g + 0.9182 * b;

        // LMS to ICtCp
        double i = 0.4000 * l + 0.4000 * m + 0.2000 * s;
        double ct = 4.4550 * l - 4.8510 * m + 0.3960 * s;
        double cp = 0.8056 * l + 0.3572 * m - 1.1628 * s;

        IComponent = new ColorRange(i);
        CtComponent = new ColorRange(ct);
        CpComponent = new ColorRange(cp);
    }

    public Color ConvertTo()
    {
        double i = IComponent;
        double ct = CtComponent;
        double cp = CpComponent;

        // ICtCp to LMS
        double l = i + 0.0976 * ct + 0.2052 * cp;
        double m = i - 0.1139 * ct + 0.1332 * cp;
        double s = i + 0.0326 * ct - 0.6769 * cp;

        // LMS to RGB
        double r = 1.8601 * l - 1.1295 * m + 0.2199 * s;
        double g = 0.3611 * l + 0.6389 * m - 0.0000 * s;
        double b = 0.0000 * l + 0.0000 * m + 1.0891 * s;

        // Apply gamma correction
        r = Delinearize(r);
        g = Delinearize(g);
        b = Delinearize(b);

        return ColorUtils.AdaptFromDoubleTo(r, g, b);
    }

    public ColorComponent[] GetComponents()
    {
        return new[]
        {
            new ColorComponent("Intensity", "I", I, 100.0, "0.0", "%"),
            new ColorComponent("Chromaticity blue-yellow", "Ct", Ct, 100.0, "0.0", ""),
            new ColorComponent("Chromaticity red-green", "Cp", Cp, 100.0, "0.0", "")
        };
    }

    internal static double Linearize(double c)
    {
        if (c <= 0.04045)
            return c / 12.92;
        else
            return Math.Pow((c + 0.055) / 1.055, 2.4);
    }

    internal static double Delinearize(double c)
    {
        if (c <= 0.0031308)
            return c * 12.92;
        else
            return 1.055 * Math.Pow(c, 1.0 / 2.4) - 0.055;
    }
}

public struct IPT : IColorSpace
{
    public ColorRange IntensityComponent { get; set; }
    public ColorRange ProtanComponent { get; set; }
    public ColorRange TritanComponent { get; set; }

    public IPT(ColorRange i, ColorRange p, ColorRange t)
    {
        IntensityComponent = i;
        ProtanComponent = p;
        TritanComponent = t;
    }

    public double I => IntensityComponent * 100.0;
    public double P => ProtanComponent * 100.0;
    public double T => TritanComponent * 100.0;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            IntensityComponent,
            ProtanComponent,
            TritanComponent
        };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length > 0) IntensityComponent = values[0];
        if (values.Length > 1) ProtanComponent = values[1];
        if (values.Length > 2) TritanComponent = values[2];
    }

    public int ComponentCount => 3;

    public void ConvertFrom(Color color)
    {
        // Convert sRGB to linear double RGB (0.0 to 1.0)
        var channels = ColorUtils.AdaptToDoubleFrom(color);

        // Linearize
        var r = Math.Pow(channels[0], 2.2);
        var g = Math.Pow(channels[1], 2.2);
        var b = Math.Pow(channels[2], 2.2);

        // RGB to LMS
        double l = 0.4002 * r + 0.7076 * g + -0.0808 * b;
        double m = -0.2263 * r + 1.1653 * g + 0.0457 * b;
        double s = 0.0 * r + 0.0 * g + 0.9182 * b;

        // Nonlinearity
        l = Math.Pow(l, 0.43);
        m = Math.Pow(m, 0.43);
        s = Math.Pow(s, 0.43);

        // LMS to IPT
        double i = 0.4000 * l + 0.4000 * m + 0.2000 * s;
        double p = 4.4550 * l - 4.8510 * m + 0.3960 * s;
        double t = 0.8056 * l + 0.3572 * m - 1.1628 * s;

        IntensityComponent = new ColorRange(i);
        ProtanComponent = new ColorRange(p);
        TritanComponent = new ColorRange(t);
    }

    public Color ConvertTo()
    {
        double i = IntensityComponent;
        double p = ProtanComponent;
        double t = TritanComponent;

        // IPT to LMS
        double l = i + 0.0976 * p + 0.2052 * t;
        double m = i - 0.1139 * p + 0.1332 * t;
        double s = i + 0.0326 * p - 0.6769 * t;

        // Inverse nonlinearity
        l = Math.Pow(l, 1.0 / 0.43);
        m = Math.Pow(m, 1.0 / 0.43);
        s = Math.Pow(s, 1.0 / 0.43);

        // LMS to RGB
        double r = 1.8601 * l - 1.1295 * m + 0.2199 * s;
        double g = 0.3611 * l + 0.6389 * m - 0.0000 * s;
        double b = 0.0000 * l + 0.0000 * m + 1.0891 * s;

        // Gamma correction
        r = Math.Pow(r, 1 / 2.2);
        g = Math.Pow(g, 1 / 2.2);
        b = Math.Pow(b, 1 / 2.2);

        return ColorUtils.AdaptFromDoubleTo(r, g, b);
    }

    public ColorComponent[] GetComponents()
    {
        return new[]
        {
            new ColorComponent("Intensity", "I", I, 100.0, "0.0", "%"),
            new ColorComponent("Protan", "P", P, 100.0, "0.0", ""),
            new ColorComponent("Tritan", "T", T, 100.0, "0.0", "")
        };
    }
}

public struct HunterLab
{
    public double L { get; set; }
    public double a { get; set; }
    public double b { get; set; }

    public HunterLab(double L, double a, double b)
    {
        this.L = L;
        this.a = a;
        this.b = b;
    }

    // Umrechnung von XYZ in Hunter Lab
    public static HunterLab FromXYZ(double X, double Y, double Z)
    {
        double L = 100 * (Y / 100);
        double a = 100 * (X - Y) / 100;
        double b = 100 * (Y - Z) / 100;

        return new HunterLab(L, a, b);
    }

    // Umrechnung von Hunter Lab in XYZ
    public static (double X, double Y, double Z) ToXYZ(HunterLab hunterLab)
    {
        double X = (hunterLab.a * 100) + hunterLab.L;
        double Y = hunterLab.L;
        double Z = (hunterLab.b * 100) + hunterLab.L;

        return (X, Y, Z);
    }
}

public struct Lab : IColorSpace
{
    public Lab(double l, double a, double b)
    {
        L = l;
        A = a;
        B = b;
    }

    public double L { get; set; } // Helligkeit
    public double A { get; set; } // Grün/Rot
    public double B { get; set; } // Blau/Gelb

    public int ComponentCount => 3;

    public ColorRange[] GetValues()
    {
        // Muss nachbessern
        throw new NotSupportedException();
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length >= 1) L = values[0] * 100.0;
        if (values.Length >= 2) A = values[1] * 255.0 - 128.0;
        if (values.Length >= 3) B = values[2] * 255.0 - 128.0;
    }

    public ColorComponent[] GetComponents()
    {
        return new[]
        {
            new ColorComponent("Lightness", "L", L, 100.0, "0.0", ""),
            new ColorComponent("GreenComponent–RedComponent", "a", A, 255.0, "-128", ""),
            new ColorComponent("BlueComponent–Yellow", "b", B, 255.0, "-128", "")
        };
    }

    public void ConvertFrom(Color color)
    {
        var xyz = new XYZ();
        xyz.ConvertFrom(color);

        double x = xyz.X / 95.047;
        double y = xyz.Y / 100.000;
        double z = xyz.Z / 108.883;

        x = x > 0.008856 ? Math.Pow(x, 1.0 / 3.0) : (7.787 * x) + (16.0 / 116.0);
        y = y > 0.008856 ? Math.Pow(y, 1.0 / 3.0) : (7.787 * y) + (16.0 / 116.0);
        z = z > 0.008856 ? Math.Pow(z, 1.0 / 3.0) : (7.787 * z) + (16.0 / 116.0);

        L = (116.0 * y) - 16.0;
        A = 500.0 * (x - y);
        B = 200.0 * (y - z);
    }

    public Color ConvertTo()
    {
        double y = (L + 16.0) / 116.0;
        double x = A / 500.0 + y;
        double z = y - B / 200.0;

        double x3 = Math.Pow(x, 3.0);
        double y3 = Math.Pow(y, 3.0);
        double z3 = Math.Pow(z, 3.0);

        x = x3 > 0.008856 ? x3 : (x - 16.0 / 116.0) / 7.787;
        y = y3 > 0.008856 ? y3 : (y - 16.0 / 116.0) / 7.787;
        z = z3 > 0.008856 ? z3 : (z - 16.0 / 116.0) / 7.787;

        var xyz = new XYZ
        {
            X = x * 95.047,
            Y = y * 100.000,
            Z = z * 108.883
        };

        return xyz.ConvertTo();
    }
}

public struct LMS
{
    public double L { get; set; }
    public double M { get; set; }
    public double S { get; set; }

    public LMS(double L, double M, double S)
    {
        this.L = L;
        this.M = M;
        this.S = S;
    }

    // Umrechnung von RGB in LMS
    public static LMS FromRGB(double R, double G, double B)
    {
        double L = 0.4002 * R + 0.7075 * G - 0.0808 * B;
        double M = -0.2280 * R + 1.1506 * G + 0.0612 * B;
        double S = 0.0000 * R + 0.0000 * G + 0.9184 * B;

        return new LMS(L, M, S);
    }

    // Umrechnung von LMS in RGB
    public static (double R, double G, double B) ToRGB(LMS lms)
    {
        double R = 1.9835 * lms.L - 1.4593 * lms.M + 0.1211 * lms.S;
        double G = -0.9780 * lms.L + 1.8765 * lms.M + 0.0410 * lms.S;
        double B = 0.0715 * lms.L - 0.1221 * lms.M + 1.0086 * lms.S;

        return (R, G, B);
    }
}

public struct Luv
{
    public double L { get; set; }
    public double u { get; set; }
    public double v { get; set; }

    public Luv(double L, double u, double v)
    {
        this.L = L;
        this.u = u;
        this.v = v;
    }

    // Umrechnung von XYZ in Luv
    public static Luv FromXYZ(double X, double Y, double Z)
    {
        double uPrime = (4 * X) / (X + 15 * Y + 3 * Z);
        double vPrime = (9 * Y) / (X + 15 * Y + 3 * Z);

        double L = 116 * Math.Pow(Y / 100.0, 1.0 / 3.0) - 16;
        if (L < 0) L = 0;

        double uComponent = 13 * L * (uPrime - 0.19793943);
        double vComponent = 13 * L * (vPrime - 0.46831507);

        return new Luv(L, uComponent, vComponent);
    }

    // Umrechnung von Luv in XYZ
    public static (double X, double Y, double Z) ToXYZ(Luv luv)
    {
        double uPrime = (luv.u + 0.19793943) / (13 * luv.L) + 0.19793943;
        double vPrime = (luv.v + 0.46831507) / (13 * luv.L) + 0.46831507;

        double Y = Math.Pow((luv.L + 16) / 116.0, 3.0) * 100.0;
        double X = (9 * uPrime * Y) / (4 * vPrime);
        double Z = (Y - 0.95047 * X - 1.09625 * Y) / 0.01805;

        return (X, Y, Z);
    }
}

public struct OKLab : IColorSpace
{
    public ColorRange LComponent { get; set; }
    public ColorRange AComponent { get; set; }
    public ColorRange BComponent { get; set; }

    public OKLab(ColorRange l, ColorRange a, ColorRange b)
    {
        LComponent = l;
        AComponent = a;
        BComponent = b;
    }

    public double L => LComponent * 100.0;
    public double A => AComponent * 100.0;
    public double B => BComponent * 100.0;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            LComponent,
            AComponent,
            BComponent
        };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length > 0) LComponent = values[0];
        if (values.Length > 1) AComponent = values[1];
        if (values.Length > 2) BComponent = values[2];
    }

    public int ComponentCount => 3;

    public void ConvertFrom(Color color)
    {
        // Convert sRGB to linear double RGB (0.0 to 1.0)
        var channels = ColorUtils.AdaptToDoubleFrom(color);

        // linearize
        var r = ICtCp.Linearize(channels[0]);
        var g = ICtCp.Linearize(channels[1]);
        var b = ICtCp.Linearize(channels[2]);

        // RGB → LMS
        var l = 0.412221 * r + 0.536332 * g + 0.051445 * b;
        var m = 0.211903 * r + 0.680699 * g + 0.107397 * b;
        var s = 0.088302 * r + 0.281718 * g + 0.629978 * b;

        l = Math.Cbrt(l);
        m = Math.Cbrt(m);
        s = Math.Cbrt(s);

        var L = 0.210454 * l + 0.793617 * m - 0.004072 * s;
        var A = 1.977998 * l - 2.428592 * m + 0.450593 * s;
        var B = 0.025904 * l + 0.782771 * m - 0.808675 * s;

        LComponent = new ColorRange(L);
        AComponent = new ColorRange(A);
        BComponent = new ColorRange(B);
    }

    public Color ConvertTo()
    {
        double L = LComponent;
        double A = AComponent;
        double B = BComponent;

        var l_ = L + 0.3963377774 * A + 0.2158037573 * B;
        var m_ = L - 0.1055613458 * A - 0.0638541728 * B;
        var s_ = L - 0.0894841775 * A - 1.2914855480 * B;

        l_ = l_ * l_ * l_;
        m_ = m_ * m_ * m_;
        s_ = s_ * s_ * s_;

        var r = +4.0767417 * l_ - 3.3077116 * m_ + 0.2309699 * s_;
        var g = -1.2684380 * l_ + 2.6097574 * m_ - 0.3413194 * s_;
        var b = -0.0041961 * l_ - 0.7034186 * m_ + 1.7076147 * s_;

        return ColorUtils.AdaptFromDoubleTo(r, g, b);
    }

    public ColorComponent[] GetComponents()
    {
        return new[]
        {
            new ColorComponent("Lightness", "L", L, 100.0, "0.0", ""),
            new ColorComponent("GreenComponent-RedComponent", "A", A, 100.0, "0.0", ""),
            new ColorComponent("BlueComponent-Yellow", "B", B, 100.0, "0.0", "")
        };
    }
}

public struct RGB : IColorSpace
{
    public RGB(ColorRange r, ColorRange g, ColorRange b)
    {
        RedComponent = r;
        GreenComponent = g;
        BlueComponent = b;
    }

    public RGB(Color color) : this()
    {
        ConvertFrom(color);
    }

    public ColorRange RedComponent { get; set; }

    public ColorRange GreenComponent { get; set; }

    public ColorRange BlueComponent { get; set; }

    public byte Red
    {
        get => Convert.ToByte(RedComponent * 255.0);
    }

    public byte R
    {
        get => Red;
    }

    public byte Green
    {
        get => Convert.ToByte(GreenComponent * 255.0);
    }

    public byte G
    {
        get => Green;
    }

    public byte Blue
    {
        get => Convert.ToByte(BlueComponent * 255.0);
    }

    public byte B
    {
        get => Blue;
    }

    public int ComponentCount
    {
        get => 3;
    }

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            RedComponent,
            GreenComponent,
            BlueComponent
        };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length == 1)
        {
            RedComponent = values[0];
        }
        else if (values.Length == 2)
        {
            RedComponent = values[0];
            GreenComponent = values[1];
        }
        else if (values.Length == 3)
        {
            RedComponent = values[0];
            GreenComponent = values[1];
            BlueComponent = values[2];
        }
    }

    public void ConvertFrom(Color color)
    {
        var channels = ColorUtils.AdaptFrom(color);

        RedComponent = new ColorRange(channels[0] / 255.0);
        GreenComponent = new ColorRange(channels[1] / 255.0);
        BlueComponent = new ColorRange(channels[2] / 255.0);
    }

    public Color ConvertTo()
    {
        return ColorUtils.AdaptTo(
            Red,
            Green,
            Blue,
            255);
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("RedComponent", "R", Red,  byte.MaxValue, "0", ""),
            new ColorComponent("GreenComponent", "G", Green,  byte.MaxValue, "0", ""),
            new ColorComponent("BlueComponent", "B", Blue, byte.MaxValue, "0", "")
        };
    }

    public override string ToString()
    {
        return $"RGB({RedComponent * 255.0:F0}, {GreenComponent * 255.0:F0}, {BlueComponent * 255.0:F0})";
    }
}

public struct RGBW : IColorSpace
{
    public RGBW(ColorRange r, ColorRange g, ColorRange b, ColorRange w)
    {
        this.RedComponent = r;
        this.GreenComponent = g;
        this.BlueComponent = b;
        this.WhiteComponent = w;
    }

    public ColorRange RedComponent { get; set; }
    public ColorRange GreenComponent { get; set; }
    public ColorRange BlueComponent { get; set; }
    public ColorRange WhiteComponent { get; set; }

    // Umrechnungen der Farbkomponenten, falls erforderlich
    public double R => this.RedComponent * 100.0;
    public double G => this.GreenComponent * 100.0;
    public double B => this.BlueComponent * 100.0;
    public double W => this.WhiteComponent * 100.0;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            RedComponent,
            GreenComponent,
            BlueComponent,
            WhiteComponent
        };
    }

    // Method to set values, in case of different inputs
    public void SetValues(params ColorRange[] values)
    {
        if (values.Length == 4)
        {
            this.RedComponent = values[0];
            this.GreenComponent = values[1];
            this.BlueComponent = values[2];
            this.WhiteComponent = values[3];
        }
    }

    public int ComponentCount => 4;

    // Konvertierungen zwischen RGBW und anderen Farbmodellen
    public void ConvertFrom(Color color)
    {
        // Umrechnungslogik für die Umwandlung aus RGB oder einem anderen Modell
    }

    public Color ConvertTo()
    {
        // Logik zur Rückumwandlung in eine andere Farbe
        return ColorUtils.AdaptTo((byte)(this.R), (byte)(this.G), (byte)(this.B), (byte)(255));
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("RedComponent", "R", this.R, 100.0, "0.0", "%"),
            new ColorComponent("GreenComponent", "G", this.G, 100.0, "0.0", "%"),
            new ColorComponent("BlueComponent", "B", this.B, 100.0, "0.0", "%"),
            new ColorComponent("WhiteComponent", "W", this.W, 100.0, "0.0", "%")
        };
    }
}

public struct YCbCr : IColorSpace
{
    public ColorRange YComponent { get; set; }
    public ColorRange CbComponent { get; set; }
    public ColorRange CrComponent { get; set; }

    public YCbCr(ColorRange y, ColorRange cb, ColorRange cr)
    {
        this.YComponent = y;
        this.CbComponent = cb;
        this.CrComponent = cr;
    }

    public double Y => YComponent * 255.0;
    public double Cb => CbComponent * 255.0;
    public double Cr => CrComponent * 255.0;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            YComponent,
            CbComponent,
            CrComponent
        };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length > 0) YComponent = values[0];
        if (values.Length > 1) CbComponent = values[1];
        if (values.Length > 2) CrComponent = values[2];
    }

    public int ComponentCount => 3;

    public void ConvertFrom(Color color)
    {
        // Convert sRGB to linear double RGB (0.0 to 1.0)
        var channels = ColorUtils.AdaptToDoubleFrom(color);
        var r = channels[0];
        var g = channels[1];
        var b = channels[2];

        var y = 0.299 * r + 0.587 * g + 0.114 * b;
        var cb = (b - y) * 0.564 + 0.5;
        var cr = (r - y) * 0.713 + 0.5;

        YComponent = new ColorRange(y);
        CbComponent = new ColorRange(cb);
        CrComponent = new ColorRange(cr);
    }

    public Color ConvertTo()
    {
        var y = YComponent;
        var cb = CbComponent - 0.5;
        var cr = CrComponent - 0.5;

        var r = y + 1.403 * cr;
        var g = y - 0.344 * cb - 0.714 * cr;
        var b = y + 1.770 * cb;

        return ColorUtils.AdaptFromDoubleTo(r, g, b);
    }

    public ColorComponent[] GetComponents()
    {
        return new[]
        {
            new ColorComponent("Luma", "Y", Y, 255.0, "0.0", ""),
            new ColorComponent("BlueComponent-diff", "Cb", Cb, 255.0, "0.0", ""),
            new ColorComponent("RedComponent-diff", "Cr", Cr, 255.0, "0.0", "")
        };
    }
}

public struct YUV : IColorSpace
{
    public ColorRange YComponent { get; set; }
    public ColorRange UComponent { get; set; }
    public ColorRange VComponent { get; set; }

    public YUV(ColorRange y, ColorRange u, ColorRange v)
    {
        YComponent = y;
        UComponent = u;
        VComponent = v;
    }

    public double Y => YComponent * 255.0;
    public double U => UComponent * 255.0;
    public double V => VComponent * 255.0;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            YComponent,
            UComponent,
            VComponent
        };
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length > 0) YComponent = values[0];
        if (values.Length > 1) UComponent = values[1];
        if (values.Length > 2) VComponent = values[2];
    }

    public int ComponentCount => 3;

    public void ConvertFrom(Color color)
    {
        // Convert sRGB to linear double RGB (0.0 to 1.0)
        var channels = ColorUtils.AdaptToDoubleFrom(color);
        var r = channels[0];
        var g = channels[1];
        var b = channels[2];

        var y = 0.299 * r + 0.587 * g + 0.114 * b;
        var u = -0.14713 * r - 0.28886 * g + 0.436 * b;
        var v = 0.615 * r - 0.51499 * g - 0.10001 * b;

        YComponent = new ColorRange(y);
        UComponent = new ColorRange(u);
        VComponent = new ColorRange(v);
    }

    public Color ConvertTo()
    {
        var y = YComponent;
        var u = UComponent;
        var v = VComponent;

        var r = y + 1.13983 * v;
        var g = y - 0.39465 * u - 0.58060 * v;
        var b = y + 2.03211 * u;

        return ColorUtils.AdaptFromDoubleTo(r, g, b);
    }

    public ColorComponent[] GetComponents()
    {
        return new[]
        {
            new ColorComponent("Luma", "Y", Y, 255.0, "0.0", ""),
            new ColorComponent("U (blue projection)", "U", U, 255.0, "0.0", ""),
            new ColorComponent("V (red projection)", "V", V, 255.0, "0.0", "")
        };
    }
}

public struct TCS : IColorSpace
{
    public TCS(ColorRange red, ColorRange green, ColorRange blue, ColorRange luminance)
    {
        this.RedComponent = red;
        this.GreenComponent = green;
        this.BlueComponent = blue;
        this.LuminanceComponent = luminance;
    }

    public ColorRange RedComponent { get; set; }
    public ColorRange GreenComponent { get; set; }
    public ColorRange BlueComponent { get; set; }
    public ColorRange LuminanceComponent { get; set; }

    public double R => this.RedComponent * 100.0;
    public double G => this.GreenComponent * 100.0;
    public double B => this.BlueComponent * 100.0;
    public double L => this.LuminanceComponent * 100.0;

    public ColorRange[] GetValues()
    {
        return new ColorRange[] {
            RedComponent,
            GreenComponent,
            BlueComponent,
            LuminanceComponent
        };
    }

    // Method to set values
    public void SetValues(params ColorRange[] values)
    {
        if (values.Length == 4)
        {
            this.RedComponent = values[0];
            this.GreenComponent = values[1];
            this.BlueComponent = values[2];
            this.LuminanceComponent = values[3];
        }
    }

    public int ComponentCount => 4;

    // Konvertierungen zwischen TCS und anderen Farbmodellen
    public void ConvertFrom(Color color)
    {
        // Umrechnungslogik von einem anderen Farbmodell (z. B. RGB) nach TCS
        var channels = ColorUtils.AdaptFrom(color);
        // Berechne TCS-Komponenten basierend auf diesen Kanälen
    }

    public Color ConvertTo()
    {
        // Umrechnung der TCS-Komponenten in eine Standardfarbe, z. B. RGB
        return ColorUtils.AdaptTo((byte)(this.R), (byte)(this.G), (byte)(this.B), (byte)255);
    }

    public ColorComponent[] GetComponents()
    {
        return new ColorComponent[]
        {
            new ColorComponent("RedComponent", "R", this.R, 100.0, "0.0", "%"),
            new ColorComponent("GreenComponent", "G", this.G, 100.0, "0.0", "%"),
            new ColorComponent("BlueComponent", "B", this.B, 100.0, "0.0", "%"),
            new ColorComponent("LuminanceComponent", "L", this.L, 100.0, "0.0", "%")
        };
    }
}

public struct XYZ : IColorSpace
{
    public XYZ(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double X { get; set; } // Helligkeit
    public double Y { get; set; }
    public double Z { get; set; }

    public int ComponentCount => 3;

    public ColorRange[] GetValues()
    {
        // Nachbessern...
        throw new NotImplementedException();
    }

    public void SetValues(params ColorRange[] values)
    {
        if (values.Length >= 1) X = values[0] * 100.0;
        if (values.Length >= 2) Y = values[1] * 100.0;
        if (values.Length >= 3) Z = values[2] * 100.0;
    }

    public ColorComponent[] GetComponents()
    {
        return new[]
        {
            new ColorComponent("X", "X", X, 100.0, "0.0", ""),
            new ColorComponent("Y", "Y", Y, 100.0, "0.0", ""),
            new ColorComponent("Z", "Z", Z, 100.0, "0.0", "")
        };
    }

    public void ConvertFrom(Color color)
    {
        var channels = ColorUtils.AdaptFrom(color);

        double r = channels[0] / 255.0;
        double g = channels[1] / 255.0;
        double b = channels[2] / 255.0;

        r = r > 0.04045 ? Math.Pow((r + 0.055) / 1.055, 2.4) : r / 12.92;
        g = g > 0.04045 ? Math.Pow((g + 0.055) / 1.055, 2.4) : g / 12.92;
        b = b > 0.04045 ? Math.Pow((b + 0.055) / 1.055, 2.4) : b / 12.92;

        r *= 100;
        g *= 100;
        b *= 100;

        // Observer = 2°, Illuminant = D65
        X = r * 0.4124 + g * 0.3576 + b * 0.1805;
        Y = r * 0.2126 + g * 0.7152 + b * 0.0722;
        Z = r * 0.0193 + g * 0.1192 + b * 0.9505;
    }

    public Color ConvertTo()
    {
        double x = X / 100.0;
        double y = Y / 100.0;
        double z = Z / 100.0;

        double r = x * 3.2406 + y * -1.5372 + z * -0.4986;
        double g = x * -0.9689 + y * 1.8758 + z * 0.0415;
        double b = x * 0.0557 + y * -0.2040 + z * 1.0570;

        r = r > 0.0031308 ? 1.055 * Math.Pow(r, 1.0 / 2.4) - 0.055 : 12.92 * r;
        g = g > 0.0031308 ? 1.055 * Math.Pow(g, 1.0 / 2.4) - 0.055 : 12.92 * g;
        b = b > 0.0031308 ? 1.055 * Math.Pow(b, 1.0 / 2.4) - 0.055 : 12.92 * b;

        return ColorUtils.AdaptTo(
            ColorUtils.Clamp((int)Math.Round(r * 255), 0, 255),
            ColorUtils.Clamp((int)Math.Round(g * 255), 0, 255),
            ColorUtils.Clamp((int)Math.Round(b * 255), 0, 255),
            255
        );
    }
}

