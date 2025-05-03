using HomeFinance2.Application.Interfaces;
using MediatR;

namespace HomeFinance2.Application.ApiService.CQRS.Commands.RegisterFinance
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
            await _financeService.PublishFinanceInTopic(request.financeDto);
            return true;
        }
    }
}
