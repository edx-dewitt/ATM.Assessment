using ATM.ViewModels;
using FluentValidation.TestHelper;

namespace ATM.Validation.Tests;

[TestFixture]
public class TransactionRequestValidatorTests
{
    private TransactionRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new TransactionRequestValidator();
    }

    [Test]
    public void Should_Have_Error_When_AccountId_Is_Zero()
    {
        var model = new TransactionRequest { AccountId = 0, Amount = 100 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AccountId);
    }

    [Test]
    public void Should_Have_Error_When_Amount_Is_Zero()
    {
        var model = new TransactionRequest { AccountId = 1, Amount = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Test]
    public void Should_Not_Have_Error_When_Request_Is_Valid()
    {
        var model = new TransactionRequest { AccountId = 1, Amount = 50 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.AccountId);
        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }
}