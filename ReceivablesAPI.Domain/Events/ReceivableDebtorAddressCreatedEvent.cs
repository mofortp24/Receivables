namespace ReceivablesAPI.Domain.Events;

public class ReceivableDebtorAddressCreatedEvent : BaseEvent
{
    public ReceivableDebtorAddressCreatedEvent(ReceivableDebtorAddress item)
    {
        Item = item;
    }

    public ReceivableDebtorAddress Item { get; }
}
