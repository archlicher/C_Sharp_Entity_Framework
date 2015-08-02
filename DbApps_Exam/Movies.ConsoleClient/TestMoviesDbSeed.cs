using System.Data.Entity;
using System.IO;
using System.Web.Script.Serialization;
using Movies.Model;

namespace Movies.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Movies.Data;

    public class TestMoviesDbSeed
    {
        static void Main()
        {
            var context = new MoviesContext();

            /*var users = context.Users.Select(u => u.Username).ToList();
            foreach (var user in users)
            {
                Console.WriteLine(user);
            }*/

            var serialiazer = new JavaScriptSerializer();

            //Problem 6 Query 1 - Adult Movies
            //QueryAdultMovies(context, serialiazer);

            //Problem 6 Query 2 - Users and Rated Movies
            //var selectedUser = context.Users.FirstOrDefault(u => u.Username == "jmeyery");
            //QueryUsersRatedMovies(context, serialiazer, selectedUser);

            //Problem 6 Query 3 - Top 10 Favourite Movies
            QueryTop10FavouriteMovies(context, serialiazer);
        }

        private static void QueryAdultMovies(MoviesContext context, JavaScriptSerializer serializer)
        {
            var adultMovies = context.Movies
                .Where(m => m.AgeRestiction == Restriction.Adult)
                .Select(m => new
                {
                    title = m.Title,
                    ratingsGiven = m.Ratings.Count
                }).ToList();

            var json = serializer.Serialize(adultMovies);
            File.WriteAllText("../../adult-movies.json", json);
        }

        private static void QueryUsersRatedMovies(MoviesContext context, JavaScriptSerializer serializer, User user)
        {
            var usersAndMovies = context.Users
                .Where(u => u.Username == user.Username)
                .Select(u => new
                {
                    username = u.Username,
                    ratedMovies = u.Ratings.Select(r => new
                    {
                        title = r.Movie.Title,
                        userRating = r.Stars,
                        averageRating = r.Movie.Ratings.Average(mr => mr.Stars)
                    }).OrderBy(r => r.title)
                }).ToList();

            var json = serializer.Serialize(usersAndMovies);
            string file = "../../rated-movies-by-" + user.Username + ".json";
            File.WriteAllText(file, json);
        }

        private static void QueryTop10FavouriteMovies(MoviesContext context, JavaScriptSerializer serializer)
        {
            var top10Movies = context.Movies
                .Select(m => new
                {
                    isbn = m.Isbn,
                    title = m.Title,
                    favouritedBy = context.Users.Where(u => u.FavouriteMovies.Contains(m)).Select(u => u.Username)
                })
                .OrderByDescending(m => m.favouritedBy.Count())
                .ThenBy(m => m.title)
                .Take(10)
                .ToList();

            var json = serializer.Serialize(top10Movies);
            File.WriteAllText("../../top-10-favourite-movies.json", json);
        }
    }
}
