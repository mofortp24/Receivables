namespace ReceivablesAPI.Domain.Events;

public class ReceivableBatchSuccessfullySavedEvent : BaseEvent
{
    public ReceivableBatchSuccessfullySavedEvent(ReceivableBatch item)
    {
        Item = item;
    }

    public ReceivableBatch Item { get; }
}
