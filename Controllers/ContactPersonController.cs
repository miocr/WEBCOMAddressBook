using System;
using System.Linq;
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
    public class ContactPersonController : Controller
    {
        private readonly AddressBookDbContext _context;

        public ContactPersonController(AddressBookDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string currentFilter,
            string searchString,
            int? page)
        {
            if (!String.IsNullOrEmpty(searchString))
                page = 1;
            else
                searchString = currentFilter;

            IQueryable<ContactPerson> query = _context.ContatPersons
                .Include(_cp => _cp.ContactAddresses);

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(_cp =>
                    _cp.Name.Contains(searchString) ||
                    _cp.Surname.Contains(searchString) ||
                    _cp.Phone == searchString || _cp.Phone == searchString);
            }

            IEnumerable<ContactPerson> contactPersons = await query.ToListAsync();

            return View(contactPersons);
        }

        public async Task<IActionResult> ContactDetail(int? id)
        {
            if (id == null)
                return NotFound();

            ContactPerson contactPerson = await _context.ContatPersons
                .Include(_cp => _cp.ContactAddresses)
                .SingleAsync(_cp => _cp.Id == id);

            return View(contactPerson);
        }

        [Authorize]
        public async Task<IActionResult> ContactEdit(int? id)
        {
            if (id == null)
                return NotFound();

            ContactPerson contactPerson = await _context.ContatPersons
               .Include(_cp => _cp.ContactAddresses)
               .SingleAsync(_cp => _cp.Id == id);

            if (contactPerson == null)
                return NotFound();

            return View(contactPerson);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactEditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContactPerson contactPersonUpdated = await _context.ContatPersons
                    .SingleAsync(_cp => _cp.Id == id);

            if (await TryUpdateModelAsync<ContactPerson>(
                contactPersonUpdated, "",
                _cp => _cp.Name, _cp => _cp.Surname,
                _cp => _cp.Email, _cp => _cp.Phone,
                _cp => _cp.GenderType, _cp => _cp.Birthdate
                ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }

            return View(contactPersonUpdated);
        }

        public IActionResult ContactCreate()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactCreate(
            [Bind("ContactPerson," +
            "ContactPerson.Name,ContactPerson.Surname,ContactPerson.Email," +
            "ContactPerson.Phone,ContactPerson.Birthdate,ContactPerson.GenerType," +
            "ContactAddress," +
            "ContactAddress.Street,ContactAddress.City,ContactAddress.ZipCode")]
            ContactViewModel newContact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.ContatPersons.Add(newContact.ContactPerson);
                    newContact.ContactAddress.ContactPerson = newContact.ContactPerson;
                    _context.ContactAddresses.Add(newContact.ContactAddress);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(newContact);
        }



        public async Task<IActionResult> ContactDelete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            ContactPerson contactPerson = await _context.ContatPersons
                .SingleAsync(_cp => _cp.Id == id.Value);

            if (contactPerson == null)
                return NotFound();

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(contactPerson);
        }

        [HttpPost, ActionName("ContactDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactDeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            ContactPerson contactPerson = await _context.ContatPersons
                .Include(_cp => _cp.ContactAddresses)
                .AsNoTracking()
                .SingleAsync(_cp => _cp.Id == id.Value);

            if (contactPerson == null)
                return RedirectToAction("Index");

            try
            {
                /*
                IList<ContactAddress> addresses = await _context.ContactAddresses
                    .Where(_ca => _ca.ContactPerson.Id == id.Value)
                    .ToListAsync();
                */

                if (contactPerson.ContactAddresses != null)
                {
                    foreach (ContactAddress address in contactPerson.ContactAddresses)
                        _context.ContactAddresses.Remove(address);
                }
                _context.ContatPersons.Remove(contactPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("ContactDelete", new { id = id, saveChangesError = true });
            }
        }

    }
}