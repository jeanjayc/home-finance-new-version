using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using HomeFinance2.Application.FinanceService.DTO;
using HomeFinance2.Application.Interfaces;
using HomeFinance2.Application.Service;
using HomeFinance2.Domain.Entities;
using HomeFinance2.Domain.Interfaces;
using HomeFinance2.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Homefinance2.Processing;

public class Function
{

    public Function()
    {
    }

    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var financeService = serviceProvider.GetService<IFinanceService>();

        foreach (var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context, financeService);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context, IFinanceService financeService)
    {
        context.Logger.LogInformation($"Iniciando leitura da mensagem da fila");

        var financeDto = JsonSerializer.Deserialize<FinanceDTO>(message.Body);

        await financeService.CreateFinance(financeDto);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddTransient<IFinancesRepository, FinanceRepository>();
        services.AddTransient<IFinanceService, FinanceService>();
        services.AddTransient<IAwsService, AwsService>();
        services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(
            new AmazonDynamoDBConfig()
            {
                ServiceURL = "http://localhost.localstack.cloud:4566",
                AuthenticationRegion = "us-east-1"
            }));
        services.AddSingleton<IAmazonSQS>(_ => new AmazonSQSClient(
            new BasicAWSCredentials("dummy", "dummy"),
            new AmazonSQSConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.USIsoEast1,
                ServiceURL = "http://localhost.localstack.cloud:4566",
                UseHttp = true
            }));
        //services.AddSingleton<IAmazonDynamoDB>();
        services.AddAWSService<IAmazonS3>();
    }
}