// © Tobias Sachs
// 22.01.2023
// Updated 08.05.2025

// ####################################
// # Hilfscode zum Generieren         #
// #       NICHT LÖSCHEN !!!!         #
// ####################################

//using System.ComponentModel;
//using System.Text.RegularExpressions;

//namespace tcs.Colors;

//public sealed partial class RALDSPColors
//{
//    // RALDesignSystemPlus.txt
//    [Browsable(false)]
//    internal static string ParseToCoding(string[] lines)
//    {
//        string li = "";
//        string lo = "";

//        static void GetName(string[] s, out string key, out string txt)
//        {
//            var nmin = 7;
//            var nmax = s.Length - 1;
//            var d = nmax - nmin;

//            var n = "";

//            for (int i = 0; i <= d; i++)
//            {
//                n += s[i];
//            }

//            var keyregex = new Regex("[-_';,\t\r ]|[\n]{2}");
//            key = keyregex.Replace(n, "");
//            txt = n;
//        }

//        foreach (var line in lines)
//        {
//            var r = line.Replace("\t", " ").Replace("  ", " ").Trim();
//            li += r + "\n";

//            var w = r.Split(" ");
//            var len = w.Length;

//            GetName(w, out var key, out var name);
//            var code = w[len - 1];
//            var red = w[len - 4];
//            var green = w[len - 3];
//            var blue = w[len - 2];
//            var context = code;

//            lo += $@"public static ColorName {key} {{ get => new ColorName(""{name}"", ColorUtils.AdaptTo({red}, {green}, {blue}), ""RAL {context}""); }}";
//            lo += "\n";
//        }

//        return lo;
//    }
//}
