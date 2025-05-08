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

public static class ColorExtensions
{

    public static Color Adjust<T>(this Color color, float amount) where T : IColorTransformer, new()
    {
        var transformer = new T();
        return transformer.Transform(color, amount);
    }

    public static Color Adjust<T>(this Color color, float amount, float factor) where T : IFactorColorTransformer, new()
    {
        var transformer = new T();
        return transformer.Transform(color, amount);
    }

    public static Color Blend<T>(this Color color, Color other, float amount) where T : IColorBlender, new()
    {
        var blender = new T();
        return blender.Blend(color, other, amount);
    }

    public static Color FromSpace<T>(this T space) where T : struct, IColorSpace
    {
        return space.ConvertTo();
    }

    public static T ToSpace<T>(this Color color) where T : struct, IColorSpace
    {
        var space = new T();
        space.ConvertFrom(color);
        return space;
    }

    public static TOutput Transform<TInput, TOutput>(this TInput space) where TInput : struct, IColorSpace where TOutput : struct, IColorSpace
    {
        var output = new TOutput();
        output.ConvertFrom(space.ConvertTo());
        return output;
    }

}