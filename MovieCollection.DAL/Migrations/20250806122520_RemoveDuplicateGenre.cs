using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCollection.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDuplicateGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 7);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "GenreName" },
                values: new object[] { 7, "Horror" });
        }
    }
}
