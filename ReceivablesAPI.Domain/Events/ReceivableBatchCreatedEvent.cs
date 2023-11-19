namespace ReceivablesAPI.Domain.Events;

public class ReceivableBatchCreatedEvent : BaseEvent
{
    public ReceivableBatchCreatedEvent(ReceivableBatch item)
    {
        Item = item;
    }

    public ReceivableBatch Item { get; }
}
