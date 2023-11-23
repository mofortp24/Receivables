namespace ReceivablesAPI.Application.Common.Providers;

public static class ValidationMessageProvider
{
    // AddReceivablesCommandValidator
    public static readonly string AtLeastOneReceivableMustBeProvided = "At least one receivable must be provided";

    // ReceivableDto
    public static readonly string ReferenceIsRequired = "Reference is required";
    public static readonly string ReferenceMaxLengthExceeded = "Reference maximum length of 50 characters exceeded";
    public static readonly string CurrencyCodeIsRequired = "Currency code is required";
    public static readonly string CurrencyCodeLengthIncorrect = "Currency code must be exact 3 characters";
    public static readonly string CurrencyCodeFormatUnsupported = "Provided Currency code was not recognized as a valid currency code";
    public static readonly string IssueDateIsRequired = "Issue Date is required";
    public static readonly string IssueDateIncorrectFormat = $"Issue Date format is incorrect. Accepted format is { ValidationFormatProviders.DateTimeAcceptableFormat }";
    public static readonly string OpeningValueMustBeGreaterThanZero = "Opening Value must be greater than zero";
    public static readonly string PaidValueMustBeGreaterThanOrEqualToZero = "Paid Value must be greater than or equal to zero";
    public static readonly string DueDateIsRequired = "Due Date is required";
    public static readonly string DueDateIncorrectFormat = $"Due Date format is incorrect. Accepted format is { ValidationFormatProviders.DateTimeAcceptableFormat }";
    public static readonly string ClosedDateIncorrectFormat = $"Closed Date format is incorrect. Accepted format is { ValidationFormatProviders.DateTimeAcceptableFormat }";
    
    // ReceivableDto/Debtor
    public static readonly string DebtorNameIsRequired = "Debtor Name is required";
    public static readonly string DebtorNameMaxLengthExceeded = "Debtor Name length must be maximum 100 characters";
    public static readonly string DebtorReferenceIsRequired = "Debtor Reference is required";
    public static readonly string DebtorReferenceMaxLengthExceeded = "Debtor Reference length must be maximum 50 characters";

    // ReceivableDto/DebtorAddress
    public static readonly string DebtorAddress1MaxLengthExceeded = "Debtor primary Address length must be maximum 250 characters";
    public static readonly string DebtorAddress2MaxLengthExceeded = "Debtor secondary Address length must be maximum 250 characters";
    public static readonly string DebtorTownMaxLengthExceeded = "Debtor Town name length must be maximum 50 characters";
    public static readonly string DebtorStateMaxLengthExceeded = "Debtor State name length must be maximum 50 characters";
    public static readonly string DebtorZipMaxLengthExceeded = "Debtor ZIP Code length must be maximum 10 characters";
    public static readonly string DebtorCountryCodeIsRequired = "Debtor Country Code is required";
    public static readonly string DebtorCountryCodeLengthIncorrect = "Debtor Country Code must be exact 2 characters";
    public static readonly string DebtorCountryCodeFormatUnsupported = "Provided Country Code was not recognized as a valid country code";
    public static readonly string DebtorRegistrationNumberMaxLengthExceeded = "Debtor Registration Number length must be maximum 15 characters";

    // GetReceivablesOpenClosedSummaryQueryValidator
    public static readonly string ReceivablesSummaryDateMustBeProvided = "Receivables Summary Date must be provided";
    public static readonly string ReceivablesSummaryDateMustNotBeInTheFuture = "Receivables Summary Date must not be in the future";
       
}
