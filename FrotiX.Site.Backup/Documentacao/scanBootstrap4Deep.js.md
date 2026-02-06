# scanBootstrap4Deep.js — Scanner Profundo de Bootstrap 4

> **Arquivo:** `scanBootstrap4Deep.js`  
> **Papel:** varredura avançada + inspeção via navegador com Puppeteer.

---

## ✅ Visão Geral

Este script é uma versão avançada do scanner. Além do scan estático, ele:

- Abre o dashboard com **Puppeteer**.
- Detecta versão real de Bootstrap carregada em runtime.
- Gera relatório HTML consolidado.

---

## 🔧 Responsabilidades Principais

1. **Escanear arquivos locais** (regex + glob).
2. **Inspecionar runtime** (scripts/estilos carregados).
3. **Gerar relatório HTML** com evidências.

---

## 🧩 Snippets Comentados

```javascript
const browser = await puppeteer.launch({ headless: "new" });
const page = await browser.newPage();
await page.goto(url, { waitUntil: "networkidle2" });
```

```javascript
const bsVersion = await page.evaluate(() => {
    return window.bootstrap?.Modal?.VERSION || "NÃO DETECTADO";
});
```

---

## ✅ Observações Técnicas

- Útil para verificar **Bootstrap escondido** via libs legadas.
- Exige servidor local rodando para acessar a URL.
- Gera saída no arquivo `bootstrap4-report.html`.


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

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
