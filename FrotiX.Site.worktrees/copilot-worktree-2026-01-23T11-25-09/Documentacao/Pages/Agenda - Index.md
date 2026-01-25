# Documenta��o: Agenda de Viagens

> **Ultima Atualizacao**: 22/01/2026 16:00
> **Versao Atual**: 5.3

---

# PARTE 1: DOCUMENTA��O DA FUNCIONALIDADE

## Objetivos

A p�gina **Agenda de Viagens** (`Pages/Agenda/Index.cshtml`) permite:
- ? Visualizar todas as viagens e eventos em um calend�rio interativo (FullCalendar 6)
- ? Agendar novas viagens com configura��es de recorr�ncia complexas (Di�ria, Semanal, Quinzenal, Mensal, Variada)
- ? Editar agendamentos existentes (com suporte a edi��o em massa de recorrentes)
- ? Transformar agendamentos em viagens abertas ou realizadas
- ? Monitorar ocupa��o de ve�culos e motoristas em tempo real
- ? Gerenciar conflitos de hor�rio automaticamente
- ? Validar dados com sistema inteligente (IA) para datas, horas e quilometragem

---

## Arquivos Envolvidos

### 1. Pages/Agenda/Index.cshtml
**Fun��o**: View principal com calend�rio FullCalendar e modal complexo de agendamento

**Estrutura**:
- Legenda de cores de status
- Calend�rio FullCalendar (`#agenda`)
- Modal Bootstrap complexo (`#modalViagens`) com 7 se��es
- Scripts JavaScript modulares

---

### 2. Pages/Agenda/Index.cshtml.cs
**Fun��o**: PageModel que inicializa dados para os componentes

**Problema**: Modal precisa de listas pr�-carregadas (motoristas, ve�culos, finalidades, eventos, etc.)

**Solu��o**: Carregar listas no OnGet usando helpers especializados

**C�digo**:
```csharp
public void OnGet()
{
    // ? Inicializa dados usando helpers especializados
    FrotiX.Pages.Viagens.IndexModel.Initialize(_unitOfWork);
    ViewData["dataCombustivel"] = new ListaNivelCombustivel(_unitOfWork).NivelCombustivelList();
    ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
    ViewData["lstVeiculos"] = new ListaVeiculos(_unitOfWork).VeiculosList();
    ViewData["lstSetor"] = new ListaSetores(_unitOfWork).SetoresList();
    ViewData["lstStatus"] = new ListaStatus(_unitOfWork).StatusList();
    ViewData["lstEventos"] = new ListaEvento(_unitOfWork).EventosList();
}
```

---

### 3. wwwroot/js/agendamento/main.js
**Fun��o**: Ponto de entrada principal, inicializa��o de componentes e handlers globais

#### 3.1. Inicializa��o do Calend�rio
**Problema**: Calend�rio precisa carregar eventos do per�odo vis�vel e permitir intera��es (click, drag, resize)

**Solu��o**: Configurar FullCalendar com eventos via AJAX e handlers de intera��o

**C�digo**:
```javascript
window.InitializeCalendar = function(URL) {
    var calendarEl = document.getElementById("agenda");
    
    window.calendar = new FullCalendar.Calendar(calendarEl, {
        timeZone: "local",
        lazyFetching: true,  // ? Carrega eventos sob demanda
        headerToolbar: {
            left: "prev,next today",
            center: "title",
            right: "dayGridMonth,timeGridWeek,timeGridDay"
        },
        buttonText: {
            today: "Hoje",
            dayGridMonth: "mensal",
            timeGridWeek: "semanal",
            timeGridDay: "di�rio"
        },
        initialView: "timeGridWeek",  // Visualiza��o semanal por padr�o
        locale: "pt-br",
        events: {
            url: "/api/Agenda/CarregaViagens",
            method: "GET",
            failure: function() {
                AppToast.show('Vermelho', 'Erro ao carregar eventos!');
            }
        },
        eventClick: function(info) {
            // ? Abre modal para edi��o
            abrirModalEdicao(info.event.id);
        },
        dateClick: function(info) {
            // ? Abre modal para novo agendamento na data clicada
            abrirModalNovo(info.dateStr);
        },
        eventDidMount: function(info) {
            // ? Personaliza��o visual de cada evento
            // Adiciona tooltips, classes CSS, etc.
        }
    });
    
    calendar.render();
};
```

#### 3.2. Bot�o de Confirma��o (Salvar Agendamento)
**Problema**: Usu�rio precisa salvar agendamento ap�s preencher formul�rio complexo com valida��es

**Solu��o**: Handler que valida campos, verifica conflitos, cria objeto e envia para API

**C�digo**:
```javascript
$("#btnConfirma").off("click").on("click", async function (event) {
    try {
        event.preventDefault();
        const $btn = $(this);
        
        // ? Previne clique duplo
        if ($btn.prop("disabled")) {
            return;
        }
        
        $btn.prop("disabled", true);
        
        const viagemId = document.getElementById("txtViagemId").value;
        
        // ? Valida��o completa de campos
        const validado = await window.ValidaCampos(viagemId);
        if (!validado) {
            $btn.prop("disabled", false);
            return;
        }
        
        // ? Valida��o IA (se dispon�vel)
        const isRegistraViagem = $("#btnConfirma").text().includes("Registra Viagem");
        if (isRegistraViagem && typeof window.validarFinalizacaoConsolidadaIA === 'function') {
            const iaValida = await window.validarFinalizacaoConsolidadaIA({
                dataInicial: DataInicial,
                horaInicial: HoraInicial,
                dataFinal: DataFinal,
                horaFinal: HoraFinal,
                kmInicial: KmInicial,
                kmFinal: KmFinal,
                veiculoId: veiculoId
            });
            
            if (!iaValida) {
                $btn.prop("disabled", false);
                return;
            }
        }
        
        // ? Cria objeto de agendamento
        const agendamento = window.criarAgendamentoNovo();
        
        // ? Verifica conflitos antes de salvar
        const conflitos = await window.verificarConflitos(agendamento);
        if (conflitos.temConflito) {
            const confirma = await Alerta.Confirmar(
                "Conflito de Hor�rio",
                `O ve�culo/motorista j� est� ocupado neste hor�rio. Deseja continuar mesmo assim?`,
                "Sim, Continuar",
                "Cancelar"
            );
            
            if (!confirma) {
                $btn.prop("disabled", false);
                return;
            }
        }
        
        // ? Envia para API
        const resposta = await window.enviarNovoAgendamento(agendamento);
        
        if (resposta.success) {
            $('#modalViagens').modal('hide');
            window.calendar.refetchEvents(); // ? Atualiza calend�rio
            Alerta.Sucesso('Sucesso', 'Agendamento salvo com sucesso');
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("main.js", "btnConfirma.click", error);
    } finally {
        $btn.prop("disabled", false);
    }
});
```

---

### 4. wwwroot/js/agendamento/components/calendario.js
**Fun��o**: Configura��o e handlers do FullCalendar

#### 4.1. Formata��o de Eventos
**Problema**: Eventos precisam ter cores e t�tulos espec�ficos por status

**Solu��o**: Fun��o que formata eventos retornados da API com cores e propriedades estendidas

**C�digo**: A formata��o � feita no backend (endpoint `CarregaViagens`), mas o calend�rio pode customizar via `eventDidMount`

---

### 5. wwwroot/js/agendamento/components/modal-viagem-novo.js
**Fun��o**: L�gica completa do modal de agendamento

#### 5.1. Cria��o de Objeto de Agendamento
**Problema**: Formul�rio tem 50+ campos que precisam ser coletados e formatados para envio � API

**Solu��o**: Fun��o que l� todos os componentes Syncfusion e monta objeto JSON

**C�digo**:
```javascript
window.criarAgendamentoNovo = function () {
    try {
        // ? Obter inst�ncias dos componentes Syncfusion
        const txtDataInicial = document.getElementById("txtDataInicial")?.ej2_instances?.[0];
        const txtHoraInicial = $("#txtHoraInicial").val();
        const lstMotorista = document.getElementById("lstMotorista")?.ej2_instances?.[0];
        const lstVeiculo = document.getElementById("lstVeiculo")?.ej2_instances?.[0];
        const lstRecorrente = document.getElementById("lstRecorrente")?.ej2_instances?.[0];
        const rteDescricao = document.getElementById("rteDescricao")?.ej2_instances?.[0];
        
        // ? Extrair valores
        const dataInicialValue = txtDataInicial?.value;
        const motoristaId = lstMotorista?.value;
        const veiculoId = lstVeiculo?.value;
        const recorrente = lstRecorrente?.value || "N";
        
        // ? Montar objeto de agendamento
        const agendamento = {
            ViagemId: document.getElementById("txtViagemId").value || "00000000-0000-0000-0000-000000000000",
            DataInicial: dataInicialValue ? new Date(dataInicialValue).toISOString() : null,
            HoraInicio: txtHoraInicial || null,
            MotoristaId: motoristaId || null,
            VeiculoId: veiculoId || null,
            Recorrente: recorrente,
            Status: document.getElementById("txtStatus").value || "Agendada",
            Descricao: rteDescricao?.value || ""
        };
        
        // ? Processar recorr�ncia se necess�rio
        if (recorrente === "S") {
            const datasSelecionadas = window.gerarDatasRecorrencia();
            agendamento.DatasSelecionadas = datasSelecionadas;
        }
        
        return agendamento;
    } catch (error) {
        Alerta.TratamentoErroComLinha("modal-viagem-novo.js", "criarAgendamentoNovo", error);
        return null;
    }
};
```

#### 5.2. Envio para API
**Problema**: Objeto precisa ser enviado para API com tratamento de erros e feedback ao usu�rio

**Solu��o**: Fun��o ass�ncrona que envia POST e trata resposta

