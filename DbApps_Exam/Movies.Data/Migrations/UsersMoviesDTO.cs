namespace Movies.Data.Migrations
{
    using System.Collections.Generic;

    public class UsersMoviesDTO
    {
        public string Username { get; set; }
        public ICollection<string> FavouriteMovies { get; set; }
    }
}
