using HomeFinance2.Domain.Entities;

namespace HomeFinance2.Domain.Interfaces;

public interface IFinancesRepository
{
    Task<Finance> GetFinanceById(Guid id);
    Task<IEnumerable<Finance>> GetFinances();
    Task Create(Finance finance); 
    Task Update(Finance finance);
    Task Delete(Guid id);
}