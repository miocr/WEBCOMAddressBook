using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using AddressBook.Data;
using AddressBook.Models;
using AddressBook.Models.AddressBookViewModels;
using AddressBook.Services;

namespace AddressBook.Controllers
{

    //[Authorize]
    public class AddressBookController : Controller
    {

        private readonly AddressBookDbContext _context;

        public AddressBookController(AddressBookDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*
            ContactViewModel contactViewModel = new ContactViewModel();
            contactViewModel.ContactPerson = await _context.ContatPersons
                 .Include(cp => cp.ContactAddresses)
                 .SingleAsync(cp => cp.Id == id.Value);
            */

            ContactPerson contactPerson = await _context.ContatPersons
                .Include(_cp => _cp.ContactAddresses)
                .SingleOrDefaultAsync(_cp => _cp.Id == id);

            return View(contactPerson);
        }
    }
}