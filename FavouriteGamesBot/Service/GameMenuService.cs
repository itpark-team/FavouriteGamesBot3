using FavouriteGamesBot.Bot.Router;
using FavouriteGamesBot.Bot;
using FavouriteGamesBot.Db.DbConnector;
using FavouriteGamesBot.Db.Models;
using FavouriteGamesBot.Db.Repositories.Implemintations;
using FavouriteGamesBot.Db.Repositories.Interfaces;
using FavouriteGamesBot.Util.Button;
using FavouriteGamesBot.Util.String;
using Telegram.Bot.Types;
using System.Reflection.Metadata;

namespace FavouriteGamesBot.Service;

public class GameMenuService
{
    private IGamesRepository _gamesRepository;
    private IGamesListsRepository _gamesListsRepository;

    public GameMenuService(IGamesRepository gamesRepository, IGamesListsRepository gamesListsRepository)
    {
        _gamesRepository = gamesRepository;
        _gamesListsRepository = gamesListsRepository;
    }

    public BotMessage ProcessInputTitle(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.GameTitleMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.GameTitleInputError);
        }

        transmittedData.State = States.GameMenu.InputPrice;
        transmittedData.DataStorage.AddOrUpdate("gameTitle", textData);

        return new BotMessage(DialogsStringsStorage.GamePriceInput);
    }

    public BotMessage ProcessInputPrice(string textData, TransmittedData transmittedData)
    {
        if (int.TryParse(textData, out int inputPrice) == false)
        {
            return new BotMessage(DialogsStringsStorage.GamePriceErrorInput);
        }
        if (inputPrice < 0)
        {
            return new BotMessage(DialogsStringsStorage.GamePriceErrorInput);
        }

        transmittedData.State = States.GameMenu.InputRating;
        transmittedData.DataStorage.AddOrUpdate("gamePrice", inputPrice);

        return new BotMessage(DialogsStringsStorage.GameRatingInput);
    }

    public BotMessage ProcessInputRating(string textData, TransmittedData transmittedData)
    {
        if (int.TryParse(textData, out int inputRating) == false)
        {
            return new BotMessage(DialogsStringsStorage.GameRatingErrorInput);
        }
        if (inputRating < ConstraintStringsStorage.GameMinRating || inputRating > ConstraintStringsStorage.GameMaxRating)
        {
            return new BotMessage(DialogsStringsStorage.GameRatingErrorInput);
        }

        transmittedData.State = States.GameMenu.InputComment;
        transmittedData.DataStorage.AddOrUpdate("gameRating", inputRating);

        return new BotMessage(DialogsStringsStorage.GameCommentInput);
    }

    public BotMessage ProcessInputComment(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.GameCommentMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.GameCommentInputError);
        }

        transmittedData.State = States.GameMenu.InputCreatingConfirmation;

        Db.Models.Game game = new Db.Models.Game();

        game.Title = (string)transmittedData.DataStorage.Get("gameTitle");
        game.Price = (int)transmittedData.DataStorage.Get("gamePrice");
        game.Rating = (int)transmittedData.DataStorage.Get("gameRating");
        game.Comment = textData;

        transmittedData.DataStorage.AddOrUpdate("Game", game);

        return new BotMessage(DialogsStringsStorage.Confirmation + DialogsStringsStorage.CreatedGameParameters(game), InlineKeyboardMarkupStorage.GameConfirmation);
    }

    public BotMessage ProcessInputCreatingConfirmation(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.GameMenu.Cancel.CallBackData)
        {
            GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;
            return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
        }
        if (textData == BotButtonsStorage.GameMenu.Confirm.CallBackData)
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;

            Db.Models.Game game = (Db.Models.Game)transmittedData.DataStorage.Get("Game");

            GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));
            _gamesListsRepository.AddGameInGamesList(gamesList, game);
            transmittedData.DataStorage.Delete("Game");

            return new BotMessage(DialogsStringsStorage.GameAdded(gamesList.Games.Last()), InlineKeyboardMarkupStorage.ListMenuChoose);
        }

        throw new Exception("Неизвестная ошибка в ProcessInputCreatingConfirmation");
    }

    public BotMessage ProcessClickOnInlineButtonListGames(string textData, TransmittedData transmittedData)
    {
        GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

        if (textData == BotButtonsStorage.ListMenu.BackToLists.Name)
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;

            return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose, true);
        }

        Db.Models.Game game = _gamesRepository.GetGameByTitle(gamesList, textData);

        if (game == null)
        {
            return new BotMessage(DialogsStringsStorage.GamesInList, ReplyKeyboardMarkupStorage.CreateKeyboardGames((List<Db.Models.Game>)gamesList.Games));
        }

        transmittedData.State = States.GameMenu.ClickInlineButtonInActionWithGameMenu;
        transmittedData.DataStorage.AddOrUpdate("gameId", game.Id);

        return new BotMessage(DialogsStringsStorage.ChoosedGameParameters(game), InlineKeyboardMarkupStorage.GameMenuChoose, true);
    }

    public BotMessage ProcessClickInlineButtonInActionWithGameMenu(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.GameMenu.EditGame.CallBackData)
        {
            transmittedData.State = States.GameMenu.ChooseEditParameter;

            return new BotMessage(DialogsStringsStorage.ChooseGameEditParameter, InlineKeyboardMarkupStorage.GameEditing);
        }
        if (textData == BotButtonsStorage.GameMenu.DeleteGame.CallBackData)
        {
            transmittedData.State = States.GameMenu.InputDeletingConfirmation;

            return new BotMessage(DialogsStringsStorage.GameDeletingConfirmation, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        if (textData == BotButtonsStorage.GameMenu.BackToListOfGames.CallBackData)
        {
            List<Db.Models.Game> games = (List<Db.Models.Game>)_gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId")).Games;

            transmittedData.State = States.GameMenu.ClickOnInlineButtonListGames;

            return new BotMessage(DialogsStringsStorage.GamesInList, ReplyKeyboardMarkupStorage.CreateKeyboardGames(games));
        }

        throw new Exception("Неизвестная ошибка в ProcessClickInlineButtonInActionWithGameMenu");
    }

    public BotMessage ProcessInputDeletingConfirmation(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.GameMenu.Confirm.CallBackData)
        {
            _gamesRepository.DeleteGame((int)transmittedData.DataStorage.Get("gameId"));

            List<Db.Models.Game> games = (List<Db.Models.Game>)_gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId")).Games;
            if (games.Count == 0)
            {
                GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));

                transmittedData.State = States.ListMenu.ClickActionButtonWithList;

                return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
            }

            transmittedData.State = States.GameMenu.ClickOnInlineButtonListGames;

            return new BotMessage(DialogsStringsStorage.GamesInList, ReplyKeyboardMarkupStorage.CreateKeyboardGames(games));
        }
        if (textData == BotButtonsStorage.GameMenu.Cancel.CallBackData)
        {
            transmittedData.State = States.GameMenu.ClickInlineButtonInActionWithGameMenu;
            Db.Models.Game game = _gamesRepository.GetGameById((int)transmittedData.DataStorage.Get("gameId"));
            return new BotMessage(DialogsStringsStorage.ChoosedGameParameters(game), InlineKeyboardMarkupStorage.GameMenuChoose, true);
        }
        
        throw new Exception("Неизвестная ошибка в ProcessInputDeletingConfirmation");
    }

    public BotMessage ProcessChooseEditParameter(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.GameMenu.InputTitle.CallBackData)
        {
            transmittedData.State = States.GameMenu.EditingInputTitle;

            return new BotMessage(DialogsStringsStorage.GameTitleInput);
        }
        if (textData == BotButtonsStorage.GameMenu.InputPrice.CallBackData)
        {
            transmittedData.State = States.GameMenu.EditingInputPrice;

            return new BotMessage(DialogsStringsStorage.GamePriceInput);
        }
        if (textData == BotButtonsStorage.GameMenu.InputRating.CallBackData)
        {
            transmittedData.State = States.GameMenu.EditingInputRating;

            return new BotMessage(DialogsStringsStorage.GameRatingInput);
        }
        if (textData == BotButtonsStorage.GameMenu.InputComment.CallBackData)
        {
            transmittedData.State = States.GameMenu.EditingInputComment;

            return new BotMessage(DialogsStringsStorage.GameCommentInput);
        }
        if (textData == BotButtonsStorage.GameMenu.Back.CallBackData)
        {
            transmittedData.State = States.GameMenu.ClickInlineButtonInActionWithGameMenu;
            Db.Models.Game game = _gamesRepository.GetGameById((int)transmittedData.DataStorage.Get("gameId"));

            return new BotMessage(DialogsStringsStorage.ChoosedGameParameters(game), InlineKeyboardMarkupStorage.GameMenuChoose);
        }

        throw new Exception("Неизвестная ошибка в ProcessChooseEditParameter");
    }

    public BotMessage ProcessEditingInputTitle(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.GameTitleMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.GameTitleInputError);
        }

        transmittedData.State = States.GameMenu.ChooseEditParameter;

        Db.Models.Game game = _gamesRepository.GetGameById((int)transmittedData.DataStorage.Get("gameId"));
        game.Title = textData;

        _gamesRepository.UpdateGame(game);

        return new BotMessage(DialogsStringsStorage.ChooseGameEditParameter, InlineKeyboardMarkupStorage.GameEditing);
    }
    public BotMessage ProcessEditingInputPrice(string textData, TransmittedData transmittedData)
    {
        if (int.TryParse(textData, out int inputPrice) == false)
        {
            return new BotMessage(DialogsStringsStorage.GamePriceErrorInput);
        }
        if (inputPrice < 0)
        {
            return new BotMessage(DialogsStringsStorage.GamePriceErrorInput);
        }

        transmittedData.State = States.GameMenu.ChooseEditParameter;

        Db.Models.Game game = _gamesRepository.GetGameById((int)transmittedData.DataStorage.Get("gameId"));
        game.Price = inputPrice;

        _gamesRepository.UpdateGame(game);

        return new BotMessage(DialogsStringsStorage.ChooseGameEditParameter, InlineKeyboardMarkupStorage.GameEditing);
    }
    public BotMessage ProcessEditingInputRating(string textData, TransmittedData transmittedData)
    {
        if (int.TryParse(textData, out int inputRating) == false)
        {
            return new BotMessage(DialogsStringsStorage.GameRatingErrorInput);
        }
        if (inputRating < ConstraintStringsStorage.GameMinRating || inputRating > ConstraintStringsStorage.GameMaxRating)
        {
            return new BotMessage(DialogsStringsStorage.GameRatingErrorInput);
        }

        transmittedData.State = States.GameMenu.ChooseEditParameter;

        Db.Models.Game game = _gamesRepository.GetGameById((int)transmittedData.DataStorage.Get("gameId"));
        game.Rating = inputRating;

        _gamesRepository.UpdateGame(game);
        return new BotMessage(DialogsStringsStorage.ChooseGameEditParameter, InlineKeyboardMarkupStorage.GameEditing);
    }
    public BotMessage ProcessEditingInputComment(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.GameCommentMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.GameCommentInputError);
        }

        transmittedData.State = States.GameMenu.ChooseEditParameter;

        Db.Models.Game game = _gamesRepository.GetGameById((int)transmittedData.DataStorage.Get("gameId"));
        game.Comment = textData;

        _gamesRepository.UpdateGame(game);
        return new BotMessage(DialogsStringsStorage.ChooseGameEditParameter, InlineKeyboardMarkupStorage.GameEditing);
    }
}