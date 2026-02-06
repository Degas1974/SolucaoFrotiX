# SUPER PROMPT: Problema Data Final Recorrência no Modal de Agendamento

## CONTEXTO DO PROBLEMA

Estou trabalhando em um sistema ASP.NET Core MVC (.NET 10) chamado FrotiX que gerencia agendamentos de viagens. Existe um modal de edição de agendamentos que usa componentes Syncfusion EJ2 DatePicker.

**O PROBLEMA**: O campo "Data Final Recorrência" (um DatePicker Syncfusion) NÃO aparece preenchido quando abrimos o PRIMEIRO agendamento recorrente da sessão para edição. Se fecharmos e abrirmos novamente (o mesmo ou outro), aí aparece.

## TENTATIVAS ANTERIORES QUE FALHARAM

### Tentativa 1: Polling com Verificação de Componente
Tentei usar polling recursivo verificando se o componente Syncfusion estava renderizado:

```javascript
function aguardarComponenteESetar(tentativa = 0)
{
    const maxTentativas = 20;
    const txtFinalRecorrencia = document.getElementById("txtFinalRecorrencia");

    if (!txtFinalRecorrencia)
    {
        if (tentativa < maxTentativas)
        {
            setTimeout(() => aguardarComponenteESetar(tentativa + 1), 100);
        }
        return;
    }

    const instance = txtFinalRecorrencia.ej2_instances?.[0];

    if (instance?.isRendered === true || txtFinalRecorrencia !== null)
    {
        // Setar valor aqui
        instance.value = new Date(objViagem.dataFinalRecorrencia);
    }
    else if (tentativa < maxTentativas)
    {
        setTimeout(() => aguardarComponenteESetar(tentativa + 1), 100);
    }
}
```

**RESULTADO**: Não funcionou. Data continuou não aparecendo no primeiro load.

### Tentativa 2: Substituir DatePicker por Campo de Texto (SOLUÇÃO ATUAL)
Implementei uma solução onde, ao abrir o modal em modo de EDIÇÃO, o DatePicker é ocultado e um campo de texto readonly é exibido no lugar.

**ARQUIVOS MODIFICADOS**:

#### 1. `Pages/Agenda/Index.cshtml` (linha ~1472)
```html
<ejs-datepicker id="txtFinalRecorrencia"
                format="dd/MM/yyyy"
                placeholder="Selecione a data final"
                locale="pt-BR"
                min="@DateTime.Today"
                cssClass="e-outline">
</ejs-datepicker>

<!-- Campo de texto para exibir data em modo de edição -->
<input type="text"
       id="txtFinalRecorrenciaTexto"
       class="form-control e-outline"
       readonly
       style="display:none;"
       placeholder="dd/MM/yyyy">
```

#### 2. `wwwroot/js/agendamento/components/exibe-viagem.js` (4 ocorrências)
Nas funções:
- `exibeViagemAgendamentoRecorrente()` (linha ~1650)
- `exibeViagemAgendamentoRecorrenteVeiculoMotorista()` (linha ~1748)
- `exibeViagemEventoRecorrente()` (linha ~3506)
- `exibeViagemEventoRecorrenteVeiculoMotorista()` (linha ~3601)

