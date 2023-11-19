namespace ReceivablesAPI.Domain.Entities;

public class ReceivableDebtorAddress
{
    public string? DebtorAddress1 { get; set; }
    public string? DebtorAddress2 { get; set; }
    public string? DebtorTown { get; set; }
    public string? DebtorState { get; set; }
    public string? DebtorZip { get; set; }
    public CountryCode DebtorCountryCode { get; set; }
    public string? DebtorRegistrationNumber { get; set; }
}