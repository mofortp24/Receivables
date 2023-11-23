namespace ReceivablesAPI.Application.Common.Providers
{
    public interface IBatchReferenceProvider
    {
        public string GenerateNextBatchReference<T>(CancellationToken cancellationToken);
    }
}
