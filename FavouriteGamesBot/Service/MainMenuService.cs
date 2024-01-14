using FavouriteGamesBot.Bot;
using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Db.DbConnector;
using FavouriteGamesBot.Db.Models;
using FavouriteGamesBot.Db.Repositories.Interfaces;
using FavouriteGamesBot.Util.Button;
using FavouriteGamesBot.Util.String;
using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteGamesBot.Service;

public class MainMenuService
{
    private IGamesListsRepository _gamesListsRepository;

    public MainMenuService(IGamesListsRepository gamesListsRepository)
    {
        _gamesListsRepository = gamesListsRepository;
    }

    public BotMessage ProcessClickOnInlineButton(string textData, TransmittedData transmittedData)
    {
        List<GamesList> gamesLists = _gamesListsRepository.GetGamesListsByChatId(transmittedData.ChatId);

        if (textData == ConstraintStringsStorage.CreateGameList)
        {
            transmittedData.State = States.ListMenu.InputListName;
            return new BotMessage(DialogsStringsStorage.ListNameInput);
        }
        if (textData == ConstraintStringsStorage.Lists)
        {
            if (gamesLists.Count == 0)
            {
                return new BotMessage(DialogsStringsStorage.NoLists + DialogsStringsStorage.MainMenu,
                    InlineKeyboardMarkupStorage.MainMenuChoose);
            }

            transmittedData.State = States.ListMenu.ClickOnInlineButtonUserLists;

            return new BotMessage(DialogsStringsStorage.MyLists, ReplyKeyboardMarkupStorage.CreateKeyboardGamesLists(gamesLists));
        }
        if (textData == ConstraintStringsStorage.Recomendations)
        {
            //todo
        }

        return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);

        throw new Exception("Неизвестная ошибка в ProcessClickOnInlineButton");
    }
}