```javascript
if (objViagem.dataFinalRecorrencia)
{
    try
    {
        const txtFinalRecorrencia = document.getElementById("txtFinalRecorrencia");
        const txtFinalRecorrenciaTexto = document.getElementById("txtFinalRecorrenciaTexto");

        console.log("🔍 DEBUG Data Final Recorrência:");
        console.log("  - txtFinalRecorrencia existe?", !!txtFinalRecorrencia);
        console.log("  - txtFinalRecorrenciaTexto existe?", !!txtFinalRecorrenciaTexto);
        console.log("  - dataFinalRecorrencia:", objViagem.dataFinalRecorrencia);

        if (txtFinalRecorrenciaTexto)
        {
            const dataFinal = new Date(objViagem.dataFinalRecorrencia);
            const dia = String(dataFinal.getDate()).padStart(2, '0');
            const mes = String(dataFinal.getMonth() + 1).padStart(2, '0');
            const ano = dataFinal.getFullYear();
            const dataFormatada = `${dia}/${mes}/${ano}`;

            txtFinalRecorrenciaTexto.value = dataFormatada;
            txtFinalRecorrenciaTexto.style.display = "block";
            console.log("  - Campo de texto definido como:", dataFormatada);

            if (txtFinalRecorrencia)
            {
                txtFinalRecorrencia.style.display = "none";

                const wrapper = txtFinalRecorrencia.closest('.e-input-group');
                if (wrapper) {
                    wrapper.style.display = "none";
                    console.log("  - Wrapper do DatePicker também ocultado");
                }
            }

            console.log(`✅ Data Final Recorrência exibida em campo de texto: ${dataFormatada}`);
        }
        else
        {
            console.error("❌ Campo txtFinalRecorrenciaTexto não encontrado no DOM!");
        }
    }
    catch (error)
    {
        console.error("❌ Erro ao definir Data Final Recorrência:", error);
    }
}
```

#### 3. `wwwroot/js/agendamento/components/modal-viagem-novo.js` (linha ~2732)
Restauração ao fechar modal:

```javascript
function limparCamposModalViagens()
{
    try
    {
        // ... outros campos ...

        // ✅ RESTAURAR DatePicker de Data Final Recorrência
        console.log("🔄 [ModalViagem] Restaurando DatePicker de Data Final Recorrência...");
        const txtFinalRecorrencia = document.getElementById("txtFinalRecorrencia");
        const txtFinalRecorrenciaTexto = document.getElementById("txtFinalRecorrenciaTexto");

        if (txtFinalRecorrenciaTexto)
        {
            txtFinalRecorrenciaTexto.value = "";
            txtFinalRecorrenciaTexto.style.display = "none";
        }

        if (txtFinalRecorrencia)
        {
            txtFinalRecorrencia.style.display = "block";

            if (txtFinalRecorrencia.ej2_instances && txtFinalRecorrencia.ej2_instances[0])
            {
                txtFinalRecorrencia.ej2_instances[0].value = null;
                txtFinalRecorrencia.ej2_instances[0].enabled = true;
                window.refreshComponenteSafe("txtFinalRecorrencia");
            }

            const wrapper = txtFinalRecorrencia.closest('.e-input-group');
            if (wrapper) {
                wrapper.style.display = "block";
            }
        }
    }
    catch (error)
    {
        console.error("❌ Erro ao restaurar DatePicker:", error);
    }
}
```

## O PROBLEMA PERSISTENTE

**APESAR DE TODO O CÓDIGO ESTAR COMMITADO E BUILD REFEITO**, quando abrimos o modal de edição, o DatePicker Syncfusion AINDA aparece (vazio) ao invés do campo de texto.

**Evidências de que o código não está sendo executado**:

1. Os logs de debug "🔍 DEBUG Data Final Recorrência:" NÃO aparecem no console
2. O DatePicker continua visível
3. O campo de texto `txtFinalRecorrenciaTexto` NÃO aparece

**Já tentamos**:
- ✅ Hard refresh (Ctrl+Shift+R)
- ✅ Modo anônimo do navegador
- ✅ Limpar pastas bin/obj
- ✅ `dotnet build --no-incremental`
- ✅ Verificar que os arquivos JavaScript têm o código correto

**Versão da aplicação no console mostra que mudou**, mas o código NÃO está sendo executado.

## ESTRUTURA DO PROJETO

### Bundling/Minification
O projeto usa bundling e minification do ASP.NET Core. Os arquivos JavaScript estão em:
```
wwwroot/
├── js/
│   ├── agendamento/
│   │   ├── components/
│   │   │   ├── exibe-viagem.js          ← Exibe dados no modal
│   │   │   ├── modal-viagem-novo.js     ← Limpa campos ao fechar
│   │   │   └── validacao.js
│   │   └── services/
│   └── ...
```

