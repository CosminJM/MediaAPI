using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Media.DataAccess.Migrations
{
    public partial class FilteringByChannelName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilteredByChannelName",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilteredByChannelName",
                table: "Videos");
        }
    }
}
