using ATM.ViewModels;
using FluentValidation;

namespace ATM.Validation
{
    public class TransferRequestValidator : AbstractValidator<TransferRequest>
    {
        public TransferRequestValidator()
        {

        }
    }
}