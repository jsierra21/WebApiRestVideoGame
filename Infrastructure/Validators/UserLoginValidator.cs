using Core.Entities;
using Core.Messages;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(entity => entity.Correo)
                .EmailAddress().WithMessage(ErrorMessage.ValueError)
                .Length(1, 256).WithMessage(ErrorMessage.LengthError)
                .WithName("Correo");

            RuleFor(entity => entity.Password)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //.Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[.,*?_#$%!=¡?)(/\\-+])(?=.*[A-Z]).{8,50}$").WithMessage(ErrorMessage.MatchError)
                .WithName("Password");
        }
    }

    public class AccountLoginValidator : AbstractValidator<AccountLogin>
    {
        public AccountLoginValidator()
        {
            RuleFor(entity => entity.Correo)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                .Length(1, 60).WithMessage(ErrorMessage.LengthError)
                .WithName("Correo");

            RuleFor(entity => entity.Password)
                .NotNull().WithMessage(ErrorMessage.NullError)
                .NotEmpty().WithMessage(ErrorMessage.EmptyError)
                //.Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[.,*?_#$%!=¡?)(/\\-+])(?=.*[A-Z]).{8,50}$").WithMessage(ErrorMessage.MatchError)
                .WithName("Password");
        }
    }
}
