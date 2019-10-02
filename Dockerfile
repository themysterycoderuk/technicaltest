FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# Copy all source files/directories under /app
COPY . .

# Build web application
WORKDIR /app/WebSolution
ENV ASPNETCORE_ENVIRONMENT Production
RUN dotnet publish -c Release -o out

# Compose the oputput of this build with a .NET Core 2.2 runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/WebSolution/out ./
RUN ls
ENTRYPOINT ["dotnet", "WebSolution.dll"]
