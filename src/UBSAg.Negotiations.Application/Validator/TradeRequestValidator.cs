using FluentValidation;
using UBSAg.Negotiations.Application.Requests;

namespace UBSAg.Negotiations.Application.Validator
{
    public class TradeRequestValidator : AbstractValidator<TradeRequest>
    {
        public TradeRequestValidator()
        {
            RuleFor(x => x.Value)
                .GreaterThan(0)
                .WithMessage("O valor do trade deve ser maior que zero.");

            RuleFor(x => x.ClientSector)
                .NotEmpty()
                .WithMessage("O setor do cliente não pode ser nulo ou vazio.")
                .MinimumLength(3)
                .WithMessage("O setor do cliente deve ter no mínimo 3 caracteres.");
        }
    }
}
