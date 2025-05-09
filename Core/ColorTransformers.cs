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

// OK
public sealed class OpacityColorTransformer : IColorTransformer
{
    // 0 = Normal
    // 1 = Transparent

    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Berechne den Alpha-Wert, indem du amount von 0 bis 1 skalierst und auf 255 umrechnest.
        byte alpha = (byte)(255f * (1f - (float)amount));

        return ColorUtils.AdaptTo(
            channels[0],
            channels[1],
            channels[2],
            alpha
        );
    }
}


// OK
public sealed class BrightColorTransformer : IColorTransformer /*, IColorChannelTransformer*/
{
    // 0 = Normal
    // 1 = Hell

    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Aufhellung: Interpolation von Originalwert zu 255 (Weiß)
        byte Adjust(byte c) => (byte)(c + (255f - c) * (float)amount);

        return ColorUtils.AdaptTo(
            Adjust(channels[0]),
            Adjust(channels[1]),
            Adjust(channels[2]),
            channels[3]
        );
    }

    //public Color Transform<T>(Color color, T channel_amount) where T : struct, IColorSpace
    //{
    //    var space = color.ToSpace<T>();
    //    var output = new T();

    //    var count = output.ComponentCount;
    //    var values = output.GetValues();

    //    for (var i = 0; i < count; i++)
    //    {

    //    }
    //}
}

// OK
public sealed class SwapRedBlueColorTransformer : IColorTransformer
{
    // Vertauscht Rot und Blau abhängig vom 'amount'
    // 0 = Originalfarbe, 1 = R und B getauscht
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte r = channels[0]; // Original Rot
        byte g = channels[1]; // Original Grün
        byte b = channels[2]; // Original Blau
        byte a = channels[3]; // Alpha

        // Swap R und B: Interpolation
        byte new_r = (byte)(b * (float)amount + r * (1f - (float)amount));
        byte new_b = (byte)(r * (float)amount + b * (1f - (float)amount));

        // Grün bleibt gleich, wird aber ebenfalls interpoliert, falls gewünscht
        byte new_g = (byte)(g * (float)amount + g * (1f - (float)amount)); // vereinfacht: g

        return ColorUtils.AdaptTo(new_r, new_g, new_b, a);
    }
}

// OK
public sealed class SwapRedGreenColorTransformer : IColorTransformer
{
    // 0 = Originalfarbe, 1 = Rot und Grün vertauscht

    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte r = channels[0]; // Original Rot
        byte g = channels[1]; // Original Grün
        byte b = channels[2]; // Original Blau
        byte a = channels[3]; // Alpha

        // Interpolation: Rot und Grün vertauschen
        byte new_r = (byte)(r * (1f - (float)amount) + g * (float)amount);
        byte new_g = (byte)(g * (1f - (float)amount) + r * (float)amount);

        // Blau bleibt unverändert
        return ColorUtils.AdaptTo(new_r, new_g, b, a);
    }
}

// OK
public sealed class SwapGreenBlueColorTransformer : IColorTransformer
{
    // 0 = Originalfarbe, 1 = Grün und Blau vertauscht
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte r = channels[0]; // Original Rot
        byte g = channels[1]; // Original Grün
        byte b = channels[2]; // Original Blau
        byte a = channels[3]; // Alpha

        // Interpolation: Grün und Blau vertauschen
        byte new_g = (byte)(g * (1f - (float)amount) + b * (float)amount);
        byte new_b = (byte)(b * (1f - (float)amount) + g * (float)amount);

        // Rot bleibt unverändert
        return ColorUtils.AdaptTo(r, new_g, new_b, a);
    }
}

// OK
public sealed class RotateChannelsTransformer : IColorTransformer
{
    // 0 = Originalfarbe, 1 = R → G → B → R (Rotation der Kanäle)
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte r = channels[0]; // Original Rot
        byte g = channels[1]; // Original Grün
        byte b = channels[2]; // Original Blau
        byte a = channels[3]; // Alpha

        // Berechne den rotierenden Offset basierend auf 'amount'
        byte new_r = (byte)(r * (1f - (float)amount) + g * (float)amount);
        byte new_g = (byte)(g * (1f - (float)amount) + b * (float)amount);
        byte new_b = (byte)(b * (1f - (float)amount) + r * (float)amount);

        return ColorUtils.AdaptTo(new_r, new_g, new_b, a);
    }
}

