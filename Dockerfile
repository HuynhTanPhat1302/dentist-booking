# Set the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Copy the published output of the API project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY DentistBooking.API/DentistBooking.API.csproj DentistBooking.API/
COPY DentistBooking.Application/DentistBooking.Application.csproj DentistBooking.Application/
COPY DentistBooking.Domain/DentistBooking.Domain.csproj DentistBooking.Domain/
COPY DentistBooking.Infrastructure/DentistBooking.Infrastructure.csproj DentistBooking.Infrastructure/
RUN dotnet restore DentistBooking.API/DentistBooking.API.csproj
COPY . .
WORKDIR /src/DentistBooking.API
RUN dotnet publish -c Release -o /app/publish

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DentistBooking.API.dll"]
