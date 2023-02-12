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
                    Comment = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
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
                name: "AdjudicatorPanelHistroy",
                columns: table => new
                {
                    AdjudicatorPanelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "Ref to Competition"),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_AdjudicatorPanelHistroy_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId");
                },
                comment: "History of an AdjudicatorPanelof a CompetitionClass");

            migrationBuilder.CreateTable(
                name: "AdjudicatorPanels",
                columns: table => new
                {
                    AdjudicatorPanelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to Competition"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjudicatorPanels", x => x.AdjudicatorPanelId);
                    table.ForeignKey(
                        name: "FK_AdjudicatorPanels_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "An AdjudicatorPanelof a CompetitionClass");

            migrationBuilder.CreateTable(
                name: "CompetitionClassesHistory",
                columns: table => new
                {
                    CompetitionClassId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrgClassId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false, comment: "'Internal' Org-Id of class of CompetitionClass"),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to Competition"),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    CompetitionClassName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Discipline = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    AgeClass = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    AgeGroup = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Class = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    MinStartsForPromotion = table.Column<int>(type: "INTEGER", nullable: false),
                    MinPointsForPromotion = table.Column<int>(type: "INTEGER", nullable: false),
                    PointsForFirst = table.Column<int>(type: "INTEGER", nullable: false),
                    ExtraManualStarter = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Ignore = table.Column<bool>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_CompetitionClassesHistory_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "History of Classes of a Competition");

            migrationBuilder.CreateTable(
                name: "TableVersionInfos",
                columns: table => new
                {
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to Competition"),
                    TableName = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    CurrentVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableVersionInfos", x => new { x.CompetitionId, x.TableName, x.CurrentVersion });
                    table.ForeignKey(
                        name: "FK_TableVersionInfos_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdjudicatorHistory",
                columns: table => new
                {
                    AdjudicatorlId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AdjudicatorPanelId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "Ref to AdjudicatorPanel"),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_AdjudicatorHistory_AdjudicatorPanels_AdjudicatorPanelId",
                        column: x => x.AdjudicatorPanelId,
                        principalTable: "AdjudicatorPanels",
                        principalColumn: "AdjudicatorPanelId");
                },
                comment: "Histroy of an Adjudicatorof a CompetitionClass");

            migrationBuilder.CreateTable(
                name: "Adjudicators",
                columns: table => new
                {
                    AdjudicatorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AdjudicatorPanelId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to AdjudicatorPanel"),
                    Abbreviation = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adjudicators", x => x.AdjudicatorId);
                    table.ForeignKey(
                        name: "FK_Adjudicators_AdjudicatorPanels_AdjudicatorPanelId",
                        column: x => x.AdjudicatorPanelId,
                        principalTable: "AdjudicatorPanels",
                        principalColumn: "AdjudicatorPanelId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "An Adjudicatorof a CompetitionClass");

            migrationBuilder.CreateTable(
                name: "CompetitionClasses",
                columns: table => new
                {
                    CompetitionClassId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrgClassId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false, comment: "'Internal' Org-Id of class of CompetitionClass"),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to Competition"),
                    AdjudicatorPanelId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to AdjudicatorPanel"),
                    CompetitionClassName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Discipline = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    AgeClass = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    AgeGroup = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Class = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    MinStartsForPromotion = table.Column<int>(type: "INTEGER", nullable: false),
                    MinPointsForPromotion = table.Column<int>(type: "INTEGER", nullable: false),
                    PointsForFirst = table.Column<int>(type: "INTEGER", nullable: false),
                    ExtraManualStarter = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Ignore = table.Column<bool>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionClasses", x => x.CompetitionClassId);
                    table.ForeignKey(
                        name: "FK_CompetitionClasses_AdjudicatorPanels_AdjudicatorPanelId",
                        column: x => x.AdjudicatorPanelId,
                        principalTable: "AdjudicatorPanels",
                        principalColumn: "AdjudicatorPanelId",
                        onDelete: ReferentialAction.Cascade);
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
                    StartNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    NamePartA = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    OrgIdPartA = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false, comment: "'Internal' Org-Id of PartA"),
                    NamePartB = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    OrgIdPartB = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    ClubName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    OrgIdClub = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    OrgPointsPartA = table.Column<int>(type: "INTEGER", nullable: false),
                    OrgStartsPartA = table.Column<int>(type: "INTEGER", nullable: false),
                    MinStartsForPromotionPartA = table.Column<int>(type: "INTEGER", nullable: true),
                    OrgPointsPartB = table.Column<int>(type: "INTEGER", nullable: true),
                    OrgStartsPartB = table.Column<int>(type: "INTEGER", nullable: true),
                    MinStartsForPromotionPartB = table.Column<int>(type: "INTEGER", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Ignore = table.Column<bool>(type: "INTEGER", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "ParticipantsHistory",
                columns: table => new
                {
                    ParticipantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to Competition"),
                    CompetitionClassId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "Ref to CompetitionClass"),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    StartNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    NamePartA = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    OrgIdPartA = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false, comment: "'Internal' Org-Id of PartA"),
                    NamePartB = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    OrgIdPartB = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    ClubName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    OrgIdClub = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    OrgPointsPartA = table.Column<int>(type: "INTEGER", nullable: false),
                    OrgStartsPartA = table.Column<int>(type: "INTEGER", nullable: false),
                    MinStartsForPromotionPartA = table.Column<int>(type: "INTEGER", nullable: true),
                    OrgPointsPartB = table.Column<int>(type: "INTEGER", nullable: true),
                    OrgStartsPartB = table.Column<int>(type: "INTEGER", nullable: true),
                    MinStartsForPromotionPartB = table.Column<int>(type: "INTEGER", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Ignore = table.Column<bool>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_ParticipantsHistory_CompetitionClasses_CompetitionClassId",
                        column: x => x.CompetitionClassId,
                        principalTable: "CompetitionClasses",
                        principalColumn: "CompetitionClassId");
                    table.ForeignKey(
                        name: "FK_ParticipantsHistory_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "History of Participants of a Competition");

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorHistory_AdjudicatorlId_AdjudicatorPanelId",
                table: "AdjudicatorHistory",
                columns: new[] { "AdjudicatorlId", "AdjudicatorPanelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorHistory_AdjudicatorPanelId",
                table: "AdjudicatorHistory",
                column: "AdjudicatorPanelId");

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorHistory_Name_AdjudicatorPanelId",
                table: "AdjudicatorHistory",
                columns: new[] { "Name", "AdjudicatorPanelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorPanelHistroy_AdjudicatorPanelId_CompetitionId",
                table: "AdjudicatorPanelHistroy",
                columns: new[] { "AdjudicatorPanelId", "CompetitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorPanelHistroy_CompetitionId",
                table: "AdjudicatorPanelHistroy",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorPanelHistroy_Name_CompetitionId",
                table: "AdjudicatorPanelHistroy",
                columns: new[] { "Name", "CompetitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorPanels_AdjudicatorPanelId_CompetitionId",
                table: "AdjudicatorPanels",
                columns: new[] { "AdjudicatorPanelId", "CompetitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorPanels_CompetitionId",
                table: "AdjudicatorPanels",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorPanels_Name_CompetitionId",
                table: "AdjudicatorPanels",
                columns: new[] { "Name", "CompetitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adjudicators_AdjudicatorId_AdjudicatorPanelId",
                table: "Adjudicators",
                columns: new[] { "AdjudicatorId", "AdjudicatorPanelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adjudicators_AdjudicatorPanelId",
                table: "Adjudicators",
                column: "AdjudicatorPanelId");

            migrationBuilder.CreateIndex(
                name: "IX_Adjudicators_Name_AdjudicatorPanelId",
                table: "Adjudicators",
                columns: new[] { "Name", "AdjudicatorPanelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionClasses_AdjudicatorPanelId",
                table: "CompetitionClasses",
                column: "AdjudicatorPanelId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionClasses_CompetitionId_CompetitionClassName",
                table: "CompetitionClasses",
                columns: new[] { "CompetitionId", "CompetitionClassName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionClasses_CompetitionId_OrgClassId",
                table: "CompetitionClasses",
                columns: new[] { "CompetitionId", "OrgClassId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionClassesHistory_CompetitionId_CompetitionClassName_Version",
                table: "CompetitionClassesHistory",
                columns: new[] { "CompetitionId", "CompetitionClassName", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionClassesHistory_CompetitionId_OrgClassId_Version",
                table: "CompetitionClassesHistory",
                columns: new[] { "CompetitionId", "OrgClassId", "Version" },
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

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantsHistory_CompetitionClassId",
                table: "ParticipantsHistory",
                column: "CompetitionClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantsHistory_CompetitionId_ParticipantId_Version",
                table: "ParticipantsHistory",
                columns: new[] { "CompetitionId", "ParticipantId", "Version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdjudicatorHistory");

            migrationBuilder.DropTable(
                name: "AdjudicatorPanelHistroy");

            migrationBuilder.DropTable(
                name: "Adjudicators");

            migrationBuilder.DropTable(
                name: "CompetitionClassesHistory");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "ParticipantsHistory");

            migrationBuilder.DropTable(
                name: "TableVersionInfos");

            migrationBuilder.DropTable(
                name: "CompetitionClasses");

            migrationBuilder.DropTable(
                name: "AdjudicatorPanels");

            migrationBuilder.DropTable(
                name: "Competitions");
        }
    }
}
