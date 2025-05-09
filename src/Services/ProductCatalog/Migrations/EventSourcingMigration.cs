using Core.Infrastructure.EF;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Migrations;

[DbContext(typeof(ProductCatalogReadDbContext))]
[Migration("20250503150237_Init_EventSourcing")]
public class EventSourcingMigration: Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.MigrationScript();
    }
}