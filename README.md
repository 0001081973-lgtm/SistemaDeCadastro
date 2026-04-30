# Sistema de Cadastro - Serviço de Notificação

Sistema desenvolvido em C# .NET 8 com API REST, persistência em SQLite e interface WPF.
A integração ocorre entre o sistema de cadastro de usuários e um serviço de notificação simulado.

## Tecnologias utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8 com SQLite
- WPF (Windows Presentation Foundation)
- Swagger / OpenAPI
- Padrão MVVM
- DTO (Data Transfer Object)

## Estrutura do projeto

```
SistemaDeCadastro/
├── Shared/                        → modelos de dados compartilhados
│   └── CadastroData.cs
├── ApiCadastro/                   → API REST principal
│   ├── Config/ApiConfig.cs
│   ├── Controllers/
│   │   ├── CadastroController.cs
│   │   └── NotificacaoController.cs
│   ├── Data/CadastroDbContext.cs
│   ├── DTO/CadastroDTO.cs
│   └── Migrations/
├── CadastroSimulator/             → app console que gera cadastros automaticos
│   └── Program.cs
└── CadastroInterface/             → interface WPF para visualizacao dos dados
    ├── Command/RelayCommand.cs
    ├── DTO/CadastroDTO.cs
    ├── Model/MainViewModel.cs
    └── Views/MainWindow.xaml
```

## Como executar

### Pré-requisitos

- Visual Studio 2022
- .NET 8 SDK
- DB Browser for SQLite (opcional, para visualizar o banco)

### Passo 1 — Clonar o repositório

```bash
git clone https://github.com/SEU_USUARIO/SistemaDeCadastro.git
cd SistemaDeCadastro
```

### Passo 2 — Aplicar as Migrations e criar o banco

Abra o **Package Manager Console** no Visual Studio:

```
Ferramentas → Gerenciador de Pacotes NuGet → Console do Gerenciador de Pacotes
```

Selecione o projeto **ApiCadastro** no seletor e rode:

```
Update-Database
```

O arquivo `cadastro_sistema.db` será criado automaticamente.

### Passo 3 — Iniciar a API

Defina o **ApiCadastro** como projeto de inicialização e clique em **Play**.

O Swagger estará disponível em:

```
https://localhost:44320/swagger
```

### Passo 4 — Iniciar o Simulador

Abra um segundo terminal na pasta do projeto e rode:

```bash
cd CadastroSimulator
dotnet run
```

O simulador vai gerar cadastros automaticamente a cada 2 segundos e enviar para a API.

### Passo 5 — Abrir a Interface WPF

Defina o **CadastroInterface** como projeto de inicialização, clique em **Play** e depois clique no botão **Atualizar** para carregar os dados da API.

---

## Documentação da API

**Base URL:** `https://localhost:44320`  
**Documentação interativa:** `https://localhost:44320/swagger`

---

### Modelo de dados — CadastroDTO

| Campo | Tipo | Obrigatório | Descrição |
|---|---|---|---|
| nome | string | sim | nome completo do usuario |
| email | string | sim | email do usuario |
| telefone | string | não | telefone de contato |
| departamento | string | não | departamento do usuario |

---

## Endpoints — Cadastro

### `POST /api/v1/cadastros`

Cadastra um novo usuario no sistema e salva no banco de dados.

**Corpo da requisição:**
```json
{
  "nome": "Ana Lima",
  "email": "ana.lima@empresa.com",
  "telefone": "(31) 99999-9999",
  "departamento": "TI"
}
```

**Respostas:**

| Código | Descrição |
|---|---|
| 200 | Cadastro realizado com sucesso |
| 400 | Nome ou Email nao podem ser vazios |

**Exemplo de resposta 200:**
```json
{
  "id": 1,
  "nome": "Ana Lima",
  "email": "ana.lima@empresa.com",
  "telefone": "(31) 99999-9999",
  "departamento": "TI",
  "timestamp": "2026-04-29T14:30:00"
}
```

