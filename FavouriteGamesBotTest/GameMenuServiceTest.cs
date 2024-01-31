using FavouriteGamesBot.Bot;
using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Db.Models;
using FavouriteGamesBot.Db.Repositories.Interfaces;
using FavouriteGamesBot.Service;
using FavouriteGamesBot.Util.String;
using Moq;

namespace FavouriteGamesBotTest;

public class GameMenuServiceTest
{
    [Fact]
    public void ProcessInputDeletingConfirmation_ThrowException()
    {
        GameMenuService gameMenuService = new GameMenuService(null, null);

        TransmittedData transmittedData = new TransmittedData(0);
        string textData = "wrong";

        Assert.Throws<Exception>(() => gameMenuService.ProcessInputDeletingConfirmation(textData, transmittedData));
    }

    [Fact]
    public void ProcessInputDeletingConfirmation_CancelButton_ReturnExpectedGame()
    {
        Game game = new Game()
        {
            Title = "title",
            Price = 111,
            Rating = 111,
            Comment = "comment"
        };

        Mock<IGamesRepository> gamesRepository = new Mock<IGamesRepository>();
        gamesRepository.Setup(exp => exp.GetGameById(0)).Returns(game);

        GameMenuService gameMenuService = new GameMenuService(gamesRepository.Object, null);

        TransmittedData transmittedData = new TransmittedData(0);
        transmittedData.DataStorage.AddOrUpdate("gameId", 0);
        string textData = "Cancel";

        string expectedText = DialogsStringsStorage.ChoosedGameParameters(game);

        BotMessage botMessage = gameMenuService.ProcessInputDeletingConfirmation(textData, transmittedData);
        string actualText = botMessage.Text;

        Assert.Equal(expectedText, actualText);
    }
}