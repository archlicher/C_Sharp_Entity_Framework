namespace ExportCharactersUsersAsJson
{
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;
    using Db_First;

    public class ExportCharsPlayersJson
    {
        static void Main()
        {
            var context = new DiabloEntities();

            var charactersPlayers = context.Characters.Select(c => new
            {
                name = c.Name,
                playedBy =
                    context.Users.Where(u => c.UsersGames.Select(ug => ug.UserId).Contains(u.Id))
                        .Select(u => u.Username)
            }).OrderBy(c => c.name)
                .ToList();


            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(charactersPlayers);
            File.WriteAllText("../../characters.json", json);
        }
    }
}
