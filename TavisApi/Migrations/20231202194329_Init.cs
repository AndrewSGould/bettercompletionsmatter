using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrueAchievementId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    TrueAchievement = table.Column<int>(type: "integer", nullable: true),
                    Gamerscore = table.Column<int>(type: "integer", nullable: true),
                    AchievementCount = table.Column<int>(type: "integer", nullable: true),
                    Publisher = table.Column<string>(type: "text", nullable: true),
                    Developer = table.Column<string>(type: "text", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GamersWithGame = table.Column<int>(type: "integer", nullable: true),
                    GamersCompleted = table.Column<int>(type: "integer", nullable: true),
                    BaseCompletionEstimate = table.Column<double>(type: "double precision", nullable: true),
                    SiteRatio = table.Column<double>(type: "double precision", nullable: true),
                    SiteRating = table.Column<double>(type: "double precision", nullable: true),
                    Unobtainables = table.Column<bool>(type: "boolean", nullable: false),
                    ServerClosure = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InstallSize = table.Column<double>(type: "double precision", nullable: true),
                    FullCompletionEstimate = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlayerCount = table.Column<int>(type: "integer", nullable: true),
                    TaHits = table.Column<int>(type: "integer", nullable: true),
                    Profile = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Xuid = table.Column<string>(type: "text", nullable: true),
                    Gamertag = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    Area = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BcmCompletionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameId = table.Column<int>(type: "integer", nullable: true),
                    SiteRatio = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmCompletionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BcmCompletionHistory_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FeatureLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OneXEnhanced = table.Column<bool>(type: "boolean", nullable: false),
                    BackwardsCompat = table.Column<bool>(type: "boolean", nullable: false),
                    NotBackwardsCompat = table.Column<bool>(type: "boolean", nullable: false),
                    PlayAnywhere = table.Column<bool>(type: "boolean", nullable: false),
                    SmartDelivery = table.Column<bool>(type: "boolean", nullable: false),
                    OptimizedForSeries = table.Column<bool>(type: "boolean", nullable: false),
                    Crossplay = table.Column<bool>(type: "boolean", nullable: false),
                    Hdr = table.Column<bool>(type: "boolean", nullable: false),
                    xCloudTouch = table.Column<bool>(type: "boolean", nullable: false),
                    GamePass = table.Column<bool>(type: "boolean", nullable: false),
                    CloudGaming = table.Column<bool>(type: "boolean", nullable: false),
                    PcGamePass = table.Column<bool>(type: "boolean", nullable: false),
                    EaPlay = table.Column<bool>(type: "boolean", nullable: false),
                    GamePreview = table.Column<bool>(type: "boolean", nullable: false),
                    IdAtXbox = table.Column<bool>(type: "boolean", nullable: false),
                    OnSteam = table.Column<bool>(type: "boolean", nullable: false),
                    GamesWithGold = table.Column<bool>(type: "boolean", nullable: false),
                    TransferableProgress = table.Column<bool>(type: "boolean", nullable: false),
                    FeatureListOfGameId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureLists_Games_FeatureListOfGameId",
                        column: x => x.FeatureListOfGameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameGenres",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false)
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
                name: "BcmPlayers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TrueAchievementId = table.Column<int>(type: "integer", nullable: false),
                    Registration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSync = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BcmPlayers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscordLogins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    TokenType = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Password = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRegistration",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RegistrationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegistration", x => new { x.UserId, x.RegistrationId });
                    table.ForeignKey(
                        name: "FK_UserRegistration_Registrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRegistration_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BcmPlayerCompletionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    BcmPlayerId = table.Column<long>(type: "bigint", nullable: true),
                    GameId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmPlayerCompletionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BcmPlayerCompletionHistory_BcmPlayers_BcmPlayerId",
                        column: x => x.BcmPlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BcmPlayerCompletionHistory_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BcmPlayerGames",
                columns: table => new
                {
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    Platform = table.Column<int>(type: "integer", nullable: true),
                    TrueAchievement = table.Column<int>(type: "integer", nullable: true),
                    Gamerscore = table.Column<int>(type: "integer", nullable: true),
                    AchievementCount = table.Column<int>(type: "integer", nullable: true),
                    StartedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUnlock = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ownership = table.Column<int>(type: "integer", nullable: true),
                    NotForContests = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmPlayerGames", x => new { x.GameId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_BcmPlayerGames_BcmPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BcmPlayerGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BcmRgsc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Issued = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Rerolled = table.Column<bool>(type: "boolean", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: true),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    BcmPlayerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmRgsc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BcmRgsc_BcmPlayers_BcmPlayerId",
                        column: x => x.BcmPlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BcmStats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rank = table.Column<int>(type: "integer", nullable: true),
                    RankMovement = table.Column<int>(type: "integer", nullable: true),
                    Completions = table.Column<int>(type: "integer", nullable: true),
                    AverageRatio = table.Column<double>(type: "double precision", nullable: true),
                    HighestRatio = table.Column<double>(type: "double precision", nullable: true),
                    AverageTimeEstimate = table.Column<double>(type: "double precision", nullable: true),
                    HighestTimeEstimate = table.Column<double>(type: "double precision", nullable: true),
                    AveragePoints = table.Column<double>(type: "double precision", nullable: true),
                    BasePoints = table.Column<double>(type: "double precision", nullable: true),
                    BonusPoints = table.Column<double>(type: "double precision", nullable: true),
                    TotalPoints = table.Column<double>(type: "double precision", nullable: true),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BcmStats_BcmPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "BcmPlayers",
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
                    { 26, "Collectable Card Game" },
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
                    { 42, "Fighting" },
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
                    { 82, "Turn Based" },
                    { 83, "Swimming" },
                    { 84, "Surfing" },
                    { 85, "Badminton" },
                    { 86, "Table Tennis" },
                    { 87, "Skating" },
                    { 88, "Lacrosse" },
                    { 89, "Skydiving" }
                });

            migrationBuilder.InsertData(
                table: "Registrations",
                columns: new[] { "Id", "EndDate", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Better Completions Matter", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, null, "Calendar Project", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BcmCompletionHistory_GameId",
                table: "BcmCompletionHistory",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_BcmPlayerCompletionHistory_BcmPlayerId",
                table: "BcmPlayerCompletionHistory",
                column: "BcmPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_BcmPlayerCompletionHistory_GameId",
                table: "BcmPlayerCompletionHistory",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_BcmPlayerGames_PlayerId",
                table: "BcmPlayerGames",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_BcmPlayers_UserId",
                table: "BcmPlayers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BcmRgsc_BcmPlayerId",
                table: "BcmRgsc",
                column: "BcmPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_BcmStats_PlayerId",
                table: "BcmStats",
                column: "PlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordLogins_UserId",
                table: "DiscordLogins",
                column: "UserId",
                unique: true);

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
                name: "IX_Logins_UserId",
                table: "Logins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRegistration_RegistrationId",
                table: "UserRegistration",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BcmCompletionHistory");

            migrationBuilder.DropTable(
                name: "BcmPlayerCompletionHistory");

            migrationBuilder.DropTable(
                name: "BcmPlayerGames");

            migrationBuilder.DropTable(
                name: "BcmRgsc");

            migrationBuilder.DropTable(
                name: "BcmStats");

            migrationBuilder.DropTable(
                name: "DiscordLogins");

            migrationBuilder.DropTable(
                name: "FeatureLists");

            migrationBuilder.DropTable(
                name: "GameGenres");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "SyncHistory");

            migrationBuilder.DropTable(
                name: "UserRegistration");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "BcmPlayers");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
