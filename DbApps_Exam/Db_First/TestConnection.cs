using System;
using System.Linq;

namespace Db_First
{
    public class TestConnection
    {
        static void Main()
        {
            var context = new DiabloEntities();

            var characterNames = context.Characters.Select(c => c.Name).ToList();
            foreach (var name in characterNames)
            {
                Console.WriteLine(name);
            }
        }
    }
}
