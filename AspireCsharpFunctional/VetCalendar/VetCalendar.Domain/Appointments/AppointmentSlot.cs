using CSharpFunctionalExtensions;

namespace VetCalendar.Domain.Appointments;

public sealed record AppointmentSlot
{
    public DateOnly Date { get; }
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }

    public static readonly TimeSpan DefaultDuration = TimeSpan.FromMinutes(30);

    private AppointmentSlot(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        Date      = date;
        StartTime = startTime;
        EndTime = endTime;
    }

    public static Result<AppointmentSlot> Create(
        DateOnly date,
        TimeOnly startTime)
    {
        // j'ai fais le choix de SQL server par défaut sans réfléchir.
        // avec sql serveur si je fais une race condition de réservation le même jour à 9h00 et 9h10, même si la précondition select du repo est respectée, on peut avoir un overlap =>
        // Write SKEW possible (serializable pas suffisant)
        // Avec Postgre isolation SSI => pas de soucis write skew ni perf car optimistic lock
        // dans notre cas booking vétérinaire, ça semble cohérent niveau métier d'accepter que sur 9h00 , 9h30, 10h00 etc et refuser 9h10 car sinon trou dans planing
        // donc Ensure ce cas
        return Result.Success(startTime)
            .Ensure(t => t.Minute is 0 or 30, DomainErrors.Appointment.InvalidStartMinute)
            .Map(t => new AppointmentSlot(date, startTime, startTime.Add(DefaultDuration)));
    }
}