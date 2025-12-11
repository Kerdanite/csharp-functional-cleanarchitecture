using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetCalendar.Domain.Appointments;
using VetCalendar.Domain.Clients;
using VetCalendar.Domain.Clients.Patients;

namespace VetCalendar.Infrastructure.Persistence.Configuration;

internal sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,                   
                value => new AppointmentId(value))     
            .ValueGeneratedNever();

        builder.Property(a => a.ClientId)
            .HasConversion(
                id => id.Value,
                value => new ClientId(value))
            .IsRequired();

        builder.Property(a => a.PatientId)
            .HasConversion(
                id => id.Value,
                value => new PatientId(value))
            .IsRequired();

        builder.HasOne<Client>()
            .WithMany() 
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(a => a.Slot, slotBuilder =>
        {
            slotBuilder.Property(s => s.Date)
                .HasColumnName("Date")
                .IsRequired();

            slotBuilder.Property(s => s.StartTime)
                .HasColumnName("StartTime")
                .IsRequired();

            slotBuilder.Property(s => s.EndTime)
                .HasColumnName("EndTime")
                .IsRequired();

            slotBuilder.WithOwner();
        });

        builder.Property(a => a.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.Status)
            .HasConversion<int>() // enum → int
            .IsRequired();

        builder.HasIndex(a => a.ClientId);
        builder.HasIndex(a => a.PatientId);
        builder.HasIndex("Date", "StartTime");
      
    }
}