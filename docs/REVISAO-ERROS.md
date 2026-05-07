# Revisão de Erros — Carona Alvinegra ✅

> **Data:** 07/05/2026
> **Build:** 0 erros, 0 warnings (todos os projetos)
> **Projeto:** [`CaronaAlvinegra.sln`](../CaronaAlvinegra.sln)
> **Status:** ✅ **TODAS AS CORREÇÕES FORAM APLICADAS E VERIFICADAS**

---

## 🔴 Bugs Graves (Impactam o Funcionamento) — ✅ Corrigidos

### 1. `GrupoAppService.AdicionarMembroAsync` não popula a coleção `_membros` do grupo

**Arquivo:** [`GrupoAppService.cs:108`](../src/CaronaAlvinegra.Application/Services/GrupoAppService.cs:108)

**✅ Corrigido:** Adicionada a chamada `grupo.AdicionarMembro(usuario)` logo após `usuario.VincularAoGrupo(grupo)`.

```csharp
// GrupoAppService.cs
usuario.VincularAoGrupo(grupo);
grupo.AdicionarMembro(usuario);  // ✅ Adicionado
```

**Também corrigido em:** [`DataSeed.cs`](../src/CaronaAlvinegra.Infrastructure/Data/DataSeed.cs) — Adicionado loop que chama `grupo.AdicionarMembro(usuario)` para todos os usuários com `GrupoId` antes do `SaveChangesAsync`.

---

### 2. `PresencaAppService.ListarPresencasDoJogoAsync` retorna `ConfirmadoEm` incorreto

**Arquivo:** [`PresencaAppService.cs:108`](../src/CaronaAlvinegra.Application/Services/PresencaAppService.cs:108)

**✅ Corrigido:** Agora carrega os registros reais de `Presenca` via `_presencaRepo.FindAsync()` e faz lookup do `ConfirmadoEm` por dicionário, em vez de usar `DateTime.UtcNow`.

---

### 3. `RotaAppService.RemoverAsync` não verifica grupos vinculados à rota

**Arquivo:** [`RotaAppService.cs:57`](../src/CaronaAlvinegra.Application/Services/RotaAppService.cs:57)

**✅ Corrigido:** Adicionada dependência `IGrupoRepository` e validação que verifica se existem grupos com `RotaId == id` antes de excluir. Se houver, lança `DomainException` com os nomes dos grupos.

---

## 🟡 Problemas de Consistência e Design — ✅ Corrigidos

### 4. `AlocadorService` acessa `grupo.Membros` sem Include no repositório

**Arquivo:** [`GrupoRepository.cs`](../src/CaronaAlvinegra.Infrastructure/Repositories/GrupoRepository.cs)

**✅ Corrigido:** Adicionado `override GetAllAsync` com `.Include(g => g.Membros)` no `GrupoRepository`.

---

### 5. `Veiculo.Alocacoes` ignorado no EF mas tabela existe

**Arquivo:** [`AppDbContext.cs:137`](../src/CaronaAlvinegra.Infrastructure/Data/AppDbContext.cs:137)

**✅ Corrigido:** Removido `entity.Ignore(e => e.Alocacoes)`. A navegação `Alocacao → Veiculo` foi configurada com `.WithMany(v => v.Alocacoes)`, permitindo que o EF Core popule a coleção corretamente.

---

### 6. `UpdateUsuarioValidator` existe mas nunca é usado

**Arquivo:** [`UsuarioAppService.cs:57`](../src/CaronaAlvinegra.Application/Services/UsuarioAppService.cs:57)

**✅ Corrigido:** Adicionada dependência `IValidator<UpdateUsuarioRequest>` e chamada `await _updateValidator.ValidateAndThrowAsync(request, ct)` no início do método `AtualizarAsync`.

---

### 7. Vulnerabilidade de segurança no AutoMapper

**Arquivo:** [`CaronaAlvinegra.Application.csproj`](../src/CaronaAlvinegra.Application/CaronaAlvinegra.Application.csproj)

