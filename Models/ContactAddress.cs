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
            [Display(Name="Hlavní")]
            Default,
            [Display(Name="Dodací")]
            Delivery,
            [Display(Name="Korespondenční")]
            Correspondence
        };

        [Key]
        public int Id {get; set;}

        public ContactPerson ContactPerson {get; set;}

        [Required(ErrorMessage = "Tato hodnota musí být zadaná")]
        [Display(Name="Typ adresy")]
        public AddressTypeEnum AddressType {get; set;}

        [Required(ErrorMessage = "Tato hodnota musí být zadaná")]
        [MaxLength(100, ErrorMessage = "100 znaků maximálně")]
        [Display(Name="Ulice")]
        public string Street {get;set;}

        [MaxLength(50, ErrorMessage = "Max 50 znaků")]
        [Display(Name="Dodatek")]
        public string StreetAdd {get; set;}

        [Required(ErrorMessage = "Tato hodnota musí být zadaná")]
        [MaxLength(100, ErrorMessage = "100 znaků maximálně")]
        [Display(Name = "Město")]
        public string City {get;set;}

        [Required(ErrorMessage = "Tato hodnota musí být zadaná")]
        [MinLength(6, ErrorMessage = "PSČ ve formátu '123 45'")]
        [MaxLength(6, ErrorMessage = "PSČ ve formátu '123 45'")]
        [Display(Name="PSČ")]
        [DataType(DataType.PostalCode)]
        public string ZipCode   {get;set;}


    }
}