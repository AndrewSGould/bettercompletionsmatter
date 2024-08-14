using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "games",
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
                    FullCompletionEstimate = table.Column<double>(type: "double precision", nullable: true),
                    ManuallyScored = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "synchistory",
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
                    table.PrimaryKey("PK_synchistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Xuid = table.Column<string>(type: "text", nullable: true),
                    Gamertag = table.Column<string>(type: "text", nullable: true),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    Area = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "featurelists",
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
                    table.PrimaryKey("PK_featurelists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_featurelists_games_FeatureListOfGameId",
                        column: x => x.FeatureListOfGameId,
                        principalTable: "games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "gamegenres",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false),
                    LastSync = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gamegenres", x => new { x.GameId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_gamegenres_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gamegenres_genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "discordlogins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    TokenType = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discordlogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_discordlogins_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "logins",
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
                    table.PrimaryKey("PK_logins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_logins_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TrueAchievementId = table.Column<int>(type: "integer", nullable: false),
                    LastSync = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_players_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userregistration",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userregistration", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_userregistration_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userrole",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userrole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_userrole_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userrole_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playergames",
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
                    table.PrimaryKey("PK_playergames", x => new { x.GameId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_playergames_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playergames_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "None" },
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
                    { 89, "Skydiving" },
                    { 90, "Music & Rhythm" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_discordlogins_UserId",
                table: "discordlogins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_featurelists_FeatureListOfGameId",
                table: "featurelists",
                column: "FeatureListOfGameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_gamegenres_GenreId",
                table: "gamegenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_logins_UserId",
                table: "logins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_playergames_GameId_PlayerId",
                table: "playergames",
                columns: new[] { "GameId", "PlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_playergames_PlayerId",
                table: "playergames",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_players_UserId",
                table: "players",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userrole_RoleId",
                table: "userrole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "discordlogins");

            migrationBuilder.DropTable(
                name: "featurelists");

            migrationBuilder.DropTable(
                name: "gamegenres");

            migrationBuilder.DropTable(
                name: "logins");

            migrationBuilder.DropTable(
                name: "playergames");

            migrationBuilder.DropTable(
                name: "synchistory");

            migrationBuilder.DropTable(
                name: "userregistration");

            migrationBuilder.DropTable(
                name: "userrole");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
