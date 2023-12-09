using System;
using FavouriteGamesBot.Service;

namespace FavouriteGamesBot.Bot.Router;

public class ChatsRouter
{
    private ChatTransmittedDataPairs _chatTransmittedDataPairs;
    private ServiceManager _servicesManager;

    public ChatsRouter()
    {
        _servicesManager = new ServiceManager();
        _chatTransmittedDataPairs = new ChatTransmittedDataPairs();
    }

    public BotMessage Route(long chatId, string textData)
    {
        if (_chatTransmittedDataPairs.ContainsKey(chatId) == false)
        {
            _chatTransmittedDataPairs.CreateNew(chatId);
        }

        TransmittedData transmittedData = _chatTransmittedDataPairs.GetByChatId(chatId);

        Console.WriteLine($"РОУТЕР chatId = {chatId}; State = {transmittedData.State}");

        return _servicesManager.ProcessBotUpdate(textData, transmittedData);
    }
}