**C�digo**:
```javascript
window.enviarNovoAgendamento = async function (agendamento) {
    try {
        const resposta = await fetch('/api/Agenda/Agendamento', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(agendamento)
        });
        
        const resultado = await resposta.json();
        
        if (!resposta.ok) {
            throw new Error(resultado.message || 'Erro ao salvar agendamento');
        }
        
        return resultado;
    } catch (error) {
        Alerta.TratamentoErroComLinha("modal-viagem-novo.js", "enviarNovoAgendamento", error);
        return { success: false, message: error.message };
    }
};
```

---

### 6. wwwroot/js/agendamento/components/recorrencia.js
**Nota (20/01/2026)**: A escrita dos campos de recorr�ncia agora � centralizada em `recorrencia-controller.js`, evitando sobrescritas no modal.
**Fun��o**: L�gica de gera��o de datas recorrentes

#### 6.1. Gera��o de Recorr�ncia Di�ria
**Problema**: Usu�rio precisa criar agendamentos para todos os dias entre duas datas

**Solu��o**: Fun��o que gera array de datas di�rias entre data inicial e final

**C�digo**:
```javascript
gerarRecorrenciaDiaria(dataAtual, dataFinalFormatada, datas) {
    try {
        let data = moment(dataAtual);
        const dataFinal = moment(dataFinalFormatada);
        
        // ? Gera datas di�rias at� data final
        while (data.isSameOrBefore(dataFinal)) {
            datas.push(data.format('YYYY-MM-DD'));
            data.add(1, 'days');
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaDiaria", error);
    }
}
```

#### 6.2. Gera��o de Recorr�ncia Semanal
**Problema**: Usu�rio precisa criar agendamentos para dias espec�ficos da semana (ex: Segunda, Quarta, Sexta)

**Solu��o**: Fun��o que gera datas apenas nos dias da semana selecionados

**C�digo**:
```javascript
gerarRecorrenciaPorPeriodo(tipoRecorrencia, dataAtual, dataFinalFormatada, diasSelecionadosIndex, datas) {
    try {
        let data = moment(dataAtual);
        const dataFinal = moment(dataFinalFormatada);
        const intervalo = tipoRecorrencia === "Q" ? 2 : 1; // Quinzenal = 2 semanas
        
        // ? Gera datas apenas nos dias selecionados
        while (data.isSameOrBefore(dataFinal)) {
            const diaSemana = data.day(); // 0=Domingo, 1=Segunda, etc.
            
            if (diasSelecionadosIndex.includes(diaSemana)) {
                datas.push(data.format('YYYY-MM-DD'));
            }
            
            data.add(intervalo, 'weeks');
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaPorPeriodo", error);
    }
}
```

#### 6.3. Gera��o de Recorr�ncia Mensal
**Problema**: Usu�rio precisa criar agendamentos no mesmo dia do m�s (ex: dia 15 de cada m�s)

**Solu��o**: Fun��o que gera datas no mesmo dia do m�s at� data final

**C�digo**: Similar � di�ria, mas incrementa por m�s

#### 6.4. Gera��o de Recorr�ncia Variada
**Problema**: Usu�rio precisa criar agendamentos em datas espec�ficas selecionadas manualmente no calend�rio

**Solu��o**: Fun��o que l� datas selecionadas no Syncfusion Calendar e retorna array

**C�digo**:
```javascript
gerarRecorrenciaVariada(datas) {
    try {
        const calendarObj = document.getElementById("calDatasSelecionadas")?.ej2_instances?.[0];
        
        if (!calendarObj || !calendarObj.values || calendarObj.values.length === 0) {
            console.error("Nenhuma data selecionada no calend�rio");
            return;
        }
        
        // ? Converte datas selecionadas para formato YYYY-MM-DD
        calendarObj.values.forEach(data => {
            datas.push(moment(data).format('YYYY-MM-DD'));
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaVariada", error);
    }
}
```

---

### 7. Controllers/AgendaController.cs
**Fun��o**: Endpoints API para opera��es com agenda

#### 7.1. GET `/api/Agenda/CarregaViagens`
**Problema**: FullCalendar precisa de eventos formatados para exibir no calend�rio

**Solu��o**: Endpoint que busca viagens da view `ViewViagensAgenda` e formata para FullCalendar

**C�digo**:
```csharp
[HttpGet("CarregaViagens")]
public IActionResult CarregaViagens(DateTime start, DateTime end)
{
    try
    {
        // ? Ajuste de timezone (FullCalendar envia UTC, banco est� UTC-3)
        DateTime startMenos3 = start.AddHours(-3);
        DateTime endMenos3 = end.AddHours(-3);
        
        // ? Busca na view otimizada
        var viagens = _context.ViewViagensAgenda
            .AsNoTracking()
            .Where(v => v.DataInicial.HasValue
                && v.DataInicial >= startMenos3
                && v.DataInicial < endMenos3)
            .ToList();
        
        // ? Formata para FullCalendar
        var eventos = viagens.Select(v => new
        {
            id = v.ViagemId.ToString(),
            title = v.Titulo ?? "Viagem",
            start = v.Start?.ToString("yyyy-MM-ddTHH:mm:ss") ?? v.DataInicial?.ToString("yyyy-MM-ddTHH:mm:ss"),
            end = v.End?.ToString("yyyy-MM-ddTHH:mm:ss") ?? v.DataInicial?.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss"),
            backgroundColor = v.CorEvento ?? "#808080",
            textColor = v.CorTexto ?? "#FFFFFF",
            extendedProps = new
            {
                status = v.Status,
                veiculo = v.PlacaVeiculo,
                motorista = v.NomeMotorista
            }
        }).ToList();
        
        return Ok(eventos);
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("AgendaController.cs", "CarregaViagens", error);
        return StatusCode(500);
    }
}
```

#### 7.2. POST `/api/Agenda/Agendamento`
**Problema**: Frontend precisa criar/editar agendamentos com suporte a recorr�ncia e m�ltiplos cen�rios

**Solu��o**: Endpoint complexo que trata 3 cen�rios principais (novo �nico, novo recorrente, edi��o)

**C�digo - Cen�rio 1: Novo Agendamento �nico**:
```csharp
bool isNew = viagem.ViagemId == Guid.Empty;

if (isNew == true && viagem.Recorrente != "S")
{
    // ? Cria agendamento �nico
    Viagem novaViagem = new Viagem();
    AtualizarDadosAgendamento(novaViagem, viagem);
    novaViagem.Status = "Agendada";
    novaViagem.StatusAgendamento = true;
    novaViagem.FoiAgendamento = false;
    novaViagem.UsuarioIdAgendamento = currentUserID;
    novaViagem.DataAgendamento = DateTime.Now;
    
    _unitOfWork.Viagem.Add(novaViagem);
    _unitOfWork.Save();
    
    return Ok(new { success = true, viagemId = novaViagem.ViagemId });
}
```

**C�digo - Cen�rio 2: Novo Agendamento Recorrente**:
```csharp
if (isNew == true && viagem.Recorrente == "S")
{
    Guid primeiraViagemId = Guid.Empty;
    bool primeiraIteracao = true;
    
    // ? Cria primeira viagem da s�rie
    Viagem novaViagem = new Viagem();
    AtualizarDadosAgendamento(novaViagem, viagem);
    novaViagem.DataInicial = DatasSelecionadasAdicao.First();
    novaViagem.UsuarioIdAgendamento = currentUserID;
    novaViagem.DataAgendamento = DateTime.Now;
    
    _unitOfWork.Viagem.Add(novaViagem);
    _unitOfWork.Save();
    
    primeiraViagemId = novaViagem.ViagemId;
    novaViagem.RecorrenciaViagemId = primeiraViagemId;
    _unitOfWork.Viagem.Update(novaViagem);
    
    // ? Cria demais viagens da s�rie
    foreach (var dataSelecionada in DatasSelecionadasAdicao.Skip(1))
    {
        Viagem novaViagemRecorrente = new Viagem();
        AtualizarDadosAgendamento(novaViagemRecorrente, viagem);
        novaViagemRecorrente.DataInicial = dataSelecionada;
        novaViagemRecorrente.RecorrenciaViagemId = primeiraViagemId; // ? Todas apontam para primeira
        
        _unitOfWork.Viagem.Add(novaViagemRecorrente);
    }
    
    _unitOfWork.Save();
    
    return Ok(new { success = true, totalCriado = DatasSelecionadasAdicao.Count });
}
```

**C�digo - Cen�rio 3: Editar Agendamento**:
```csharp
if (isNew == false)
{
    var viagemExistente = await _unitOfWork.Viagem.GetFirstOrDefaultAsync(
        v => v.ViagemId == viagem.ViagemId
    );
    
    if (viagemExistente == null)
    {
        return NotFound();
    }
    
    // ? Atualiza campos
    AtualizarDadosAgendamento(viagemExistente, viagem);
    
    // ? Se transformando em viagem
    if (viagem.Status == "Aberta" || viagem.Status == "Realizada")
    {
        viagemExistente.FoiAgendamento = true;
        viagemExistente.UsuarioIdCriacao = currentUserID;
        viagemExistente.DataCriacao = DateTime.Now;
    }
    
    _unitOfWork.Viagem.Update(viagemExistente);
    _unitOfWork.Save();
    
    return Ok(new { success = true });
}
```

#### 7.3. GET `/api/Agenda/VerificarAgendamento`
**Problema**: Frontend precisa verificar conflitos de hor�rio antes de salvar

**Solu��o**: Endpoint que verifica sobreposi��o temporal de viagens para ve�culo/motorista

