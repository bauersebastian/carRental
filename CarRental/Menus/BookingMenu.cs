using System;
using System.Threading.Tasks;
using CarRental.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Menus
{
    public class BookingMenu
    {
        public static bool BookingMenuConsole()
        {
            Console.Clear();
            Console.Title = "Autovermietung VAWi GmbH / Buchungen verwalten";
            Console.WriteLine("Buchungen verwalten");
            Console.WriteLine("Bitte wählen Sie eine Option:");
            Console.WriteLine("1) Buchung anlegen");
            Console.WriteLine("2) Buchung bearbeiten");
            Console.WriteLine("3) Buchung löschen");
            Console.WriteLine("4) Buchung anzeigen");
            Console.WriteLine("5) zum Hauptmenü");
            Console.Write("Auswahl von Option Nummer: ");
            switch (Console.ReadLine())
            {
                case "1":
                    var car = createBooking();
                    Console.WriteLine("Buchung wurde angelegt");
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "2":
                    var editedCar = editBooking();
                    Console.WriteLine("Auto wurde geändert.");
                    Task.Delay(2000).Wait();
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    return false;
                case "3":
                    deleteBooking();
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "4":
                    Console.WriteLine(showBooking());
                    Console.WriteLine("<Enter> drücken um fortzufahren.");
                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                    Console.WriteLine("Zurück zum Hauptmenü");
                    Task.Delay(2000).Wait();
                    return false;
                case "5":
                    return false;
                default:
                    // Show a message
                    Console.WriteLine("Bitte eine valide Option eingeben.");
                    // Wait for 2 seconds, otherwise we don't see the message
                    Task.Delay(2000).Wait();
                    return true;
            }

        }

        public static Booking createBooking()
        {
            var bookingCollection = BookingCollection.Instance;
            var customerCollection = CustomerCollection.Instance;
            var carCategoryCollection = CarCategoryCollection.Instance;
            var carCollection = CarCollection.Instance;
            Booking newBooking;
            // if we have bookings get a new ID - highest value of all ids
            if (bookingCollection.bookings.Count > 0)
            {
                var bookingNewId = bookingCollection.CurrentId;
                // increment id
                ++bookingNewId;
                newBooking = new Booking(bookingNewId);
            }
            else
            {
                // no bookings yet - create a new one without external id
                newBooking = new Booking();
            }
            Console.Clear();
            Console.Title = "Autovermietung VAWi GmbH / Buchung anlegen";
            Console.WriteLine("Buchung anlegen");
            Console.WriteLine("Kunde auswählen:");
            foreach(Customer customer in customerCollection.customers)
            {
                Console.WriteLine(customer);
            }
            newBooking.CustomerID = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Kategorie des Autos - bitte wählen:");
            foreach (CarCategory carCategory in carCategoryCollection.carCategories)
            {
                Console.WriteLine(carCategory);
            }
            newBooking.CarCategoryID = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Auto aus der gewählten Kategorie - bitte auswählen:");
            var possibleCars = carCollection.cars
                .Where(records => records.CarCategoryID == newBooking.CarCategoryID)
                .ToList();
            foreach (Car car in possibleCars)
            {
                Console.WriteLine(car);
            }
            newBooking.CarID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Bitte Startdatum der Buchung im Format dd-MM-yyyy eingeben:");
            string v = Console.ReadLine();
            DateTime dt;
            while (!DateTime.TryParseExact(v, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Datums Eingabe bitte im genannten Format vornehmen.");
                v = Console.ReadLine();
            }
            newBooking.StartDate = dt;
            Console.Write("Bitte Endedatum im Format dd-MM-yyyy eingeben:");
            v = Console.ReadLine();
            while (!DateTime.TryParseExact(v, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Datums Eingabe bitte im genannten Format vornehmen.");
                v = Console.ReadLine();
            }
            newBooking.EndDate = dt;

            bookingCollection.bookings.Add(newBooking);
            bookingCollection.SerializeToXML(bookingCollection.bookings);
            return newBooking;
        }

        public static Booking editBooking()
        {
            var bookingCollection = BookingCollection.Instance;
            int bookingId;
            // if there are no bookings we leave the method
            if (bookingCollection.bookings == null)
            {
                return null;
            }
            Console.Clear();
            foreach (Booking record in bookingCollection.bookings)
            {
                Console.WriteLine(record);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Buchungsnummer eingeben: ");
            try
            {
                bookingId = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Bitte eine Nummer eingeben!", e);
            }

            try
            {
                Booking editedBooking = bookingCollection.bookings
                .Single(booking => booking.BookingID == bookingId);

                Console.Write(Environment.NewLine);
                Console.WriteLine("Ohne Eingabe, bleibt der bisherige Wert bestehen.");

                //Kunde
                //Kategorie
                //Auto
                //Startdatum
                //Enddatum
                Console.WriteLine("Bisheriger Kunde: " + editedBooking.CustomerID);
                Console.Write("Neuer Kunde: ");
                string v = Console.ReadLine();
                

                // save the changes to xml file
                bookingCollection.SerializeToXML(bookingCollection.bookings);


                return editedBooking;
            }
            catch (Exception e)
            {
                throw new Exception("Daten nicht gefunden. Bitte valide Kundennummer eingeben.", e);
            }
        }

        public static void deleteBooking()
        {
            var bookingCollection = BookingCollection.Instance;
            int bookingId;
            Console.Clear();
            foreach (Booking booking in bookingCollection.bookings)
            {
                Console.WriteLine(booking);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Buchungs-ID eingeben: ");
            try
            {
                bookingId = Convert.ToInt32(Console.ReadLine());
                Booking deleteBooking = bookingCollection.bookings
                .Single(booking => booking.BookingID == bookingId);
                Console.Write(Environment.NewLine);
                Console.Write("Soll die Buchung " + deleteBooking + "wirklich gelöscht werden? (j/n): ");
                switch (Console.ReadLine())
                {
                    case "j":
                        bookingCollection.bookings.Remove(deleteBooking);
                        Console.WriteLine("Buchung wurde gelöscht");
                        // save the changes to xml file
                        bookingCollection.SerializeToXML(bookingCollection.bookings);
                        break;
                    default:
                        Console.WriteLine("Löschen abgebrochen");
                        break;
                }
            }
            catch (InvalidCastException e)
            {
                throw new InvalidCastException("Bitte eine Nummer eingeben!", e);
            }
            catch (FormatException e)
            {
                throw new InvalidCastException("Bitte eine valide Buchungs-ID eingeben - keine Buchstaben!", e);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("Die angegebene Buchungs-ID wurde nicht gefunden", e);
            }
        }

        public static string showBooking()
        {
            var bookingCollection = BookingCollection.Instance;
            int bookingId;
            Console.Clear();
            foreach (Booking booking in bookingCollection.bookings)
            {
                Console.WriteLine(booking);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Buchungs-ID eingeben: ");
            try
            {
                bookingId = Convert.ToInt32(Console.ReadLine());
                // get the car by id
                Booking showBooking = bookingCollection.bookings
                    .Single(record => record.BookingID == bookingId);
                Console.Write(Environment.NewLine);
                return showBooking.showDetails();
            }
            catch (InvalidCastException e)
            {
                throw new InvalidCastException("Bitte eine Nummer eingeben!", e);
            }
            catch (FormatException e)
            {
                throw new InvalidCastException("Bitte eine valide Buchungs-ID eingeben - keine Buchstaben!", e);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("Die angegebene Buchungs-ID wurde nicht gefunden", e);
            }
        }
    }
}
