using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstApp.API.Migrations
{
    public partial class PublicCloudinaryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicCloudinaryId",
                table: "Photos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicCloudinaryId",
                table: "Photos");
        }
    }
}
