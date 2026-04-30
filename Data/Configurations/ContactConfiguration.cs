using ContactManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactManagement.Data.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasIndex(c => c.Phone).IsUnique();
        builder.HasIndex(c => c.Email).IsUnique();
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
