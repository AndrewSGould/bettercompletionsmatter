using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tavis.Models;
using TavisApi.Models;

namespace TavisApi.Context;

public class YearlyChallengeConfiguration : IEntityTypeConfiguration<YearlyChallenge>
{
  public void Configure(EntityTypeBuilder<YearlyChallenge> builder)
  {
    builder
      .HasKey(c => c.Id);

    builder
      .HasData(
        new YearlyChallenge
        {
          Id = 1,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "A Wise Wookie Once Said",
          Description = "Complete any game with an audible fictional language",
        },
        new YearlyChallenge
        {
          Id = 2,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Captain Kirkland",
          Description = "Complete any game in which you can pilot a spaceship",
        },
        new YearlyChallenge
        {
          Id = 3,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Dave's Buxom",
          Description = "Complete any game featuring scantily clad women (or shirtless men!)",
        },
        new YearlyChallenge
        {
          Id = 4,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Ex-lax Gesture",
          Description = "Complete any game where a character is able to use, or is portrayed as having used, the toilet",
        },
        new YearlyChallenge
        {
          Id = 5,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "He was Going to Design a Challenge, but Then He Got High",
          Description = "Complete any game in which your character can consume the drugs",
        },
        new YearlyChallenge
        {
          Id = 6,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "I Don't Even Know You And I Hate You",
          Description = "Complete any game at or below a 2.5 star rating on TA, suckers",
        },
        new YearlyChallenge
        {
          Id = 7,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "In the Vein of Nerdy Neo",
          Description = "Complete any game tagged with the \"Metroidvania\" genre on TrueAchievements",
        },
        new YearlyChallenge
        {
          Id = 8,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "It's Garbage",
          Description = "Complete any game that isn't in the Yakuza series",
        },
        new YearlyChallenge
        {
          Id = 9,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "llams dna taerG",
          Description = "Complete a game where you directly control an animal character",
        },
        new YearlyChallenge
        {
          Id = 10,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Match It!",
          Description = "Complete any game that smrnov has completed throughout 2024",
        },
        new YearlyChallenge
        {
          Id = 11,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Min's Best Friend",
          Description = "Complete any game in which your character has a canine companion",
        },
        new YearlyChallenge
        {
          Id = 12,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Welldoneington Balbo",
          Description = "Complete any game with an achievement flagged \"Time Consuming\"",
        },
        new YearlyChallenge
        {
          Id = 13,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "My Mom Beat Zelda 2",
          Description = "Complete any game in which your character's mother is represented or mentioned in game",
        },
        new YearlyChallenge
        {
          Id = 14,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Pretty Fly for an Albino Kid",
          Description = "Complete any game that has a skin tone slider during character creation",
        },
        new YearlyChallenge
        {
          Id = 15,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Saben, of the Rothschilds",
          Description = "Complete any game that has an achievement for accumulating currency",
        },
        new YearlyChallenge
        {
          Id = 16,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Scorching Strands",
          Description = "Complete any game that has a character or creature with flames instead of hair",
        },
        new YearlyChallenge
        {
          Id = 17,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Supafly Streamin'",
          Description = "Complete any game that has been streamed on the official BCMX Twitch account",
        },
        new YearlyChallenge
        {
          Id = 18,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "The Last of the Monacans",
          Description = "Complete any game that has a title featuring the letter string \"DD\"",
        },
        new YearlyChallenge
        {
          Id = 19,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Ultimate Despair vs. Capcom 3",
          Description = "Complete any game tagged with the \"Fighting\" genre on TrueAchievements",
        },
        new YearlyChallenge
        {
          Id = 20,
          Category = Data.YearlyCategory.CommunityStar,
          Title = "Welcome to Scardew Valley",
          Description = "Complete any game that has a farming mechanic that is a major element in gameplay",
        },
        new YearlyChallenge
        {
          Id = 21,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Curator",
          Description = "Complete any game of the same genre as your most completed genre",
        },
        new YearlyChallenge
        {
          Id = 22,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Explorer",
          Description = "Complete any game in the bottom 50% of your most completed genres",
        },
        new YearlyChallenge
        {
          Id = 23,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Connoisseur",
          Description = "Complete any game with less than 5,000 tracked gamers and a rating over 4 on TrueAchievements",
        },
        new YearlyChallenge
        {
          Id = 24,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Hipster",
          Description = "Complete any game with less than 1,000 tracked gamers",
        },
        new YearlyChallenge
        {
          Id = 25,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Critic",
          Description = "Complete any game that has a Site Rating of 4.25 or higher",
        },
        new YearlyChallenge
        {
          Id = 26,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Socialite",
          Description = "Complete any game that was completed by 3 other BCM participants this year",
        },
        new YearlyChallenge
        {
          Id = 27,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Inclusionist",
          Description = "Complete any game with Cross-Play or the Xbox on Steam tag",
        },
        new YearlyChallenge
        {
          Id = 28,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Professional",
          Description = "Complete any game that ranks in the top 10% of your highest ratio completed games of all time",
        },
        new YearlyChallenge
        {
          Id = 29,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Contrarian",
          Description = "Complete any game where your personal rating is 1.5 stars above or below the Site Average. You must rate the game on TA for this to count",
        },
        new YearlyChallenge
        {
          Id = 30,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Pioneer",
          Description = "Complete any game with less than 100 completions",
        },
        new YearlyChallenge
        {
          Id = 31,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Conformist",
          Description = "Complete any game with more than 7,500 tracked gamers",
        },
        new YearlyChallenge
        {
          Id = 32,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Minimalist",
          Description = "Complete any game with an install size of under 200MB",
        },
        new YearlyChallenge
        {
          Id = 33,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Hedonist",
          Description = "Complete any game with an install size of over 30GB",
        },
        new YearlyChallenge
        {
          Id = 34,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Traditionalist",
          Description = "Complete any game that has exactly 50 achievements total",
        },
        new YearlyChallenge
        {
          Id = 35,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Arcade Traditionalist",
          Description = "Complete any game that has exactly 12 achievements total",
        },
        new YearlyChallenge
        {
          Id = 36,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Orthographer",
          Description = "Complete any game where the title contains 14 or more unique letters",
        },
        new YearlyChallenge
        {
          Id = 37,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Training Arc: The Calendar Project",
          Description = "Complete any game that was released the year you joined Xbox Live. Are you ready to traverse history?",
        },
        new YearlyChallenge
        {
          Id = 38,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Training Arc: The Genre Project",
          Description = "Complete any game with 4 or more genres. Are you ready to collect them all?",
        },
        new YearlyChallenge
        {
          Id = 39,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Training Arc: Head-to-Head",
          Description = "Complete any game that includes an achievement flagged \"versus\", or \"online skill\". Are you ready to throw down?",
        },
        new YearlyChallenge
        {
          Id = 40,
          Category = Data.YearlyCategory.TheTAVIS,
          Title = "The Training Arc: Raid Boss",
          Description = "Complete any Xbox 360 game over 100 hours. Are you ready for the next battle?",
        },
        new YearlyChallenge
        {
          Id = 41,
          Category = Data.YearlyCategory.RetirementParty,
          Title = "Game Passed",
          Description = "Complete any game after it's already left Game Pass",
        },
        new YearlyChallenge
        {
          Id = 42,
          Category = Data.YearlyCategory.RetirementParty,
          Title = "Hope There is a Guide",
          Description = "Complete any game with 10 or more achievements flagged missable on TrueAchievements",
        },
        new YearlyChallenge
        {
          Id = 43,
          Category = Data.YearlyCategory.RetirementParty,
          Title = "I Didn't Know that I Was Watching a Movie",
          Description = "Complete any game with 1 cutscene that is at least 10 minutes or longer",
        },
        new YearlyChallenge
        {
          Id = 44,
          Category = Data.YearlyCategory.RetirementParty,
          Title = "Leave Your Xbox On",
          Description = "Complete any game with an achievement for cumulative play time",
        },
        new YearlyChallenge
        {
          Id = 45,
          Category = Data.YearlyCategory.RetirementParty,
          Title = "Open Minded",
          Description = "Complete any game where your character has access to mind control abilities",
        },
        new YearlyChallenge
        {
          Id = 46,
          Category = Data.YearlyCategory.RetirementParty,
          Title = "Pet Rock",
          Description = "Complete any game where you can throw a rock to distract enemies (it must be a distraction mechanic, not an attack mechanic)",
        },
        new YearlyChallenge
        {
          Id = 47,
          Category = Data.YearlyCategory.RetirementParty,
          Title = "Prime Completions Matter",
          Description = "Complete any game published on Xbox by Prime Matter",
        },
        new YearlyChallenge
        {
          Id = 48,
          Category = Data.YearlyCategory.RetirementParty,
          Title = "Radiation Celebration",
          Description = "Complete any game where your character can wear a gasmask or hazmat suit",
        },
        new YearlyChallenge
        {
          Id = 49,
          Category = Data.YearlyCategory.RetirementParty,
          Title = "They Said The Thing",
          Description = "Complete any game where a character says the name of the game in the dialogue",
        },
        new YearlyChallenge
        {
          Id = 50,
          Category = Data.YearlyCategory.RetirementParty,
          Title = "What a Mouthful",
          Description = "Complete any game with six or more words in the title",
        }
      );
  }
}

