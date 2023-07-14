using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Generated.Mapper;

internal record struct MapperProperty
{
    public readonly string type;
    public readonly string name;
    public readonly bool isNullable;
    public readonly bool isReferenceType;
    public readonly ImmutableArray<ITypeSymbol> targetTypes;
    public ITypeSymbol? TargetType { get; set; }

    public MapperProperty(string type, string name, bool isNullable, bool isReferenceType, ImmutableArray<ITypeSymbol> targetTypes)
    {
        this.type = type;
        this.name = name;
        this.isNullable = isNullable;
        this.isReferenceType = isReferenceType;
        this.targetTypes = targetTypes;
    }
}