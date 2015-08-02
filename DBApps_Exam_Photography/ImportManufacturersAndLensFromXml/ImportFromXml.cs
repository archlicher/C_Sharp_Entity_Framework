using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using DB_First;

namespace ImportManufacturersAndLensFromXml
{
    public class ImportFromXml
    {
        static void Main()
        {
            var context = new PhotographySystemEntities();
            var doc = XDocument.Load("../../imports.xml");
            var manufacturers = doc.XPathSelectElements("/manufacturers-and-lenses/manufacturer");

            int process = 1;
            foreach (var manufacturer in manufacturers)
            {
                Console.WriteLine("Processing manufacturer #{0} ...", process);
                Manufacturer manu = ProcessManufacturer(manufacturer, context);
                var lenses = manufacturer.XPathSelectElements("lenses/lens");
                ProcessLenses(lenses, manu, context);
                Console.WriteLine();
            }
        }

        private static void ProcessLenses(IEnumerable<XElement> lenses, Manufacturer manu, PhotographySystemEntities context)
        {
            foreach (var lens in lenses)
            {

                var lensModel = lens.Attribute("model").Value;
                var lensType = lens.Attribute("type").Value;
                var xlensPrice = lens.Attribute("price");
                string lensPrice = null;
                if (xlensPrice != null)
                {
                    lensPrice = xlensPrice.Value;
                }
                var newLens = context.Lenses.FirstOrDefault(l => l.Model == lensModel);
                if (newLens == null)
                {
                    newLens = new Lens {Model = lensModel, Type = lensType};
                    if (lensPrice != null)
                    {
                        newLens.Price = decimal.Parse(lensPrice);                        
                    }
                    context.Lenses.Add(newLens);
                    manu.Lenses.Add(newLens);
                    context.SaveChanges();
                    Console.WriteLine("Created lens: {0}", lensModel);
                }
                else
                {
                    Console.WriteLine("Existing lens: {0}", lensModel);
                }
            }
        }

        private static Manufacturer ProcessManufacturer(XElement manufacturer, PhotographySystemEntities context)
        {
            var manuName = manufacturer.Element("manufacturer-name").Value;
            var manu = context.Manufacturers.FirstOrDefault(m => m.Name == manuName);
            if (manu == null)
            {
                manu = new Manufacturer {Name = manuName};
                context.Manufacturers.Add(manu);
                context.SaveChanges();
                Console.WriteLine("Created manufacturer: {0}", manuName);
            }
            else
            {
                Console.WriteLine("Existing manufacturer: {0}", manuName);
            }
            return manu;
        }
    }
}
