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

# 1. Abra o Docker Desktop e aguarde ele iniciar (ícone fica verde)

# 2. Abra qualquer terminal (PowerShell, CMD, Terminal do Windows)

# 3. Navegue até a raiz do projeto
 - cd C:\...[caminho de pastas até a raiz do projeto]

# 4. Suba os containers
- docker-compose up -d --build

# 5. Acessar API
- http://localhost:5000/swagger

# 6. Para parar tudo:
- docker-compose down


## 📡 Endpoints
- POST /api/cards
- GET /api/health

## 🧪 Executar Testes
- Os testes rodam fora do Docker, com o .NET SDK instalado:
- Rodar na raiz do projeto de tests:
- dotnet test



