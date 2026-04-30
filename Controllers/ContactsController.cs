using ContactManagement.Data;
using ContactManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Controllers;

public class ContactsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContactsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var contacts = await _context.Contacts.ToListAsync();
        return View(contacts);
    }

    public async Task<IActionResult> Details(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null) return NotFound();
        return View(contact);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Contact contact)
    {
        if (!ModelState.IsValid) return View(contact);

        if (await _context.Contacts.AnyAsync(c => c.Phone == contact.Phone))
            ModelState.AddModelError("Phone", "This phone number is already in use.");

        if (await _context.Contacts.AnyAsync(c => c.Email == contact.Email))
            ModelState.AddModelError("Email", "This email is already in use.");

        if (!ModelState.IsValid) return View(contact);

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null) return NotFound();
        return View(contact);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Contact contact)
    {
        if (id != contact.Id) return NotFound();

        if (!ModelState.IsValid) return View(contact);

        if (await _context.Contacts.AnyAsync(c => c.Phone == contact.Phone && c.Id != id))
            ModelState.AddModelError("Phone", "This phone number is already in use.");

        if (await _context.Contacts.AnyAsync(c => c.Email == contact.Email && c.Id != id))
            ModelState.AddModelError("Email", "This email is already in use.");

        if (!ModelState.IsValid) return View(contact);

        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null) return NotFound();

        contact.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
