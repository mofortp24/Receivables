using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceivablesAPI.Application.Common.Providers;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;

namespace ReceivablesAPI.Application.UnitTests.TestFeed;
public static class CorrectReceivableDtoFeed
{
    public static ReceivableDto GetTestInstance() => GetTestInstance(false);
    public static ReceivableDto GetTestInstance(bool isClosed) => GetTestInstance(isClosed, false);
    public static ReceivableDto GetTestInstance(bool isClosed, bool isCancelled) => GetTestInstance(isClosed, string.Empty, isCancelled);
    public static ReceivableDto GetTestInstance(bool isClosed, string closedDate, bool isCancelled)
    {
        return new ReceivableDto
        {
            Reference = "RCVBL/01/2023",
            CurrencyCode = "PLN",
            IssueDate = DateTime.Now.ToString(ValidationFormatProviders.DateTimeAcceptableFormat),
            
            OpeningValue = 654.32m,
            PaidValue = 123.45m,
            DueDate = DateTime.Now.AddMonths(1).ToString(ValidationFormatProviders.DateTimeAcceptableFormat),
            ClosedDate = closedDate,

            Cancelled = isCancelled,

            DebtorName = "John Kowalsky", 
            DebtorReference = "DBT_Ref_01",

            DebtorAddress1 = "Address 1/23", 
            DebtorAddress2 = "Address 2/48",
            DebtorTown = "Warsaw",
            DebtorState = "Mazowieckie", 
            DebtorZip = "02-333",
            DebtorCountryCode = "PL", 
            DebtorRegistrationNumber = "PL 87654"
        };
    }
}
