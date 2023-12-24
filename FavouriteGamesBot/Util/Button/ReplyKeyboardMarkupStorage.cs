using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Db.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteGamesBot.Util.Button;

public class ReplyKeyboardMarkupStorage
{
    public static ReplyKeyboardMarkup CreateKeyboardGamesLists(List<GamesList> gamesLists)
    {
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