﻿using System.Collections.Generic;
using FavouriteGamesBot.Db.Models;

namespace FavouriteGamesBot.Db.Repositories.Interfaces;

public interface IGamesListsRepository
{
    List<GamesList> GetGamesListsByChatId(long chatId);
    GamesList GetGamesListByTitle(string title, long chatId);
    GamesList GetGamesListById(int id);
    void AddGameInGamesList(GamesList gamesList, Game game);
    void AddGamesList(long chatId, string title);
    void UpdateGamesListTitle(int gamesListId, string title);
    void UpdateGamesListPrivacy(int gamesListId, bool isPrivate);
    void DeleteGamesList(int gamesListId);
}