// OK
public sealed class ColorBurnColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte ColorBurn(byte channel)
        {
            return (byte)Math.Max(0, channel - (255 - channel) * (float)amount);
        }

        return ColorUtils.AdaptTo(
            ColorBurn(channels[0]),
            ColorBurn(channels[1]),
            ColorBurn(channels[2]),
            channels[3]
        );
    }
}

// Bug (Amount = 0 nicht korrekt)
public sealed class ContrastColorTransformer : IFactorColorTransformer
{
    public float FactorMinimum => 1f;

    public float FactorMaximum => 10f;

    public Color Transform(Color color, ColorRange amount, float factor)
    {
        // Begrenzen des amount zwischen 0 (kein Kontrast) und 1 (maximaler Kontrast)
        var delta = ((float)amount + 0.5f) * factor;
        return Calculate(color, delta, factor);
    }

    public Color Transform(Color color, ColorRange amount)
    {
        return Transform(color, amount, 2f);
    }

    internal static Color Calculate(Color color, float amount, float factor)
    {
        // Kanäle extrahieren
        var channels = ColorUtils.AdaptFrom(color);

        // Berechnung des Mittelwerts (Grauwert) der RGB-Kanäle
        float average = (channels[0] + channels[1] + channels[2]) / 3f;

        // Berechnung der Kontrasteffekte
        byte Adjust(byte channel)
        {
            // Normiere den Kanal (Wert von 0 bis 255)
            float normalized = channel / 255f;

            // Berechne den neuen Wert mit Kontrast
            // Um maximalen Kontrast zu erzielen, verschieben wir den Farbwert stark in Richtung 0 oder 1
            float contrastAdjusted = (normalized - 0.5f) * amount + 0.5f;

            // Stellen Sie sicher, dass der Wert im Bereich von 0 bis 1 bleibt
            contrastAdjusted = Math.Clamp(contrastAdjusted, 0f, 1f);

            // Skaliere zurück auf den Bereich 0-255
            return (byte)(contrastAdjusted * 255f);
        }

        // Wende den Kontrast auf alle Kanäle an
        return ColorUtils.AdaptTo(
            Adjust(channels[0]),
            Adjust(channels[1]),
            Adjust(channels[2]),
            channels[3]
        );
    }
}

// OK
public sealed class HarmonyColorTransformer : IColorTransformer
{
    // Gegenteil von Kontrast

    public float FactorMinimum => 1f;

    public float FactorMaximum => 10f;

    public Color Transform(Color color, ColorRange amount)
    {
        // Begrenzen des amount zwischen 0 (kein Kontrast) und 1 (maximaler Kontrast)
        amount = new ColorRange(1f - amount);
        return ContrastColorTransformer.Calculate(color, amount, 1f);
    }
}

// Failed!!
//public sealed class DarkenColorTransformer : IColorTransformer
//{
//    public Color Transform(Color color, ColorRange amount)
//    {
//        var channels = ColorUtils.AdaptFrom(color);

//        // amount: 0 = keine Abdunkelung, 1 = maximale Abdunkelung (Schwarz)
//        amount = Math.Clamp(amount, 0f, 1f);

//        byte Darken(byte channel)
//        {
//            // Lineare Abdunkelung auf Basis des Amounts
//            float normalized = channel / 255f;
//            float adjusted = (float)Math.Pow(normalized, 1f + amount); // Sanftere Abdunkelung
//            return ColorUtils.Clamp(adjusted * 255f, 0f, 255f);
//        }

//        return ColorUtils.AdaptTo(
//            Darken(channels[0]),
//            Darken(channels[1]),
//            Darken(channels[2]),
//            channels[3]
//        );
//    }
//}

// OK
public sealed class DimColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);
        int delta = (int)(255f - 255f * (float)amount);

        byte Adjust(byte channel)
        {
            return (byte)((channel / 255f) * delta);
        }

        return ColorUtils.AdaptTo(
            Adjust(channels[0]),
            Adjust(channels[1]),
            Adjust(channels[2]),
            channels[3]
        );
    }
}

