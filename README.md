# Credit Card Validator API

API REST para validação e cadastro de cartões de crédito, com identificação de bandeira via algoritmo Luhn.

## 🛠️ Tecnologias

- .NET 8 / C# 12
- SQL Server 2022
- MediatR (padrão Command/Handler)
- FluentValidation
- Entity Framework Core 8
- Docker & Docker Compose
- xUnit + FluentAssertions (TDD)

## 🚀 Como Executar via Docker

### Pré-requisitos
- [Docker](https://www.docker.com/) instalado e em execução

### Subir a aplicação
- docker-compose up -d --build
- A API estará disponível em: `http://localhost:5000/swagger`

### Parar os containers
- docker-compose down

## 📡 Endpoints
- POST /api/cards
- GET /api/health

## 🧪 Executar Testes
- docker-compose exec api dotnet test



