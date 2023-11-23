using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceivablesAPI.Application.Common.Providers;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;
using NUnit.Framework;
using FluentValidation.TestHelper;
using ReceivablesAPI.Application.UnitTests.TestFeed;

namespace ReceivablesAPI.Application.UnitTests.Receivables.Commands.AddReceivable;
[TestFixture]
public class AddReceivablesCommandValidatorTests
{
    private AddReceivablesCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new AddReceivablesCommandValidator();
    }

    [Test]
    public void ReceivableList_Should_Not_Be_Empty()
    {
        // Arrange
        var receivablesCommand = new AddReceivablesCommand();

        // Act
        var result = _validator.TestValidate(receivablesCommand);

        // Assert
        result.ShouldHaveValidationErrorFor<List<ReceivableDto>>(x => x.Receivables.ReceivableList)
            .WithErrorMessage(ValidationMessageProvider.AtLeastOneReceivableMustBeProvided);
    }

    [Test]
    public void ReceivableList_Items_Should_Follow_ReceivableDtoValidator_Rules()
    {
        // Arrange
        var receivablesCommand = new AddReceivablesCommand()
        {
            Receivables = new ReceivablesDto
            {
                ReceivableList = new List<ReceivableDto>()
                {
                    CorrectReceivableDtoFeed.GetTestInstance()
                }
            }
        };

        // Act
        var result = _validator.TestValidate(receivablesCommand);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
