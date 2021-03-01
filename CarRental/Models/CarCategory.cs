using System;
using System.Threading;
namespace CarRental.Models
{
    public class CarCategory
    {
        private static int nextId;
        public int CarCategoryID { get; set; }
        public string Name { get; set; }
        public Decimal RentalFeeDay { get; set; }
        public Decimal RentalFeeWeek {
            get
            {
                // calculate the weekly fee based on the daily fee
                return RentalFeeDay * 7;
            }
        }

        // parameterless constructor
        public CarCategory()
        {
            CarCategoryID = Interlocked.Increment(ref nextId);
        }

        // pass in an external id
        public CarCategory(int id)
        {
            CarCategoryID = id;
        }

        public override string ToString()
        {
            return ("ID: " + CarCategoryID + " " + Name);
        }
    }
}
