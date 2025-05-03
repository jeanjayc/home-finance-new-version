using HomeFinance2.Application.FinanceService.DTO;
using MediatR;

namespace HomeFinance2.Application.ApiService.CQRS.Commands.RegisterFinance
{
    public record RegisterFinanceCommand(string financeDto) : IRequest<bool>
    {
    }
}
