using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using HomeFinance2.Domain.Entities;
using HomeFinance2.Domain.Interfaces;

namespace HomeFinance2.Infrastructure.Repository;

public class FinanceRepository : IFinancesRepository
{
    private readonly IAmazonDynamoDB _client;
    private readonly string _tableName = "HomeFinanceTable";

    public FinanceRepository(IAmazonDynamoDB client)
    {
        _client = client;
    }

    public Task<Finance> GetFinanceById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Finance>> GetFinances()
    {
        throw new NotImplementedException();
    }

    public async Task Create(Finance finance)
    {
        if (finance is null)
            return;
        
        finance.Pk = "FINANCE#" + Guid.NewGuid().ToString();  
        
        var financeAsJson = JsonSerializer.Serialize(finance);
        var financeAsAttributes = Document.FromJson(financeAsJson)
            .ToAttributeMap();
        var createItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = financeAsAttributes
        };
        
        await _client.PutItemAsync(_tableName, financeAsAttributes);
    }

    public Task Update(Finance finance)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}