// OK
public sealed class DepthColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // amount: 0 = niedrigere Farbtiefe, 1 = normale Farbtiefe
        // Wir verwenden hier eine Skalierung auf ein bestimmtes Tiefenniveau

        // Berechne die Anzahl der Farbstufen für die reduzierte Farbtiefe
        int depth_steps = (int)(255f - (float)amount * 255f); // Bereich von 0 bis 255 abhängig von 'amount'

        // Funktion zum Anpassen der Farbtiefe eines Kanals
        byte Adjust(byte channel)
        {
            int adjusted = (int)(channel / 255f * depth_steps); // Werte auf die reduzierte Farbtiefe komprimieren
            return (byte)(adjusted * (255f / depth_steps)); // Zurückskalieren auf den ursprünglichen Bereich
        }

        return ColorUtils.AdaptTo(
            Adjust(channels[0]), // R
            Adjust(channels[1]), // G
            Adjust(channels[2]), // B
            channels[3] // Alpha bleibt unverändert
        );
    }
}

// OK
public sealed class GrayColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Grauwert berechnen nach Luminanzformel
        int gray = (int)(channels[0] * 0.3f + channels[1] * 0.59f + channels[2] * 0.11f);

        // Interpolation zwischen Originalfarbe und Grauwert
        byte Blend(byte original, int gray_value) =>
            (byte)Math.Round(original + (gray_value - original) * (float)amount);

        return ColorUtils.AdaptTo(
            Blend(channels[0], gray),
            Blend(channels[1], gray),
            Blend(channels[2], gray),
            channels[3]
        );
    }
}

// OK
public sealed class HueColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        var hsv = new Spaces.HSV(color);
        hsv.HueComponent = new ColorRange(((float)hsv.HueComponent.Value + (float)amount) % 1f);

        return hsv.ConvertTo();
    }
}

// OK
public sealed class SaturationColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);
        var hsv = new Spaces.HSV(color);
        var min_saturation = hsv.SaturationComponent; // Standardwert

        hsv.SaturationComponent = new ColorRange((float)min_saturation + (float)amount);

        return hsv.ConvertTo();
    }
}

// OK
public sealed class DesaturationColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);
        var hsv = new Spaces.HSV(color);
        var max_saturation = hsv.SaturationComponent; // Standardwert

        hsv.SaturationComponent = new ColorRange((float)max_saturation - (float)amount);

        return hsv.ConvertTo();
    }
}

// OK
public sealed class NegativeColorTransformer : IColorTransformer
{
    // Invert

    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Invertierte Farbe berechnen
        byte Invert(byte original) =>
            (byte)(255 - original);  // Direkte Invertierung

        // Interpolation zwischen Original und invertierter Farbe
        byte Mix(byte original) =>
            (byte)(original * (1f - (float)amount) + Invert(original) * (float)amount);

        return ColorUtils.AdaptTo(
            Mix(channels[0]),
            Mix(channels[1]),
            Mix(channels[2]),
            channels[3]
        );
    }
}


// OK
public sealed class PosterizeColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Berechnen des Delta-Werts und der Schritte basierend auf amount
        var delta = 1f - amount;
        int steps = ColorUtils.Clamp((int)(delta * 16), 2, 256);  // Mindestens 2 Stufen für die Posterization
        float step_size = 255f / (steps - 1);

        byte Posterize(byte channel)
        {
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

// OK
public sealed class SolarizeColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte Solarize(byte channel)
        {
            // Keine Änderung bei amount = 0
            if (amount == 0f)
                return channel;

            // Solarisation ab einem Helligkeitswert von 128
            if (channel < 128)
                return channel;

            // Teilweise invertieren, je nach amount
            float t = (channel - 128f) / 127f; // Bereich von 0 bis 1
            float inverted = 255f - (127f * t); // klassische Invertierung ab 128
            float mixed = channel * (1f - (float)amount) + inverted * amount;

            return (byte)Math.Clamp(Math.Round(mixed), 0, 255);
        }

        return ColorUtils.AdaptTo(
            Solarize(channels[0]),
            Solarize(channels[1]),
            Solarize(channels[2]),
            channels[3]
        );
    }
}

// OK
public sealed class ColdColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte r = (byte)(channels[0] * (1f - 0.5f * (float)amount)); // Reduzieren von Rot
        byte b = (byte)Math.Clamp(channels[2] + (float)amount * 100f, 0f, 255f); // Erhöhen von Blau

        return ColorUtils.AdaptTo(
            r,
            channels[1],
            b,
            channels[3]
        );
    }
}

