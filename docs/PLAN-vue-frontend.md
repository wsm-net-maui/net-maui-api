# 🗺️ PLAN-vue-frontend: Painel Administrativo com Vue.js e PrimeVue

## 1. Contexto
- **Objetivo**: Criar um front-end administrativo (Dashboard/Painel) para gerenciar dados da aplicação, consumindo a API .NET existente.
- **Stack Escolhida**: Vue.js (Vue 3, Composition API) + PrimeVue (Framework de componentes UI modernos).
- **Agentes Envolvidos**: `@project-planner`, `@frontend-specialist`.

## 2. 🛑 Portão Socrático (Perguntas em Aberto)
> [!IMPORTANT]
> **Ação Necessária:** Por favor, responda a estas perguntas para que possamos refinar o plano final antes de iniciar a escrita do código.

1. **Autenticação:** A sua API .NET já possui endpoints para geração de tokens JWT? Precisaremos implementar controle de acesso no painel (ex: o que o Admin vê vs. o que um Funcionário vê)?
sim

2. **Módulos Iniciais:** Quais serão as primeiras telas a serem desenvolvidas para termos o "MVP"? (Ex: Dashboard geral, CRUD de Serviços, CRUD de Funcionários, Agenda de Horários)?
CRUD de Serviços, CRUD de Funcionários, Agenda de Horários

3. **Localização do Projeto:** Devemos criar este novo projeto front-end dentro da mesma pasta raiz da API (ex: `net-maui-api/Wsm.Frontend`) ou ele ficará em um repositório totalmente separado?
separado

4. **Framework Base:** Você prefere usar **Vite + Vue 3 SPA** (mais simples e direto, excelente para painéis) ou **Nuxt.js** (framework mais opinativo com rotas automáticas)? Recomendo o Vite para este caso.
Vite + Vue 3 SPA

## 3. Fases de Implementação (Proposta Inicial)

### Fase 1: Setup e Arquitetura Base
- Inicializar o projeto Vue 3 com Vite.
- Instalar e configurar o PrimeVue (temas, ícones, Tailwind/CSS Base).
- Configurar biblioteca de requisições HTTP (Axios) com a URL base da API .NET.
- Configurar o `vue-router` para navegação.

### Fase 2: Layout Base e Autenticação
- Implementar a tela de Login (`/login`).
- Desenvolver o Layout Administrativo principal (Sidebar de navegação, Topbar, Main Content).
- Lógica de autenticação: armazenar token, interceptors do Axios para injetar o header `Authorization`.

### Fase 3: Implementação dos Módulos Core
- **Módulo de Funcionários:** Listagem em Data Table (PrimeVue), modais de Criação, Edição, Exclusão.
- **Módulo de Serviços:** Integração com a API baseada nos seus repositórios (`IServicoRepository`).
- **Horários de Atendimento:** Interface para visualização e gerenciamento de horários.

### Fase 4: Refinamento, UX e Testes
- Adicionar validações nos formulários (ex: VeeValidate/Zod).
- Adicionar feedback global de sucesso/erro (Toast messages).
- Teste de integração do Front com a API local, resolvendo possíveis problemas de CORS.

## 4. Checklist de Verificação (Pre-Deploy)
- [ ] O projeto inicializa e compila corretamente?
- [ ] PrimeVue renderiza os componentes no padrão estético desejado?
- [ ] O login se comunica com sucesso com a API .NET?
- [ ] As rotas privadas estão devidamente protegidas?

## Próximos Passos
Após responder às perguntas acima e validarmos os detalhes, inicie a implementação.
