using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AddressBook.Models;
using AddressBook.Models.AddressBookViewModels;

namespace AddressBook.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AddressBookDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any contact.
            if (context.ContatPersons.Any())
            {
                return;   // DB has been seeded
            }

            var contactPersons = new ContactPerson[]
            {
                new ContactPerson { Name = "Karel",   Surname = "Novák",
                    GenderType = ContactPerson.GenderEnum.Male,
                    Email = "novak@novak.com",
                    Phone = "123456789",
                    Birthdate = DateTime.Parse("1999-01-01") },
                new ContactPerson { Name = "Jana",   Surname = "Nováčková",
                    GenderType = ContactPerson.GenderEnum.Female,
                    Email = "novackova@novackova.com",
                    Phone = "987654321",
                    Birthdate = DateTime.Parse("2000-12-31") }
            };

            foreach (ContactPerson contactPerson in contactPersons)
            {
                context.ContatPersons.Add(contactPerson);
            }

            context.SaveChanges();

            var addresses = new ContactAddress[]
            {
                new ContactAddress { Street = "Fakturační 1", City = "Novákovice",
                    ZipCode = "111 00", AddressType = ContactAddress.AddressTypeEnum.Invoice,
                    ContactPerson = contactPersons[0]},
                new ContactAddress { Street = "Dodací 2", City = "Karlovice",
                    ZipCode = "666 00", AddressType = ContactAddress.AddressTypeEnum.Delivery,
                    ContactPerson = contactPersons[0]},
                new ContactAddress { Street = "Korespondenční 3", City = "Dopisovice",
                    ZipCode = "777 00", AddressType = ContactAddress.AddressTypeEnum.Correspondence,
                    ContactPerson = contactPersons[0]},
                new ContactAddress { Street = "Fakturační 1", City = "Janovice",
                    ZipCode = "555 00", AddressType = ContactAddress.AddressTypeEnum.Invoice,
                    ContactPerson = contactPersons[1]},
            };

            foreach (ContactAddress address in addresses)
            {
                context.ContactAddresses.Add(address);
            }

            context.SaveChanges();
            
        }
    }
}