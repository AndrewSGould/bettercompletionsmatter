using Tavis.Models;
using static WebApi.Controllers.RaidBossController;

namespace TavisApi.Services;

public interface IRaidBossService
{
  List<Player> GetPlayers();
}