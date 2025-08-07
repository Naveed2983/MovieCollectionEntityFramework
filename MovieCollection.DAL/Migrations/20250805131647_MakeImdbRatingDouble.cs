using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCollection.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakeImdbRatingDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ImdbRating",
                table: "MovieRatings",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "ImdbRating",
                table: "MovieRatings",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
