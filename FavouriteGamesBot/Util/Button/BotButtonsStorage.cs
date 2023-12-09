namespace FavouriteGamesBot.Util.Button;

public static class BotButtonsStorage
{
    public static MainMenu MainMenu { get; } = new();
    public static ListMenu ListMenu { get; } = new();
    public static GameMenu GameMenu { get; } = new();
    public static Recomendation Recomendation { get; } = new();
    public static SubmitData SubmitData { get; } = new();
    public static System System { get; } = new();
}

public class MainMenu
{
    public BotButton CreateGameList { get; } = new("Создать список", "CreateGameList");
    public BotButton Lists { get; } = new("Просмотреть свои списки", "Lists");
    public BotButton Recomendation { get; } = new("Рекомендации", "Recomendation");
}

public class ListMenu
{
    public BotButton AddGame { get; } = new("Добавить игру", "AddGame");
    public BotButton CheckGames { get; } = new("Посмотреть игры", "CheckGames");
    public BotButton RenameList { get; } = new("Переименовать список", "RenameList");
    public BotButton ChangeListPrivacy { get; } = new("Настроить приватность", "ChangeListPrivacy");
    public BotButton DeleteList { get; } = new("Удалить список", "DeleteList");
    public BotButton BackToLists { get; } = new("Назад", "BackToLists");


    public BotButton PublicList { get; } = new("Публичный", "PublicList");
    public BotButton PrivateList { get; } = new("Приватный", "PrivateList");


    public BotButton List { get; } = new("Список", "ListBtn");
    public BotButton Game { get; } = new("IT-PARK", "Game");
    public BotButton BackToList { get; } = new("Назад", "BackToList");
}

public class GameMenu
{
    public BotButton ChangeGame { get; } = new("Изменить игру", "ChangeGame");
    public BotButton DeleteGame { get; } = new("Удалить игру", "DeleteGame");
    public BotButton BackToListOfGames { get; } = new("Назад", "BackToListOfGames");
}

public class Recomendation
{
    public BotButton Game { get; } = new("Игра", "Game");
}

public class SubmitData
{
    public BotButton CorrectData { get; } = new("Да", "CorrectData");
    public BotButton NotCorrectData { get; } = new("Нет", "NotCorrectData");
}

public class System
{
    public BotButton ButtonBack { get; } = new("Обратно в главное меню", "ButtonBack");
}