using HomeFinance2.Application.FinanceService.DTO;
using MediatR;

namespace HomeFinance2.Application.FinanceService.CQRS.Commands.RegisterFinance
{
    public record RegisterFinanceCommand(FinanceDTO finance) : IRequest<bool>
    {
    }
}
