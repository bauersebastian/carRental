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
                return this.bookings.Max(x => x.CarID);
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
        public bool bookingExists(int id = 1)
        {
            var bookingCollection = BookingCollection.Instance;
            if (bookingCollection.bookings.Count == 0)
            {
                return false;
            }
            else
            {
                Booking showBooking = bookingCollection.bookings
                    .SingleOrDefault(record => record.BookingID == id);
                if (showBooking != null)
                {
                    return true;
                } else
                {
                    return false;
                }
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
    }
}
