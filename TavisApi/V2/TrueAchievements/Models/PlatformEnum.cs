using Ardalis.SmartEnum;

namespace TavisApi.V2.TrueAchievements.Models;

public sealed class Platform : SmartEnum<Platform> {
	public static readonly Platform None = new Platform(0, "none");
	public static readonly Platform Xbox360 = new Platform(1, "xbox-360");
	public static readonly Platform XboxOne = new Platform(2, "xbox-one");
	public static readonly Platform XboxSeriesXS = new Platform(3, "xbox-series-x-s");
	public static readonly Platform Windows = new Platform(4, "windows");
	public static readonly Platform WP = new Platform(5, "windows-phone");
	public static readonly Platform GFWL = new Platform(6, "games-for-windows-live");
	public static readonly Platform Web = new Platform(7, "web");
	public static readonly Platform iOS = new Platform(8, "ios");
	public static readonly Platform Android = new Platform(9, "android");
	public static readonly Platform AppleTV = new Platform(10, "apple-tv");
	public static readonly Platform Switch = new Platform(11, "nintendo-switch");

	private Platform(int id, string name) : base(name, id)
	{
	}
}
