using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace CarRental.Models
{
    // use a singleton pattern for one instance of all bookings in a collection
    public sealed class BookingCollection
    {
        private BookingCollection()
        {
            bookings = new List<Booking>();
        }
        private static BookingCollection instance = null;
        public static BookingCollection Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new BookingCollection();
                }
                return instance;
            }
        }

        public List<Booking> bookings { get; set; }

        public int CurrentId
        {
            get
            {
                return this.bookings.Max(x => x.BookingID);
            }
        }

        public void SerializeToXML(List<Booking> bookings)
        {
            XmlSerializer bookingSerializer = new XmlSerializer(typeof(List<Booking>));
            using (StreamWriter bookingWriter = new StreamWriter("bookings.xml"))
            {
                bookingSerializer.Serialize(bookingWriter, bookings);
            }

        }

        public List<Booking> DeserializeFromXML()
        {
            XmlSerializer bookingSerializer = new XmlSerializer(typeof(List<Booking>));
            List<Booking> bookings;
            using (FileStream bookingFileStream = new FileStream("bookings.xml", FileMode.Open))
            {
                try
                {
                    bookings = (List<Booking>)bookingSerializer.Deserialize(bookingFileStream);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Beim lesen der Daten ist ein Fehler aufgetreten", e);
                }

            }

            return bookings;
        }

        // Helper functions
        // check if there are any bookings
        public bool bookingExists()
        {
            var bookingCollection = BookingCollection.Instance;
            if (bookingCollection.bookings.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Booking getBooking(int id = 1)
        {
            var bookingCollection = BookingCollection.Instance;
            if (bookingCollection.bookings.Count == 0)
            {
                return null;
            }
            else
            {
                Booking showBooking = bookingCollection.bookings
                    .SingleOrDefault(record => record.BookingID == id);
                if (showBooking != null)
                {
                    return showBooking;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Booking> getBookingByCustomer(int id)
        {
            var bookingCollection = BookingCollection.Instance;
            var possibleBookings = bookingCollection.bookings
                .Where(records => records.CustomerID == id)
                .ToList();
            return possibleBookings;
        }

        // check available cars in given time for a booking
        public static List<Car> getAvailableCars(DateTime start, DateTime end, int cc)
        {
            var carCollection = CarCollection.Instance;
            var bookingCollection = BookingCollection.Instance;
            List<Car> availableCars = new List<Car>();
            List<Car> carsOfCategory = carCollection.cars
                .Where(records => records.CarCategoryID == cc)
                .ToList();

            // check if booking exists in given time
            foreach (Car car in carsOfCategory)
            {
                List<Booking> checkBooking = bookingCollection.bookings
                    .FindAll(records => records.CarID == car.CarID && (records.StartDate <= end && records.EndDate >= start));
                    
                if (checkBooking.Count == 0)
                {
                    availableCars.Add(car);
                }

            }
            return availableCars;
        }

        // special case for editing existing bookings
        // therefore we overload the method
        public static List<Car> getAvailableCars(DateTime start, DateTime end, int cc, int bookingId)
        {
            var carCollection = CarCollection.Instance;
            var bookingCollection = BookingCollection.Instance;
            List<Car> availableCars = new List<Car>();
            List<Car> carsOfCategory = carCollection.cars
                .Where(records => records.CarCategoryID == cc)
                .ToList();

            // check if booking exists in given time
            foreach (Car car in carsOfCategory)
            {
                List<Booking> checkBooking = bookingCollection.bookings
                    .FindAll(records => records.CarID == car.CarID && (records.StartDate <= end && records.EndDate >= start));

                if (checkBooking.Count == 0)
                {
                    availableCars.Add(car);
                } else
                {
                    // we have to check if the booking we edit is in the collection
                    // if yes the car is available
                    Booking bookingWeEdit = checkBooking.SingleOrDefault(record => record.BookingID == bookingId);
                    if (bookingWeEdit != null)
                    {
                        availableCars.Add(car);
                    }
                }

            }
            return availableCars;
        }
    }
}
