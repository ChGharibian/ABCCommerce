using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABCCommerce.Server.Migrations
{
    /// <inheritdoc />
    public partial class ManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Items_ItemId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Sellers_SellerId",
                table: "Listings");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Items_ItemId",
                table: "Listings",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Sellers_SellerId",
                table: "Listings",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Items_ItemId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Sellers_SellerId",
                table: "Listings");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Listings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Listings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Items_ItemId",
                table: "Listings",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Sellers_SellerId",
                table: "Listings",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id");
        }
    }
}
