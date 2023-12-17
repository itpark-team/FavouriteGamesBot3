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
    public string ClickButtonGameInListGamesMenu { get; } = "ClickButtonGameInListGamesMenu";

    public string ClickButtonChangePrivacy { get; } =
        "ClickButtonChangePrivacy";

    public string ClickOnDeleteListButton { get; } = "ClickOnDeleteListButton";
}

public class GameMenu
{
    public string ClickInlineButtonInActionWithGameMenu { get; } =
        "ClickInlineButtonInActionWithGameMenu";

    public string ClickInlineDeleteGameButton { get; } = "ClickInlineDeleteGameButton";
    public string InputGameName { get; } = "InputGameName";
}

public class Recomendation
{
    public string ClickOnInlineButtonInMenuReccomendationGames { get; } =
        "ClickOnInlineButtonInMenuReccomendationGames";
}