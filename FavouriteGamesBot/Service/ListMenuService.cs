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
        else
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;
            transmittedData.DataStorage.AddOrUpdate("listId", gamesList.Id);

            return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
        }

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
        if (textData.Length <= 20)
        {
            GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

            transmittedData.State = States.ListMenu.ClickActionButtonWithList;

            _gamesListsRepository.UpdateGamesListTitle(gamesList.Id, textData);

            return new BotMessage(DialogsStringsStorage.ListNameChangeSuccess, InlineKeyboardMarkupStorage.ListMenuChoose);
        }
        else
        {
            return new BotMessage(DialogsStringsStorage.ListNameInputError, null);
        }

        throw new Exception("Неизвестная ошибка в ProcessNewListName");
    }

    public BotMessage ProcessGameAdding(string textData, TransmittedData transmittedData)
    {
        string state = transmittedData.State;

        if (state == States.GameMenu.InputGameName)
        {
            if (textData.Length > 25)
            {
                return new BotMessage(DialogsStringsStorage.GamePriceErrorInput, null);
            }
            else
            {
                transmittedData.State = States.GameMenu.InputPrice;
                transmittedData.DataStorage.AddOrUpdate("gameTitle", textData);

                return new BotMessage(DialogsStringsStorage.GamePriceInput, null);
            }
        }
        else if (state == States.GameMenu.InputPrice)
        {
            if (int.TryParse(textData, out int num) == false)
            {
                return new BotMessage(DialogsStringsStorage.GamePriceErrorInput, null);
            }
            else
            {
                if (num > 0)
                {
                    transmittedData.State = States.GameMenu.InputRating;
                    transmittedData.DataStorage.AddOrUpdate("gamePrice", num);

                    return new BotMessage(DialogsStringsStorage.GameRatingInput, null);
                }
                else
                {
                    return new BotMessage(DialogsStringsStorage.GamePriceErrorInput, null);
                }
            }

        }
        else if (state == States.GameMenu.InputRating)
        {

            if (int.TryParse(textData, out int num) == false)
            {
                return new BotMessage(DialogsStringsStorage.GameRatingErrorInput, null);
            }
            else
            {
                if (num > -1 && num < 11)
                {
                    transmittedData.State = States.GameMenu.InputComment;
                    transmittedData.DataStorage.AddOrUpdate("gameRating", num);

                    return new BotMessage(DialogsStringsStorage.GameDescriptionInput, null);
                }
                else
                {
                    return new BotMessage(DialogsStringsStorage.GameRatingErrorInput, null);
                }
            }
        }
        else if (state == States.GameMenu.InputComment)
        {
            transmittedData.State = States.GameMenu.InputCheck;
            transmittedData.DataStorage.AddOrUpdate("gameComment", textData);

            return new BotMessage(DialogsStringsStorage.Confirmation, null);
        }
        else if (state == States.GameMenu.InputCheck)
        {
            if (textData == "да")
            {
                transmittedData.State = States.ListMenu.ClickActionButtonWithList;

                string title = (string)transmittedData.DataStorage.Get("gameTitle");
                int price = (int)transmittedData.DataStorage.Get("gamePrice");
                int rating = (int)transmittedData.DataStorage.Get("gameRating");
                string comment = (string)transmittedData.DataStorage.Get("gameComment");

                GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

                gamesList.Games.Add(new Game() { Title = title, Price = price, Rating = rating, Comment = comment });
                FgbDbContext dbContext = new FgbDbContext();
                dbContext.GamesLists.Update(gamesList);
                dbContext.SaveChanges();

                return new BotMessage(DialogsStringsStorage.GameAdded(gamesList.Games.Last()), InlineKeyboardMarkupStorage.ListMenuChoose);
            }
            else if (textData == "нет")
            {
                transmittedData.State = States.ListMenu.ClickActionButtonWithList;
                return new BotMessage(DialogsStringsStorage.Confirmation, null);
            }

            return new BotMessage(DialogsStringsStorage.Confirmation, null);
        }

        return new BotMessage(DialogsStringsStorage.GameNameInput, null);
    }

    public BotMessage ProcessClickOnDeleteListButton(string textData, TransmittedData transmittedData)
    {
        GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

        if (textData == "да")
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;

            _gamesListsRepository.DeleteGamesList((int)transmittedData.DataStorage.Get("listId"));

            return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
        }
        else if (textData == "нет")
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;

            return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
        }

        return new BotMessage(DialogsStringsStorage.ListDeletedConfirmation, null);
    }

    public BotMessage ProcessClickActionButtonWithList(string textData, TransmittedData transmittedData)
    {
        GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));
//todo
        switch (textData)
        {
            case "AddGame":
                transmittedData.State = States.GameMenu.InputGameName;
                return new BotMessage(DialogsStringsStorage.GameNameInput, null);
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

                return new BotMessage(DialogsStringsStorage.ListDeletedConfirmation, null);
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

        return new ReplyKeyboardMarkup(rows.ToArray());
    }

}