using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBook.Data;
using AddressBook.Models;
using AddressBook.Models.AddressBookViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Controllers
{
    public class AddressBookController : Controller
    {
        private readonly AddressBookDbContext _context;

        public AddressBookController(AddressBookDbContext context)
        {
            _context = context;
        }

        #region ContactPerson Actions

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
                ViewBag.searchString = searchString;
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

        [Authorize]
        [HttpPost, ActionName("ContactEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactEditPost(int? id)
        {
            if (id == null)
                return NotFound();

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
                    return RedirectToAction("ContactEdit");
                }
                catch (DbUpdateException /* ex */ )
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }

            return View(contactPersonUpdated);
        }

        [Authorize]
        public IActionResult ContactCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactCreate(
            [Bind ("ContactPerson," +
                "ContactPerson.Name,ContactPerson.Surname,ContactPerson.Email," +
                "ContactPerson.Phone,ContactPerson.Birthdate,ContactPerson.GenerType," +
                "ContactAddress," +
                "ContactAddress.Street,ContactAddress.City,ContactAddress.ZipCode")] ContactViewModel newContact)
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
            catch (DbUpdateException /* ex */ )
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(newContact);
        }

        [Authorize]
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

        [Authorize]
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
                if (contactPerson.ContactAddresses != null)
                {
                    foreach (ContactAddress address in contactPerson.ContactAddresses)
                        _context.ContactAddresses.Remove(address);
                }
                _context.ContatPersons.Remove(contactPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException /* ex */ )
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("ContactDelete", new { id = id, saveChangesError = true });
            }
        }

        #endregion

        #region ContactAddress Actions
        [Authorize]
        public IActionResult AddressCreate(string contactId)
        {
            ContactPerson contactPerson = _context.ContatPersons
                .Single(_cp => _cp.Id == Convert.ToInt32(contactId));

            if (contactPerson == null)
                return NotFound();

            ContactViewModel contactViewModel = new ContactViewModel();
            contactViewModel.ContactPerson = contactPerson;

            return View(contactViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddressCreate(
            [Bind("ContactPerson,ContactAddress,ContactAddress.Street,ContactAddress.StreetAdd,AddressTypeEnum.ContactAddress.City,ContactAddress.ZipCode")] ContactViewModel contactView)
        {
            try
            {
                ContactPerson contactPerson = _context.ContatPersons
                    .Single(_cp => _cp.Id == contactView.ContactPerson.Id);

                if (contactPerson == null)
                    return NotFound();

                // Ignorujeme chybějící povinné údaje pro ContactPerson
                foreach (string key in ModelState.Keys)
                    if (key.Contains("ContactPerson"))
                        ModelState.Remove(key);

                if (ModelState.IsValid)
                {
                    contactView.ContactAddress.ContactPerson = contactPerson;
                    _context.ContactAddresses.Add(contactView.ContactAddress);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ContactEdit", new { contactPerson.Id });
                }
            }
            catch (DbUpdateException /* ex */ )
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(contactView);
        }

        [Authorize]
        public async Task<IActionResult> AddressDelete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            ContactAddress contactAddress = await _context.ContactAddresses
                .Include(ca => ca.ContactPerson)
                .SingleAsync(ca => ca.Id == id.Value);

            if (contactAddress == null)
                return NotFound();

            try
            {
                _context.ContactAddresses.Remove(contactAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction("ContactEdit", new { contactAddress.ContactPerson.Id });
            }
            catch (DbUpdateException /* ex */ )
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("ContactEdit", new { contactAddress.ContactPerson.Id, saveChangesError = true });
            }
        }

        [Authorize]
        public async Task<IActionResult> AddressEdit(int? id)
        {
            if (id == null)
                return NotFound();

            ContactAddress contactAddress = await _context.ContactAddresses
                .Include(ca => ca.ContactPerson)
                .SingleAsync(ca => ca.Id == id);

            if (contactAddress == null)
                return NotFound();

            return View(contactAddress);
        }

        [Authorize]
        [HttpPost, ActionName("AddressEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactAddressPost(int? id)
        {
            if (id == null)
                return NotFound();

            ContactAddress contactAddressUpdated = await _context.ContactAddresses
                .Include(ca => ca.ContactPerson)
                .SingleAsync(ca => ca.Id == id);

            if (await TryUpdateModelAsync<ContactAddress>(
                    contactAddressUpdated, "",
                    ca => ca.AddressType, ca => ca.Street, ca => ca.StreetAdd,
                    ca => ca.City, ca => ca.ZipCode
                ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ContactEdit",
                        new { contactAddressUpdated.ContactPerson.Id, saveChangesError = true });

                }
                catch (DbUpdateException /* ex */ )
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }

            return View(contactAddressUpdated);
        }

        #endregion

    }
}