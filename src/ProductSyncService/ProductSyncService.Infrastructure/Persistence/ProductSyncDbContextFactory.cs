using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductSyncService.Infrastructure.Persistence;

// public class ProductSyncDbContextFactory(IConfiguration configuration): IDesignTimeDbContextFactory<ProductSyncDbContext>
// {
//     public ProductSyncDbContext CreateDbContext(string[] args)
//     {
//         Console.WriteLine("using design time builder");
//         var optionsBuilder = new DbContextOptionsBuilder<ProductSyncDbContext>();
//         string? connectionString = configuration.GetConnectionString("Database");
//         if (string.IsNullOrEmpty(connectionString))
//         {
//             throw new ArgumentNullException(nameof(connectionString));
//         }
//         Console.WriteLine($"Connection String: {connectionString}");
//         optionsBuilder.UseSqlServer(connectionString, sqlConfig =>
//         {
//             sqlConfig.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null);
//         });
//         return new ProductSyncDbContext(optionsBuilder.Options);
//     }
// }