// (C) Tobias Sachs
// Added at 2025-09-05

using System;
using System.Collections.Generic;
using System.Numerics;


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

public sealed class ColorTransformerFactory
{
    private Color? _color;
    private readonly List<Func<Color, Color>> _tasks;

    private ColorTransformerFactory()
    {
        _tasks = new List<Func<Color, Color>>();
    }

    public static ColorTransformerFactory Create()
    {
        return new ColorTransformerFactory();
    }

    public ColorTransformerFactory Add<T>(ColorRange amount) where T : IColorTransformer, new()
    {
        _tasks.Add((c) =>
        {
            return c.Adjust<T>(amount);
        });
        return this;
    }

    public ColorTransformerFactory Add<T>(ColorRange amount, float factor) where T : IFactorColorTransformer, new()
    {
        _tasks.Add((c) =>
        {
            return c.Adjust<T>(amount, factor);
        });
        return this;
    }

    public ColorTransformerFactory Add(ColorRange amount, IFactorColorTransformer transformer)
    {
        _tasks.Add((c) =>
        {
            return c.Adjust(amount, transformer);
        });
        return this;
    }

    public Color Transform(Color color)
    {
        var output = color;
        foreach (var task in _tasks)
        {
            output = task.Invoke(output);
        }
        return output;
    }
}