**C�digo**:
```csharp
[HttpGet("VerificarAgendamento")]
public async Task<IActionResult> VerificarAgendamento(
    Guid? veiculoId,
    Guid? motoristaId,
    DateTime dataInicial,
    DateTime? dataFinal,
    Guid? viagemIdExcluir)
{
    try
    {
        var query = _unitOfWork.Viagem.GetAll()
            .Where(v =>
                (veiculoId.HasValue && v.VeiculoId == veiculoId.Value) ||
                (motoristaId.HasValue && v.MotoristaId == motoristaId.Value))
            .Where(v => v.Status != "Cancelada")
            .Where(v => viagemIdExcluir == null || v.ViagemId != viagemIdExcluir.Value);
        
        // ? Verifica sobreposi��o temporal
        var conflitos = await query
            .Where(v =>
                v.DataInicial < (dataFinal ?? dataInicial.AddHours(1)) &&
                (v.DataFinal ?? v.DataInicial.AddHours(1)) > dataInicial
            )
            .Select(v => new
            {
                v.ViagemId,
                v.DataInicial,
                v.DataFinal,
                v.Status,
                PlacaVeiculo = v.Veiculo.Placa,
                NomeMotorista = v.Motorista.Nome
            })
            .ToListAsync();
        
        return Ok(new
        {
            temConflito = conflitos.Any(),
            conflitos = conflitos
        });
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("AgendaController.cs", "VerificarAgendamento", error);
        return StatusCode(500);
    }
}
```

#### 7.4. GET `/api/Agenda/ObterAgendamento`
**Problema**: Frontend precisa buscar dados de viagem para preencher modal de edi��o

**Solu��o**: Endpoint que retorna dados completos da viagem com relacionamentos

**C�digo**:
```csharp
[HttpGet("ObterAgendamento")]
public async Task<IActionResult> ObterAgendamento(Guid id)
{
    try
    {
        // ? Busca viagem com relacionamentos
        var viagem = await _unitOfWork.Viagem.GetFirstOrDefaultAsync(
            v => v.ViagemId == id,
            includeProperties: "Motorista,Veiculo,Requisitante,SetorSolicitante,Evento"
        );
        
        if (viagem == null)
        {
            return NotFound();
        }
        
        // ? Monta objeto de resposta
        var resposta = new
        {
            viagemId = viagem.ViagemId,
            dataInicial = viagem.DataInicial,
            horaInicio = viagem.HoraInicio?.ToString("HH:mm"),
            dataFinal = viagem.DataFinal,
            horaFim = viagem.HoraFim?.ToString("HH:mm"),
            origem = viagem.Origem,
            destino = viagem.Destino,
            finalidadeId = viagem.FinalidadeId,
            motoristaId = viagem.MotoristaId,
            veiculoId = viagem.VeiculoId,
            kmInicial = viagem.KmInicial,
            kmFinal = viagem.KmFinal,
            requisitanteId = viagem.RequisitanteId,
            setorSolicitanteId = viagem.SetorSolicitanteId,
            eventoId = viagem.EventoId,
            status = viagem.Status,
            descricao = viagem.Descricao,
            recorrenciaViagemId = viagem.RecorrenciaViagemId,
            recorrente = viagem.RecorrenciaViagemId != null ? "S" : "N"
        };
        
        return Ok(resposta);
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamento", error);
        return StatusCode(500);
    }
}
```

---

## Fluxo de Funcionamento

### Carregamento da P�gina
```
1. P�gina carrega (OnGet)
   ?
2. Backend carrega listas para componentes (motoristas, ve�culos, etc.)
   ?
3. Frontend inicializa componentes Syncfusion
   ?
4. Frontend inicializa FullCalendar chamando InitializeCalendar()
   ?
5. Calend�rio faz requisi��o GET para /api/Agenda/CarregaViagens?start=...&end=...
   ?
6. Backend retorna eventos formatados da ViewViagensAgenda
   ?
7. Calend�rio renderiza eventos com cores e tooltips
```

### Cria��o de Novo Agendamento
```
1. Usu�rio clica em data no calend�rio (dateClick)
   ?
2. Modal Bootstrap abre com data pr�-preenchida
   ?
3. Usu�rio preenche formul�rio (origem, destino, motorista, ve�culo, etc.)
   ?
4. Se selecionou recorr�ncia:
   - Seleciona tipo (Di�ria, Semanal, etc.)
   - Configura per�odo e dias
   - Sistema gera array de datas
   ?
5. Usu�rio clica em "Confirmar"
   ?
6. Valida��o completa de campos (ValidaCampos)
   ?
7. Valida��o IA (se dispon�vel)
   ?
8. Verifica��o de conflitos (VerificarAgendamento)
   ?
9. Se h� conflitos: mostra alerta e pergunta se deseja continuar
   ?
10. Cria objeto de agendamento (criarAgendamentoNovo)
   ?
11. Requisi��o POST para /api/Agenda/Agendamento
   ?
12. Backend processa (cria �nico ou m�ltiplos se recorrente)
   ?
13. Calend�rio atualiza (refetchEvents)
   ?
14. Modal fecha
```

### Edi��o de Agendamento
```
1. Usu�rio clica em evento no calend�rio (eventClick)
   ?
2. Modal Bootstrap abre
   ?
3. Requisi��o GET para /api/Agenda/ObterAgendamento?id=guid
   ?
4. Backend retorna dados completos da viagem
   ?
5. Frontend preenche todos os campos do modal
   ?
6. Usu�rio edita campos desejados
   ?
7. Clica em "Confirmar"
   ?
8. Valida��es e verifica��o de conflitos (mesmo fluxo de cria��o)
   ?
9. Requisi��o POST para /api/Agenda/Agendamento (com ViagemId preenchido)
   ?
10. Backend atualiza viagem existente
   ?
11. Calend�rio atualiza
```

---

## Endpoints API Resumidos

| M�todo | Endpoint | Descri��o | Par�metros |
|--------|----------|-----------|------------|
| GET | `/api/Agenda/CarregaViagens` | Retorna eventos para calend�rio | `start`, `end` (DateTime) |
| POST | `/api/Agenda/Agendamento` | Cria/atualiza agendamento | `{ViagemId, DataInicial, HoraInicio, ...}` |
| GET | `/api/Agenda/VerificarAgendamento` | Verifica conflitos de hor�rio | `veiculoId`, `motoristaId`, `dataInicial`, `dataFinal` |
| GET | `/api/Agenda/ObterAgendamento` | Busca dados para edi��o | `id` (Guid) |
| GET | `/api/Agenda/BuscarViagensRecorrencia` | Busca s�rie recorrente | `id` (Guid) |
| POST | `/api/Agenda/ApagaAgendamento` | Exclui agendamento | `{ViagemId}` |

---

## Troubleshooting

### Problema: Calend�rio n�o carrega eventos
**Causa**: Erro no endpoint `/api/Agenda/CarregaViagens` ou view `ViewViagensAgenda` n�o existe  
**Solu��o**: 
- Verificar logs do servidor
- Verificar se view existe no banco de dados
- Testar endpoint manualmente: `/api/Agenda/TesteView`
- Verificar Network Tab para erros na requisi��o

### Problema: Modal n�o abre ao clicar em evento
**Causa**: Event handler `eventClick` n�o est� registrado ou ID do evento est� incorreto  
**Solu��o**: 
- Verificar se `InitializeCalendar()` foi chamado
- Verificar se fun��o `abrirModalEdicao()` existe
- Verificar console do navegador por erros JavaScript

### Problema: Recorr�ncia n�o gera datas corretas
**Causa**: L�gica de gera��o de datas est� incorreta ou componentes n�o est�o inicializados  
**Solu��o**: 
- Verificar se componentes Syncfusion est�o inicializados
- Verificar se fun��o `gerarDatasRecorrencia()` est� sendo chamada
- Verificar console para logs de debug

### Problema: Conflitos n�o s�o detectados
**Causa**: Endpoint `/api/Agenda/VerificarAgendamento` n�o est� sendo chamado ou retorna resultado incorreto  
**Solu��o**: 
- Verificar se fun��o `verificarConflitos()` est� sendo chamada antes de salvar
- Verificar Network Tab para requisi��o de verifica��o
- Testar endpoint manualmente com par�metros conhecidos

---

# PARTE 2: LOG DE MODIFICA��ES/CORRE��ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [22/01/2026] - Limpeza: Remo��o de banner de teste e ajuste de layout

**Descri��o**: Removido banner vermelho de teste que estava fixo no topo da p�gina e ajustado largura da coluna do campo "Dias da Semana" para melhor alinhamento horizontal.

**Mudan�as**:

- Remo��o do div de teste com banner vermelho (linhas 3-6)
- Ajuste da largura da coluna `lstDias` de `col-md-6` para `col-md-3` (linha 1578)
- Atualiza��o do coment�rio para refletir o novo tamanho

**Arquivos Afetados**:

- `Pages/Agenda/Index.cshtml` (linhas 3-6, 1578)

**Impacto**:

- Interface limpa sem elementos de teste
- Melhor alinhamento dos campos de filtro

**Status**: ✅ **Conclu�do**

**Respons�vel**: Claude Sonnet 4.5

**Vers�o**: 5.1

---

## [20/01/2026] - Fix: Remocao de ScriptsBasePlugins duplicado

**Descricao**: Removida a inclusao extra de `_ScriptsBasePlugins` no `Index.cshtml` da Agenda, evitando scripts duplicados e o erro de `ejTooltip` ja declarado no console.

**Mudancas**:
- Remocao de `@await Html.PartialAsync("_ScriptsBasePlugins")` do `Index.cshtml`

**Impacto**:
- Scripts base carregados apenas uma vez
- Reduz erros de inicializacao e logs duplicados

**Status**: ? Concluido

**Responsavel**: Codex

**Versao**: 4.9


## [18/01/2026 05:35] - Corre��o de Bug Visual: Duplica��o de Campo "Data Final Recorr�ncia" ao Alternar Edi��o -> Novo

**Descri��o**: Corrigido bug visual onde o campo "Data Final Recorr�ncia" aparecia duplicado (DatePicker e Textbox Readonly) ao alternar de edi��o para novo agendamento.

**Problema**:
- Ao abrir um agendamento para edi��o, o DatePicker era ocultado e substitu�do por um campo de texto readonly.
- Ao clicar em "Novo Agendamento" (ap�s editar), a l�gica de reset tentava restaurar o DatePicker, mas o estilo inline do campo de texto persistia ou conflitava com o re-render do Syncfusion.
- Resultado: Ambos os campos apareciam vis�veis simultaneamente.

