/// © Tobias Sachs
/// Colors/IColorSpace.cs
/// 22.01.2023

namespace tcs.Colors;

public interface IColorSpace
{
    ColorCode ConvertTo();

    void ConvertFrom(ColorCode code);

    void SetValues(params ColorRange[] values);

    int ComponentCount { get; }

    ColorComponent[] GetComponents();
}
