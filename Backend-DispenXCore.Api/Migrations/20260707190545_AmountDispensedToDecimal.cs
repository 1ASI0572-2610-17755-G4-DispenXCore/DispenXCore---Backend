using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_DispenXCore.Api.Migrations
{
    /// <inheritdoc />
    public partial class AmountDispensedToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AmountDispensed",
                table: "DispenserEvents",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AmountDispensed",
                table: "DispenserEvents",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
