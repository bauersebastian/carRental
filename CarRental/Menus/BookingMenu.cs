using System;
using System.Threading.Tasks;
using CarRental.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Menus
{
    public class BookingMenu
    {
        /// <summary>
        /// Displays the Booking Menu to the user
        /// </summary>
        /// <returns>
        /// A boolean value in order to leave or stay in menu
        /// </returns>
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
                    var newBooking = createBooking();
                    if (newBooking != null)
                    {
                        Console.WriteLine("Buchung wurde angelegt");
                    }
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    // Wait for 2 seconds, otherwise we don't see the message
                    Task.Delay(2000).Wait();
                    return false;
                case "2":
                    var editedBooking = editBooking();
                    if (editedBooking != null)
                    {
                        Console.WriteLine("Buchung wurde geändert.");
                    }
                    else
                    {
                        Console.WriteLine("Keine Buchung zum Kunden gefunden oder fehlerhafte Eingabe.");
                    }
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
                    Task.Delay(2000).Wait();
                    return true;
            }

        }

        /// <summary>
        /// Creates a new booking
        /// </summary>
        /// <returns>
        /// A newly created booking
        /// </returns>
        public static Booking createBooking()
        {
            // get all necessary collections first
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
            var v = Console.ReadLine();
            // check if we can find a customer with that id
            Customer bookingCustomer = CustomerMenu.getCustomerByIdDialog(v);
            if (bookingCustomer == null)
            {
                Console.WriteLine("Kein Kunde zur ID gefunden. Abbruch.");
                Task.Delay(2000).Wait();
                return null;
            } else
            {
                newBooking.CustomerID = bookingCustomer.CustomerID;
            }
            Console.WriteLine("Kategorie des Autos - bitte wählen:");
            foreach (CarCategory carCategory in carCategoryCollection.carCategories)
            {
                Console.WriteLine(carCategory);
            }
            v = Console.ReadLine();

            // check if an int was entered and we can find that category
            var cc = Helper.checkInt(v);
            CarCategory carCategorySelect = carCategoryCollection.getCarCategoryById(cc);
            if (carCategorySelect == null)
            {
                Console.WriteLine("Keine Kategorie zur ID gefunden. Abbruch.");
                Task.Delay(2000).Wait();
                return null;
            } else
            {
                newBooking.CarCategoryID = cc;
            }
            Console.Write("Bitte Startdatum der Buchung im Format dd-MM-yyyy eingeben:");
            v = Console.ReadLine();
            // make sure we got the right input format for a date
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
            if (newBooking.EndDate < newBooking.StartDate)
            {
                Console.WriteLine("Das Endedatum kann nicht vor dem Startdatum liegen. Abbruch.");
                Task.Delay(2000).Wait();
                return null;
            }

            // now check if a car is availble in the given time frame
            Console.WriteLine("Verfügbare Autos zum Zeitraum in der gewählten Kategorie werden ermittelt...");
            Console.WriteLine("Verfügbare Autos aus der gewählten Kategorie - bitte auswählen:");
            var possibleCars = BookingCollection.getAvailableCars(newBooking.StartDate, newBooking.EndDate, newBooking.CarCategoryID);
            if (possibleCars.Count == 0)
            {
                Console.WriteLine("Keine Autos zur Kategorie und zum gewählten Zeitraum gefunden.");
                Task.Delay(2000).Wait();
                return null;
            }
            foreach (Car car in possibleCars)
            {
                Console.WriteLine(car);
            }
            v = Console.ReadLine();
            var ci = Helper.checkInt(v);
            Car selectedCar = carCollection.getCarById(ci);
            if (selectedCar == null)
            {
                Console.WriteLine("Kein Auto zur ID gefunden. Abbruch.");
                Task.Delay(2000).Wait();
                return null;
            } else
            {
                newBooking.CarID = ci;
            }

            // add booking, save and leave
            bookingCollection.bookings.Add(newBooking);
            bookingCollection.SerializeToXML(bookingCollection.bookings);
            return newBooking;
        }

        /// <summary>
        /// Method for editing existing bookings
        /// </summary>
        /// <returns>
        /// The edited booking or null.
        /// </returns>
        public static Booking editBooking()
        {
            var bookingCollection = BookingCollection.Instance;
            var customerCollection = CustomerCollection.Instance;
            var carCategoryCollection = CarCategoryCollection.Instance;
            var carCollection = CarCollection.Instance;
            // get all bookings of a given customer
            Booking editedBooking = getBookingByCustomerDialog();
            if (editedBooking == null)
            {
                return null;
            }

            Console.Write(Environment.NewLine);
            Console.WriteLine("Ohne Eingabe, bleibt der bisherige Wert bestehen.");

            // Change customer
            Console.WriteLine("Bisheriger Kunde: " + editedBooking.CustomerID);
            Console.WriteLine("Neuer Kunde - bitte auswählen: ");
            foreach (Customer record in customerCollection.customers)
            {
                Console.WriteLine(record);
            }
            Console.WriteLine("Kundennummer eingeben:");
            var v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                int customerIdEntered = Helper.checkInt(v);
                Customer selectedCustomer = customerCollection.getCustomerById(customerIdEntered);
                if (selectedCustomer == null)
                {
                    Console.WriteLine("Kein Kunde zur ID gefunden. Abbruch.");
                    Task.Delay(2000).Wait();
                    return null;
                } else
                {
                    editedBooking.CustomerID = customerIdEntered;
                }
                
            } else
            {
                // leave the id as it is
                editedBooking.CustomerID = editedBooking.CustomerID;
            }
            // change category
            Console.WriteLine("Bisheriger Kategorie: " + editedBooking.CarCategoryID);
            Console.WriteLine("Neue Kategorie - bitte auswählen: ");
            foreach (CarCategory carCategory in carCategoryCollection.carCategories)
            {
                Console.WriteLine(carCategory);
            }
            v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                int carCategoryIdEntered = Helper.checkInt(v);
                CarCategory selectedCarCategory = carCategoryCollection.getCarCategoryById(carCategoryIdEntered);
                if (selectedCarCategory == null)
                {
                    Console.WriteLine("Kein Kunde zur ID gefunden. Abbruch.");
                    Task.Delay(2000).Wait();
                    return null;
                } else
                {
                    editedBooking.CarCategoryID = carCategoryIdEntered;
                }
            }
            else
            {
                // leave the id as it is
                editedBooking.CarCategoryID = editedBooking.CarCategoryID;
            }

            // Change start date
            Console.WriteLine("Bisheriges Startdatum " + editedBooking.StartDate.ToString("dd.MM.yyyy"));
            Console.Write("Bitte Startdatum der Buchung im Format dd-MM-yyyy eingeben:");
            v = Console.ReadLine();
            DateTime dt;
            while (!DateTime.TryParseExact(v, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Datums Eingabe bitte im genannten Format vornehmen. Musseingabe bei Änderung von Buchungen.");
                v = Console.ReadLine();
            }
            editedBooking.StartDate = dt;

            // Change end date
            Console.WriteLine("Bisheriges Enddatum " + editedBooking.EndDate.ToString("dd.MM.yyyy"));
            Console.Write("Bitte Endedatum im Format dd-MM-yyyy eingeben:");
            v = Console.ReadLine();
            while (!DateTime.TryParseExact(v, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Datums Eingabe bitte im genannten Format vornehmen. Musseingabe bei Änderung von Buchungen.");
                v = Console.ReadLine();
            }
            editedBooking.EndDate = dt;
            if (editedBooking.EndDate < editedBooking.StartDate)
            {
                Console.WriteLine("Das Endedatum kann nicht vor dem Startdatum liegen. Abbruch.");
                Task.Delay(2000).Wait();
                return null;
            }

            // Change car
            // now check if a car is availble in the given time frame
            Console.WriteLine("Verfügbare Autos zum Zeitraum in der gewählten Kategorie werden ermittelt...");

            Console.WriteLine("Verfügbare Autos aus der gewählten Kategorie - bitte auswählen:");
            var possibleCars = BookingCollection.getAvailableCars(editedBooking.StartDate, editedBooking.EndDate, editedBooking.CarCategoryID);
            if (possibleCars.Count == 0)
            {
                Console.WriteLine("Keine Autos zur Kategorie und zum gewählten Zeitraum gefunden.");
                Task.Delay(2000).Wait();
                return null;
            }
            foreach (Car car in possibleCars)
            {
                Console.WriteLine(car);
            }
            v = Console.ReadLine();
            var ci = Helper.checkInt(v);
            Car selectedCar = carCollection.getCarById(ci);
            if (selectedCar == null)
            {
                Console.WriteLine("Kein Auto zur ID gefunden. Abbruch.");
                Task.Delay(2000).Wait();
                return null;
            }
            else
            {
                editedBooking.CarID = ci;
            }

            // save the changes to xml file
            bookingCollection.SerializeToXML(bookingCollection.bookings);

            return editedBooking;
            
           
        }

        /// <summary>
        /// Method for deleting existing bookings.
        /// </summary>
        public static void deleteBooking()
        {
            var bookingCollection = BookingCollection.Instance;
            Console.Clear();
            // get bookings by customer
            Booking deleteBooking = getBookingByCustomerDialog();
            if (deleteBooking == null)
            {
                Console.WriteLine("Keine Buchung gefunden. Abbruch.");
                Task.Delay(2000).Wait();
                return;
            }
            // we found a booking - really delete it?
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
                    Task.Delay(2000).Wait();
                    break;
            }
           
        }

        /// <summary>
        /// Method for showing all the details of a booking
        /// </summary>
        /// <returns>
        /// String with the details of the selected booking
        /// </returns>
        public static string showBooking()
        {
            var bookingCollection = BookingCollection.Instance;
            Console.Clear();
            // are there any bookings yet?
            if (!bookingCollection.bookingExists())
            {
                return "Keine Buchungen vorhanden";
            }
            // get booking by customer
            Booking showBooking = getBookingByCustomerDialog();
            Console.Write(Environment.NewLine);
            if (showBooking != null)
            {
                return showBooking.showDetails();
            } else
            {
                return "Keine Buchung zur ID gefunden. Bitte Eingabe prüfen.";
            }
        }

        // Helper functions

        /// <summary>
        /// Chooses a booking by selecting the customer first.
        /// </summary>
        /// <returns>
        /// The booking we want to edit.
        /// </returns>
        public static Booking getBookingByCustomerDialog()
        {
            var customerCollection = CustomerCollection.Instance;
            var bookingCollection = BookingCollection.Instance;
            int customerId = 0;
            int bookingId = 0;
            // do we actually have customers yet?
            if (customerCollection.customers.Count == 0)
            {
                Console.WriteLine("noch keine Kunden angelegt bisher.");
                Task.Delay(2000).Wait();
                return null;
            }
            Console.WriteLine("Für welchen Kunden soll eine Buchung ausgewählt werden?");
            foreach (Customer record in customerCollection.customers)
            {
                Console.WriteLine(record);
            }
            // Read the bookings for a certain customer
            Console.WriteLine("Kundennummer eingeben:");
            var v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                int ci;
                while (!Int32.TryParse(v, out ci))
                {
                    Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                    v = Console.ReadLine();
                }
                customerId = ci;
            }
            var possibleBookings = bookingCollection.getBookingByCustomer(customerId);
            if (possibleBookings.Count == 0)
            {
                return null;
            }
            Console.WriteLine("Bitte eine der folgenden Buchungen wählen:");
            foreach (Booking booking in possibleBookings)
            {
                Console.WriteLine(booking);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Buchungsnummer eingeben: ");
            v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                int bi;
                while (!Int32.TryParse(v, out bi))
                {
                    Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                    v = Console.ReadLine();
                }
                bookingId = bi;
            }

            Booking editedBooking = possibleBookings
                .SingleOrDefault(booking => booking.BookingID == bookingId);
            // check if the booking exists or return
            if (editedBooking == null)
            {
                return null;
            } else
            {
                return editedBooking;
            }
        }

    }
}
