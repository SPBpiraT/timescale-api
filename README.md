# timescale-api

Web API for processing CSV files, saving and aggregating data.

## Architecture

The application is implemented using a three-layer architecture and divided into layers:

- TimeScale.WebApi (Presentation)
- TimeScale.BLL (Business Logic)
- TimeScale.DAL (DataAccess)

Additionally, a separate assembly (TimeScale.Shared) for utilities used across all layers.

## TechStack
- .NET 8
- EF Core
- PostgreSQL
- Swagger

## Project startup

### Database setup

```sh
# Pull PostgreSQL docker image
docker pull postgres

# Start database container
docker run -itd -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin -p 5432:5432 --name pgcontainer postgres
```

### Start the application

```sh
# Start in Dev mode
dotnet run --launch-profile https
```

Access Swagger UI at: https://localhost:7020/swagger/index.html
