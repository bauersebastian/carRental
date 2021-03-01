using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace CarRental.Models
{
    // use a singleton pattern for one instance of all carCategories in a collection
    public sealed class CarCategoryCollection
    {
        private CarCategoryCollection()
        {
            carCategories = new List<CarCategory>();
        }
        private static CarCategoryCollection instance = null;
        public static CarCategoryCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CarCategoryCollection();
                }
                return instance;
            }
        }

        public List<CarCategory> carCategories { get; set; }
        public int CurrentId
        {
            get
            {
                return this.carCategories.Max(x => x.CarCategoryID);
            }
        }

        public void SerializeToXML(List<CarCategory> carCategories)
        {
            XmlSerializer carCategorySerializer = new XmlSerializer(typeof(List<CarCategory>));
            using (StreamWriter carCategoryWriter = new StreamWriter("carcategories.xml"))
            {
                carCategorySerializer.Serialize(carCategoryWriter, carCategories);
            }

        }

        public List<CarCategory> DeserializeFromXML()
        {
            XmlSerializer carCategorySerializer = new XmlSerializer(typeof(List<CarCategory>));
            List<CarCategory> carCategories;
            using (FileStream carCategoryFileStream = new FileStream("carcategories.xml", FileMode.Open))
            {
                try
                {
                    carCategories = (List<CarCategory>)carCategorySerializer.Deserialize(carCategoryFileStream);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Beim lesen der Daten ist ein Fehler aufgetreten", e);
                }

            }

            return carCategories;
        }

    }
}
