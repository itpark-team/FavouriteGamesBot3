using FavouriteGamesBot.Db.DbConnector;
using FavouriteGamesBot.Db.Models;
using FavouriteGamesBot.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        GamesList gamesList = _dbContext.GamesLists.Where(x => x.Id == gamesListId).Include(x => x.Games).FirstOrDefault();
        return gamesList.Games as List<Game>;
    }
    public Game GetGameById(int id)
    {
        return _dbContext.Games.Where(x=>x.Id == id).FirstOrDefault();
    }

    public Game GetGameByTitle(GamesList gamesList, string title)
    {
        foreach (Game game in gamesList.Games)
        {
            if (game.Title == title)
            {
                return game;
            }
        }

        return null;
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

    public void DeleteGame(int gameId)
    {
        Game game = _dbContext.Games.Where(x => x.Id == gameId).FirstOrDefault();
        _dbContext.Games.Remove(game);
        _dbContext.SaveChanges();
    }
}