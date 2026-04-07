# Weather App

Aplicação full-stack para registro e consulta de temperaturas por cidade ou coordenadas geográficas. Usuários autenticados registram leituras de temperatura e visualizam o histórico dos últimos 30 dias em lista e gráfico interativo.

---

## Sumário

- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Configuração e execução](#configuração-e-execução)
- [Variáveis de ambiente](#variáveis-de-ambiente)
- [API Reference](#api-reference)
- [Testes](#testes)
- [Estrutura do projeto](#estrutura-do-projeto)

---

## Tecnologias

**Backend**
- .NET 8 / C# — REST API
- Clean Architecture com CQRS + MediatR
- Entity Framework Core + PostgreSQL
- JWT Bearer Authentication (HMAC-SHA256)
- Rate Limiting nativo do ASP.NET Core
- xUnit + Moq + Testcontainers

**Frontend**
- Vue 3 + TypeScript + Vite
- Pinia (state management)
- Vue Router 4 (history mode com guards de autenticação)
- Axios com interceptor JWT
- Chart.js (gráfico de histórico)
- Tailwind CSS

**Infraestrutura**
- Docker + Docker Compose
- Nginx (serve SPA + proxy reverso para `/api/`)
- Multi-stage builds (SDK → runtime, Node → Nginx)

---

## Arquitetura

### Backend — Clean Architecture

```
┌─────────────────────────────────────────────────────┐
│  WeatherApp.API          Controllers, Swagger, DI   │
│           │                                         │
│  WeatherApp.Application  Commands, Queries, Handlers│
│           │                                         │
│  WeatherApp.Domain       Entities, Interfaces       │
│           │                                         │
│  WeatherApp.Infrastructure  EF Core, Repositories,  │
│                             JwtService, Providers   │
└─────────────────────────────────────────────────────┘
```

- **Domain** não possui nenhuma dependência externa — somente entidades e contratos.
- **Application** orquestra os casos de uso via MediatR; nunca acessa infraestrutura diretamente.
- **Infrastructure** implementa os contratos do Domain: repositórios EF Core, `JwtService` (BCrypt + JWT), `OpenWeatherProvider` e `FakeWeatherProvider`.
- **API** é uma camada fina: valida o request, despacha ao MediatR e devolve a resposta HTTP.
- As **migrações são aplicadas automaticamente** na inicialização do container via `db.Database.MigrateAsync()`.

### Frontend — Vue 3 SPA

```
Router → Guard de autenticação
  ├── LoginView / RegisterView     (públicas)
  └── HomeView / HistoryView       (protegidas — exigem token)
        └── Pinia Stores
              ├── authStore        token JWT persiste em localStorage
              └── temperatureStore leitura atual + histórico
                    └── api.ts / authApi.ts
                          └── Axios (interceptor injeta Bearer token)
```

O Nginx proxy o tráfego de `/api/` para o container da API, eliminando CORS em produção. A URL da API é injetada em tempo de build via `ARG VITE_API_URL`.

---

## Pré-requisitos

- Docker >= 24
- Docker Compose >= 2
- Chave gratuita do [OpenWeatherMap](https://openweathermap.org/api) *(opcional — há um provider fake para desenvolvimento)*

---

## Configuração e execução

### 1. Clone o repositório

```bash
git clone https://github.com/andredias20/desafio-fullstack.git
cd desafio-fullstack
```

### 2. Configure as variáveis de ambiente

Crie um arquivo `.env` na raiz do projeto:

```env
# JWT — obrigatório, mínimo 32 bytes
JWT_SECRET=troque-por-uma-chave-secreta-longa-e-aleatoria

# Provider de clima
# Use "fake" para desenvolvimento sem chave de API
# Use "openweather" para dados reais
USE_PROVIDER=fake
OPENWEATHER_API_KEY=

# Banco de dados (valores padrão funcionam localmente)
POSTGRES_USER=weather
POSTGRES_PASSWORD=weather123
POSTGRES_DB=weatherdb
```

> **`JWT_SECRET` é obrigatório.** A aplicação rejeita a inicialização se a chave tiver menos de 32 bytes.

### 3. Suba os serviços

```bash
docker compose up -d --build
```

O Compose sobe três serviços em ordem: `db` → `api` (aguarda health check do banco) → `frontend` (aguarda health check da API).

### 4. Acesse

| Serviço | URL |
|---------|-----|
| Frontend | http://localhost |
| Swagger (API) | http://localhost:5000/swagger |
| Health check | http://localhost:5000/health |

## Variáveis de ambiente

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| `JWT_SECRET` | Chave HMAC-SHA256 para assinar tokens (≥ 32 bytes) | **obrigatório** |
| `JWT_ISSUER` | Claim `iss` do token | `weatherapp` |
| `JWT_AUDIENCE` | Claim `aud` do token | `weatherapp-frontend` |
| `USE_PROVIDER` | `openweather` ou `fake` | `fake` |
| `OPENWEATHER_API_KEY` | Chave da API OpenWeatherMap | — |
| `POSTGRES_USER` | Usuário do PostgreSQL | `weather` |
| `POSTGRES_PASSWORD` | Senha do PostgreSQL | `weather123` |
| `POSTGRES_DB` | Nome do banco | `weatherdb` |
| `VITE_API_URL` | URL da API (injetada no build do frontend) | `http://localhost:5000` |

---

## API Reference

### Autenticação

> Endpoints públicos com **rate limiting**: 10 requisições por minuto por IP. Excedido, retorna `429 Too Many Requests`.

#### `POST /api/auth/register`

Cria uma nova conta de usuário.

```json
// Request body
{ "email": "usuario@email.com", "password": "senha123" }

// 201 Created — sem body
// 409 Conflict — e-mail já cadastrado
```

#### `POST /api/auth/login`

Autentica e retorna o token JWT.

```json
// Request body
{ "email": "usuario@email.com", "password": "senha123" }

// 200 OK
{ "token": "eyJhbGciOiJIUzI1NiIs..." }

// 401 Unauthorized — credenciais inválidas
```

---

### Temperatura

> Todos os endpoints abaixo exigem o header:
> ```
> Authorization: Bearer <token>
> ```

#### `POST /api/temperature/city`

Consulta a temperatura atual da cidade na API de clima e persiste o registro.

```json
// Request body
{ "cityName": "São Paulo" }

// 201 Created
{ "temperatureCelsius": 22.5 }
```

#### `POST /api/temperature/coordinates`

Consulta e registra por latitude e longitude.

```json
// Request body
{ "latitude": -23.5505, "longitude": -46.6333 }

// 201 Created
{ "temperatureCelsius": 22.5 }
```

#### `GET /api/temperature/history`

Retorna os registros dos últimos 30 dias. Filtro por cidade **ou** coordenadas.

```
GET /api/temperature/history?cityName=São Paulo
GET /api/temperature/history?latitude=-23.5505&longitude=-46.6333
```

```json
// 200 OK
[
  {
    "id": "3fa85f64-...",
    "cityName": "São Paulo",
    "latitude": -23.5505,
    "longitude": -46.6333,
    "temperatureCelsius": 22.5,
    "recordedAt": "2026-04-07T10:30:00Z"
  }
]
```

---

### Health

#### `GET /health`

```json
// 200 OK
{ "status": "Healthy" }
```

A documentação interativa completa (Swagger UI) está disponível em `/swagger`.

---

## Testes

O projeto possui testes unitários e de integração.

```bash
cd backend

# Todos os testes (integração requer Docker — sobe PostgreSQL via Testcontainers)
dotnet test

# Somente testes unitários (sem Docker)
dotnet test --filter "Category!=Integration"
```

**Cobertura:**

| Camada | Tipo | Ferramentas |
|--------|------|-------------|
| Handlers (Application) | Unitário | xUnit + Moq |
| Controllers (API) | Unitário | xUnit + Moq |
| Repositórios (Infrastructure) | Integração | xUnit + Testcontainers + PostgreSQL real |

---

## Estrutura do projeto

```
weather-app/
├── backend/
│   ├── WeatherApp.API/
│   │   ├── Controllers/         AuthController, TemperatureController
│   │   ├── Requests/            DTOs de entrada (request bodies)
│   │   ├── Queries/             Query params do histórico
│   │   └── Program.cs           Configuração, DI, middlewares
│   ├── WeatherApp.Application/
│   │   ├── Commands/            RegisterUser, Login, RegisterTemperature*
│   │   ├── Handlers/            Um handler por Command/Query
│   │   ├── Queries/             GetTemperatureHistoryQuery
│   │   └── DTOs/                TemperatureRecordDto
│   ├── WeatherApp.Domain/
│   │   ├── Entities/            User, TemperatureRecord
│   │   ├── Interfaces/          IUserRepository, ITemperatureRepository,
│   │   │                        IWeatherProvider, IJwtService
│   │   └── Models/              WeatherResult
│   ├── WeatherApp.Infrastructure/
│   │   ├── Persistence/         AppDbContext, repositórios EF Core,
│   │   │                        Configurations, Migrations
│   │   ├── Services/            JwtService (BCrypt + JWT)
│   │   └── WeatherProviders/    OpenWeatherProvider, FakeWeatherProvider
│   └── WeatherApp.Tests/
│       ├── Unit/                Testes de handlers e controllers
│       └── Integration/         Testes de repositório com banco real
├── frontend/
│   ├── src/
│   │   ├── views/               LoginView, RegisterView, HomeView, HistoryView
│   │   ├── components/          SearchForm, TemperatureCard, HistoryList, HistoryChart
│   │   ├── stores/              authStore, temperatureStore (Pinia)
│   │   ├── services/            api.ts, authApi.ts (Axios)
│   │   ├── types/               Interfaces TypeScript
│   │   └── router/              index.ts com navigation guards
│   └── Dockerfile               node:20-alpine (build) → nginx:alpine (serve)
├── docker-compose.yml
├── nginx.conf                   SPA fallback + proxy reverso /api/
└── README.md
```
