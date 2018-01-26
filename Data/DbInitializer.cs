using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using AddressBook.Models;
using AddressBook.Models.AddressBookViewModels;
using AddressBook.Migrations;

namespace AddressBook.Data
{
    public static class SampleData
    {

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
                return;

            ApplicationUser demoUser = new ApplicationUser();
            demoUser.UserName = "demo@demo.org";
            demoUser.Email = "demo@demo.org";
            demoUser.NormalizedEmail = "DEMO@DEMO.ORG";
            demoUser.NormalizedUserName = "DEMO@DEMO.ORG";
            // Password: "Abcd.1234"
            demoUser.PasswordHash = "AQAAAAEAACcQAAAAENMGma+TO897piNW7acxeJj2Ndh81ruUODAJpJGAv+PtTwIVP6+e9Nfwv5AOJDmizQ==";
            demoUser.LockoutEnabled = true;
            demoUser.SecurityStamp = "4146b593-d287-4182-a282-441277aadec1";

            context.Users.Add(demoUser);
            context.SaveChanges();
        }

        public static void Initialize(AddressBookDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.ContatPersons.Any())
                return;

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
                new ContactAddress { Street = "Hlavní ulice 2", City = "Novákovice",
                    ZipCode = "111 00", AddressType = ContactAddress.AddressTypeEnum.Default,
                    ContactPerson = contactPersons[0]},
                new ContactAddress { Street = "Dodací naměstí 8", City = "Karlovice",
                    ZipCode = "666 00", AddressType = ContactAddress.AddressTypeEnum.Delivery,
                    ContactPerson = contactPersons[0]},
                new ContactAddress { Street = "Korespondenční 16", City = "Dopisovice",
                    ZipCode = "777 00", AddressType = ContactAddress.AddressTypeEnum.Correspondence,
                    ContactPerson = contactPersons[0]},
                new ContactAddress { Street = "Hlavní ulice 10", City = "Janovice",
                    ZipCode = "555 00", AddressType = ContactAddress.AddressTypeEnum.Default,
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