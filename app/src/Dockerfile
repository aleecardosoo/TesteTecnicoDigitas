FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ./ /src

RUN dotnet restore "TesteTecnicoDigitas.Api/TesteTecnicoDigitas.sln"
RUN dotnet build "TesteTecnicoDigitas.Api/TesteTecnicoDigitas.sln"

FROM build as publish
RUN dotnet publish "TesteTecnicoDigitas.Api/TesteTecnicoDigitas.sln" -c Release -o /app

FROM base AS final
WORKDIR /app
copy --from=publish /app .

RUN addgroup --system dotnet-runner
RUN adduser --system dotnet-runner --ingroup dotnet-runner
RUN chown -R dotnet-runner:dotnet-runner /app

ENTRYPOINT ["dotnet", "TesteTecnicoDigitas.Api.dll"]