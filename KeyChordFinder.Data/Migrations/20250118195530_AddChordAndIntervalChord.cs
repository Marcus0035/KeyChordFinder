using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeyChordFinder.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChordAndIntervalChord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntervalChord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IntervalId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChordId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntervalChord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntervalChord_Chords_ChordId",
                        column: x => x.ChordId,
                        principalTable: "Chords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntervalChord_Intervals_IntervalId",
                        column: x => x.IntervalId,
                        principalTable: "Intervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntervalChord_ChordId",
                table: "IntervalChord",
                column: "ChordId");

            migrationBuilder.CreateIndex(
                name: "IX_IntervalChord_IntervalId",
                table: "IntervalChord",
                column: "IntervalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntervalChord");

            migrationBuilder.DropTable(
                name: "Chords");
        }
    }
}
