using System;

#if SASOGINE
using Microsoft.Xna.Framework;
namespace sachssoft.Sasogine.Graphics.Colors.Transformers;
#elif MONOGAME
using Microsoft.Xna.Framework;
namespace sachssoft.Monogame.Colors.Transformers;
#elif AVALONIA
using Ava = Avalonia;
namespace sachssoft.Avalonia.Colors.Transformers;
using Color = Ava.Media.Color;
#elif DRAWING
namespace sachssoft.Drawing.Colors.Transformers;
using Color = System.Drawing.Color;
#elif WPF
namespace sachssoft.WPF.Colors.Transformers;
using Color = System.Windows.Media.Color;
#elif SKIA
namespace sachssoft.Skia.Colors.Transformers;
using Color = SkiaSharp.SKColor;
#else
namespace sachssoft.Colors.Transformers;
using Color = sachssoft.Colors.ColorCode;
#endif

public sealed class AdditiveColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte Add(byte channel)
        {
            return ColorUtils.Clamp(channel + amount * 255f, 0, 255); // Amount beeinflusst den Wert
        }

        return ColorUtils.AdaptTo(
            Add(channels[0]),
            Add(channels[1]),
            Add(channels[2]),
            channels[3]
        );
    }
}

public sealed class AlphaColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // amount ∈ [0, 1] → direkte Festlegung des Alpha-Werts
        return ColorUtils.AdaptTo(
            channels[0],
            channels[1],
            channels[2],
            (byte)(amount * 255f)
        );
    }
}

public sealed class BrightColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // amount ∈ [0, 1] → 0 = schwarz, 1 = unverändert
        amount = Math.Clamp(amount, 0f, 1f);

        byte Adjust(byte c) => (byte)(c * amount);

        return ColorUtils.AdaptTo(
            Adjust(channels[0]),
            Adjust(channels[1]),
            Adjust(channels[2]),
            channels[3]
        );
    }
}

public sealed class ChannelSwapColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Rot mit Blau vertauschen
        byte swapped_red = channels[2]; // B
        byte swapped_green = channels[1]; // G
        byte swapped_blue = channels[0]; // R

        return ColorUtils.AdaptTo(
            (byte)(swapped_red * amount + channels[0] * (1f - amount)),
            (byte)(swapped_green * amount + channels[1] * (1f - amount)),
            (byte)(swapped_blue * amount + channels[2] * (1f - amount)),
            channels[3]
        );
    }
}

public sealed class ChromaticAberrationColorTransformer : IFactorColorTransformer
{
    public float FactorMinimum => 0.0f; // Keine chromatische Aberration
    public float FactorMaximum => 1.0f; // Maximale chromatische Aberration

    public Color Transform(Color color, float amount, float factor)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Beschränkung des Faktorenbereichs
        factor = Math.Clamp(factor, FactorMinimum, FactorMaximum);

        // Chromatische Aberration - Verschiebung von Farben je nach Amount
        byte ApplyChromaticAberration(byte channel, float amount, float factor)
        {
            return ColorUtils.Clamp(channel + (channel * factor * amount), 0, 255);
        }

        // Verschiebe Farben je nach Aberration
        return ColorUtils.AdaptTo(
            ApplyChromaticAberration(channels[0], amount, factor),
            ApplyChromaticAberration(channels[1], amount, factor),
            ApplyChromaticAberration(channels[2], amount, factor),
            channels[3]
        );
    }

    public Color Transform(Color color, float amount)
    {
        return Transform(color, amount, 0.3f); // Standardwert für chromatische Aberration
    }
}

public sealed class ColorBalanceColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte AdjustBalance(byte channel, float amount)
        {
            return ColorUtils.Clamp(channel + (channel - 128) * amount, 0, 255);
        }

        return ColorUtils.AdaptTo(
            AdjustBalance(channels[0], amount),
            AdjustBalance(channels[1], amount),
            AdjustBalance(channels[2], amount),
            channels[3]
        );
    }
}
public sealed class ColorBurnColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte ColorBurn(byte channel)
        {
            return (byte)Math.Max(0, channel - (255 - channel) * amount);
        }

        return ColorUtils.AdaptTo(
            ColorBurn(channels[0]),
            ColorBurn(channels[1]),
            ColorBurn(channels[2]),
            channels[3]
        );
    }
}

