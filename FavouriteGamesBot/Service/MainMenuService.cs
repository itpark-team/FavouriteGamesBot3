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

        switch (textData)
        {
            case "CreateGameList":

                if (gamesLists.Count < 5)
                {
                    transmittedData.State = States.ListMenu.InputListName;
                    return new BotMessage(DialogsStringsStorage.ListNameInput, null);
                }
                else
                    return new BotMessage(DialogsStringsStorage.MyListsMaxCount + DialogsStringsStorage.MainMenu,
                 InlineKeyboardMarkupStorage.MainMenuChoose);

                break;
            case "Lists":

                if (gamesLists.Count != 0)
                {

                    transmittedData.State = States.ListMenu.ClickOnInlineButtonUserLists;

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

                    return new BotMessage(DialogsStringsStorage.MyLists, new InlineKeyboardMarkup(rows.ToArray()));
                }
                else
                {
                    return new BotMessage(DialogsStringsStorage.NoLists + DialogsStringsStorage.MainMenu,
                        InlineKeyboardMarkupStorage.MainMenuChoose);
                }

                break;
            case "Recomendation":
                //todo
                break;
            default:
                return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
                break;
        }

        throw new Exception("Неизвестная ошибка в ProcessClickOnInlineButton");
    }
}