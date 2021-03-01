using System;
using System.Threading.Tasks;
using CarRental.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Menus
{
    public class CarMenu
    {
        public static bool CarMenuConsole()
        {
            Console.Clear();
            Console.Title = "Autovermietung VAWi GmbH / Autos verwalten";
            Console.WriteLine("Autos verwalten");
            Console.WriteLine("Bitte wählen Sie eine Option:");
            Console.WriteLine("1) Auto anlegen");
            Console.WriteLine("2) Auto bearbeiten");
            Console.WriteLine("3) Auto löschen");
            Console.WriteLine("4) Auto / Autokategorie anzeigen");
            Console.WriteLine("5) zum Hauptmenü");
            Console.Write("Auswahl von Option Nummer: ");
            switch (Console.ReadLine())
            {
                case "1":
                    var car = createCar();
                    Console.WriteLine("Auto wurde angelegt");
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "2":
                    var editedCar = editCar();
                    Console.WriteLine("Auto wurde geändert.");
                    Task.Delay(2000).Wait();
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    return false;
                case "3":
                    deleteCar();
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "4":
                    Console.WriteLine(showCar());
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

        public static Car createCar()
        {
            var carCollection = CarCollection.Instance;
            var carCategoryCollection = CarCategoryCollection.Instance;
            Car newCar;
            // if we have cars get a new ID - highest value of all ids
            if (carCollection.cars.Count > 0)
            {
                var carNewId = carCollection.CurrentId;
                // increment id
                ++carNewId;
                newCar = new Car(carNewId);
            }
            else
            {
                // no cars yet - create a new one without external id
                newCar = new Car();
            }
            Console.Clear();
            Console.Title = "Autovermietung VAWi GmbH / Auto anlegen";
            Console.WriteLine("Auto anlegen");
            Console.Write("Hersteller:");
            newCar.Brand = Console.ReadLine();
            Console.Write("Modell:");
            newCar.Model = Console.ReadLine();
            Console.WriteLine("Kategorie des Autos - bitte wählen:");
            foreach (CarCategory carCategory in carCategoryCollection.carCategories)
            {
                Console.WriteLine(carCategory);
            }
            newCar.CarCategoryID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Leistung in PS:");
            newCar.HorsePower = Convert.ToInt32(Console.ReadLine());
            Console.Write("Kofferraumvolumen:");
            newCar.LuggageCompartment = Convert.ToInt32(Console.ReadLine());
            Console.Write("Anzahl der Türen:");
            newCar.Doors = Convert.ToInt32(Console.ReadLine());
            Console.Write("Automatik oder manuelle Gangschaltung (a/m):");
            newCar.Transmission = Console.ReadLine();
            carCollection.cars.Add(newCar);
            carCollection.SerializeToXML(carCollection.cars);
            return newCar;
        }

        public static Car editCar()
        {
            var carCollection = CarCollection.Instance;
            var carCategoryCollection = CarCategoryCollection.Instance;
            int carId;
            Console.Clear();
            foreach (Car car in carCollection.cars)
            {
                Console.WriteLine(car);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Auto ID eingeben: ");
            try
            {
                carId = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Bitte eine Nummer eingeben!", e);
            }

            try
            {
                Car editedCar = carCollection.cars
                .Single(car => car.CarID == carId);

                Console.Write(Environment.NewLine);
                Console.WriteLine("Ohne Eingabe, bleibt der bisherige Wert bestehen.");
                Console.WriteLine("Bisherige Modellbezeichnung: " + editedCar.Model);
                Console.Write("Neue Bezeichnung: ");
                string v = Console.ReadLine();
                editedCar.Model = !string.IsNullOrEmpty(v) ? v : editedCar.Model;
                Console.WriteLine("Bisherige Marke: " + editedCar.Brand);
                Console.Write("Neue Marke: ");
                v = Console.ReadLine();
                editedCar.Brand = !string.IsNullOrEmpty(v) ? v : editedCar.Brand;
                Console.WriteLine("Bisherige Autokategorie: " + editedCar.CarCategoryID);
                Console.WriteLine("Neue Autokategorie wählen: ");
                foreach (CarCategory carCategory in carCategoryCollection.carCategories)
                {
                    Console.WriteLine(carCategory);
                }
                v = Console.ReadLine();
                if (!string.IsNullOrEmpty(v))
                {
                    int cc;
                    while (!Int32.TryParse(v, out cc))
                    {
                        Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                        v = Console.ReadLine();
                    }
                    editedCar.CarCategoryID = cc;
                }
                Console.WriteLine("Bisherige PS: " + editedCar.HorsePower);
                Console.Write("Neue PS: ");
                v = Console.ReadLine();
                if (!string.IsNullOrEmpty(v))
                {
                    int hp;
                    while (!Int32.TryParse(v, out hp))
                    {
                        Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                        v = Console.ReadLine();
                    }
                    editedCar.HorsePower = hp;
                }
                Console.WriteLine("Bisheriges Kofferraumvolumen: " + editedCar.LuggageCompartment);
                Console.Write("Neues Kofferraumvolumen: ");
                v = Console.ReadLine();
                if (!string.IsNullOrEmpty(v))
                {
                    int lc;
                    while (!Int32.TryParse(v, out lc))
                    {
                        Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                        v = Console.ReadLine();
                    }
                    editedCar.LuggageCompartment = lc;
                }
                Console.WriteLine("Bisherige Anzahl an Türen: " + editedCar.Doors);
                Console.Write("Neue Anzahl an Türen: ");
                v = Console.ReadLine();
                if (!string.IsNullOrEmpty(v))
                {
                    int doors;
                    while (!Int32.TryParse(v, out doors))
                    {
                        Console.WriteLine("Bitte eine gültige Zahl eingeben!");
                        v = Console.ReadLine();
                    }
                    editedCar.Doors = doors;
                }

                Console.WriteLine("Bisheriges Getriebe: " + editedCar.Transmission);
                Console.Write("Neues Getriebe (a/m): ");
                v = Console.ReadLine();
                editedCar.Transmission = !string.IsNullOrEmpty(v) ? v : editedCar.Transmission;

                // save the changes to xml file
                carCollection.SerializeToXML(carCollection.cars);


                return editedCar;
            }
            catch (Exception e)
            {
                throw new Exception("Daten nicht gefunden. Bitte valide Auto ID eingeben.", e);
            }


        }

        public static void deleteCar()
        {
            var carCollection = CarCollection.Instance;
            int carId;
            Console.Clear();
            foreach (Car car in carCollection.cars)
            {
                Console.WriteLine(car);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Auto ID eingeben: ");
            try
            {
                carId = Convert.ToInt32(Console.ReadLine());
                Car deleteCar = carCollection.cars
                .First(car => car.CarID == carId);
                Console.Write(Environment.NewLine);
                Console.Write("Soll das Auto " + deleteCar + "wirklich gelöscht werden? (j/n): ");
                switch (Console.ReadLine())
                {
                    case "j":
                        carCollection.cars.Remove(deleteCar);
                        Console.WriteLine("Autokategorie wurde gelöscht");
                        // save the changes to xml file
                        carCollection.SerializeToXML(carCollection.cars);
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
                throw new InvalidCastException("Bitte eine valide Auto ID eingeben - keine Buchstaben!", e);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("Die angegebene Auto ID wurde nicht gefunden", e);
            }

        }

        public static string showCar()
        {
            var carCollection = CarCollection.Instance;
            int carId;
            Console.Clear();
            foreach (Car car in carCollection.cars)
            {
                Console.WriteLine(car);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Auto ID eingeben: ");
            try
            {
                carId = Convert.ToInt32(Console.ReadLine());
                // get the car by id
                Car showCar = carCollection.cars
                    .First(car => car.CarID == carId);
                Console.Write(Environment.NewLine);
                return showCar.carDetails();
            }
            catch (InvalidCastException e)
            {
                throw new InvalidCastException("Bitte eine Nummer eingeben!", e);
            }
            catch (FormatException e)
            {
                throw new InvalidCastException("Bitte eine valide Auto ID eingeben - keine Buchstaben!", e);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("Die angegebene Auto ID wurde nicht gefunden", e);
            }

        }
    }
}
