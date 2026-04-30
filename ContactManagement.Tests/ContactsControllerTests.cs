using ContactManagement.Controllers;
using ContactManagement.Data;
using ContactManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Tests;

public class ContactsControllerTests
{
    private ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Create_InvalidModel_ReturnsViewWithErrors()
    {
        var db = CreateDbContext();
        var controller = new ContactsController(db);
        controller.ModelState.AddModelError("Name", "Required");

        var contact = new Contact { Name = "", Phone = "912345678", Email = "test@test.com" };
        var result = await controller.Create(contact);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
    }

    [Fact]
    public async Task Create_ValidContact_RedirectsToIndex()
    {
        var db = CreateDbContext();
        var controller = new ContactsController(db);

        var contact = new Contact { Name = "Gabriel", Phone = "912345678", Email = "gabriel@test.com" };
        var result = await controller.Create(contact);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }

    [Fact]
    public async Task Create_DuplicatePhone_ReturnsViewWithError()
    {
        var db = CreateDbContext();
        db.Contacts.Add(new Contact { Name = "Existing", Phone = "912345678", Email = "existing@test.com" });
        await db.SaveChangesAsync();

        var controller = new ContactsController(db);
        var contact = new Contact { Name = "Gabriel", Phone = "912345678", Email = "gabriel@test.com" };
        var result = await controller.Create(contact);

        Assert.IsType<ViewResult>(result);
        Assert.True(controller.ModelState.ContainsKey("Phone"));
    }

    [Fact]
    public async Task Create_DuplicateEmail_ReturnsViewWithError()
    {
        var db = CreateDbContext();
        db.Contacts.Add(new Contact { Name = "Existing", Phone = "911111111", Email = "gabriel@test.com" });
        await db.SaveChangesAsync();

        var controller = new ContactsController(db);
        var contact = new Contact { Name = "Gabriel", Phone = "912345678", Email = "gabriel@test.com" };
        var result = await controller.Create(contact);

        Assert.IsType<ViewResult>(result);
        Assert.True(controller.ModelState.ContainsKey("Email"));
    }

    [Fact]
    public async Task Edit_InvalidModel_ReturnsViewWithErrors()
    {
        var db = CreateDbContext();
        var existing = new Contact { Name = "Gabriel", Phone = "912345678", Email = "gabriel@test.com" };
        db.Contacts.Add(existing);
        await db.SaveChangesAsync();

        var controller = new ContactsController(db);
        controller.ModelState.AddModelError("Name", "Required");

        var result = await controller.Edit(existing.Id, existing);

        Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
    }

    [Fact]
    public async Task Edit_DuplicatePhone_ReturnsViewWithError()
    {
        var db = CreateDbContext();
        db.Contacts.Add(new Contact { Name = "Other", Phone = "911111111", Email = "other@test.com" });
        var target = new Contact { Name = "Gabriel", Phone = "912345678", Email = "gabriel@test.com" };
        db.Contacts.Add(target);
        await db.SaveChangesAsync();

        var controller = new ContactsController(db);
        target.Phone = "911111111";
        var result = await controller.Edit(target.Id, target);

        Assert.IsType<ViewResult>(result);
        Assert.True(controller.ModelState.ContainsKey("Phone"));
    }
}
