using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class PrefillYearlyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "YearlyChallenges",
                columns: new[] { "Id", "Category", "Description", "Title" },
                values: new object[,]
                {
                    { 2L, 0, "Complete any game in which you can pilot a spaceship", "Captain Kirkland" },
                    { 3L, 0, "Complete any game featuring scantily clad women (or shirtless men!)", "Dave's Buxom" },
                    { 4L, 0, "Complete any game where a character is able to use, or is portrayed as having used, the toilet", "Ex-lax Gesture" },
                    { 5L, 0, "Complete any game in which your character can consume the drugs", "He was Going to Design a Challenge, but Then He Got High" },
                    { 6L, 0, "Complete any game at or below a 2.5 star rating on TA, suckers", "I Don't Even Know You And I Hate You" },
                    { 7L, 0, "Complete any game tagged with the \"Metroidvania\" genre on TrueAchievements", "In the Vein of Nerdy Neo" },
                    { 8L, 0, "Complete any game that isn't in the Yakuza series", "It's Garbage" },
                    { 9L, 0, "Complete a game where you directly control an animal character", "llams dna taerG" },
                    { 10L, 0, "Complete any game that smrnov has completed throughout 2024", "Match It!" },
                    { 11L, 0, "Complete any game in which your character has a canine companion", "Min's Best Friend" },
                    { 12L, 0, "Complete any game with an achievement flagged \"Time Consuming\"", "Welldoneington Balbo" },
                    { 13L, 0, "Complete any game in which your character's mother is represented or mentioned in game", "My Mom Beat Zelda 2" },
                    { 14L, 0, "Complete any game that has a skin tone slider during character creation", "Pretty Fly for an Albino Kid" },
                    { 15L, 0, "Complete any game that has an achievement for accumulating currency", "Saben, of the Rothschilds" },
                    { 16L, 0, "Complete any game that has a character or creature with flames instead of hair", "Scorching Strands" },
                    { 17L, 0, "Complete any game that has been streamed on the official BCMX Twitch account", "Supafly Streamin'" },
                    { 18L, 0, "Complete any game that has a title featuring the letter string \"DD\"", "The Last of the Monacans" },
                    { 19L, 0, "Complete any game tagged with the \"Fighting\" genre on TrueAchievements", "Ultimate Despair vs. Capcom 3" },
                    { 20L, 0, "Complete any game that has a farming mechanic that is a major element in gameplay", "Welcome to Scardew Valley" },
                    { 21L, 1, "Complete any game of the same genre as your most completed genre", "The Curator" },
                    { 22L, 1, "Complete any game in the bottom 50% of your most completed genres", "The Explorer" },
                    { 23L, 1, "Complete any game with less than 5,000 tracked gamers and a rating over 4 on TrueAchievements", "The Connoisseur" },
                    { 24L, 1, "Complete any game with less than 1,000 tracked gamers", "The Hipster" },
                    { 25L, 1, "Complete any game that has a Site Rating of 4.25 or higher", "The Critic" },
                    { 26L, 1, "Complete any game that was completed by 3 other BCM participants this year", "The Socialite" },
                    { 27L, 1, "Complete any game with Cross-Play or the Xbox on Steam tag", "The Inclusionist" },
                    { 28L, 1, "Complete any game that ranks in the top 10% of your highest ratio completed games of all time", "The Professional" },
                    { 29L, 1, "Complete any game where your personal rating is 1.5 stars above or below the Site Average. You must rate the game on TA for this to count", "The Contrarian" },
                    { 30L, 1, "Complete any game with less than 100 completions", "The Pioneer" },
                    { 31L, 1, "Complete any game with more than 7,500 tracked gamers", "The Conformist" },
                    { 32L, 1, "Complete any game with an install size of under 200MB", "The Minimalist" },
                    { 33L, 1, "Complete any game with an install size of over 30GB", "The Hedonist" },
                    { 34L, 1, "Complete any game that has exactly 50 achievements total", "The Traditionalist" },
                    { 35L, 1, "Complete any game that has exactly 12 achievements total", "The Arcade Traditionalist" },
                    { 36L, 1, "Complete any game where the title contains 14 or more unique letters", "The Orthographer" },
                    { 37L, 1, "Complete any game that was released the year you joined Xbox Live. Are you ready to traverse history?", "The Training Arc: The Calendar Project" },
                    { 38L, 1, "Complete any game with 4 or more genres. Are you ready to collect them all?", "The Training Arc: The Genre Project" },
                    { 39L, 1, "Complete any game that includes an achievement flagged \"versus\", or \"online skill\". Are you ready to throw down?", "The Training Arc: Head-to-Head" },
                    { 40L, 1, "Complete any Xbox 360 game over 100 hours. Are you ready for the next battle?", "The Training Arc: Raid Boss" },
                    { 41L, 2, "Complete any game after it's already left Game Pass", "Game Passed" },
                    { 42L, 2, "Complete any game with 10 or more achievements flagged missable on TrueAchievements", "Hope There is a Guide" },
                    { 43L, 2, "Complete any game with 1 cutscene that is at least 10 minutes or longer", "I Didn't Know that I Was Watching a Movie" },
                    { 44L, 2, "Complete any game with an achievement for cumulative play time", "Leave Your Xbox On" },
                    { 45L, 2, "Complete any game where your character has access to mind control abilities", "Open Minded" },
                    { 46L, 2, "Complete any game where you can throw a rock to distract enemies (it must be a distraction mechanic, not an attack mechanic)", "Pet Rock" },
                    { 47L, 2, "Complete any game published on Xbox by Prime Matter", "Prime Completions Matter" },
                    { 48L, 2, "Complete any game where your character can wear a gasmask or hazmat suit", "Radiation Celebration" },
                    { 49L, 2, "Complete any game where a character says the name of the game in the dialogue", "They Said The Thing" },
                    { 50L, 2, "Complete any game with six or more words in the title", "What a Mouthful" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 42L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 45L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 46L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 47L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 48L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "YearlyChallenges",
                keyColumn: "Id",
                keyValue: 50L);
        }
    }
}
