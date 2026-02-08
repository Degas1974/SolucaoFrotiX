# wwwroot/js/agendamento/components/recorrencia.js

**ARQUIVO NOVO** | 377 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```javascript
class GerenciadorRecorrencia
{
    constructor()
    {
        this.datasSelecionadas = [];
    }

    ajustarDataInicialRecorrente(tipoRecorrencia)
    {
        try
        {
            const datas = [];

            if (tipoRecorrencia === "V")
            {
                this.gerarRecorrenciaVariada(datas);
                return datas.length > 0 ? datas : null;
            }

            let dataAtual = window.getKendoDateValue("txtDataInicial");
            const dataFinal = window.getKendoDateValue("txtFinalRecorrencia");

            if (!dataAtual || !dataFinal)
            {
                console.error("Data Inicial ou Data Final não encontrada.");
                return null;
            }

            dataAtual = moment(dataAtual).toISOString().split("T")[0];
            const dataFinalFormatada = moment(dataFinal).toISOString().split("T")[0];

            let diasSelecionados = document.getElementById("lstDias")?.ej2_instances?.[0]?.value || [];

            if (tipoRecorrencia === "M")
            {
                diasSelecionados = [].concat(document.getElementById("lstDiasMes")?.ej2_instances?.[0]?.value || []);
            }

            let diasSelecionadosIndex = [];
            if (tipoRecorrencia !== "M")
            {
                diasSelecionadosIndex = diasSelecionados.map(dia => ({
                    Sunday: 0,
                    Monday: 1,
                    Tuesday: 2,
                    Wednesday: 3,
                    Thursday: 4,
                    Friday: 5,
                    Saturday: 6
                }[dia]));
            }

            if (tipoRecorrencia === "D")
            {
                this.gerarRecorrenciaDiaria(dataAtual, dataFinalFormatada, datas);
            } else if (tipoRecorrencia === "M")
            {
                this.gerarRecorrenciaMensal(dataAtual, dataFinalFormatada, diasSelecionados, datas);
            } else
            {
                this.gerarRecorrenciaPorPeriodo(tipoRecorrencia, dataAtual, dataFinalFormatada, diasSelecionadosIndex, datas);
            }

            return datas.length > 0 ? datas : null;

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "ajustarDataInicialRecorrente", error);
            return null;
        }
    }

    gerarRecorrenciaVariada(datas)
    {
        try
        {
            const calendarObj = document.getElementById("calDatasSelecionadas")?.ej2_instances?.[0];

            if (!calendarObj || !calendarObj.values || calendarObj.values.length === 0)
            {
                console.error("Nenhuma data selecionada no calendário para recorrência do tipo 'V'.");
                return;
            }

            console.log("📅 [Variada] Datas selecionadas no calendário:", calendarObj.values);

            const datasOrdenadas = calendarObj.values
                .filter(date => date)
                .map(date => new Date(date))
                .sort((a, b) => a - b);

            console.log("📅 [Variada] Datas ordenadas:", datasOrdenadas);

            datasOrdenadas.forEach(date =>
            {
                try
                {
                    datas.push(moment(date).format("YYYY-MM-DD"));
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaVariada_forEach", error);
                }
            });

            console.log("✅ [Variada] Array de datas gerado:", datas);

            console.log("🔧 [Variada] Campo DatasSelecionadas será ignorado - enviando datas individualmente");

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaVariada", error);
        }
    }

    gerarRecorrenciaMensal(dataAtual, dataFinal, diasSelecionados, datas)
    {
        try
        {
            dataAtual = moment(dataAtual);
            dataFinal = moment(dataFinal);

            while (dataAtual.isSameOrBefore(dataFinal))
            {
                const mesAtual = dataAtual.month();
                const anoAtual = dataAtual.year();

                diasSelecionados.forEach(diaDoMes =>
                {
                    const dataEspecifica = moment([anoAtual, mesAtual, diaDoMes]);

                    if (dataEspecifica.isValid() &&
                        dataEspecifica.month() === mesAtual &&
                        dataEspecifica.isSameOrAfter(moment(dataAtual).startOf("day")) &&
                        dataEspecifica.isSameOrBefore(dataFinal))
                    {
                        if (!datas.includes(dataEspecifica.format("YYYY-MM-DD")))
                        {
                            datas.push(dataEspecifica.format("YYYY-MM-DD"));
                        }
                    }
                });

                dataAtual.add(1, "month").startOf("month");
            }

            datas.sort((a, b) => moment(a).diff(moment(b)));

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaMensal", error);
        }
    }

    gerarRecorrenciaDiaria(dataAtual, dataFinal, datas)
    {
        try
        {
            dataAtual = moment(dataAtual);
            dataFinal = moment(dataFinal);

            while (dataAtual.isSameOrBefore(dataFinal))
            {
                const dayOfWeek = dataAtual.isoWeekday();
                if (dayOfWeek >= 1 && dayOfWeek <= 5)
                {
                    datas.push(dataAtual.format("YYYY-MM-DD"));
                }
                dataAtual.add(1, "days");
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaDiaria", error);
        }
    }

    gerarRecorrenciaPorPeriodo(tipoRecorrencia, dataAtual, dataFinal, diasSelecionadosIndex, datas)
    {
        try
        {
            dataAtual = moment(dataAtual);
            dataFinal = moment(dataFinal);

            if (tipoRecorrencia === "Q")
            {
                dataAtual = moment(dataAtual).day(8);
            }

            while (dataAtual.isSameOrBefore(dataFinal))
            {
                diasSelecionadosIndex.forEach(diaSelecionado =>
                {
                    let proximaData = moment(dataAtual).day(diaSelecionado);
                    if (proximaData.isBefore(dataAtual)) proximaData.add(1, "week");
                    if (proximaData.isSameOrBefore(dataFinal) && !datas.includes(proximaData.format("YYYY-MM-DD")))
                    {
                        datas.push(proximaData.format("YYYY-MM-DD"));
                    }
                });

                switch (tipoRecorrencia)
                {
                    case "S":
                        dataAtual.add(1, "week");
                        break;
                    case "Q":
                        dataAtual.add(2, "weeks");
                        break;
                    default:
                        console.error("Tipo de recorrência inválido: ", tipoRecorrencia);
                        return;
                }

                if (dataAtual.isAfter(dataFinal)) break;
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaPorPeriodo", error);
        }
    }

    async handleRecurrence(periodoRecorrente, datasRecorrentes)
    {
        try
        {
            if (!datasRecorrentes || datasRecorrentes.length === 0)
            {
                console.error("❌ Nenhuma data inicial válida retornada para o período.");
                AppToast.show("Vermelho", "Erro: Nenhuma data válida para a recorrência", 3000);
                throw new Error("Nenhuma data válida para recorrência");
            }

            console.log(`📅 Processando ${datasRecorrentes.length} agendamento(s) recorrente(s)...`);
            console.log(`📋 Datas a processar:`, datasRecorrentes);

            console.log("📤 Criando primeiro agendamento...");
            let primeiroAgendamento = window.criarAgendamento(null, null, datasRecorrentes[0]);

            if (!primeiroAgendamento)
            {
                console.error("❌ criarAgendamento retornou NULL");
                throw new Error("Erro ao criar objeto do primeiro agendamento");
            }

            primeiroAgendamento.RecorrenciaViagemId = "00000000-0000-0000-0000-000000000000";

            let agendamentoObj;
            let recorrenciaViagemId = null;

            try
            {
                agendamentoObj = await window.enviarNovoAgendamento(primeiroAgendamento, false);

                if (!agendamentoObj)
                {
                    console.error("❌ Primeiro agendamento falhou - resposta vazia");
                    throw new Error("Erro ao criar o primeiro agendamento.");
                }

                console.log("📦 Resposta completa do primeiro agendamento:", agendamentoObj);

                recorrenciaViagemId = agendamentoObj.novaViagem?.viagemId ||
                    agendamentoObj.viagemId ||
                    agendamentoObj.data?.viagemId ||
                    agendamentoObj.data ||
                    agendamentoObj.id ||
                    null;

                if (!recorrenciaViagemId || recorrenciaViagemId === "00000000-0000-0000-0000-000000000000")
                {

                    if (agendamentoObj.data && typeof agendamentoObj.data === 'object')
                    {
                        recorrenciaViagemId = agendamentoObj.data.viagemId ||
                            agendamentoObj.data.id ||
                            agendamentoObj.data.value?.viagemId ||
                            null;
                    }
                }

                if (!recorrenciaViagemId || recorrenciaViagemId === "00000000-0000-0000-0000-000000000000")
                {
                    console.error("❌ ViagemId não retornado pela API");
                    console.error(" Estrutura da resposta:", JSON.stringify(agendamentoObj, null, 2));

                    console.warn("⚠️ Continuando sem RecorrenciaViagemId (será gravado como GUID vazio)");
                }

                console.log("✅ Primeiro agendamento criado:");
                console.log(" 📅 Data:", datasRecorrentes[0]);
                console.log(" 🔑 ViagemId capturado:", recorrenciaViagemId || "NÃO CAPTURADO");
            }
            catch (error)
            {
                console.error("❌ Falha no primeiro agendamento:", error);
                Alerta.TratamentoErroComLinha("recorrencia.js", "handleRecurrence_primeiro", error);
                AppToast.show("Vermelho", "Erro ao criar o primeiro agendamento", 3000);
                throw error;
            }

            if (datasRecorrentes.length > 1)
            {
                console.log(`📤 Criando ${datasRecorrentes.length - 1} agendamento(s) subsequente(s)...`);
                console.log(`🔗 Usando RecorrenciaViagemId: ${recorrenciaViagemId || "GUID VAZIO"}`);

                let sucessos = 0;
                let falhas = 0;

                for (let i = 1; i < datasRecorrentes.length; i++)
                {
                    const dataAtual = datasRecorrentes[i];

                    console.log(`\n 📤 Criando agendamento ${i}/${datasRecorrentes.length - 1}...`);
                    console.log(` 📅 Data: ${dataAtual}`);
                    console.log(` 🔗 RecorrenciaViagemId: ${recorrenciaViagemId || "00000000-0000-0000-0000-000000000000"}`);

                    const agendamentoSubsequente = window.criarAgendamento(
                        null,
                        recorrenciaViagemId || "00000000-0000-0000-0000-000000000000",
                        dataAtual
                    );

                    if (!agendamentoSubsequente)
                    {
                        falhas++;
                        console.error(` ❌ Falha ao criar objeto do agendamento ${i}`);
                        continue;
                    }

                    try
                    {
                        await window.enviarNovoAgendamento(
                            agendamentoSubsequente,
                            false
                        );

                        sucessos++;
                        console.log(` ✅ Agendamento ${i} criado com sucesso`);
                    }
                    catch (error)
                    {
                        falhas++;
                        console.error(` ❌ Falha no agendamento ${i}:`, error);
                        Alerta.TratamentoErroComLinha("recorrencia.js", "handleRecurrence_subsequente", error);
                    }
                }

                console.log(`\n📊 Resultado: ${sucessos + 1}/${datasRecorrentes.length} agendamentos criados`);

                if (falhas > 0)
                {
                    const mensagem = `${sucessos + 1} de ${datasRecorrentes.length} agendamentos criados. ${falhas} falharam.`;
                    console.warn("⚠️", mensagem);
                    AppToast.show("Amarelo", mensagem, 5000);

                    return {
                        sucesso: true,
                        totalCriados: sucessos + 1,
                        totalFalhas: falhas,
                        parcial: true
                    };
                }
            }

            console.log("✅ Todos os agendamentos foram criados!");
            return {
                sucesso: true,
                totalCriados: datasRecorrentes.length,
                totalFalhas: 0,
                parcial: false
            };

        }
        catch (error)
        {
            console.error("❌ Erro geral em handleRecurrence:", error);
            Alerta.TratamentoErroComLinha("recorrencia.js", "handleRecurrence", error);
            throw error;
        }
    }

    atualizarCalendarioExistente(datas)
    {
        try
        {
            const selectedDates = datas.map(data => new Date(data));
            const calendarObj = document.getElementById("calDatasSelecionadas").ej2_instances[0];

            calendarObj.values = selectedDates;
            calendarObj.refresh();
            calendarObj.isMultiSelection = false;

            const readOnlyElement = document.getElementById('readOnly-checkbox');
            if (readOnlyElement)
            {
                readOnlyElement.checked = true;
                readOnlyElement.disabled = true;
            }

            console.log("Calendário atualizado para modo somente leitura.");
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "atualizarCalendarioExistente", error);
        }
    }

    async corrigirRecorrenciaViagemId(viagemId)
    {
        try
        {
            console.log("🔧 Corrigindo RecorrenciaViagemId do primeiro registro...");

            const payload = {
                ViagemId: viagemId,
                RecorrenciaViagemId: viagemId
            };

            console.log("📤 Enviando correção:", payload);

            console.warn("⚠️ API de correção não implementada - o backend deve atualizar RecorrenciaViagemId = ViagemId para o primeiro registro");

            return true;
        }
        catch (error)
        {
            console.error("❌ Erro ao corrigir RecorrenciaViagemId:", error);
            return false;
        }
    }
}

window.gerenciadorRecorrencia = new GerenciadorRecorrencia();

window.ajustarDataInicialRecorrente = function (tipoRecorrencia)
{
    return window.gerenciadorRecorrencia.ajustarDataInicialRecorrente(tipoRecorrencia);
};

window.handleRecurrence = function (periodoRecorrente, datasRecorrentes)
{
    return window.gerenciadorRecorrencia.handleRecurrence(periodoRecorrente, datasRecorrentes);
};

window.atualizarCalendarioExistente = function (datas)
{
    return window.gerenciadorRecorrencia.atualizarCalendarioExistente(datas);
};

window.corrigirRecorrenciaViagemId = function (viagemId)
{
    return window.gerenciadorRecorrencia.corrigirRecorrenciaViagemId(viagemId);
};

console.log("✅ GerenciadorRecorrencia inicializado");
```
