using System;
using System.Threading.Tasks;
using CarRental.Models;
using System.Collections.Generic;
using System.Linq;
namespace CarRental.Menus
{
    public class CarCategoryMenu
    {
        /// <summary>
        /// Shows the Car Category Menu to the user
        /// </summary>
        /// <returns>
        /// A boolean value if we should show the menu or not
        /// </returns>
        public static bool CarCategoryMenuConsole()
        {
            Console.Clear();
            Console.Title = "Autovermietung VAWi GmbH / Autokategorien verwalten";
            Console.WriteLine("Autokategorien verwalten");
            Console.WriteLine("Bitte wählen Sie eine Option:");
            Console.WriteLine("1) Autokategorie anlegen");
            Console.WriteLine("2) Autokategorie bearbeiten");
            Console.WriteLine("3) Autokategorie löschen");
            Console.WriteLine("4) zum Hauptmenü");
            Console.Write("Auswahl von Option Nummer: ");
            switch (Console.ReadLine())
            {
                case "1":
                    var carCategory = createCarCategory();
                    Console.WriteLine("Kategorie wurde angelegt");
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "2":
                    var editedCarCategory = editCarCategory();
                    if (editedCarCategory != null)
                    {
                        Console.WriteLine("Kategorie wurde geändert.");
                    }
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "3":
                    deleteCarCategory();
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "4":
                    return false;
                default:
                    // Show a message
                    Console.WriteLine("Bitte eine valide Option eingeben.");
                    // Wait for 2 seconds, otherwise we don't see the message
                    Task.Delay(2000).Wait();
                    return true;
            }
        }

        /// <summary>
        /// Method for creating a new car category.
        /// </summary>
        /// <returns>
        /// The object of the newly created car category
        /// </returns>
        public static CarCategory createCarCategory()
        {
            var carCategoryCollection = CarCategoryCollection.Instance;
            CarCategory newCarCategory;
            // if we have carCategories get a new ID - highest value of all ids
            if (carCategoryCollection.carCategories.Count > 0)
            {
                var carCategoryNewId = carCategoryCollection.CurrentId;
                // increment id
                ++carCategoryNewId;
                newCarCategory = new CarCategory(carCategoryNewId);
            }
            else
            {
                // no customers yet - create a new one without external id
                newCarCategory = new CarCategory();
            }
            Console.Clear();
            Console.Title = "Autovermietung VAWi GmbH / Autokategorie anlegen";
            Console.WriteLine("Autokategorie anlegen");
            Console.Write("Bezeichnung:");
            newCarCategory.Name = Console.ReadLine();
            Console.Write("Tagesgebühr:");
            var v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                int fee;
                while (!Int32.TryParse(v, out fee))
                {
                    Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                    v = Console.ReadLine();
                }
                newCarCategory.RentalFeeDay = fee;
            }
            // Add the category and save it
            carCategoryCollection.carCategories.Add(newCarCategory);
            carCategoryCollection.SerializeToXML(carCategoryCollection.carCategories);
            return newCarCategory;
        }

        /// <summary>
        /// Method for editing existing Car Categories
        /// </summary>
        /// <returns>
        /// The edited car category or null, if the category was not found.
        /// </returns>
        public static CarCategory editCarCategory()
        {
            var carCategoryCollection = CarCategoryCollection.Instance;
            // initialize carCategoryId
            int carCategoryId = 0;
            Console.Clear();
            foreach (CarCategory cc in carCategoryCollection.carCategories)
            {
                Console.WriteLine(cc);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Autokategorie ID eingeben: ");
            var v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                int cc;
                while (!Int32.TryParse(v, out cc))
                {
                    Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                    v = Console.ReadLine();
                }
                carCategoryId = cc;
            }

            CarCategory editedCarCategory = carCategoryCollection.getCarCategoryById(carCategoryId);
            if (editedCarCategory == null)
            {
                Console.WriteLine("Keine Autokategorie mit dieser ID gefunden.");
                Task.Delay(2000).Wait();
                return null;
            }
            Console.Write(Environment.NewLine);
            Console.WriteLine("Ohne Eingabe, bleibt der bisherige Wert bestehen.");
            Console.WriteLine("Bisherige Bezeichnung: " + editedCarCategory.Name);
            Console.Write("Neue Bezeichnung: ");
            v = Console.ReadLine();
            editedCarCategory.Name = !string.IsNullOrEmpty(v) ? v : editedCarCategory.Name;
            Console.WriteLine("Bisherige Tagesgebühr: " + editedCarCategory.RentalFeeDay);
            Console.Write("Neue Tagesgebühr: ");
            v = Console.ReadLine();
            Decimal fee;
            if (!string.IsNullOrEmpty(v)) {
                while (!Decimal.TryParse(v, out fee))
                {
                    Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                    v = Console.ReadLine();
                }
                editedCarCategory.RentalFeeDay = fee > 0 ? fee : editedCarCategory.RentalFeeDay;
            }
            // save the changes to xml file
            carCategoryCollection.SerializeToXML(carCategoryCollection.carCategories);


            return editedCarCategory;

        }

        /// <summary>
        /// Method for deleting existing Car Categories.
        /// </summary>
        public static void deleteCarCategory()
        {
            var carCategoryCollection = CarCategoryCollection.Instance;
            int carCategoryId = 0;
            Console.Clear();
            foreach (CarCategory cc in carCategoryCollection.carCategories)
            {
                Console.WriteLine(cc);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Autokategorie ID eingeben: ");
            var v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                int cc;
                while (!Int32.TryParse(v, out cc))
                {
                    Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                    v = Console.ReadLine();
                }
                carCategoryId = cc;
            }
            CarCategory deleteCarCategory = carCategoryCollection.getCarCategoryById(carCategoryId);
            if (deleteCarCategory == null)
            {
                Console.WriteLine("Keine Autokategorie mit dieser ID gefunden.");
                Task.Delay(2000).Wait();
                return;
            }
            Console.Write(Environment.NewLine);
            Console.WriteLine("Hinweis: Abhängige Daten wie z.B. Buchungen werden nicht verändert oder gelöscht.");
            Console.Write("Soll die Autokategorie " + deleteCarCategory + " wirklich gelöscht werden? (j/n): ");
            switch (Console.ReadLine())
            {
                case "j":
                    carCategoryCollection.carCategories.Remove(deleteCarCategory);
                    Console.WriteLine("Autokategorie wurde gelöscht");
                    // save the changes to xml file
                    carCategoryCollection.SerializeToXML(carCategoryCollection.carCategories);
                    break;
                default:
                    Console.WriteLine("Löschen abgebrochen");
                    break;
            }
        }
    }
}
