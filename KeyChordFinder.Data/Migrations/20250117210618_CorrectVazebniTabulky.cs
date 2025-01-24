using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeyChordFinder.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrectVazebniTabulky : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntervalsId",
                table: "IntervalScale");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IntervalsId",
                table: "IntervalScale",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
