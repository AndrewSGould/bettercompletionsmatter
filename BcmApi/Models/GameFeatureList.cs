namespace Bcm.Models;
public class FeatureList {
  public int Id {get;set;}
  public bool OneXEnhanced {get;set;}
  public bool BackwardsCompat {get;set;}
  public bool NotBackwardsCompat {get;set;}
  public bool PlayAnywhere {get;set;}
  public bool SmartDelivery {get;set;}
  public bool OptimizedForSeries {get;set;}
  public bool Crossplay {get;set;}
  public bool Hdr {get;set;}
  public bool xCloudTouch {get;set;}
  public bool GamePass {get;set;}
  public bool CloudGaming {get;set;}
  public bool PcGamePass {get;set;}
  public bool EaPlay {get;set;}
  public bool GamePreview {get;set;}
  public bool IdAtXbox {get;set;}
  public bool OnSteam {get;set;}
  public bool GamesWithGold {get;set;}
  public bool TransferableProgress {get;set;}


  public int FeatureListOfGameId { get; set; }
  public Game Game { get; set; }
}
