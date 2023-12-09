using System.Collections.Generic;

namespace FavouriteGamesBot.Db.Models;

public partial class GamesList
{
    public int Id { get; set; }

    public string Title { get; set; }

    public long ChatId { get; set; }

    public bool IsPrivate { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}