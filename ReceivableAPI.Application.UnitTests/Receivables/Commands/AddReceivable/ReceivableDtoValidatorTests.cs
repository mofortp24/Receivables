using ReceivablesAPI.Application.Receivables.Commands.AddReceivables;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;
using ReceivablesAPI.Application.UnitTests.Common;
using ReceivablesAPI.Application.Common.Providers;

namespace ReceivablesAPI.Application.UnitTests.Receivables.Commands.AddReceivable;

[TestFixture]
public class ReceivableDtoValidatorTests
{
    private ReceivableDtoValidator _validator;
    private ReceivableDto _dto;

    [SetUp]
    public void Setup()
    {
        _validator = new ReceivableDtoValidator();
        _dto = new ReceivableDto();
    }

    [Test]
    public void Reference_Should_Not_Be_Empty_And_Maximum_Length_50()
    {
        // Empty reference should fail
        _dto.Reference = string.Empty;
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.Reference, "");

        // Exceeding maximum length should fail
        _dto.Reference = new string('a', 51);
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.Reference, _dto.Reference); 

        // Valid reference should pass
        _dto.Reference = "ValidReference";
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.Reference, _dto.Reference);

        _dto.Reference = new string('a', 50);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.Reference, _dto.Reference); 
    }

    [Test]
    public void CurrencyCode_Should_Follow_Rules()
    {
        // Empty currency code should fail
        _dto.CurrencyCode = string.Empty;
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.CurrencyCode, _dto.CurrencyCode);

        // Invalid length should fail
        _dto.CurrencyCode = "ABCDEF";
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.CurrencyCode, _dto.CurrencyCode);

        // Valid code but not a supported enum value should fail
        _dto.CurrencyCode = "US2";
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.CurrencyCode, _dto.CurrencyCode);

        // Valid currency code should pass
        _dto.CurrencyCode = "USD";
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.CurrencyCode, _dto.CurrencyCode);
    }

    [Test]
    public void IssueDate_Should_Follow_Rules()
    {
        // Empty IssueDate should fail
        _dto.IssueDate = string.Empty;
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.IssueDate, _dto.IssueDate);
        
        // Invalid IssueDate formats should fail
        _dto.IssueDate = "InvalidDate";
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.IssueDate, _dto.IssueDate);

        _dto.IssueDate = "2023-11-22";
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.IssueDate, _dto.IssueDate);

        // Valid IssueDate format should pass
        _dto.IssueDate = DateTime.Now.ToString(ValidationFormatProviders.DateTimeAcceptableFormat);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.IssueDate, _dto.IssueDate);
    }

    [Test]
    public void OpeningValue_Should_Be_Greater_Than_Zero()
    {
        var dto = new ReceivableDto();

        // Zero OpeningValue should fail
        dto.OpeningValue = 0;
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.OpeningValue, 0);

        // Positive OpeningValue should pass
        dto.OpeningValue = 100;
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.OpeningValue, 100);
    }

    [Test]
    public void PaidValue_Should_Be_Greater_Than_Or_Equal_To_Zero()
    {
        // Negative PaidValue should fail
        _dto.PaidValue = -50;
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.PaidValue, _dto.PaidValue);

        // Zero or Positive PaidValue should pass
        _dto.PaidValue = 0;
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.PaidValue, _dto.PaidValue);
        _dto.PaidValue = 50;
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.PaidValue, _dto.PaidValue);
    }

    [Test]
    public void DueDate_Should_Follow_Rules()
    {
        // Empty DueDate should fail
        _dto.DueDate = String.Empty;
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DueDate, _dto.DueDate);

        // Invalid DueDate formats should fail
        _dto.DueDate = "InvalidDate";
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DueDate, _dto.DueDate);

        _dto.DueDate = "2023-11-22";
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DueDate, _dto.DueDate);

        // Valid DueDate format should pass
        _dto.DueDate = DateTime.Now.ToString(ValidationFormatProviders.DateTimeAcceptableFormat);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DueDate, _dto.DueDate);
    }

    [Test]
    public void ClosedDate_Should_Follow_Rules()
    {
        // Invalid ClosedDate formats should fail
        _dto.ClosedDate = "InvalidDate";
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.ClosedDate, _dto.ClosedDate);

        _dto.ClosedDate = "2023-11-22";
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.ClosedDate, _dto.ClosedDate);

        // Valid ClosedDate format should pass
        _dto.ClosedDate = DateTime.Now.ToString(ValidationFormatProviders.DateTimeAcceptableFormat);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.ClosedDate, _dto.ClosedDate);
    }

    [Test]
    public void DebtorName_Should_Follow_Rules()
    {
        // Empty DebtorName should fail
        _dto.DebtorName = string.Empty;
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorName, _dto.DebtorName);

        // Exceeding maximum length should fail
        _dto.DebtorName = new string('a', 101);
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorName, _dto.DebtorName);

        // Valid DebtorName should pass
        _dto.DebtorName = "ValidDebtorName";
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorName, _dto.DebtorName);

        _dto.DebtorName = new string('a', 100);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorName, _dto.DebtorName);
    }

    [Test]
    public void DebtorReference_Should_Follow_Rules()
    {
        // Empty DebtorReference should fail
        _dto.DebtorReference = string.Empty;
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorReference, _dto.DebtorReference);

        // Exceeding maximum length should fail
        _dto.DebtorReference = new string('a', 51);
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorReference, _dto.DebtorReference);

        // Valid DebtorReference should pass
        _dto.DebtorReference = "ValidDebtorReference";
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorReference, _dto.DebtorReference);

        _dto.DebtorReference = new string('a', 50);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorReference, _dto.DebtorReference);
    }

    [Test]
    public void DebtorAddress1_Should_Follow_Rules()
    {
        // Exceeding maximum length for DebtorAddress1 should fail
        _dto.DebtorAddress1 = new string('a', 251);
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorAddress1, _dto.DebtorAddress1);

        // Valid DebtorAddress1 length should pass
        _dto.DebtorAddress1 = new string('a', 250);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorAddress1, _dto.DebtorAddress1);
    }

    [Test]
    public void DebtorAddress2_Should_Follow_Rules()
    {
        // Exceeding maximum length for DebtorAddress2 should fail
        _dto.DebtorAddress2 = new string('a', 251);
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorAddress2, _dto.DebtorAddress2);

        // Valid DebtorAddress1 length should pass
        _dto.DebtorAddress2 = new string('a', 250);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorAddress2, _dto.DebtorAddress2);
    }

    [Test]
    public void DebtorTown_And_DebtorState_Should_Follow_Rules()
    {
        // Exceeding maximum length for DebtorTown should fail
        _dto.DebtorTown = new string('a', 51);
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorTown, _dto.DebtorTown);

        // Valid DebtorTown length should pass
        _dto.DebtorTown = new string('a', 50);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorTown, _dto.DebtorTown);

        // Exceeding maximum length for DebtorState should fail
        _dto.DebtorState = new string('a', 51);
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorState, _dto.DebtorState);

        // Valid DebtorState length should pass
        _dto.DebtorState = new string('a', 50);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorState, _dto.DebtorState);
    }

    [Test]
    public void DebtorZip_Should_Follow_Rules()
    {
        // Exceeding maximum length for DebtorZip should fail
        _dto.DebtorZip = new string('a', 11);
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorZip, _dto.DebtorZip);

        // Valid DebtorZip length should pass
        _dto.DebtorZip = new string('a', 10);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorZip, _dto.DebtorZip);
    }

    [Test]
    public void DebtorCountryCode_Should_Follow_Rules()
    {
        // Empty DebtorCountryCode should fail
        _dto.DebtorCountryCode = string.Empty;
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorCountryCode, _dto.DebtorCountryCode);

        // Incorrect length for DebtorCountryCode should fail
        _dto.DebtorCountryCode = "XYZ";
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorCountryCode, _dto.DebtorCountryCode);

        // Valid but unsupported DebtorCountryCode should fail
        _dto.DebtorCountryCode = "U1";
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorCountryCode, _dto.DebtorCountryCode);

        // Supported DebtorCountryCode should pass
        _dto.DebtorCountryCode = "PL";
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorCountryCode, _dto.DebtorCountryCode);
    }

    [Test]
    public void DebtorRegistrationNumber_Should_Follow_Rules()
    {
        // Exceeding maximum length for DebtorRegistrationNumber should fail
        _dto.DebtorRegistrationNumber = new string('a', 16);
        _validator.ShouldHaveValidationErrorFor(_dto, x => x.DebtorRegistrationNumber, _dto.DebtorRegistrationNumber);

        // Valid DebtorRegistrationNumber length should pass
        _dto.DebtorRegistrationNumber = new string('a', 15);
        _validator.ShouldNotHaveValidationErrorFor(_dto, x => x.DebtorRegistrationNumber, _dto.DebtorRegistrationNumber);
    }

}