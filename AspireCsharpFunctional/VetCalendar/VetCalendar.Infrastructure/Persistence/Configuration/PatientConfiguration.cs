using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetCalendar.Domain.Customers;
using VetCalendar.Domain.Patients;

namespace VetCalendar.Infrastructure.Persistence.Configuration;

internal sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,            
                value => new PatientId(value)) 
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasConversion(
                name => name.Value,                  
                value => PetName.Create(value).Value 
            )
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Species)
            .HasConversion(
                s => s.Value,                 
                value => Species.Create(value).Value 
            )
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.BirthDate)
            .HasColumnType("date");

        builder.HasIndex("ClientId");
    }
}