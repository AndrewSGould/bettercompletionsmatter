using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class ResetEFMigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contests", x => x.Id);
                });

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
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrueAchievementId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    Area = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LastSync = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
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
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Xuid = table.Column<string>(type: "text", nullable: true),
                    Gamertag = table.Column<string>(type: "text", nullable: true)
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
                name: "BcmRgsc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Issued = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Rerolled = table.Column<bool>(type: "boolean", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: true),
                    PlayerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmRgsc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BcmRgsc_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
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
                    PlayerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BcmStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerCompletionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: true),
                    GameId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCompletionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerCompletionHistory_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerCompletionHistory_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerContests",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    ContestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerContests", x => new { x.ContestId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_PlayerContests_Contests_ContestId",
                        column: x => x.ContestId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerContests_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerGames",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "DiscordLogins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    TokenType = table.Column<string>(type: "text", nullable: true),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
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
                name: "UserUserRole",
                columns: table => new
                {
                    UserRolesId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUserRole", x => new { x.UserRolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserUserRole_UserRoles_UserRolesId",
                        column: x => x.UserRolesId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUserRole_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Contests",
                columns: new[] { "Id", "EndDate", "Name", "StartDate" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Better Completions Matter", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

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
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "LastSync", "Name", "Region", "TrueAchievementId" },
                values: new object[,]
                {
                    { 1, "Ohio", true, null, "kT Echo", "United States", 104571 },
                    { 2, "Ohio", true, null, "eohjay", "United States", 266752 },
                    { 3, "Ohio", true, null, "IronFistofSnuff", "United States", 405202 },
                    { 4, null, false, null, "zzScanMan1", null, 461682 },
                    { 5, "Georgia", true, null, "SwiftSupafly", "United States", 691631 },
                    { 6, "New York", false, null, "DubstepEdgelord", "United States", 644155 },
                    { 7, null, true, null, "Infamous", "England", 518665 },
                    { 8, "California", true, null, "Luke17000", "United States", 1013881 },
                    { 9, "Kentucky", true, null, "lucas1987", "United States", 4838 },
                    { 10, null, true, null, "Fine Feat", "Austrailia", 695506 },
                    { 11, "Ontario", true, null, "smrnov", "Canada", 1815 },
                    { 12, "Georgia", true, null, "Sir Paulygon", "United States", 746750 },
                    { 13, "Victoria", false, null, "Rossco7530", "Austrailia", 385301 },
                    { 14, null, false, null, "True Veteran", "Austrailia", 992494 },
                    { 15, "New Zealand", true, null, "CasualExile", "New Zealand", 561724 },
                    { 16, "Wisconsin", true, null, "darkwing1232", "United States", 644384 },
                    { 17, "Suffolk", true, null, "JimbotUK", "England", 567743 },
                    { 18, "Hesse", true, null, "xMagicMunKix", "Germany", 312377 },
                    { 19, "New York", true, null, "Inferno118", "United States", 405939 },
                    { 20, "Hesse", true, null, "AC Rock3tman", "Germany", 503921 },
                    { 21, "Michigan", true, null, "tackleglass54", "United States", 585579 },
                    { 22, "Illinois", true, null, "IcyThrasher", "United States", 115320 },
                    { 23, "Texas", true, null, "TXMOOK", "United States", 1872 },
                    { 24, null, true, null, "BemusedBox", "England", 5540 },
                    { 25, null, true, null, "nuttywray", "England", 292068 },
                    { 26, "Missouri", true, null, "Christoph 5782", "United States", 441743 },
                    { 27, "Quebec", true, null, "Mtld", "Canada", 8962 },
                    { 28, "Virginia", false, null, "Lw N1GHTM4R3", "United States", 978641 },
                    { 29, "Nottinghamshire", true, null, "Erutaerc", "England", 75572 },
                    { 30, "Hungary", true, null, "WildWhiteNoise", "Hungary", 332637 },
                    { 31, "Rhineland-Palatinate", true, null, "GD GodSpeed", "Germany", 767838 },
                    { 32, null, false, null, "Legohead 1977", "England", 273370 },
                    { 33, "California", true, null, "Saurvivalist", "United States", 1001899 },
                    { 34, null, true, null, "UltimateDespair", "England", 386783 },
                    { 35, "Michigan", false, null, "radnonnahs", "United States", 483713 },
                    { 36, null, true, null, "Wakapeil", "Sweden", 316981 },
                    { 37, "North Carolina", false, null, "DaDuelingDonuts", "United States", 661240 },
                    { 38, "New Jersey", false, null, "Oriole2682", "United States", 251492 },
                    { 39, "Washington", true, null, "NBA Kirkland", "United States", 18165 },
                    { 40, "North Carolina", false, null, "Xx Phatryda xX", "United States", 540888 },
                    { 41, null, true, null, "Fista Roboto", "Austrailia", 514795 },
                    { 42, "Florida", true, null, "HegemonicHater", "United States", 431830 },
                    { 43, null, false, null, "Team Brether", "England", 109626 },
                    { 44, "Oklahoma", true, null, "Mattism", "United States", 17362 },
                    { 45, null, true, null, "Alyssiya", "England", 667139 },
                    { 46, "Georgia", false, null, "MajinFro", "United States", 389388 },
                    { 47, "Florida", true, null, "Whisperin Clown", "United States", 460875 },
                    { 48, "New Hampshire", false, null, "Mental Knight 5", "United States", 273989 },
                    { 49, null, false, null, "ChinDocta", "Austrailia", 473608 },
                    { 50, "Devon", true, null, "A1exRD", "England", 108134 },
                    { 51, "Washington", true, null, "Ethigy", "United States", 662432 },
                    { 52, "Rhineland-Palatinate", false, null, "Nichtl", "Germany", 596837 },
                    { 53, "Pennsylvania", false, null, "PangoBara", "United States", 507793 },
                    { 54, "Iowa", true, null, "HawkeyeBarry20", "United States", 548044 },
                    { 55, "New York", true, null, "KawiNinjaRider7", "United States", 636969 },
                    { 56, null, true, null, "BenL72", "England", 280034 },
                    { 57, "Minnesota", true, null, "Freamwhole", "United States", 276088 },
                    { 58, "Virginia", true, null, "Proulx", "United States", 78779 },
                    { 59, null, false, null, "zzUrbanSpaceman", "Austrailia", 60207 },
                    { 60, null, true, null, "A 0 E Monkey", "England", 27797 },
                    { 61, "Georgia", false, null, "Majinbro", "United States", 38595 },
                    { 62, "Connecticut", true, null, "Icefiretn", "United States", 126013 },
                    { 63, "Massachusetts", true, null, "LORDOFDOOKIE69", "United States", 130600 },
                    { 64, "North Carolina", false, null, "RetroChief1969", "United States", 685511 },
                    { 65, "California", true, null, "EldritchSS", "United States", 11497 },
                    { 66, "Tennessee", false, null, "rawkerdude", "United States", 25889 },
                    { 67, "Illinois", false, null, "iMaginaryy", "United States", 351310 },
                    { 68, "South Carolina", true, null, "Kez001", "United States", 15074 },
                    { 69, null, true, null, "WoodsMonk", "England", 46893 },
                    { 70, "Washington", true, null, "W1cked Girl", "United States", 391205 },
                    { 71, "Cardiff", true, null, "Kitty Skies", "Wales", 321249 },
                    { 72, "Connecticut", true, null, "MrGompers", "United States", 53128 },
                    { 73, "Netherlands", true, null, "GTKrouwel83", "Netherlands", 991656 },
                    { 74, "Kentucky", true, null, "CouchBurglar", "United States", 710980 },
                    { 75, "Virginia", true, null, "SprinkyDink", "United States", 391799 },
                    { 76, null, true, null, "boldyno1", "England", 57548 },
                    { 77, "Virginia", true, null, "FreakyRO", "United States", 389092 },
                    { 78, "Kansas", true, null, "hotcurls3088", "United States", 259643 },
                    { 79, "Georgia", true, null, "Kaiteh", "United States", 714727 },
                    { 80, null, true, null, "Yinga Garten", "United Kingdom", 107004 },
                    { 81, "Minnesota", false, null, "Skeptical Mario", "United States", 47628 },
                    { 82, "New Jersey", true, null, "WildwoodMike", "United States", 286696 },
                    { 83, "New Hampshire", true, null, "ChewieOnIce", "England", 115479 },
                    { 84, "Croatia", false, null, "Igneus DarkSide", "Croatia", 275395 },
                    { 85, "Ireland", true, null, "DavidMcC1989", "Ireland", 89301 },
                    { 86, "Pennsylvania", true, null, "RadicalSniper99", "United States", 378519 },
                    { 87, "Nevada", false, null, "Hotdogmcgee", "United States", 4860 },
                    { 88, null, true, null, "HenkeXD", "Sweden", 332575 },
                    { 89, "Maryland", true, null, "KooshMoose", "United States", 435315 },
                    { 90, "Virginia", false, null, "Xynvincible", "United States", 58478 },
                    { 91, "Wisconsin", true, null, "Whtthfgg", "United States", 365315 },
                    { 92, "Colorado", false, null, "PlayUltimate711", "United States", 125044 },
                    { 93, "British Columbia", true, null, "Seamus McLimey", "Canada", 119394 },
                    { 94, "Lancashire", true, null, "Northern Lass", "England", 40704 },
                    { 95, "California", true, null, "xNeo21x", "United States", 37540 },
                    { 96, null, true, null, "Redanian", "England", 401906 },
                    { 97, "New Jersey", false, null, "TBonePhone", "United States", 64912 },
                    { 98, "Lower Saxony", true, null, "IxGermanBeastxI", "Germany", 734459 },
                    { 99, "Georgia", true, null, "PhillipWendell", "United States", 78294 },
                    { 100, null, false, null, "C64 Mat", "England", 338344 },
                    { 101, null, true, null, "Benjii Redux", "Austrailia", 519817 },
                    { 102, null, false, null, "KATAKL1ZM", "England", 286468 },
                    { 103, null, false, null, "Mark B", "England", 408078 },
                    { 104, "Iowa", true, null, "Slayer Reigning", "United States", 82490 },
                    { 105, "Derbyshire", true, null, "xLAx JesteR", "England", 135630 },
                    { 106, "Ontario", true, null, "CrunchyGoblin68", "Canada", 450068 },
                    { 107, "Massachusetts", true, null, "SaucySlingo", "United States", 333080 },
                    { 108, "Lower Saxony", true, null, "III Torpedo III", "Germany", 94956 },
                    { 109, null, true, null, "Eliphelet77", "England", 78762 },
                    { 110, "Cardiff", true, null, "Lord Zell", "Wales", 434741 },
                    { 111, "Oregon", false, null, "Boda Yett", "United States", 907614 },
                    { 112, "Tennessee", true, null, "Not A Designer", "United States", 800715 },
                    { 113, null, true, null, "Muetschens", "Switzerland", 64293 },
                    { 114, "New York", true, null, "Big Ell", "United States", 64295 },
                    { 115, "New York", true, null, "K4rn4ge", "United States", 2898 },
                    { 116, "Texas", true, null, "bryan dot exe", "United States", 134205 },
                    { 117, "Slovenia", false, null, "Kaneman", "Slovenia", 410425 },
                    { 118, "Georgia", false, null, "DudeWithTheFace", "United States", 24047 },
                    { 119, "Indiana", true, null, "MadLefty2097", "United States", 684657 },
                    { 120, "California", true, null, "retstak", "United States", 447768 },
                    { 121, "Ohio", true, null, "Death Dealers", "United States", 615774 },
                    { 122, "New York", true, null, "Matrarch", "United States", 5581 },
                    { 123, "Pennsylvania", false, null, "omgeezus", "United States", 13608 },
                    { 124, null, true, null, "FlutteryChicken", "England", 7499 },
                    { 125, "Florida", false, null, "CarpeAdam79", "United States", 651526 },
                    { 126, null, true, null, "Hatton90", "England", 647169 },
                    { 127, "Indiana", true, null, "Triple Triad777", "United States", 538641 },
                    { 128, "Texas", false, null, "Buffs", "United States", 105056 },
                    { 129, "Pennsylvania", true, null, "BPBPBPBPBPBPBP", "United States", 38717 },
                    { 130, "West Virginia", false, null, "CHERRY CHEERIOS", "United States", 266948 },
                    { 131, "Jamaica", false, null, "DJB Hustlin", "Jamaica", 54140 },
                    { 132, "Ontario", false, null, "ITS ALivEx", "Canada", 328184 },
                    { 133, "New York", false, null, "TheAlphaSeagull", "United States", 893882 },
                    { 134, "Ontario", false, null, "mdp 73", "Canada", 1995 },
                    { 135, null, false, null, "ListlessDragon", "Switzerland", 533284 },
                    { 136, "Wisconsin", true, null, "WeezyFuzz", "United States", 435101 },
                    { 137, "Louisiana", true, null, "ILethalStang", "United States", 628247 },
                    { 138, "Ontario", false, null, "IrishWarrior022", "Canada", 392553 },
                    { 139, "Arizona", false, null, "General Tynstar", "United States", 6420 },
                    { 140, "British Columbia", true, null, "Ace", "Canada", 684086 },
                    { 141, "Ohio", false, null, "DirtyMcNasty126", "United States", 1022271 },
                    { 142, "Michigan", false, null, "Bsmittel", "United States", 439921 },
                    { 143, "Texas", true, null, "SKOOT2006", "United States", 387165 },
                    { 144, null, true, null, "Simpso", "England", 111184 },
                    { 145, "Illinois", true, null, "BlazeFlareon", "United States", 451792 },
                    { 146, null, true, null, "MrWolfw00d", "England", 375330 },
                    { 147, null, false, null, "AgileDuke", "England", 50502 },
                    { 148, "Wisconsin", false, null, "JeffMomm", "United States", 349158 },
                    { 149, null, true, null, "MattiasAnderson", "Sweden", 349605 },
                    { 150, "Colorado", false, null, "Kyleia", "United States", 711432 },
                    { 151, null, false, null, "Miller N7", "England", 896662 },
                    { 152, "North Carolina", false, null, "Fresh336669", "United States", 380552 },
                    { 153, "Suffolk", false, null, "MOT Astro", "England", 721181 },
                    { 154, "California", true, null, "Facial La Fleur", "United States", 4936 },
                    { 155, "Ohio", false, null, "AnonymousODB", "United States", 437038 },
                    { 156, "Maryland", false, null, "FuFuCuddilyPoof", "United States", 635863 },
                    { 157, null, false, null, "xI The Rock Ix", null, 408827 },
                    { 158, null, false, null, "tatersoup19", null, 85256 },
                    { 159, null, false, null, "nightw0lf", null, 347191 },
                    { 160, null, true, null, "N0TH THA G0D", null, 1725 },
                    { 161, null, true, null, "emz fergi", "Scotland", 702307 },
                    { 162, null, false, null, "Inigomontoya80", null, 262143 },
                    { 163, null, true, null, "PRTM CLUESCROL", null, 597081 },
                    { 164, null, true, null, "AZ Mongoose", "United States", 48289 },
                    { 165, null, false, null, "Vulgar Latin", null, 76517 },
                    { 166, null, false, null, "TobyLinn", null, 643897 },
                    { 167, null, false, null, "Jblacq", null, 52223 },
                    { 168, null, false, null, "Enigma Gamer 77", null, 370170 },
                    { 169, null, false, null, "Xpovos", null, 318602 },
                    { 170, null, true, null, "Shadow", null, 409281 },
                    { 171, null, false, null, "Ahayzo", null, 276943 },
                    { 172, null, false, null, "Darklord Davis", null, 364130 },
                    { 173, null, false, null, "logicslayer", null, 97393 },
                    { 174, null, false, null, "HyRoad V2", null, 723406 },
                    { 175, null, false, null, "GT3OptionFan", null, 257340 },
                    { 176, null, false, null, "kungfuskills", null, 357761 },
                    { 177, null, true, null, "AlbinoKidELITE", "England", 61615 },
                    { 178, null, true, null, "bye571", "United States", 884637 },
                    { 179, null, true, null, "DanTheWhale", "United States", 414125 },
                    { 180, null, true, null, "Dave Bodom", "England", 344585 },
                    { 181, null, true, null, "dubdeetwothree", "Monaco", 758474 },
                    { 182, "Brazil", true, null, "FAREP Bunitin", "South America", 413009 },
                    { 183, null, true, null, "Inv1s1bl", "Austrailia", 357029 },
                    { 184, null, true, null, "J Battlestar", "United States", 107749 },
                    { 185, "Ohio", true, null, "Krazie", "United States", 1438 },
                    { 186, null, true, null, "Lonsta DaMonsta", "United States", 1632 },
                    { 187, null, true, null, "MagicalChild", "United States", 10343 },
                    { 188, "Washington", true, null, "McTennisD", "United States", 2308 },
                    { 189, null, true, null, "meanmachine832", "England", 734556 },
                    { 190, null, true, null, "Mephisto4thewin", "Austrailia", 15824 },
                    { 191, "Nevada", true, null, "MinPin", "United States", 5677 },
                    { 192, "Brazil", true, null, "princit", "South America", 274932 },
                    { 193, null, true, null, "QuantumGrey17", "United States", 660376 },
                    { 194, null, true, null, "SabenRothschild", "United Kingdom", 523662 },
                    { 195, "Pennsylvania", true, null, "Scarovese", "United States", 393252 },
                    { 196, null, true, null, "SmokenRocket", "United States", 11749 },
                    { 197, null, true, null, "Sniggit", "United States", 603590 },
                    { 198, null, true, null, "ToadStyleVenom", "United States", 65260 },
                    { 199, "Brazil", true, null, "wellingtonbalbo", "South America", 386934 },
                    { 200, null, true, null, "KingsOfDispair", null, 404521 },
                    { 201, null, true, null, "EdenWeekes86", null, 689458 },
                    { 202, null, true, null, "MobileSuitVB", null, 719694 },
                    { 203, "Argentina", true, null, "AcaelusT", "South America", 684497 },
                    { 204, null, true, null, "Ow Nitram", null, 44122 },
                    { 205, null, true, null, "PaunchyDeer473", null, 349069 },
                    { 206, null, true, null, "SlayingUrchin3", null, 613063 },
                    { 207, null, true, null, "Raw Sauce Ross", null, 368168 },
                    { 208, null, true, null, "DANIELJJ14", null, 567738 },
                    { 209, null, true, null, "Saint Riley", null, 380038 },
                    { 210, null, true, null, "GoatFondler1", null, 1009214 },
                    { 211, null, true, null, "Morbid237", null, 315362 },
                    { 212, null, true, null, "Chezno", null, 743880 },
                    { 213, null, true, null, "JonnyDelicious", null, 20116 }
                });

            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 5 },
                    { 1, 7 },
                    { 1, 8 },
                    { 1, 9 },
                    { 1, 10 },
                    { 1, 11 },
                    { 1, 12 },
                    { 1, 15 },
                    { 1, 16 },
                    { 1, 17 },
                    { 1, 18 },
                    { 1, 19 },
                    { 1, 20 },
                    { 1, 21 },
                    { 1, 22 },
                    { 1, 23 },
                    { 1, 24 },
                    { 1, 25 },
                    { 1, 26 },
                    { 1, 27 },
                    { 1, 29 },
                    { 1, 30 },
                    { 1, 31 },
                    { 1, 33 },
                    { 1, 34 },
                    { 1, 36 },
                    { 1, 39 },
                    { 1, 41 },
                    { 1, 42 },
                    { 1, 44 },
                    { 1, 45 },
                    { 1, 47 },
                    { 1, 50 },
                    { 1, 51 },
                    { 1, 54 },
                    { 1, 55 },
                    { 1, 56 },
                    { 1, 57 },
                    { 1, 58 },
                    { 1, 60 },
                    { 1, 62 },
                    { 1, 63 },
                    { 1, 65 },
                    { 1, 68 },
                    { 1, 69 },
                    { 1, 70 },
                    { 1, 71 },
                    { 1, 72 },
                    { 1, 73 },
                    { 1, 74 },
                    { 1, 75 },
                    { 1, 76 },
                    { 1, 77 },
                    { 1, 78 },
                    { 1, 79 },
                    { 1, 80 },
                    { 1, 82 },
                    { 1, 83 },
                    { 1, 85 },
                    { 1, 86 },
                    { 1, 88 },
                    { 1, 89 },
                    { 1, 91 },
                    { 1, 93 },
                    { 1, 94 },
                    { 1, 95 },
                    { 1, 96 },
                    { 1, 98 },
                    { 1, 99 },
                    { 1, 101 },
                    { 1, 104 },
                    { 1, 105 },
                    { 1, 106 },
                    { 1, 107 },
                    { 1, 108 },
                    { 1, 109 },
                    { 1, 110 },
                    { 1, 112 },
                    { 1, 113 },
                    { 1, 114 },
                    { 1, 115 },
                    { 1, 116 },
                    { 1, 119 },
                    { 1, 120 },
                    { 1, 121 },
                    { 1, 122 },
                    { 1, 124 },
                    { 1, 126 },
                    { 1, 127 },
                    { 1, 129 },
                    { 1, 136 },
                    { 1, 137 },
                    { 1, 140 },
                    { 1, 143 },
                    { 1, 144 },
                    { 1, 145 },
                    { 1, 146 },
                    { 1, 149 },
                    { 1, 154 },
                    { 1, 160 },
                    { 1, 161 },
                    { 1, 163 },
                    { 1, 164 },
                    { 1, 170 },
                    { 1, 177 },
                    { 1, 178 },
                    { 1, 179 },
                    { 1, 180 },
                    { 1, 181 },
                    { 1, 182 },
                    { 1, 183 },
                    { 1, 184 },
                    { 1, 185 },
                    { 1, 186 },
                    { 1, 187 },
                    { 1, 188 },
                    { 1, 189 },
                    { 1, 190 },
                    { 1, 191 },
                    { 1, 192 },
                    { 1, 193 },
                    { 1, 194 },
                    { 1, 195 },
                    { 1, 196 },
                    { 1, 197 },
                    { 1, 198 },
                    { 1, 199 },
                    { 1, 200 },
                    { 1, 201 },
                    { 1, 202 },
                    { 1, 203 },
                    { 1, 204 },
                    { 1, 205 },
                    { 1, 206 },
                    { 1, 207 },
                    { 1, 208 },
                    { 1, 209 },
                    { 1, 210 },
                    { 1, 211 },
                    { 1, 212 },
                    { 1, 213 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BcmCompletionHistory_GameId",
                table: "BcmCompletionHistory",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_BcmRgsc_PlayerId",
                table: "BcmRgsc",
                column: "PlayerId");

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
                name: "IX_PlayerCompletionHistory_GameId",
                table: "PlayerCompletionHistory",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCompletionHistory_PlayerId",
                table: "PlayerCompletionHistory",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerContests_PlayerId",
                table: "PlayerContests",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGames_PlayerId",
                table: "PlayerGames",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUserRole_UsersId",
                table: "UserUserRole",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BcmCompletionHistory");

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
                name: "PlayerCompletionHistory");

            migrationBuilder.DropTable(
                name: "PlayerContests");

            migrationBuilder.DropTable(
                name: "PlayerGames");

            migrationBuilder.DropTable(
                name: "SyncHistory");

            migrationBuilder.DropTable(
                name: "UserUserRole");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Contests");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
