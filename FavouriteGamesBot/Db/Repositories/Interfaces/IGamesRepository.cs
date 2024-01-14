using FavouriteGamesBot.Db.Models;

namespace FavouriteGamesBot.Db.Repositories.Interfaces;

public interface IGamesRepository
{
    List<Game> GetGamesByGamesListId(int gamesListId);
    Game GetGameByTitle(GamesList gamesList, string title);
    void AddGame(int listId, string title, int price, int rating, string comment);
    void UpdateGame(Game game);
    void DeleteGame(int gameId);
    Game GetGameById(int id);
}