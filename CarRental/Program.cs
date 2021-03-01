using System;
using System.Threading.Tasks;
using CarRental.Models;

namespace CarRental
{
    class Program
    {
        static void Main(string[] args)
        {
            // instantiate our singleton collections of objects
            var bookingCollection = BookingCollection.Instance;
            var customerCollection = CustomerCollection.Instance;
            var carCollection = CarCollection.Instance;
            var carCategoryCollection = CarCategoryCollection.Instance;
            // load existing data from xml files
            try
            {
                customerCollection.customers = customerCollection.DeserializeFromXML();
                carCategoryCollection.carCategories = carCategoryCollection.DeserializeFromXML();
                carCollection.cars = carCollection.DeserializeFromXML();
                bookingCollection.bookings = bookingCollection.DeserializeFromXML();
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Keine Datei gefunden. Programm startet initial.");
            }
            
            // keep track if we should show the menu or leave the program
            bool showMenu = true;

            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }

        private static bool MainMenu()
        {
            // show all options and ask the user which way to go
            Console.Clear();
            Console.Title = "Autovermietung VAWi GmbH";
            Console.WriteLine("Autovermietung VAWi GmbH");
            Console.WriteLine("Bitte wählen Sie eine Option:");
            Console.WriteLine("1) Kunden verwalten");
            Console.WriteLine("2) Autokategorien verwalten");
            Console.WriteLine("3) Autos verwalten");
            Console.WriteLine("4) Buchungen verwalten");
            Console.WriteLine("5) Anwendung verlassen");
            Console.Write("Auswahl von Option Nummer: ");

            switch (Console.ReadLine())
            {
                case "1":
                    // go to the customer menu
                    bool showCustomerMenu = true;
                    while (showCustomerMenu)
                    {
                        showCustomerMenu = Menus.CustomerMenu.CustomerMenuConsole();
                    }
                    // Go back to main menu
                    return true;
                case "2":
                    // go to the car category menu
                    bool showCarCategoryMenu = true;
                    while (showCarCategoryMenu)
                    {
                        showCarCategoryMenu = Menus.CarCategoryMenu.CarCategoryMenuConsole();
                    }
                    // Go back to main menu
                    return true;
                case "3":
                    // go to the car menu
                    bool showCarMenu = true;
                    while (showCarMenu)
                    {
                        showCarMenu = Menus.CarMenu.CarMenuConsole();
                    }
                    // Go back to main menu
                    return true;
                case "4":
                    // go to the booking menu
                    bool showBookingMenu = true;
                    while (showBookingMenu)
                    {
                        showBookingMenu = Menus.BookingMenu.BookingMenuConsole();
                    }
                    // Go back to main menu
                    return true;
                case "5":
                    // leave the app
                    return false;
                default:
                    // Show a message
                    Console.WriteLine("Bitte eine valide Option eingeben.");
                    // Wait for 2 seconds, otherwise we don't see the message
                    Task.Delay(2000).Wait();
                    return true;
            }
        }
    }
}
