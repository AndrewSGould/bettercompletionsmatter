namespace TavisApi.Services;

//https://www.trueachievements.com/gamecollection?executeformfunction&function=AjaxList&params=oGameCollection%7CoGameCollection_TimeZone=Eastern%20Standard%20Time%26txtGamerID%3D104571%26ddlSortBy%3DTitlename%26ddlDLCInclusionSetting%3DAllDLC%26ddlCompletionStatus%3DAll%26ddlTitleType%3DGame%26ddlContestStatus%3DAll%26asdGamePropertyID%3D-1%26oGameCollection_Order%3DDatecompleted%26oGameCollection_Page%3D1%26oGameCollection_ItemsPerPage%3D10000%26oGameCollection_ShowAll%3DFalse%26txtGameRegionID%3D2%26GameView%3DoptListView%26chkColTitlename%3DTrue%26chkColCompletionestincDLC%3DTrue%26chkColUnobtainables%3DTrue%26chkColSiteratio%3DTrue%26chkColPlatform%3DTrue%26chkColServerclosure%3DTrue%26chkColNotNotForContests%3DTrue%26chkColSitescore%3DTrue%26chkColOfficialScore%3DTrue%26chkColItems%3DTrue%26chkColDatestarted%3DTrue%26chkColDatecompleted%3DTrue%26chkColLastunlock%3DTrue%26chkColOwnershipstatus%3DTrue%26chkColPublisher%3DTrue%26chkColDeveloper%3DTrue%26chkColReleasedate%3DTrue%26chkColGamerswithgame%3DTrue%26chkColGamerscompleted%3DTrue%26chkColGamerscompletedperentage%3DTrue%26chkColCompletionestimate%3DTrue%26chkColSiterating%3DTrue%26chkColNotforcontests%3DTrue%26chkColInstallsize%3DTrue

public class TA_GameCollection : ITA_GameCollection {

  public string ParseManager(int playerTrueAchId, int page) {
    return ParseManager(playerTrueAchId, page, new SyncOptions());
  }

  public string ParseManager(int playerTrueAchId, int page, SyncOptions gameCollectionOptions) {
    BuildDefaultOptions(gameCollectionOptions);
    return DynamicParse(playerTrueAchId, page, gameCollectionOptions);
  }

  private SyncOptions BuildDefaultOptions(SyncOptions gcOptions) {
    if (gcOptions.CompletionStatus == null)
      gcOptions.CompletionStatus = SyncOption_CompletionStatus.All;

    if (gcOptions.ContestStatus == null)
      gcOptions.ContestStatus = SyncOption_ContestStatus.All;

    if (gcOptions.TimeZone == null)
      gcOptions.TimeZone = SyncOption_Timezone.EST;

    return gcOptions;
  }

  private string DynamicParse(int playerTrueAchId, int page, SyncOptions gameCollectionOptions) {
    return "https://www.trueachievements.com/gamecollection?executeformfunction&function=AjaxList&params=oGameCollection%7Co" +
      $"GameCollection_TimeZone={gameCollectionOptions.TimeZone.Value}" +
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

  public class SyncOptions {
    public SyncOption_CompletionStatus? CompletionStatus {get; set;}
    public SyncOption_ContestStatus? ContestStatus {get; set;}
    public DateTime? DateCutoff {get;set;}
    public DateTime? LastUnlockCutoff {get; set;}
    public SyncOption_Timezone? TimeZone {get;set;}
  }

  public class SyncOption_CompletionStatus {
    private SyncOption_CompletionStatus(string value) { Value = value; }

    public string Value {get; private set;}

    public static SyncOption_CompletionStatus All { get { return new SyncOption_CompletionStatus("All"); }}
    public static SyncOption_CompletionStatus Complete { get { return new SyncOption_CompletionStatus("Complete%20only"); }}
    public static SyncOption_CompletionStatus Incomplete { get { return new SyncOption_CompletionStatus("Incomplete%20only"); }}
  }

  public class SyncOption_Timezone {
    private SyncOption_Timezone(string value) { Value = value; }
    
    public string Value {get; private set;}

    public static SyncOption_Timezone EST {get { return new SyncOption_Timezone("Eastern%20Standard%20Time"); }}
  }

  public class SyncOption_ContestStatus {
    private SyncOption_ContestStatus(string value) { Value = value; }

    public string Value {get; private set;}

    public static SyncOption_ContestStatus All { get { return new SyncOption_ContestStatus("All"); }}
    public static SyncOption_ContestStatus ForContests { get { return new SyncOption_ContestStatus("For%20contests"); }}
    public static SyncOption_ContestStatus NotForContests { get { return new SyncOption_ContestStatus("Not%20for%20contests"); }}
  }
}
