using Microsoft.EntityFrameworkCore;
using Oshimiri.Data;
using Oshimiri.DataTransfer;
using Oshimiri.Models;
using Oshimiri.ViewModels;
using Roselyn.Generated.Source.Extension;

namespace Oshimiri.Services
{
    public class CategroryService : BaseModelService<Category>, ICategoryService
    {
        private readonly OshimiriDbContext context;
        private readonly ILogger<BaseModelService<Category>> logger;

        public CategroryService(OshimiriDbContext context, ILogger<BaseModelService<Category>> logger) 
        {
            this.context = context;
            this.logger = logger;
        }
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryViewModel dto)
        {
            Category category = UpdateAudit(dto.ToCategory());
            await context.AddAsync(category);
            await context.SaveChangesAsync();
            logger.LogInformation("Saved category {id} to database", category.Id);
            return category.ToCategoryDTO();
        }

        public Task<CategoryDTO[]> GetAllCategoriesAsync()
        {
            return context.Categories.Select(static category => category.ToCategoryDTO()).ToArrayAsync();
        }

        public Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            return context.Categories
                .Where(category => category.Id == id)
                .Select(static category => category.ToCategoryDTO())
                .FirstOrDefaultAsync();
        }
    }
}
