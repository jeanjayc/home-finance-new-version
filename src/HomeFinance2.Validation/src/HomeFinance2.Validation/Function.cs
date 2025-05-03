using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.S3;
using HomeFinance2.Application.FinanceService.DTO;
using HomeFinance2.Application.Interfaces;
using HomeFinance2.Application.Service;
using HomeFinance2.Domain.Interfaces;
using HomeFinance2.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Homefinance2.Validation;

public class Function
{
    public Function()
    {
    }

    public async Task<string> FunctionHandler(SNSEvent evnt, ILambdaContext context)
    {
        // Configuração dos serviços
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Obter instância da classe App do contêiner DI
        var app = serviceProvider.GetRequiredService<App>();

        // Logging
        context.Logger.LogInformation($"Processando {evnt.Records.Count} registros SNS.");

        // Processar cada registro SNS
        foreach (var record in evnt.Records)
        {
            await ProcessRecordAsync(record, context, app);
        }

        return $"Processados com sucesso {evnt.Records.Count} registros.";
    }

    private async Task ProcessRecordAsync(SNSEvent.SNSRecord record, ILambdaContext context, App app)
    {
        try
        {
            context.Logger.LogInformation($"Processando registro SNS: {record.Sns.MessageId}");

            var message = record.Sns.Message;
            var result = await app.Run(message);

            context.Logger.LogInformation($"Processamento concluído: {result}");
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Erro ao processar registro: {ex.Message}");
            throw;
        }

        await Task.CompletedTask;
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
        //services.AddAWSService<IAmazonDynamoDB>();
        services.AddAWSService<IAmazonS3>();
        services.AddTransient<App>();
    }

    public class App
    {
        private readonly IFinanceService _financeService;

        public App(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<string> Run(string message)
        {
            // Aqui você pode usar _financeService para processar a mensagem
            Console.WriteLine($"Processando mensagem: {message}");
            var financeDto = JsonSerializer.Deserialize<FinanceDTO>(message);
            await _financeService.CreateFinance(financeDto);
            return "Processamento concluído";
        }
    }
}
