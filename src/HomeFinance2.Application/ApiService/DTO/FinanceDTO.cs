namespace HomeFinance2.Application.FinanceService.DTO
{
    public record FinanceDTO(string name,string description, DateTime dueDate, decimal amount);
}
