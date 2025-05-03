using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using HomeFinance2.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HomeFinance2.Application.Service;

public class AwsService : IAwsService
{
    private IAmazonSimpleNotificationService _client;
    private readonly IConfiguration _configuration;

    public AwsService(IConfiguration configuration)
    {
        _configuration = configuration;
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
}