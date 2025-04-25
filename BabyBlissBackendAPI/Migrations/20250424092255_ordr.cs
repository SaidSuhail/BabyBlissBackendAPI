using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyBlissBackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class ordr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_userAddress_AddressId",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "FK_order_users_UserId",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "FK_orderItems_order_OrderId",
                table: "orderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order",
                table: "order");

            migrationBuilder.RenameTable(
                name: "order",
                newName: "orders");

            migrationBuilder.RenameIndex(
                name: "IX_order_UserId",
                table: "orders",
                newName: "IX_orders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_order_AddressId",
                table: "orders",
                newName: "IX_orders_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orders",
                table: "orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orderItems_orders_OrderId",
                table: "orderItems",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_userAddress_AddressId",
                table: "orders",
                column: "AddressId",
                principalTable: "userAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_UserId",
                table: "orders",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderItems_orders_OrderId",
                table: "orderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_userAddress_AddressId",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_UserId",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orders",
                table: "orders");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "order");

            migrationBuilder.RenameIndex(
                name: "IX_orders_UserId",
                table: "order",
                newName: "IX_order_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_orders_AddressId",
                table: "order",
                newName: "IX_order_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order",
                table: "order",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_userAddress_AddressId",
                table: "order",
                column: "AddressId",
                principalTable: "userAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_users_UserId",
                table: "order",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orderItems_order_OrderId",
                table: "orderItems",
                column: "OrderId",
                principalTable: "order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
