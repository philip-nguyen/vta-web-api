using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vtaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    TripId = table.Column<string>(type: "TEXT", nullable: false),
                    StartTime = table.Column<string>(type: "TEXT", nullable: true),
                    StartDate = table.Column<string>(type: "TEXT", nullable: true),
                    ScheduleRelationship = table.Column<string>(type: "TEXT", nullable: true),
                    RouteId = table.Column<string>(type: "TEXT", nullable: true),
                    DirectionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.TripId);
                });

            migrationBuilder.CreateTable(
                name: "TripUpdates",
                columns: table => new
                {
                    TripUpdateId = table.Column<string>(type: "TEXT", nullable: false),
                    TripId = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleId = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripUpdates", x => x.TripUpdateId);
                });

            migrationBuilder.CreateTable(
                name: "StopTimeUpdates",
                columns: table => new
                {
                    TripUpdateId = table.Column<string>(type: "TEXT", nullable: false),
                    StopSequence = table.Column<int>(type: "INTEGER", nullable: false),
                    ArrivalTime = table.Column<string>(type: "TEXT", nullable: true),
                    StopId = table.Column<string>(type: "TEXT", nullable: true),
                    ScheduleRelationship = table.Column<string>(type: "TEXT", nullable: true),
                    TripUpdateId1 = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopTimeUpdates", x => x.TripUpdateId);
                    table.ForeignKey(
                        name: "FK_StopTimeUpdates_TripUpdates_TripUpdateId1",
                        column: x => x.TripUpdateId1,
                        principalTable: "TripUpdates",
                        principalColumn: "TripUpdateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StopTimeUpdates_TripUpdateId1",
                table: "StopTimeUpdates",
                column: "TripUpdateId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StopTimeUpdates");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "TripUpdates");
        }
    }
}
