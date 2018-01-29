using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace AddressBook.Models
{
    public class ContactPerson
    {
        public enum GenderEnum
        {
            [Display(Name = "Mužské")]
            Male,
            [Display(Name = "Ženské")]
            Female
        };

        public ContactPerson()
        {
            ContactAddresses = new HashSet<ContactAddress>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tato hodnota musí být zadaná")]
        [MaxLength(50, ErrorMessage = "50 znaků maximálně")]
        [Display(Name = "Jméno")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Tato hodnota musí být zadaná")]
        [MaxLength(50, ErrorMessage = "50 znaků maximálně")]
        [Display(Name = "Příjmení")]
        public string Surname { get; set; }

        [Display(Name = "Celé jméno")]
        public string FullName
        {
            get { return Name + ", " + Surname; }
        }

        [Required(ErrorMessage = "Tato hodnota musí být zadaná")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Chybný formát")]
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Chybný formát")]
        [MaxLength(12, ErrorMessage = "Max 12 znaků")]
        public string Phone { get; set; }

        [Display(Name = "Datum narození")]
        [DataType(DataType.Date, ErrorMessage = "Chybný formát (dd.mm.rrrr)")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Pohlaví")]
        public GenderEnum GenderType { get; set; }

        [Display(Name = "Adresy")]
        public IEnumerable<ContactAddress> ContactAddresses { get; set; }
    }

}