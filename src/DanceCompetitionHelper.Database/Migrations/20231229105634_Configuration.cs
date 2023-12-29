using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanceCompetitionHelper.Database.Migrations
{
    /// <inheritdoc />
    public partial class Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetitionVenues",
                columns: table => new
                {
                    CompetitionVenueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to Competition"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionVenues", x => x.CompetitionVenueId);
                    table.ForeignKey(
                        name: "FK_CompetitionVenues_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Organization = table.Column<int>(type: "INTEGER", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to Competition"),
                    CompetitionClassId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to CompetitionClass"),
                    CompetitionVenueId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to CompetitionVenue"),
                    Key = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false, comment: "Key of the Configuration Value"),
                    Value = table.Column<string>(type: "TEXT", nullable: true, comment: "Value itself"),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => new { x.Organization, x.CompetitionId, x.CompetitionClassId, x.CompetitionVenueId, x.Key });
                },
                comment: "Configurations");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionVenues_CompetitionId_Name",
                table: "CompetitionVenues",
                columns: new[] { "CompetitionId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionVenues_Created",
                table: "CompetitionVenues",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Configurations_Created",
                table: "Configurations",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Configurations_Key",
                table: "Configurations",
                column: "Key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionVenues");

            migrationBuilder.DropTable(
                name: "Configurations");
        }
    }
}
