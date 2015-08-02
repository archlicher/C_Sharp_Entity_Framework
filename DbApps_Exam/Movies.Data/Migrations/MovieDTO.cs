namespace Movies.Data.Migrations
{
    using Movies.Model;

    class MovieDTO
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public Restriction AgeRestriction { get; set; }
    }
}
