namespace VetCalendar.Domain;

public static partial class DomainErrors
{
    public static class Appointment
    {
        public const string ReasonTooLong          = "Appointment reason is too long.";
        public const string SlotAlreadyBooked    = "This appointment slot is already booked.";
    }
}