using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanceCompetitionHelper.Database.Migrations
{
    /// <inheritdoc />
    public partial class Rework_TableVersions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdjudicatorPanelsHistroy_Competitions_CompetitionId",
                table: "AdjudicatorPanelsHistroy");

            migrationBuilder.DropForeignKey(
                name: "FK_AdjudicatorsHistory_AdjudicatorPanelsHistroy_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "AdjudicatorsHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionClassesHistory_AdjudicatorPanelsHistroy_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "CompetitionClassesHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantsHistory_CompetitionClassesHistory_CompetitionClassHistoryId_CompetitionClassHistoryVersion",
                table: "ParticipantsHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableVersionInfos",
                table: "TableVersionInfos");

            migrationBuilder.DropIndex(
                name: "IX_ParticipantsHistory_CompetitionClassHistoryId_CompetitionClassHistoryVersion",
                table: "ParticipantsHistory");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionClassesHistory_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "CompetitionClassesHistory");

            migrationBuilder.DropIndex(
                name: "IX_AdjudicatorsHistory_AdjudicatorHistoryId_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "AdjudicatorsHistory");
            migrationBuilder.DropIndex(
                name: "IX_AdjudicatorsHistory_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "AdjudicatorsHistory");

            migrationBuilder.DropIndex(
                name: "IX_AdjudicatorsHistory_Name_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "AdjudicatorsHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdjudicatorPanelsHistroy",
                table: "AdjudicatorPanelsHistroy");

            migrationBuilder.DropColumn(
                name: "TableName",
                table: "TableVersionInfos");

            migrationBuilder.DropColumn(
                name: "CompetitionClassHistoryVersion",
                table: "ParticipantsHistory");

            migrationBuilder.DropColumn(
                name: "AdjudicatorPanelHistoryVersion",
                table: "CompetitionClassesHistory");

            migrationBuilder.DropColumn(
                name: "AdjudicatorPanelHistoryVersion",
                table: "AdjudicatorsHistory");

            migrationBuilder.RenameTable(
                name: "AdjudicatorPanelsHistroy",
                newName: "AdjudicatorPanelsHistory");

            migrationBuilder.RenameIndex(
                name: "IX_AdjudicatorPanelsHistroy_Name_CompetitionId_Version",
                table: "AdjudicatorPanelsHistory",
                newName: "IX_AdjudicatorPanelsHistory_Name_CompetitionId_Version");

            migrationBuilder.RenameIndex(
                name: "IX_AdjudicatorPanelsHistroy_Created",
                table: "AdjudicatorPanelsHistory",
                newName: "IX_AdjudicatorPanelsHistory_Created");

            migrationBuilder.RenameIndex(
                name: "IX_AdjudicatorPanelsHistroy_CompetitionId",
                table: "AdjudicatorPanelsHistory",
                newName: "IX_AdjudicatorPanelsHistory_CompetitionId");

            migrationBuilder.RenameIndex(
                name: "IX_AdjudicatorPanelsHistroy_AdjudicatorPanelHistoryId_CompetitionId_Version",
                table: "AdjudicatorPanelsHistory",
                newName: "IX_AdjudicatorPanelsHistory_AdjudicatorPanelHistoryId_CompetitionId_Version");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableVersionInfos",
                table: "TableVersionInfos",
                columns: new[] { "CompetitionId", "CurrentVersion" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdjudicatorPanelsHistory",
                table: "AdjudicatorPanelsHistory",
                columns: new[] { "AdjudicatorPanelHistoryId", "Version" });

            migrationBuilder.CreateTable(
                name: "CompetitionVenuesHistory",
                columns: table => new
                {
                    CompetitionVenueHistoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Ref to Competition"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    LengthInMeter = table.Column<int>(type: "INTEGER", nullable: false),
                    WidthInMeter = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row created at (UTC)"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row created by"),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "Row last modified at (UTC)"),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, comment: "Row last modified by")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionVenuesHistory", x => new { x.CompetitionVenueHistoryId, x.Version });
                    table.ForeignKey(
                        name: "FK_CompetitionVenuesHistory_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationsHistory",
                columns: table => new
                {
                    ConfigurationValueHistoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    Organization = table.Column<int>(type: "INTEGER", nullable: true),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "Ref to Competition"),
                    CompetitionClassHistroyId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "Ref to CompetitionClass"),
                    CompetitionVenueHistoryId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "Ref to CompetitionVenue"),
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
                    table.PrimaryKey("PK_ConfigurationsHistory", x => new { x.ConfigurationValueHistoryId, x.Version });
                    table.ForeignKey(
                        name: "FK_ConfigurationsHistory_CompetitionClassesHistory_CompetitionClassHistroyId_Version",
                        columns: x => new { x.CompetitionClassHistroyId, x.Version },
                        principalTable: "CompetitionClassesHistory",
                        principalColumns: new[] { "CompetitionClassHistoryId", "Version" });
                    table.ForeignKey(
                        name: "FK_ConfigurationsHistory_CompetitionVenuesHistory_CompetitionVenueHistoryId_Version",
                        columns: x => new { x.CompetitionVenueHistoryId, x.Version },
                        principalTable: "CompetitionVenuesHistory",
                        principalColumns: new[] { "CompetitionVenueHistoryId", "Version" });
                    table.ForeignKey(
                        name: "FK_ConfigurationsHistory_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId");
                },
                comment: "Configurations");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantsHistory_CompetitionClassHistoryId_Version",
                table: "ParticipantsHistory",
                columns: new[] { "CompetitionClassHistoryId", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionClassesHistory_AdjudicatorPanelHistoryId_Version",
                table: "CompetitionClassesHistory",
                columns: new[] { "AdjudicatorPanelHistoryId", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorsHistory_AdjudicatorHistoryId_AdjudicatorPanelHistoryId_Version",
                table: "AdjudicatorsHistory",
                columns: new[] { "AdjudicatorHistoryId", "AdjudicatorPanelHistoryId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorsHistory_AdjudicatorPanelHistoryId_Version",
                table: "AdjudicatorsHistory",
                columns: new[] { "AdjudicatorPanelHistoryId", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorsHistory_Name_AdjudicatorPanelHistoryId_Version",
                table: "AdjudicatorsHistory",
                columns: new[] { "Name", "AdjudicatorPanelHistoryId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionVenuesHistory_CompetitionId_Name_Version",
                table: "CompetitionVenuesHistory",
                columns: new[] { "CompetitionId", "Name", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionVenuesHistory_Created",
                table: "CompetitionVenuesHistory",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationsHistory_CompetitionClassHistroyId_Version",
                table: "ConfigurationsHistory",
                columns: new[] { "CompetitionClassHistroyId", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationsHistory_CompetitionId",
                table: "ConfigurationsHistory",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationsHistory_CompetitionVenueHistoryId_Version",
                table: "ConfigurationsHistory",
                columns: new[] { "CompetitionVenueHistoryId", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationsHistory_Created",
                table: "ConfigurationsHistory",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationsHistory_Key",
                table: "ConfigurationsHistory",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationsHistory_Organization_CompetitionId_CompetitionClassHistroyId_CompetitionVenueHistoryId_Key_Version",
                table: "ConfigurationsHistory",
                columns: new[] { "Organization", "CompetitionId", "CompetitionClassHistroyId", "CompetitionVenueHistoryId", "Key", "Version" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdjudicatorPanelsHistory_Competitions_CompetitionId",
                table: "AdjudicatorPanelsHistory",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdjudicatorsHistory_AdjudicatorPanelsHistory_AdjudicatorPanelHistoryId_Version",
                table: "AdjudicatorsHistory",
                columns: new[] { "AdjudicatorPanelHistoryId", "Version" },
                principalTable: "AdjudicatorPanelsHistory",
                principalColumns: new[] { "AdjudicatorPanelHistoryId", "Version" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionClassesHistory_AdjudicatorPanelsHistory_AdjudicatorPanelHistoryId_Version",
                table: "CompetitionClassesHistory",
                columns: new[] { "AdjudicatorPanelHistoryId", "Version" },
                principalTable: "AdjudicatorPanelsHistory",
                principalColumns: new[] { "AdjudicatorPanelHistoryId", "Version" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantsHistory_CompetitionClassesHistory_CompetitionClassHistoryId_Version",
                table: "ParticipantsHistory",
                columns: new[] { "CompetitionClassHistoryId", "Version" },
                principalTable: "CompetitionClassesHistory",
                principalColumns: new[] { "CompetitionClassHistoryId", "Version" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdjudicatorPanelsHistory_Competitions_CompetitionId",
                table: "AdjudicatorPanelsHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AdjudicatorsHistory_AdjudicatorPanelsHistory_AdjudicatorPanelHistoryId_Version",
                table: "AdjudicatorsHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionClassesHistory_AdjudicatorPanelsHistory_AdjudicatorPanelHistoryId_Version",
                table: "CompetitionClassesHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantsHistory_CompetitionClassesHistory_CompetitionClassHistoryId_Version",
                table: "ParticipantsHistory");

            migrationBuilder.DropTable(
                name: "ConfigurationsHistory");

            migrationBuilder.DropTable(
                name: "CompetitionVenuesHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableVersionInfos",
                table: "TableVersionInfos");

            migrationBuilder.DropIndex(
                name: "IX_ParticipantsHistory_CompetitionClassHistoryId_Version",
                table: "ParticipantsHistory");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionClassesHistory_AdjudicatorPanelHistoryId_Version",
                table: "CompetitionClassesHistory");

            migrationBuilder.DropIndex(
                name: "IX_AdjudicatorsHistory_AdjudicatorHistoryId_AdjudicatorPanelHistoryId_Version",
                table: "AdjudicatorsHistory");

            migrationBuilder.DropIndex(
                name: "IX_AdjudicatorsHistory_AdjudicatorPanelHistoryId_Version",
                table: "AdjudicatorsHistory");

            migrationBuilder.DropIndex(
                name: "IX_AdjudicatorsHistory_Name_AdjudicatorPanelHistoryId_Version",
                table: "AdjudicatorsHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdjudicatorPanelsHistory",
                table: "AdjudicatorPanelsHistory");

            migrationBuilder.RenameTable(
                name: "AdjudicatorPanelsHistory",
                newName: "AdjudicatorPanelsHistroy");

            migrationBuilder.RenameIndex(
                name: "IX_AdjudicatorPanelsHistory_Name_CompetitionId_Version",
                table: "AdjudicatorPanelsHistroy",
                newName: "IX_AdjudicatorPanelsHistroy_Name_CompetitionId_Version");

            migrationBuilder.RenameIndex(
                name: "IX_AdjudicatorPanelsHistory_Created",
                table: "AdjudicatorPanelsHistroy",
                newName: "IX_AdjudicatorPanelsHistroy_Created");

            migrationBuilder.RenameIndex(
                name: "IX_AdjudicatorPanelsHistory_CompetitionId",
                table: "AdjudicatorPanelsHistroy",
                newName: "IX_AdjudicatorPanelsHistroy_CompetitionId");

            migrationBuilder.RenameIndex(
                name: "IX_AdjudicatorPanelsHistory_AdjudicatorPanelHistoryId_CompetitionId_Version",
                table: "AdjudicatorPanelsHistroy",
                newName: "IX_AdjudicatorPanelsHistroy_AdjudicatorPanelHistoryId_CompetitionId_Version");

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "TableVersionInfos",
                type: "TEXT",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CompetitionClassHistoryVersion",
                table: "ParticipantsHistory",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdjudicatorPanelHistoryVersion",
                table: "CompetitionClassesHistory",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdjudicatorPanelHistoryVersion",
                table: "AdjudicatorsHistory",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableVersionInfos",
                table: "TableVersionInfos",
                columns: new[] { "CompetitionId", "TableName", "CurrentVersion" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdjudicatorPanelsHistroy",
                table: "AdjudicatorPanelsHistroy",
                columns: new[] { "AdjudicatorPanelHistoryId", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantsHistory_CompetitionClassHistoryId_CompetitionClassHistoryVersion",
                table: "ParticipantsHistory",
                columns: new[] { "CompetitionClassHistoryId", "CompetitionClassHistoryVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionClassesHistory_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "CompetitionClassesHistory",
                columns: new[] { "AdjudicatorPanelHistoryId", "AdjudicatorPanelHistoryVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorsHistory_AdjudicatorHistoryId_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "AdjudicatorsHistory",
                columns: new[] { "AdjudicatorHistoryId", "AdjudicatorPanelHistoryId", "AdjudicatorPanelHistoryVersion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorsHistory_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "AdjudicatorsHistory",
                columns: new[] { "AdjudicatorPanelHistoryId", "AdjudicatorPanelHistoryVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_AdjudicatorsHistory_Name_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "AdjudicatorsHistory",
                columns: new[] { "Name", "AdjudicatorPanelHistoryId", "AdjudicatorPanelHistoryVersion" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdjudicatorPanelsHistroy_Competitions_CompetitionId",
                table: "AdjudicatorPanelsHistroy",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdjudicatorsHistory_AdjudicatorPanelsHistroy_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "AdjudicatorsHistory",
                columns: new[] { "AdjudicatorPanelHistoryId", "AdjudicatorPanelHistoryVersion" },
                principalTable: "AdjudicatorPanelsHistroy",
                principalColumns: new[] { "AdjudicatorPanelHistoryId", "Version" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionClassesHistory_AdjudicatorPanelsHistroy_AdjudicatorPanelHistoryId_AdjudicatorPanelHistoryVersion",
                table: "CompetitionClassesHistory",
                columns: new[] { "AdjudicatorPanelHistoryId", "AdjudicatorPanelHistoryVersion" },
                principalTable: "AdjudicatorPanelsHistroy",
                principalColumns: new[] { "AdjudicatorPanelHistoryId", "Version" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantsHistory_CompetitionClassesHistory_CompetitionClassHistoryId_CompetitionClassHistoryVersion",
                table: "ParticipantsHistory",
                columns: new[] { "CompetitionClassHistoryId", "CompetitionClassHistoryVersion" },
                principalTable: "CompetitionClassesHistory",
                principalColumns: new[] { "CompetitionClassHistoryId", "Version" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
