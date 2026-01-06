using System.ComponentModel;

namespace AspireCsharpFunctional.ApiService.Request;

public sealed record BookAppointmentRequest(
    Guid ClientId,
    Guid PatientId,
    [property: DefaultValue("2025-01-10")]
    DateOnly Date,
    [property: DefaultValue("09:00:00")]
    TimeOnly StartTime,
    [property: DefaultValue("Vaccination annuelle")]
    string? Reason);