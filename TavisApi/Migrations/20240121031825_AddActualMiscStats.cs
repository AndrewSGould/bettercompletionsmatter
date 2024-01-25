using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class AddActualMiscStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BcmMiscStats",
                columns: new[] { "Id", "HistoricalStats", "PlayerId" },
                values: new object[,]
                {
                    { 1L, "[{\"Year\":2022,\"Rgsc\":8,\"FullCombo\":false,\"Placement\":31},{\"Year\":2023,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":71}]", 76L },
                    { 2L, "[{\"Year\":2020,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":7},{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":40},{\"Year\":2022,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":93},{\"Year\":2023,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":73}]", 53L },
                    { 3L, "[{\"Year\":2023,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":70}]", 152L },
                    { 4L, "[{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":119},{\"Year\":2023,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":82}]", 25L },
                    { 5L, "[{\"Year\":2021,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":31}]", 155L },
                    { 6L, "[{\"Year\":2023,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":22}]", 27L },
                    { 7L, "[{\"Year\":2020,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":9},{\"Year\":2021,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":41},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":34},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":26}]", 39L },
                    { 8L, "[{\"Year\":2018,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":73},{\"Year\":2019,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":69},{\"Year\":2020,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":57},{\"Year\":2021,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":74},{\"Year\":2022,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":104},{\"Year\":2023,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":90}]", 101L },
                    { 9L, "[{\"Year\":2020,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":28},{\"Year\":2021,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":95},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":147},{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":139}]", 123L },
                    { 10L, "[{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":67},{\"Year\":2022,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":132}]", 148L },
                    { 11L, "[{\"Year\":2022,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":21},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":56}]", 29L },
                    { 12L, "[{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":117}]", 142L },
                    { 13L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":21},{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":26},{\"Year\":2020,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":3},{\"Year\":2021,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":8},{\"Year\":2022,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":17},{\"Year\":2023,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":102}]", 50L },
                    { 14L, "[{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":101},{\"Year\":2022,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":85},{\"Year\":2023,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":119}]", 163L },
                    { 15L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":4},{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":33}]", 57L },
                    { 16L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":55},{\"Year\":2019,\"Rgsc\":7,\"FullCombo\":false,\"Placement\":50},{\"Year\":2020,\"Rgsc\":10,\"FullCombo\":false,\"Placement\":54},{\"Year\":2021,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":86},{\"Year\":2022,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":78},{\"Year\":2023,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":103}]", 143L },
                    { 17L, "[{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":6}]", 87L },
                    { 18L, "[{\"Year\":2020,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":40},{\"Year\":2021,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":28},{\"Year\":2022,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":25},{\"Year\":2023,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":64}]", 32L },
                    { 19L, "[{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":32}]", 22L },
                    { 20L, "[{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":114},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":126}]", 49L },
                    { 21L, "[{\"Year\":2020,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":11},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":46}]", 10L },
                    { 22L, "[{\"Year\":2021,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":2},{\"Year\":2022,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":4}]", 156L },
                    { 23L, "[{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":121}]", 140L },
                    { 24L, "[{\"Year\":2019,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":57},{\"Year\":2020,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":49},{\"Year\":2021,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":82},{\"Year\":2022,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":94},{\"Year\":2023,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":40}]", 38L },
                    { 25L, "[{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":42}]", 8L },
                    { 26L, "[{\"Year\":2020,\"Rgsc\":7,\"FullCombo\":false,\"Placement\":12},{\"Year\":2021,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":5},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":8},{\"Year\":2023,\"Rgsc\":9,\"FullCombo\":false,\"Placement\":17}]", 3L },
                    { 27L, "[{\"Year\":2021,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":7},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":16},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":47}]", 16L },
                    { 28L, "[{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":1},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":3},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":2}]", 134L },
                    { 29L, "[{\"Year\":2021,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":52},{\"Year\":2022,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":38},{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":20}]", 28L },
                    { 30L, "[{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":107},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":92}]", 85L },
                    { 31L, "[{\"Year\":2022,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":75},{\"Year\":2023,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":101}]", 92L },
                    { 32L, "[{\"Year\":2019,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":34},{\"Year\":2020,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":27},{\"Year\":2021,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":59},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":35},{\"Year\":2023,\"Rgsc\":8,\"FullCombo\":false,\"Placement\":58}]", 84L },
                    { 33L, "[{\"Year\":2020,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":83},{\"Year\":2021,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":97},{\"Year\":2022,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":128},{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":130}]", 75L },
                    { 34L, "[{\"Year\":2020,\"Rgsc\":9,\"FullCombo\":false,\"Placement\":30},{\"Year\":2021,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":65},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":46},{\"Year\":2023,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":76}]", 35L },
                    { 35L, "[{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":15},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":5}]", 5L },
                    { 36L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":19},{\"Year\":2019,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":36},{\"Year\":2020,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":36},{\"Year\":2021,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":64},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":61},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":51}]", 14L },
                    { 37L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":72},{\"Year\":2019,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":43},{\"Year\":2020,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":35},{\"Year\":2021,\"Rgsc\":7,\"FullCombo\":false,\"Placement\":75},{\"Year\":2022,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":96},{\"Year\":2023,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":112}]", 30L },
                    { 38L, "[{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":19},{\"Year\":2023,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":38}]", 15L },
                    { 39L, "[{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":7},{\"Year\":2020,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":6},{\"Year\":2021,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":3},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":5},{\"Year\":2023,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":15}]", 21L },
                    { 40L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":83},{\"Year\":2020,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":18},{\"Year\":2022,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":29},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":50}]", 58L },
                    { 41L, "[{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":81},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":142}]", 124L },
                    { 42L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":24},{\"Year\":2019,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":12},{\"Year\":2020,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":16},{\"Year\":2021,\"Rgsc\":7,\"FullCombo\":false,\"Placement\":50},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":23},{\"Year\":2023,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":57}]", 12L },
                    { 43L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":26},{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":52},{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":69},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":69}]", 81L },
                    { 44L, "[{\"Year\":2019,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":10},{\"Year\":2020,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":2},{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":39},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":43},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":84}]", 56L },
                    { 45L, "[{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":135}]", 133L },
                    { 46L, "[{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":43},{\"Year\":2022,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":55},{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":80}]", 34L },
                    { 47L, "[{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":62},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":69},{\"Year\":2023,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":81}]", 18L },
                    { 48L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":62},{\"Year\":2019,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":35},{\"Year\":2020,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":60},{\"Year\":2021,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":93},{\"Year\":2022,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":92},{\"Year\":2023,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":97}]", 73L },
                    { 49L, "[{\"Year\":2023,\"Rgsc\":10,\"FullCombo\":false,\"Placement\":87}]", 141L },
                    { 50L, "[{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":38},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":76},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":61}]", 105L },
                    { 51L, "[{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":59},{\"Year\":2020,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":80},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":84},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":62}]", 43L },
                    { 52L, "[{\"Year\":2020,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":25},{\"Year\":2021,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":13},{\"Year\":2022,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":6},{\"Year\":2023,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":12}]", 46L },
                    { 53L, "[{\"Year\":2021,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":51},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":1},{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":29}]", 93L },
                    { 54L, "[{\"Year\":2020,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":10},{\"Year\":2021,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":29},{\"Year\":2022,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":105},{\"Year\":2023,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":77}]", 48L },
                    { 55L, "[{\"Year\":2018,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":46},{\"Year\":2019,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":64},{\"Year\":2020,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":20},{\"Year\":2021,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":46},{\"Year\":2022,\"Rgsc\":7,\"FullCombo\":false,\"Placement\":112},{\"Year\":2023,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":109}]", 100L },
                    { 56L, "[{\"Year\":2023,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":30}]", 69L },
                    { 57L, "[{\"Year\":2019,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":70},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":44}]", 86L },
                    { 58L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":69},{\"Year\":2019,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":30},{\"Year\":2021,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":89},{\"Year\":2023,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":67}]", 19L },
                    { 59L, "[{\"Year\":2020,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":77},{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":122},{\"Year\":2022,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":80},{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":132}]", 68L },
                    { 60L, "[{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":79},{\"Year\":2020,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":70},{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":91},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":89},{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":104}]", 63L },
                    { 61L, "[{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":19}]", 33L },
                    { 62L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":42},{\"Year\":2019,\"Rgsc\":7,\"FullCombo\":false,\"Placement\":11},{\"Year\":2020,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":8},{\"Year\":2021,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":54},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":18},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":59}]", 44L },
                    { 63L, "[{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":66},{\"Year\":2020,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":44},{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":61},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":65},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":68}]", 36L },
                    { 64L, "[{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":39},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":27}]", 77L },
                    { 65L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":90},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":111}]", 59L },
                    { 66L, "[{\"Year\":2019,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":18},{\"Year\":2023,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":14}]", 47L },
                    { 67L, "[{\"Year\":2019,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":19},{\"Year\":2020,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":22},{\"Year\":2021,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":55},{\"Year\":2022,\"Rgsc\":10,\"FullCombo\":false,\"Placement\":56},{\"Year\":2023,\"Rgsc\":9,\"FullCombo\":false,\"Placement\":48}]", 62L },
                    { 68L, "[{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":156},{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":37}]", 7L },
                    { 69L, "[{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":43}]", 91L },
                    { 70L, "[{\"Year\":2019,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":21},{\"Year\":2020,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":63},{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":67},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":87},{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":63}]", 89L },
                    { 71L, "[{\"Year\":2022,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":124},{\"Year\":2023,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":83}]", 51L },
                    { 72L, "[{\"Year\":2022,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":24}]", 146L },
                    { 73L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":27},{\"Year\":2019,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":3},{\"Year\":2020,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":14},{\"Year\":2021,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":30},{\"Year\":2023,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":74}]", 24L },
                    { 74L, "[{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":130},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":32},{\"Year\":2023,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":35}]", 64L },
                    { 75L, "[{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":7}]", 6L },
                    { 76L, "[{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":68},{\"Year\":2022,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":108},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":133}]", 145L },
                    { 77L, "[{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":79}]", 96L },
                    { 78L, "[{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":112},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":143},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":128}]", 41L },
                    { 79L, "[{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":20},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":11},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":49}]", 60L },
                    { 80L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":92},{\"Year\":2019,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":4},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":64}]", 78L },
                    { 81L, "[{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":104},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":138},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":141}]", 103L },
                    { 82L, "[{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":82},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":65}]", 23L },
                    { 83L, "[{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":34}]", 144L },
                    { 84L, "[{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":2},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":8}]", 20L },
                    { 85L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":1},{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":91},{\"Year\":2020,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":46},{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":80},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":83},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":106}]", 70L },
                    { 86L, "[{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":10},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":3}]", 26L },
                    { 87L, "[{\"Year\":2019,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":42},{\"Year\":2020,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":58},{\"Year\":2021,\"Rgsc\":10,\"FullCombo\":false,\"Placement\":53},{\"Year\":2023,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":94}]", 72L },
                    { 88L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":97},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":129},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":25}]", 55L },
                    { 89L, "[{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":9}]", 97L },
                    { 90L, "[{\"Year\":2019,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":62},{\"Year\":2020,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":45},{\"Year\":2021,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":32},{\"Year\":2022,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":12},{\"Year\":2023,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":21}]", 45L },
                    { 91L, "[{\"Year\":2021,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":16},{\"Year\":2022,\"Rgsc\":11,\"FullCombo\":false,\"Placement\":53},{\"Year\":2023,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":13}]", 61L },
                    { 92L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":8},{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":89},{\"Year\":2020,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":47},{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":118},{\"Year\":2022,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":91},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":142}]", 165L },
                    { 93L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":45}]", 54L },
                    { 94L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":3}]", 135L },
                    { 95L, "[{\"Year\":2020,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":75},{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":76},{\"Year\":2022,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":136},{\"Year\":2023,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":53}]", 65L },
                    { 96L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":38},{\"Year\":2019,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":85},{\"Year\":2023,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":33}]", 37L },
                    { 97L, "[{\"Year\":2020,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":41},{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":58},{\"Year\":2022,\"Rgsc\":8,\"FullCombo\":false,\"Placement\":41},{\"Year\":2023,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":45}]", 88L },
                    { 98L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":98},{\"Year\":2019,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":45},{\"Year\":2020,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":48},{\"Year\":2021,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":100},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":73},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":123}]", 79L },
                    { 99L, "[{\"Year\":2018,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":17},{\"Year\":2019,\"Rgsc\":10,\"FullCombo\":false,\"Placement\":14},{\"Year\":2020,\"Rgsc\":7,\"FullCombo\":false,\"Placement\":26},{\"Year\":2021,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":25},{\"Year\":2022,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":37},{\"Year\":2023,\"Rgsc\":5,\"FullCombo\":false,\"Placement\":18}]", 17L },
                    { 100L, "[{\"Year\":2019,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":54},{\"Year\":2020,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":52},{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":72},{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":97},{\"Year\":2023,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":86}]", 52L },
                    { 101L, "[{\"Year\":2022,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":90},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":93}]", 161L },
                    { 102L, "[{\"Year\":2019,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":74},{\"Year\":2020,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":79},{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":98},{\"Year\":2022,\"Rgsc\":8,\"FullCombo\":false,\"Placement\":102},{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":137}]", 42L },
                    { 103L, "[{\"Year\":2020,\"Rgsc\":6,\"FullCombo\":false,\"Placement\":17},{\"Year\":2021,\"Rgsc\":3,\"FullCombo\":false,\"Placement\":84},{\"Year\":2022,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":70},{\"Year\":2023,\"Rgsc\":4,\"FullCombo\":false,\"Placement\":78}]", 83L },
                    { 104L, "[{\"Year\":2020,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":53},{\"Year\":2021,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":94},{\"Year\":2022,\"Rgsc\":2,\"FullCombo\":false,\"Placement\":88},{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":99}]", 71L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 42L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 45L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 46L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 47L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 48L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 50L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 51L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 52L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 53L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 54L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 55L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 56L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 57L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 58L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 59L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 60L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 61L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 62L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 63L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 64L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 65L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 66L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 67L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 68L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 69L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 70L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 71L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 72L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 73L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 74L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 75L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 76L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 77L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 78L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 79L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 80L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 81L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 82L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 83L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 84L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 85L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 86L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 87L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 88L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 89L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 90L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 91L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 92L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 93L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 94L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 95L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 96L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 97L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 98L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 99L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 102L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 103L);

            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 104L);
        }
    }
}
