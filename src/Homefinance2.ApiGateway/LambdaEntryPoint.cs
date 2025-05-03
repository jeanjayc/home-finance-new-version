namespace Homefinance2.ApiGateway;

/// <summary>
/// This class extends from APIGatewayProxyFunction which contains the method FunctionHandlerAsync which is the 
/// actual Lambda function entry point. The Lambda handler field should be set to
/// 
/// Homefinance2.ApiGateway::Homefinance2.ApiGateway.LambdaEntryPoint::FunctionHandlerAsync
/// </summary>
public class LambdaEntryPoint :


    Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder
            .UseStartup<Startup>();
    }

    protected override void Init(IHostBuilder builder)
    {
    }
}