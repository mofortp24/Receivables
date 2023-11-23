using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Internal;
using Newtonsoft.Json.Linq;

namespace ReceivablesAPI.Application.UnitTests.Common;
public static class ValidationTestHelperExtensions
{
    public static void ShouldHaveValidationError<T, TProperty>(this AbstractValidator<T> validator, T instance, Expression<Func<T, TProperty>> property)
    {
        var propertyName = (property.Body as System.Linq.Expressions.MemberExpression)?.Member.Name;
        var result = validator.Validate(instance);
        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == propertyName));
    }

    public static void ShouldNotHaveValidationError<T, TProperty>(this AbstractValidator<T> validator, T instance, Expression<Func<T, TProperty>> property)
    {
        var propertyName = (property.Body as System.Linq.Expressions.MemberExpression)?.Member.Name;
        var result = validator.Validate(instance);
        Assert.IsFalse(result.Errors.Any(e => e.PropertyName == propertyName));
    }

    public static void ShouldHaveValidationErrorFor<T, TProperty>(this AbstractValidator<T> validator, T instance, Expression<Func<T, TProperty>> expression, TProperty value)
    {
        var propertyName = (expression.Body as MemberExpression)?.Member.Name;

        var validationResult = validator.Validate(instance);
        var error = validationResult.Errors.FirstOrDefault(e => e.PropertyName == propertyName && (value == null || e.AttemptedValue.Equals(value)));

        if (error == null)
        {
            throw new AssertionException($"Expected a validation error for property '{propertyName}' with value '{value}', but no error was found.");
        }
    }
    public static void ShouldNotHaveValidationErrorFor<T, TProperty>(this AbstractValidator<T> validator, T instance, Expression<Func<T, TProperty>> expression, TProperty value)
    {
        var propertyName = (expression.Body as MemberExpression)?.Member.Name;

        var validationResult = validator.Validate(instance);
        var error = validationResult.Errors.FirstOrDefault(e => e.PropertyName == propertyName && (value == null || e.AttemptedValue.Equals(value)));

        if (error != null)
        {
            throw new AssertionException($"Unexpected validation error for property '{propertyName}' with value '{value}'. Error found: '{error.ErrorMessage}'");
        }
    }
}
