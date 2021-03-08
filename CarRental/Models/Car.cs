using System;
using System.Threading;
using System.Linq;
namespace CarRental.Models
{
    public class Car
    {
        private static int nextId;
        public int CarID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int HorsePower { get; set; }
        public int LuggageCompartment { get; set; }
        public int Doors { get; set; }
        // only automatic or manual allowed
        public string Transmission { get; set; }
        // Relation to CarCategories
        // one car belongs to one category
        public int CarCategoryID { get; set; }

        // parameterless constructor
        public Car()
        {
            CarID = Interlocked.Increment(ref nextId);
        }

        // pass in an external id
        public Car(int id)
        {
            CarID = id;
        }

        public override string ToString()
        {
            return ("ID: " + CarID + " " + Brand + " " + Model);
        }

        public string carDetails()
        {
            var carCategoryCollection = CarCategoryCollection.Instance;
            // get details of carCategory to car
            CarCategory carCategory = carCategoryCollection.carCategories
                .First(cc => cc.CarCategoryID == this.CarCategoryID);

            return ("ID: " + CarID + "\n" + "Marke: " + Brand + "\n"
                + "Modell: " + Model + "\n" + "PS: " + HorsePower + "\n"
                + "Kofferraumvolumen: " + LuggageCompartment + "\n" + "Türen: " + Doors + "\n"
                + "Art des Getriebes : " + Transmission + "\n" + "Kategorie: " + carCategory + "\n"
                + "Tagesgebühr: " + carCategory.RentalFeeDay + "\n" + "Wochengebühr: " + carCategory.RentalFeeWeek);
        }

    }
}
