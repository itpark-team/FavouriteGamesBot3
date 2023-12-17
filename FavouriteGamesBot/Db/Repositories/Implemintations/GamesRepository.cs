using FavouriteGamesBot.Db.DbConnector;
using FavouriteGamesBot.Db.Models;
using FavouriteGamesBot.Db.Repositories.Interfaces;

namespace FavouriteGamesBot.Db.Repositories.Implemintations;

public class GamesRepository : IGamesRepository
{
    private FgbDbContext _dbContext;

    public GamesRepository(FgbDbContext db)
    {
        _dbContext = db;
    }

    public List<Game> GetGamesByGamesListId(int gamesListId)
    {
        GamesList gamesList = _dbContext.GamesLists.Where(x => x.Id == gamesListId).FirstOrDefault();
        return gamesList.Games as List<Game>;
    }

    public void AddGame(int listId, string title, int price, int rating, string comment)
    {
        GamesList gamesList = _dbContext.GamesLists.Where(x => x.Id == listId).FirstOrDefault();

        gamesList.Games.Add(new Game()
        {
            Title = title,
            Price = price,
            Rating = rating,
            Comment = comment
        });

        _dbContext.GamesLists.Update(gamesList);
        _dbContext.SaveChanges();
    }

    public void UpdateGame(int gameId, string title, int price, int rating, string comment)
    {
        Game game = _dbContext.Games.Where(x => x.Id == gameId).FirstOrDefault();

        if (title != "")
            game.Title = title;
        if (price != 0)
            game.Price = price;
        if (rating != 0)
            game.Rating = rating;
        if (comment != "")
            game.Comment = comment;

        _dbContext.Games.Update(game);
        _dbContext.SaveChanges();
    }

    public void DeleteGame(int gameId, int gamesListId)
    {
        GamesList gamesList = _dbContext.GamesLists.Where(x => x.Id == gamesListId).FirstOrDefault();

        foreach (var game in gamesList.Games)
        {
            if (game.Id == gameId)
            {
                gamesList.Games.Remove(game);
                _dbContext.Games.Remove(game);
            }
        }

        _dbContext.GamesLists.Update(gamesList);
        _dbContext.SaveChanges();
    }
}