**Solu��o Implementada**:
1. **CSS Dedicado**: Criada classe `.ftx-ocultar-recorrencia-texto { display:none !important; }` em `Pages/Agenda/Index.cshtml`.
2. **Refatora��o JS**: Substitu�da manipula��o direta de `style.display` por `classList.add/remove` e remo��o expl�cita de estilos inline conflitantes.
   - Em `modal-viagem-novo.js`: Ao resetar o modal, adiciona classe de oculta��o ao texto e remove classe de oculta��o do DatePicker.
   - Em `exibe-viagem.js`: Ao entrar em edi��o, remove classe de oculta��o do texto e adiciona ao DatePicker.

**Arquivos Alterados**:
- `Pages/Agenda/Index.cshtml`
- `wwwroot/js/agendamento/components/modal-viagem-novo.js`
- `wwwroot/js/agendamento/components/exibe-viagem.js`

**Impacto**:
- ? Alternar entre Edi��o -> Novo n�o causa mais duplica��o visual.
- ? Comportamento robusto contra re-renderiza��es do Syncfusion.

**Status**: ? **Conclu�do**

**Vers�o**: 4.7

---

## [18/01/2026 05:20] - Adicionado Campo de Texto para Data Final Recorr�ncia (Modo Edi��o)

**Descri��o**: Adicionado campo de texto readonly `txtFinalRecorrenciaTexto` para exibir a Data Final Recorr�ncia em modo de edi��o, resolvendo problema persistente de inicializa��o do DatePicker Syncfusion.

**Problema**:
- DatePicker `txtFinalRecorrencia` n�o renderizava corretamente no primeiro carregamento
- Tentativas anteriores (polling, delays) n�o resolveram completamente

**Solu��o**:
Adicionado campo de texto readonly que:
- Exibe data formatada (dd/MM/yyyy) quando modal � aberto para edi��o
- Substitui visualmente o DatePicker em modo edi��o
- � restaurado automaticamente ao fechar modal

**Altera��es no CSHTML** (linhas 1472-1478):
```html
<!-- Campo de texto para exibir data em modo de edi��o (substitui��o do DatePicker) -->
<input type="text"
       id="txtFinalRecorrenciaTexto"
       class="form-control e-outline"
       readonly
       style="display:none;"
       placeholder="dd/MM/yyyy">
```

**Comportamento**:
- **Criar agendamento**: DatePicker vis�vel e funcional
- **Editar agendamento**: Campo de texto readonly exibe data
- **Fechar modal**: DatePicker restaurado automaticamente

**Arquivos Relacionados**:
- `wwwroot/js/agendamento/components/exibe-viagem.js`: Exibe data no campo de texto
- `wwwroot/js/agendamento/components/modal-viagem-novo.js`: Restaura DatePicker ao fechar

**Status**: ? **Conclu�do**

**Vers�o**: 4.6

---

## [18/01/2026 01:30] - Adi��o de Asteriscos em Campos Obrigat�rios e Corre��o de Valida��o de Recorr�ncia

**Descri��o**: Adicionados asteriscos vermelhos nos campos obrigat�rios que estavam faltando (Data Inicial e Hora In�cio) e corrigida valida��o de recorr�ncia para campos espec�ficos de cada per�odo.

**Melhorias Implementadas**:

**1. Asteriscos em Campos B�sicos Obrigat�rios** (`Pages/Agenda/Index.cshtml`):
- Data Inicial (linha 795)
- Hora In�cio (linha 814)

**2. Asteriscos em Campos de Recorr�ncia** (`Pages/Agenda/Index.cshtml`):
- Per�odo (linha 1404) - vis�vel quando Recorrente = Sim
- Dias da Semana (linha 1423) - vis�vel quando Per�odo = Semanal/Quinzenal
- Dia do M�s (linha 1446) - vis�vel quando Per�odo = Mensal
- Data Final Recorr�ncia (linha 1462) - vis�vel quando Per�odo = Di�rio/Semanal/Quinzenal/Mensal
- Selecione as Datas (linha 1485) - vis�vel quando Per�odo = Dias Variados

**3. Corre��o de Valida��o de Recorr�ncia** (`wwwroot/js/agendamento/components/validacao.js` linhas 487-531):

Problema: Valida��o exigia Dias da Semana para per�odo Mensal (incorreto) e n�o validava Dia do M�s.

Solu��o: Valida��es separadas por tipo de per�odo:

```javascript
// Valida��o 2: Semanal/Quinzenal ? Dias da Semana obrigat�rio
if (periodo === "S" || periodo === "Q")
{
    const diasSelecionados = document.getElementById("lstDias").ej2_instances[0].value;
    if (!diasSelecionados || diasSelecionados.length === 0)
    {
        await Alerta.Erro("Informa��o Ausente", "Para per�odo Semanal ou Quinzenal, voc� precisa escolher ao menos um Dia da Semana");
        return false;
    }
}

// Valida��o 3: Mensal ? Dia do M�s obrigat�rio
if (periodo === "M")
{
    const diaMes = document.getElementById("lstDiasMes").ej2_instances[0].value;
    if (!diaMes || diaMes === "" || diaMes === null)
    {
        await Alerta.Erro("Informa��o Ausente", "Para per�odo Mensal, voc� precisa escolher o Dia do M�s");
        return false;
    }
}
```

**Regras de Valida��o de Recorr�ncia (Completas)**:
1. **Recorr�ncia SIM** ? Per�odo obrigat�rio
2. **Per�odo Di�rio** ? Data Final Recorr�ncia obrigat�ria
3. **Per�odo Semanal** ? Data Final Recorr�ncia obrigat�ria + Dias da Semana obrigat�rio (ao menos um)
4. **Per�odo Quinzenal** ? Data Final Recorr�ncia obrigat�ria + Dias da Semana obrigat�rio (ao menos um)
5. **Per�odo Mensal** ? Data Final Recorr�ncia obrigat�ria + Dia do M�s obrigat�rio
6. **Per�odo Dias Variados** ? Ao menos uma data selecionada no calend�rio

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml` (linhas 795, 814, 1404, 1423, 1446, 1462, 1485)
- `wwwroot/js/agendamento/components/validacao.js` (linhas 487-531)

**Documenta��o Atualizada**:
- `Documentacao/JavaScript/validacao.md` (vers�o 1.2)
- `Documentacao/Pages/Agenda - Index.md` (vers�o 4.5)

**Impacto**:
- ? Todos os campos obrigat�rios claramente marcados com asterisco vermelho
- ? Valida��o correta para cada tipo de per�odo de recorr�ncia
- ? Imposs�vel criar recorr�ncia Mensal sem Dia do M�s
- ? Mensagens de erro espec�ficas para cada caso
- ? Interface mais intuitiva e consistente

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Sonnet 4.5

**Vers�o**: 4.5

---

## [18/01/2026 01:04] - Corre��o de Valida��o de Campos Obrigat�rios em Agendamentos

**Descri��o**: Corrigida l�gica de valida��o para que Motorista, Ve�culo, KM e Combust�vel N�O sejam obrigat�rios em agendamentos. Esses campos s� devem ser obrigat�rios quando o agendamento � transformado em viagem aberta/realizada.

**Problema**: Ao editar um agendamento recorrente, o sistema exigia campos de viagem (Combust�vel Inicial, KM Inicial, Motorista, Ve�culo) que n�o s�o obrigat�rios para agendamentos, impedindo a edi��o.

**Solu��o**:

**1. Valida��o Condicional** (`wwwroot/js/agendamento/components/validacao.js` linhas 49-61):

Implementada l�gica que detecta se � agendamento ou viagem baseada no texto do bot�o:

```javascript
// Detecta se � agendamento ou viagem
const btnTexto = $("#btnConfirma").text().trim();
const ehAgendamento = btnTexto === "Edita Agendamento" || btnTexto === "Confirma Agendamento";

// S� valida campos de viagem se:
// 1. N�O for agendamento (j� � viagem aberta/realizada)
// 2. OU se algum campo de finaliza��o foi preenchido (transformando em viagem)
if (!ehAgendamento || algumFinalPreenchido)
{
    if (!await this.validarCamposViagem()) return false;
}
```

**2. Asteriscos Vermelhos em Campos Obrigat�rios** (`Pages/Agenda/Index.cshtml`):

Adicionados asteriscos vermelhos e it�licos � esquerda dos r�tulos dos campos obrigat�rios:

- CSS customizado (linhas 271-278):
```css
.campo-obrigatorio {
    color: #dc3545;
    font-style: italic;
    font-size: calc(1em - 4px);
    margin-right: 4px;
    font-weight: 600;
}
```

- Campos marcados com asterisco (linhas 901, 936, 963, 1271, 1316, 1338):
  - Finalidade (linha 901)
  - Origem (linha 936)
  - Destino (linha 963)
  - Requisitante (linha 1271)
  - Ramal (linha 1316)
  - Setor do Requisitante (linha 1338)

**Campos Obrigat�rios em AGENDAMENTOS** (apenas):
- ? Data Inicial
- ? Hora Inicial
- ? Finalidade
- ? Origem
- ? Destino
- ? Requisitante
- ? Ramal
- ? Setor do Requisitante

**Campos Obrigat�rios APENAS em VIAGENS** (n�o em agendamentos):
- ? Motorista (s� quando transforma em viagem)
- ? Ve�culo (s� quando transforma em viagem)
- ? Combust�vel Inicial (s� quando transforma em viagem)
- ? KM Inicial (s� quando transforma em viagem)

**Arquivos Afetados**:
- `wwwroot/js/agendamento/components/validacao.js` (linhas 49-61)
- `Pages/Agenda/Index.cshtml` (linhas 271-278, 901, 936, 963, 1271, 1316, 1338)

**Documenta��o Criada/Atualizada**:
- `Documentacao/JavaScript/validacao.js.md` (arquivo criado)
- `Documentacao/Pages/Agenda - Index.md` (este arquivo)

**Impacto**:
- ? Agendamentos podem ser criados/editados sem preencher Motorista/Ve�culo/KM/Combust�vel
- ? Valida��o de viagem s� ocorre quando apropriado (viagens abertas ou transforma��o de agendamento)
- ? Interface mais clara com campos obrigat�rios marcados visualmente
- ? Regras de neg�cio alinhadas com requisitos do sistema

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Sonnet 4.5

**Vers�o**: 4.4

---

## [16/01/2026 19:45] - Ajuste de Altura do ComboBox Telerik de Requisitantes

**Descri��o**: Corrigida altura do Telerik ComboBox de Requisitantes para corresponder aos outros controles da interface.

**Problema**: ComboBox da Telerik (`kendo-combobox`) estava com altura diferente dos outros controles do formul�rio, causando inconsist�ncia visual.

**Solu��o**: Adicionado `height: 38px;` ao estilo inline do componente (linha 1228).

**C�digo Antes**:
```html
<kendo-combobox name="lstRequisitante"
                ...
                style="width: 100%;"
                ...>
