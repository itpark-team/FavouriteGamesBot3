using System;
using FavouriteGamesBot.Bot;
using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Util;
using FavouriteGamesBot.Util.Button;
using FavouriteGamesBot.Util.String;

namespace FavouriteGamesBot.Service;

public class StartMenuService
{
    public BotMessage ProcessCommandStart(string textData, TransmittedData transmittedData)
    {
        if (textData == SystemStringsStorage.CommandStart)
        {
            transmittedData.State = States.MainMenu.ClickOnInlineButton;
            
            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
        }
        else
        {
            return new BotMessage(DialogsStringsStorage.CommandStartInputErrorInput);
        }

        throw new Exception("Неизвестная ошибка в ProcessCommandStart");
    }
}