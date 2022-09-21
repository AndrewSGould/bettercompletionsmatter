using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class InitialRecreate : Migration
    {
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

            migrationBuilder.InsertData(
                table: "Contests",
                columns: new[] { "Id", "EndDate", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Better Completions Matter", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2022, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Raid Boss", new DateTime(2022, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
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
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "LastSync", "Name", "Region", "TrueAchievementId" },
                values: new object[,]
                {
                    { 1, "Ohio", true, null, "kT Echo", "United States", 104571 },
                    { 2, "Ohio", true, null, "eohjay", "United States", 266752 },
                    { 3, "Ohio", true, null, "IronFistofSnuff", "United States", 405202 },
                    { 4, null, false, null, "zzScanMan1", null, 461682 },
                    { 5, "Georgia", true, null, "SwiftSupafly", "United States", 691631 },
                    { 6, "New York", true, null, "DubstepEdgelord", "United States", 644155 },
                    { 7, null, true, null, "Infamous", "England", 518665 },
                    { 8, "California", true, null, "Luke17000", "United States", 1013881 },
                    { 9, "Kentucky", true, null, "lucas1987", "United States", 4838 },
                    { 10, null, true, null, "Fine Feat", "Austrailia", 695506 },
                    { 11, "Ontario", true, null, "smrnov", "Canada", 1815 },
                    { 12, "Georgia", true, null, "Sir Paulygon", "United States", 746750 },
                    { 13, "Victoria", true, null, "Rossco7530", "Austrailia", 385301 },
                    { 14, null, true, null, "True Veteran", "Austrailia", 992494 },
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
                    { 28, "Virginia", true, null, "Lw N1GHTM4R3", "United States", 978641 },
                    { 29, "Nottinghamshire", true, null, "Erutaerc", "England", 75572 },
                    { 30, "Hungary", true, null, "WildWhiteNoise", "Hungary", 332637 },
                    { 31, "Rhineland-Palatinate", true, null, "GD GodSpeed", "Germany", 767838 },
                    { 32, null, true, null, "Legohead 1977", "England", 273370 },
                    { 33, "California", true, null, "Saurvivalist", "United States", 1001899 },
                    { 34, null, true, null, "UltimateDespair", "England", 386783 },
                    { 35, "Michigan", true, null, "radnonnahs", "United States", 483713 },
                    { 36, null, true, null, "Wakapeil", "Sweden", 316981 },
                    { 37, "North Carolina", true, null, "DaDuelingDonuts", "United States", 661240 },
                    { 38, "New Jersey", true, null, "Oriole2682", "United States", 251492 },
                    { 39, "Washington", true, null, "NBA Kirkland", "United States", 18165 },
                    { 40, "North Carolina", true, null, "Xx Phatryda xX", "United States", 540888 },
                    { 41, null, true, null, "Fista Roboto", "Austrailia", 514795 },
                    { 42, "Florida", true, null, "HegemonicHater", "United States", 431830 },
                    { 43, null, true, null, "Team Brether", "England", 109626 },
                    { 44, "Oklahoma", true, null, "Mattism", "United States", 17362 },
                    { 45, null, true, null, "Alyssiya", "England", 667139 },
                    { 46, "Georgia", true, null, "MajinFro", "United States", 389388 },
                    { 47, "Florida", true, null, "Whisperin Clown", "United States", 460875 },
                    { 48, "New Hampshire", true, null, "Mental Knight 5", "United States", 273989 },
                    { 49, null, true, null, "ChinDocta", "Austrailia", 473608 },
                    { 50, "Devon", true, null, "A1exRD", "England", 108134 },
                    { 51, "Washington", true, null, "Ethigy", "United States", 662432 },
                    { 52, "Rhineland-Palatinate", true, null, "Nichtl", "Germany", 596837 },
                    { 53, "Pennsylvania", true, null, "PangoBara", "United States", 507793 },
                    { 54, "Iowa", true, null, "HawkeyeBarry20", "United States", 548044 },
                    { 55, "New York", true, null, "KawiNinjaRider7", "United States", 636969 },
                    { 56, null, true, null, "BenL72", "England", 280034 },
                    { 57, "Minnesota", true, null, "Freamwhole", "United States", 276088 },
                    { 58, "Virginia", true, null, "Proulx", "United States", 78779 },
                    { 59, null, true, null, "zzUrbanSpaceman", "Austrailia", 60207 },
                    { 60, null, true, null, "A 0 E Monkey", "England", 27797 },
                    { 61, "Georgia", true, null, "Majinbro", "United States", 38595 },
                    { 62, "Connecticut", true, null, "Icefiretn", "United States", 126013 },
                    { 63, "Massachusetts", true, null, "LORDOFDOOKIE69", "United States", 130600 },
                    { 64, "North Carolina", true, null, "RetroChief1969", "United States", 685511 },
                    { 65, "California", true, null, "EldritchSS", "United States", 11497 },
                    { 66, "Tennessee", true, null, "rawkerdude", "United States", 25889 },
                    { 67, "Illinois", true, null, "iMaginaryy", "United States", 351310 },
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
                    { 81, "Minnesota", true, null, "Skeptical Mario", "United States", 47628 },
                    { 82, "New Jersey", true, null, "WildwoodMike", "United States", 286696 },
                    { 83, "New Hampshire", true, null, "ChewieOnIce", "England", 115479 },
                    { 84, "Croatia", true, null, "Igneus DarkSide", "Croatia", 275395 },
                    { 85, "Ireland", true, null, "DavidMcC1989", "Ireland", 89301 },
                    { 86, "Pennsylvania", true, null, "RadicalSniper99", "United States", 378519 },
                    { 87, "Nevada", true, null, "Hotdogmcgee", "United States", 4860 },
                    { 88, null, true, null, "HenkeXD", "Sweden", 332575 },
                    { 89, "Maryland", true, null, "KooshMoose", "United States", 435315 },
                    { 90, "Virginia", true, null, "Xynvincible", "United States", 58478 },
                    { 91, "Wisconsin", true, null, "Whtthfgg", "United States", 365315 },
                    { 92, "Colorado", true, null, "PlayUltimate711", "United States", 125044 },
                    { 93, "British Columbia", true, null, "Seamus McLimey", "Canada", 119394 },
                    { 94, "Lancashire", true, null, "Northern Lass", "England", 40704 },
                    { 95, "California", true, null, "xNeo21x", "United States", 37540 },
                    { 96, null, true, null, "Redanian", "England", 401906 },
                    { 97, "New Jersey", true, null, "TBonePhone", "United States", 64912 },
                    { 98, "Lower Saxony", true, null, "IxGermanBeastxI", "Germany", 734459 },
                    { 99, "Georgia", true, null, "PhillipWendell", "United States", 78294 },
                    { 100, null, true, null, "C64 Mat", "England", 338344 },
                    { 101, null, true, null, "Benjii Redux", "Austrailia", 519817 },
                    { 102, null, true, null, "KATAKL1ZM", "England", 286468 },
                    { 103, null, true, null, "Mark B", "England", 408078 },
                    { 104, "Iowa", true, null, "Slayer Reigning", "United States", 82490 },
                    { 105, "Derbyshire", true, null, "xLAx JesteR", "England", 135630 },
                    { 106, "Ontario", true, null, "CrunchyGoblin68", "Canada", 450068 },
                    { 107, "Massachusetts", true, null, "SaucySlingo", "United States", 333080 },
                    { 108, "Lower Saxony", true, null, "III Torpedo III", "Germany", 94956 },
                    { 109, null, true, null, "Eliphelet77", "England", 78762 },
                    { 110, "Cardiff", true, null, "Lord Zell", "Wales", 434741 },
                    { 111, "Oregon", true, null, "Boda Yett", "United States", 907614 },
                    { 112, "Tennessee", true, null, "Not A Designer", "United States", 800715 },
                    { 113, null, true, null, "Muetschens", "Switzerland", 64293 },
                    { 114, "New York", true, null, "Big Ell", "United States", 64295 },
                    { 115, "New York", true, null, "K4rn4ge", "United States", 2898 },
                    { 116, "Texas", true, null, "bryan dot exe", "United States", 134205 },
                    { 117, "Slovenia", true, null, "Kaneman", "Slovenia", 410425 },
                    { 118, "Georgia", true, null, "DudeWithTheFace", "United States", 24047 },
                    { 119, "Indiana", true, null, "MadLefty2097", "United States", 684657 },
                    { 120, "California", true, null, "retstak", "United States", 447768 },
                    { 121, "Ohio", true, null, "Death Dealers", "United States", 615774 },
                    { 122, "New York", true, null, "Matrarch", "United States", 5581 },
                    { 123, "Pennsylvania", true, null, "omgeezus", "United States", 13608 },
                    { 124, null, true, null, "FlutteryChicken", "England", 7499 },
                    { 125, "Florida", true, null, "CarpeAdam79", "United States", 651526 },
                    { 126, null, true, null, "Hatton90", "England", 647169 },
                    { 127, "Indiana", true, null, "Triple Triad777", "United States", 538641 },
                    { 128, "Texas", true, null, "Buffs", "United States", 105056 },
                    { 129, "Pennsylvania", true, null, "BPBPBPBPBPBPBP", "United States", 38717 },
                    { 130, "West Virginia", true, null, "CHERRY CHEERIOS", "United States", 266948 },
                    { 131, "Jamaica", true, null, "DJB Hustlin", "Jamaica", 54140 },
                    { 132, "Ontario", true, null, "ITS ALivEx", "Canada", 328184 },
                    { 133, "New York", true, null, "TheAlphaSeagull", "United States", 893882 },
                    { 134, "Ontario", true, null, "mdp 73", "Canada", 1995 },
                    { 135, null, true, null, "ListlessDragon", "Switzerland", 533284 },
                    { 136, "Wisconsin", true, null, "WeezyFuzz", "United States", 435101 },
                    { 137, "Louisiana", true, null, "ILethalStang", "United States", 628247 },
                    { 138, "Ontario", true, null, "IrishWarrior022", "Canada", 392553 },
                    { 139, "Arizona", true, null, "General Tynstar", "United States", 6420 },
                    { 140, "British Columbia", true, null, "Ace", "Canada", 684086 },
                    { 141, "Ohio", true, null, "DirtyMcNasty126", "United States", 1022271 },
                    { 142, "Michigan", true, null, "Bsmittel", "United States", 439921 },
                    { 143, "Texas", true, null, "SKOOT2006", "United States", 387165 },
                    { 144, null, true, null, "Simpso", "England", 111184 },
                    { 145, "Illinois", true, null, "BlazeFlareon", "United States", 451792 },
                    { 146, null, true, null, "MrWolfw00d", "England", 375330 },
                    { 147, null, true, null, "AgileDuke", "England", 50502 },
                    { 148, "Wisconsin", true, null, "JeffMomm", "United States", 349158 },
                    { 149, null, true, null, "MattiasAnderson", "Sweden", 349605 },
                    { 150, "Colorado", true, null, "Kyleia", "United States", 711432 },
                    { 151, null, true, null, "Miller N7", "England", 896662 },
                    { 152, "North Carolina", true, null, "Fresh336669", "United States", 380552 },
                    { 153, "Suffolk", true, null, "MOT Astro", "England", 721181 },
                    { 154, "California", true, null, "Facial La Fleur", "United States", 4936 },
                    { 155, "Ohio", true, null, "AnonymousODB", "United States", 437038 },
                    { 156, "Maryland", true, null, "FuFuCuddilyPoof", "United States", 635863 },
                    { 157, null, true, null, "xI The Rock Ix", null, 408827 },
                    { 158, null, true, null, "tatersoup19", null, 85256 },
                    { 159, null, true, null, "nightw0lf", null, 347191 },
                    { 160, null, true, null, "N龱T廾 T廾A G龱D", null, 1725 },
                    { 161, null, true, null, "emz fergi", null, 702307 },
                    { 162, null, true, null, "Inigomontoya80", null, 262143 },
                    { 163, null, true, null, "PRTM CLUESCROL", null, 597081 },
                    { 164, null, true, null, "AZ Mongoose", null, 48289 },
                    { 165, null, true, null, "Vulgar Latin", null, 76517 },
                    { 166, null, true, null, "TobyLinn", null, 643897 },
                    { 167, null, true, null, "Jblacq", null, 52223 },
                    { 168, null, true, null, "Enigma Gamer 77", null, 370170 },
                    { 169, null, true, null, "Xpovos", null, 318602 },
                    { 170, null, true, null, "Shadow", null, 409281 },
                    { 171, null, true, null, "Ahayzo", null, 276943 },
                    { 172, null, true, null, "Darklord Davis", null, 364130 },
                    { 173, null, true, null, "logicslayer", null, 97393 },
                    { 174, null, true, null, "HyRoad V2", null, 723406 },
                    { 175, null, true, null, "GT3OptionFan", null, 257340 },
                    { 176, null, true, null, "kungfuskills", null, 357761 }
                });

            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 },
                    { 1, 8 },
                    { 1, 9 },
                    { 1, 10 },
                    { 1, 11 },
                    { 1, 12 },
                    { 1, 13 },
                    { 1, 14 },
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
                    { 1, 28 },
                    { 1, 29 },
                    { 1, 30 },
                    { 1, 31 },
                    { 1, 32 },
                    { 1, 33 },
                    { 1, 34 },
                    { 1, 35 },
                    { 1, 36 },
                    { 1, 37 },
                    { 1, 38 },
                    { 1, 39 },
                    { 1, 40 },
                    { 1, 41 },
                    { 1, 42 },
                    { 1, 43 },
                    { 1, 44 },
                    { 1, 45 },
                    { 1, 46 },
                    { 1, 47 },
                    { 1, 48 },
                    { 1, 49 },
                    { 1, 50 },
                    { 1, 51 },
                    { 1, 52 },
                    { 1, 53 },
                    { 1, 54 },
                    { 1, 55 },
                    { 1, 56 },
                    { 1, 57 },
                    { 1, 58 },
                    { 1, 59 },
                    { 1, 60 },
                    { 1, 61 },
                    { 1, 62 },
                    { 1, 63 },
                    { 1, 64 },
                    { 1, 65 },
                    { 1, 66 },
                    { 1, 67 },
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
                    { 1, 81 },
                    { 1, 82 },
                    { 1, 83 },
                    { 1, 84 },
                    { 1, 85 },
                    { 1, 86 },
                    { 1, 87 },
                    { 1, 88 },
                    { 1, 89 },
                    { 1, 90 },
                    { 1, 91 },
                    { 1, 92 },
                    { 1, 93 },
                    { 1, 94 },
                    { 1, 95 },
                    { 1, 96 },
                    { 1, 97 },
                    { 1, 98 },
                    { 1, 99 },
                    { 1, 100 },
                    { 1, 101 },
                    { 1, 102 },
                    { 1, 103 },
                    { 1, 104 },
                    { 1, 105 },
                    { 1, 106 },
                    { 1, 107 },
                    { 1, 108 },
                    { 1, 109 },
                    { 1, 110 },
                    { 1, 111 },
                    { 1, 112 },
                    { 1, 113 },
                    { 1, 114 },
                    { 1, 115 },
                    { 1, 116 },
                    { 1, 117 },
                    { 1, 118 },
                    { 1, 119 },
                    { 1, 120 },
                    { 1, 121 },
                    { 1, 122 },
                    { 1, 123 },
                    { 1, 124 },
                    { 1, 125 },
                    { 1, 126 },
                    { 1, 127 },
                    { 1, 128 },
                    { 1, 129 },
                    { 1, 130 },
                    { 1, 131 },
                    { 1, 132 },
                    { 1, 133 },
                    { 1, 134 },
                    { 1, 135 },
                    { 1, 136 },
                    { 1, 137 },
                    { 1, 138 },
                    { 1, 139 },
                    { 1, 140 },
                    { 1, 141 },
                    { 1, 142 },
                    { 1, 143 },
                    { 1, 144 },
                    { 1, 145 },
                    { 1, 146 },
                    { 1, 147 },
                    { 1, 148 },
                    { 1, 149 },
                    { 1, 150 },
                    { 1, 151 },
                    { 1, 152 },
                    { 1, 153 },
                    { 1, 154 },
                    { 1, 155 },
                    { 1, 156 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 7 },
                    { 2, 8 },
                    { 2, 9 },
                    { 2, 11 },
                    { 2, 12 },
                    { 2, 17 },
                    { 2, 22 },
                    { 2, 29 },
                    { 2, 32 },
                    { 2, 36 },
                    { 2, 39 },
                    { 2, 48 },
                    { 2, 49 },
                    { 2, 50 },
                    { 2, 54 },
                    { 2, 56 },
                    { 2, 59 },
                    { 2, 62 },
                    { 2, 64 },
                    { 2, 66 },
                    { 2, 72 },
                    { 2, 77 },
                    { 2, 81 },
                    { 2, 83 },
                    { 2, 89 },
                    { 2, 91 },
                    { 2, 93 },
                    { 2, 94 },
                    { 2, 95 },
                    { 2, 105 },
                    { 2, 106 },
                    { 2, 114 },
                    { 2, 118 },
                    { 2, 119 },
                    { 2, 120 },
                    { 2, 122 },
                    { 2, 124 },
                    { 2, 126 },
                    { 2, 132 },
                    { 2, 134 },
                    { 2, 136 },
                    { 2, 140 },
                    { 2, 144 },
                    { 2, 157 },
                    { 2, 158 },
                    { 2, 159 },
                    { 2, 160 },
                    { 2, 161 },
                    { 2, 162 },
                    { 2, 163 },
                    { 2, 164 },
                    { 2, 165 },
                    { 2, 166 },
                    { 2, 167 },
                    { 2, 168 },
                    { 2, 169 },
                    { 2, 170 },
                    { 2, 171 },
                    { 2, 172 },
                    { 2, 173 },
                    { 2, 174 },
                    { 2, 175 },
                    { 2, 176 }
                });

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
                name: "IX_PlayerContests_PlayerId",
                table: "PlayerContests",
                column: "PlayerId");

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
                name: "PlayerContests");

            migrationBuilder.DropTable(
                name: "PlayerGames");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Contests");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
