using Amazon.DynamoDBv2;
using Amazon.S3;
using HomeFinance2.Application.ApiService.CQRS.Commands.RegisterFinance;
using HomeFinance2.Application.Interfaces;
using HomeFinance2.Application.Service;
using HomeFinance2.Domain.Interfaces;
using HomeFinance2.Infrastructure.Repository;
using LocalStack.Client.Extensions;

namespace Homefinance2.ApiGateway;

public class Startup
{
    private readonly IWebHostEnvironment _env;
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        _env = env;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddLocalStack(Configuration);
        services.AddDefaultAwsOptions(Configuration.GetAWSOptions());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterFinanceCommand).Assembly));
        services.AddTransient<IFinancesRepository, FinanceRepository>();
        services.AddTransient<IFinanceService, FinanceService>();
        services.AddTransient<IAwsService, AwsService>();

        services.AddAwsService<IAmazonS3>();
        if (_env.IsDevelopment())
        {
            services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(
                new AmazonDynamoDBConfig()
                {
                    ServiceURL = "http://localhost.localstack.cloud:4566",
                    AuthenticationRegion = "us-east-1"
                }));
        }
        else
        {
            services.AddAWSService<IAmazonDynamoDB>();
        }
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app)
    {
        if (_env.IsDevelopment() || true)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });
    }
}