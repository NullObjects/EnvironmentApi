using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EnvironmentApi.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceStatus",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CPUTemperature = table.Column<float>(type: "float", nullable: false),
                    CPUOccupancyRate = table.Column<float>(type: "float", nullable: false),
                    RAMOccupancyRate = table.Column<float>(type: "float", nullable: false),
                    SDCardOccupancyRate = table.Column<float>(type: "float", nullable: false),
                    HDDOccupancyRate = table.Column<float>(type: "float", nullable: false),
                    RecordTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_DeviceStatus", x => x.ID); });

            migrationBuilder.CreateTable(
                name: "Environment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Temperature = table.Column<float>(type: "float", nullable: false),
                    Humidity = table.Column<float>(type: "float", nullable: false),
                    RecordTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Environment", x => x.ID); });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "varchar(255)", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    Password = table.Column<string>(type: "longtext", nullable: true),
                    Role = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_User", x => x.UserName); });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] {"UserName", "Email", "Password", "Role"},
                values: new object[] {"admin", "admin@admin.com", "PBK+UIN0ifA9lVueYaEM8g==", "public::admin"});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceStatus");

            migrationBuilder.DropTable(
                name: "Environment");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}