### Como os arquivos são referenciados
No `_Layout.cshtml` ou nas páginas, os scripts são incluídos assim:
```html
<script src="~/js/agendamento/components/exibe-viagem.js"></script>
<script src="~/js/agendamento/components/modal-viagem-novo.js"></script>
```

## INFORMAÇÕES TÉCNICAS

### Stack
- **Backend**: ASP.NET Core MVC (.NET 10)
- **Frontend**: jQuery 3.x + Syncfusion EJ2
- **Build**: MSBuild com bundling/minification padrão

### Componente Syncfusion
```javascript
// Como acessar instância do DatePicker
const element = document.getElementById("txtFinalRecorrencia");
const instance = element.ej2_instances[0];

// Propriedades úteis
instance.value          // Data atual
instance.isRendered     // Se está renderizado
instance.enabled        // Se está habilitado
```

## O QUE PRECISO

Preciso de uma solução que:

1. **GARANTA que o código JavaScript seja executado** após o build
2. **Resolva o problema de cache/bundling** que está impedindo a atualização
3. **Faça o campo de texto aparecer** no lugar do DatePicker em modo de edição

**OU**, se a solução de campo de texto não é viável:

4. **Encontre uma forma de REALMENTE fazer o DatePicker Syncfusion ser preenchido no primeiro load**

## PERGUNTAS ESPECÍFICAS

1. Existe algum cache de bundling do ASP.NET Core que não estamos limpando?
2. Precisa adicionar alguma configuração no `Startup.cs` ou `Program.cs`?
3. Existe alguma forma de forçar o navegador a recarregar os arquivos JavaScript bundled?
4. O problema pode ser ordem de carregamento de scripts?
5. Existe alguma configuração específica do Syncfusion que impeça a manipulação do DOM do componente?

## ARQUIVOS PARA ANÁLISE

Se precisar ver algum arquivo específico, posso fornecer:
- `Startup.cs` (configuração da aplicação)
- `_Layout.cshtml` (inclusão de scripts)
- `Pages/Agenda/Index.cshtml` (página completa)
- Qualquer outro arquivo relevante

## RESULTADO ESPERADO

Quando o usuário abrir um agendamento recorrente para edição:

**ESPERADO**:
```
[ Motorista: Alexandre ]  [ Veículo: JFP-6345 ]
[ Data Inicial: 07/01/2026 ]  [ Hora: 07:00 ]
[ Recorrente: Sim ]  [ Período: Diário ]
[ Data Final Recorrência: 20/01/2026 ]  ← Campo de texto READONLY
```

**ACONTECENDO**:
```
[ Motorista: Alexandre ]  [ Veículo: JFP-6345 ]
[ Data Inicial: 07/01/2026 ]  [ Hora: 07:00 ]
[ Recorrente: Sim ]  [ Período: Diário ]
[ Data Final Recorrência: (vazio) ]  ← DatePicker VAZIO
```

## LOGS DO CONSOLE

Quando abrimos o modal, DEVERIA aparecer:
```
🔍 DEBUG Data Final Recorrência:
  - txtFinalRecorrencia existe? true
  - txtFinalRecorrenciaTexto existe? true
  - dataFinalRecorrencia: 2026-01-20T00:00:00
  - Campo de texto definido como: 20/01/2026
  - Wrapper do DatePicker também ocultado
✅ Data Final Recorrência exibida em campo de texto: 20/01/2026
```

Mas NÃO aparece nada. Apenas logs normais de abertura do modal.

---

**POR FAVOR, AJUDE-ME A:**
1. Diagnosticar por que o código JavaScript não está sendo executado após o build
2. Encontrar uma solução definitiva para este problema
3. Se necessário, propor uma abordagem completamente diferente

**ESTOU ABERTO A QUALQUER SOLUÇÃO**, inclusive refatorar completamente a forma como o modal é preenchido, se necessário.
