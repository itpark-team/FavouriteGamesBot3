﻿using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteGamesBot.Util.Button;

public class InlineKeyboardMarkupStorage
{
    public static InlineKeyboardMarkup MainMenuChoose = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.MainMenu.CreateGameList.Name,
                BotButtonsStorage.MainMenu.CreateGameList.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.MainMenu.Lists.Name,
                BotButtonsStorage.MainMenu.Lists.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.MainMenu.Recomendation.Name,
                BotButtonsStorage.MainMenu.Recomendation.CallBackData)
        }
    });

    public static InlineKeyboardMarkup ChooseList = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.List.Name,
                BotButtonsStorage.ListMenu.List.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.System.ButtonBack.Name,
                BotButtonsStorage.System.ButtonBack.CallBackData)
        }
    });

    public static InlineKeyboardMarkup ListMenuChoose = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.AddGame.Name,
                BotButtonsStorage.ListMenu.AddGame.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.CheckGames.Name,
                BotButtonsStorage.ListMenu.CheckGames.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.RenameList.Name,
                BotButtonsStorage.ListMenu.RenameList.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.ChangeListPrivacy.Name,
                BotButtonsStorage.ListMenu.ChangeListPrivacy.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.DeleteList.Name,
                BotButtonsStorage.ListMenu.DeleteList.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.BackToLists.Name,
                BotButtonsStorage.ListMenu.BackToLists.CallBackData)
        }

    });

    public static InlineKeyboardMarkup ListOfGamesChoose = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.Game.Name,
                BotButtonsStorage.ListMenu.Game.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.BackToList.Name,
                BotButtonsStorage.ListMenu.BackToList.CallBackData)
        }
    });

    public static InlineKeyboardMarkup GameMenuChoose = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GameMenu.EditGame.Name,
                BotButtonsStorage.GameMenu.EditGame.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GameMenu.DeleteGame.Name,
                BotButtonsStorage.GameMenu.DeleteGame.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GameMenu.BackToListOfGames.Name,
                BotButtonsStorage.GameMenu.BackToListOfGames.CallBackData)
        }
    });

    public static InlineKeyboardMarkup GameConfirmation = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GameMenu.Confirm.Name,
                BotButtonsStorage.GameMenu.Confirm.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GameMenu.Cancel.Name,
                BotButtonsStorage.GameMenu.Cancel.CallBackData),
        }
    });

    public static InlineKeyboardMarkup GameEditing = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GameMenu.InputTitle.Name,
                BotButtonsStorage.GameMenu.InputTitle.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GameMenu.InputPrice.Name,
                BotButtonsStorage.GameMenu.InputPrice.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GameMenu.InputRating.Name,
                BotButtonsStorage.GameMenu.InputRating.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GameMenu.InputComment.Name,
                BotButtonsStorage.GameMenu.InputComment.CallBackData)
        },
        new[]
        {
              InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GameMenu.Back.Name,
                BotButtonsStorage.GameMenu.Back.CallBackData)
        }
    });

    public static InlineKeyboardMarkup ChooseListPrivacy = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.PublicList.Name,
                BotButtonsStorage.ListMenu.PublicList.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.PrivateList.Name,
                BotButtonsStorage.ListMenu.PrivateList.CallBackData)
        }
    });

    public static InlineKeyboardMarkup RecomentaionGames = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.Recomendation.Game.Name,
                BotButtonsStorage.Recomendation.Game.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.System.ButtonBack.Name,
                BotButtonsStorage.System.ButtonBack.CallBackData)
        }
    });

    public static InlineKeyboardMarkup RecomentaionGameMenu = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.Recomendation.Game.Name,
                BotButtonsStorage.Recomendation.Game.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.System.ButtonBack.Name,
                BotButtonsStorage.System.ButtonBack.CallBackData)
        }
    });

    public static InlineKeyboardMarkup SubmitDataChoose = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.SubmitData.CorrectData.Name,
                BotButtonsStorage.SubmitData.CorrectData.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.SubmitData.NotCorrectData.Name,
                BotButtonsStorage.SubmitData.NotCorrectData.CallBackData)
        }
    });

    public static InlineKeyboardMarkup ChooseListToAddGame = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.List.Name,
                BotButtonsStorage.ListMenu.List.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.SubmitData.NotCorrectData.Name,
                BotButtonsStorage.SubmitData.NotCorrectData.CallBackData)
        }
    });
}