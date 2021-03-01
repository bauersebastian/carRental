using System;
using System.Threading.Tasks;
using CarRental.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Menus
{
    public class CustomerMenu
    {
        public static bool CustomerMenuConsole()
        {
            Console.Clear();
            Console.Title = "Autovermietung VAWi GmbH / Kunden verwalten";
            Console.WriteLine("Kunden verwalten");
            Console.WriteLine("Bitte wählen Sie eine Option:");
            Console.WriteLine("1) Kunden anlegen");
            Console.WriteLine("2) Kunden bearbeiten");
            Console.WriteLine("3) Kunden löschen");
            Console.WriteLine("4) Kunden anzeigen / ausgeben");
            Console.WriteLine("5) zum Hauptmenü");
            Console.Write("Auswahl von Option Nummer: ");
            switch (Console.ReadLine())
            {
                case "1":
                    var customer = createCustomer();
                    Console.WriteLine("Kunde wurde angelegt");
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "2":
                    var editedcustomer = editCustomer();
                    if (editedcustomer != null)
                    {
                        Console.WriteLine("Kunde wurde geändert.");
                    } else
                    {
                        Console.WriteLine("Noch keine Kunden angelegt.");
                    }
                    
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "3":
                    deleteCustomer();
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "4":
                    Console.WriteLine(showCustomer());
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

        public static Customer createCustomer()
        {
            var customerCollection = CustomerCollection.Instance;
            Customer newCustomer;
            // if we have customers get a new ID - highest value of all ids
            if (customerCollection.customers.Count > 0)
            {
                var customerNewId = customerCollection.CurrentId;
                // increment id
                ++customerNewId;
                newCustomer = new Customer(customerNewId);
            }
            else
            {
                // no customers yet - create a new one without external id
                newCustomer = new Customer();
            }
            Console.Clear();
            Console.Title = "Autovermietung VAWi GmbH / Kunden anlegen";
            Console.WriteLine("Kunde anlegen");
            Console.Write("Vorname:");
            newCustomer.FirstName = Console.ReadLine();
            Console.Write("Nachname:");
            newCustomer.LastName = Console.ReadLine();
            Console.Write("Straße:");
            newCustomer.Street = Console.ReadLine();
            Console.Write("Postleitzahl:");
            newCustomer.Postcode = Console.ReadLine();
            Console.Write("Ort:");
            newCustomer.City = Console.ReadLine();
            Console.Write("IBAN:");
            newCustomer.IBAN = Console.ReadLine();
            Console.Write("Bitte Geburtsdatum im Format dd-MM-yyyy eingeben:");
            string v = Console.ReadLine();
            DateTime dt;
            while (!DateTime.TryParseExact(v, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Datums Eingabe bitte im genannten Format vornehmen.");
                v = Console.ReadLine();
            }
            newCustomer.Birthday = dt;
            customerCollection.customers.Add(newCustomer);
            customerCollection.SerializeToXML(customerCollection.customers);
            return newCustomer;
        }

        public static Customer editCustomer()
        {
            var customerCollection = CustomerCollection.Instance;
            int customerId;
            // if there are no customers we leave the method
            if (customerCollection.customers == null)
            {
                return null;
            }
            Console.Clear();
            foreach (Customer c in customerCollection.customers)
            {
                Console.WriteLine(c);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Kundennummer eingeben: ");
            try
            {
                customerId = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Bitte eine Nummer eingeben!", e);
            }

            try
            {
                Customer editedCustomer = customerCollection.customers
                .Single(customer => customer.CustomerID == customerId);

                Console.Write(Environment.NewLine);
                Console.WriteLine("Ohne Eingabe, bleibt der bisherige Wert bestehen.");
                Console.WriteLine("Bisheriger Vorname: " + editedCustomer.FirstName);
                Console.Write("Neuer Vorname: ");
                string v = Console.ReadLine();
                editedCustomer.FirstName = !string.IsNullOrEmpty(v) ? v : editedCustomer.FirstName;
                Console.WriteLine("Bisheriger Nachname: " + editedCustomer.LastName);
                Console.Write("Neuer Nachname: ");
                v = Console.ReadLine();
                editedCustomer.LastName = !string.IsNullOrEmpty(v) ? v : editedCustomer.LastName;
                Console.WriteLine("Bisherige Straße: " + editedCustomer.Street);
                Console.Write("Neue Straße: ");
                v = Console.ReadLine();
                editedCustomer.Street = !string.IsNullOrEmpty(v) ? v : editedCustomer.Street;
                Console.WriteLine("Bisherige Postleitzahl: " + editedCustomer.Postcode);
                Console.Write("Neue PLZ: ");
                v = Console.ReadLine();
                editedCustomer.Postcode = !string.IsNullOrEmpty(v) ? v : editedCustomer.Postcode;
                Console.WriteLine("Bisheriger Ort: " + editedCustomer.City);
                Console.Write("Neuer Ort: ");
                v = Console.ReadLine();
                editedCustomer.City = !string.IsNullOrEmpty(v) ? v : editedCustomer.City;
                Console.WriteLine("Bisherige IBAN: " + editedCustomer.IBAN);
                Console.Write("Neue IBAN: ");
                v = Console.ReadLine();
                editedCustomer.IBAN = !string.IsNullOrEmpty(v) ? v : editedCustomer.IBAN;

                // save the changes to xml file
                customerCollection.SerializeToXML(customerCollection.customers);


                return editedCustomer;
            }
            catch (Exception e)
            {
                throw new Exception("Daten nicht gefunden. Bitte valide Kundennummer eingeben.", e);
            }

            
        }

        public static void deleteCustomer()
        {
            var customerCollection = CustomerCollection.Instance;
            int customerId;
            // if there are no customers we leave the method
            if (customerCollection.customers == null)
            {
                Console.WriteLine("Keine Kunden vorhanden");
                Task.Delay(2000).Wait();
                return;
            }
            Console.Clear();
            foreach (Customer c in customerCollection.customers)
            {
                Console.WriteLine(c);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Kundennummer eingeben: ");
            try
            {
                customerId = Convert.ToInt32(Console.ReadLine());
                Customer deleteCustomer = customerCollection.customers
                .First(customer => customer.CustomerID == customerId);
                Console.Write(Environment.NewLine);
                Console.Write("Soll der Kunde " + deleteCustomer + "wirklich gelöscht werden? (j/n): ");
                switch (Console.ReadLine())
                {
                    case "j":
                        customerCollection.customers.Remove(deleteCustomer);
                        Console.WriteLine("Kunde wurde gelöscht");
                        // save the changes to xml file
                        customerCollection.SerializeToXML(customerCollection.customers);
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
                throw new InvalidCastException("Bitte eine valide Kundennummer eingeben - keine Buchstaben!", e);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("Die angegebene Kundennummer wurde nicht gefunden", e);
            }
        }

        public static string showCustomer()
        {
            var customerCollection = CustomerCollection.Instance;
            int customerId;
            // if there are no customers we leave the method
            if (customerCollection.customers == null)
            {
                Console.WriteLine("Keine Kunden vorhanden");
                Task.Delay(2000).Wait();
                return "keine Kunden";
            }
            Console.Clear();
            foreach (Customer c in customerCollection.customers)
            {
                Console.WriteLine(c);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Kundennummer eingeben zur Ausgabe der Kundendetails: ");
            try
            {
                customerId = Convert.ToInt32(Console.ReadLine());
                Customer showCustomer = customerCollection.customers
                .First(customer => customer.CustomerID == customerId);
                Console.Write(Environment.NewLine);
                return showCustomer.customerDetails();
            }
            catch (InvalidCastException e)
            {
                throw new InvalidCastException("Bitte eine Nummer eingeben!", e);
            }
            catch (FormatException e)
            {
                throw new InvalidCastException("Bitte eine valide Kundennummer eingeben - keine Buchstaben!", e);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("Die angegebene Kundennummer wurde nicht gefunden", e);
            }
        }
    }
}
