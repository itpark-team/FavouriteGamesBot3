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
        ListMenuService listMenuService = new ListMenuService(gamesRepository, gamesListsRepository);
        GameMenuService gameMenuService = new GameMenuService(gamesRepository, gamesListsRepository);

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
        _methods[States.ListMenu.ListInputDeletingConfirmation] = listMenuService.ProcessClickOnDeleteListButton;
        #endregion

        #region GameMenu  
        _methods[States.GameMenu.InputTitle] = gameMenuService.ProcessInputTitle;
        _methods[States.GameMenu.InputPrice] = gameMenuService.ProcessInputPrice;
        _methods[States.GameMenu.InputRating] = gameMenuService.ProcessInputRating;
        _methods[States.GameMenu.InputComment] = gameMenuService.ProcessInputComment;
        _methods[States.GameMenu.InputCreatingConfirmation] = gameMenuService.ProcessInputCreatingConfirmation;
        
        _methods[States.GameMenu.ClickOnInlineButtonListGames] = gameMenuService.ProcessClickOnInlineButtonListGames;
        _methods[States.GameMenu.ClickInlineButtonInActionWithGameMenu] = gameMenuService.ProcessClickInlineButtonInActionWithGameMenu;
        _methods[States.GameMenu.InputDeletingConfirmation] = gameMenuService.ProcessInputDeletingConfirmation;
        _methods[States.GameMenu.ChooseEditParameter] = gameMenuService.ProcessChooseEditParameter;
        _methods[States.GameMenu.EditingInputTitle] = gameMenuService.ProcessEditingInputTitle;
        _methods[States.GameMenu.EditingInputPrice] = gameMenuService.ProcessEditingInputPrice;
        _methods[States.GameMenu.EditingInputRating] = gameMenuService.ProcessEditingInputRating;
        _methods[States.GameMenu.EditingInputComment] = gameMenuService.ProcessEditingInputComment;
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