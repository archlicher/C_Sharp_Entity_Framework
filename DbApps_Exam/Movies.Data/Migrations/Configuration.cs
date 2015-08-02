using System.Web.Script.Serialization;
using Movies.Model;

namespace Movies.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Movies.Data.MoviesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MoviesContext context)
        {
            if (!context.Countries.Any())
            {
                var serializer = new JavaScriptSerializer();
                var json = File.ReadAllText("../../countries.json");
                var countries = serializer.Deserialize<CountryDTO[]>(json);
                foreach (var ct in countries)
                {
                    var country = new Country(){Name = ct.Name};
                    context.Countries.Add(country);
                }
                context.SaveChanges();

                json = File.ReadAllText("../../users.json");
                var users = serializer.Deserialize<UserDTO[]>(json);
                foreach (var us in users)
                {
                    var user = new User()
                    {
                        Username = us.Username
                    };
                    if (us.Age != null)
                    {
                        user.Age = us.Age;
                    }
                    if (us.Email != null)
                    {
                        user.Email = us.Email;
                    }
                    context.Users.Add(user);
                }
                context.SaveChanges();

                foreach (var us in users)
                {
                    if (us.Country != null)
                    {
                        var country = context.Countries.FirstOrDefault(c => c.Name == us.Country);
                        var user = context.Users.FirstOrDefault(u => u.Username == us.Username);
                        if (country != null)
                        {
                            country.Users.Add(user);
                        }
                    }
                }
                context.SaveChanges();

                json = File.ReadAllText("../../movies.json");
                var movies = serializer.Deserialize<MovieDTO[]>(json);
                foreach (var mv in movies)
                {
                    var movie = new Movie()
                    {
                        Isbn = mv.Isbn,
                        Title = mv.Title,
                        AgeRestiction = mv.AgeRestriction
                    };
                    context.Movies.Add(movie);
                }
                context.SaveChanges();

                json = File.ReadAllText("../../movie-ratings.json");
                var ratings = serializer.Deserialize<MovieRatingsDTO[]>(json);
                foreach (var rate in ratings)
                {
                    var rating = new Rating()
                    {
                        User = context.Users.FirstOrDefault(u => u.Username == rate.User),
                        Movie = context.Movies.FirstOrDefault(m => m.Isbn == rate.Movie),
                        Stars = rate.Rating
                    };
                    context.Ratings.Add(rating);
                }
                context.SaveChanges();

                json = File.ReadAllText("../../users-and-favourite-movies.json");
                var userMovies = serializer.Deserialize<UsersMoviesDTO[]>(json);
                foreach (var usMv in userMovies)
                {
                    var user = context.Users.FirstOrDefault(u => u.Username==usMv.Username);
                    foreach (var favMovie in usMv.FavouriteMovies)
                    {
                        var movie = context.Movies.FirstOrDefault(m => m.Isbn == favMovie);
                        user.FavouriteMovies.Add(movie);
                    }
                    
                }
                context.SaveChanges();
            }
        }
    }
}
