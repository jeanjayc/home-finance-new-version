using HomeFinance2.Application.FinanceService.DTO;

namespace HomeFinance2.Application.FinanceService.Interfaces
{
    public interface IFinanceService
    {
        Task<FinanceDTO> GetFinanceById(Guid id);
        Task<IEnumerable<FinanceDTO>> GetAll();

        Task CreateFinance(FinanceDTO financeDTO);
        Task UpdateFinance(Guid id, FinanceDTO financeDTO);
        Task DeleteFinance(Guid id);
    }
}
