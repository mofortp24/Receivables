using ReceivablesAPI.Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;
using ReceivablesAPI.Application.Common.Providers;

namespace ReceivableAPI.Application.UnitTests.Exceptions;

public class ValidationExceptionTests
{
    [Test]
    public void DefaultConstructorCreatesAnEmptyErrorDictionary()
    {
        var actual = new ValidationException().Errors;

        actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }

    [Test]
    public void SingleValidationFailureCreatesASingleElementInErrorDictionary()
    {
        string propertyName = "Reference";
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure(propertyName, ValidationMessageProvider.DebtorReferenceIsRequired),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { propertyName });
        actual[propertyName].Should().BeEquivalentTo(new string[] { ValidationMessageProvider.DebtorReferenceIsRequired });
    }

    [Test]
    public void MultipleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        string propertyReferenceName = "Reference";
        string propertyCurrencyName = "CurrencyCode";

        var failures = new List<ValidationFailure>
            {
                new (propertyReferenceName, ValidationMessageProvider.ReferenceIsRequired),
                new (propertyReferenceName, ValidationMessageProvider.ReferenceMaxLengthExceeded),
                new (propertyCurrencyName, ValidationMessageProvider.CurrencyCodeIsRequired),
                new (propertyCurrencyName, ValidationMessageProvider.CurrencyCodeFormatUnsupported),
                new (propertyCurrencyName, ValidationMessageProvider.CurrencyCodeLengthIncorrect)
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { propertyReferenceName, propertyCurrencyName });

        actual[propertyReferenceName].Should().BeEquivalentTo(new string[]
        {
            ValidationMessageProvider.ReferenceIsRequired,
            ValidationMessageProvider.ReferenceMaxLengthExceeded,
        });

        actual[propertyCurrencyName].Should().BeEquivalentTo(new string[]
        {
            ValidationMessageProvider.CurrencyCodeIsRequired,
            ValidationMessageProvider.CurrencyCodeFormatUnsupported,
            ValidationMessageProvider.CurrencyCodeLengthIncorrect
        });
    }
}
