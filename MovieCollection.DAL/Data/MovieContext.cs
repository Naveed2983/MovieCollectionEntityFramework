using Microsoft.EntityFrameworkCore;
using MovieCollection.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.DAL.Data
{
    public class MovieContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MovieCollectionDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieRating> MovieRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.UserName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(u => u.UserName)
                      .IsUnique();

                entity.Property(u => u.FullName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Password)
                      .IsRequired()
                      .HasMaxLength(20);

            });

            modelBuilder.Entity<MovieRating>()
                .Property(r => r.ImdbRating)
                .HasColumnType("float");

            modelBuilder.Entity<MovieRating>()
               .HasKey(r => r.MovieId);

            modelBuilder.Entity<Movie>()
                .HasOne(m=>m.User) 
                .WithMany(u=>u.Movies)
                .HasForeignKey(m=>m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movie>()
                .HasMany(m=>m.Genres)
                .WithMany(g=>g.Movies)
                .UsingEntity(j=>j.ToTable("MovieGenres"));
            
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Rating)
                .WithOne(r => r.Movie)
                .HasForeignKey<MovieRating>(r => r.MovieId)
                .IsRequired();

            modelBuilder.Entity<Genre>().HasData(
                new Genre { GenreId = 1, GenreName = "Action" },
                new Genre { GenreId = 2, GenreName = "Comedy" },
                new Genre { GenreId = 3, GenreName = "Drama" },
                new Genre { GenreId = 4, GenreName = "Horror" },
                new Genre { GenreId = 5, GenreName = "Sci-Fi" },
                new Genre { GenreId = 6, GenreName = "Thriller" }
                );
        }
    }
}
