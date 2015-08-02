namespace FinishedGamesXml
{
    using System.Linq;
    using System.Xml.Linq;
    using Db_First;

    public class FinishedGamesToXml
    {
        static void Main()
        {
            var context = new DiabloEntities();

            var finishedGames = context.Games
                .Where(g => g.IsFinished == true)
                .Select(g => new
                {
                    name = g.Name,
                    duration = g.Duration,
                    users = context.Users.Where(u => g.UsersGames.Select(ug => ug.UserId).Contains(u.Id))
                    .Select(u => new
                    {
                        username = u.Username,
                        ip = u.IpAddress
                    })
                }).OrderBy(g => g.name)
                .ThenBy(g => g.duration)
                .ToList();

            var root = new XElement("games");
            foreach (var fGame in finishedGames)
            {
                var game = new XElement("game", new XAttribute("name", fGame.name));
                if (fGame.duration != null)
                {
                    game.Add(new XAttribute("duration", fGame.duration));
                }
                var users = new XElement("users");
                foreach (var us in fGame.users)
                {
                    var user = new XElement("user", new XAttribute("username", us.username),
                        new XAttribute("ip-address", us.ip));
                    users.Add(user);
                }
                game.Add(users);
                root.Add(game);
            }
            var doc = new XDocument();
            doc.Add(root);
            doc.Save("../../finished-games.xml");
        }
    }
}
