using CustomerInvoice_WebApp.Enums;
using CustomerInvoice_WebApp.Models;
using System.Collections.Generic;

namespace CustomerInvoice_WebApp.Data
{
    internal static class DbInitializer
    {
        public static void Initialize(CustomerInvoiceDbContext context)
        {
            if (!context.Customers.Any() && !context.Invoices.Any())
            {
                SeedData(context);
            }
        }

        private static void SeedData(CustomerInvoiceDbContext context)
        {
            var customers = GetPreconfiguredCustomers();
            context.Customers.AddRange(customers);
            context.SaveChanges();

            var invoices = GetPreconfiguredInvoices(customers);
            context.Invoices.AddRange(invoices);
            context.SaveChanges();
        }

        private static List<Customer> GetPreconfiguredCustomers()
        {
            return new List<Customer>
            {
                CreateCustomer("Avi Davi", "avi.d@gmail.com", "23 Tzahal St", "Givatime", "6733541"),
                CreateCustomer("Dov Oz", "dov.o@gmail.com", "15 Shenkin St", "Tel Aviv", "5734665"),
                CreateCustomer("Mosh Ben Ari", "mosh.b.a@gmail.com", "15 HaHof Shel Mosh St", "Eilat", "4563722"),
                CreateCustomer("Amos Dotan", "amos.d@gmail.com", "45 Halechi St", "Ramat Gan", "3352647")
            };
        }

        private static List<Invoice> GetPreconfiguredInvoices(List<Customer> customers)
        {
            return new List<Invoice>
            {
                CreateInvoice(DateTime.Now, InvoiceStatus.Unpaid, 100.00m, customers[0].Id),
                CreateInvoice(DateTime.Now.AddDays(-5), InvoiceStatus.Paid, 3005.00m, customers[0].Id),
                CreateInvoice(DateTime.Now.AddDays(-30), InvoiceStatus.Overdue, 146.00m, customers[0].Id),
                CreateInvoice(DateTime.Now, InvoiceStatus.Overdue, 277.00m, customers[1].Id),
                CreateInvoice(DateTime.Now, InvoiceStatus.Cancelled, 87.00m, customers[2].Id),
                CreateInvoice(DateTime.Now, InvoiceStatus.Paid, 4600.00m, customers[3].Id)
            };
        }

        private static Customer CreateCustomer(string name, string email, string street, string city, string zipCode)
        {
            return new Customer
            {
                Name = name,
                Email = email,
                Address = new Address { Street = street, City = city, State = "Israel", ZipCode = zipCode }
            };
        }

        private static Invoice CreateInvoice(DateTime date, InvoiceStatus status, decimal amount, int customerId)
        {
            return new Invoice { Date = date, Status = status, Amount = amount, CustomerId = customerId };
        }
    }
}
