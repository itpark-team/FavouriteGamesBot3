using FavouriteGamesBot.Db.Models;

namespace FavouriteGamesBot.Bot.Router;

public class States
{
    public static StartMenu StartMenu { get; } = new();
    public static MainMenu MainMenu { get; } = new();
    public static ListMenu ListMenu { get; } = new();
    public static GameMenu GameMenu { get; } = new();
    public static Recomendation Recomendation { get; } = new();
}

public class StartMenu
{
    public string CommandStart { get; } = "CommandStart";
}

public class MainMenu
{
    public string ClickOnInlineButton { get; } = "ClickOnInlineButton";
}

public class ListMenu
{
    public string InputListName { get; } = "InputListName";
    public string ClickOnInlineButtonUserLists { get; } = "ClickOnInlineButtonUserLists";
    public string ClickActionButtonWithList { get; } = "ClickActionButtonWithList";
    public string NewListName { get; } = "NewListName";
    public string ClickButtonChangePrivacy { get; } = "ClickButtonChangePrivacy";
    public string ListInputDeletingConfirmation { get; } = "ListInputDeletingConfirmation";
}

public class GameMenu
{
    public string ClickOnInlineButtonListGames { get; } = "ProcessClickOnInlineButtonListGames";
    public string ClickInlineButtonInActionWithGameMenu { get; } = "ClickInlineButtonInActionWithGameMenu";
    public string ChooseEditParameter { get; } = "ChooseEditParameter";
    public string InputTitle { get; } = "InputTitle";
    public string InputPrice { get; } = "InputPrice";
    public string InputRating { get; } = "InputRating";
    public string InputComment { get; } = "InputComment";
    public string EditingInputTitle { get; } = "EditingInputTitle";
    public string EditingInputPrice { get; } = "EditingInputPrice";
    public string EditingInputRating { get; } = "EditingInputRating";
    public string EditingInputComment { get; } = "EditingInputComment";
    public string InputCreatingConfirmation { get; } = "InputCreatingConfirmation";
    public string InputDeletingConfirmation { get; } = "InputDeletingConfirmation";
}

public class Recomendation
{
    public string ClickOnInlineButtonInMenuReccomendationGames { get; } = "ClickOnInlineButtonInMenuReccomendationGames";
}