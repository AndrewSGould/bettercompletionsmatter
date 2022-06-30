using static TavisApi.Services.TA_GameCollection;

namespace TavisApi.Services;

//https://www.trueachievements.com/gamecollection?executeformfunction&function=AjaxList&params=oGameCollection%7CoGameCollection_TimeZone=Eastern%20Standard%20Time%26txtGamerID%3D104571%26ddlSortBy%3DTitlename%26ddlDLCInclusionSetting%3DAllDLC%26ddlCompletionStatus%3DAll%26ddlTitleType%3DGame%26ddlContestStatus%3DAll%26asdGamePropertyID%3D-1%26oGameCollection_Order%3DDatecompleted%26oGameCollection_Page%3D1%26oGameCollection_ItemsPerPage%3D10000%26oGameCollection_ShowAll%3DFalse%26txtGameRegionID%3D2%26GameView%3DoptListView%26chkColTitlename%3DTrue%26chkColCompletionestincDLC%3DTrue%26chkColUnobtainables%3DTrue%26chkColSiteratio%3DTrue%26chkColPlatform%3DTrue%26chkColServerclosure%3DTrue%26chkColNotNotForContests%3DTrue%26chkColSitescore%3DTrue%26chkColOfficialScore%3DTrue%26chkColItems%3DTrue%26chkColDatestarted%3DTrue%26chkColDatecompleted%3DTrue%26chkColLastunlock%3DTrue%26chkColOwnershipstatus%3DTrue%26chkColPublisher%3DTrue%26chkColDeveloper%3DTrue%26chkColReleasedate%3DTrue%26chkColGamerswithgame%3DTrue%26chkColGamerscompleted%3DTrue%26chkColGamerscompletedperentage%3DTrue%26chkColCompletionestimate%3DTrue%26chkColSiterating%3DTrue%26chkColNotforcontests%3DTrue%26chkColInstallsize%3DTrue

public interface ITA_GameCollection {
  string ParseManager(int playerTrueAchId, int page);
  string ParseManager(int playerTrueAchId, int page, TA_GC_Options gameCollectionOptions);
}

public class TA_GameCollection : ITA_GameCollection {
  public string ParseManager(int playerTrueAchId, int page) {
    return ParseManager(playerTrueAchId, page, new TA_GC_Options());
  }

  public string ParseManager(int playerTrueAchId, int page, TA_GC_Options gameCollectionOptions) {
    BuildDefaultOptions(gameCollectionOptions);
    return DynamicParse(playerTrueAchId, page, gameCollectionOptions);
  }

  private TA_GC_Options BuildDefaultOptions(TA_GC_Options gcOptions) {
    if (gcOptions.CompletionStatus == null)
      gcOptions.CompletionStatus = TAGC_CompletionStatus.All;

    if (gcOptions.ContestStatus == null)
      gcOptions.ContestStatus = TAGC_ContestStatus.All;

    return gcOptions;
  }

  private string DynamicParse(int playerTrueAchId, int page, TA_GC_Options gameCollectionOptions) {
    return "https://www.trueachievements.com/gamecollection?executeformfunction&function=AjaxList&params=oGameCollection%7Co" +
      "GameCollection_TimeZone=Eastern%20Standard%20Time" +
      $"%26txtGamerID%3D{playerTrueAchId}" +
      "%26ddlSortBy%3DTitlename" +
      "%26ddlDLCInclusionSetting%3DAllDLC" +
      $"%26ddlCompletionStatus%3D{gameCollectionOptions.CompletionStatus?.Value}" +
      "%26ddlTitleType%3DGame" +
      $"%26ddlContestStatus%3D{gameCollectionOptions.ContestStatus?.Value}" +
      "%26asdGamePropertyID%3D-1" +
      "%26oGameCollection_Order%3DDatecompleted" +
      $"%26oGameCollection_Page%3D{page}" +
      "%26oGameCollection_ItemsPerPage%3D10000" +
      "%26oGameCollection_ShowAll%3DFalse" +
      "%26txtGameRegionID%3D2" +
      "%26GameView%3DoptListView" +
      "%26chkColTitlename%3DTrue" +
      "%26chkColCompletionestincDLC%3DTrue" +
      "%26chkColUnobtainables%3DTrue" +
      "%26chkColSiteratio%3DTrue" +
      "%26chkColPlatform%3DTrue" +
      "%26chkColServerclosure%3DTrue" +
      "%26chkColNotNotForContests%3DTrue" +
      "%26chkColSitescore%3DTrue" +
      "%26chkColOfficialScore%3DTrue" +
      "%26chkColItems%3DTrue" +
      "%26chkColDatestarted%3DTrue" +
      "%26chkColDatecompleted%3DTrue" +
      "%26chkColLastunlock%3DTrue" +
      "%26chkColOwnershipstatus%3DTrue" +
      "%26chkColPublisher%3DTrue" +
      "%26chkColDeveloper%3DTrue" +
      "%26chkColReleasedate%3DTrue" +
      "%26chkColGamerswithgame%3DTrue" +
      "%26chkColGamerscompleted%3DTrue" +
      "%26chkColGamerscompletedperentage%3DTrue" +
      "%26chkColCompletionestimate%3DTrue" +
      "%26chkColSiterating%3DTrue" +
      "%26chkColNotforcontests%3DTrue" +
      "%26chkColInstallsize%3DTrue";
  }

  public class TA_GC_Options {
    public TAGC_CompletionStatus? CompletionStatus {get; set;}
    public TAGC_ContestStatus? ContestStatus {get; set;}
    public DateTime? DateCutoff {get;set;}
  }

  public class TAGC_CompletionStatus {
    private TAGC_CompletionStatus(string value) { Value = value; }

    public string Value {get; private set;}

    public static TAGC_CompletionStatus All { get { return new TAGC_CompletionStatus("All"); }}
    public static TAGC_CompletionStatus Complete { get { return new TAGC_CompletionStatus("Complete%20only"); }}
    public static TAGC_CompletionStatus Incomplete { get { return new TAGC_CompletionStatus("Incomplete%20only"); }}
  }

  public class TAGC_ContestStatus {
    private TAGC_ContestStatus(string value) { Value = value; }

    public string Value {get; private set;}

    public static TAGC_ContestStatus All { get { return new TAGC_ContestStatus("All"); }}
    public static TAGC_ContestStatus ForContests { get { return new TAGC_ContestStatus("For%20contests"); }}
    public static TAGC_ContestStatus NotForContests { get { return new TAGC_ContestStatus("Not%20for%20contests"); }}
  }
}