public sealed class ContrastColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // amount: 1 = Originalkontrast, 0 = vollständig grau
        amount = Math.Clamp(amount, 0f, 1f);

        byte Adjust(byte channel)
        {
            float normalized = channel / 255f;
            float adjusted = ((normalized - 0.5f) * amount + 0.5f) * 255f;
            return ColorUtils.Clamp(adjusted, 0f, 255f);
        }

        return ColorUtils.AdaptTo(
            Adjust(channels[0]),
            Adjust(channels[1]),
            Adjust(channels[2]),
            channels[3]
        );
    }
}

public sealed class DarkenColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // amount: 0 = keine Abdunkelung, 1 = maximale Abdunkelung
        amount = Math.Clamp(amount, 0f, 1f);

        byte Darken(byte channel) =>
            ColorUtils.Clamp(channel - (channel * amount), 0, 255);

        return ColorUtils.AdaptTo(
            Darken(channels[0]),
            Darken(channels[1]),
            Darken(channels[2]),
            channels[3]
        );
    }
}

public sealed class DepthColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // amount: 0 = niedrigere Farbtiefe, 1 = normale Farbtiefe
        // Um die Farbtiefe zu simulieren, wird der Farbkanal auf den entsprechenden Bereich beschränkt.

        // Zuerst berechnen wir die maximale Anzahl an Stufen basierend auf der Farbtiefe
        int max_depth = (int)(255 * amount); // Bereich von 0 bis 255, abhängig von 'amount'

        byte Adjust(byte channel)
        {
            return (byte)((channel / 255f) * max_depth);
        }

        return ColorUtils.AdaptTo(
            Adjust(channels[0]),
            Adjust(channels[1]),
            Adjust(channels[2]),
            channels[3]
        );
    }
}

public class DesaturationTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        amount = Math.Clamp(amount, 0f, 1f); // Begrenzung auf [0, 1]

        float gray = (channels[0] + channels[1] + channels[2]) / 3f;

        byte Mix(byte original) =>
            (byte)Math.Clamp(original * (1 - amount) + gray * amount, 0, 255);

        return ColorUtils.AdaptTo(
            Mix(channels[0]),
            Mix(channels[1]),
            Mix(channels[2]),
            channels[3]
        );
    }
}

public sealed class DivideColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte Add(byte channel)
        {
            return ColorUtils.Clamp(channel / amount, 0, 255);
        }

        return ColorUtils.AdaptTo(
            Add(channels[0]),
            Add(channels[1]),
            Add(channels[2]),
            channels[3]
        );
    }
}

public sealed class GammaColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        if (amount <= 0f)
            return color; // keine Veränderung bei amount = 0

        float inverse_gamma = 1.0f / amount;

        byte Adjust(byte channel)
        {
            float normalized = channel / 255f;
            float corrected = MathF.Pow(normalized, inverse_gamma) * 255f;
            return ColorUtils.Clamp(corrected, 0f, 255f);
        }

        return ColorUtils.AdaptTo(
            Adjust(channels[0]),
            Adjust(channels[1]),
            Adjust(channels[2]),
            channels[3]
        );
    }
}

public sealed class GrayColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Sicherstellen, dass amount im Bereich [0, 1] liegt
        amount = Math.Clamp(amount, 0f, 1f);

        // Grauwert berechnen nach Luminanzformel
        int gray = (int)(channels[0] * 0.3 + channels[1] * 0.59 + channels[2] * 0.11);

        // Interpolation zwischen Originalfarbe und Grauwert
        byte Blend(byte original, int gray_value) =>
            (byte)Math.Round(original + (gray_value - original) * amount);

        return ColorUtils.AdaptTo(
            Blend(channels[0], gray),
            Blend(channels[1], gray),
            Blend(channels[2], gray),
            channels[3]
        );
    }
}

public sealed class LerpColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Verwende amount direkt als Interpolationsfaktor (0 = keine Änderung, 1 = maximal)
        byte Lerp(byte channel)
        {
            return ColorUtils.Clamp(channel + (255 - channel) * amount, 0, 255);
        }

        return ColorUtils.AdaptTo(
            Lerp(channels[0]),
            Lerp(channels[1]),
            Lerp(channels[2]),
            channels[3]
        );
    }
}

