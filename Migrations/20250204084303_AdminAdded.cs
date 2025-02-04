using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_Rental_Backend_Application.Migrations
{
    /// <inheritdoc />
    public partial class AdminAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Admin_Admin_ID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_Admin_ID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Admin_ID",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Admin",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Admin_Email",
                table: "Admin",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Admin_Email",
                table: "Admin");

            migrationBuilder.AddColumn<int>(
                name: "Admin_ID",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Admin",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Admin_ID",
                table: "Bookings",
                column: "Admin_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Admin_Admin_ID",
                table: "Bookings",
                column: "Admin_ID",
                principalTable: "Admin",
                principalColumn: "Admin_ID");
        }
    }
}
