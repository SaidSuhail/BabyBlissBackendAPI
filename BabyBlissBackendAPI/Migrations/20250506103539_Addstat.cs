using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyBlissBackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class Addstat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrderStatus",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "ORDER PLACED",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "OderPlaced");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrderStatus",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "OderPlaced",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "ORDER PLACED");
        }
    }
}
