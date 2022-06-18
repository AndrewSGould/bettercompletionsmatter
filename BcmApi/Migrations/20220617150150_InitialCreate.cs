using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BcmApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrueAchievementId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrueAchievement = table.Column<int>(type: "int", nullable: true),
                    Gamerscore = table.Column<int>(type: "int", nullable: true),
                    AchievementCount = table.Column<int>(type: "int", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Developer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GamersWithGame = table.Column<int>(type: "int", nullable: true),
                    GamersCompleted = table.Column<int>(type: "int", nullable: true),
                    BaseCompletionEstimate = table.Column<double>(type: "float", nullable: true),
                    SiteRatio = table.Column<double>(type: "float", nullable: true),
                    SiteRating = table.Column<double>(type: "float", nullable: true),
                    Unobtainables = table.Column<bool>(type: "bit", nullable: false),
                    ServerClosure = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InstallSize = table.Column<double>(type: "float", nullable: true),
                    FullCompletionEstimate = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrueAchievementId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OneXEnhanced = table.Column<bool>(type: "bit", nullable: false),
                    BackwardsCompat = table.Column<bool>(type: "bit", nullable: false),
                    NotBackwardsCompat = table.Column<bool>(type: "bit", nullable: false),
                    PlayAnywhere = table.Column<bool>(type: "bit", nullable: false),
                    SmartDelivery = table.Column<bool>(type: "bit", nullable: false),
                    OptimizedForSeries = table.Column<bool>(type: "bit", nullable: false),
                    Crossplay = table.Column<bool>(type: "bit", nullable: false),
                    Hdr = table.Column<bool>(type: "bit", nullable: false),
                    xCloudTouch = table.Column<bool>(type: "bit", nullable: false),
                    GamePass = table.Column<bool>(type: "bit", nullable: false),
                    CloudGaming = table.Column<bool>(type: "bit", nullable: false),
                    PcGamePass = table.Column<bool>(type: "bit", nullable: false),
                    EaPlay = table.Column<bool>(type: "bit", nullable: false),
                    GamePreview = table.Column<bool>(type: "bit", nullable: false),
                    IdAtXbox = table.Column<bool>(type: "bit", nullable: false),
                    OnSteam = table.Column<bool>(type: "bit", nullable: false),
                    GamesWithGold = table.Column<bool>(type: "bit", nullable: false),
                    FeatureListOfGameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureLists_Games_FeatureListOfGameId",
                        column: x => x.FeatureListOfGameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameGenres",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenres", x => new { x.GameId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_GameGenres_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerGames",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Platform = table.Column<int>(type: "int", nullable: true),
                    TrueAchievement = table.Column<int>(type: "int", nullable: true),
                    Gamerscore = table.Column<int>(type: "int", nullable: true),
                    AchievementCount = table.Column<int>(type: "int", nullable: true),
                    StartedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUnlock = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ownership = table.Column<int>(type: "int", nullable: true),
                    NotForContests = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGames", x => new { x.GameId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_PlayerGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerGames_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Sports" },
                    { 3, "Football" },
                    { 4, "Third Person Shooter" },
                    { 5, "Action Horror" },
                    { 6, "Action-Adventure" },
                    { 7, "Action-RPG" },
                    { 8, "Role Playing" },
                    { 9, "Hack & Slash" },
                    { 10, "Aerial" },
                    { 11, "Vehicular Combat" },
                    { 12, "American Football" },
                    { 13, "Arcade Racing" },
                    { 14, "Automobile" },
                    { 15, "Australian Football" },
                    { 16, "Baseball" },
                    { 17, "Basketball" },
                    { 18, "First Person Shooter" },
                    { 19, "Battle Royale" },
                    { 20, "Beat 'em up" },
                    { 21, "Bowling" },
                    { 22, "Boxing" },
                    { 23, "Bull Sports" },
                    { 24, "Card & Board" },
                    { 25, "Casino" },
                    { 26, "Collectible Card Game" },
                    { 27, "Collection" },
                    { 28, "Adventure" },
                    { 29, "Point & Click" },
                    { 30, "Cricket" },
                    { 31, "Cue Sports" },
                    { 32, "Platformer" },
                    { 33, "Cycling" },
                    { 34, "Dance" },
                    { 35, "Darts" },
                    { 36, "Dodgeball" },
                    { 37, "Open World" },
                    { 38, "Dungeon Crawler" },
                    { 39, "Educational & Trivia" },
                    { 40, "Party" },
                    { 41, "Equestrian Sports" },
                    { 42, "Fighting" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 43, "Fishing" },
                    { 44, "Golf" },
                    { 45, "Handball" },
                    { 46, "Simulation" },
                    { 47, "Health & Fitness" },
                    { 48, "Hockey" },
                    { 49, "Hunting" },
                    { 50, "Management" },
                    { 51, "Mech" },
                    { 52, "Metroidvania" },
                    { 53, "Mixed Martial Arts" },
                    { 54, "MMO" },
                    { 55, "MOBA" },
                    { 56, "Motocross" },
                    { 57, "On Rails" },
                    { 58, "Music" },
                    { 59, "Naval" },
                    { 60, "Survival" },
                    { 61, "Paintball" },
                    { 62, "Pinball" },
                    { 63, "Puzzle" },
                    { 64, "Strategy" },
                    { 65, "Real Time" },
                    { 66, "Roguelite" },
                    { 67, "Rugby" },
                    { 68, "Run & Gun" },
                    { 69, "Sandbox" },
                    { 70, "Shoot 'em up" },
                    { 71, "Simulation Racing" },
                    { 72, "Skateboarding" },
                    { 73, "Skiing" },
                    { 74, "Snowboarding" },
                    { 75, "Stealth" },
                    { 76, "Survival Horror" },
                    { 77, "Tennis" },
                    { 78, "Tower Defence" },
                    { 79, "Visual Novel" },
                    { 80, "Volleyball" },
                    { 81, "Wrestling" },
                    { 82, "Turn Based" }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "Name", "Region", "TrueAchievementId" },
                values: new object[,]
                {
                    { 1, "Ohio", false, "kT Echo", "United States", 104571 },
                    { 2, "Ohio", false, "eohjay", "United States", 266752 }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "Name", "Region", "TrueAchievementId" },
                values: new object[] { 3, "Ohio", false, "IronFistofSnuff", "United States", 405202 });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "Name", "Region", "TrueAchievementId" },
                values: new object[] { 4, null, false, "zzScanMan1", null, 461682 });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureLists_FeatureListOfGameId",
                table: "FeatureLists",
                column: "FeatureListOfGameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameGenres_GenreId",
                table: "GameGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGames_PlayerId",
                table: "PlayerGames",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureLists");

            migrationBuilder.DropTable(
                name: "GameGenres");

            migrationBuilder.DropTable(
                name: "PlayerGames");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
