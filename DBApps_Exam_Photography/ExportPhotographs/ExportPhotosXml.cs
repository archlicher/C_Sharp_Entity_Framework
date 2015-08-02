namespace ExportPhotographs
{
    using System.Linq;
    using System.Xml.Linq;
    using DB_First;

    public class ExportPhotosXml
    {
        static void Main()
        {
            var context = new PhotographySystemEntities();

            var photos = context.Photographs
                .Select(p => new
                {
                    title = p.Title,
                    category = p.Category.Name,
                    link = p.Link,
                    cameraManu = p.Equipment.Camera.Manufacturer.Name,
                    cameraModel = p.Equipment.Camera.Model,
                    cameraPxs = p.Equipment.Camera.Megapixels,
                    lensManu = p.Equipment.Lens.Manufacturer.Name,
                    lensModel = p.Equipment.Lens.Model,
                    lensPrice = p.Equipment.Lens.Price
                }).OrderBy(p => p.title)
                .ToList();

            var root = new XElement("photographs");
            foreach (var ph in photos)
            {
                var xPhoto = new XElement("photograph");
                xPhoto.Add(new XAttribute("title", ph.title));
                xPhoto.Add(new XElement("category", ph.category));
                xPhoto.Add(new XElement("link", ph.link));
                var xEquip = new XElement("equipment");
                xEquip.Add(new XElement("camera", ph.cameraManu + " " + ph.cameraModel, new XAttribute("megapixels", ph.cameraPxs)));
                var lens = new XElement("lens", ph.lensManu + " " + ph.lensModel);
                if (ph.lensPrice != null)
                {
                    lens.Add(new XAttribute("price", ph.lensPrice));
                }
                xEquip.Add(lens);
                xPhoto.Add(xEquip);
                root.Add(xPhoto);
            }
            var doc = new XDocument();
            doc.Add(root);
            doc.Save("../../photographs.xml");
        }
    }
}
