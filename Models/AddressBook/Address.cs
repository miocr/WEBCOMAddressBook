using System;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
    public class Address
    {
        public enum AddressTypeEnum
        {
            Invoice,
            Delivery,
            Correspondence
        }

        [Key]
        public int Id {get; set;}

        public ContactPerson ContactPerson {get; set;}

        public AddressTypeEnum AddressType {get; set;}

        [Required, MaxLength(100, ErrorMessage="Max 100 znaků")]
        [Display(Name="Ulice")]
        public string Street {get;set;}

        [Display(Name="Dodatek")]
        public string StreetAdd {get; set;}
        public string City {get;set;}

        [Required]
        [MinLength(6, ErrorMessage="PSČ ve formátu '123 45'")]
        [MaxLength(6, ErrorMessage="PSČ ve formátu '123 45'")]
        [DataType(DataType.PostalCode)]
        public string ZipCode   {get;set;}


    }
}