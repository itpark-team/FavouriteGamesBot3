using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Db.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteGamesBot.Util.Button;

public class ReplyKeyboardMarkupStorage
{
    public static ReplyKeyboardMarkup CreateKeyboardGamesLists(List<GamesList> gamesLists)
    {
        var rows = new List<KeyboardButton[]>();

        for (var i = 0; i < gamesLists.Count; i++)
        {
            rows.Add(new[] { new KeyboardButton(gamesLists[i].Title) });
        }

        rows.Add(new[] { new KeyboardButton("Назад") });

        return new ReplyKeyboardMarkup(rows.ToArray()) { ResizeKeyboard = true };
    }
    public static ReplyKeyboardMarkup CreateKeyboardGames(List<Game> games)
    {
        var rows = new List<KeyboardButton[]>();

        for (var i = 0; i < games.Count; i++)
        {
            rows.Add(new[] { new KeyboardButton(games[i].Title) });
        }
        rows.Add(new[] { new KeyboardButton("Назад") });

        return new ReplyKeyboardMarkup(rows.ToArray()) { ResizeKeyboard = true };
    }
}