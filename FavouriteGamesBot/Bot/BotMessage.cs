using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteGamesBot.Bot;

public class BotMessage
{
    public string Text { get; }
    public IReplyMarkup KeyboardMarkup { get; }

    public BotMessage(string text)
    {
        Text = text;
        KeyboardMarkup = null;
    }

    public BotMessage(string text, IReplyMarkup keyboardMarkup)
    {
        Text = text;
        KeyboardMarkup = keyboardMarkup;
    }
}