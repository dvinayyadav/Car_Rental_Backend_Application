using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_Rental_Backend_Application.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Admins_Admin_ID",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "Admin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admin",
                table: "Admin",
                column: "Admin_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Admin_Admin_ID",
                table: "Bookings",
                column: "Admin_ID",
                principalTable: "Admin",
                principalColumn: "Admin_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Admin_Admin_ID",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admin",
                table: "Admin");

            migrationBuilder.RenameTable(
                name: "Admin",
                newName: "Admins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "Admin_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Admins_Admin_ID",
                table: "Bookings",
                column: "Admin_ID",
                principalTable: "Admins",
                principalColumn: "Admin_ID");
        }
    }
}