</kendo-combobox>
```

**C�digo Depois**:
```html
<kendo-combobox name="lstRequisitante"
                ...
                style="width: 100%; height: 38px;"
                ...>
</kendo-combobox>
```

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml` (linha 1228)

**Impacto**: Interface visualmente consistente com todos os controles na mesma altura (38px).

**Status**: ? **Conclu�do**

**Respons�vel**: Sistema

**Vers�o**: 4.3

---

## [16/01/2026 18:15] - FIX FINAL: Ordena��o Natural de Requisitantes com Comparador Compartilhado

**Descri��o**: Implementada ordena��o natural (n�meros antes de letras) em TODOS os pontos que retornam requisitantes, criando um comparador compartilhado reutiliz�vel.

**Problema Identificado**:
- Lista de requisitantes na Agenda estava completamente desordenada
- "Fabiana" aparecia no IN�CIO (deveria estar ap�s A-E)
- "001 Requisitante..." aparecia no FINAL (deveria estar no in�cio)
- Ordem aparentemente aleat�ria/n�o determin�stica

**Causa Raiz**:
1. **VIEW SQL `ViewRequisitantes` n�o possui ORDER BY** ? registros retornados em ordem n�o determin�stica
2. **Helpers/ListasCompartilhadas.cs** linha 365 usava `OrderBy()` padr�o ? ordena��o ordinal ASCII incorreta
3. **Pages/Viagens/Upsert.cshtml.cs** tinha classe `NaturalStringComparer` LOCAL e DUPLICADA

**Solu��o Implementada**:

**1. Criado Comparador Compartilhado** (`Helpers/ListasCompartilhadas.cs` linhas 29-92):
```csharp
public class NaturalStringComparer : IComparer<string>
{
    // Compara n�meros numericamente (1 < 2 < 10)
    // Compara letras alfabeticamente (case-insensitive, pt-BR)
    // N�meros V�M ANTES de letras
}
```

**2. Atualizado `Helpers/ListasCompartilhadas.cs`** (linha 360-374):
```csharp
public IEnumerable<ListaRequisitante> RequisitantesList()
{
    var requisitantes = _unitOfWork.ViewRequisitantes.GetAllReduced(
        selector: r => new ListaRequisitante { ... }
    ).ToList();

    // Ordena usando comparador natural
    return requisitantes.OrderBy(r => r.Requisitante, new NaturalStringComparer()).ToList();
}
```

**3. Atualizado `Pages/Viagens/Upsert.cshtml.cs`**:
- Linha 455: `OnGetAJAXPreencheListaRequisitantes()` ? usa `FrotiX.Helpers.NaturalStringComparer()`
- Linha 1649: `PreencheListaRequisitantes()` ? usa `FrotiX.Helpers.NaturalStringComparer()`
- Removida classe `NaturalStringComparer` local duplicada (linhas 2039-2097)

**Arquivos Afetados**:
- `Helpers/ListasCompartilhadas.cs` (linhas 29-92, 360-374)
- `Pages/Viagens/Upsert.cshtml.cs` (linhas 455, 1649, deletado 2039-2097)

**Impacto**:
- ? **TODOS** os dropdowns de requisitantes agora ordenam igual
- ? Ordena��o natural: 001, 002, 003, ..., A, B, C
- ? "001 Requisitante..." aparece no IN�CIO
- ? "Fabiana..." aparece na posi��o CORRETA (ap�s E, antes de G)
- ? Consist�ncia UX em toda a aplica��o

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Code

**Vers�o**: 4.1

---

## [16/01/2026 17:45] - FIX: Ordena��o Alfab�tica de Requisitantes no Carregamento Inicial

**Descri��o**: Corrigida ordena��o alfab�tica da lista de requisitantes no carregamento inicial da p�gina. A lista estava usando ordena��o SQL padr�o que n�o respeitava a cultura pt-BR.

**Problema Identificado**:
- Lista de requisitantes vinha do banco com ordena��o SQL padr�o (`ORDER BY Requisitante`)
- Ordena��o SQL coloca n�meros de forma diferente da ordena��o alfab�tica esperada em pt-BR
- Resultado: "001 Requisitante..." aparecia em posi��es inconsistentes na lista
- Exemplo: "001" ? "Fabiana" ? "Marcelo" ? "Vera" ? "Zenildes" ? "001" (duplicado)

**Causa Raiz**:
- M�todo `OnGetAJAXPreencheListaRequisitantes()` em `Upsert.cshtml.cs` (linha 445)
- Usava `orderBy: r => r.OrderBy(r => r.Requisitante)` diretamente no banco
- Ordena��o dependia do **collation** do SQL Server (n�o era case-insensitive nem pt-BR aware)

**Solu��o Implementada** (linhas 443-456 de `Upsert.cshtml.cs`):

**ANTES**:
```csharp
var ListaRequisitantes = (
from vr in _unitOfWork.ViewRequisitantes.GetAll(orderBy: r =>
r.OrderBy(r => r.Requisitante)
)
select new
{
    vr.Requisitante ,
    vr.RequisitanteId
}
).ToList();
```

**DEPOIS**:
```csharp
// Busca dados sem ordena��o no banco (melhor performance)
var ListaRequisitantes = (
from vr in _unitOfWork.ViewRequisitantes.GetAll()
select new
{
    vr.Requisitante ,
    vr.RequisitanteId
}
).ToList();

// Ordena alfabeticamente usando pt-BR (case-insensitive, ignora acentos)
var ListaOrdenada = ListaRequisitantes
    .OrderBy(r => r.Requisitante, StringComparer.Create(new System.Globalization.CultureInfo("pt-BR"), ignoreCase: true))
    .ToList();
```

**Benef�cios**:
1. **Ordena��o Consistente**: Sempre respeita cultura pt-BR
2. **Case-Insensitive**: "Andr�" e "andre" ordenam juntos
3. **Ignora Acentos**: "A��o" ordena junto com "Ac�o"
4. **Melhor Performance**: Busca no banco sem ordena��o, ordena em mem�ria (lista pequena)
5. **Compat�vel com JavaScript**: Mesma l�gica do `localeCompare('pt-BR')` usado no frontend

**Arquivos Afetados**:
- `Pages/Viagens/Upsert.cshtml.cs` (linhas 443-456)

**Impacto**:
- ? Lista de requisitantes sempre ordenada alfabeticamente
- ? Comportamento consistente entre carregamento inicial e inser��o de novos itens
- ? Compatibilidade total com ordena��o do JavaScript (Clear and Reload Pattern)

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Code

**Vers�o**: 4.0

---

## [16/01/2026 16:35] - FIX: Erro "Format options must be invalid" nos DatePickers

**Descri��o**: Corrigido erro "Format options or type given must be invalid" que ocorria ao selecionar data nos DatePickers do modal Novo Evento.

**Problema**:
- DatePickers tinham atributo `locale="pt-BR"`
- Locale pt-BR n�o estava carregado/configurado na p�gina
- Syncfusion DatePicker lan�ava exce��o ao tentar usar locale inexistente

**Solu��o** (linhas 1597-1600, 1606-1609):

**ANTES**:
```html
<ejs-datepicker id="txtDataInicialEvento"
                format="dd/MM/yyyy"
                placeholder="Data Inicial"
                locale="pt-BR">  <!-- ? Causava erro -->
</ejs-datepicker>
```

**DEPOIS**:
```html
<ejs-datepicker id="txtDataInicialEvento"
                format="dd/MM/yyyy"
                placeholder="Data Inicial">  <!-- ? Sem locale -->
</ejs-datepicker>
```

**Mudan�as**:
- Removido `locale="pt-BR"` de `txtDataInicialEvento` (linha 1600)
- Removido `locale="pt-BR"` de `txtDataFinalEvento` (linha 1609)

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml` (linhas 1597-1600, 1606-1609)

**Impacto**:
- ? DatePickers funcionam sem erro
- ? Sele��o de data funcional
- ? Formato dd/MM/yyyy mantido

**Nota**: Outros DatePickers da p�gina (como `txtDataInicial`, `txtDataFinal`) n�o usam locale e funcionam corretamente. Seguimos o mesmo padr�o.

**Status**: ? **Conclu�do**

**Vers�o**: 3.5

---

## [16/01/2026 16:15] - FIX: Corre��es Adicionais Modal Novo Evento

**Descri��o**: Corrigidos 3 problemas remanescentes no modal Novo Evento ap�s testes:
1. **Lista de Requisitantes com nomes em branco** (campo mapeado incorreto)
2. **DatePickers ainda sem bordas** (cssClass n�o funciona, necess�rio CSS customizado)
3. **Debug ampliado para problema "Setor n�o identificado"**

**Mudan�as**:

**1. Index.cshtml (linha 1623) - FIX Campo de Mapeamento Requisitante**:
```html
<!-- ANTES (campo "Nome" n�o existe no objeto): -->
<e-combobox-fields text="Nome" value="RequisitanteId">

