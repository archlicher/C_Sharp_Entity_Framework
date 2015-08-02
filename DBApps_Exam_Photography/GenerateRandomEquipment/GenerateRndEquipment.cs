using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using DB_First;

namespace GenerateRandomEquipment
{
    public class GenerateRndEquipment
    {
        static void Main()
        {
            var context = new PhotographySystemEntities();
            var doc = XDocument.Load("../../generate-equipment.xml");
            var generates = doc.XPathSelectElements("/generate-random-equipments/generate");
            int process = 1;
            foreach (var generate in generates)
            {
                Console.WriteLine("Processing manufacturer #{0} ...", process);
                var xmlSpec = new XmlSpec();
                if (generate.Element("manufacturer") != null)
                {
                    xmlSpec.ManuName = generate.Element("manufacturer").Value;
                }
                if (generate.Attribute("generate-count") != null)
                {
                    xmlSpec.Count = int.Parse(generate.Attribute("generate-count").Value);
                }
                GenRndEquip(xmlSpec, context);
                Console.WriteLine();
            }
        }

        private class XmlSpec
        {
            public XmlSpec()
            {
                this.ManuName = "Nikon";
                this.Count = 10;
            }
            public string ManuName { get; set; }
            public int Count { get; set; }
        }

        private static void GenRndEquip(XmlSpec xmlSpec, PhotographySystemEntities context)
        {
            Random rnd = new Random();
            for (int i = 0; i < xmlSpec.Count; i++)
            {
                var equipment = new Equipment();
                var lenses = context.Lenses.Where(l => l.Manufacturer.Name == xmlSpec.ManuName).Select(l => l).ToList();
                var cameras = context.Cameras.Where(c => c.Manufacturer.Name == xmlSpec.ManuName).Select(c => c).ToList();
                equipment.Lens = lenses.ElementAt(rnd.Next(lenses.Count()));
                equipment.Camera = cameras.ElementAt(rnd.Next(cameras.Count()));
                context.Equipments.Add(equipment);
                context.SaveChanges();
                Console.WriteLine("Equipment added: {0} (Camera: {1} - Lens: {2})", 
                    xmlSpec.ManuName, 
                    equipment.Camera.Model,
                    equipment.Lens.Model);
            }
        }
    }
}
