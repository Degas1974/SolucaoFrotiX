# wwwroot/js/agendamento/components/recorrencia-logic.js

**ARQUIVO NOVO** | 1000 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```javascript
window.calendario = null;
window.datasSelecionadas = [];
window.ignorarEventosRecorrencia = false;

window.inicializarLogicaRecorrencia = function ()
{
    try
    {
        console.log("ðŸ”§ Inicializando lógica de recorrência...");

        if (window.inicializarDropdownPeriodos)
        {
            console.log("ðŸ“‹ Inicializando dropdown de perí­odos...");
            window.inicializarDropdownPeriodos();
        }
        else
        {
            console.warn("âš ï¸ Função inicializarDropdownPeriodos não encontrada");
        }

        setTimeout(() =>
        {

            esconderTodosCamposRecorrencia();

            setTimeout(() =>
            {
                const lstRecorrenteElement = document.getElementById("lstRecorrente");
                if (lstRecorrenteElement && lstRecorrenteElement.ej2_instances)
                {
                    const lstRecorrente = lstRecorrenteElement.ej2_instances[0];
                    if (lstRecorrente)
                    {

                        console.log("ðŸ” DataSource de lstRecorrente:", lstRecorrente.dataSource);

                        const itemNao = lstRecorrente.dataSource?.find(item =>
                            item.Descricao === "Não" ||
                            item.Descricao === "Nao" ||
                            item.RecorrenteId === "N"
                        );

                        if (itemNao)
                        {
                            console.log("ðŸ“‹ Item 'Não' encontrado:", itemNao);
                            lstRecorrente.value = itemNao.RecorrenteId;
                            lstRecorrente.dataBind();

                            console.log("âœ… lstRecorrente definido como 'Não' (padrío)");
                        }
                        else
                        {
                            console.warn("âš ï¸ Item 'Não' não encontrado no dataSource");
                        }
                    }
                    else
                    {
                        console.warn("âš ï¸ Instância lstRecorrente não encontrada");
                    }
                }
                else
                {
                    console.warn("âš ï¸ lstRecorrente não encontrado no DOM");
                }
            }, 200);

            configurarEventHandlerRecorrente();
            configurarEventHandlerPeriodo();

            console.log("âœ… Lógica de recorrência inicializada");

        }, 300);

    } catch (error)
    {
        console.error("âŒ Erro ao inicializar lógica de recorrência:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-logic.js", "inicializarLogicaRecorrencia", error);
        }
    }
};
{
    try
    {
        console.log("ðŸ”§ Inicializando lógica de recorrência...");

        esconderTodosCamposRecorrencia();

        setTimeout(() =>
        {
            const lstRecorrenteElement = document.getElementById("lstRecorrente");
            if (lstRecorrenteElement && lstRecorrenteElement.ej2_instances)
            {
                const lstRecorrente = lstRecorrenteElement.ej2_instances[0];
                if (lstRecorrente)
                {
                    lstRecorrente.value = "N";
                    lstRecorrente.dataBind();
                    console.log("âœ… lstRecorrente definido como 'Não'");
                }
            }
        }, 100);

        configurarEventHandlerRecorrente();

        configurarEventHandlerPeriodo();

        console.log("âœ… Lógica de recorrência inicializada");

    } catch (error)
    {
        console.error("âŒ Erro ao inicializar lógica de recorrência:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-logic.js", "inicializarLogicaRecorrencia", error);
        }
    }
};

function esconderTodosCamposRecorrencia()
{
    try
    {
        const camposParaEsconder = [
            "divPeriodo",
            "divDias",
            "divDiaMes",
            "divFinalRecorrencia",
            "calendarContainer"
        ];

        camposParaEsconder.forEach(id =>
        {
            const elemento = document.getElementById(id);
            if (elemento)
            {

                elemento.style.setProperty('display', 'none', 'important');
            }
        });

        console.log("âœ… Todos os campos de recorrência escondidos (exceto lstRecorrente)");

    } catch (error)
    {
        console.error("âŒ Erro ao esconder campos:", error);
    }
}

function configurarEventHandlerRecorrente()
{
    try
    {
        const lstRecorrenteElement = document.getElementById("lstRecorrente");

        if (!lstRecorrenteElement || !lstRecorrenteElement.ej2_instances)
        {
            console.warn("âš ï¸ lstRecorrente não encontrado");
            return;
        }

        const lstRecorrente = lstRecorrenteElement.ej2_instances[0];

        if (!lstRecorrente)
        {
            console.warn("âš ï¸ Instância lstRecorrente não encontrada");
            return;
        }

        lstRecorrente.change = function (args)
        {
            aoMudarRecorrente(args);
        };

        console.log("âœ… Event handler lstRecorrente configurado");

    } catch (error)
    {
        console.error("âŒ Erro ao configurar event handler recorrente:", error);
    }
}

function aoMudarRecorrente(args)
{
    try
    {
        console.log("ðŸ”„ lstRecorrente mudou - DEBUG COMPLETO:");
        console.log(" - args completo:", args);
        console.log(" - args.value:", args.value);
        console.log(" - args.itemData:", args.itemData);
        console.log(" - args.itemData?.RecorrenteId:", args.itemData?.RecorrenteId);
        console.log(" - args.itemData?.Descricao:", args.itemData?.Descricao);

        if (window.ignorarEventosRecorrencia)
        {
            console.log("ðŸ“Œ Ignorando evento de recorrente (carregando dados)");
            return;
        }

        const valor = args.value || args.itemData?.RecorrenteId || args.itemData?.Value;
        const descricao = args.itemData?.Descricao || args.itemData?.Text || "";

        console.log(" - Valor extraÃ­do:", valor);
        console.log(" - Descrição extraÃ­da:", descricao);

        const divPeriodo = document.getElementById("divPeriodo");
        console.log(" - divPeriodo existe?", divPeriodo ? "SIM" : "NÃO");

        limparCamposRecorrenciaAoMudar();

        const ehSim = valor === "S" ||
            valor === "Sim" ||
            descricao === "Sim" ||
            descricao.toLowerCase() === "sim";

        console.log(" - Ã‰ SIM?", ehSim);

        if (ehSim)
        {
            console.log(" âœ… Selecionou SIM - Mostrar lstPeriodo");

            if (divPeriodo)
            {
                console.log(" â†’ Aplicando display:block no divPeriodo...");

                divPeriodo.style.setProperty('display', 'block', 'important');
                console.log(" â†’ Display aplicado. Valor atual:", window.getComputedStyle(divPeriodo).display);

                const lstPeriodosElement = document.getElementById("lstPeriodos");
                if (lstPeriodosElement && lstPeriodosElement.ej2_instances)
                {
                    const lstPeriodos = lstPeriodosElement.ej2_instances[0];
                    if (lstPeriodos)
                    {
                        lstPeriodos.value = null;
                        lstPeriodos.dataBind();
                    }
                }
            }
            else
            {
                console.error(" âŒ divPeriodo NÃO FOI ENCONTRADO!");
            }
        }
        else
        {
            console.log(" âŒ Selecionou NÃO - Esconder todos os campos");
            esconderTodosCamposRecorrencia();
        }

    } catch (error)
    {
        console.error("âŒ Erro em aoMudarRecorrente:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-logic.js", "aoMudarRecorrente", error);
        }
    }
}

function configurarEventHandlerPeriodo()
{
    try
    {
        console.log("ðŸ”§ Tentando configurar event handler de lstPeriodos...");

        let tentativas = 0;
        const maxTentativas = 10;

        const intervalo = setInterval(() =>
        {
            tentativas++;
            console.log(` â†’ Tentativa ${tentativas}/${maxTentativas}...`);

            const lstPeriodosElement = document.getElementById("lstPeriodos");

            if (!lstPeriodosElement)
            {
                console.warn(` âš ï¸ lstPeriodos não encontrado (tentativa ${tentativas})`);
                if (tentativas >= maxTentativas)
                {
                    clearInterval(intervalo);
                    console.error(" âŒ lstPeriodos não encontrado após todas tentativas");
                }
                return;
            }

            if (!lstPeriodosElement.ej2_instances || !lstPeriodosElement.ej2_instances[0])
            {
                console.warn(` âš ï¸ lstPeriodos não inicializado ainda (tentativa ${tentativas})`);
                if (tentativas >= maxTentativas)
                {
                    clearInterval(intervalo);
                    console.error(" âŒ lstPeriodos não inicializado após todas tentativas");
                }
                return;
            }

            clearInterval(intervalo);

            const lstPeriodos = lstPeriodosElement.ej2_instances[0];

            console.log(" âœ… lstPeriodos encontrado! Configurando evento...");
            console.log(" ðŸ“‹ DataSource atual:", lstPeriodos.dataSource);

            lstPeriodos.change = null;

            lstPeriodos.change = function (args)
            {
                console.log("ðŸŽ¯ EVENT HANDLER CHAMADO! lstPeriodos mudou!");
                aoMudarPeriodo(args);
            };

            console.log(" âœ… Event handler lstPeriodos configurado com sucesso!");

        }, 200);

    } catch (error)
    {
        console.error("âŒ Erro ao configurar event handler perí­odo:", error);
    }
}

function aoMudarPeriodo(args)
{
    try
    {
        console.log("ðŸ”„ lstPeriodos mudou - DEBUG COMPLETO:");
        console.log(" - args completo:", args);
        console.log(" - args.value:", args.value);
        console.log(" - args.itemData:", args.itemData);

        if (window.ignorarEventosRecorrencia)
        {
            console.log("ðŸ“Œ Ignorando evento de perí­odo (carregando dados)");
            return;
        }

        const valor = args.value || args.itemData?.Value || args.itemData?.PeriodoId;
        const texto = args.itemData?.Text || args.itemData?.Periodo || "";

        console.log(" ðŸ“‹ Valor extraÃ­do:", valor);
        console.log(" ðŸ“‹ Texto extraÃ­do:", texto);

        console.log(" ðŸ§¹ Escondendo campos especí­ficos...");
        esconderCamposEspecificosPeriodo();

        console.log(" ðŸ” Verificando qual perí­odo foi selecionado...");

        switch (valor)
        {
            case "D":
                console.log(" âž¡ï¸ Perí­odo: DIÃRIO - Mostrar apenas txtFinalRecorrencia");
                mostrarTxtFinalRecorrencia();
                break;

            case "S":
            case "Q":
                console.log(" âž¡ï¸ Perí­odo: SEMANAL/QUINZENAL - Mostrar lstDias + txtFinalRecorrencia");
                mostrarLstDias();
                mostrarTxtFinalRecorrencia();
                break;

            case "M":
                console.log(" âž¡ï¸ Perí­odo: MENSAL - Mostrar lstDiasMes + txtFinalRecorrencia");
                mostrarLstDiasMes();
                mostrarTxtFinalRecorrencia();
                break;

            case "V":
                console.log(" âž¡ï¸ Perí­odo: DIAS VARIADOS - Mostrar calendário com badge");
                mostrarCalendarioComBadge();
                break;

            default:
                console.log(" âš ï¸ Perí­odo não reconhecido:", valor, texto);
                console.log(" ðŸ’¡ Tentando pelo texto...");

                const textoLower = texto.toLowerCase();

                if (textoLower.includes("diário") || textoLower.includes("diario"))
                {
                    console.log(" âž¡ï¸ Detectado pelo texto: DIÃRIO");
                    mostrarTxtFinalRecorrencia();
                }
                else if (textoLower.includes("semanal"))
                {
                    console.log(" âž¡ï¸ Detectado pelo texto: SEMANAL");
                    mostrarLstDias();
                    mostrarTxtFinalRecorrencia();
                }
                else if (textoLower.includes("quinzenal"))
                {
                    console.log(" âž¡ï¸ Detectado pelo texto: QUINZENAL");
                    mostrarLstDias();
                    mostrarTxtFinalRecorrencia();
                }
                else if (textoLower.includes("mensal"))
                {
                    console.log(" âž¡ï¸ Detectado pelo texto: MENSAL");
                    mostrarLstDiasMes();
                    mostrarTxtFinalRecorrencia();
                }
                else if (textoLower.includes("variado") || textoLower.includes("variada"))
                {
                    console.log(" âž¡ï¸ Detectado pelo texto: DIAS VARIADOS");
                    mostrarCalendarioComBadge();
                }
                else
                {
                    console.error(" âŒ Perí­odo não pôde ser identificado!");
                }
                break;
        }

        console.log(" âœ… aoMudarPeriodo concluÃ­do");

    } catch (error)
    {
        console.error("âŒ Erro em aoMudarPeriodo:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-logic.js", "aoMudarPeriodo", error);
        }
    }
}

function esconderCamposEspecificosPeriodo()
{

    document.body.classList.remove('modo-criacao-variada');
    document.body.classList.remove('modo-edicao-variada');

    const campos = [
        "divDias",
        "divDiaMes",
        "divFinalRecorrencia",
        "calendarContainer"
    ];

    campos.forEach(id =>
    {
        const elemento = document.getElementById(id);
        if (elemento)
        {

            elemento.style.setProperty('display', 'none', 'important');
        }
    });
}

function mostrarTxtFinalRecorrencia()
{
    const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
    if (divFinalRecorrencia)
    {

        divFinalRecorrencia.style.setProperty('display', 'block', 'important');
        console.log(" âœ… txtFinalRecorrencia exibido");
    }
}

function mostrarLstDias()
{
    try
    {
        const divDias = document.getElementById("divDias");
        if (divDias)
        {

            divDias.style.setProperty('display', 'block', 'important');
            console.log(" ✅ lstDias container exibido");

            setTimeout(() =>
            {
                if (typeof window.inicializarLstDias === 'function')
                {
                    const sucesso = window.inicializarLstDias();
                    if (sucesso)
                    {
                        console.log(" ✅ lstDias populado com dias da semana");
                    }
                    else
                    {
                        console.warn(" ⚠️ lstDias não pôde ser populado (controle não renderizado)");
                    }
                }
                else
                {
                    console.error(" ❌ Função window.inicializarLstDias não encontrada!");
                }
            }, 100);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-logic.js", "mostrarLstDias", error);
    }
}


... (+500 linhas)
```
