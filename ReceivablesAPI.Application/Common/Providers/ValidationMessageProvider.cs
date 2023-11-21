using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceivablesAPI.Application.Common.Providers;
public static class ValidationMessageProvider
{
    // AddReceivablesCommandValidator
    public static readonly string AtLeastOneReceivableMustBeProvided = "At least one receivable must be provided";

    // ReceivableDto
    public static readonly string ReferenceIsRequired = "Reference is required";
    public static readonly string CurrencyCodeIsRequired = "Currency code is required";
    public static readonly string IssueDateIsRequired = "Issue date is required";
    public static readonly string OpeningValueMustBeGreaterThanZero = "Opening value must be greater than zero";
    public static readonly string PaidValueMustBeGreaterThanOrEqualToZero = "Paid value must be greater than or equal to zero";
    public static readonly string DueDateIsRequired = "Due date is required";
    public static readonly string DebtorNameIsRequired = "Debtor name is required";
    public static readonly string DebtorReferenceIsRequired = "Debtor reference is required";
    public static readonly string DebtorCountryCodeIsRequired = "Debtor country code is required";

    // GetReceivablesOpenClosedSummaryQueryValidator
    public static readonly string ReceivablesSummaryDateMustBeProvided = "Receivables Summary Date must be provided";
    public static readonly string ReceivablesSummaryDateMustNotBeInTheFuture = "Receivables Summary Date must not be in the future";
       
}
