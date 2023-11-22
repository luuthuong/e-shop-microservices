using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductSyncService.Infrastructure.Persistence;

// public class ProductSyncDbContextFactory: IDesignTimeDbContextFactory<ProductSyncDbContext>
// {
//     public ProductSyncDbContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<ProductSyncDbContext>();
//         string? connectionString =
//             "Encrypt=False;TrustServerCertificate=True;Server=localhost;Database=ProductSyncDB;User Id=sa;Password=@123azkaw2fhtu";
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