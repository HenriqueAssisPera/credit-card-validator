# Credit Card Validator API

API REST para validação e cadastro de cartões de crédito, com identificação de bandeira via algoritmo Luhn.

## 🛠️ Tecnologias

- .NET 8 / C# 12
- SQL Server 2022
- RabbitMQ 3 (com Management UI)
- MassTransit 8 (publisher + consumer)
- MediatR (padrão Command/Handler)
- FluentValidation
- Entity Framework Core 8
- Docker & Docker Compose
- xUnit + FluentAssertions (TDD)

## 🚀 Como Executar via Docker

### Pré-requisitos
- [Docker](https://www.docker.com/) instalado e em execução

## Subir a aplicação

- Abra o Docker Desktop e aguarde ele iniciar (ícone fica verde)
- Abra qualquer terminal (PowerShell, CMD, Terminal do Windows)

- Navegue até a raiz do projeto
- cd C:\...[caminho de pastas até a raiz do projeto]

- Suba os containers
- docker-compose up -d --build

- Acessar API
- http://localhost:5000/swagger

- Para parar tudo:
- docker-compose down


## 📡 Endpoints
- POST /api/cards
- GET /api/health

## 🐇 Mensageria (RabbitMQ + MassTransit)

### Fluxo
1. O endpoint `POST /api/cards` valida e persiste o cartão no banco.
2. Após a persistência bem-sucedida, o handler publica um evento `CardRegisteredEvent` no RabbitMQ via MassTransit.
3. O `CardRegisteredConsumer` (na mesma aplicação) consome o evento e registra um log estruturado com os dados do cartão.

### Verificando o funcionamento
1. Suba os containers com `docker-compose up -d --build`.
2. Cadastre um cartão válido via Swagger (`POST /api/cards`).
3. Verifique os logs do container da API:
   - `docker logs creditcard-api`
   - Procure por: `Event consumed — CardId: ...`
4. Acesse o painel do RabbitMQ em http://localhost:15672:
   - Na aba **Queues**, a fila `card-registered-event` estará listada.
   - A coluna **Messages (Ack)** mostra quantas mensagens foram consumidas com sucesso.

## 🧪 Testes
- Os testes rodam fora do Docker, com o .NET SDK instalado:
- Rodar na raiz do projeto de tests:
	- dotnet test
- cenários exigidos pelo desafio técnico
- identificação de bandeira
- validação por Luhn
- cenários de entrada inválida
- regras de validação da API

- cenários de entrada:
	- cartão válido
	- cartão inválido por Luhn
	- cartão com bandeira desconhecida
	- nome vazio
	- número do cartão nulo ou vazio
	- número do cartão com caracteres não numéricos
	- data de nascimento futura

## Decisões técnicas
- Utilização do padrão Command/Handler com MediatR para separar a lógica de negócios da camada de apresentação.
- Validação de entrada com FluentValidation para garantir que os dados recebidos estejam corretos antes de processar a lógica de negócios.
- Uso do Entity Framework Core para facilitar o acesso ao banco de dados e a manipulação de entidades.
- Docker para containerização da aplicação, garantindo consistência entre ambientes de desenvolvimento e produção.
- xUnit e FluentAssertions para testes unitários, seguindo a abordagem TDD para garantir a qualidade do código e a cobertura de testes.
- Implementação do algoritmo de Luhn para validação de cartões de crédito, garantindo que apenas números válidos sejam processados.
- Identificação de bandeira do cartão com base nos padrões de número, permitindo categorizar os cartões corretamente.
- MassTransit com RabbitMQ para emissão de evento assíncrono após cadastro bem-sucedido, desacoplando a notificação da persistência.

## Limitações Atuais
- Para manter foco no escopo principal do desafio, alguns pontos não foram aprofundados nesta entrega:
	- Autenticação e autorização: A API não possui mecanismos de segurança implementados, como autenticação JWT ou OAuth.
	- ausência de autenticação/autorização
	- ausência de rate limiting
	- ausência de verificação de duplicidade de cartão
	- ausência de idempotência para múltiplos envios da mesma requisição
	- persistência do número do cartão sem mascaramento/tokenização
	- a publicação do evento ocorre após o SaveChanges — em caso de falha na publicação, o cartão é salvo mas o evento pode ser perdido (sem Outbox Pattern)

## Melhorias futuras
- Possíveis evoluções para o projeto:
	- Implementação de autenticação e autorização para proteger os endpoints da API
	- hash/tokenização do número do cartão
	- mascaramento de dados sensíveis
	- verificação de duplicidade
	- idempotência no cadastro
	- rate limiting
	- observabilidade com logs estruturados e tracing
	- Outbox Pattern (MassTransit + EF Core) para garantir consistência entre persistência e publicação de evento
	- consumidores adicionais para auditoria e integração
	- externalização total de segredos e configurações por ambiente