# Implementação Módulo Barbearia (Híbrido)

Este plano descreve a implementação do módulo de barbearia usando a Opção C (Abordagem Híbrida) e incluindo o controle de horários disponíveis, seguindo a Clean Architecture do projeto.

## Tipo de Projeto
WEB / BACKEND (API .NET MAUI Backend)

## Critérios de Sucesso
- Entidades criadas corretamente no domínio, estendendo e se relacionando com a entidade base `Usuario`.
- Tabela para controle de horários disponíveis.
- Mapeamento completo no Entity Framework Core.
- Serviços e Controladores (CRUD) funcionais para FuncionarioPerfil, ClientePerfil, Servico e Horarios.

## Tech Stack
- .NET 10 C# (Domínio, Aplicação e API)
- Entity Framework Core (Mapeamento Relacional e Migrações)

## Estrutura de Arquivos

### [Componente] Wsm.Domain
#### [NEW] Wsm.Domain/Entities/FuncionarioPerfil.cs
#### [NEW] Wsm.Domain/Entities/ClientePerfil.cs
#### [NEW] Wsm.Domain/Entities/Servico.cs
#### [NEW] Wsm.Domain/Entities/HorarioAtendimento.cs

### [Componente] Wsm.Infra.Estrutura
#### [MODIFY] Wsm.Infra.Estrutura/Data/ApplicationDbContext.cs
#### [NEW] Wsm.Infra.Estrutura/Data/Configurations/FuncionarioPerfilConfiguration.cs
#### [NEW] Wsm.Infra.Estrutura/Data/Configurations/ClientePerfilConfiguration.cs
#### [NEW] Wsm.Infra.Estrutura/Data/Configurations/ServicoConfiguration.cs
#### [NEW] Wsm.Infra.Estrutura/Data/Configurations/HorarioAtendimentoConfiguration.cs

### [Componente] Wsm.Application
#### [NEW] Wsm.Application/DTOs/Barbearia/... (Vários arquivos de DTO)
#### [NEW] Wsm.Application/Interfaces/Barbearia/... (IServicoService, etc.)
#### [NEW] Wsm.Application/Services/Barbearia/... (Implementação)

### [Componente] Wsm.Api (Controllers)
#### [NEW] Controllers/ServicosController.cs
#### [NEW] Controllers/FuncionariosController.cs
#### [NEW] Controllers/ClientesController.cs
#### [NEW] Controllers/HorariosController.cs

## Divisão de Tarefas (Task Breakdown)

- `[ ]` **Task 1**: Criar Entidades de Domínio (`FuncionarioPerfil`, `ClientePerfil`, `Servico`, `HorarioAtendimento`). 
  - **Agente**: `backend-specialist` | **Skill**: `database-design`
  - **INPUT→OUTPUT→VERIFY**: Input: Contexto atual -> Output: Classes no Domain -> Verify: Não há erros de compilação no Wsm.Domain.
- `[ ]` **Task 2**: Criar Configurações do EF Core e Atualizar `ApplicationDbContext`.
  - **Agente**: `backend-specialist` | **Skill**: `database-design`
  - **INPUT→OUTPUT→VERIFY**: Input: Entidades -> Output: Classes Configuration e DbSets -> Verify: Build com sucesso e DbSets expostos.
- `[ ]` **Task 3**: Criar DTOs e Interfaces na camada de Application.
  - **Agente**: `backend-specialist` | **Skill**: `api-patterns`
  - **INPUT→OUTPUT→VERIFY**: Input: Entidades -> Output: DTOs e Interfaces -> Verify: Build da camada Application passa.
- `[ ]` **Task 4**: Implementar Serviços na camada Application.
  - **Agente**: `backend-specialist` | **Skill**: `clean-code`
  - **INPUT→OUTPUT→VERIFY**: Input: Interfaces -> Output: Classes implementando regras de negócio e CRUD -> Verify: Compila sem erros.
- `[ ]` **Task 5**: Criar Controllers na API.
  - **Agente**: `backend-specialist` | **Skill**: `api-patterns`
  - **INPUT→OUTPUT→VERIFY**: Input: Serviços -> Output: Endpoints REST -> Verify: Build completo e Swagger exibe os novos endpoints.

## Perguntas em Aberto (User Review Required)

> [!IMPORTANT]
> A entidade `HorarioAtendimento` foi pensada para vincular um `FuncionarioPerfil` aos dias da semana e intervalos de hora em que ele atende (ex: Funcionario X atende Seg-Sex das 08:00 às 20:00) com intervalos de 30 em 30 minutos. Ou seja, essa abordagem resolve a necessidade de "controlar os horários"? Ou você imagina uma tabela diferente (por exemplo, horários específicos de agendamento no calendário)?
> **Resposta**: Sim, essa abordagem resolve a necessidade de "controlar os horários". O `HorarioAtendimento` é responsável por definir os dias da semana e os intervalos de hora em que um `FuncionarioPerfil` atende. Isso permite que o sistema controle os horários disponíveis para agendamento.

## Plano de Verificação
- Compilar o projeto (`dotnet build`).
- Gerar a Migration do Entity Framework para aplicar as tabelas no banco de dados.
- Subir a API e testar via Swagger a criação e listagem das entidades.
