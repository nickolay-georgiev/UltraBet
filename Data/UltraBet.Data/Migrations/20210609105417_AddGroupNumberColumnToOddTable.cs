namespace UltraBet.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddGroupNumberColumnToOddTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupNumber",
                table: "Odds",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupNumber",
                table: "Odds");
        }
    }
}
