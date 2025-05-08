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

[Description("RAL")]
public sealed class RALColors : ColorNameEnumerator
{
}
