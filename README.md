# Weather App

Aplicação full-stack para consulta e registro de temperaturas por cidade ou coordenadas geográficas, com visualização de histórico.

## Tecnologias

- **Backend:** .NET 8 (C#), Clean Architecture, EF Core, PostgreSQL
- **Frontend:** Vue 3 + TypeScript, Vite, Pinia, Chart.js
- **Infra:** Docker, docker-compose, Nginx

## Pré-requisitos

- Docker >= 24 e Docker Compose >= 2
- Chave de API do [OpenWeatherMap](https://openweathermap.org/api) (gratuita)

## Como executar

1. Clone o repositório:
```bash
   git clone https://github.com/seu-usuario/weather-app.git
   cd weather-app
```

2. Crie o arquivo de variáveis de ambiente:
```bash
   cp .env.example .env
```

3. Preencha sua chave no `.env`:
```env
   OPENWEATHER_API_KEY=sua_chave_aqui
```

4. Suba os serviços:
```bash
   docker compose up -d --build
```

5. Acesse:
   - Frontend: http://localhost:80
   - API + Swagger: http://localhost:5000/swagger
   - Health check: http://localhost:5000/health

## Variáveis de ambiente

| Variável | Descrição | Padrão |
|---|---|---|
| `OPENWEATHER_API_KEY` | Chave da API OpenWeatherMap | — |
| `USE_PROVIDER` | `OpenWeather` ou `Fake` | `OpenWeather` |
| `POSTGRES_USER` | Usuário do banco | `weather` |
| `POSTGRES_PASSWORD` | Senha do banco | `weather123` |
| `POSTGRES_DB` | Nome do banco | `weatherdb` |
| `VITE_API_URL` | URL da API para o frontend | `http://localhost:5000` |

> Para desenvolvimento sem chave de API, use `USE_PROVIDER=Fake` no `.env`.

## Endpoints principais

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/temperature/city` | Registra temperatura por cidade |
| `POST` | `/api/temperature/coordinates` | Registra temperatura por lat/lon |
| `GET` | `/api/temperature/history` | Histórico dos últimos 30 dias |
| `GET` | `/health` | Health check da API |

Documentação completa disponível no Swagger em `/swagger`.

## Executar testes
```bash
# Testes unitários e de integração
docker compose run --rm api dotnet test

# Ou localmente (requer .NET 8 SDK)
cd backend
dotnet test
```

## Estrutura do projeto

weather-app/
├── backend/
│   ├── WeatherApp.Domain/        # Entidades e interfaces
│   ├── WeatherApp.Application/   # CQRS, casos de uso
│   ├── WeatherApp.Infrastructure/# EF Core, providers externos
│   ├── WeatherApp.API/           # Controllers, Swagger, DI
│   └── WeatherApp.Tests/         # Testes unitários e integração
├── frontend/                     # Vue 3 + TypeScript
├── .env.example
├── docker-compose.yml
└── README.md