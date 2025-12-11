using Microsoft.EntityFrameworkCore;
using VetCalendar.Domain.Appointments;
using VetCalendar.Domain.Clients;
using VetCalendar.Domain.Clients.Patients;
using VetCalendar.Infrastructure.Persistence;

namespace VetCalendar.Infrastructure.Tests
{
    public class AppointmentRepositoryTests
    {
        private VetCalendarDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<VetCalendarDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new VetCalendarDbContext(options);
        }


        [Fact]
        public async Task IsSlotAvailableAsync_ShouldDetectOverlap()
        {
            var db = CreateDbContext();

            var repo = new AppointmentRepository(db);

            // Un rendez-vous existant : 09:00 > 09:30
            var existingSlotResult = AppointmentSlot.Create(
                new DateOnly(2025, 1, 10),
                new TimeOnly(9, 0));

            Assert.True(existingSlotResult.IsSuccess);

            var existingAppointment = Appointment.Book(
                clientId: ClientId.New(),
                patientId: PatientId.New(),
                slot: existingSlotResult.Value,
                reason: "Test").Value;

            db.Appointments.Add(existingAppointment);
            await db.SaveChangesAsync();

            // Nouveau créneau qui overlap : 09:10 > 09:40
            var newSlotStart = new TimeOnly(9, 10);

            // Act
            var isAvailable = await repo.IsSlotAvailableAsync(
                date: new DateOnly(2025, 1, 10),
                startTime: newSlotStart,
                ct: CancellationToken.None);

            Assert.False(isAvailable);
        }

        [Fact]
        public async Task IsSlotAvailableAsync_ShouldReturnTrue_WhenNewSlotIsAdjacentButNotOverlapping()
        {
            await using var dbContext = CreateDbContext();

            var repository = new AppointmentRepository(dbContext);

            var date = new DateOnly(2025, 1, 10);

            // Rendez-vous existant : 09:00–09:30
            var existingSlotResult = AppointmentSlot.Create(date, new TimeOnly(9, 0));
            Assert.True(existingSlotResult.IsSuccess);
            var existingSlot = existingSlotResult.Value;

            var existingAppointmentResult = Appointment.Book(
                clientId:  new ClientId(Guid.NewGuid()),
                patientId: new PatientId(Guid.NewGuid()),
                slot:      existingSlot,
                reason:    "Existing appointment");

            Assert.True(existingAppointmentResult.IsSuccess);
            var existingAppointment = existingAppointmentResult.Value;

            dbContext.Appointments.Add(existingAppointment);
            await dbContext.SaveChangesAsync();

            // Nouveau créneau : 09:30–10:00 > OK (adjacent mais pas overlappé)
            var newStart = new TimeOnly(9, 30);

            var isAvailable = await repository.IsSlotAvailableAsync(
                date,
                newStart,
                CancellationToken.None);

            Assert.True(isAvailable);
        }
    }
}