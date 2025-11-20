using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByCodersTecApi.Infrastructure.Data.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20251120_000000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Stores",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Owner = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Stores", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Transactions",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Type = table.Column<int>(type: "int", nullable: false),
                OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                Card = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                StoreId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Transactions", x => x.Id);
                table.ForeignKey(
                    name: "FK_Transactions_Stores_StoreId",
                    column: x => x.StoreId,
                    principalTable: "Stores",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Stores_Name_Owner",
            table: "Stores",
            columns: new[] { "Name", "Owner" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_StoreId",
            table: "Transactions",
            column: "StoreId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Transactions");

        migrationBuilder.DropTable(
            name: "Stores");
    }
}
