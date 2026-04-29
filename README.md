# Sistema de Cadastro - Serviço de Notificação

Projeto desenvolvido para a disciplina, baseado na integração entre um sistema de cadastro e um serviço de notificação simulado.

## Sobre o sistema

Quando um novo usuário é cadastrado, a API gera automaticamente uma notificação para ele e salva tudo no banco SQLite. O simulador fica gerando cadastros de forma aleatória a cada 2 segundos, e a interface WPF permite visualizar os cadastros e as notificações em tempo real.

## Projetos

- **ApiCadastro** - API REST que recebe os cadastros e gera as notificações (persiste em SQLite via EF Core)
- **CadastroSimulator** - App console que simula cadastros automaticos (salva localmente em simulator_log.db)
- **CadastroInterface** - Interface WPF para visualizar os dados (salva localmente em interface_log.db)
- **Shared** - Modelos de dados compartilhados entre os projetos

## Como rodar

1. Abrir a solução no Visual Studio
2. Rodar o projeto **ApiCadastro** (o Swagger vai abrir no navegador, banco criado automaticamente)
3. Rodar o **CadastroSimulator** em outro terminal
4. Rodar o **CadastroInterface** e clicar em Atualizar para ver os dados

## Endpoints da API

| Método | Rota | Descrição |
|---|---|---|
| POST | /api/v1/cadastros | cria um novo cadastro e gera notificacao |
| GET | /api/v1/cadastros | lista todos os cadastros |
| GET | /api/v1/cadastros/{id} | busca por id |
| PUT | /api/v1/cadastros/{id} | atualiza um cadastro |
| DELETE | /api/v1/cadastros/{id} | remove um cadastro |
| GET | /api/v1/notificacoes | lista todas as notificacoes |
| GET | /api/v1/notificacoes/por-cadastro/{id} | notificacoes de um cadastro |
