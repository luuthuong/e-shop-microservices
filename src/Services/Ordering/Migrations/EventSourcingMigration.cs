using Core.Infrastructure.EF;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Ordering.Infrastructure;

namespace Ordering.Migrations;

[DbContext(typeof(OrderReadDbContext))]
[Migration("20250503150237_Init_EventSourcing")]
public class EventSourcingMigration: Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.MigrationScript();
    }
}