using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace agileTrackerServer.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionToEpicAndUserStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "user_stories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "epics",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "user_stories");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "epics");
        }
    }
}
