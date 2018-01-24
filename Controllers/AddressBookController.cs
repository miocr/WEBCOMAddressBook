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
    public class AddressBookController : Controller
    {
        private readonly AddressBookDbContext _context;

        public AddressBookController(AddressBookDbContext context)
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

        // POST: Students/Edit/5
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

        // GET: ContactCreate
        public IActionResult ContactCreate()
        {
            return View();
        }

        // POST: ContactCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactCreate(
            [Bind("Name,Surname,Email,Phone,Birthdate,GenerType")]
            ContactPerson newContactPerson)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(newContactPerson);
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
            return View(newContactPerson);
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