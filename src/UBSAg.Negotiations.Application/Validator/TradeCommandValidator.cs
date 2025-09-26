using FluentValidation;
using UBSAg.Negotiations.Application.Commands;

namespace UBSAg.Negotiations.Application.Validator
{
    public class TradeCommandValidator : AbstractValidator<TradeCommand>
    {
        public TradeCommandValidator() 
        {
            RuleFor(x => x.Trades)
                .NotNull()
                .WithMessage("A lista de trades não pode ser nula.");

            RuleFor(x => x.Trades)
                .NotEmpty()
                .WithMessage("A lista de trades não pode ser vazia.");

            RuleForEach(x => x.Trades).SetValidator(new TradeRequestValidator());

        }
    }
}
