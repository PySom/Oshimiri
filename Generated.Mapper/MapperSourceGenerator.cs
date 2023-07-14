using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;

namespace Generated.Mapper;


[Generator]
public class MapperSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(PostInitializationOutputCallback);
        var syntaxProvider = context.SyntaxProvider.CreateSyntaxProvider(static (SyntaxNode node, CancellationToken cancellationToken) => {
            return SyntacticPredicate(node);
            }, static (GeneratorSyntaxContext context, CancellationToken cancellationToken) =>
            {
                return SyntacticTransform(context, cancellationToken);
            })
            .Where(static type => type is not null)
            .Select(static (type, cancellationToken) => TransformType(type!, cancellationToken))
            .WithComparer(MapperClassEqualityComparer.Instance);

        context.RegisterSourceOutput(syntaxProvider, Execute);
    }

    private static MapperClass TransformType(INamedTypeSymbol type, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        string? nmspace = type.ContainingNamespace.IsGlobalNamespace
            ? null : type.ContainingNamespace.ToDisplayString();
        string className = type.Name;
        var properties = type.GetMembers()
                                .Where(static member => member.Kind == SymbolKind.Property)
                                .Select(TransformProperty)
                                .ToImmutableArray();
        var propsAsString = string.Join("-", properties.Select(static property => property.name));
        var propertyBag = PrepareDictionaryForProperties(properties);
        return new MapperClass(type, nmspace, className, propertyBag, propsAsString);

    }

    private static MapperProperty TransformProperty(ISymbol symbol)
    {
        var property = Unsafe.As<IPropertySymbol>(symbol);
        var type = property.Type.ToDisplayString();
        var name = property.Name;
        var isNullable = property.Type.NullableAnnotation == NullableAnnotation.Annotated;
        var isReferenceType = property.Type.IsReferenceType;
        var targetTypes = ExtractTypeFromAttribute(property.GetAttributes());
        return new MapperProperty(type, name,isNullable, isReferenceType, targetTypes);
    }

    private static ImmutableArray<ITypeSymbol> ExtractTypeFromAttribute(ImmutableArray<AttributeData> attributes)
    {
        return attributes.Where(static attribute => attribute?.AttributeClass?.ToDisplayString()?.StartsWith("Generated.Mapper.Helpers.MapAttribute") ?? false)
            .Where(static attribute => attribute is not null && attribute.AttributeClass is not null && attribute.AttributeClass!.TypeArguments.Length == 1)
            .Select(static attribute => attribute.AttributeClass!.TypeArguments[0]).ToImmutableArray();
    }

    private static ImmutableDictionary<ITypeSymbol, ImmutableArray<MapperProperty>> PrepareDictionaryForProperties(ImmutableArray<MapperProperty> properties)
    {
        return properties.Aggregate(new Dictionary<ITypeSymbol, ImmutableArray<MapperProperty>>(SymbolEqualityComparer.Default), (bag, current) => 
        { 
            foreach (var type in current.targetTypes)
            {
                current.TargetType = type;
                if (bag.ContainsKey(type))
                {
                    bag[type] = bag[type].Add(current);
                }
                else
                {
                    bag[type] = ImmutableArray.Create(current);
                }
            }
            return bag;
        }).ToImmutableDictionary(SymbolEqualityComparer.Default);
    }

    private static void CreateClassForProperties(SourceProductionContext context, ImmutableArray<MapperProperty> properties, ITypeSymbol type)
    {

        string className = type.Name;
        var genCode = new StringBuilder(80);
        genCode.Append($$"""
            {{AttributeGeneratorHelper.GeneratedHeaderComment}}
            namespace {{type.ContainingNamespace.ToDisplayString()}};
            {{AttributeGeneratorHelper.GeneratedCodeAttribute}}
            public partial class {{className}}
            {

            """);
        foreach (var property in properties)
        {
            var requiredText = property.isReferenceType && !property.isNullable ? "required" : string.Empty;
            genCode.Append($"\tpublic {requiredText} {property.type} {property.name} {{get; set;}}\n");
        }
        genCode.Append("\n}");
        context.AddSource($"Roselyn.Generated.Mapper.{className}.g.cs", genCode.ToString());
    }

    private static void GenerateExtensionMethodForMapper(SourceProductionContext context, ImmutableArray<MapperProperty> properties, ITypeSymbol type, INamedTypeSymbol containingType)
    {
        var genCode = new StringBuilder(80);
        genCode.Append($$"""
            {{AttributeGeneratorHelper.GeneratedHeaderComment}}
            namespace Roselyn.Generated.Source.Extension;
            {{AttributeGeneratorHelper.GeneratedCodeAttribute}}
            public static class MapperExtensionFor{{type.Name}}
            {
                public static {{containingType.ToDisplayString()}} To{{containingType.Name}}(this {{type.ToDisplayString()}} item)
                {
                    return new() 
                        {
            """);
        foreach (var property in properties)
        {
            genCode.Append($"\t{property.name} = item.{property.name},\n");
        }
        genCode.Append("\t};");
        genCode.Append("\n}\n\n");
        genCode.Append($$""""
            public static {{type.ToDisplayString()}} To{{type.Name}}(this {{containingType.ToDisplayString()}} item)
                {
                    return new() 
                        {
            """");
        foreach (var property in properties)
        {
            genCode.Append($"\t{property.name} = item.{property.name},\n");
        }
        genCode.Append("\t};");
        genCode.Append("\n}\n\n");
        genCode.Append("\n}");
        context.AddSource($"Roselyn.Generated.Mapper.MapperExtensionFor{type.Name}.g.cs", genCode.ToString());

    }

    private static void Execute(SourceProductionContext context, MapperClass mapperClassContext) 
    {
        foreach (var item in mapperClassContext.properties)
        {
            CreateClassForProperties(context, item.Value, item.Key);
            GenerateExtensionMethodForMapper(context, item.Value, item.Key, mapperClassContext.type);
        }
    }


    private static void PostInitializationOutputCallback(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource("Roselyn.Generated.MapperAttribute.g.cs", AttributeGeneratorHelper.MapperAttribute);
        context.AddSource("Roselyn.Generated.MapAttribute.g.cs", AttributeGeneratorHelper.MapAttribute);
    }

    private static bool SyntacticPredicate(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax
        {
            AttributeLists.Count: > 0
        } potentialCandidate
          && !potentialCandidate.Modifiers.Any(SyntaxKind.StaticKeyword);
    }

    private static INamedTypeSymbol? SyntacticTransform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        ClassDeclarationSyntax potentialCandidate = Unsafe.As<ClassDeclarationSyntax>(context.Node);
        INamedTypeSymbol? symbol = context.SemanticModel.GetDeclaredSymbol(potentialCandidate, cancellationToken);
        if(symbol is INamedTypeSymbol type)
        {
            INamedTypeSymbol? mapperAttribute = context.SemanticModel.Compilation.GetTypeByMetadataName("Generated.Mapper.Helpers.MapperAttribute");
            
            if (ContainsAttributeOfInterest(potentialCandidate, context.SemanticModel, mapperAttribute))
            {
                return type;
            }
        }
        return null;
    }

    private static bool ContainsAttributeOfInterest(ClassDeclarationSyntax classDeclarationSyntax, SemanticModel
        semanticModel, INamedTypeSymbol? expectedAttribute)
    {
        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(attribute);
                ISymbol? symbol = symbolInfo.Symbol;

                if (symbol is not null && SymbolEqualityComparer.Default.Equals(symbol.ContainingSymbol, expectedAttribute))
                {
                    return true;
                }
            }
        }

        return false;

    }

    

}
