namespace ReceivablesAPI.Domain.Events;

public class ReceivableCreatedEvent : BaseEvent
{
    public ReceivableCreatedEvent(Receivable item)
    {
        Item = item;
    }

    public Receivable Item { get; }
}
