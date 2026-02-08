# MotoristaCache.cs — Cache de Motoristas

> **Arquivo:** `Cache/MotoristaCache.cs`  
> **Papel:** manter cache em memória de motoristas com foto base64.

---

## ✅ Visão Geral

Carrega motoristas de `ViewMotoristasViagem` e mantém uma lista em memória com foto em Base64 (fallback para imagem padrão).

---

## 🔧 Estrutura e Dependências

- `IUnitOfWork` para consulta reduzida.
- `IServiceScopeFactory` injetado (não utilizado diretamente no arquivo atual).
- Lock interno para segurança de acesso ao cache.

---

## 🧩 Snippet Comentado

```csharp
public void LoadMotoristas()
{
    lock (_lock)
    {
        var motoristas = _unitOfWork.ViewMotoristasViagem.GetAllReduced(
            selector: m => new { m.MotoristaId, Nome = m.MotoristaCondutor, m.Foto },
            orderBy: q => q.OrderBy(m => m.MotoristaCondutor)
        ).ToList();

        _cachedMotoristas = motoristas.Select(m =>
        {
            string fotoBase64;
            if (m.Foto != null && m.Foto.Length > 0)
            {
                try { fotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(m.Foto)}"; }
                catch { fotoBase64 = "/images/barbudo.jpg"; }
            }
            else
            {
                fotoBase64 = "/images/barbudo.jpg";
            }

            return new { m.MotoristaId, Nome = m.Nome, Foto = fotoBase64 };
        }).Cast<object>().ToList();
    }
}
```

---

## ✅ Observações Técnicas

- `GetMotoristas` garante fallback de foto quando em branco.
- `MotoristaDto` existe no mesmo arquivo, mas o cache usa objetos anônimos.

---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [22/01/2026] - Atualizacao: Padronizacao de Cards Internos

**Descricao**: Ajuste dos comentarios internos para o card padrao FrotiX conforme RegrasDesenvolvimentoFrotiX.md.

**Arquivos Afetados**:

- Cache/MotoristaCache.cs

**Mudancas**:

- Adicionados cards completos em construtor e metodos principais.

**Motivo**:

- Conformidade com o padrao de documentacao interna.

**Impacto**:

- Nenhuma alteracao funcional (apenas comentarios).

**Status**: ✅ Concluido

**Responsavel**: GitHub Copilot

**Versao**: Incremento de patch

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:

- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:

- âŒ **ANTES**: \_unitOfWork.Entity.AsTracking().Get(id) ou \_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: \_unitOfWork.Entity.GetWithTracking(id) ou \_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**:

- OtimizaÃ§Ã£o de memÃ³ria e performance
- Tracking seletivo (apenas quando necessÃ¡rio para Update/Delete)
- PadrÃ£o mais limpo e explÃ­cito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seÃ§Ã£o 4.2)

**Impacto**:

- Melhoria de performance em operaÃ§Ãµes de leitura (usa AsNoTracking por padrÃ£o)
- Tracking correto em operaÃ§Ãµes de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

**VersÃ£o**: Incremento de patch
