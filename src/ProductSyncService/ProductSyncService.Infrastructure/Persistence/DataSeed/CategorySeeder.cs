using Core.EF;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductSyncService.Domain.Categories;

namespace ProductSyncService.Infrastructure.Persistence.DataSeed;

public class CategorySeeder: IDbSeeder<ProductSyncDbContext>
{
    public string Key => "1696093200-category-init-data";
    public async Task DoAsync(ProductSyncDbContext dbContext, IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILogger<ProductSeeder>>();
        logger.LogInformation("Category seeding data...");

        var categories = new List<Category>()
        {
            Category.Create("Iphone"),
            Category.Create("Samsung"),
            Category.Create("Lenovo Laptop"),
        };
        await dbContext.Category!.AddRangeAsync(categories);
        await dbContext.SaveChangeAsync();
    }
}