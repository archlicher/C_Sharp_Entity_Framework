using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace ExportManufacturersCamerasToJson
{
    using DB_First;

    class ExportManuCamToJson
    {
        static void Main()
        {
            var context = new PhotographySystemEntities();

            var manuAndCams = context.Manufacturers
                .Include(m => m.Cameras)
                .Select(m => new
                {
                    manufacturer = m.Name,
                    cameras = m.Cameras.Select(c => new
                    {
                        model = c.Model,
                        price = c.Price
                    }).OrderBy(c => c.model)
                })
                .OrderBy(m => m.manufacturer)
                .ToList();

            var serializer = new JavaScriptSerializer();

            var json = serializer.Serialize(manuAndCams);
            File.WriteAllText("../../manufactureres-and-cameras.json", json);
        }
    }
}
