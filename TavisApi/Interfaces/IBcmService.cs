using Tavis.Models;

namespace TavisApi.Services;

public interface IBcmService
{
  List<Player> GetPlayers();
}