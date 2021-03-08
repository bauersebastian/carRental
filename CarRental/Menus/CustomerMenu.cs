using System;
using System.Threading.Tasks;
using CarRental.Models;
using System.Collections.Generic;
using System.Linq;
// NuGet Package for IBAN validation
using IbanNet;

namespace CarRental.Menus
{
    public class CustomerMenu
    {
        /// <summary>
        /// Displays the Customer Menu to the User
        /// </summary>
        /// <returns>
        /// A boolean value in order to stay or leave the menu.
        /// </returns>
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
                        Console.WriteLine("Keinen Kunden zur Eingabe gefunden.");
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
            string iban = checkIBAN(Console.ReadLine());            
            newCustomer.IBAN = iban;
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
            // if there are no customers we leave the method
            if (customerCollection.customers.Count == 0)
            {
                return null;
            }
            Console.Clear();
            Console.WriteLine("Bitte einen existierenden Kunden zur Bearbeitung auswählen:");
            foreach (Customer c in customerCollection.customers)
            {
                Console.WriteLine(c);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Kundennummer eingeben: ");
            var v = Console.ReadLine();
            Customer editedCustomer = getCustomerByIdDialog(v);
            if (editedCustomer == null)
            {
                return null;
            }
            // we have a editable customer now - go on
            Console.Write(Environment.NewLine);
            Console.WriteLine("Ohne Eingabe, bleibt der bisherige Wert bestehen.");
            Console.WriteLine("Bisheriger Vorname: " + editedCustomer.FirstName);
            Console.Write("Neuer Vorname: ");
            v = Console.ReadLine();
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
            // only accept valid IBAN
            editedCustomer.IBAN = !string.IsNullOrEmpty(v) ? checkIBAN(v) : editedCustomer.IBAN;

            // save the changes to xml file
            customerCollection.SerializeToXML(customerCollection.customers);

            return editedCustomer;
            
        }

        public static void deleteCustomer()
        {
            var customerCollection = CustomerCollection.Instance;
            // if there are no customers we leave the method
            if (customerCollection.customers.Count == 0)
            {
                Console.WriteLine("Keine Kunden vorhanden");
                Task.Delay(2000).Wait();
                return;
            }
            Console.Clear();
            Console.WriteLine("Bitte einen existierenden Kunden zur Löschung auswählen:");
            foreach (Customer c in customerCollection.customers)
            {
                Console.WriteLine(c);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Kundennummer eingeben: ");
            var v = Console.ReadLine();
            Customer deleteCustomer = getCustomerByIdDialog(v);
            if (deleteCustomer == null)
            {
                Console.WriteLine("Kein Kunde mit dieser ID vorhanden");
                Task.Delay(2000).Wait();
                return;
            }
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

        public static string showCustomer()
        {
            var customerCollection = CustomerCollection.Instance;
            // if there are no customers we leave the method
            if (customerCollection.customers.Count == 0)
            {
                return "Keine Kunden vorhanden";
            }
            Console.Clear();
            foreach (Customer c in customerCollection.customers)
            {
                Console.WriteLine(c);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Kundennummer eingeben zur Ausgabe der Kundendetails: ");
            var v = Console.ReadLine();
            Customer showCustomer = getCustomerByIdDialog(v);
            if (showCustomer == null)
            {
                return "Kein Kunde zu eingegebener ID gefunden.";
            }
            Console.Write(Environment.NewLine);
            return showCustomer.customerDetails();
        }


        // Helper functions

        /// <summary>
        /// Checks if a entered IBAN is valid.
        /// </summary>
        /// <param name="iban"></param>
        /// <returns>A valid IBAN.</returns>
        public static string checkIBAN(string iban)
        {
            IIbanValidator ibanChecker = new IbanValidator();
            ValidationResult validationResult = ibanChecker.Validate(iban);
            while (!validationResult.IsValid)
            {
                Console.WriteLine("Bitte eine gültige IBAN eingeben!");
                iban = Console.ReadLine();
                validationResult = ibanChecker.Validate(iban);
            }
            return iban;
        }

        /// <summary>
        /// Gets a customer by ID and checks the input of the dialog
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// A selected customer or null.
        /// </returns>
        public static Customer getCustomerByIdDialog(string id)
        {
            var customerCollection = CustomerCollection.Instance;
            int customerId = 0;
            // make sure we got a number as input
            if (!string.IsNullOrEmpty(id))
            {
                int ci;
                while (!Int32.TryParse(id, out ci))
                {
                    Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                    id = Console.ReadLine();
                }
                customerId = ci;
            }
            // make sure we get a valid customer with the given id
            Customer editedCustomer = customerCollection.getCustomerById(customerId);
            if (editedCustomer == null)
            {
                return null;
            } else
            {
                return editedCustomer;
            }
        }
    }
}
