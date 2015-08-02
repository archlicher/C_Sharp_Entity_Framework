namespace Movies.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        private ICollection<Rating> ratings;
        private ICollection<Movie> favouriteMovies;

        public User()
        {
            this.ratings = new HashSet<Rating>();
            this.favouriteMovies = new HashSet<Movie>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        public int? Age { get; set; }

        public string Email { get; set; }

        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Rating> Ratings
        {
            get { return this.ratings; }
            set { this.ratings = value; }
        }

        public virtual ICollection<Movie> FavouriteMovies
        {
            get { return this.favouriteMovies; }
            set { this.favouriteMovies = value; }
        }
    }
}
