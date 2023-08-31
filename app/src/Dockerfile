FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /src
COPY ./ /src

RUN dotnet restore "TesteTecnicoDigitas.sln"
RUN dotnet build "TesteTecnicoDigitas.sln"

FROM build as publish
RUN dotnet publish "TesteTecnicoDigitas.sln" -c Release -o /app

FROM base AS final
WORKDIR /app
copy --from=publish /app .

RUN addgroup --system dotnet-runner
RUN adduser --system dotnet-runner --ingroup dotnet-runner
RUN chown -R dotnet-runner:dotnet-runner /app

ENTRYPOINT ["dotnet", "TesteTecnicoDigitas.dll"]