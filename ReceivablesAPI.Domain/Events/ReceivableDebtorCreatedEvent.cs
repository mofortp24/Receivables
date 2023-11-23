namespace ReceivablesAPI.Domain.Events;

public class ReceivableDebtorCreatedEvent : BaseEvent
{
    public ReceivableDebtorCreatedEvent(ReceivableDebtor item)
    {
        Item = item;
    }

    public ReceivableDebtor Item { get; }
}
