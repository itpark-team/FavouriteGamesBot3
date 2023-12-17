using System;
using System.Collections.Generic;
using FavouriteGamesBot.Bot;
using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Db.DbConnector;
using FavouriteGamesBot.Db.Repositories.Implemintations;
using FavouriteGamesBot.Db.Repositories.Interfaces;
using FavouriteGamesBot.Util.Button;
using FavouriteGamesBot.Util.String;

namespace FavouriteGamesBot.Service;

public class ServiceManager
{
    private Dictionary<string, Func<string, TransmittedData, BotMessage>>
        _methods;

    public ServiceManager()
    {
        FgbDbContext db = new FgbDbContext();

        IGamesListsRepository gamesListsRepository = new GamesListsRepository(db);
        IGamesRepository gamesRepository = new GamesRepository(db);
        
        StartMenuService startMenuService = new StartMenuService();
        MainMenuService mainMenuService = new MainMenuService(gamesListsRepository);
        ListMenuService listMenuService = new ListMenuService(gamesListsRepository);
        
        _methods =
            new Dictionary<string, Func<string, TransmittedData, BotMessage>>();

        #region StartMenu
        _methods[States.StartMenu.CommandStart] = startMenuService.ProcessCommandStart;
        #endregion

        #region MainMenu
        _methods[States.MainMenu.ClickOnInlineButton] = mainMenuService.ProcessClickOnInlineButton;
        #endregion

        #region ListMenu
        _methods[States.ListMenu.InputListName] = listMenuService.ProcessInputListName;
        _methods[States.ListMenu.ClickOnInlineButtonUserLists] = listMenuService.ProcessClickOnInlineButtonUserLists;
        _methods[States.ListMenu.ClickActionButtonWithList] = listMenuService.ProcessClickActionButtonWithList;
        _methods[States.ListMenu.NewListName] = listMenuService.ProcessNewListName;
        _methods[States.ListMenu.ClickButtonChangePrivacy] = listMenuService.ProcessClickButtonChangePrivacy;
        #endregion
    }

    public BotMessage ProcessBotUpdate(string textData, TransmittedData transmittedData)
    {
        if (textData == SystemStringsStorage.CommandReset)
        {
            transmittedData.State = States.MainMenu.ClickOnInlineButton;
            transmittedData.DataStorage.Clear();
            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
        }

        Func<string, TransmittedData, BotMessage> serviceMethod = _methods[transmittedData.State];
        return serviceMethod.Invoke(textData, transmittedData);
    }
}