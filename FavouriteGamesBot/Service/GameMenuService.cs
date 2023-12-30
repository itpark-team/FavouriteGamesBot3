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

    public BotMessage ProcessGameCreating(string textData, TransmittedData transmittedData)
    {
        string state = transmittedData.State;

        if (state == States.GameMenu.InputTitle)
        {
            if (textData.Length > 25)
            {
                return new BotMessage(DialogsStringsStorage.GamePriceErrorInput, null);
            }
            else
            {
                transmittedData.State = States.GameMenu.InputPrice;
                transmittedData.DataStorage.AddOrUpdate("gameTitle", textData);

                return new BotMessage(DialogsStringsStorage.GamePriceInput, null);
            }
        }
        else if (state == States.GameMenu.InputPrice)
        {
            if (int.TryParse(textData, out int num) == false)
            {
                return new BotMessage(DialogsStringsStorage.GamePriceErrorInput, null);
            }
            else
            {
                if (num > 0)
                {
                    transmittedData.State = States.GameMenu.InputRating;
                    transmittedData.DataStorage.AddOrUpdate("gamePrice", num);

                    return new BotMessage(DialogsStringsStorage.GameRatingInput, null);
                }
                else
                {
                    return new BotMessage(DialogsStringsStorage.GamePriceErrorInput, null);
                }
            }

        }
        else if (state == States.GameMenu.InputRating)
        {

            if (int.TryParse(textData, out int num) == false)
            {
                return new BotMessage(DialogsStringsStorage.GameRatingErrorInput, null);
            }
            else
            {
                if (num < -1 || num > 11)
                {
                    return new BotMessage(DialogsStringsStorage.GameRatingErrorInput, null);
                }
                else
                {
                    transmittedData.State = States.GameMenu.InputComment;
                    transmittedData.DataStorage.AddOrUpdate("gameRating", num);

                    return new BotMessage(DialogsStringsStorage.GameDescriptionInput, null);
                }
            }
        }
        else if (state == States.GameMenu.InputComment)
        {
            transmittedData.State = States.GameMenu.InputCreatingConfirmation;
            transmittedData.DataStorage.AddOrUpdate("gameComment", textData);

            return new BotMessage(DialogsStringsStorage.Confirmation, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (state == States.GameMenu.InputCreatingConfirmation)
        {
            if (textData == BotButtonsStorage.GameMenu.Confirm.CallBackData)
            {
                transmittedData.State = States.ListMenu.ClickActionButtonWithList;

                Db.Models.Game game = new Db.Models.Game();

                game.Title = (string)transmittedData.DataStorage.Get("gameTitle");
                game.Price = (int)transmittedData.DataStorage.Get("gamePrice");
                game.Rating = (int)transmittedData.DataStorage.Get("gameRating");
                game.Comment = (string)transmittedData.DataStorage.Get("gameComment");

                GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));
                _gamesListsRepository.AddGameInGamesList(gamesList, game);

                return new BotMessage(DialogsStringsStorage.GameAdded(gamesList.Games.Last()), InlineKeyboardMarkupStorage.ListMenuChoose);
            }
            else if (textData == BotButtonsStorage.GameMenu.Cancel.CallBackData)
            {
                GamesList gamesList = _gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId"));
                transmittedData.State = States.ListMenu.ClickActionButtonWithList;
                return new BotMessage(DialogsStringsStorage.ChoosedList(gamesList), InlineKeyboardMarkupStorage.ListMenuChoose);
            }

            return new BotMessage(DialogsStringsStorage.Confirmation, InlineKeyboardMarkupStorage.GameConfirmation);
        }

        return new BotMessage(DialogsStringsStorage.GameNameInput, null);
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

        throw new NotImplementedException();
    }

    public BotMessage ProcessClickInlineButtonInActionWithGameMenu(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.GameMenu.EditGame.CallBackData)
        {
            transmittedData.State = States.GameMenu.ChooseEditParameter;

            return new BotMessage(DialogsStringsStorage.ChooseGameEditParameter, InlineKeyboardMarkupStorage.GameEditing);
        }
        else if (textData == BotButtonsStorage.GameMenu.DeleteGame.CallBackData)
        {
            transmittedData.State = States.GameMenu.InputDeletingConfirmation;

            return new BotMessage(DialogsStringsStorage.GameDeletingConfirmation, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (textData == BotButtonsStorage.GameMenu.BackToListOfGames.CallBackData)
        {
            List<Db.Models.Game> games = (List<Db.Models.Game>)_gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId")).Games;

            transmittedData.State = States.GameMenu.ClickOnInlineButtonListGames;

            return new BotMessage(DialogsStringsStorage.GamesInList, ReplyKeyboardMarkupStorage.CreateKeyboardGames(games));
        }

        throw new NotImplementedException();
    }

    public BotMessage ProcessInputDeletingConfirmation(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.GameMenu.Confirm.CallBackData)
        {
            List<Db.Models.Game> games = (List<Db.Models.Game>)_gamesListsRepository.GetGamesListById((int)transmittedData.DataStorage.Get("listId")).Games;

            transmittedData.State = States.GameMenu.ClickOnInlineButtonListGames;

            _gamesRepository.DeleteGame((int)transmittedData.DataStorage.Get("gameId"));

            return new BotMessage(DialogsStringsStorage.GamesInList, ReplyKeyboardMarkupStorage.CreateKeyboardGames(games));
        }
        else if (textData == BotButtonsStorage.GameMenu.Cancel.CallBackData)
        {
            transmittedData.State = States.GameMenu.ClickInlineButtonInActionWithGameMenu;
            Db.Models.Game game = _gamesRepository.GetGameById((int)transmittedData.DataStorage.Get("gameId"));
            return new BotMessage(DialogsStringsStorage.ChoosedGameParameters(game), InlineKeyboardMarkupStorage.GameMenuChoose, true);
        }

        return new BotMessage(DialogsStringsStorage.GameDeletingConfirmation, InlineKeyboardMarkupStorage.GameConfirmation);

        throw new NotImplementedException();
    }

    public BotMessage ProcessChooseEditParameter(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.GameMenu.InputTitle.CallBackData)
        {
            transmittedData.State = States.GameMenu.InputTitle;
            transmittedData.DataStorage.AddOrUpdate("parameter", "title");

            return new BotMessage(DialogsStringsStorage.Write + BotButtonsStorage.GameMenu.InputTitle.CallBackData, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (textData == BotButtonsStorage.GameMenu.InputPrice.CallBackData)
        {
            transmittedData.State = States.GameMenu.InputPrice;
            transmittedData.DataStorage.AddOrUpdate("parameter", "price");

            return new BotMessage(DialogsStringsStorage.Write + BotButtonsStorage.GameMenu.InputPrice.CallBackData, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (textData == BotButtonsStorage.GameMenu.InputRating.CallBackData)
        {
            transmittedData.State = States.GameMenu.InputRating;
            transmittedData.DataStorage.AddOrUpdate("parameter", "rating");

            return new BotMessage(DialogsStringsStorage.Write + BotButtonsStorage.GameMenu.InputRating.CallBackData, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (textData == BotButtonsStorage.GameMenu.InputComment.CallBackData)
        {
            transmittedData.State = States.GameMenu.InputComment;
            transmittedData.DataStorage.AddOrUpdate("parameter", "comment");

            return new BotMessage(DialogsStringsStorage.Write + BotButtonsStorage.GameMenu.InputComment.CallBackData, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (textData == BotButtonsStorage.GameMenu.Back.CallBackData)
        {
            transmittedData.State = States.GameMenu.ClickInlineButtonInActionWithGameMenu;
            Db.Models.Game game = _gamesRepository.GetGameById((int)transmittedData.DataStorage.Get("gameId"));

            return new BotMessage(DialogsStringsStorage.ChoosedGameParameters(game), InlineKeyboardMarkupStorage.GameMenuChoose);
        }

        return new BotMessage(DialogsStringsStorage.ChooseGameEditParameter, InlineKeyboardMarkupStorage.GameEditing);

        throw new NotImplementedException();
    }

    public BotMessage ProcessInputEditingGameParameter(string textData, TransmittedData transmittedData)
    {
        string parameter = (string)transmittedData.DataStorage.Get("parameter");

        if (parameter == DialogsStringsStorage.Title)
        {

            return new BotMessage(DialogsStringsStorage.Write + BotButtonsStorage.GameMenu.InputTitle.CallBackData, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (parameter == DialogsStringsStorage.Price)
        {
            transmittedData.State = States.GameMenu.InputPrice;

            return new BotMessage(DialogsStringsStorage.Write + BotButtonsStorage.GameMenu.InputPrice.CallBackData, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (parameter == DialogsStringsStorage.Rating)
        {
            transmittedData.State = States.GameMenu.InputRating;

            return new BotMessage(DialogsStringsStorage.Write + BotButtonsStorage.GameMenu.InputRating.CallBackData, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if (parameter == DialogsStringsStorage.Comment)
        {
            transmittedData.State = States.GameMenu.InputComment;

            return new BotMessage(DialogsStringsStorage.Write + BotButtonsStorage.GameMenu.InputComment.CallBackData, InlineKeyboardMarkupStorage.GameConfirmation);
        }
        else if(textData == BotButtonsStorage.GameMenu.Back.CallBackData)
        {

        }

        throw new NotImplementedException();
    }
}