**✅ Corrigido:** AutoMapper atualizado de `13.0.1` para `16.1.1`. Ajustado registro em [`Program.cs`](../src/CaronaAlvinegra.Api/Program.cs) para usar a nova API (`cfg.AddProfile<DomainToDtoProfile>()`).

---

## 🔵 Problemas no Frontend (JavaScript) — ✅ Corrigidos

### 8. `alocarJogo` e `verAlocacao` chamam POST em vez de GET do resultado salvo

**Arquivos:** [`JogoEndpoints.cs`](../src/CaronaAlvinegra.Api/Endpoints/JogoEndpoints.cs), [`app.js`](../src/CaronaAlvinegra.Api/wwwroot/js/app.js)

**✅ Corrigido:** Adicionado endpoint GET `/api/jogos/{id}/alocar` em `JogoEndpoints.cs`. Em `app.js`, os métodos `verAlocacao` e `copiarWhatsApp` agora chamam `JogoApi.obterAlocacao(jogoId)` (GET) em vez de `JogoApi.alocar(jogoId)` (POST).

---

### 9. Modal de edição de rota referencia funcionalidade inexistente

**Arquivos:** [`api.js`](../src/CaronaAlvinegra.Api/wwwroot/js/api.js), [`app.js`](../src/CaronaAlvinegra.Api/wwwroot/js/app.js)

**✅ Corrigido:** Adicionado método `RotaApi.obter(id)` em `api.js`. Adicionada função `editarRota(id)` e botão de editar (`&#9998;`) na tabela de rotas em `app.js`.

---

### 10. `API_BASE` vazia não é configurável

**Arquivo:** [`api.js:5`](../src/CaronaAlvinegra.Api/wwwroot/js/api.js:5)

**✅ Corrigido:** `API_BASE` agora é configurável via `__API_BASE__` (defined at build time) ou `window.env.API_BASE` (runtime), mantendo fallback para `''` (mesmo origin).

---

## ⚠️ Ressalvas / Pontos de Atenção

### 11. Nenhum mecanismo de autenticação

Todo o sistema é acessível sem autenticação — tanto a API quanto o SPA. A tela de login no frontend é apenas cosmética (simula um "entrar" com nome).

### 12. Geração de IDs com `Guid.NewGuid()` no construtor de `AggregateRoot`

**Arquivo:** [`AggregateRoot.cs:11`](../src/CaronaAlvinegra.Domain/Entities/AggregateRoot.cs:11)

IDs são gerados no construtor, antes de salvar no banco. No `DataSeed` isso requer `SaveChangesAsync` intermediários para obter os IDs antes de criar objetos dependentes.

---

## ✅ O que Está Correto

- Estrutura DDD bem organizada com AggregateRoot, Domain Services, Application Services, Repositories
- Uso correto de `UnitOfWork` e `IUnitOfWork`
- Boa separação de responsabilidades nas camadas
- Middleware de Exception Handling bem estruturado com tratamento para `DomainException`, `ValidationException` e `DbUpdateException`
- Algoritmo de alocação (`AlocadorService`) bem implementado com fases distintas (Greedy, Consolidação, Classificação)
- Validação com FluentValidation presente na criação das entidades
- Configuração de EF Core adequada (índices únicos, FKs, delete behaviors)
- Build compila sem erros

---

## 📋 Resumo

| Categoria | Qtde | Corrigidos | Descrição |
|-----------|------|------------|-----------|
| 🔴 Bugs graves | 3 | ✅ 3/3 | Grupo sem membros em memória, timestamp incorreto, falta validação de grupo ao excluir rota |
| 🟡 Problemas de design | 4 | ✅ 4/4 | Include faltando, navegação ignorada, validator não usado, vulnerabilidade AutoMapper |
| 🔵 Frontend | 3 | ✅ 3/3 | POST em vez de GET, edição de rota não implementada, API_BASE fixa |
| ⚠️ Ressalvas | 2 | ⚠️ Mantidas | Sem autenticação, IDs gerados em construtor (não-actionáveis) |