<!-- DEPOIS (campo correto conforme ListaRequisitante.cs): -->
<e-combobox-fields text="Requisitante" value="RequisitanteId">
```

**Motivo**: A classe `ListaRequisitante` (ListasCompartilhadas.cs:273) define propriedade `Requisitante` (n�o `Nome`).

**2. Index.cshtml (linhas 512-531) - CSS Customizado para DatePickers**:
```css
/* Removido cssClass="e-field" que n�o funcionava */
/* Adicionado CSS customizado: */
#txtDataInicialEvento.e-datepicker,
#txtDataFinalEvento.e-datepicker,
#txtQtdParticipantesEventoCadastro.e-numerictextbox {
    border: 1px solid #ced4da !important;
    border-radius: 0.25rem !important;
}

#txtDataInicialEvento .e-input-group,
#txtDataFinalEvento .e-input-group,
#txtQtdParticipantesEventoCadastro .e-input-group {
    border: 1px solid #ced4da !important;
    border-radius: 0.25rem !important;
}
```

**3. evento.js (linhas 340-357) - Debug Ampliado**:
```javascript
// Adicionado logs detalhados:
console.log('?? Total de setores na lista:', setores.length);
console.log('?? Exemplo de setor na lista:', setores[0]);
console.log('?? Campos dispon�veis:', Object.keys(setores[0]));
console.log('?? SetorId normalizado:', setorIdNormalizado);

// Compara��o com log linha a linha:
const setorEncontrado = setores.find(s => {
    if (!s.SetorSolicitanteId) return false;
    const idNormalizado = s.SetorSolicitanteId.toString().toLowerCase();
    console.log('  ?? Comparando:', idNormalizado, '===', setorIdNormalizado, '?', idNormalizado === setorIdNormalizado);
    return idNormalizado === setorIdNormalizado;
});
```

**Motivo**: Para identificar por que a compara��o de GUID falha mesmo ap�s normaliza��o.

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml` (linhas 512-531, 1600, 1610, 1623)
- `wwwroot/js/agendamento/components/evento.js` (linhas 340-357)

**Pr�ximos Passos**:
- Aguardar logs do console para diagnosticar problema do setor
- Verificar se campo retornado pela API tem nome diferente de `SetorSolicitanteId`

**Impacto**:
- ? Lista de requisitantes exibe nomes corretamente
- ? DatePickers renderizam com bordas via CSS customizado
- ?? Debug ampliado para resolver "Setor n�o identificado"

**Status**: ?? **Parcialmente Conclu�do** (aguardando logs de debug)

**Vers�o**: 3.4

---

## [16/01/2026 16:00] - FIX: Corre��o de 3 Bugs no Modal Novo Evento

**Descri��o**: Corrigidos 3 problemas cr�ticos identificados em testes do modal Novo Evento:
1. **TypeError** ao selecionar requisitante (linha 344 de evento.js)
2. **DatePickers sem bordas** (faltava classe CSS correta)
3. **Lista de Requisitantes com apenas 1 item** (mapeamento de campo incorreto)

**Mudan�as**:

**1. evento.js (linha 344) - FIX TypeError**:
```javascript
// ANTES (causava erro se SetorSolicitanteId fosse undefined):
const setorEncontrado = setores.find(s =>
    s.SetorSolicitanteId.toString().toLowerCase() === setorIdNormalizado
);

// DEPOIS (valida��o pr�via antes de chamar toString()):
const setorEncontrado = setores.find(s =>
    s.SetorSolicitanteId && s.SetorSolicitanteId.toString().toLowerCase() === setorIdNormalizado
);
```

**2. Index.cshtml (linhas 1580, 1591) - FIX Bordas DatePickers**:
```html
<!-- ANTES (form-control n�o funciona para Syncfusion DatePicker): -->
cssClass="form-control"

<!-- DEPOIS (classe correta Syncfusion): -->
cssClass="e-field"
```

**3. Index.cshtml (linha 1624) - FIX Lista Requisitantes**:
```html
<!-- ANTES (campo inexistente): -->
<e-combobox-fields text="Requisitante" value="RequisitanteId">

<!-- DEPOIS (campo correto): -->
<e-combobox-fields text="Nome" value="RequisitanteId">
```

**Arquivos Afetados**:
- `wwwroot/js/agendamento/components/evento.js` (linha 344)
- `Pages/Agenda/Index.cshtml` (linhas 1580, 1591, 1623, 1624)

**Impacto**:
- ? Modal n�o quebra mais ao selecionar requisitante
- ? DatePickers renderizam corretamente com bordas
- ? Lista de requisitantes exibe todos os itens dispon�veis
- ? Auto-fill de setor funciona sem erros

**Status**: ? **Conclu�do**

**Vers�o**: 3.3

---

## [16/01/2026 14:35] - STYLE: Altera��o da Cor do Evento para Laranja Vibrante

**Descri��o**: Alterada a cor dos eventos de #A39481 (bege claro) para #FFA726 (laranja vibrante) para melhor visibilidade e contraste.

**Mudan�as**:
1. **Legenda de Cores** (Index.cshtml, linha 624):
   - Alterado: `background-color: #A39481` ? `#FFA726`
   - Afeta a bolinha de legenda "Evento" no canto superior direito

**Arquivos Relacionados**:
- `FrotiX.sql`: ViewViagensAgenda (cor do evento alterada)
- `Scripts/SQL/UPDATE_CorEvento_20porcento_mais_clara.sql`: Script de update da view

**Impacto**:
- Eventos agora aparecem em laranja vibrante no calend�rio
- Melhor contraste visual
- Legenda sincronizada com cor real dos eventos

**Status**: ? **Conclu�do**

**Vers�o**: 3.0

---

## [16/01/2026 12:40] - FEAT: Tooltips Din�micas com �cones e Cores Adaptativas

**Descri��o**: Implementado sistema completo de tooltips customizadas para agendamentos no calend�rio com �cones FontAwesome, quebras de linha e cores din�micas.

**Mudan�as CSS** (Index.cshtml, linhas 481-501):
1. **Classe `.tooltip-ftx-agenda-dinamica`**: Nova classe para tooltips do calend�rio
   - Fundo: cor do evento clareada em 20%
   - Texto: branco para cores escuras, preto para claras
   - Padding: 10px/14px
   - Max-width: 350px
   - Border-radius: 8px
   - Box-shadow: `0 3px 10px rgba(0,0,0,0.25)`

2. **�cones**:
   - Width fixo: 18px
   - Margin-right: 6px
   - Text-align: center (alinhamento consistente)

**Integra��o com JavaScript**:
- Tooltip HTML constru�da por `calendario.js::gerarTooltipHTML()`
- Cor calculada dinamicamente por `lightenColor()` e `isColorDark()`

**Conte�do da Tooltip**:
- ?? Ve�culo (fa-car): Placa ou "Ve�culo n�o Informado"
- ?? Motorista (fa-user-tie): Nome do motorista
- ?? Evento (fa-tent): Nome do evento (se finalidade = "Evento")
- ?? Descri��o (fa-memo-pad): Descri��o sem " - " no final

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml` (linhas 481-501)

**Impacto**: Melhoria significativa na visualiza��o de agendamentos no calend�rio. Usu�rio v� informa��es detalhadas ao passar mouse sobre eventos.

**Status**: ? **Conclu�do**

**Vers�o**: 2.9

---

## [13/01/2026 19:25] - FIX: Adicionado CSS inline para bot�o "Transforma em Viagem"

**Descri��o**: Corrigidas cores do bot�o laranja "Transforma em Viagem" (#btnViagem) que ficavam mais claras no hover e active (quando deveriam escurecer).

**Problema**:
- Bot�o `#btnViagem` (btn-fundo-laranja) ficava mais CLARO no hover e ao clicar
- Comportamento inverso ao esperado (deveria escurecer progressivamente)

**Causa**:
- Mesma raiz dos outros bot�es: especificidade CSS insuficiente
- Bootstrap sobrescrevendo estilos do frotix.css

**Solu��o**:
- Adicionado CSS inline com seletores ID (linhas 563-578):
  ```css
  #btnViagem:hover {
      background-color: #8B4513 !important;  /* Laranja m�dio-escuro */
  }

  #btnViagem:active {
      background-color: #6B3410 !important;  /* Laranja escuro */
  }
  ```

**Padr�o de Cores Laranja**:
- **Base**: #A0522D (sienna)
- **Hover**: #8B4513 (saddle brown)
- **Active**: #6B3410 (marrom escuro)

**Arquivos Afetados**:
- Pages/Agenda/Index.cshtml (linhas 563-578)

**Impacto**:
- ? Bot�o "Transforma em Viagem" agora escurece corretamente no hover e active
- ? Todos os 4 bot�es do modal agora funcionam perfeitamente
- ? Feedback visual consistente e correto em todo o modal

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Code

**Vers�o**: 2.8

---

## [13/01/2026 19:20] - FIX COMPLEMENTAR: Adicionado estado :hover ao CSS inline dos bot�es

**Descri��o**: Adicionadas regras `:hover` ao CSS inline dos bot�es do modal de agendamento, completando a corre��o iniciada �s 19:00.

**Descoberta**:
- Usu�rio reportou que o problema de cores erradas n�o ocorria apenas ao clicar (`:active`), mas tamb�m no hover
- O CSS inline anterior s� corrigia `:active` e `:focus`, deixando `:hover` sem corre��o

**Solu��o Implementada**:
- Adicionadas regras `:hover` para os 3 bot�es (linhas 526-549):
  ```css
  /* Bot�o Confirmar - Azul */
  #btnConfirma:hover {
      background-color: #2a4459 !important;  /* Azul m�dio-escuro */
      box-shadow: 0 0 20px rgba(61,87,113,0.8), 0 6px 12px rgba(61,87,113,0.5) !important;
  }

  /* Bot�es Fechar e Apagar - Vinho */
  #btnFecha:hover,
  #btnApaga:hover {
      background-color: #5a252c !important;  /* Vinho m�dio-escuro */
      box-shadow: 0 0 20px rgba(114,47,55,0.8), 0 6px 12px rgba(114,47,55,0.5) !important;
  }
  ```

