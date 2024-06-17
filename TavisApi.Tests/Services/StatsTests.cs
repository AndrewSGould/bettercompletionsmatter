using Tavis.Models;
using TavisApi.Services;

namespace TavisApi.Tests.Services;

public class StatsTests {
	StatsService _statsService;

	public StatsTests()
	{
		_statsService = new StatsService(null, null);
	}

	private static List<PlayerTopGenre> eohGenres = new List<PlayerTopGenre>();
	private static List<BcmPlayerGame> eohGames = new List<BcmPlayerGame>();

	private static List<PlayerTopGenre> scarGenres = new List<PlayerTopGenre>();
	private static List<BcmPlayerGame> scarGames = new List<BcmPlayerGame>();

	static StatsTests()
	{
		scarGenres.AddRange(new List<PlayerTopGenre> {
			new PlayerTopGenre {
				GenreId = GenreList.Puzzle,
				Rank = 1,
			},
			new PlayerTopGenre {
				GenreId = GenreList.Platformer,
				Rank = 2,
			},
			new PlayerTopGenre {
				GenreId = GenreList.Adventure,
				Rank = 3,
			},
			new PlayerTopGenre {
				GenreId = GenreList.RP,
				Rank = 4,
			},
			new PlayerTopGenre {
				GenreId = GenreList.Simulation,
				Rank = 5,
			},
		});

		scarGames = new List<BcmPlayerGame> {
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 4),
				Game = new Game {
					Title = "Monster Harvest",
					FullCompletionEstimate = 8,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.TurnBased },
						new GameGenre { GenreId = GenreList.RP },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 1),
				Game = new Game {
					Title = "Planet Alpha",
					FullCompletionEstimate = 8,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Platformer },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 1),
				Game = new Game {
					Title = "No Place Like Home",
					FullCompletionEstimate = 20,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Simulation },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 2),
				Game = new Game {
					Title = "The Walking Dead Season Two (Xbox 360)",
					FullCompletionEstimate = 10,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Adventure },
						new GameGenre { GenreId = GenreList.PointAndClick },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 4),
				Game = new Game {
					Title = "Megaquarium",
					FullCompletionEstimate = 25,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Simulation },
						new GameGenre { GenreId = GenreList.Management },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 5),
				Game = new Game {
					Title = "Afterparty",
					FullCompletionEstimate = 12,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Adventure },
						new GameGenre { GenreId = GenreList.PointAndClick },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 5),
				Game = new Game {
					Title = "Lightyear Frontier",
					FullCompletionEstimate = 12,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Simulation },
						new GameGenre { GenreId = GenreList.OpenWorld },
						new GameGenre { GenreId = GenreList.Management },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 7),
				Game = new Game {
					Title = "Fez",
					FullCompletionEstimate = 15,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Puzzle },
						new GameGenre { GenreId = GenreList.Platformer },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 8),
				Game = new Game {
					Title = "Bug Fables: The Everlasting Sapling",
					FullCompletionEstimate = 50,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.RP },
						new GameGenre { GenreId = GenreList.TurnBased },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 10),
				Game = new Game {
					Title = "Powerwash Simulator",
					FullCompletionEstimate = 65,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Simulation },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 12),
				Game = new Game {
					Title = "Supraland: Six Inches Under",
					FullCompletionEstimate = 20,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.FPS },
						new GameGenre { GenreId = GenreList.Puzzle },
						new GameGenre { GenreId = GenreList.Platformer },
						new GameGenre { GenreId = GenreList.Metroidvania },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 19),
				Game = new Game {
					Title = "Chicory: A Colorful Tale",
					FullCompletionEstimate = 20,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Adventure },
						new GameGenre { GenreId = GenreList.Puzzle },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 22),
				Game = new Game {
					Title = "Minecraft Dungeons",
					FullCompletionEstimate = 104,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.ARPG },
						new GameGenre { GenreId = GenreList.RP },
						new GameGenre { GenreId = GenreList.HackAndSlash },
						new GameGenre { GenreId = GenreList.DungeonCrawler },
					},
				}
			},
		};

		eohGenres.AddRange(new List<PlayerTopGenre> {
			new PlayerTopGenre {
				GenreId = GenreList.Strategy,
				Rank = 1,
			},
			new PlayerTopGenre {
				GenreId = GenreList.TurnBased,
				Rank = 2,
			},
			new PlayerTopGenre {
				GenreId = GenreList.RP,
				Rank = 3,
			},
			new PlayerTopGenre {
				GenreId = GenreList.Adventure,
				Rank = 4,
			},
			new PlayerTopGenre {
				GenreId = GenreList.Platformer,
				Rank = 5,
			},
		});

		eohGames = new List<BcmPlayerGame> {
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 13),
				Game = new Game {
					Title = "Immortal Planet",
					FullCompletionEstimate = 20,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.ARPG },
						new GameGenre { GenreId = GenreList.RP },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 28),
				Game = new Game {
					Title = "Hollow Knight: Voidheart Edition",
					FullCompletionEstimate = 80,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Platformer },
						new GameGenre { GenreId = GenreList.Metroidvania },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 31),
				Game = new Game {
					Title = "One Step From Eden",
					FullCompletionEstimate = 50,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Roguelite },
						new GameGenre { GenreId = GenreList.Strategy },
						new GameGenre { GenreId = GenreList.RealTime },
					},
				}
			},
			new BcmPlayerGame {
				CompletionDate = new DateTime(2024, 5, 31),
				Game = new Game {
					Title = "Human Fall Flat",
					FullCompletionEstimate = 27,
					GameGenres = new List<GameGenre> {
						new GameGenre { GenreId = GenreList.Puzzle },
						new GameGenre { GenreId = GenreList.Platformer },
					},
				}
			},
		};
	}

	[Fact]
	public void TestTowerThings()
	{
		var result = _statsService.DoTheThing(eohGames, eohGenres);
		Assert.Equal(4, result.FloorCount);
	}

	[Fact]
	public void TestTowerThings2()
	{
		var result = _statsService.DoTheThing(scarGames, scarGenres);
		Assert.Equal(13, result.FloorCount);
	}
}
