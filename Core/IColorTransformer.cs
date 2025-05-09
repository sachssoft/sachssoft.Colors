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

// Werte nur zwischen 0 und 1

public interface IColorTransformer
{
    Color Transform(Color color, ColorRange amount);

}

public interface IColorChannelTransformer
{
    Color Transform<T>(Color color, T channel_amount) where T : struct, IColorSpace;
}

public interface IFactorColorTransformer : IColorTransformer
{
    float FactorMinimum { get; }

    float FactorMaximum { get; }

    Color Transform(Color color, ColorRange amount, float factor);

}
