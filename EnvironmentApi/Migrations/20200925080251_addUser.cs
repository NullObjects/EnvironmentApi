using Microsoft.EntityFrameworkCore.Migrations;

namespace EnvironmentApi.Migrations
{
    public partial class addUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true)
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
                name: "User");
        }
    }
}