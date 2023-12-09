using System.Collections.Generic;

namespace FavouriteGamesBot.Db.Models;

public partial class Game
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    public int Price { get; set; }

    public virtual ICollection<GamesList> GameLists { get; set; } = new List<GamesList>();
}