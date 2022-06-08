using System.ComponentModel.DataAnnotations.Schema;

namespace BcmApi.Models {
  public class PlayerCompletedGame {
    [Column("Player_Id")]
    public int PlayerId {get; set;}
    [Column("Game_Id")]
    public int GameId {get; set;}
  }
}