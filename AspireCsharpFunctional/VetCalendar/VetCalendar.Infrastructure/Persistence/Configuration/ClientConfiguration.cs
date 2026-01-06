using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetCalendar.Domain.Clients;

namespace VetCalendar.Infrastructure.Persistence.Configuration;

internal sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,                   
                value => new ClientId(value))     
            .ValueGeneratedNever();

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .HasConversion(
                email => email.Value,                
                value => Email.Create(value).Value) 
            .IsRequired()
            .HasMaxLength(254);

        builder.Property(c => c.PhoneNumber)
            .HasConversion(
                phone => phone.Value,                  
                value => PhoneNumber.Create(value).Value) 
            .IsRequired()
            .HasMaxLength(50);

        builder
            .HasMany(c => c.Patients)
            .WithOne()
            .HasForeignKey("ClientId")
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(c => c.Patients)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .AutoInclude();

        builder.HasIndex(c => c.Email).IsUnique();
        builder.HasIndex(c => c.PhoneNumber).IsUnique();
    }
}