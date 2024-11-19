using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABCCommerce.Server.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedAndReservedDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Last4",
                table: "Transactions",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "CartItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationExpirationDate",
                table: "CartItems",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Last4",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ReservationExpirationDate",
                table: "CartItems");
        }
    }
}
