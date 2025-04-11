using CashFlow.Commnication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses;

public class ExpenseValidator : AbstractValidator<RequestExpenseJson>
{
    public ExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage(ResourceErrorsMessages.TITULO_OBRIGATORIO);
        RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage(ResourceErrorsMessages.VALOR_MAIOR_QUE_ZERO);
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorsMessages.DIVIDA_NAO_PODE_SER_DO_FUTURO);
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage(ResourceErrorsMessages.TIPO_PAGAMENTO_INVALIDO);
    }
}
