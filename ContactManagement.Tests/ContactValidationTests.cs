using System.ComponentModel.DataAnnotations;
using ContactManagement.Models;

namespace ContactManagement.Tests;

public class ContactValidationTests
{
    private static IList<ValidationResult> Validate(Contact contact)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(contact);
        Validator.TryValidateObject(contact, context, results, true);
        return results;
    }

    [Fact]
    public void Name_TooShort_FailsValidation()
    {
        var contact = new Contact { Name = "Ana", Phone = "912345678", Email = "ana@test.com" };
        var errors = Validate(contact);
        Assert.Contains(errors, e => e.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Name_WithMoreThan5Chars_PassesValidation()
    {
        var contact = new Contact { Name = "Gabriel", Phone = "912345678", Email = "gabriel@test.com" };
        var errors = Validate(contact);
        Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Phone_WithLetters_FailsValidation()
    {
        var contact = new Contact { Name = "Gabriel", Phone = "91234567A", Email = "gabriel@test.com" };
        var errors = Validate(contact);
        Assert.Contains(errors, e => e.MemberNames.Contains("Phone"));
    }

    [Fact]
    public void Phone_TooShort_FailsValidation()
    {
        var contact = new Contact { Name = "Gabriel", Phone = "91234", Email = "gabriel@test.com" };
        var errors = Validate(contact);
        Assert.Contains(errors, e => e.MemberNames.Contains("Phone"));
    }

    [Fact]
    public void Phone_With9Digits_PassesValidation()
    {
        var contact = new Contact { Name = "Gabriel", Phone = "912345678", Email = "gabriel@test.com" };
        var errors = Validate(contact);
        Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Phone"));
    }

    [Fact]
    public void Email_InvalidFormat_FailsValidation()
    {
        var contact = new Contact { Name = "Gabriel", Phone = "912345678", Email = "not-an-email" };
        var errors = Validate(contact);
        Assert.Contains(errors, e => e.MemberNames.Contains("Email"));
    }

    [Fact]
    public void Email_ValidFormat_PassesValidation()
    {
        var contact = new Contact { Name = "Gabriel", Phone = "912345678", Email = "gabriel@test.com" };
        var errors = Validate(contact);
        Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Email"));
    }

    [Fact]
    public void Name_Empty_FailsValidation()
    {
        var contact = new Contact { Name = "", Phone = "912345678", Email = "gabriel@test.com" };
        var errors = Validate(contact);
        Assert.Contains(errors, e => e.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Phone_Empty_FailsValidation()
    {
        var contact = new Contact { Name = "Gabriel", Phone = "", Email = "gabriel@test.com" };
        var errors = Validate(contact);
        Assert.Contains(errors, e => e.MemberNames.Contains("Phone"));
    }

    [Fact]
    public void Email_Empty_FailsValidation()
    {
        var contact = new Contact { Name = "Gabriel", Phone = "912345678", Email = "" };
        var errors = Validate(contact);
        Assert.Contains(errors, e => e.MemberNames.Contains("Email"));
    }
}