---

### `GET /api/v1/cadastros`

Lista todos os cadastros salvos no banco de dados.

**Respostas:**

| Código | Descrição |
|---|---|
| 200 | Lista retornada com sucesso |

**Exemplo de resposta 200:**
```json
[
  {
    "id": 1,
    "nome": "Ana Lima",
    "email": "ana.lima@empresa.com",
    "telefone": "(31) 99999-9999",
    "departamento": "TI",
    "timestamp": "2026-04-29T14:30:00"
  },
  {
    "id": 2,
    "nome": "Carlos Souza",
    "email": "carlos.souza@empresa.com",
    "telefone": "(31) 98888-8888",
    "departamento": "RH",
    "timestamp": "2026-04-29T14:30:02"
  }
]
```

---

### `GET /api/v1/cadastros/{id}`

Busca um cadastro especifico pelo ID.

**Parâmetros:**

| Parâmetro | Tipo | Descrição |
|---|---|---|
| id | int | ID do cadastro a ser buscado |

**Respostas:**

| Código | Descrição |
|---|---|
| 200 | Cadastro encontrado com sucesso |
| 404 | Cadastro nao encontrado |

**Exemplo de requisição:**
```
GET /api/v1/cadastros/1
```

---

### `PUT /api/v1/cadastros/{id}`

Atualiza os dados de um cadastro existente.

**Parâmetros:**

| Parâmetro | Tipo | Descrição |
|---|---|---|
| id | int | ID do cadastro a ser atualizado |

**Corpo da requisição:**
```json
{
  "nome": "Ana Lima Atualizada",
  "email": "ana.lima@empresa.com",
  "telefone": "(31) 97777-7777",
  "departamento": "Financeiro"
}
```

**Respostas:**

| Código | Descrição |
|---|---|
| 200 | Cadastro atualizado com sucesso |
| 404 | Cadastro nao encontrado |

---

### `DELETE /api/v1/cadastros/{id}`

Remove um cadastro do banco de dados pelo ID.

**Parâmetros:**

| Parâmetro | Tipo | Descrição |
|---|---|---|
| id | int | ID do cadastro a ser removido |

**Respostas:**

| Código | Descrição |
|---|---|
| 200 | Cadastro deletado com sucesso |
| 404 | Cadastro nao encontrado |

---

## Endpoints — Notificacao

### `GET /api/v1/notificacoes`

Lista todas as notificacoes geradas pelo sistema.

**Respostas:**

| Código | Descrição |
|---|---|
| 200 | Lista retornada com sucesso |

**Exemplo de resposta 200:**
```json
[
  {
    "id": 1,
    "cadastroId": 1,
    "nomeDestinatario": "Ana Lima",
    "email": "ana.lima@empresa.com",
    "mensagem": "Bem-vindo(a) Ana Lima! Seu cadastro foi realizado com sucesso.",
    "status": "Enviado",
    "timestamp": "2026-04-29T14:30:00"
  }
]
```

---

### `GET /api/v1/notificacoes/por-cadastro/{cadastroId}`

Busca todas as notificacoes vinculadas a um cadastro especifico.

**Parâmetros:**

| Parâmetro | Tipo | Descrição |
|---|---|---|
| cadastroId | int | ID do cadastro para filtrar as notificacoes |

**Respostas:**

| Código | Descrição |
|---|---|
| 200 | Lista retornada com sucesso |

**Exemplo de requisição:**
```
GET /api/v1/notificacoes/por-cadastro/1
```

---

## Persistência de dados

| Banco | Projeto | Tecnologia |
|---|---|---|
| `cadastro_sistema.db` | ApiCadastro | Entity Framework Core + Migrations |
| `interface_log.db` | CadastroInterface | Microsoft.Data.Sqlite direto |
| `simulator_log.db` | CadastroSimulator | Microsoft.Data.Sqlite direto |
