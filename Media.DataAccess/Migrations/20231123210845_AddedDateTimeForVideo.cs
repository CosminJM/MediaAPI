using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Media.DataAccess.Migrations
{
    public partial class AddedDateTimeForVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDateTime",
                table: "Videos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedDateTime",
                table: "Videos");
        }
    }
}
