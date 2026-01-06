using System.ComponentModel;

namespace AspireCsharpFunctional.ApiService.Request;

public sealed record CreateClientRequest(
    [property: DefaultValue("John")]
    string FirstName,
    [property: DefaultValue("Doe")]
    string LastName,
    [property: DefaultValue("john.doe@example.com")]
    string Email,
    [property: DefaultValue("+33601020304")]
    string PhoneNumber);