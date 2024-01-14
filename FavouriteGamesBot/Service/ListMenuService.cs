using FavouriteGamesBot.Bot;
using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Db.DbConnector;
using FavouriteGamesBot.Db.Models;
using FavouriteGamesBot.Db.Repositories.Implemintations;
using FavouriteGamesBot.Db.Repositories.Interfaces;
using FavouriteGamesBot.Util.Button;
using FavouriteGamesBot.Util.String;
using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteGamesBot.Service;

public class ListMenuService
{
    private IGamesRepository _gamesRepository;
    private IGamesListsRepository _gamesListsRepository;

    public ListMenuService(IGamesRepository gamesRepository, IGamesListsRepository gamesListsRepository)
    {
        _gamesRepository = gamesRepository;
        _gamesListsRepository = gamesListsRepository;
    }

    public BotMessage ProcessInputListName(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.ListNameMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.ListNameInputError, null);
        }

        transmittedData.State = States.MainMenu.ClickOnInlineButton;
        _gamesListsRepository.AddGamesList(transmittedData.ChatId, textData);

        return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
    }

    public BotMessage ProcessClickOnInlineButtonUserLists(string textData, TransmittedData transmittedData)
    {
        if (textData == ConstraintStringsStorage.Back)
        {
            transmittedData.State = States.MainMenu.ClickOnInlineButton;

            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose, true);
        }

        GamesList gamesList = _gamesListsRepository.GetGamesListByTitle(textData, transmittedData.ChatId);

        if (gamesList == null)
        {
            List<GamesList> gamesLists = _gamesListsRepository.GetGamesListsByChatId(transmittedData.ChatId);
            return new BotMessage(DialogsStringsStorage.PressButton, ReplyKeyboardMarkupStorage.CreateKeyboardGamesLists(gamesLists));
        }

        transmittedData.State = States.ListMenu.ClickActionButtonWithList;
        transmittedData.DataStorage.AddOrUpdate("listId", gamesList.Id);

        return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose, true);
    }

    public BotMessage ProcessClickButtonChangePrivacy(string textData, TransmittedData transmittedData)
    {
          GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

        if (textData == ConstraintStringsStorage.PublicList)
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;
            _gamesListsRepository.UpdateGamesListPrivacy(gamesList.Id, false);
            return new BotMessage(DialogsStringsStorage.ListPrivacySelected(gamesList) + DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
        }
        if (textData == ConstraintStringsStorage.PrivateList)
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;
            _gamesListsRepository.UpdateGamesListPrivacy(gamesList.Id, true);
            return new BotMessage(DialogsStringsStorage.ListPrivacySelected(gamesList) + DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
        }

        return new BotMessage(DialogsStringsStorage.ListPrivacyInfo(gamesList) + DialogsStringsStorage.ListPrivacy, InlineKeyboardMarkupStorage.ChooseListPrivacy);
    }

    public BotMessage ProcessNewListName(string textData, TransmittedData transmittedData)
    {
         if (textData.Length > ConstraintStringsStorage.ListNameMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.ListNameInputError, null);
        }

        GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

        transmittedData.State = States.ListMenu.ClickActionButtonWithList;

        _gamesListsRepository.UpdateGamesListTitle(gamesList.Id, textData);

        return new BotMessage(DialogsStringsStorage.ListNameChangeSuccess, InlineKeyboardMarkupStorage.ListMenuChoose);
    }

    public BotMessage ProcessClickOnDeleteListButton(string textData, TransmittedData transmittedData)
    {
        GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

        if (textData == BotButtonsStorage.GameMenu.Confirm.CallBackData)
        {
            _gamesListsRepository.DeleteGamesList((int)transmittedData.DataStorage.Get("listId"));

            transmittedData.State = States.MainMenu.ClickOnInlineButton;

            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
        }
        else if (textData == BotButtonsStorage.GameMenu.Cancel.CallBackData)
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;

            return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList),
                InlineKeyboardMarkupStorage.ListMenuChoose);
        }

        return new BotMessage(DialogsStringsStorage.ListDeletedConfirmation, null);
    }

    public BotMessage ProcessClickActionButtonWithList(string textData, TransmittedData transmittedData)
    {
       GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

        if (textData == BotButtonsStorage.ListMenu.AddGame.CallBackData)
        {
            transmittedData.State = States.GameMenu.InputTitle;
            return new BotMessage(DialogsStringsStorage.GameTitleInput, null);
        }
        else if (textData == BotButtonsStorage.ListMenu.CheckGames.CallBackData)
        {
            if (((List<Game>)gamesList.Games).Count == 0)
                return new BotMessage(DialogsStringsStorage.GamesAreNull + "\n\n" + DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);

            transmittedData.State = States.GameMenu.ClickOnInlineButtonListGames;
            return new BotMessage(DialogsStringsStorage.GamesInList, ReplyKeyboardMarkupStorage.CreateKeyboardGames((List<Game>)gamesList.Games));
        }
        else if (textData == BotButtonsStorage.ListMenu.RenameList.CallBackData)
        {
            transmittedData.State = States.ListMenu.NewListName;

            return new BotMessage(DialogsStringsStorage.NewListNameInput, null);
        }
        else if (textData == BotButtonsStorage.ListMenu.ChangeListPrivacy.CallBackData)
        {
            transmittedData.State = States.ListMenu.ClickButtonChangePrivacy;

            return new BotMessage(DialogsStringsStorage.ListPrivacyInfo(gamesList) + DialogsStringsStorage.ListPrivacy, InlineKeyboardMarkupStorage.ChooseListPrivacy);
        }
        else if (textData == BotButtonsStorage.ListMenu.DeleteList.CallBackData)
        {
            transmittedData.State = States.ListMenu.ListInputDeletingConfirmation;

            return new BotMessage(DialogsStringsStorage.ListDeletedConfirmation, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (textData == BotButtonsStorage.ListMenu.BackToLists.CallBackData)
        {
            transmittedData.State = States.ListMenu.ClickOnInlineButtonUserLists;
            transmittedData.DataStorage.Delete("listId");
            List<GamesList> gamesLists = _gamesListsRepository.GetGamesListsByChatId(transmittedData.ChatId);

            return new BotMessage(DialogsStringsStorage.MyLists, ReplyKeyboardMarkupStorage.CreateKeyboardGamesLists(gamesLists));
        }

        return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
     }
}