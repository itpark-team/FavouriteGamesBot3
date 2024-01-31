using FavouriteGamesBot.Bot;
using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Service;

namespace FavouriteGamesBotTest;

public class StartMenuServiceTest
{
    [Fact]
    public void ProcessCommandStart_ReturnStatesMainMenuClickOnInlineButton()
    {
        //arrange - подготовка
        StartMenuService startMenuService = new StartMenuService();

        TransmittedData transmittedData = new TransmittedData(0);
        string textData = "/start";

        //act - действие
        BotMessage botMessage = startMenuService.ProcessCommandStart(textData, transmittedData);

        string expectedState = "ClickOnInlineButton";
        string actualState = transmittedData.State;

        //assert - проверка
        Assert.Equal(expectedState, actualState);
    }
}