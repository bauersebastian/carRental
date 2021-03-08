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
                    if (car != null)
                    {
                        Console.WriteLine("Auto wurde angelegt");
                    }
                    else
                    {
                        Console.WriteLine("Das Auto konnte nicht angelegt werden. Bitte Eingaben prüfen.");
                    }
                    Console.WriteLine("Zurück zum Hauptmenü.");
                    Task.Delay(2000).Wait();
                    return false;
                case "2":
                    var editedCar = editCar();
                    if (editedCar == null)
                    {
                        Console.WriteLine("Es wurde kein Eintrag zur ID gefunden.");
                    } else
                    {
                        Console.WriteLine("Auto wurde geändert.");
                    }
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
            var v = Console.ReadLine();
            var cc = Helper.checkInt(v);
            CarCategory carCategorySelect = carCategoryCollection.getCarCategoryById(cc);
            if (carCategorySelect == null)
            {
                return null;
            }
            else
            {
                newCar.CarCategoryID = cc;
            }
            Console.Write("Leistung in PS:");
            newCar.HorsePower = Helper.checkInt(Console.ReadLine());
            Console.Write("Kofferraumvolumen:");
            newCar.LuggageCompartment = Helper.checkInt(Console.ReadLine());
            Console.Write("Anzahl der Türen:");
            newCar.Doors = Helper.checkInt(Console.ReadLine());
            Console.Write("Automatik oder manuelle Gangschaltung (a/m):");
            v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                while (v != "a" && v != "m")
                {
                    Console.WriteLine("Bitte einen gültigen Wert eingeben! (nur a oder m zulässig)");
                    v = Console.ReadLine();
                }
                newCar.Transmission = v;
            }
            
            carCollection.cars.Add(newCar);
            carCollection.SerializeToXML(carCollection.cars);
            return newCar;
        }

        public static Car editCar()
        {
            var carCollection = CarCollection.Instance;
            var carCategoryCollection = CarCategoryCollection.Instance;
            // are there any cars at the moment?
            if (carCollection.cars.Count == 0)
            {
                Console.WriteLine("Es wurden noch keine Autos angelegt.");
                Task.Delay(2000).Wait();
                return null;
            }
            int carId;
            Console.Clear();
            foreach (Car car in carCollection.cars)
            {
                Console.WriteLine(car);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Auto ID eingeben: ");
            carId = Helper.checkInt(Console.ReadLine());
            Car editedCar = carCollection.getCarById(carId);
            if (editedCar == null)
            {
                return null;
            }
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
                int cc = Helper.checkInt(v);
                CarCategory carCategorySelect = carCategoryCollection.getCarCategoryById(cc);
                if (carCategorySelect != null)
                {
                    editedCar.CarCategoryID = cc;
                } else
                {
                    return null;
                }
            }
            Console.WriteLine("Bisherige PS: " + editedCar.HorsePower);
            Console.Write("Neue PS: ");
            v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                var hp = Helper.checkInt(v);
                editedCar.HorsePower = hp;
            }
            Console.WriteLine("Bisheriges Kofferraumvolumen: " + editedCar.LuggageCompartment);
            Console.Write("Neues Kofferraumvolumen: ");
            v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                var lc = Helper.checkInt(v);
                editedCar.LuggageCompartment = lc;
            }
            Console.WriteLine("Bisherige Anzahl an Türen: " + editedCar.Doors);
            Console.Write("Neue Anzahl an Türen: ");
            v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                var doors = Helper.checkInt(v);
                editedCar.Doors = doors;
            }

            Console.WriteLine("Bisheriges Getriebe: " + editedCar.Transmission);
            Console.Write("Neues Getriebe (a/m): ");
            v = Console.ReadLine();
            if (!string.IsNullOrEmpty(v))
            {
                while (v != "a" && v != "m")
                {
                    Console.WriteLine("Bitte einen gültigen Wert eingeben! (nur a oder m zulässig)");
                    v = Console.ReadLine();
                }
                editedCar.Transmission = v;
            }
           
            // save the changes to xml file
            carCollection.SerializeToXML(carCollection.cars);

            return editedCar;
            
        }

        public static void deleteCar()
        {
            var carCollection = CarCollection.Instance;
            // are there any cars at the moment?
            if (carCollection.cars.Count == 0)
            {
                Console.WriteLine("Es wurden noch keine Autos angelegt.");
                Task.Delay(2000).Wait();
                return;
            }
            int carId;
            Console.Clear();
            foreach (Car car in carCollection.cars)
            {
                Console.WriteLine(car);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Auto ID eingeben: ");
            carId = Helper.checkInt(Console.ReadLine());
            Car deleteCar = carCollection.getCarById(carId);
            if (deleteCar == null)
            {
                Console.WriteLine("Löschen abgebrochen - ID wurde nicht gefunden.");
                Task.Delay(2000).Wait();
                return;
            }
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

        public static string showCar()
        {
            var carCollection = CarCollection.Instance;
            int carId;
            Console.Clear();
            // are there any cars at the moment?
            if (carCollection.cars.Count == 0)
            {
                return "Es wurden noch keine Autos angelegt.";
            }
            foreach (Car car in carCollection.cars)
            {
                Console.WriteLine(car);
            }
            Console.Write(Environment.NewLine);
            Console.Write("Auto ID eingeben: ");
            carId = Helper.checkInt(Console.ReadLine());
            Car showCar = carCollection.getCarById(carId);
            if (showCar == null)
            {
                return "Kein Auto mit dieser ID gefunden";
            } else
            {
                Console.Write(Environment.NewLine);
                return showCar.carDetails();
            }
        }

    }
}
