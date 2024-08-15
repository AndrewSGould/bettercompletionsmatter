using Moq;
using TavisApi.Models;
using TavisApi.TrueAchievements;
using TavisApi.TrueAchievements.Interfaces;
using TavisApi.TrueAchievements.Models;

namespace TavisApi.Tests;

public class SyncTests {
	private readonly DataSync _dataSync;
	private readonly Mock<TavisContext> _mockTavisContext;
	private readonly Mock<IParser> _mockParser;
	private readonly Mock<ITA_GameCollection> _mockTAGameCollection;

	public SyncTests()
	{
		_mockTavisContext = new Mock<TavisContext>();
		_mockParser = new Mock<IParser>();
		_mockTAGameCollection = new Mock<ITA_GameCollection>();

		_dataSync = new DataSync(_mockTavisContext.Object, _mockParser.Object, _mockTAGameCollection.Object);
	}

	private static List<Game> testGames = new List<Game>();
	private static List<TA_CollectionEntry> testEntries = new List<TA_CollectionEntry>();

	static SyncTests()
	{
		// 'Remains' use case
		testGames.Add(new Game { Id = 1, FullCompletionEstimate = 1000 });
		testEntries.Add(new TA_CollectionEntry { Id = 1, FullCompletionEstimate = null, TotalGamerscore = 1000, BaseCompletionEstimate = 0.5 });

		// 'Gem Wizard Tactics' use case
		testGames.Add(new Game { Id = 2, FullCompletionEstimate = null });
		testEntries.Add(new TA_CollectionEntry { Id = 2, FullCompletionEstimate = null, TotalGamerscore = 1000, BaseCompletionEstimate = -1 });

		// 'Gem Wizard Tactics' manually scored use case
		testGames.Add(new Game { Id = 3, FullCompletionEstimate = 5 });
		testEntries.Add(new TA_CollectionEntry { Id = 3, FullCompletionEstimate = null, TotalGamerscore = 1000, BaseCompletionEstimate = -1 });

		// 'Elden Ring' use case
		testGames.Add(new Game { Id = 4, FullCompletionEstimate = null });
		testEntries.Add(new TA_CollectionEntry { Id = 4, FullCompletionEstimate = null, TotalGamerscore = 1000, BaseCompletionEstimate = 100 });

		// 'Elden Ring' manually scored use case
		testGames.Add(new Game { Id = 5, FullCompletionEstimate = 100 });
		testEntries.Add(new TA_CollectionEntry { Id = 5, FullCompletionEstimate = null, TotalGamerscore = 1000, BaseCompletionEstimate = 100 });

		// 'FF8'/normal score use case
		testGames.Add(new Game { Id = 6, FullCompletionEstimate = 25 });
		testEntries.Add(new TA_CollectionEntry { Id = 6, FullCompletionEstimate = 25, TotalGamerscore = 1000, BaseCompletionEstimate = 25 });

		// 1k w/ base score use case - Elden Ring
		testGames.Add(new Game { Id = 7, FullCompletionEstimate = null }); ;
		testEntries.Add(new TA_CollectionEntry { Id = 7, FullCompletionEstimate = null, TotalGamerscore = 1000, BaseCompletionEstimate = 100 });

		// 1k w/o base score use case
		testGames.Add(new Game { Id = 8, FullCompletionEstimate = null });
		testEntries.Add(new TA_CollectionEntry { Id = 8, FullCompletionEstimate = null, TotalGamerscore = 1000, BaseCompletionEstimate = -1 });

		// 1k w/o base score manually scored use case
		testGames.Add(new Game { Id = 9, FullCompletionEstimate = 50 });
		testEntries.Add(new TA_CollectionEntry { Id = 9, FullCompletionEstimate = null, TotalGamerscore = 1000, BaseCompletionEstimate = -1 });

		// 1k w/o base score manually scored use case
		testGames.Add(new Game { Id = 10, FullCompletionEstimate = 200 });
		testEntries.Add(new TA_CollectionEntry { Id = 10, FullCompletionEstimate = 100, TotalGamerscore = 1000, BaseCompletionEstimate = 80 });
	}

	[Theory]
	[InlineData(0, 0, 0.5)]
	[InlineData(1, 1, null)]
	[InlineData(2, 2, 5.0)]
	[InlineData(3, 3, 100.0)]
	[InlineData(4, 4, 100.0)]
	[InlineData(5, 5, 25.0)]
	[InlineData(6, 6, 100.0)]
	[InlineData(7, 7, null)]
	[InlineData(8, 8, 50.0)]
	[InlineData(9, 9, 100.0)]
	public void Properly_Estimates_Games(int tavisGameIndex, int scannedGameIndex, double? expectedEstimate)
	{
		var tavisGame = testGames[tavisGameIndex];
		var scannedGame = testEntries[scannedGameIndex];

		double? estimateResult = _dataSync.DetermineCompletionEstimate(tavisGame, scannedGame);
		Assert.Equal(expectedEstimate, estimateResult);
	}
}