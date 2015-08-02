
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using Contacts.Model;

namespace Contacts.ConsoleApp
{
    using Data;
    using System.Data.Entity;

    public class ContactExec
    {
        static void Main()
        {
            var context = new PhonebookContext();

            var listAllData = context.Contacts
                .Include(c => c.Emails)
                .Include(c => c.Phones)
                .ToList();

            foreach (var contact in listAllData)
            {
                Console.WriteLine("Name: {0}", contact.Name);
                if (contact.Company != null) Console.WriteLine("    Company name: {0}", contact.Company);
                if (contact.Postion != null) Console.WriteLine("    Position: {0}", contact.Postion);
                if (contact.Url != null) Console.WriteLine("    Url: {0}", contact.Url);
                if (contact.Phones.Count > 0)
                {
                    string phones = contact.Phones.Aggregate("", (current, phone) => current + (" " + phone.PhoneNumber));
                    phones = phones.Trim();
                    Console.Write("    Phones: {0}", phones);
                    Console.WriteLine();
                }
                if (contact.Emails.Count > 0)
                {
                    string emails = contact.Emails.Aggregate("", (current, email) => current + (" " + email.EmailAddress));
                    emails = emails.Trim();
                    Console.Write("    Emails: {0}", emails);
                    Console.WriteLine();
                }
                if (contact.Notes.Count > 0)
                {
                    string notes = contact.Notes.Aggregate("", (current, note) => current + (" " + note));
                    notes = notes.Trim();
                    Console.Write("    Notes: {0}", notes);
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n################################################################################################\n");

            var json = File.ReadAllText("../../contacts.json");
            var serializer = new JavaScriptSerializer();
            var contacts = serializer.Deserialize<ContactDTO[]>(json);

            foreach (var contact in contacts)
            {
                try
                {
                    SaveContactInDb(contact);
                    Console.WriteLine("Contact {0} imported", contact.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
            }
        }

        private static void SaveContactInDb(ContactDTO contact)
        {
            if (contact.Name == null)
            {
                throw new ArgumentException("Name is required");
            }
            var ct = new Contact()
            {
                Name = contact.Name
            };
            if (contact.Company != null) ct.Company = contact.Company;
            if (contact.Site != null) ct.Url = contact.Site;
            if (contact.Position != null) ct.Postion = contact.Position;
            if (contact.Notes != null)
            {
                ct.Notes = new HashSet<string>();
                ct.Notes.Add(contact.Notes);
            }
            if (contact.Emails != null)
            {
                ct.Emails = new HashSet<Email>();
                foreach (var em in contact.Emails.Select(email => new Email() {EmailAddress = email}))
                {
                    ct.Emails.Add(em);
                }
            }
            if (contact.Phones != null)
            {
                ct.Phones = new HashSet<Phone>();
                foreach (var ph in contact.Phones.Select(phone => new Phone() {PhoneNumber = phone}))
                {
                    ct.Phones.Add(ph);
                }
            }
            var context = new PhonebookContext();
            context.Contacts.Add(ct);
            context.SaveChanges();
        }
    }
}