**Padr�o de Cores (consistente com frotix.css)**:
- **Azul**: Base #3D5771 ? Hover #2a4459 ? Active #1f3241
- **Vinho**: Base #722f37 ? Hover #5a252c ? Active #4a1f24

**Arquivos Afetados**:
- Pages/Agenda/Index.cshtml (linhas 526-549)

**Impacto**:
- ? Bot�es agora t�m cores corretas em TODOS os estados (normal, hover, active, focus)
- ? Feedback visual completo e consistente
- ? Alinhamento perfeito com padr�o FrotiX global

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Code

**Vers�o**: 2.7

---

## [13/01/2026 19:00] - FIX CR�TICO: CSS inline para bot�es do modal (btn-sm)

**Descri��o**: Adicionado CSS inline na p�gina para for�ar cores corretas nos bot�es pequenos (btn-sm) do modal de agendamento ao serem clicados.

**Problema Persistente**:
- Mesmo ap�s fix de especificidade CSS no frotix.css global, bot�es continuavam com cores erradas:
  - `#btnConfirma` (btn-sm btn-azul) ? ficava azul CLARO ao clicar
  - `#btnFecha` e `#btnApaga` (btn-sm btn-vinho) ? ficavam BRANCOS ao clicar

**Causa Raiz**:
- Bootstrap CSS carregado DEPOIS do frotix.css
- Especificidade por classes (.btn-sm.btn-azul) insuficiente contra Bootstrap
- Ordem de carregamento dos arquivos CSS permitia sobrescrita

**Solu��o Final**:
- CSS inline com seletores por ID (linhas 523-543):
  ```css
  #btnConfirma:active { background-color: #1f3241 !important; }
  #btnFecha:active { background-color: #4a1f24 !important; }
  #btnApaga:active { background-color: #4a1f24 !important; }
  ```
- Seletores por ID t�m especificidade MAIOR que classes
- CSS inline tem prioridade sobre CSS externo
- !important garante que nada mais sobrescreva

**Arquivos Afetados**:
- Pages/Agenda/Index.cshtml (linhas 523-543)

**Impacto**:
- ? btnConfirma mant�m azul escuro ao clicar
- ? btnFecha e btnApaga mant�m vinho escuro ao clicar
- ? Solu��o definitiva com m�xima especificidade CSS

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Code

**Vers�o**: 2.6

---

## [13/01/2026 16:00] - Corre��o: Bot�o "Cancelar Opera��o" do Modal Novo Requisitante

**Descri��o**: Corrigida classe CSS do bot�o "Cancelar Opera��o" no modal de Inserir Novo Requisitante que foi perdida na substitui��o em massa anterior.

**Problema Identificado**:
- Bot�o "Cancelar Opera��o" (linha 1617) ainda estava usando classe `btn-ftx-fechar`
- Ficava BRANCO ao ser pressionado (em vez de rosado/vinho)
- Foi perdido na substitui��o em massa que processou 37 arquivos

