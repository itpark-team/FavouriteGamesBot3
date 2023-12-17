using System.Collections.Generic;
using System.Linq;
using FavouriteGamesBot.Db.DbConnector;
using FavouriteGamesBot.Db.Models;
using FavouriteGamesBot.Db.Repositories.Interfaces;

namespace FavouriteGamesBot.Db.Repositories.Implemintations;

public class GamesListsRepository : IGamesListsRepository
{
    private FgbDbContext _dbContext;

    public GamesListsRepository(FgbDbContext db)
    {
        _dbContext = db;
    }

    public List<GamesList> GetGamesListsByChatId(long chatId)
    {
        return _dbContext.GamesLists.Where(x => x.ChatId == chatId).ToList();
    }

    public GamesList GetGamesListById(int id)
    {
        return _dbContext.GamesLists.Where(x => x.Id == id).FirstOrDefault();
    }

    public void AddGamesList(long chatId, string title)
    {
        _dbContext.GamesLists.Add(new GamesList()
        {
            ChatId = chatId,
            Title = title,
            IsPrivate = true
        });
        _dbContext.SaveChanges();
    }

    public void UpdateGamesListTitle(int gamesListId, string title)
    {
        GamesList gamesList = _dbContext.GamesLists.Where(x => x.Id == gamesListId).FirstOrDefault();
        gamesList.Title = title;
        _dbContext.GamesLists.Update(gamesList);
        _dbContext.SaveChanges();
    }

    public void UpdateGamesListPrivacy(int gamesListId, bool isPrivate)
    {
        GamesList gamesList = _dbContext.GamesLists.Where(x => x.Id == gamesListId).FirstOrDefault();
        gamesList.IsPrivate = isPrivate;
        _dbContext.GamesLists.Update(gamesList);
        _dbContext.SaveChanges();
    }

    public void DeleteGamesList(int gamesListId)
    {
        GamesList gamesList = _dbContext.GamesLists.Where(x => x.Id == gamesListId).FirstOrDefault();

        foreach (var game in gamesList.Games)
        {
            gamesList.Games.Remove(game);
            _dbContext.Games.Remove(game);
        }
        
        _dbContext.GamesLists.Remove(gamesList);
        _dbContext.SaveChanges();
    }
}