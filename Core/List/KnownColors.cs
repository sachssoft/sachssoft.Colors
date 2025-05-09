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

// Equivalent to WPF, WinForms, HTML and CSS Colors
[Description("Known Colors")]
public sealed class KnownColors : ColorNameEnumerator
{
    public static ColorName AliceBlue { get => new("Alice BlueComponent", ColorUtils.AdaptTo(240, 248, 255), ""); }
    public static ColorName AntiqueWhite { get => new("Antique WhiteComponent", ColorUtils.AdaptTo(250, 235, 215), ""); }
    public static ColorName Aqua { get => new("Aqua", ColorUtils.AdaptTo(0, 255, 255), ""); }
    public static ColorName Aquamarine { get => new("Aquamarine", ColorUtils.AdaptTo(127, 255, 212), ""); }
    public static ColorName Azure { get => new("Azure", ColorUtils.AdaptTo(240, 255, 255), ""); }
    public static ColorName Beige { get => new("Beige", ColorUtils.AdaptTo(245, 245, 220), ""); }
    public static ColorName Bisque { get => new("Bisque", ColorUtils.AdaptTo(255, 228, 196), ""); }
    public static ColorName Black { get => new("Black", ColorUtils.AdaptTo(0, 0, 0), ""); }
    public static ColorName BlanchedAlmond { get => new("Blanched Almond", ColorUtils.AdaptTo(255, 235, 205), ""); }
    public static ColorName Blue { get => new("BlueComponent", ColorUtils.AdaptTo(0, 0, 255), ""); }
    public static ColorName BlueViolet { get => new("BlueComponent Violet", ColorUtils.AdaptTo(138, 43, 226), ""); }
    public static ColorName Brown { get => new("Brown", ColorUtils.AdaptTo(165, 42, 42), ""); }
    public static ColorName BurlyWood { get => new("Burly Wood", ColorUtils.AdaptTo(222, 184, 135), ""); }
    public static ColorName CadetBlue { get => new("Cadet BlueComponent", ColorUtils.AdaptTo(95, 158, 160), ""); }
    public static ColorName Chartreuse { get => new("Chartreuse", ColorUtils.AdaptTo(127, 255, 0), ""); }
    public static ColorName Chocolate { get => new("Chocolate", ColorUtils.AdaptTo(210, 105, 30), ""); }
    public static ColorName Coral { get => new("Coral", ColorUtils.AdaptTo(255, 127, 80), ""); }
    public static ColorName CornflowerBlue { get => new("Cornflower BlueComponent", ColorUtils.AdaptTo(100, 149, 237), ""); }
    public static ColorName Cornsilk { get => new("Cornsilk", ColorUtils.AdaptTo(255, 248, 220), ""); }
    public static ColorName Crimson { get => new("Crimson", ColorUtils.AdaptTo(220, 20, 60), ""); }
    public static ColorName Cyan { get => new("Cyan", ColorUtils.AdaptTo(0, 255, 255), ""); }
    public static ColorName DarkBlue { get => new("Dark BlueComponent", ColorUtils.AdaptTo(0, 0, 139), ""); }
    public static ColorName DarkCyan { get => new("Dark Cyan", ColorUtils.AdaptTo(0, 139, 139), ""); }
    public static ColorName DarkGoldenrod { get => new("Dark Goldenrod", ColorUtils.AdaptTo(184, 134, 11), ""); }
    public static ColorName DarkGray { get => new("Dark Gray", ColorUtils.AdaptTo(169, 169, 169), ""); }
    public static ColorName DarkGreen { get => new("Dark GreenComponent", ColorUtils.AdaptTo(0, 100, 0), ""); }
    public static ColorName DarkKhaki { get => new("Dark Khaki", ColorUtils.AdaptTo(189, 183, 107), ""); }
    public static ColorName DarkMagenta { get => new("Dark Magenta", ColorUtils.AdaptTo(139, 0, 139), ""); }
    public static ColorName DarkOliveGreen { get => new("Dark Olive GreenComponent", ColorUtils.AdaptTo(85, 107, 47), ""); }
    public static ColorName DarkOrange { get => new("Dark Orange", ColorUtils.AdaptTo(255, 140, 0), ""); }
    public static ColorName DarkOrchid { get => new("Dark Orchid", ColorUtils.AdaptTo(153, 50, 204), ""); }
    public static ColorName DarkRed { get => new("Dark RedComponent", ColorUtils.AdaptTo(139, 0, 0), ""); }
    public static ColorName DarkSalmon { get => new("Dark Salmon", ColorUtils.AdaptTo(233, 150, 122), ""); }
    public static ColorName DarkSeaGreen { get => new("Dark Sea GreenComponent", ColorUtils.AdaptTo(143, 188, 143), ""); }
    public static ColorName DarkSlateBlue { get => new("Dark Slate BlueComponent", ColorUtils.AdaptTo(72, 61, 139), ""); }
    public static ColorName DarkSlateGray { get => new("Dark Slate Gray", ColorUtils.AdaptTo(47, 79, 79), ""); }
    public static ColorName DarkTurquoise { get => new("Dark Turquoise", ColorUtils.AdaptTo(0, 206, 209), ""); }
    public static ColorName DarkViolet { get => new("Dark Violet", ColorUtils.AdaptTo(148, 0, 211), ""); }
    public static ColorName DeepPink { get => new("Deep Pink", ColorUtils.AdaptTo(255, 20, 147), ""); }
    public static ColorName DeepSkyBlue { get => new("Deep Sky BlueComponent", ColorUtils.AdaptTo(0, 191, 255), ""); }
    public static ColorName DimGray { get => new("Dim Gray", ColorUtils.AdaptTo(105, 105, 105), ""); }
    public static ColorName DodgerBlue { get => new("Dodger BlueComponent", ColorUtils.AdaptTo(30, 144, 255), ""); }
    public static ColorName Firebrick { get => new("Firebrick", ColorUtils.AdaptTo(178, 34, 34), ""); }
    public static ColorName FloralWhite { get => new("Floral WhiteComponent", ColorUtils.AdaptTo(255, 250, 240), ""); }
    public static ColorName ForestGreen { get => new("Forest GreenComponent", ColorUtils.AdaptTo(34, 139, 34), ""); }
    public static ColorName Fuchsia { get => new("Fuchsia", ColorUtils.AdaptTo(255, 0, 255), ""); }
    public static ColorName Gainsboro { get => new("Gainsboro", ColorUtils.AdaptTo(220, 220, 220), ""); }
    public static ColorName GhostWhite { get => new("Ghost WhiteComponent", ColorUtils.AdaptTo(248, 248, 255), ""); }
    public static ColorName Gold { get => new("Gold", ColorUtils.AdaptTo(255, 215, 0), ""); }
    public static ColorName Goldenrod { get => new("Goldenrod", ColorUtils.AdaptTo(218, 165, 32), ""); }
    public static ColorName Gray { get => new("Gray", ColorUtils.AdaptTo(128, 128, 128), ""); }
    public static ColorName Green { get => new("GreenComponent", ColorUtils.AdaptTo(0, 128, 0), ""); }
    public static ColorName GreenYellow { get => new("GreenComponent Yellow", ColorUtils.AdaptTo(173, 255, 47), ""); }
    public static ColorName Honeydew { get => new("Honeydew", ColorUtils.AdaptTo(240, 255, 240), ""); }
    public static ColorName HotPink { get => new("Hot Pink", ColorUtils.AdaptTo(255, 105, 180), ""); }
    public static ColorName IndianRed { get => new("Indian RedComponent", ColorUtils.AdaptTo(205, 92, 92), ""); }
    public static ColorName Indigo { get => new("Indigo", ColorUtils.AdaptTo(75, 0, 130), ""); }
    public static ColorName Ivory { get => new("Ivory", ColorUtils.AdaptTo(255, 255, 240), ""); }
    public static ColorName Khaki { get => new("Khaki", ColorUtils.AdaptTo(240, 230, 140), ""); }
    public static ColorName Lavender { get => new("Lavender", ColorUtils.AdaptTo(230, 230, 250), ""); }
    public static ColorName LavenderBlush { get => new("Lavender Blush", ColorUtils.AdaptTo(255, 240, 245), ""); }
    public static ColorName LawnGreen { get => new("Lawn GreenComponent", ColorUtils.AdaptTo(124, 252, 0), ""); }
    public static ColorName LemonChiffon { get => new("Lemon Chiffon", ColorUtils.AdaptTo(255, 250, 205), ""); }
    public static ColorName LightBlue { get => new("Light BlueComponent", ColorUtils.AdaptTo(173, 216, 230), ""); }
    public static ColorName LightCoral { get => new("Light Coral", ColorUtils.AdaptTo(240, 128, 128), ""); }
    public static ColorName LightCyan { get => new("Light Cyan", ColorUtils.AdaptTo(224, 255, 255), ""); }
    public static ColorName LightGoldenrodYellow { get => new("Light Goldenrod Yellow", ColorUtils.AdaptTo(250, 250, 210), ""); }
    public static ColorName LightGray { get => new("Light Gray", ColorUtils.AdaptTo(211, 211, 211), ""); }
    public static ColorName LightGreen { get => new("Light GreenComponent", ColorUtils.AdaptTo(144, 238, 144), ""); }
    public static ColorName LightPink { get => new("Light Pink", ColorUtils.AdaptTo(255, 182, 193), ""); }
    public static ColorName LightSalmon { get => new("Light Salmon", ColorUtils.AdaptTo(255, 160, 122), ""); }
    public static ColorName LightSeaGreen { get => new("Light Sea GreenComponent", ColorUtils.AdaptTo(32, 178, 170), ""); }
    public static ColorName LightSkyBlue { get => new("Light Sky BlueComponent", ColorUtils.AdaptTo(135, 206, 250), ""); }
    public static ColorName LightSlateGray { get => new("Light Slate Gray", ColorUtils.AdaptTo(119, 136, 153), ""); }
    public static ColorName LightSteelBlue { get => new("Light Steel BlueComponent", ColorUtils.AdaptTo(176, 196, 222), ""); }
    public static ColorName LightYellow { get => new("Light Yellow", ColorUtils.AdaptTo(255, 255, 224), ""); }
    public static ColorName Lime { get => new("Lime", ColorUtils.AdaptTo(0, 255, 0), ""); }
    public static ColorName LimeGreen { get => new("Lime GreenComponent", ColorUtils.AdaptTo(50, 205, 50), ""); }
    public static ColorName Linen { get => new("Linen", ColorUtils.AdaptTo(250, 240, 230), ""); }
    public static ColorName Magenta { get => new("Magenta", ColorUtils.AdaptTo(255, 0, 255), ""); }
    public static ColorName Maroon { get => new("Maroon", ColorUtils.AdaptTo(128, 0, 0), ""); }
    public static ColorName MediumAquamarine { get => new("Medium Aquamarine", ColorUtils.AdaptTo(102, 205, 170), ""); }
    public static ColorName MediumBlue { get => new("Medium BlueComponent", ColorUtils.AdaptTo(0, 0, 205), ""); }
    public static ColorName MediumOrchid { get => new("Medium Orchid", ColorUtils.AdaptTo(186, 85, 211), ""); }
    public static ColorName MediumPurple { get => new("Medium Purple", ColorUtils.AdaptTo(147, 112, 219), ""); }
    public static ColorName MediumSeaGreen { get => new("Medium Sea GreenComponent", ColorUtils.AdaptTo(60, 179, 113), ""); }
    public static ColorName MediumSlateBlue { get => new("Medium Slate BlueComponent", ColorUtils.AdaptTo(123, 104, 238), ""); }
    public static ColorName MediumSpringGreen { get => new("Medium Spring GreenComponent", ColorUtils.AdaptTo(0, 250, 154), ""); }
    public static ColorName MediumTurquoise { get => new("Medium Turquoise", ColorUtils.AdaptTo(72, 209, 204), ""); }
    public static ColorName MediumVioletRed { get => new("Medium VioletRed", ColorUtils.AdaptTo(199, 21, 133), ""); }
    public static ColorName MidnightBlue { get => new("Midnight BlueComponent", ColorUtils.AdaptTo(25, 25, 112), ""); }
    public static ColorName MintCream { get => new("Mint Cream", ColorUtils.AdaptTo(245, 255, 250), ""); }
    public static ColorName MistyRose { get => new("Misty Rose", ColorUtils.AdaptTo(255, 228, 225), ""); }
    public static ColorName Moccasin { get => new("Moccasin", ColorUtils.AdaptTo(255, 228, 181), ""); }
    public static ColorName NavajoWhite { get => new("Navajo WhiteComponent", ColorUtils.AdaptTo(255, 222, 173), ""); }
    public static ColorName Navy { get => new("Navy", ColorUtils.AdaptTo(0, 0, 128), ""); }
    public static ColorName OldLace { get => new("Old Lace", ColorUtils.AdaptTo(253, 245, 230), ""); }
    public static ColorName Olive { get => new("Olive", ColorUtils.AdaptTo(128, 128, 0), ""); }
    public static ColorName OliveDrab { get => new("Olive Drab", ColorUtils.AdaptTo(107, 142, 35), ""); }
    public static ColorName Orange { get => new("Orange", ColorUtils.AdaptTo(255, 165, 0), ""); }
    public static ColorName OrangeRed { get => new("Orange RedComponent", ColorUtils.AdaptTo(255, 69, 0), ""); }
    public static ColorName Orchid { get => new("Orchid", ColorUtils.AdaptTo(218, 112, 214), ""); }
    public static ColorName PaleGoldenrod { get => new("Pale Goldenrod", ColorUtils.AdaptTo(238, 232, 170), ""); }
    public static ColorName PaleGreen { get => new("Pale GreenComponent", ColorUtils.AdaptTo(152, 251, 152), ""); }
    public static ColorName PaleTurquoise { get => new("Pale Turquoise", ColorUtils.AdaptTo(175, 238, 238), ""); }
    public static ColorName PaleVioletRed { get => new("Pale Violet RedComponent", ColorUtils.AdaptTo(219, 112, 147), ""); }
    public static ColorName PapayaWhip { get => new("Papaya Whip", ColorUtils.AdaptTo(255, 239, 213), ""); }
    public static ColorName PeachPuff { get => new("Peach Puff", ColorUtils.AdaptTo(255, 218, 185), ""); }
    public static ColorName Peru { get => new("Peru", ColorUtils.AdaptTo(205, 133, 63), ""); }
    public static ColorName Pink { get => new("Pink", ColorUtils.AdaptTo(255, 192, 203), ""); }
    public static ColorName Plum { get => new("Plum", ColorUtils.AdaptTo(221, 160, 221), ""); }
    public static ColorName PowderBlue { get => new("Powder BlueComponent", ColorUtils.AdaptTo(176, 224, 230), ""); }
    public static ColorName Purple { get => new("Purple", ColorUtils.AdaptTo(128, 0, 128), ""); }
    public static ColorName Red { get => new("RedComponent", ColorUtils.AdaptTo(255, 0, 0), ""); }
    public static ColorName RosyBrown { get => new("Rosy Brown", ColorUtils.AdaptTo(188, 143, 143), ""); }
    public static ColorName RoyalBlue { get => new("Royal BlueComponent", ColorUtils.AdaptTo(65, 105, 225), ""); }
    public static ColorName SaddleBrown { get => new("Saddle Brown", ColorUtils.AdaptTo(139, 69, 19), ""); }
    public static ColorName Salmon { get => new("Salmon", ColorUtils.AdaptTo(250, 128, 114), ""); }
    public static ColorName SandyBrown { get => new("Sandy Brown", ColorUtils.AdaptTo(244, 164, 96), ""); }
    public static ColorName SeaGreen { get => new("Sea GreenComponent", ColorUtils.AdaptTo(46, 139, 87), ""); }
    public static ColorName SeaShell { get => new("Sea Shell", ColorUtils.AdaptTo(255, 245, 238), ""); }
    public static ColorName Sienna { get => new("Sienna", ColorUtils.AdaptTo(160, 82, 45), ""); }
    public static ColorName Silver { get => new("Silver", ColorUtils.AdaptTo(192, 192, 192), ""); }
    public static ColorName SkyBlue { get => new("Sky BlueComponent", ColorUtils.AdaptTo(135, 206, 235), ""); }
    public static ColorName SlateBlue { get => new("Slate BlueComponent", ColorUtils.AdaptTo(106, 90, 205), ""); }
    public static ColorName SlateGray { get => new("Slate Gray", ColorUtils.AdaptTo(112, 128, 144), ""); }
    public static ColorName Snow { get => new("Snow", ColorUtils.AdaptTo(255, 250, 250), ""); }
    public static ColorName SpringGreen { get => new("Spring GreenComponent", ColorUtils.AdaptTo(0, 255, 127), ""); }
    public static ColorName SteelBlue { get => new("Steel BlueComponent", ColorUtils.AdaptTo(70, 130, 180), ""); }
    public static ColorName Tan { get => new("Tan", ColorUtils.AdaptTo(210, 180, 140), ""); }
    public static ColorName Teal { get => new("Teal", ColorUtils.AdaptTo(0, 128, 128), ""); }
    public static ColorName Thistle { get => new("Thistle", ColorUtils.AdaptTo(216, 191, 216), ""); }
    public static ColorName Tomato { get => new("Tomato", ColorUtils.AdaptTo(255, 99, 71), ""); }
    public static ColorName Transparent { get => new("Transparent", ColorUtils.AdaptTo(0, 0, 0, 0), ""); }
    public static ColorName Turquoise { get => new("Turquoise", ColorUtils.AdaptTo(64, 224, 208), ""); }
    public static ColorName Violet { get => new("Violet", ColorUtils.AdaptTo(238, 130, 238), ""); }
    public static ColorName Wheat { get => new("Wheat", ColorUtils.AdaptTo(245, 222, 179), ""); }
    public static ColorName White { get => new("WhiteComponent", ColorUtils.AdaptTo(255, 255, 255), ""); }
    public static ColorName WhiteSmoke { get => new("WhiteComponent Smoke", ColorUtils.AdaptTo(245, 245, 245), ""); }
    public static ColorName Yellow { get => new("Yellow", ColorUtils.AdaptTo(255, 255, 0), ""); }
    public static ColorName YellowGreen { get => new("Yellow GreenComponent", ColorUtils.AdaptTo(154, 205, 50), ""); }
}
