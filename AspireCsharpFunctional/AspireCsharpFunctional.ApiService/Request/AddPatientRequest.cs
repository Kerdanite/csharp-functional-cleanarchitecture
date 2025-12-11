using System.ComponentModel;

namespace AspireCsharpFunctional.ApiService.Request;

public sealed record AddPatientRequest(
    [property: DefaultValue("Misty")]
    string Name,
    [property: DefaultValue("Cat")]
    string Species,
    [property: DefaultValue("2020-05-12")]
    DateOnly BirthDate);