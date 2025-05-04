using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class addtitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleImageName",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleImageName",
                table: "News");
        }
    }
}
