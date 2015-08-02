
namespace DB_First
{
    using System.Data.Entity;
    using System;
    using System.Linq;

    public class TestConnection
    {
        public static void Main()
        {
            var context = new PhotographySystemEntities();

            var listManufacModel = context.Manufacturers
                .Include(m => m.Cameras)
                .Select(m => new
                {
                    manu = m.Name,
                    cam = m.Cameras.Select(c => c.Model).OrderBy(c => c)
                })
                .OrderBy(mc => mc.manu)
                .ToList();
            foreach (var mc in listManufacModel)
            {
                foreach (var model in mc.cam)
                {
                    Console.WriteLine("{0} {1}", mc.manu, model );
                }
            }
        }
    }
}