// OK
public sealed class HotColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        byte b = (byte)(channels[2] * (1f - 0.5f * (float)amount)); // Reduzieren von Blau
        byte r = (byte)Math.Clamp(channels[0] + (float)amount * 100f, 0f, 255f); // Erhöhen von Rot

        return ColorUtils.AdaptTo(
            r,
            channels[1],
            b,
            channels[3]
        );
    }
}

// OK => Verschiebe zu Adjustment 
//public sealed class TemperatureColorTransformer : IColorTransformer
//{
//    public Color Transform(Color color, float temperatureAmount)
//    {
//        var channels = ColorUtils.AdaptFrom(color);

//        // temperatureAmount: 
//        // 0 = kälter (blau), 
//        // 1 = neutral (keine Änderung), 
//        // 2 = wärmer (rot)

//        // Angenommene Temperaturwerte zwischen 0 und 2
//        // 0 -> kühler (blauer)
//        // 1 -> neutral
//        // 2 -> wärmer (roter)

//        // Berechne die Anpassung für Rot und Blau
//        byte AdjustRed(byte r, ColorRange amount)
//        {
//            // Wenn es kühler werden soll, den Rotanteil verringern
//            if (amount < 1.0f)
//                return (byte)(r * (1 - amount * 0.5f)); // Je näher 0, desto weniger rot
//            return r;
//        }

//        byte AdjustBlue(byte b, ColorRange amount)
//        {
//            // Wenn es wärmer werden soll, den Blauanteil verringern
//            if (amount > 1.0f)
//                return (byte)(b * (1 - (amount - 1) * 0.5f)); // Je näher 2, desto weniger blau
//            return b;
//        }

//        byte r = AdjustRed(channels[0], temperatureAmount + 0.5f);
//        byte b = AdjustBlue(channels[2], temperatureAmount + 0.5f);

//        return ColorUtils.AdaptTo(
//            r,
//            channels[1],
//            b,
//            channels[3]
//        );
//    }
//}
//
//

// OK
public sealed class MidToneColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Betrag von amount zwischen 0 und 1 für feinere Kontrolle
        //amount = Math.Clamp(amount, 0f, 1f);

        byte Adjust(byte channel)
        {
            if (channel < 128)
            {
                // Abgedunkelte Bereiche (0-127) werden weiter abgedunkelt
                return ColorUtils.Clamp(channel - (byte)((float)amount * (128f - channel)), 0f, 255f);
            }
            else
            {
                // Helle Bereiche (128-255) werden aufgehellt
                return ColorUtils.Clamp(channel + (byte)((float)amount * (255f - channel)), 0f, 255f);
            }
        }

        return ColorUtils.AdaptTo(
            Adjust(channels[0]),
            Adjust(channels[1]),
            Adjust(channels[2]),
            channels[3] // Beibehaltung der Transparenz (Alpha)
        );
    }
}

// OK
public sealed class InvertedMidToneColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Dynamische Anpassung des Kanals (0-127 -> heller, 128-255 -> dunkler)
        byte Adjust(byte channel)
        {
            if (channel <= 127)
                return (byte)(channel + (255f - channel) * (float)amount);  // Heller machen (bei 0 wird maximal heller)
            else
                return (byte)(channel - (channel - 128f) * (float)amount);  // Dunkler machen (bei 255 wird maximal dunkler)
        }

        return ColorUtils.AdaptTo(
            Adjust(channels[0]), // Red
            Adjust(channels[1]), // Green
            Adjust(channels[2]), // Blue
            channels[3]          // Alpha
        );
    }
}

// OK, aber Name unklar
public sealed class ToneExposureColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        var channels = ColorUtils.AdaptFrom(color);

        // Dynamische Anpassung des Kanals (0-127 -> heller, 128-255 -> dunkler)
        byte Adjust(byte channel)
        {
            if (channel <= (255f * amount))
                return (byte)(channel + (255f - channel) * (float)amount);  // Heller machen (bei 0 wird maximal heller)
            else
                return (byte)(channel - (channel - 128f) * (float)amount);  // Dunkler machen (bei 255 wird maximal dunkler)
        }

        return ColorUtils.AdaptTo(
            Adjust(channels[0]), // Red
            Adjust(channels[1]), // Green
            Adjust(channels[2]), // Blue
            channels[3]          // Alpha
        );
    }
}

