namespace HomeFinance2.Application.Interfaces;

public interface IAwsService
{
    Task PublishToTopic(string message);
}