using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Generated.Mapper;

internal readonly record struct MapperClass
{
    public readonly INamedTypeSymbol type;
    public readonly string? nmspace;
    public readonly string className;
    public readonly ImmutableDictionary<ITypeSymbol, ImmutableArray<MapperProperty>> properties;
    public readonly string propsAsString;

    public MapperClass(INamedTypeSymbol type, string? nmspace, string className, ImmutableDictionary<ITypeSymbol, ImmutableArray<MapperProperty>> properties, string propsAsString)
    {
        this.type = type;
        this.nmspace = nmspace;
        this.className = className;
        this.properties = properties;
        this.propsAsString = propsAsString;
    }

    public override string ToString()
    {
        return $"{nmspace}-{className}-{type.Name}-{propsAsString}";
    }
}

internal sealed class MapperClassEqualityComparer : IEqualityComparer<MapperClass>
{
    private MapperClassEqualityComparer() { }

    public static MapperClassEqualityComparer Instance { get; } = new();

    public bool Equals(MapperClass x, MapperClass y)
    {
        return x.ToString() == y.ToString();
    }

    public int GetHashCode(MapperClass obj)
    {
        return obj.ToString().GetHashCode();
    }
}