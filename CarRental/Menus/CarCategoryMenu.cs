using System;
using System.Threading.Tasks;
using CarRental.Models;
using System.Collections.Generic;
using System.Linq;
namespace CarRental.Menus
{
    public class CarCategoryMenu
    {
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
                    Console.WriteLine("Kategorie wurde geändert.");
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
            newCarCategory.RentalFeeDay = Convert.ToInt32(Console.ReadLine());
            carCategoryCollection.carCategories.Add(newCarCategory);
            carCategoryCollection.SerializeToXML(carCategoryCollection.carCategories);
            return newCarCategory;
        }

        public static CarCategory editCarCategory()
        {
            var carCategoryCollection = CarCategoryCollection.Instance;
            int carCategoryId;
            Console.Clear();
            foreach (CarCategory cc in carCategoryCollection.carCategories)
            {
                Console.WriteLine(cc);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Autokategorie ID eingeben: ");
            try
            {
                carCategoryId = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Bitte eine Nummer eingeben!", e);
            }

            try
            {
                CarCategory editedCarCategory = carCategoryCollection.carCategories
                .Single(carCategory => carCategory.CarCategoryID == carCategoryId);

                Console.Write(Environment.NewLine);
                Console.WriteLine("Ohne Eingabe, bleibt der bisherige Wert bestehen.");
                Console.WriteLine("Bisherige Bezeichnung: " + editedCarCategory.Name);
                Console.Write("Neue Bezeichnung: ");
                string v = Console.ReadLine();
                editedCarCategory.Name = !string.IsNullOrEmpty(v) ? v : editedCarCategory.Name;
                Console.WriteLine("Bisherige Tagesgebühr: " + editedCarCategory.RentalFeeDay);
                Console.Write("Neue Tagesgebühr: ");
                v = Console.ReadLine();
                if (!string.IsNullOrEmpty(v)) {
                    int vdec = Convert.ToInt32(v);
                    editedCarCategory.RentalFeeDay = vdec > 0 ? vdec : editedCarCategory.RentalFeeDay;
                }

                // save the changes to xml file
                carCategoryCollection.SerializeToXML(carCategoryCollection.carCategories);


                return editedCarCategory;
            }
            catch (Exception e)
            {

                throw new Exception("Daten nicht gefunden. Bitte valide Autokategorie ID eingeben.", e);
            }


        }

        public static void deleteCarCategory()
        {
            var carCategoryCollection = CarCategoryCollection.Instance;
            int carCategoryId;
            Console.Clear();
            foreach (CarCategory cc in carCategoryCollection.carCategories)
            {
                Console.WriteLine(cc);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Autokategorie ID eingeben: ");
            try
            {
                carCategoryId = Convert.ToInt32(Console.ReadLine());
                CarCategory deleteCarCategory = carCategoryCollection.carCategories
                .First(carCategory => carCategory.CarCategoryID == carCategoryId);
                Console.Write(Environment.NewLine);
                Console.Write("Soll die Autokategorie " + deleteCarCategory + "wirklich gelöscht werden? (j/n): ");
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
            catch (InvalidCastException e)
            {
                throw new InvalidCastException("Bitte eine Nummer eingeben!", e);
            }
            catch (FormatException e)
            {
                throw new InvalidCastException("Bitte eine valide Autokategorie ID eingeben - keine Buchstaben!", e);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("Die angegebene Autokategorie ID wurde nicht gefunden", e);
            }

        }
    }
}
