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
        if (textData.Length <= 20)
        {
            transmittedData.State = States.MainMenu.ClickOnInlineButton;

            _gamesListsRepository.AddGamesList(transmittedData.ChatId, textData);

            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
        }
        else
        {
            return new BotMessage(DialogsStringsStorage.ListNameInputError, null);
        }

        throw new Exception("Неизвестная ошибка в ProcessInputListName");
    }

    public BotMessage ProcessClickOnInlineButtonUserLists(string textData, TransmittedData transmittedData)
    {
        if (textData == "Назад")
        {
            transmittedData.State = States.MainMenu.ClickOnInlineButton;

            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose, true);
        }

        GamesList gamesList = _gamesListsRepository.GetGamesListByTitle(textData, transmittedData.ChatId);

        if (gamesList == null)
        {
            return new BotMessage(DialogsStringsStorage.PressButton, GetGamesListsAsKeyboard(transmittedData));
        }

        transmittedData.State = States.ListMenu.ClickActionButtonWithList;
        transmittedData.DataStorage.AddOrUpdate("listId", gamesList.Id);

        return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose,
            true);


        throw new Exception("Неизвестная ошибка в ProcessClickOnInlineButtonUserLists");
    }

    public BotMessage ProcessClickButtonChangePrivacy(string textData, TransmittedData transmittedData)
    {
        GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

        switch (textData)
        {
            case "PublicList":
                transmittedData.State = States.ListMenu.ClickActionButtonWithList;
                _gamesListsRepository.UpdateGamesListPrivacy(gamesList.Id, false);
                break;
            case "PrivateList":
                transmittedData.State = States.ListMenu.ClickActionButtonWithList;
                _gamesListsRepository.UpdateGamesListPrivacy(gamesList.Id, true);
                break;
            default:
                return new BotMessage(
                    DialogsStringsStorage.ListPrivacyInfo(gamesList) + DialogsStringsStorage.ListPrivacy,
                    InlineKeyboardMarkupStorage.ChooseListPrivacy);
                break;
        }

        return new BotMessage(
            DialogsStringsStorage.ListPrivacySelected(gamesList) + DialogsStringsStorage.ChoosedList(gamesList),
            InlineKeyboardMarkupStorage.ListMenuChoose);

        throw new Exception("Неизвестная ошибка в ProcessClickButtonChangePrivacy");
    }

    public BotMessage ProcessNewListName(string textData, TransmittedData transmittedData)
    {
        if (textData.Length <= 20)
        {
            GamesList gamesList =
                _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

            transmittedData.State = States.ListMenu.ClickActionButtonWithList;

            _gamesListsRepository.UpdateGamesListTitle(gamesList.Id, textData);

            return new BotMessage(DialogsStringsStorage.ListNameChangeSuccess,
                InlineKeyboardMarkupStorage.ListMenuChoose);
        }
        else
        {
            return new BotMessage(DialogsStringsStorage.ListNameInputError, null);
        }

        throw new Exception("Неизвестная ошибка в ProcessNewListName");
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
            return new BotMessage(DialogsStringsStorage.GameNameInput, null);
        }
        else if (textData == BotButtonsStorage.ListMenu.CheckGames.CallBackData)
        {
            if (((List<Game>)gamesList.Games).Count == 0)
                return new BotMessage(
                    DialogsStringsStorage.GamesAreNull + "\n\n" + DialogsStringsStorage.ChoosedList(gamesList),
                    InlineKeyboardMarkupStorage.ListMenuChoose);
            else
            {
                transmittedData.State = States.GameMenu.ClickOnInlineButtonListGames;
                return new BotMessage(DialogsStringsStorage.GamesInList,
                    ReplyKeyboardMarkupStorage.CreateKeyboardGames((List<Game>)gamesList.Games));
            }
        }
        else if (textData == BotButtonsStorage.ListMenu.RenameList.CallBackData)
        {
            transmittedData.State = States.ListMenu.NewListName;

            return new BotMessage(DialogsStringsStorage.NewListNameInput, null);
        }
        else if (textData == BotButtonsStorage.ListMenu.ChangeListPrivacy.CallBackData)
        {
            transmittedData.State = States.ListMenu.ClickButtonChangePrivacy;

            return new BotMessage(DialogsStringsStorage.ListPrivacyInfo(gamesList) + DialogsStringsStorage.ListPrivacy,
                InlineKeyboardMarkupStorage.ChooseListPrivacy);
        }
        else if (textData == BotButtonsStorage.ListMenu.DeleteList.CallBackData)
        {
            transmittedData.State = States.ListMenu.ListInputDeletingConfirmation;

            return new BotMessage(DialogsStringsStorage.ListDeletedConfirmation,
                InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (textData == BotButtonsStorage.ListMenu.BackToLists.CallBackData)
        {
            transmittedData.State = States.ListMenu.ClickOnInlineButtonUserLists;
            transmittedData.DataStorage.Delete("listId");

            return new BotMessage(DialogsStringsStorage.MyLists, GetGamesListsAsKeyboard(transmittedData));
        }

        return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);

        throw new Exception("Неизвестная ошибка в ProcessClickActionButtonWithList");
    }

    public ReplyKeyboardMarkup GetGamesListsAsKeyboard(TransmittedData transmittedData)
    {
        List<GamesList> gamesLists = _gamesListsRepository.GetGamesListsByChatId(transmittedData.ChatId);

        var rows = new List<KeyboardButton[]>();


        for (var i = 0; i <= gamesLists.Count; i++)
        {
            if (i == gamesLists.Count)
            {
                rows.Add(new[] { new KeyboardButton("Назад") });
            }
            else
            {
                rows.Add(new[] { new KeyboardButton(gamesLists[i].Title) });
            }
        }

        return new ReplyKeyboardMarkup(rows.ToArray()) { ResizeKeyboard = true };
    }
}