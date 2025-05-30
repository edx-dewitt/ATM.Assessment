using ATM.ViewModels;

using FluentValidation;

namespace ATM.Validation
{
    public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
    {
        public TransactionRequestValidator()
        {
            RuleFor(x => x.AccountId).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
