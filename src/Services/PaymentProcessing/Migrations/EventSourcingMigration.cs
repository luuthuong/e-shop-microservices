using Core.Infrastructure.EF;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PaymentProcessing.Infrastructure;

namespace PaymentProcessing.Migrations;

[DbContext(typeof(PaymentReadDbContext))]
[Migration("20250503150237_Init_EventSourcing")]
public class EventSourcingMigration: Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.MigrationScript();
    }
}