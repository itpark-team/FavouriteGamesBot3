using System.Collections.Generic;
using FavouriteGamesBot.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace FavouriteGamesBot.Db.DbConnector;

public partial class FgbDbContext : DbContext
{
    public FgbDbContext()
    {
    }

    public FgbDbContext(DbContextOptions<FgbDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GamesList> GamesLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=83.147.246.87:5432;Database=fgb_db;Username=fgb_user;Password=12345");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("games_pk");

            entity.ToTable("games");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("comment");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(25)
                .HasColumnName("title");

            entity.HasMany(d => d.GameLists).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GamesInGamesList",
                    r => r.HasOne<GamesList>().WithMany()
                        .HasForeignKey("GameListId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("games_in_games_list_games_list_id_fk"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("games_in_games_list_games_id_fk"),
                    j =>
                    {
                        j.HasKey("GameId", "GameListId").HasName("games_in_games_list_pk");
                        j.ToTable("games_in_games_list");
                        j.IndexerProperty<int>("GameId").HasColumnName("game_id");
                        j.IndexerProperty<int>("GameListId").HasColumnName("game_list_id");
                    });
        });

        modelBuilder.Entity<GamesList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("games_list_pk");

            entity.ToTable("games_list");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.IsPrivate).HasColumnName("is_private");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(25)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}