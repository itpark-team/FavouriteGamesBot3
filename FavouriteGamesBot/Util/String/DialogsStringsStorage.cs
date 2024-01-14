using FavouriteGamesBot.Db.Models;

namespace FavouriteGamesBot.Util.String;

public class DialogsStringsStorage
{
    public const string CommandStartInputErrorInput = "Команда не распознана. Для начала работы с ботом введите /start";

    public const string MainMenu = "Выберите действие";

    public const string ListNameInput = "Введите название списка";

    public const string ListNameInputError = "Название списка не должно превышать 20 символов\n\n";

    public const string ListCreateSuccess = "Список создан успешно!";

    public const string MyLists = "Ваши списки";

    public const string MyListsMaxCount = "Вы можете создать не более 5 списков\n\n";

    public const string NoLists = "У вас пока нет списков\n\n";

    public const string PressButton = "Нажмите на кнопку";

    public const string ListDeleted = "Список с играми удален";

    public const string ListDeletedConfirmation = "Подтвердите удаление списка";

    public static string ChoosedList(GamesList gameList)
    {
        return "Выбран список\n\n" +
               $"{gameList.Title}\n\n" +
               "Выберите действие со списком";
    }

    public const string ListPrivacy = "Выберите приватность данного списка";

    public static string ListPrivacyInfo(GamesList gameList)
    {
        string visible = gameList.IsPrivate ? "приватный" : "публичный";

        return $"Приватность данного списка: {visible}\n\n";
    }

    public static string ListPrivacySelected(GamesList gameList)
    {
        string visible = gameList.IsPrivate ? "приватным" : "публичным";

        return $"Ваш список стал {visible}\n\n";
    }

    public const string NewListNameInput = "Введите новое название списка";

    public const string ListNameChangeSuccess = "Название списка успешно изменено!";

    public const string GameTitleInput = "Введите название игры";

    public const string GameTitleInputError = "Название игры не должно превышать 25 символов";

    public const string GamePriceInput = "Введите стоимость игры";

    public const string GamePriceErrorInput = "Некорректная стоимость";

    public const string GameRatingInput = "Введите вашу оценку игры. \n(от 0 до 10)";

    public const string GameRatingErrorInput = "Некорректная оценка игры";

    public const string GameCommentInput = "Введите ваш комментарий к данной игре";

    public const string GameCommentInputError = "Комментарий не должен превышать 150 символов!";

    public const string Confirmation = "Подтвердите создание игры, написав да или нет";

    public static string CreatedGameParameters(Game game)
    {
        return "\nПроверьте правильность введённых данных:\n\n" +
               $"Игра: {game.Title}\n" +
               $"Стоимость: {game.Price}\n" +
               $"Ваша оценка: {game.Rating}\n" +
               $"Ваш комментарий: {game.Comment}\n\n" +
               "Данные введены верно?";
    }

    public static string ChoosedGameParameters(Game game)
    {
        return $"Выбрана игра: {game.Title}\n" +
               $"Стоимость: {game.Price}\n" +
               $"Ваша оценка: {game.Rating}\n" +
               $"Ваш комментарий: {game.Comment}\n\n" +
               "Выберите действие с игрой:";
    }

    public static string GameAdded(Game game)
    {
        return $"Игра {game.Title} была создана успешно!";
    }

    public const string GamesInList = "Игры в этом списке:";
    public const string ChooseGameEditParameter = "Выберите параметр который хотите изменить";

    public const string GamesAreNull = "В этом списке нет игр";

    public const string GameDeletingConfirmation = "Подтвердите удаление игры";

    public const string Write = "Введите";

    public const string Title = "title";

    public const string Price = "price";

    public const string Rating = "rating";

    public const string Comment = "comment";
}