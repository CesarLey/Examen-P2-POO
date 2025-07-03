# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia los archivos de la solución y restaura dependencias
COPY ["EducationalCoursesAPI.API/EducationalCoursesAPI.API.csproj", "EducationalCoursesAPI.API/"]
COPY ["EducationalCoursesAPI.Application/EducationalCoursesAPI.Application.csproj", "EducationalCoursesAPI.Application/"]
COPY ["EducationalCoursesAPI.Domain/EducationalCoursesAPI.Domain.csproj", "EducationalCoursesAPI.Domain/"]
COPY ["EducationalCoursesAPI.Infrastructure/EducationalCoursesAPI.Infrastructure.csproj", "EducationalCoursesAPI.Infrastructure/"]
RUN dotnet restore "EducationalCoursesAPI.API/EducationalCoursesAPI.API.csproj"

# Copia el resto del código y publica
COPY . .
WORKDIR "/src/EducationalCoursesAPI.API"
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "EducationalCoursesAPI.API.dll"] 