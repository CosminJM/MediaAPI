using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Media.DataAccess.Migrations
{
    public partial class QueriedVideoDatesFromAndTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "QueriedFromDate",
                table: "Videos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<DateTime>(
                name: "QueriedToDate",
                table: "Videos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QueriedFromDate",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "QueriedToDate",
                table: "Videos");
        }
    }
}
