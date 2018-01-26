using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models.AddressBookViewModels
{
    public class ContactViewModel
    {
        public ContactPerson ContactPerson {get;set;}

        public ContactAddress ContactAddress { get; set; }
    }

}