public sealed class LightenColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // amount: 0 = keine Aufhellung, 1 = maximale Aufhellung
        amount = Math.Clamp(amount, 0f, 1f);

        byte Lighten(byte channel) =>
            ColorUtils.Clamp(channel + (255 - channel) * amount, 0, 255);

        return ColorUtils.AdaptTo(
            Lighten(channels[0]),
            Lighten(channels[1]),
            Lighten(channels[2]),
            channels[3]
        );
    }
}

public sealed class HueColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // amount: 0 = keine Verschiebung, 1 = maximale Verschiebung
        float shift_amount = amount * 180f; // 180° Verschiebung maximal
        float radians = shift_amount * (MathF.PI / 180f); // Konvertiere zu Radians

        byte Shift(byte channel, float amount)
        {
            // Umrechnung von Farbsättigung auf einfache Weise
            return ColorUtils.Clamp((channel + amount) % 255, 0, 255);
        }

        return ColorUtils.AdaptTo(
            Shift(channels[0], radians),
            Shift(channels[1], radians),
            Shift(channels[2], radians),
            channels[3]
        );
    }
}

public sealed class IntensifyColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Sicherstellen, dass 'amount' im Bereich von 0 bis 2 liegt, um extreme Farbverstärkungen zu vermeiden
        amount = Math.Clamp(amount, 0f, 2f);

        byte Intensify(byte channel)
        {
            // Kanalwert wird basierend auf 'amount' verstärkt
            int result = (int)(channel * amount);
            return (byte)ColorUtils.Clamp(result, 0, 255);
        }

        return ColorUtils.AdaptTo(
            Intensify(channels[0]),
            Intensify(channels[1]),
            Intensify(channels[2]),
            channels[3]
        );
    }
}

public sealed class InvertColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Sicherstellen, dass amount im Bereich [0, 1] liegt
        amount = Math.Clamp(amount, 0f, 1f);

        // Interpolation zwischen Originalfarbe und invertierter Farbe
        byte Mix(byte original) =>
            (byte)(original + (255 - original) * 2 * amount);  // Invertierung und Interpolation

        return ColorUtils.AdaptTo(
            Mix(channels[0]),
            Mix(channels[1]),
            Mix(channels[2]),
            channels[3]
        );
    }
}

public sealed class MultiplyColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte Add(byte channel)
        {
            return ColorUtils.Clamp(channel * amount, 0, 255);
        }

        return ColorUtils.AdaptTo(
            Add(channels[0]),
            Add(channels[1]),
            Add(channels[2]),
            channels[3]
        );
    }
}

public sealed class PosterizeColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte Posterize(byte channel)
        {
            int steps = ColorUtils.Clamp((int)(amount * 16), 1, 255);  // Mindestens 1 Stufe
            float step_size = 255f / (steps - 1);
            return (byte)(MathF.Round(channel / step_size) * step_size);
        }

        return ColorUtils.AdaptTo(
            Posterize(channels[0]),
            Posterize(channels[1]),
            Posterize(channels[2]),
            channels[3]
        );
    }
}

public class QuantizeColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        int steps = (int)Math.Max(2, amount); // steps = 2..255
        float step_size = 255f / (steps - 1);

        byte Quantize(byte c) => (byte)(Math.Round(c / step_size) * step_size);

        return ColorUtils.AdaptTo(
            Quantize(channels[0]),
            Quantize(channels[1]),
            Quantize(channels[2]),
            channels[3]
        );
    }
}

public sealed class SaturateColorTransformer : IFactorColorTransformer
{
    public float FactorMinimum => 0.0f; // Keine Sättigung
    public float FactorMaximum => 2.0f; // Maximale Sättigung

    public Color Transform(Color color, float amount, float factor)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Beschränkung des Faktorenbereichs
        factor = Math.Clamp(factor, FactorMinimum, FactorMaximum);

        // Berechne die Sättigung
        byte BoostSaturation(byte channel, float factor)
        {
            return ColorUtils.Clamp(channel * factor * amount, 0, 255);
        }