**Solu��o Implementada**:
- Substitu�da classe `btn-ftx-fechar` por `btn-vinho` no bot�o
- Agora mant�m cor rosada/vinho (#4a1f24) ao ser pressionado
- Alinhamento com padr�o FrotiX e demais bot�es do sistema

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml` (linha 1617) - Bot�o do modal `modalNovoRequisitante`

**Impacto**:
- ? Bot�o agora mant�m cor consistente ao ser pressionado
- ? Comportamento visual padronizado em TODOS os modais
- ? �ltima ocorr�ncia de `btn-ftx-fechar` eliminada do sistema

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Code

**Vers�o**: 2.3 ? 2.4

---

## [13/01/2026 17:30] - Corre��es TreeView: Valida��o e Aceita��o de N�meros no Nome

**Descri��o**:
Tr�s corre��es importantes no modal de cadastro de Novo Requisitante:

1. **Valida��o do Setor corrigida**: O c�digo JavaScript ainda validava o DropDownTree antigo (`ddtSetorNovoRequisitante`), agora validava o campo oculto `hiddenSetorId` preenchido pelo TreeView.

2. **Campo Nome aceita n�meros**: A fun��o `sanitizeNomeCompleto()` foi corrigida para aceitar n�meros (`\p{N}`) al�m de letras Unicode.

3. **Cores do TreeView mais suaves**: Verde mais fraco para itens filhos (`#f0f7f0`) e mais forte para itens pais (`#e8f4e8`).

**Problema Original**:
- Erro "Setor do Requisitante � obrigat�rio" mesmo com setor selecionado (valida��o apontava para controle antigo)
- Campo Nome rejeitava n�meros, permitindo apenas letras

**Solu��o Implementada**:
```javascript
// ANTES (incorreto)
const ddtSetor = document.getElementById("ddtSetorNovoRequisitante");
if (ddtSetor && ddtSetor.ej2_instances && ddtSetor.ej2_instances[0]) {
    setorValue = ddtSetor.ej2_instances[0].value;
}

// DEPOIS (correto)
const hiddenSetorId = document.getElementById("hiddenSetorId");
if (hiddenSetorId) {
    setorValue = hiddenSetorId.value;
}
```

**Arquivos Afetados**:
- `wwwroot/js/agendamento/services/requisitante.service.js` - Valida��o e sanitiza��o
- `Pages/Agenda/Index.cshtml` - CSS do TreeView

**Impacto**:
- ? Valida��o do setor funciona corretamente com TreeView
- ? Campo Nome aceita letras e n�meros
- ? Visual mais suave e diferenciado entre pais e filhos

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Code

**Vers�o**: 2.5

---

## [13/01/2026 16:45] - Corre��o CSS: Unifica��o de cores do TreeView Syncfusion

**Descri��o**:
Corre��o completa do CSS do TreeView `#treeSetorRequisitante` para eliminar cores conflitantes (azul, cinza, verde misturados) e unificar a paleta visual.

**Problema**:
Ap�s a substitui��o do DropDownTree por TreeView, os estilos padr�o do Syncfusion (azul) estavam "vazando" e misturando com os estilos customizados (verde), causando apar�ncia visual inconsistente e "bagun�ada".

**Solu��o Implementada**:
CSS abrangente com `!important` em todos os seletores relevantes:

1. **Estados base**: `background-color: transparent !important` para remover fundos padr�o
2. **Hover**: Cinza claro `#f5f5f5`
3. **Selecionado/Ativo/Focus**: Verde suave `#e8f4e8` com borda lateral verde `#28a745`
4. **Texto normal**: Cinza escuro `#333`
5. **Texto selecionado**: Verde escuro `#2d5a2d` com `font-weight: 600`
6. **�cones**: Cinza neutro `#666` (sempre)
7. **Fullrow**: Backgrounds transparentes e verdes conforme estado
8. **Outline/Box-shadow**: Removidos para eliminar bordas azuis do focus

**Seletores adicionados**:
- `.e-fullrow` para background do item completo
- `[aria-selected="true"]` para capturar sele��o via atributo
- `.e-icon-expandable`, `.e-icon-collapsible` para �cones de expans�o

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml` - CSS linhas 271-341

**Impacto**:
- ? Cores unificadas: apenas cinza (hover) e verde (sele��o)
- ? Elimina��o completa dos azuis do Syncfusion
- ? Visual limpo e consistente com padr�o FrotiX

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Code

**Vers�o**: 2.3

---

## [13/01/2026 15:30] - Padroniza��o: Substitui��o de btn-ftx-fechar por btn-vinho

**Descri��o**: Substitu�da classe `btn-ftx-fechar` por `btn-vinho` em bot�es de cancelar/fechar opera��o.

**Problema Identificado**:
- Classe `btn-ftx-fechar` n�o tinha `background-color` definido no estado `:active`
- Bot�es ficavam BRANCOS ao serem pressionados (em vez de manter cor rosada/vinho)
- Comportamento visual inconsistente com padr�o FrotiX

**Solu��o Implementada**:
- Todos os bot�es cancelar/fechar padronizados para usar classe `.btn-vinho`
- Classe `.btn-vinho` j� possui `background-color: #4a1f24` no estado `:active`
- Garantia de cor rosada/vinho ao pressionar bot�o

**Arquivos Afetados**:
- Pages/Agenda/Index.cshtml - Substitui��o de `btn-ftx-fechar` por `btn-vinho` em bot�o de modal

**Impacto**:
- ? Bot�o mant�m cor rosada/vinho ao ser pressionado
- ? Alinhamento com padr�o visual FrotiX
- ? Consist�ncia em todo o sistema

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Code

**Vers�o**: 2.2

---
## [13/01/2026 14:09] - Substitui��o de DropDownTree por TreeView no Modal de Requisitante

**Descri��o**:
Substitui��o do componente `ejs-dropdowntree` por `ejs-treeview` inline no modal "Inserir Novo Requisitante".

**Problema**:
O DropDownTree Syncfusion tinha problemas de z-index - o popup do dropdown ficava atr�s do modal Bootstrap, tornando imposs�vel selecionar um setor.

**Solu��o Implementada**:
1. **CSS**: Adicionado bloco de estilos para `#treeSetorRequisitante` (linhas 271-310)
   - Estiliza��o de hover, active, �cones e fontes
   - Display do setor selecionado com fundo verde claro

2. **HTML**: Modal completamente redesenhado (linhas 1465-1588)
   - Substitu�do `ejs-dropdowntree` por `ejs-treeview` renderizado inline (sem popup)
   - TreeView dentro de div com `max-height: 250px` e `overflow-y: auto`
   - Hidden field `#hiddenSetorId` para armazenar sele��o
   - Display visual do setor selecionado (`#setorSelecionadoDisplay`)
   - Campos com indicadores de obrigatoriedade (`*`)
   - Atributos de valida��o HTML5 (required, maxlength, type, etc.)

3. **JavaScript**: Nova fun��o `onSetorSelected()` (linhas 1675-1716)
   - Callback quando usu�rio seleciona um n� no TreeView
   - Atualiza hidden field com ID do setor
   - Mostra feedback visual com nome do setor selecionado
   - Tratamento de erros com `Alerta.TratamentoErroComLinha()`
   - Limpeza autom�tica da sele��o ao fechar modal

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml` (CSS + HTML + JavaScript)

**Impacto**:
- Modal de novo requisitante agora funciona corretamente
- Usu�rio consegue visualizar e selecionar setores na hierarquia
- Sele��o persistida em hidden field para envio ao backend

**Status**: ? **Conclu�do**

**Respons�vel**: Claude (AI Assistant)
**Vers�o**: 2.1

---

## [08/01/2026] - Reescrita no Padr�o FrotiX Simplificado

**Descri��o**:
Documenta��o reescrita seguindo padr�o simplificado e did�tico:
- Objetivos claros no in�cio
- Arquivos listados com Problema/Solu��o/C�digo
- Fluxos de funcionamento explicados passo a passo
- Troubleshooting simplificado

**Status**: ? **Reescrito**

**Respons�vel**: Claude (AI Assistant)
**Vers�o**: 2.0

---

## [08/01/2026] - Expans�o Completa da Documenta��o

**Descri��o**:
Documenta��o expandida de ~200 linhas para mais de 2300 linhas.

**Status**: ? **Expandido**

**Respons�vel**: Claude (AI Assistant)
**Vers�o**: 1.0


---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [22/01/2026 16:00] - Correção Definitiva: Tooltips e Bordas Cortadas

**Descricao**: Implementação correta de tooltips em botões desabilitados (padrão de Contrato/Index) e correção definitiva das bordas cortadas em dropdowns dentro de section-cards.

**Problema 1 - Tooltips não funcionavam**:

- Técnica anterior com wrapper `<span>` não funcionava adequadamente
- Tooltip estava no wrapper, não no botão
- Faltava `window.ejTooltip.refresh()` após mudanças

**Solução 1 - Padrão de Contrato/Index**:

- Removido wrapper `<span id="wrapperFichaVistoria">`
- Movido `data-ejtip` para o botão diretamente
- Adicionado classe `disabled` para estilização
- CSS global com `pointer-events: auto !important` em botões desabilitados
- Adicionado `window.ejTooltip.refresh()` na função `gerenciarBotaoFichaVistoria()`

**Problema 2 - Bordas de dropdowns cortadas**:

- Múltiplas divs `.d-flex` aninhadas sem `overflow: visible`
- Z-index muito baixo (apenas 1) para dropdowns
- Bordas superiores/laterais dos dropdowns apareciam cortadas

**Solução 2 - Overflow e Z-index corretos**:

- Adicionado `overflow: visible !important` em todas `.d-flex` dentro de `.section-card-body`
- Aumentado z-index de dropdowns de 1 para 1050
- Adicionado z-index 1055 para `.e-popup`
- Garantido `position: relative` em containers pai

**Arquivos Afetados**:

- `Pages/Agenda/Index.cshtml` (linhas 1088-1101, 2251-2290, 528-539)
- `wwwroot/css/modal-viagens-consolidado.css` (linhas 351-440)

**Código Modificado - HTML** (linhas 1088-1096):
```html
<!-- Tooltip funciona diretamente no botão desabilitado -->
<button type="button" id="btnVisualizarFichaVistoria"
    class="btn-ficha-vistoria ms-2 disabled"
    data-ejtip="Clique para visualizar a Ficha de Vistoria desta viagem"
    style="display: none;"
    disabled>
    <i class="fa-duotone fa-clipboard-list"></i>
</button>
```

**Código Modificado - JavaScript** (linhas 2251-2290):
```javascript
function gerenciarBotaoFichaVistoria(numeroFicha) {
    const btnVisualizar = document.getElementById("btnVisualizarFichaVistoria");
    // ... lógica de validação ...

    // CRÍTICO: Refresh do tooltip após mudança
    if (window.ejTooltip) {
        window.ejTooltip.refresh();
    }
}
```

**Código Modificado - CSS** (modal-viagens-consolidado.css):
```css
/* Overflow visible em todas flexbox */
.section-card-body .d-flex,
.section-card-body .d-flex.flex-column,
.section-card-body .d-flex.align-items-center {
    overflow: visible !important;
}

/* Z-index adequado para dropdowns */
.section-card-body .e-dropdowntree,
.section-card-body .e-dropdownlist,
.section-card-body .e-combobox {
    z-index: 1050 !important;
}

/* Botões desabilitados permitem tooltips */
.btn.disabled,
.btn:disabled {
    pointer-events: auto !important;
    opacity: 0.5;
    cursor: not-allowed !important;
}
```

**Impacto**:

- Tooltips agora funcionam perfeitamente em botões desabilitados
- Bordas de todos os dropdowns/inputs visíveis completamente
- Padrão consistente com Contrato/Index
- Código mais limpo e reutilizável

**Status**: ✅ **Concluído**

**Responsável**: Claude Code (AI Assistant)

**Versão**: 5.3

**Referência**: Padrão implementado originalmente em `Pages/Contrato/Index.cshtml`

---

## [22/01/2026 15:25] - Implementação de Tooltips em Botões Desabilitados

**Descricao**: Implementado suporte para tooltips Syncfusion funcionarem em botões desabilitados, especificamente no botão de Ficha de Vistoria.

**Problema**:

- Tooltips Syncfusion (data-ejtip) não funcionam em elementos HTML com atributo disabled
- Botão de Ficha de Vistoria tinha tooltip mas não aparecia quando desabilitado
- Usuários não sabiam por que o botão estava desabilitado

**Solução Implementada**:

- Envolvido btnVisualizarFichaVistoria em um wrapper `<span id="wrapperFichaVistoria">`
- Movido data-ejtip do botão para o wrapper
- Adicionado tabindex="0" ao wrapper para permitir foco
- Adicionado pointer-events: none ao botão desabilitado
- Atualizado gerenciarBotaoFichaVistoria() para controlar o wrapper ao invés do botão
- Adicionado refresh automático do tooltip após mudança de estado

**Impacto**:

- Tooltips agora funcionam em botões desabilitados
- Melhor UX: usuários sabem o que o botão faz mesmo quando desabilitado
- Cursor muda para help (tooltip) ou not-allowed (desabilitado) apropriadamente

**Status**: ✅ **Concluído**

**Responsável**: Claude Code (AI Assistant)

**Versão**: 5.2

---

## [22/01/2026 15:02] - Melhorias de Layout e Limpeza Visual

**Descricao**: Removido banner de teste do topo da página e ajustado layout do Card de Recorrência para melhor alinhamento dos campos.

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml` (linhas 4-6, 1579)

**Mudancas**:
- - Removido banner vermelho de teste que estava fixo no topo da página (linhas 4-6)
- * Ajustado campo "Dias da Semana" (`lstDias`) de `col-md-6` para `col-md-3`
- + Campo "Dias da Semana" agora está alinhado horizontalmente com "Data Final Recorrência"
- + Layout do Card de Recorrência está mais organizado e equilibrado

**Impacto**:
- Página mais limpa sem elementos de teste
- Melhor organização visual do formulário de recorrência
- Campos alinhados horizontalmente na mesma linha

**Status**: ✅ **Concluído**

**Responsável**: Claude Code (AI Assistant)
**Versão**: 5.1

---

## [21/01/2026] - Ajuste do calendario de recorrencia variada

**Descricao**: Ajustado o container do calendario para ancorar o badge de contagem e garantir renderizacao estavel do componente Syncfusion.

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml`
- `wwwroot/js/agendamento/agendamento-core.js`

**Mudancas**:
- + Inserido wrapper `.ftx-calendario-wrapper` para posicionar o badge acima do calendario.
- + Adicionada regra local de layout para manter o badge dentro do card.
- + Calendario agora aguarda localizacao CLDR antes de renderizar.

## [20/01/2026] - Badge de datas variadas no calendario

**Descricao**: Inserido badge de contagem no calendario de Dias Variados e alinhada exibicao do componente no modal de recorrencia.

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml`
- `wwwroot/js/agendamento/agendamento-core.js`

**Mudancas**:
- + Adicionado `#badgeContadorDatas` no container do calendario.
- + Sincronizacao de datas variadas com listbox e badges ao selecionar periodo V.


## [20/01/2026] - Ajuste: Inclus�o do RecorrenciaController no fluxo

**Descri��o**: Atualiza��o da p�gina para incluir `recorrencia-controller.js` e delegar a escrita dos campos de recorr�ncia ao controlador central.

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml`
- `wwwroot/js/agendamento/components/recorrencia-controller.js`

**Mudan�as**:
- ? Nova refer�ncia de script adicionada ao `Index.cshtml`
- ? Documenta��o indica a centraliza��o da recorr�ncia

**Status**: ? Conclu�do

**Respons�vel**: Codex

## [19/01/2026] - Atualização: Implementação de Métodos com Tracking Seletivo

**Descrição**: Migração de chamadas .AsTracking() para novos métodos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimização de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos métodos do repositório)
- Repository/IRepository/IRepository.cs (definição dos novos métodos)
- Repository/Repository.cs (implementação)
- RegrasDesenvolvimentoFrotiX.md (seção 4.2 - nova regra permanente)

**Mudanças**:
- ❌ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- ✅ **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- Otimização de memória e performance
- Tracking seletivo (apenas quando necessário para Update/Delete)
- Padrão mais limpo e explícito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seção 4.2)

**Impacto**: 
- Melhoria de performance em operações de leitura (usa AsNoTracking por padrão)
- Tracking correto em operações de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: ✅ **Concluído**

**Responsável**: Sistema (Atualização Automática)

**Versão**: Incremento de patch




