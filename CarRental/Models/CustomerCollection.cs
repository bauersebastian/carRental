using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace CarRental.Models
{
    // use a singleton pattern for one instance of all customers in a collection
    public sealed class CustomerCollection
    {
        private CustomerCollection()
        {
            customers = new List<Customer>();
        }
        private static CustomerCollection instance = null;
        public static CustomerCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerCollection();
                }
                return instance;
            }
        }

        public List<Customer> customers { get; set; }
        public int CurrentId
        {
            get
            {
                return this.customers.Max(x => x.CustomerID);
            }
        }

        public void SerializeToXML(List<Customer> customers)
        {
            XmlSerializer customerSerializer = new XmlSerializer(typeof(List<Customer>));
            using (StreamWriter customerWriter = new StreamWriter("customers.xml"))
            {
                customerSerializer.Serialize(customerWriter, customers);
            }

        }

        public List<Customer> DeserializeFromXML()
        {
            XmlSerializer customerSerializer = new XmlSerializer(typeof(List<Customer>));
            List<Customer> customers;
            using (FileStream customerFileStream = new FileStream("customers.xml", FileMode.Open))
            {
                try
                {
                    customers = (List<Customer>)customerSerializer.Deserialize(customerFileStream);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Beim lesen der Daten ist ein Fehler aufgetreten", e);
                }
                
            }

            return customers;
        }

    }
}
