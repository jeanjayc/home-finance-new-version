using HomeFinance2.Application.FinanceService.Interfaces;
using MediatR;

namespace HomeFinance2.Application.FinanceService.CQRS.Commands.RegisterFinance
{
    public class RegisterFinanceCommandHandler : IRequestHandler<RegisterFinanceCommand, bool>
    {
        private readonly IFinanceService _financeService;

        public RegisterFinanceCommandHandler(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<bool> Handle(RegisterFinanceCommand request, CancellationToken cancellationToken)
        {
            await _financeService.CreateFinance(request.finance);
            return true;
        }
    }
}
