using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreditCardValidator.Migrations
{
    /// <inheritdoc />
    public partial class StoreFullCardNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFourDigits",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Cards",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "LastFourDigits",
                table: "Cards",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");
        }
    }
}
