using System.Text.Json;
using VetCalendar.Domain.Shared;

namespace VetCalendar.Infrastructure;

public class OutboxMessage
{
    public Guid Id { get; private set; }
    public DateTime OccurredOnUtc { get; private set; }
    public string Type { get; private set; }
    public string Payload { get; private set; } 
    public DateTime? ProcessedOnUtc { get; private set; }
    public int RetryCount { get; private set; }

    private OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string payload)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
        Type = type;
        Payload = payload;
    }

    public static OutboxMessage FromDomainEvent(IDomainEvent domainEvent)
    {
        var type = domainEvent.GetType();
        var typeName = $"{type.FullName}, {type.Assembly.GetName().Name}";

        var payload = JsonSerializer.Serialize(domainEvent, type);

        return new OutboxMessage(
            id: Guid.NewGuid(),
            occurredOnUtc: domainEvent.OccurredOnUtc,
            type: typeName,
            payload: payload);
    }
}