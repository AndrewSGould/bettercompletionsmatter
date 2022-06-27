using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureLists_Games_FeatureListOfGameId",
                table: "FeatureLists");

            migrationBuilder.DropIndex(
                name: "IX_FeatureLists_FeatureListOfGameId",
                table: "FeatureLists");

            migrationBuilder.AlterColumn<int>(
                name: "FeatureListOfGameId",
                table: "FeatureLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: true);

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "Name", "Region", "TrueAchievementId" },
                values: new object[,]
                {
                    { 5, "Georgia", true, "SwiftSupafly", "United States", 691631 },
                    { 6, "New York", true, "DubstepEdgelord", "United States", 644155 },
                    { 7, null, true, "Infamous", "England", 518665 },
                    { 8, "California", true, "Luke17000", "United States", 1013881 },
                    { 9, "Kentucky", true, "lucas1987", "United States", 4838 },
                    { 10, null, true, "Fine Feat", "Austrailia", 695506 },
                    { 11, "Ontario", true, "smrnov", "Canada", 1815 },
                    { 12, "Georgia", true, "Sir Paulygon", "United States", 746750 },
                    { 13, "Victoria", true, "Rossco7530", "Austrailia", 385301 },
                    { 14, null, true, "True Veteran", "Austrailia", 992494 },
                    { 15, "New Zealand", true, "CasualExile", "New Zealand", 561724 },
                    { 16, "Wisconsin", true, "darkwing1232", "United States", 644384 },
                    { 17, "Suffolk", true, "JimbotUK", "England", 567743 },
                    { 18, "Hesse", true, "xMagicMunKix", "Germany", 312377 },
                    { 19, "New York", true, "Inferno118", "United States", 405939 },
                    { 20, "Hesse", true, "AC Rock3tman", "Germany", 503921 },
                    { 21, "Michigan", true, "tackleglass54", "United States", 585579 },
                    { 22, "Illinois", true, "IcyThrasher", "United States", 115320 },
                    { 23, "Texas", true, "TXMOOK", "United States", 1872 },
                    { 24, null, true, "BemusedBox", "England", 5540 },
                    { 25, null, true, "nuttywray", "England", 292068 },
                    { 26, "Missouri", true, "Christoph 5782", "United States", 441743 },
                    { 27, "Quebec", true, "Mtld", "Canada", 8962 },
                    { 28, "Virginia", true, "Lw N1GHTM4R3", "United States", 978641 },
                    { 29, "Nottinghamshire", true, "Erutaerc", "England", 75572 },
                    { 30, "Hungary", true, "WildWhiteNoise", "Hungary", 332637 },
                    { 31, "Rhineland-Palatinate", true, "GD GodSpeed", "Germany", 767838 },
                    { 32, null, true, "Legohead 1977", "England", 273370 },
                    { 33, "California", true, "Saurvivalist", "United States", 1001899 },
                    { 34, null, true, "UltimateDespair", "England", 386783 },
                    { 35, "Michigan", true, "radnonnahs", "United States", 483713 },
                    { 36, null, true, "Wakapeil", "Sweden", 316981 },
                    { 37, "North Carolina", true, "DaDuelingDonuts", "United States", 661240 },
                    { 38, "New Jersey", true, "Oriole2682", "United States", 251492 },
                    { 39, "Washington", true, "NBA Kirkland", "United States", 18165 },
                    { 40, "North Carolina", true, "Xx Phatryda xX", "United States", 540888 },
                    { 41, null, true, "Fista Roboto", "Austrailia", 514795 },
                    { 42, "Florida", true, "HegemonicHater", "United States", 431830 },
                    { 43, null, true, "Team Brether", "England", 109626 }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "Name", "Region", "TrueAchievementId" },
                values: new object[,]
                {
                    { 44, "Oklahoma", true, "Mattism", "United States", 17362 },
                    { 45, null, true, "Alyssiya", "England", 667139 },
                    { 46, "Georgia", true, "MajinFro", "United States", 389388 },
                    { 47, "Florida", true, "Whisperin Clown", "United States", 460875 },
                    { 48, "New Hampshire", true, "Mental Knight 5", "United States", 273989 },
                    { 49, null, true, "ChinDocta", "Austrailia", 473608 },
                    { 50, "Devon", true, "A1exRD", "England", 108134 },
                    { 51, "Washington", true, "Ethigy", "United States", 662432 },
                    { 52, "Rhineland-Palatinate", true, "Nichtl", "Germany", 596837 },
                    { 53, "Pennsylvania", true, "PangoBara", "United States", 507793 },
                    { 54, "Iowa", true, "HawkeyeBarry20", "United States", 548044 },
                    { 55, "New York", true, "KawiNinjaRider7", "United States", 636969 },
                    { 56, null, true, "BenL72", "England", 280034 },
                    { 57, "Minnesota", true, "Freamwhole", "United States", 276088 },
                    { 58, "Virginia", true, "Proulx", "United States", 78779 },
                    { 59, null, true, "zzUrbanSpaceman", "Austrailia", 60207 },
                    { 60, null, true, "A 0 E Monkey", "England", 27797 },
                    { 61, "Georgia", true, "Majinbro", "United States", 38595 },
                    { 62, "Connecticut", true, "Icefiretn", "United States", 126013 },
                    { 63, "Massachusetts", true, "LORDOFDOOKIE69", "United States", 130600 },
                    { 64, "North Carolina", true, "RetroChief1969", "United States", 685511 },
                    { 65, "California", true, "EldritchSS", "United States", 11497 },
                    { 66, "Tennessee", true, "rawkerdude", "United States", 25889 },
                    { 67, "Illinois", true, "iMaginaryy", "United States", 351310 },
                    { 68, "South Carolina", true, "Kez001", "United States", 15074 },
                    { 69, null, true, "WoodsMonk", "England", 46893 },
                    { 70, "Washington", true, "W1cked Girl", "United States", 391205 },
                    { 71, "Cardiff", true, "Kitty Skies", "Wales", 321249 },
                    { 72, "Connecticut", true, "MrGompers", "United States", 53128 },
                    { 73, "Netherlands", true, "GTKrouwel83", "Netherlands", 991656 },
                    { 74, "Kentucky", true, "CouchBurglar", "United States", 710980 },
                    { 75, "Virginia", true, "SprinkyDink", "United States", 391799 },
                    { 76, null, true, "boldyno1", "England", 57548 },
                    { 77, "Virginia", true, "FreakyRO", "United States", 389092 },
                    { 78, "Kansas", true, "hotcurls3088", "United States", 259643 },
                    { 79, "Georgia", true, "Kaiteh", "United States", 714727 },
                    { 80, null, true, "Yinga Garten", "United Kingdom", 107004 },
                    { 81, "Minnesota", true, "Skeptical Mario", "United States", 47628 },
                    { 82, "New Jersey", true, "WildwoodMike", "United States", 286696 },
                    { 83, "New Hampshire", true, "ChewieOnIce", "England", 115479 },
                    { 84, "Croatia", true, "Igneus DarkSide", "Croatia", 275395 },
                    { 85, "Ireland", true, "DavidMcC1989", "Ireland", 89301 }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "Name", "Region", "TrueAchievementId" },
                values: new object[,]
                {
                    { 86, "Pennsylvania", true, "RadicalSniper99", "United States", 378519 },
                    { 87, "Nevada", true, "Hotdogmcgee", "United States", 4860 },
                    { 88, null, true, "HenkeXD", "Sweden", 332575 },
                    { 89, "Maryland", true, "KooshMoose", "United States", 435315 },
                    { 90, "Virginia", true, "Xynvincible", "United States", 58478 },
                    { 91, "Wisconsin", true, "Whtthfgg", "United States", 365315 },
                    { 92, "Colorado", true, "PlayUltimate711", "United States", 125044 },
                    { 93, "British Columbia", true, "Seamus McLimey", "Canada", 119394 },
                    { 94, "Lancashire", true, "Northern Lass", "England", 40704 },
                    { 95, "California", true, "xNeo21x", "United States", 37540 },
                    { 96, null, true, "Redanian", "England", 401906 },
                    { 97, "New Jersey", true, "TBonePhone", "United States", 64912 },
                    { 98, "Lower Saxony", true, "IxGermanBeastxI", "Germany", 734459 },
                    { 99, "Georgia", true, "PhillipWendell", "United States", 78294 },
                    { 100, null, true, "C64 Mat", "England", 338344 },
                    { 101, null, true, "Benjii Redux", "Austrailia", 519817 },
                    { 102, null, true, "KATAKL1ZM", "England", 286468 },
                    { 103, null, true, "Mark B", "England", 408078 },
                    { 104, "Iowa", true, "Slayer Reigning", "United States", 82490 },
                    { 105, "Derbyshire", true, "xLAx JesteR", "England", 135630 },
                    { 106, "Ontario", true, "CrunchyGoblin68", "Canada", 450068 },
                    { 107, "Massachusetts", true, "SaucySlingo", "United States", 333080 },
                    { 108, "Lower Saxony", true, "III Torpedo III", "Germany", 94956 },
                    { 109, null, true, "Eliphelet77", "England", 78762 },
                    { 110, "Cardiff", true, "Lord Zell", "Wales", 434741 },
                    { 111, "Oregon", true, "Boda Yett", "United States", 907614 },
                    { 112, "Tennessee", true, "Not A Designer", "United States", 800715 },
                    { 113, null, true, "Muetschens", "Switzerland", 64293 },
                    { 114, "New York", true, "Big Ell", "United States", 64295 },
                    { 115, "New York", true, "K4rn4ge", "United States", 2898 },
                    { 116, "Texas", true, "bryan dot exe", "United States", 134205 },
                    { 117, "Slovenia", true, "Kaneman", "Slovenia", 410425 },
                    { 118, "Georgia", true, "DudeWithTheFace", "United States", 24047 },
                    { 119, "Indiana", true, "MadLefty2097", "United States", 684657 },
                    { 120, "California", true, "retstak", "United States", 447768 },
                    { 121, "Ohio", true, "Death Dealers", "United States", 615774 },
                    { 122, "New York", true, "Matrarch", "United States", 5581 },
                    { 123, "Pennsylvania", true, "omgeezus", "United States", 13608 },
                    { 124, null, true, "FlutteryChicken", "England", 7499 },
                    { 125, "Florida", true, "CarpeAdam79", "United States", 651526 },
                    { 126, null, true, "Hatton90", "England", 647169 },
                    { 127, "Indiana", true, "Triple Triad777", "United States", 538641 }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "Name", "Region", "TrueAchievementId" },
                values: new object[,]
                {
                    { 128, "Texas", true, "Buffs", "United States", 105056 },
                    { 129, "Pennsylvania", true, "BPBPBPBPBPBPBP", "United States", 38717 },
                    { 130, "West Virginia", true, "CHERRY CHEERIOS", "United States", 266948 },
                    { 131, "Jamaica", true, "DJB Hustlin", "Jamaica", 54140 },
                    { 132, "Ontario", true, "ITS ALivEx", "Canada", 328184 },
                    { 133, "New York", true, "TheAlphaSeagull", "United States", 893882 },
                    { 134, "Ontario", true, "mdp 73", "Canada", 1995 },
                    { 135, null, true, "ListlessDragon", "Switzerland", 533284 },
                    { 136, "Wisconsin", true, "WeezyFuzz", "United States", 435101 },
                    { 137, "Louisiana", true, "ILethalStang", "United States", 628247 },
                    { 138, "Ontario", true, "IrishWarrior022", "Canada", 392553 },
                    { 139, "Arizona", true, "General Tynstar", "United States", 6420 },
                    { 140, "British Columbia", true, "Ace", "Canada", 684086 },
                    { 141, "Ohio", true, "DirtyMcNasty126", "United States", 1022271 },
                    { 142, "Michigan", true, "Bsmittel", "United States", 439921 },
                    { 143, "Texas", true, "SKOOT2006", "United States", 387165 },
                    { 144, null, true, "Simpso", "England", 111184 },
                    { 145, "Illinois", true, "BlazeFlareon", "United States", 451792 },
                    { 146, null, true, "MrWolfw00d", "England", 375330 },
                    { 147, null, true, "AgileDuke", "England", 50502 },
                    { 148, "Wisconsin", true, "JeffMomm", "United States", 349158 },
                    { 149, null, true, "MattiasAnderson", "Sweden", 349605 },
                    { 150, "Colorado", true, "Kyleia", "United States", 711432 },
                    { 151, null, true, "Miller N7", "England", 896662 },
                    { 152, "North Carolina", true, "Fresh336669", "United States", 380552 },
                    { 153, "Suffolk", true, "MOT Astro", "England", 721181 },
                    { 154, "California", true, "Facial La Fleur", "United States", 4936 },
                    { 155, "Ohio", true, "AnonymousODB", "United States", 437038 },
                    { 156, "Maryland", true, "FuFuCuddilyPoof", "United States", 635863 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureLists_FeatureListOfGameId",
                table: "FeatureLists",
                column: "FeatureListOfGameId",
                unique: true,
                filter: "[FeatureListOfGameId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureLists_Games_FeatureListOfGameId",
                table: "FeatureLists",
                column: "FeatureListOfGameId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureLists_Games_FeatureListOfGameId",
                table: "FeatureLists");

            migrationBuilder.DropIndex(
                name: "IX_FeatureLists_FeatureListOfGameId",
                table: "FeatureLists");

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.AlterColumn<int>(
                name: "FeatureListOfGameId",
                table: "FeatureLists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureLists_FeatureListOfGameId",
                table: "FeatureLists",
                column: "FeatureListOfGameId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureLists_Games_FeatureListOfGameId",
                table: "FeatureLists",
                column: "FeatureListOfGameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
