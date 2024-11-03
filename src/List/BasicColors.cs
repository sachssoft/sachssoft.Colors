/// © Tobias Sachs
/// Colors/HTMLColors.cs
/// 22.01.2023

using System.ComponentModel;

// https://www.w3.org/wiki/CSS/Properties/color/keywords
// https://en.wikipedia.org/wiki/Web_colors

namespace tcs.Colors;

[Description("Basic Colors")]
public sealed class BasicColors : ColorNameEnumerator
{
    public static ColorName Fuchsia { get => new("Fuchsia", new ColorCode(255, 0, 255), ""); }
    public static ColorName Purple { get => new("Purple", new ColorCode(128, 0, 128), ""); }
    public static ColorName Blue { get => new("Blue", new ColorCode(0, 0, 255), ""); }
    public static ColorName Navy { get => new("Navy", new ColorCode(0, 0, 128), ""); }
    public static ColorName Aqua { get => new("Aqua", new ColorCode(0, 255, 255), ""); }
    public static ColorName Teal { get => new("Teal", new ColorCode(0, 128, 128), ""); }
    public static ColorName Lime { get => new("Lime", new ColorCode(0, 255, 0), ""); }
    public static ColorName Green { get => new("Green", new ColorCode(0, 128, 0), ""); }
    public static ColorName Olive { get => new("Olive", new ColorCode(128, 128, 0), ""); }
    public static ColorName Yellow { get => new("Yellow", new ColorCode(255, 255, 0), ""); }
    public static ColorName Red { get => new("Red", new ColorCode(255, 0, 0), ""); }
    public static ColorName Maroon { get => new("Maroon", new ColorCode(128, 0, 0), ""); }
    public static ColorName Silver { get => new("Silver", new ColorCode(192, 192, 192), ""); }
    public static ColorName Gray { get => new("Gray", new ColorCode(128, 128, 128), ""); }
    public static ColorName White { get => new("White", new ColorCode(255, 255, 255), ""); }
    public static ColorName Black { get => new("Black", new ColorCode(0, 0, 0), ""); }
}