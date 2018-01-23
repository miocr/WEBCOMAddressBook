using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AddressBook.Models
{
    public class ContactPerson
    {
        public enum GenderEnum
        {
            [Display(Name="Mužské")]
            Male, 
            [Display(Name="Ženské")]
            Female
        }

        [Key]
        public int Id {get; set;}

        [Required, MaxLength(50)]
        [StringLength(50, ErrorMessage="Max 50 znaků")]
        [Display(Name="Jméno")]       
        public string Name {get; set;}

        [Required, MaxLength(50, ErrorMessage="50 znaků max")]
        //[StringLength(50, ErrorMessage="Max 50 znaků"]
        [Display(Name="Příjmení")]
        public string Surname {get; set;}

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return Name + ", " + Surname; }
        }

        [Required, EmailAddress]
        public string Email {get;set;}


        [DataType(DataType.PhoneNumber, ErrorMessage="Chybný formát")]
        [MaxLength(12, ErrorMessage="Max 12 znaků")]
        public string Phone {get;set;}

        [DataType(DataType.Date, ErrorMessage="Chybný formát (dd.mm.rrrr)")]
        [DisplayFormat(DataFormatString="{0:dd.mm.yyyy}", ApplyFormatInEditMode=true)]
        public DateTime Birthdate {get; set;}

        public GenderEnum GenderType {get; set;}
    }

}