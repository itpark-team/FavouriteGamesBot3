using FavouriteGamesBot.Db.Models;

namespace FavouriteGamesBot.Db.Repositories.Interfaces;

public interface IGamesRepository
{
    List<Game> GetGamesByGamesListId(int gamesListId);
    void AddGame(int listId, string title, int price, int rating, string comment);
    void UpdateGame(int gameId, string title, int price, int rating, string comment);
    void DeleteGame(int gameId, int gamesListId);
}