# FIAP Connect API — Sprint 3

Sistema de formação de grupos acadêmicos para o Challenge Oracle FIAP.

## Integrantes

| Nome | RM | Turma |
|------|----|-------|
| Alexis Rondo | 560384 | 2TDSPS |
| Vinicius Rodrigues de Oliveira | 559611 | 2TDSPS |

---

## O que foi adicionado na Sprint 3

Esta sprint evoluiu a API REST da Sprint 2 adicionando três camadas:

- **Monitoramento e Observabilidade** — Health Checks, Logging com Serilog e Tracing com OpenTelemetry
- **Testes Automatizados** — Testes unitários e de integração com xUnit seguindo o padrão AAA
- **README atualizado** com documentação dos endpoints e instruções de execução

---

## Tecnologias

- .NET 8.0 / C#
- Serilog (logging estruturado)
- OpenTelemetry (distributed tracing)
- Microsoft.Extensions.Diagnostics.HealthChecks
- xUnit + Moq (testes)
- Swagger / OpenAPI

---

## Como executar

### Pré-requisitos
- .NET 8.0 SDK instalado

### Rodando a API

```bash
# Clonar o repositório
git clone https://github.com/Sprint1-Fiap-Connect/fiap-connect-dotnet-sprint3

# Entrar na pasta do projeto
cd fiap-connect-dotnet-sprint3

# Restaurar dependências
dotnet restore

# Executar
dotnet run
```

A API estará disponível em:
- Swagger: `https://localhost:7xxx/swagger`
- Health Check: `https://localhost:7xxx/health/simple`

---

## Endpoints

### Usuário

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/usuario` | Lista todos os usuários |
| GET | `/api/usuario/{id}` | Obtém usuário por ID |
| POST | `/api/usuario` | Cria novo usuário |
| PATCH | `/api/usuario/{id}/status` | Atualiza status de busca |
| DELETE | `/api/usuario/{id}` | Remove usuário |
| GET | `/api/usuario/search` | Busca com paginação e filtros |

**Parâmetros de busca:** `nome`, `curso`, `statusBusca`, `page`, `pageSize`, `orderBy`

**Exemplo POST /api/usuario:**
```json
{
  "rm": "RM12345",
  "nomeCompleto": "João Silva",
  "emailInstitucional": "joao.silva@fiap.com.br",
  "curso": "Análise e Desenvolvimento de Sistemas",
  "periodo": "Noturno",
  "turma": "2TDSPS",
  "statusBusca": true
}
```

### Grupo

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/grupo` | Lista todos os grupos |
| GET | `/api/grupo/{id}` | Obtém grupo por ID |
| POST | `/api/grupo` | Cria novo grupo |
| PUT | `/api/grupo/{id}` | Atualiza grupo |
| DELETE | `/api/grupo/{id}` | Remove grupo |
| GET | `/api/grupo/abertos` | Lista grupos com vagas |
| GET | `/api/grupo/search` | Busca com paginação e filtros |

**Parâmetros de busca:** `nome`, `status`, `page`, `pageSize`

**Exemplo POST /api/grupo:**
```json
{
  "nomeGrupo": "Tech Innovators",
  "descricaoProjeto": "App mobile para gestão acadêmica",
  "disciplinaTema": "Mobile",
  "maxIntegrantes": 3
}
```

### Habilidade

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/habilidade` | Lista todas as habilidades |
| GET | `/api/habilidade/search` | Busca com filtros |

### Solicitação

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/solicitacao` | Cria solicitação para entrar em grupo |
| GET | `/api/solicitacao/{id}` | Obtém solicitação por ID |
| GET | `/api/solicitacao/search` | Busca solicitações com paginação |

---

## Health Checks

A API expõe dois endpoints de health check:

### GET `/health/simple`
Retorna status simplificado em JSON:
```json
{
  "status": "Healthy",
  "timestamp": "2025-04-10T10:00:00Z",
  "checks": [
    { "nome": "api", "status": "Healthy", "descricao": "API funcionando normalmente" },
    { "nome": "memoria", "status": "Healthy", "descricao": "Memória OK: 45MB" }
  ]
}
```

### GET `/health`
Retorna relatório detalhado no formato padrão do HealthChecks.UI.

---

## Logging

Os logs são gerados automaticamente pelo Serilog em dois destinos:

- **Console** — visível durante a execução da aplicação
- **Arquivo** — pasta `logs/` com arquivos diários no formato `fiapconnect-YYYYMMDD.log`

Níveis de log utilizados:
- `Information` — requisições recebidas, operações concluídas com sucesso
- `Warning` — recursos não encontrados, dados inválidos
- `Error` / `Fatal` — erros inesperados e falhas na inicialização

---

## Testes Automatizados

### Estrutura

```
FiapConnect.Tests/
├── Unit/
│   ├── UsuarioControllerTests.cs   (4 testes)
│   └── GrupoControllerTests.cs     (3 testes)
└── Integration/
    └── ApiIntegrationTests.cs      (3 testes)
```

### Como executar os testes

```bash
# Executar todos os testes
dotnet test

# Executar com detalhes
dotnet test --verbosity normal

# Executar apenas testes unitários
dotnet test --filter "FullyQualifiedName~Unit"

# Executar apenas testes de integração
dotnet test --filter "FullyQualifiedName~Integration"
```
