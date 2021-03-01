using System;
using System.Threading;

namespace CarRental.Models
{
    // Kundenobjekt zur Speicherung relevanter Informationen zum Kunden
    public class Customer
    {
        private static int nextId;
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IBAN { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public DateTime Birthday { get; set; }

        // parameterless constructor
        public Customer() {
            CustomerID = Interlocked.Increment(ref nextId);
        }

        // pass in an external id
        public Customer(int id)
        {
            CustomerID = id;
        }

        public override string ToString()
        {
            return ("ID: " + CustomerID + " " + FirstName + " " + LastName + " " + City);
        }

        public string customerDetails()
        {
            return ("ID: " + CustomerID + "\n" + "Vorname: " + FirstName + "\n"
                + "Nachname: " + LastName + "\n" + "IBAN: " + IBAN + "\n"
                + "Straße: " + Street + "\n" + "Postleitzahl: " + Postcode + "\n"
                + "Ort : " + City + "\n" + "Geburtsdatum: " + Birthday.ToString("dd.MM.yyyy"));
        }

    }

    


}
