using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using HomeFinance2.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HomeFinance2.Application.Service;

public class AwsService : IAwsService
{
    private IAmazonSimpleNotificationService _client;
    private readonly AmazonSQSClient _sqsClient;
    private readonly IConfiguration _configuration;

    public AwsService(IConfiguration configuration)
    {
        _configuration = configuration;
        _sqsClient = new AmazonSQSClient();
    }

    public async Task PublishToTopic(string message)
    {
        var config = new AmazonSimpleNotificationServiceConfig
        {
            ServiceURL = "http://localhost:4566",/*_configuration["AwsConfig:ServiceUrl"],*/
            AuthenticationRegion = "us-east-1"
        };

        _client = new AmazonSimpleNotificationServiceClient(
            new Amazon.Runtime.BasicAWSCredentials(
                "teste",
                "teste"
                //_configuration["AwsConfig:AccessKey"],
                //_configuration["AwsConfig:SecretKey"]
            ),
            config
        );

        var topicArn = "arn:aws:sns:us-east-1:000000000000:InputDataFinances"/*_configuration["AwsConfig:TopicArn"]*/;

        var request = new PublishRequest
        {
            TopicArn = topicArn,
            Message = message
        };

        var response = await _client.PublishAsync(request);
    }

    public async Task SendMessageToQueue(string message)
    {
        try
        {
            var getQueueUrlResponse = await _sqsClient.GetQueueUrlAsync("finances-process-queue");
            var queueUrl = getQueueUrlResponse.QueueUrl;

            var sendMessage = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = message
            };

            var response = await _sqsClient.SendMessageAsync(sendMessage);
            Console.WriteLine($"Mensagem enviada com sucesso. MessageId: {response.MessageId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar mensagem para SQS: {ex.Message}");
            throw;
        }
    }


    public async Task<ReceiveMessageResponse> GetMessageFromQueueProcessing()
    {
        var receivaMessage = new ReceiveMessageRequest
        {
            QueueUrl = "http://localhost.localstack.cloud:4566/000000000000/finances-process-queue",
            MaxNumberOfMessages = 1,
            WaitTimeSeconds = 2
        };

        var messageReceivabled = await _sqsClient.ReceiveMessageAsync(receivaMessage);
        return messageReceivabled;
    }
}