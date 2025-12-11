namespace VetCalendar.Domain;

public static class DomainErrors
{
    public static class Client
    {
        public const string FirstNameIsRequired = "First name is required.";
        public const string LastNameIsRequired  = "Last name is required.";
        public const string EmailIsRequired     = "Email is required.";
        public const string EmailIsInvalid        = "Email format is invalid.";
        public const string PhoneNumberIsRequired = "Phone number is required.";
        public const string PhoneNumberIsInvalid = "Phone number format is invalid.";
        public const string EmailAlreadyInUse = "A client with this email already exists.";
        public const string PhoneNumberAlreadyInUse = "A client with this phone number already exists.";
        public const string NotFound = "Client not found.";
    }

    public static class Patient
    {
        public const string NameIsRequired   = "Patient name is required.";
        public const string NameTooLong      = "Patient name is too long.";
        public const string SpeciesIsRequired = "Species is required.";
        public const string SpeciesTooLong   = "Species is too long.";
    }
}