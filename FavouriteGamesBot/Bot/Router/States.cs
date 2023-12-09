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
    public string WaitingInputListName { get; } = "WaitingInputListName";
    public string WaitingClickOnInlineButtonUserLists { get; } = "WaitingClickOnInlineButtonUserLists";
    public string WaitingClickActionButtonWithList { get; } = "WaitingClickActionButtonWithList";
    public string WaitingNewListName { get; } = "WaitingNewListName";
    public string WaitingClickButtonGameInListGamesMenu { get; } = "WaitingClickButtonGameInListGamesMenu";

    public string WaitingClickOnInlineButtonInPrivateListSettingMenu { get; } =
        "WaitingClickOnInlineButtonInPrivateListSettingMenu";

    public string ClickOnDeleteListButton { get; } = "ClickOnDeleteListButton";
}

public class GameMenu
{
    public string WaitingClickInlineButtonInActionWithGameMenu { get; } =
        "WaitingClickInlineButtonInActionWithGameMenu";

    public string ClickInlineDeleteGameButton { get; } = "ClickInlineDeleteGameButton";
    public string WaitingInputGameName { get; } = "WaitingInputGameName";
}

public class Recomendation
{
    public string WaitingClickOnInlineButtonInMenuReccomendationGames { get; } =
        "WaitingClickOnInlineButtonInMenuReccomendationGames";
}