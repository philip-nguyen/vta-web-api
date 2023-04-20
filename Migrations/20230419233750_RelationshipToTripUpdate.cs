using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vtaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipToTripUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StopTimeUpdates_TripUpdates_TripUpdateId1",
                table: "StopTimeUpdates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StopTimeUpdates",
                table: "StopTimeUpdates");

            migrationBuilder.DropIndex(
                name: "IX_StopTimeUpdates_TripUpdateId1",
                table: "StopTimeUpdates");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "TripUpdates");

            migrationBuilder.DropColumn(
                name: "TripUpdateId1",
                table: "StopTimeUpdates");

            migrationBuilder.AddColumn<string>(
                name: "TripUpdateId",
                table: "Trips",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StopTimeUpdates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StopTimeUpdates",
                table: "StopTimeUpdates",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TripUpdateId",
                table: "Trips",
                column: "TripUpdateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StopTimeUpdates_TripUpdateId",
                table: "StopTimeUpdates",
                column: "TripUpdateId");

            migrationBuilder.AddForeignKey(
                name: "FK_StopTimeUpdates_TripUpdates_TripUpdateId",
                table: "StopTimeUpdates",
                column: "TripUpdateId",
                principalTable: "TripUpdates",
                principalColumn: "TripUpdateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_TripUpdates_TripUpdateId",
                table: "Trips",
                column: "TripUpdateId",
                principalTable: "TripUpdates",
                principalColumn: "TripUpdateId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StopTimeUpdates_TripUpdates_TripUpdateId",
                table: "StopTimeUpdates");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_TripUpdates_TripUpdateId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_TripUpdateId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StopTimeUpdates",
                table: "StopTimeUpdates");

            migrationBuilder.DropIndex(
                name: "IX_StopTimeUpdates_TripUpdateId",
                table: "StopTimeUpdates");

            migrationBuilder.DropColumn(
                name: "TripUpdateId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StopTimeUpdates");

            migrationBuilder.AddColumn<string>(
                name: "TripId",
                table: "TripUpdates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TripUpdateId1",
                table: "StopTimeUpdates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StopTimeUpdates",
                table: "StopTimeUpdates",
                column: "TripUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_StopTimeUpdates_TripUpdateId1",
                table: "StopTimeUpdates",
                column: "TripUpdateId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StopTimeUpdates_TripUpdates_TripUpdateId1",
                table: "StopTimeUpdates",
                column: "TripUpdateId1",
                principalTable: "TripUpdates",
                principalColumn: "TripUpdateId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
