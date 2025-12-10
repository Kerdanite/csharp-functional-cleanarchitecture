namespace VetCalendar.Domain.Customers;

public sealed record ClientId(Guid Value)
{
    public static ClientId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}