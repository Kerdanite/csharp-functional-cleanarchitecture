using CSharpFunctionalExtensions;
using VetCalendar.Application.Abstractions;
using VetCalendar.Domain;
using VetCalendar.Domain.Appointments;
using VetCalendar.Domain.Clients;
using VetCalendar.Domain.Clients.Patients;

namespace VetCalendar.Application.BookAppointment;

public sealed class BookAppointmentCommandHandler
    : ICommandHandler<BookAppointmentCommand>
{
    private readonly IClientRepository _clientRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public BookAppointmentCommandHandler(
        IClientRepository clientRepository,
        IAppointmentRepository appointmentRepository)
    {
        _clientRepository = clientRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Result> Handle(
        BookAppointmentCommand command,
        CancellationToken cancellationToken)
    {
        var clientId  = new ClientId(command.ClientId);
        var patientId = new PatientId(command.PatientId);


        return await Maybe.From(async () => await _clientRepository.GetByIdAsync(clientId, cancellationToken))
            .ToResult(DomainErrors.Client.NotFound)
            .Ensure(client => client.Patients.Any(s => s.Id == patientId), DomainErrors.Patient.NotFoundForClient)
            .Bind(client => AppointmentSlot.Create(command.Date, command.StartTime))
            .Ensure(async slot => await _appointmentRepository.IsSlotAvailableAsync(slot.Date, slot.StartTime, cancellationToken), DomainErrors.Appointment.SlotAlreadyBooked)
            .Bind(slot => Appointment.Book(clientId, patientId, slot, command.Reason))
            .Tap(async appointment => await _appointmentRepository.AddAsync(appointment, cancellationToken));
    }
}