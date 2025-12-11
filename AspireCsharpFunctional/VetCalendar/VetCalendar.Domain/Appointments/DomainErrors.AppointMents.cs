namespace VetCalendar.Domain;

public static partial class DomainErrors
{
    public static class Appointment
    {
        public const string DurationMustBePositive = "Appointment duration must be positive.";
        public const string DurationTooLong        = "Appointment duration is too long.";
        public const string ReasonTooLong          = "Appointment reason is too long.";
    }
}