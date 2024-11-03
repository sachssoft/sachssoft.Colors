/// © Tobias Sachs
/// tcs_CoreVisuals
/// Colors/ColorName.cs
/// 22.01.2023

namespace tcs.Colors;

public readonly struct ColorName
{
    public ColorName(string name, ColorCode color, string context)
    {
        this.Name = name;
        this.Code = color;
        this.Context = context;
    }

    public ColorName(string name, ColorCode color)
    {
        this.Name = name;
        this.Code = color;
        this.Context = string.Empty;
    }

    public ColorCode Code { get; }

    public string Name { get; }

    public string Context { get; }

}
