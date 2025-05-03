using System.Reflection;
using Amazon.DynamoDBv2;
using Amazon.S3;
using HomeFinance2.Application.ApiService.CQRS.Commands.RegisterFinance;
using HomeFinance2.Application.Interfaces;
using HomeFinance2.Application.Service;
using HomeFinance2.Domain.Interfaces;
using HomeFinance2.Infrastructure.Repository;
using LocalStack.Client.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalStack(builder.Configuration);
builder.Services.AddDefaultAwsOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterFinanceCommand).Assembly));
builder.Services.AddTransient<IFinancesRepository, FinanceRepository>();
builder.Services.AddTransient<IFinanceService, FinanceService>();
builder.Services.AddTransient<IAwsService, AwsService>();

builder.Services.AddAwsService<IAmazonS3>();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(
        new AmazonDynamoDBConfig()
        {
            ServiceURL = "http://localhost.localstack.cloud:4566",
            AuthenticationRegion = "us-east-1"
        }));
}
else
{
    builder.Services.AddAWSService<IAmazonDynamoDB>();
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
