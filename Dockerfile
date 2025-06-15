FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY /src/Homefinance2.ApiGateway/Homefinance2.ApiGateway.csproj Homefinance2.ApiGateway/
COPY /src/HomeFinance2.Application/HomeFinance2.Application.csproj HomeFinance2.Application/
COPY /src/HomeFinance2.Domain/HomeFinance2.Domain.csproj HomeFinance2.Domain/
COPY /src/HomeFinance2.Infrastructure/HomeFinance2.Infrastructure.csproj HomeFinance2.Infrastructure/

RUN dotnet restore Homefinance2.ApiGateway/Homefinance2.ApiGateway.csproj

COPY . .

WORKDIR /src/Homefinance2.ApiGateway
RUN dotnet publish -c Release -o /app/out --no-restore

FROM public.ecr.aws/lambda/dotnet:8
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 80

CMD [ "Homefinance2.ApiGateway::Homefinance2.ApiGateway.LambdaEntryPoint::FunctionHandlerAsync" ]