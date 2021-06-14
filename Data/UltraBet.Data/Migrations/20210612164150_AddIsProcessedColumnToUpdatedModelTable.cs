using Microsoft.EntityFrameworkCore.Migrations;

namespace UltraBet.Data.Migrations
{
    public partial class AddIsProcessedColumnToUpdatedModelTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UpdatedModels",
                newName: "Type");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "UpdatedModels",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "UpdatedModels");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "UpdatedModels",
                newName: "Name");
        }
    }
}
