using FluentValidation;

namespace OrderGenerator.Domain.NewOrder
{
    public class NewOrderRequestValidator : AbstractValidator<NewOrderRequest>
    {
        private readonly List<string> symbols = ["PETR4", "VALE3", "VIIA4"];

        public NewOrderRequestValidator()
        {
            RuleFor(x => x.Symbol)
                .NotEmpty().WithMessage(ErrorMessages.EmptyValue)
                .Must(x => symbols.Contains(x) ).WithMessage(ErrorMessages.InvalidSymbol);

            RuleFor(x => x.Side)
                .NotEmpty().WithMessage(ErrorMessages.EmptyValue)
                .Must(x => x == '1' || x == '2' ).WithMessage(ErrorMessages.InvalidOp);

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage(ErrorMessages.EmptyValue)
                .LessThanOrEqualTo(1000).WithMessage(ErrorMessages.Exceeded1000)
                .GreaterThanOrEqualTo((decimal)0.01).WithMessage(ErrorMessages.GreaterThan001);
            
            RuleFor(x => x.OrderQty)
                .NotEmpty().WithMessage(ErrorMessages.EmptyValue)
                .LessThanOrEqualTo(100000).WithMessage(ErrorMessages.Exceeded100000)
                .GreaterThanOrEqualTo(0).WithMessage(ErrorMessages.GreaterThan0);
        }

    }
    public static class ErrorMessages
    {
        public static string EmptyValue = "O campo não pode ser vazio";
        public static string InvalidSymbol = "Symbol inválido";
        public static string Exceeded100000 = "O valor não pode exceder 100000";
        public static string Exceeded1000 = "O valor não pode exceder 1000";
        public static string GreaterThan0 = "O valor precisa ser maior que 0";
        public static string GreaterThan001 = "O valor precisa ser maior que 0.01";
        public static string InvalidOp = "Operação invalida";
    }
}