// OK
public sealed class SepiaColorTransformer : IColorTransformer
{
    public Color Transform(Color color, ColorRange amount)
    {
        // Konvertiere die Ausgangsfarbe in RGB
        var originalColor = ColorUtils.AdaptFrom(color);
        // Sepia Tone RGB 112, 66, 20

        // Berechne den gemischten Wert basierend auf `amount`
        var r = BlendChannel(originalColor[0], 112, amount);
        var g = BlendChannel(originalColor[1], 66, amount);
        var b = BlendChannel(originalColor[2], 20, amount);

        // Die Alpha-Komponente bleibt unverändert
        byte a = originalColor[3];

        // Rückgabe der neuen Farbe mit Sepia-Ton
        return ColorUtils.AdaptTo(r, g, b, a);
    }

    // Hilfsmethode zur Berechnung des gemischten Werts für eine Farbe (R, G, B)
    private byte BlendChannel(byte originalValue, byte sepiaValue, ColorRange amount)
    {
        // Mische die ursprüngliche Farbe mit dem Sepia-Wert
        return ColorUtils.Clamp((int)((originalValue * (1 - (float)amount)) + (sepiaValue * (float)amount)));
    }
}

// OK
public sealed class ExposureColorTransformer : IFactorColorTransformer
{
    public float FactorMinimum => 1f;

    public float FactorMaximum => 10f;

    public Color Transform(Color color, ColorRange amount, float factor)
    {
        var a1 = ColorUtils.AdaptFrom(color); // Die Eingabefarbe

        // Bestimme den Belichtungsfaktor (Amount)
        float exposure = 1f + ((factor - FactorMinimum) * (float)amount) + (float)amount; // Je höher der Wert, desto mehr wird die Belichtung erhöht

        // Berechne die neue RGB-Werte, indem der Belichtungsfaktor angewendet wird
        byte r = ColorUtils.Clamp((int)(a1[0] * exposure));
        byte g = ColorUtils.Clamp((int)(a1[1] * exposure));
        byte b = ColorUtils.Clamp((int)(a1[2] * exposure));
        byte a = a1[3]; // Behalte die Alpha-Komponente bei

        // Rückgabe der belichteten Farbe
        return ColorUtils.AdaptTo(r, g, b, a);
    }

    public Color Transform(Color color, ColorRange amount)
    {
        return Transform(color, amount, 1f);
    }
}

// OK
public sealed class DarknessColorTransformer : IFactorColorTransformer
{
    public float FactorMinimum => 0f;

    public float FactorMaximum => 1f;

    public Color Transform(Color color, ColorRange amount, float factor)
    {
        var a1 = ColorUtils.AdaptFrom(color); // Die Eingabefarbe

        // Bestimme den Dunkelheitsfaktor (Amount)
        float darkness = 1f - ((factor - FactorMinimum) * (float)amount); // Je höher der Wert, desto dunkler wird die Farbe

        // Berechne die neue RGB-Werte, indem der Dunkelheitsfaktor angewendet wird
        byte r = ColorUtils.Clamp((int)(a1[0] * darkness));
        byte g = ColorUtils.Clamp((int)(a1[1] * darkness));
        byte b = ColorUtils.Clamp((int)(a1[2] * darkness));
        byte a = a1[3]; // Behalte die Alpha-Komponente bei

        // Rückgabe der dunkleren Farbe
        return ColorUtils.AdaptTo(r, g, b, a);
    }

    public Color Transform(Color color, ColorRange amount)
    {
        return Transform(color, amount, 1f);
    }
}

// OK
public sealed class ShadowColorTransformer : IFactorColorTransformer
{
    public float FactorMinimum => 0f;
    public float FactorMaximum => 10f;

    public Color Transform(Color color, ColorRange amount, float factor)
    {
        var a1 = ColorUtils.AdaptFrom(color);

        // Stärke des Schattens: dunkle Bereiche werden stärker beeinflusst
        float shadow_strength = (float)amount * factor;

        // Formel: Je dunkler der Kanal, desto stärker die Abdunklung
        byte r = ColorUtils.Clamp((int)(a1[0] * (1f - shadow_strength * (1f - a1[0] / 255f))));
        byte g = ColorUtils.Clamp((int)(a1[1] * (1f - shadow_strength * (1f - a1[1] / 255f))));
        byte b = ColorUtils.Clamp((int)(a1[2] * (1f - shadow_strength * (1f - a1[2] / 255f))));
        byte a = a1[3];

        return ColorUtils.AdaptTo(r, g, b, a);
    }

    public Color Transform(Color color, ColorRange amount)
    {
        return Transform(color, amount, 1f);
    }
}

