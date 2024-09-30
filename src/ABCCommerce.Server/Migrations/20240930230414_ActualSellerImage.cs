using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABCCommerce.Server.Migrations
{
    /// <inheritdoc />
    public partial class ActualSellerImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Sellers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Sellers");
        }
    }
}
