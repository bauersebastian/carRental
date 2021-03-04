using System;
using System.Threading;
using System.Linq;
namespace CarRental.Models
{
    public class Booking
    {
        private static int nextId;
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        // Relation to cars
        // one booking belongs to one car
        public int CarID { get; set; }
        // dates for the booking
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // calculate rentalDays
        public double RentalDays
        {
            get
            {
                return (this.EndDate - this.StartDate).TotalDays;
            }
        }
        // total cost calculated by days
        public Decimal TotalCost {
            get
            {
                double totalDays = this.RentalDays;
                var carCategoryCollection = CarCategoryCollection.Instance;
                // get details of carCategory to car
                CarCategory carCategory = carCategoryCollection.carCategories
                    .Single(cc => cc.CarCategoryID == this.CarCategoryID);
                Decimal costs = carCategory.RentalFeeDay * (Decimal)totalDays;
                return costs;
            }
         }
        // Relation to CarCategories
        // one booking belongs to one category
        public int CarCategoryID { get; set; }

        // parameterless constructor
        public Booking()
        {
            BookingID = Interlocked.Increment(ref nextId);
        }

        // pass in an external id
        public Booking(int id)
        {
            BookingID = id;
        }

        public override string ToString()
        {
            return ("ID: " + BookingID + " / Buchung vom " + StartDate.ToString("dd.MM.yyyy") + " bis " + EndDate.ToString("dd.MM.yyyy"));
        }

        public string showDetails()
        {
            var carCategoryCollection = CarCategoryCollection.Instance;
            // get details of carCategory to car
            CarCategory carCategory = carCategoryCollection.carCategories
                .First(cc => cc.CarCategoryID == this.CarCategoryID);
            var carCollection = CarCollection.Instance;
            // get details of car
            Car car = carCollection.cars
                .First(car => car.CarID == this.CarID);
            var customerCollection = CustomerCollection.Instance;
            // get details of customer
            Customer customer = customerCollection.customers
                .First(cust => cust.CustomerID == this.CustomerID);

            return ("ID: " + BookingID + "\n" + "Kunde: " + customer + "\n"
                + "Kategorie: " + carCategory + "\n" + "Auto: " + car + "\n"
                + "Startdatum: " + StartDate.ToString("dd.MM.yyyy") + "\n" + "Enddatum: " + EndDate.ToString("dd.MM.yyyy") + "\n"
                + "Anzahl Leihtage: " + RentalDays +"\n"
                + "Gesamtkosten: " + TotalCost + "\n");
        }
        
    }
}
