using System;
using System.Collections.Generic;
using FavouriteGamesBot.Bot;
using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Util.Button;
using FavouriteGamesBot.Util.String;

namespace FavouriteGamesBot.Service;

public class ServiceManager
{
    private Dictionary<string, Func<string, TransmittedData, BotMessage>>
        _methods;

    public ServiceManager()
    {
        StartMenuService startMenuService = new StartMenuService();

        _methods =
            new Dictionary<string, Func<string, TransmittedData, BotMessage>>();

        _methods[States.StartMenu.CommandStart] = startMenuService.ProcessCommandStart;
    }

    public BotMessage ProcessBotUpdate(string textData, TransmittedData transmittedData)
    {
        if (textData == SystemStringsStorage.CommandReset)
        {
            transmittedData.State = States.MainMenu.ClickOnInlineButton;
            
            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
        }

        Func<string, TransmittedData, BotMessage> serviceMethod = _methods[transmittedData.State];
        return serviceMethod.Invoke(textData, transmittedData);
    }
}