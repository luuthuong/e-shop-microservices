using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductSyncService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addseedinghistorytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeedingHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeedingHistory", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeedingHistory");
        }
    }
}
