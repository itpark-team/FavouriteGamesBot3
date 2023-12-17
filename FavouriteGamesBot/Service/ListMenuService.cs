using FavouriteGamesBot.Bot;
using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Db.DbConnector;
using FavouriteGamesBot.Db.Models;
using FavouriteGamesBot.Db.Repositories.Interfaces;
using FavouriteGamesBot.Util.Button;
using FavouriteGamesBot.Util.String;
using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteGamesBot.Service;

public class ListMenuService
{
    private IGamesListsRepository _gamesListsRepository;

    public ListMenuService(IGamesListsRepository gamesListsRepository)
    {
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
        if (textData == "BackToMainMenu")
        {
            transmittedData.State = States.MainMenu.ClickOnInlineButton;

            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
        }

        if (int.TryParse(textData, out int gamesListId) == true)
        {
            GamesList gamesList = _gamesListsRepository.GetGamesListById(gamesListId);

            if (gamesList != null)
            {
                transmittedData.State = States.ListMenu.ClickActionButtonWithList;
                transmittedData.DataStorage.AddOrUpdate("listId", gamesListId);

                return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
            }
            else
                return new BotMessage(DialogsStringsStorage.PressButton, GetGamesListsAsKeyboard(transmittedData));
        }
        else
            return new BotMessage(DialogsStringsStorage.PressButton, GetGamesListsAsKeyboard(transmittedData));

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
                return new BotMessage(DialogsStringsStorage.ListPrivacyInfo(gamesList) + DialogsStringsStorage.ListPrivacy, InlineKeyboardMarkupStorage.ChooseListPrivacy);
                break;
        }

        return new BotMessage(DialogsStringsStorage.ListPrivacySelected(gamesList) + DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);

        throw new Exception("Неизвестная ошибка в ProcessClickButtonChangePrivacy");
    }

    public BotMessage ProcessNewListName(string textData, TransmittedData transmittedData)
    {
        throw new Exception("Неизвестная ошибка в ProcessNewListName");
    }

    public BotMessage ProcessClickActionButtonWithList(string textData, TransmittedData transmittedData)
    {
        GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

        switch (textData)
        {
            case "AddGame":

                break;
            case "CheckGames":

                break;
            case "RenameList":
                transmittedData.State = States.ListMenu.NewListName;

                return new BotMessage(DialogsStringsStorage.NewListNameInput, null);
                break;
            case "ChangeListPrivacy":
                transmittedData.State = States.ListMenu.ClickButtonChangePrivacy;

                return new BotMessage(DialogsStringsStorage.ListPrivacyInfo(gamesList) + DialogsStringsStorage.ListPrivacy, InlineKeyboardMarkupStorage.ChooseListPrivacy);

                break;
            case "DeleteList":
                transmittedData.State = States.ListMenu.ClickOnDeleteListButton;
                break;
            case "BackToLists":
                transmittedData.State = States.ListMenu.ClickOnInlineButtonUserLists;
                transmittedData.DataStorage.Delete("listId");

                return new BotMessage(DialogsStringsStorage.MyLists, GetGamesListsAsKeyboard(transmittedData));
                break;
            default:
                return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
                break;
        }

        throw new Exception("Неизвестная ошибка в ProcessClickActionButtonWithList");
    }

    public InlineKeyboardMarkup GetGamesListsAsKeyboard(TransmittedData transmittedData)
    {
        List<GamesList> gamesLists = _gamesListsRepository.GetGamesListsByChatId(transmittedData.ChatId);

        var rows = new List<InlineKeyboardButton[]>();


        for (var i = 0; i <= gamesLists.Count; i++)
        {
            if (i == gamesLists.Count)
            {
                rows.Add(new[] { InlineKeyboardButton.WithCallbackData("Назад", "BackToMainMenu") });
            }
            else
            {
                rows.Add(new[] { InlineKeyboardButton.WithCallbackData(gamesLists[i].Title, gamesLists[i].Id.ToString()) });
            }
        }

        return new InlineKeyboardMarkup(rows.ToArray());
    }

}