        // Wende den Sättigungsboost an
        return ColorUtils.AdaptTo(
            BoostSaturation(channels[0], factor),
            BoostSaturation(channels[1], factor),
            BoostSaturation(channels[2], factor),
            channels[3]
        );
    }

    public Color Transform(Color color, float amount)
    {
        return Transform(color, amount, 1.5f); // Standardwert für Sättigungsfaktor
    }
}

public sealed class SolarizeColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        amount = Math.Clamp(amount, 0f, 1f);

        byte Solarize(byte channel)
        {
            if (channel < 128)
                return channel;
            else
                return (byte)(255 - (channel - 128) * amount);
        }

        return ColorUtils.AdaptTo(
            Solarize(channels[0]),
            Solarize(channels[1]),
            Solarize(channels[2]),
            channels[3]
        );
    }
}

public sealed class SubtractiveColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte Add(byte channel)
        {
            return ColorUtils.Clamp(channel - amount * 255f, 0, 255);
        }

        return ColorUtils.AdaptTo(
            Add(channels[0]),
            Add(channels[1]),
            Add(channels[2]),
            channels[3]
        );
    }
}

public sealed class TemperatureColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float temperatureAmount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // temperatureAmount: 
        // 0 = kälter (blau), 
        // 1 = neutral (keine Änderung), 
        // 2 = wärmer (rot)

        // Angenommene Temperaturwerte zwischen 0 und 2
        // 0 -> kühler (blauer)
        // 1 -> neutral
        // 2 -> wärmer (roter)

        // Berechne die Anpassung für Rot und Blau
        byte AdjustRed(byte r, float amount)
        {
            // Wenn es kühler werden soll, den Rotanteil verringern
            if (amount < 1.0f)
                return (byte)(r * (1 - amount * 0.5f)); // Je näher 0, desto weniger rot
            return r;
        }

        byte AdjustBlue(byte b, float amount)
        {
            // Wenn es wärmer werden soll, den Blauanteil verringern
            if (amount > 1.0f)
                return (byte)(b * (1 - (amount - 1) * 0.5f)); // Je näher 2, desto weniger blau
            return b;
        }

        byte r = AdjustRed(channels[0], temperatureAmount);
        byte b = AdjustBlue(channels[2], temperatureAmount);

        return ColorUtils.AdaptTo(
            r,
            channels[1],
            b,
            channels[3]
        );
    }
}

public sealed class ToneColorTransformer : IFactorColorTransformer
{
    // Definiert den minimalen und maximalen Faktor
    public float FactorMinimum { get; } = 0.1f;  // Beispiel: minimaler Faktor
    public float FactorMaximum { get; } = 3.0f;  // Beispiel: maximaler Faktor

    public Color Transform(Color color, float amount, float factor)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Beschränke den Faktor im Bereich von FactorMinimum bis FactorMaximum
        factor = Math.Clamp(factor, FactorMinimum, FactorMaximum);

        // amount: 0 = keine Änderung, > 0 = stärkere Anpassung
        byte ToneAdjustment(byte channel, float factor)
        {
            return (byte)ColorUtils.Clamp((float)(channel * Math.Pow(factor, amount)), 0, 255);
        }

        return ColorUtils.AdaptTo(
            ToneAdjustment(channels[0], factor),
            ToneAdjustment(channels[1], factor),
            ToneAdjustment(channels[2], factor),
            channels[3]
        );
    }

    public Color Transform(Color color, float amount)
    {
        return Transform(color, amount, 1.0f); // Standardwert für factor
    }
}

public sealed class VibranceColorTransformer : IColorTransformer
{
    public Color Transform(Color color, float amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Wenn amount 0 ist, keine Änderung. Wenn amount 1, maximale Veränderung.
        amount = Math.Clamp(amount, 0f, 1f);
        float gray = 0.3f * channels[0] + 0.59f * channels[1] + 0.11f * channels[2];

        // Verwende amount, um die Farbbrillanz zu erhöhen
        byte Adjust(byte channel)
        {
            if (channel < 128)
                return (byte)(channel + (byte)(128 * amount));
            return channel;
        }

        return ColorUtils.AdaptTo(
            Adjust(channels[0]),
            Adjust(channels[1]),
            Adjust(channels[2]),
            channels[3]
        );
    }
}




