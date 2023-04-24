using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vtaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ActiveTripUpdatesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveTripUpdates",
                columns: table => new
                {
                    ActiveTripUpdateId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveTripUpdates", x => x.ActiveTripUpdateId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveTripUpdates");
        }
    }
}
