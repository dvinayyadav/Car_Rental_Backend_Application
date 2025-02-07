using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Car_Rental_Backend_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedPredindedAdmins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_User_ID",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "Admin_ID", "Email", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "devaravinay698.com", "Vinay@123", "devaravinay698" },
                    { 2, "narasimhagorla45@gmail.com", "Narasimha@123", "narasimhagorla45" },
                    { 3, "rupeshsanagala523@gmail.com", "Rupesh@123", "rupeshsanagala523" },
                    { 4, "ajaythella0@gmail.com", "Ajay@123", "ajaythella0" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_User_ID",
                table: "Bookings",
                column: "User_ID",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_User_ID",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "Admin",
                keyColumn: "Admin_ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Admin",
                keyColumn: "Admin_ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Admin",
                keyColumn: "Admin_ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Admin",
                keyColumn: "Admin_ID",
                keyValue: 4);

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Reservation_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Car_ID = table.Column<int>(type: "int", nullable: false),
                    User_ID = table.Column<int>(type: "int", nullable: false),
                    Pickup_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reservation_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Return_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Reservation_ID);
                    table.ForeignKey(
                        name: "FK_Reservations_Cars_Car_ID",
                        column: x => x.Car_ID,
                        principalTable: "Cars",
                        principalColumn: "Car_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Car_ID",
                table: "Reservations",
                column: "Car_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_User_ID",
                table: "Reservations",
                column: "User_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_User_ID",
                table: "Bookings",
                column: "User_ID",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
