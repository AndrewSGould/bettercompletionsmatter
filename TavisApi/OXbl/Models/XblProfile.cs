namespace TavisApi.OXbl.Models;

public class XblProfile {
	public string Id { get; set; }
	public string HostId { get; set; }
	public List<XblSettings> Settings { get; set; }
	public bool IsSponsoredUser { get; set; }
}

public class XblSettings {
	public string Id { get; set; }
	public string Value { get; set; }
}

public class XblProfiles {
	public List<XblProfile> ProfileUsers { get; set; }
}
