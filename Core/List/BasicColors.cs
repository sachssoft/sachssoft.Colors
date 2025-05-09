// © Tobias Sachs
// 22.01.2023
// Updated 08.05.2025

using System.ComponentModel;

#if SASOGINE
using Microsoft.Xna.Framework;
namespace sachssoft.Sasogine.Graphics.Colors.List;
#elif MONOGAME
using Microsoft.Xna.Framework;
namespace sachssoft.Monogame.Colors.List;
#elif AVALONIA
using Ava = Avalonia;
namespace sachssoft.Avalonia.Colors.List;
using Color = Ava.Media.Color;
#elif DRAWING
namespace sachssoft.Drawing.Colors.List;
using Color = System.Drawing.Color;
#elif WPF
namespace sachssoft.WPF.Colors.List;
using Color = System.Windows.Media.Color;
#elif SKIA
namespace sachssoft.Skia.Colors.List;
using Color = SkiaSharp.SKColor;
#else
namespace sachssoft.Colors.List;
using Color = sachssoft.Colors.ColorCode;
#endif

// https://www.w3.org/wiki/CSS/Properties/color/keywords
// https://en.wikipedia.org/wiki/Web_colors

[Description("Basic Colors")]
public sealed class BasicColors : ColorNameEnumerator
{
    public static ColorName Fuchsia { get => new("Fuchsia", ColorUtils.AdaptTo(255, 0, 255), ""); }
    public static ColorName Purple { get => new("Purple", ColorUtils.AdaptTo(128, 0, 128), ""); }
    public static ColorName Blue { get => new("BlueComponent", ColorUtils.AdaptTo(0, 0, 255), ""); }
    public static ColorName Navy { get => new("Navy", ColorUtils.AdaptTo(0, 0, 128), ""); }
    public static ColorName Aqua { get => new("Aqua", ColorUtils.AdaptTo(0, 255, 255), ""); }
    public static ColorName Teal { get => new("Teal", ColorUtils.AdaptTo(0, 128, 128), ""); }
    public static ColorName Lime { get => new("Lime", ColorUtils.AdaptTo(0, 255, 0), ""); }
    public static ColorName Green { get => new("GreenComponent", ColorUtils.AdaptTo(0, 128, 0), ""); }
    public static ColorName Olive { get => new("Olive", ColorUtils.AdaptTo(128, 128, 0), ""); }
    public static ColorName Yellow { get => new("Yellow", ColorUtils.AdaptTo(255, 255, 0), ""); }
    public static ColorName Red { get => new("RedComponent", ColorUtils.AdaptTo(255, 0, 0), ""); }
    public static ColorName Maroon { get => new("Maroon", ColorUtils.AdaptTo(128, 0, 0), ""); }
    public static ColorName Silver { get => new("Silver", ColorUtils.AdaptTo(192, 192, 192), ""); }
    public static ColorName Gray { get => new("Gray", ColorUtils.AdaptTo(128, 128, 128), ""); }
    public static ColorName White { get => new("WhiteComponent", ColorUtils.AdaptTo(255, 255, 255), ""); }
    public static ColorName Black { get => new("Black", ColorUtils.AdaptTo(0, 0, 0), ""); }
}