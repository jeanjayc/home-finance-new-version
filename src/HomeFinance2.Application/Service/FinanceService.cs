using System.Text.Json;
using HomeFinance2.Application.FinanceService.DTO;
using HomeFinance2.Application.Interfaces;
using HomeFinance2.Domain.Entities;
using HomeFinance2.Domain.Interfaces;

namespace HomeFinance2.Application.Service;

public class FinanceService : IFinanceService
{
    private readonly IFinancesRepository _repository;
    private readonly IAwsService _awsService;

    public FinanceService(IFinancesRepository repository, IAwsService awsService)
    {
        _repository = repository;
        _awsService = awsService;
    }

    public Task<FinanceDTO> GetFinanceById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FinanceDTO>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task CreateFinance(FinanceDTO financeDTO)
    {
        try
        {
            var finance = new Finance(financeDTO.name, financeDTO.description, financeDTO.amount, 0);
            await _repository.Create(finance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task PublishFinanceInTopic(string financeDTO)
    {
        if (string.IsNullOrWhiteSpace(financeDTO))
            throw new ArgumentNullException(nameof(financeDTO));
        
        await _awsService.PublishToTopic(financeDTO);
    }

    public Task UpdateFinance(Guid id, FinanceDTO financeDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFinance(Guid id)
    {
        throw new NotImplementedException();
    }
}