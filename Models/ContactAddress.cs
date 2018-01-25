using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AddressBook.Models
{
    public class ContactAddress
    {
        public enum AddressTypeEnum
        {
            [Display(Name="Fakturační")]
            Invoice,
            [Display(Name="Dodací")]
            Delivery,
            [Display(Name="Korespondenční")]
            Correspondence
        };

        [Key]
        public int Id {get; set;}

        public ContactPerson ContactPerson {get; set;}

        [Display(Name="Typ adresy")]
        public AddressTypeEnum AddressType {get; set;}

        [Required, MaxLength(100, ErrorMessage = "Max 100 znaků")]
        [Display(Name="Ulice")]
        public string Street {get;set;}

        [MaxLength(50, ErrorMessage = "Max 50 znaků")]
        [Display(Name="Dodatek")]
        public string StreetAdd {get; set;}

        [Required, MaxLength(100, ErrorMessage = "Max 100 znaků")]
        [Display(Name = "Město")]
        public string City {get;set;}

        [Required]
        [Display(Name="PSČ")]
        [MinLength(6, ErrorMessage="PSČ ve formátu '123 45'")]
        [MaxLength(6, ErrorMessage="PSČ ve formátu '123 45'")]
        [DataType(DataType.PostalCode)]
        public string ZipCode   {get;set;}


    }
}