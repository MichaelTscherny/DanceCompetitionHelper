using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanceCompetitionHelper.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Organization = table.Column<int>(type: "INTEGER", nullable: false),
                    OrgCompetitionId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false, comment: "'Internal' Org-Id of Competition"),
                    CompetitionName = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    CompetitionInfo = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    CompetitionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.CompetitionId);
                },
                comment: "A Competition 'root'");

            migrationBuilder.CreateTable(
                name: "CompetitionClasses",
                columns: table => new
                {
                    CompetitionClassId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrgClassId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false, comment: "'Internal' Org-Id of class of CompetitionClass"),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to Competition"),
                    Discipline = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    AgeClass = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    AgeGroup = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Class = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    MinStartsForPromotion = table.Column<int>(type: "INTEGER", nullable: false),
                    MinPointsForPromotion = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionClasses", x => x.CompetitionClassId);
                    table.ForeignKey(
                        name: "FK_CompetitionClasses_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "The classes of a Competition");

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    ParticipantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to Competition"),
                    CompetitionClassId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to CompetitionClass"),
                    NamePartA = table.Column<string>(type: "TEXT", nullable: false),
                    OrgIdPartA = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false, comment: "'Internal' Org-Id of class of CompetitionClass"),
                    NamePartB = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    OrgIdPartB = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    OrgIdClub = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    OrgPointsPartA = table.Column<int>(type: "INTEGER", nullable: false),
                    OrgStartsPartA = table.Column<int>(type: "INTEGER", nullable: false),
                    OrgPointsPartB = table.Column<int>(type: "INTEGER", nullable: true),
                    OrgStartsPartB = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.ParticipantId);
                    table.ForeignKey(
                        name: "FK_Participants_CompetitionClasses_CompetitionClassId",
                        column: x => x.CompetitionClassId,
                        principalTable: "CompetitionClasses",
                        principalColumn: "CompetitionClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "The Participants of a Competition");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionClasses_CompetitionId_OrgClassId",
                table: "CompetitionClasses",
                columns: new[] { "CompetitionId", "OrgClassId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_Organization_OrgCompetitionId",
                table: "Competitions",
                columns: new[] { "Organization", "OrgCompetitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_CompetitionClassId",
                table: "Participants",
                column: "CompetitionClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_CompetitionId",
                table: "Participants",
                column: "CompetitionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "CompetitionClasses");

            migrationBuilder.DropTable(
                name: "Competitions");
        }
    }
}
