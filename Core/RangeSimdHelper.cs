using System;
using System.Numerics;

internal static class ColorRangeSimdHelper
{
    internal const float RANGE_MIN_VALUE = 0.0f;
    internal const float RANGE_MAX_VALUE = 1.0f;

    public static void ClampSpan(Span<float> values)
    {
        int simd_width = Vector<float>.Count;
        int i = 0;

        var min = new Vector<float>(RANGE_MIN_VALUE);
        var max = new Vector<float>(RANGE_MAX_VALUE);

        // SIMD: in Blöcken von z. B. 4 oder 8 Werten
        for (; i <= values.Length - simd_width; i += simd_width)
        {
            var vec = new Vector<float>(values.Slice(i, simd_width));
            vec = Vector.Min(Vector.Max(vec, min), max);
            vec.CopyTo(values.Slice(i, simd_width));
        }

        // Rest: einzelner Clamp
        for (; i < values.Length; i++)
        {
            float v = values[i];
            values[i] = v < RANGE_MIN_VALUE
                ? RANGE_MIN_VALUE
                : (v > RANGE_MAX_VALUE ? RANGE_MAX_VALUE : v);
        }
    }

    public static void Multiply(Span<float> values, float factor)
    {
        int simd_width = Vector<float>.Count;
        int i = 0;

        var vec_factor = new Vector<float>(factor);

        for (; i <= values.Length - simd_width; i += simd_width)
        {
            var vec = new Vector<float>(values.Slice(i, simd_width));
            vec *= vec_factor;
            vec.CopyTo(values.Slice(i, simd_width));
        }

        for (; i < values.Length; i++)
            values[i] *= factor;
    }
}
