using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace CarRental.Models
{
    public sealed class CarCollection
    {
        // use a singleton pattern for one instance of all cars in a collection
        private CarCollection()
        {
            cars = new List<Car>();
        }
        private static CarCollection instance = null;
        public static CarCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CarCollection();
                }
                return instance;
            }
        }

        public List<Car> cars { get; set; }

        public int CurrentId
        {
            get
            {
                return this.cars.Max(x => x.CarID);
            }
        }

        public void SerializeToXML(List<Car> cars)
        {
            XmlSerializer carSerializer = new XmlSerializer(typeof(List<Car>));
            using (StreamWriter carWriter = new StreamWriter("cars.xml"))
            {
                carSerializer.Serialize(carWriter, cars);
            }

        }

        public List<Car> DeserializeFromXML()
        {
            XmlSerializer carSerializer = new XmlSerializer(typeof(List<Car>));
            List<Car> cars;
            using (FileStream carFileStream = new FileStream("cars.xml", FileMode.Open))
            {
                try
                {
                    cars = (List<Car>)carSerializer.Deserialize(carFileStream);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Beim lesen der Daten ist ein Fehler aufgetreten", e);
                }

            }

            return cars;
        }

    }
}
