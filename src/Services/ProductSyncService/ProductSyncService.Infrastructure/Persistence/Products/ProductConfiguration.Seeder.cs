using Core.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Infrastructure.Persistence.Products;

public sealed class ProductSeeder : IDbSeeder<ProductSyncDbContext>
{
    public string Key => "1698771600-product-init-data";

    public async Task DoAsync(ProductSyncDbContext dbContext, IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILogger<ProductSeeder>>();
        logger.LogInformation("Product seeding data....");

        var categories = await dbContext.Category!.ToListAsync();

        var iphoneCategory = categories.Find(x => x.Name == "Iphone");

        var samsungCategory = categories.Find(x => x.Name == "Samsung");

        var lenovoCategory = categories.Find(x => x.Name == "Lenovo Laptop");

        var products = new List<Product>()
        {
            Product.Create(
                "Iphone14",
                iphoneCategory!.Id,
                @"The iPhone 14 is Apple's latest flagship smartphone, 
                        boasting cutting-edge technology, improved camera capabilities, 
                        enhanced performance, and a sleek design. 
                        Its advanced features and intuitive interface redefine the smartphone experience for users worldwide.",
                "Revolutionary, sleek, powerful: the iPhone 14 redefines mobile technology excellence",
                Money.From(23M, Currency.Vnd.Code)
            ),
            Product.Create(
                "iPhone 14 Pro Max",
                iphoneCategory!.Id,
                @"Meet the iPhone 14 Pro Max, a pinnacle of technology with superior camera capabilities, unmatched performance, and a stunning, larger display for an immersive experience.",
                "Unparalleled innovation: iPhone 14 Pro Max sets new standards.",
                Money.From(28M, Currency.Vnd.Code)
            ),
            Product.Create(
                "Iphone 15 Pro max",
                iphoneCategory!.Id,
                @"The iPhone 15 Pro Max redefines excellence with its powerful A16 Bionic chip, 
                           stunning ProMotion display, advanced camera system, and enhanced battery life. 
                           Its sleek design and innovative features offer unparalleled performance and user experience.",
                "Powerful, sleek, innovative; iPhone 15 Pro Max redefines premium smartphone excellence.",
                Money.From(35M, Currency.Vnd.Code)
            ),
            Product.Create(
                "Samsung Galaxy S22",
                iphoneCategory!.Id,
                @"Experience the Samsung Galaxy S22, featuring cutting-edge technology, an exceptional camera system, top-tier performance, and a stunning design.",
                "Innovation redefined: Samsung Galaxy S22 leads the way.",
                Money.From(21M, Currency.Vnd.Code)
            ),

            Product.Create(
                "Samsung Galaxy Note 23",
                samsungCategory!.Id,
                @"Unleash productivity with the Samsung Galaxy Note 23. Superb stylus integration, powerful features, and sleek aesthetics.",
                "Elevated productivity: Samsung Galaxy Note 23 redefines efficiency.",
                Money.From(24M, Currency.Vnd.Code)
            ),

            Product.Create(
                "Samsung Galaxy Z Fold 4",
                samsungCategory.Id,
                @"Introducing the Samsung Galaxy Z Fold 4, a foldable marvel with revolutionary technology, versatile functionality, and a futuristic design.",
                "Versatile innovation: Samsung Galaxy Z Fold 4 leads the foldable era.",
                Money.From(27M, Currency.Vnd.Code)
            ),
            Product.Create(
                "Lenovo Yoga Slim 7i",
                lenovoCategory!.Id,
                @"Experience ultimate portability and performance with the Lenovo Yoga Slim 7i. Stunning design, powerful specs, and exceptional battery life.",
                "Sleek & powerful: Lenovo Yoga Slim 7i redefines mobility.",
                Money.From(20M, Currency.Vnd.Code)
            ),
            Product.Create(
                "Lenovo ThinkBook 14",
                lenovoCategory!.Id,
                @"The Lenovo ThinkBook 14 combines business-class features with modern design and exceptional performance for professionals seeking productivity and style.",
                "Productivity meets style: Lenovo ThinkBook 14 redefines business laptops.",
                Money.From(19M, Currency.Vnd.Code)
            ),
            Product.Create(
                "Lenovo Chromebook Duet",
                lenovoCategory!.Id,
                @"Experience versatility with the Lenovo Chromebook Duet.  A 2-in-1 device with Chrome OS, offering portability   and efficiency for work or entertainment.",
                "Versatile & portable: Lenovo Chromebook Duet adapts to your needs.",
                Money.From(12M, Currency.Vnd.Code)
            ),
            Product.Create(
                "Lenovo Legion Y740",
                lenovoCategory!.Id,
                @"The Lenovo Legion Y740 is a high-performance gaming laptop with powerful specs, immersive display, and advanced cooling for an exceptional gaming experience.",
                "Elevate your gaming: Lenovo Legion Y740 delivers top-notch performance.",
                Money.From(28M, Currency.Vnd.Code)
            ),
            Product.Create(
                "Lenovo ThinkCentre M90n Nano",
                lenovoCategory!.Id,
                @"The Lenovo ThinkCentre M90n Nano is an ultra-compact desktop offering powerful performance and space-saving design for business or home use.",
                "Compact powerhouse: Lenovo ThinkCentre M90n Nano fits anywhere.",
                Money.From(16M, Currency.Vnd.Code)
            )
        };
        
        await dbContext.Product!.AddRangeAsync(products);
        await dbContext.SaveChangeAsync();
    }
}