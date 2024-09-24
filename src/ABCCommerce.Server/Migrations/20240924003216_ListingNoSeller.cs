using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABCCommerce.Server.Migrations
{
    /// <inheritdoc />
    public partial class ListingNoSeller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Sellers_SellerId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Sellers_SellerId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_SellerId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Listings");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Sellers_SellerId",
                table: "Items",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Sellers_SellerId",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_SellerId",
                table: "Listings",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Sellers_SellerId",
                table: "Items",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Sellers_SellerId",
                table: "Listings",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
