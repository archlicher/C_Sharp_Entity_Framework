namespace ImportUsersAndGamesFromXml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Db_First;

    public class ImportUsersAndGames
    {
        static void Main()
        {
            var context = new DiabloEntities();

            var doc = XDocument.Load("../../users-and-games.xml");
            var users = doc.XPathSelectElements("/users/user");
            foreach (var us in users)
            {
                User user = ProcessUsers(us, context);
                var games = us.XPathSelectElements("games/game");
                ProcessGames(games, user, context);
            }
        }

        private static void ProcessGames(IEnumerable<XElement> games, User user, DiabloEntities context)
        {
            foreach (var gm in games)
            {
                var gameName = gm.Element("game-name").Value;
                var characterName = gm.Element("character").Attribute("name").Value;
                var characterCash = gm.Element("character").Attribute("cash").Value;
                var characterLevel = gm.Element("character").Attribute("level").Value;
                var joinedOn = gm.Element("joined-on").Value;
                var game = context.Games.FirstOrDefault(g => g.Name == gameName);
                var character = context.Characters.FirstOrDefault(c => c.Name == characterName);
                if (game != null && character != null)
                {
                    var userGame = new UsersGame()
                    {
                        Game = game,
                        User = user,
                        Character = character,
                        Level = int.Parse(characterLevel),
                        JoinedOn = DateTime.Parse(joinedOn),
                        Cash = decimal.Parse(characterCash)
                    };
                    if (
                        !context.UsersGames.Any(
                            ug =>
                                ug.Game.Name == game.Name && ug.User.Username == user.Username &&
                                ug.Character.Name == character.Name))
                    {
                        context.UsersGames.Add(userGame);
                        context.SaveChanges();
                        Console.WriteLine("User {0} successfully added to game {1}", userGame.User.Username, userGame.Game.Name);
                    }
                }
            }
        }

        private static User ProcessUsers(XElement us, DiabloEntities context)
        {
            var username = us.Attribute("username").Value;
            var regDate = us.Attribute("registration-date").Value;
            var isDeleted = us.Attribute("is-deleted").Value;
            var ipAddress = us.Attribute("ip-address").Value;
            string firstName = null;
            string lastName = null;
            string email = null;
            if (us.Attribute("first-name") != null)
            {
                firstName = us.Attribute("first-name").Value;
            }
            if (us.Attribute("last-name") != null)
            {
                lastName = us.Attribute("last-name").Value;
            }
            if (us.Attribute("email") != null)
            {
                email = us.Attribute("email").Value;
            }
            var user = context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                user = new User()
                {
                    Username = username,
                    FirstName = firstName,
                    LastName = lastName,
                    RegistrationDate = DateTime.Parse(regDate),
                    IsDeleted = int.Parse(isDeleted) != 0,
                    IpAddress = ipAddress,
                    Email = email
                };
                context.Users.Add(user);
                context.SaveChanges();
                Console.WriteLine("Successfully added user {0}", username);
            }
            else
            {
                Console.WriteLine("User {0} already exists", username);
            }
            return user;
        }
    }
}
