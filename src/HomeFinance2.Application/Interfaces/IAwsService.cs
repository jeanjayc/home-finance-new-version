using Amazon.SQS.Model;

namespace HomeFinance2.Application.Interfaces;

public interface IAwsService
{
    Task PublishToTopic(string message);
    Task SendMessageToQueue(string message);
    Task<ReceiveMessageResponse> GetMessageFromQueueProcessing();
}