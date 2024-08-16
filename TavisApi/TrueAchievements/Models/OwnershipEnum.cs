using Ardalis.SmartEnum;

namespace TavisApi.TrueAchievements.Models;

public sealed class Ownership : SmartEnum<Ownership> {
	public static readonly Ownership None = new Ownership(0, "none");
	public static readonly Ownership Owned = new Ownership(1, "Owned");
	public static readonly Ownership Loaned = new Ownership(2, "Loaned out");
	public static readonly Ownership TradeSell = new Ownership(3, "Looking to trade/sell");
	public static readonly Ownership Borrowing = new Ownership(4, "Borrowing");
	public static readonly Ownership NoLongerHave = new Ownership(5, "No longer have");
	public static readonly Ownership Renting = new Ownership(6, "Renting");
	public static readonly Ownership GwG = new Ownership(7, "Games with Gold");
	public static readonly Ownership Ordered = new Ownership(8, "Ordered");
	public static readonly Ownership Trial = new Ownership(9, "Trial");
	public static readonly Ownership EAPlay = new Ownership(10, "EA Play");
	public static readonly Ownership UnableToPlay = new Ownership(11, "Unable to play");
	public static readonly Ownership GamePass = new Ownership(12, "Xbox Game Pass");
	public static readonly Ownership PcGamePass = new Ownership(13, "PC Game Pass");
	public static readonly Ownership UbisoftPlus = new Ownership(14, "Ubisoft+");

	private Ownership(int id, string name) : base(name, id)
	{
	}
}
