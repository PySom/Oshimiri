using Oshimiri.Services;
using Oshimiri.ViewModels;

namespace Oshimiri.Endpoints;

public static class CategoriesEndpoint
{
    public static IEndpointConventionBuilder MapCategories(this IEndpointRouteBuilder endpoints)
    {
        var categoryGroup = endpoints
            .MapGroup("categories")
            .WithTags("Categories")
            .WithOpenApi();

        categoryGroup.MapGet("", async (ICategoryService categoryService) =>
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            return TypedResults.Ok(categories);
        }).WithName("GetCategories");

        categoryGroup.MapGet("{id:int}", async (int id, ICategoryService categoryService) =>
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            return category switch
            {
                not null => Results.Ok(category),
                _ => Results.NotFound(),
            };
        }).WithName("GetCategoryById");

        categoryGroup.MapPost("", async (CategoryViewModel dto, ICategoryService categoryService) =>
        {
            var category = await categoryService.CreateCategoryAsync(dto);
            return TypedResults.Created($"categories/{category.Id}", category);
        }).WithName("CreateCategory");

        

        return categoryGroup;

        
    }
}
