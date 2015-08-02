namespace Movies.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Movies.Model;
    using Movies.Data.Migrations;

    public class MoviesContext : DbContext
    {

        public MoviesContext()
            : base("MoviesContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MoviesContext, Configuration>());
        }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }

    }
}