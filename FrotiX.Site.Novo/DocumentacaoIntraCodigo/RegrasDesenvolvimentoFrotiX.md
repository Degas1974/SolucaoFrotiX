# 📋 Regras de Desenvolvimento - FrotiX

## 🎯 Objetivo
Este documento define os padrões de documentação e desenvolvimento para o projeto FrotiX, garantindo consistência, manutenibilidade e facilidade de onboarding de novos desenvolvedores.

---

## 📝 Padrão de Documentação Intra-Código

### 1. Card de Documentação (Início de Funções/Métodos)

Toda função ou método deve ter um **Card de Documentação** no formato de comentário-bloco ASCII moderno:

#### Para C# (.cs, .cshtml.cs):
```csharp
/****************************************************************************************
 * ⚡ FUNÇÃO: NomeDaFuncao
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Explicação clara da regra de negócio que a função atende.
 * 📥 ENTRADAS     : [Tipo] nomeParam - Descrição do propósito.
 *                   [Tipo] outroParam - Descrição do propósito.
 * 📤 SAÍDAS       : [Tipo] - O que a função retorna (tipos e condições).
 * 🔗 CHAMADA POR  : Controller/Service/Módulos Externos (ou funções internas se privado).
 * 🔄 CHAMA        : _metodo1(), _metodo2(), ServiceX.MetodoY().
 * 📦 DEPENDÊNCIAS : Entity Framework Core, FluentValidation, etc.
 ****************************************************************************************/
```

#### Para JavaScript (.js, `<script>` em .cshtml):
```javascript
/****************************************************************************************
 * ⚡ FUNÇÃO: nomeDaFuncao
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Explicação clara da regra de negócio que a função atende.
 * 📥 ENTRADAS     : [Tipo] nomeParam - Descrição do propósito.
 * 📤 SAÍDAS       : [Tipo] - O que a função retorna.
 * 🔗 CHAMADA POR  : Evento de UI (ex: onClick do Botão X) / Função Y.
 * 🔄 CHAMA        : funcaoA(), funcaoB(), apiX().
 * 📦 DEPENDÊNCIAS : jQuery, Chart.js, SweetAlert, etc.
 ****************************************************************************************/
```

### 2. Campos Obrigatórios do Card

| Campo | Descrição |
|-------|-----------|
| **🎯 OBJETIVO** | Regra de negócio clara e concisa |
| **📥 ENTRADAS** | Parâmetros com tipos e propósitos |
| **📤 SAÍDAS** | Tipo de retorno e condições |
| **🔗 CHAMADA POR** | Métodos privados: funções internas que chamam<br>Métodos públicos: "Módulos Externos"<br>JS em UI: "Evento de UI (ex: onClick)" |
| **🔄 CHAMA** | Funções internas ou serviços que executa |
| **📦 DEPENDÊNCIAS** | Bibliotecas externas, DI, APIs utilizadas |

---

## 💬 Comentários Inline

### ✅ Quando Comentar:
- **Lógica de negócio crítica** (ex: validações complexas, cálculos específicos)
- **Condições complexas** (if/switch com múltiplas regras)
- **Processamento de dados** (LINQ, Map/Reduce, algoritmos não-triviais)
- **Decisões arquiteturais** (por que foi escolhida uma abordagem específica)

### ❌ Não Comentar:
- O óbvio (ex: `i++; // incrementa i`)
- Código auto-explicativo
- Nomes de variáveis/métodos que já explicam o propósito

### Formato:
```csharp
// [DOC] Explicação do "por que" esta lógica existe desta forma
```

---

## 🛡️ Tratamento de Erros

### Sistema Particular FrotiX (alerta.js / sweetalert_interop.js):

Todo código C# e JavaScript deve incluir tratamento de erros usando o sistema customizado:

#### C# (Razor Pages / Controllers):
```csharp
try
{
    // Lógica principal
}
catch (Exception ex)
{
    await JS.InvokeVoidAsync("alerta.erro", $"Erro ao processar: {ex.Message}");
    // ou para controllers:
    TempData["Erro"] = $"Erro ao processar: {ex.Message}";
    return Page(); // ou RedirectToPage()
}
```

#### JavaScript:
```javascript
try {
    // Lógica principal
} catch (erro) {
    alerta.erro(`Erro ao executar operação: ${erro.message}`);
    console.error('Detalhes do erro:', erro);
}
```

### Quando Adicionar Try-Catch:
Durante o processo de documentação, se uma função **não tiver tratamento de erros**, adicionar o padrão acima e **registrar no arquivo de log** do processo.

---

## 📂 Arquivos CSHTML (Razor)

### Código C#:
Seguir o padrão C# de documentação.

### Código JavaScript em `<script>`:
Seguir o padrão JavaScript de documentação.

---

## 📊 Registro de Progresso

### Arquivo: `/FrotiX.Site/DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md`

Formato de log:
```markdown
- [x] /Caminho/Completo/Do/Arquivo.ext - Finalizado em YYYY-MM-DD
- [ ] /Caminho/Completo/Do/Arquivo.ext - PENDENTE
```

Atualizar após a conclusão de cada arquivo.

---

## 🔄 Ordem de Documentação

Seguir ordem **estritamente alfabética**:
1. Diretórios (ordem alfabética)
2. Subdiretórios (ordem alfabética dentro do diretório pai)
3. Arquivos (ordem alfabética dentro do subdiretório)

Exemplo:
```
/Areas
  /Authorization
    /Pages
      Roles.cshtml
      Roles.cshtml.cs
      Users.cshtml
      Users.cshtml.cs
  /Identity
    ...
```

---

## ♻️ Arquivos Já Documentados

Se encontrar arquivos com documentação existente:
- Aproveitar o que há de melhor
- Atualizar para o padrão atual
- Substituir se a nova versão for superior

---

## 🎨 Boas Práticas Gerais

1. **Clareza**: Documentação deve ser compreensível para desenvolvedores júniors
2. **Concisão**: Evitar redundância e verbosidade
3. **Consistência**: Seguir sempre o mesmo padrão
4. **Manutenibilidade**: Atualizar documentação quando código mudar
5. **Idioma**: Português para comentários de negócio, inglês para nomes técnicos quando aplicável

---

## 📅 Controle de Versão

**Data de Criação**: 2026-01-26
**Última Atualização**: 2026-01-26
**Versão**: 1.0
**Autor**: Arquiteto de Software Sênior (Claude)

---

## 📌 Notas Importantes

- Este padrão deve ser seguido em **todos os novos códigos**
- Refatorações devem incluir atualização de documentação
- Code reviews devem verificar conformidade com este padrão
- Ferramentas de CI/CD podem ser configuradas para validar formato de comentários
