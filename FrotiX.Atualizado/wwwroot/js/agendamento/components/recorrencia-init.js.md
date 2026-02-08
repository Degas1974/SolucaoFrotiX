# wwwroot/js/agendamento/components/recorrencia-init.js

**ARQUIVO NOVO** | 201 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```javascript
window.inicializarControlesRecorrencia = function ()
{
    try
    {
        console.log("🔧 Inicializando controles de recorrência...");

        window.inicializarLstDiasMes();
        window.inicializarLstDias();
        window.inicializarTxtFinalRecorrencia();

        console.log("✅ Controles de recorrência inicializados");

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-init.js", "inicializarControlesRecorrencia", error);
    }
};

window.inicializarLstDiasMes = function ()
{
    try
    {
        const lstDiasMesElement = document.getElementById("lstDiasMes");

        if (!lstDiasMesElement)
        {
            console.warn("⚠️ lstDiasMes não encontrado no DOM");
            return false;
        }

        if (!lstDiasMesElement.ej2_instances || !lstDiasMesElement.ej2_instances[0])
        {
            console.warn("⚠️ lstDiasMes ainda não foi renderizado");
            return false;
        }

        const lstDiasMesObj = lstDiasMesElement.ej2_instances[0];

        if (lstDiasMesObj.dataSource && lstDiasMesObj.dataSource.length > 0)
        {
            console.log("ℹ️ lstDiasMes já está populado");
            return true;
        }

        const diasDoMes = [];
        for (let i = 1; i <= 31; i++)
        {
            diasDoMes.push({
                Value: i,
                Text: i.toString()
            });
        }

        lstDiasMesObj.dataSource = diasDoMes;
        lstDiasMesObj.dataBind();

        console.log("✅ lstDiasMes populado com 31 dias");
        return true;

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-init.js", "inicializarLstDiasMes", error);
        return false;
    }
};

window.inicializarLstDias = function ()
{
    try
    {
        const lstDiasElement = document.getElementById("lstDias");

        if (!lstDiasElement)
        {
            console.warn("⚠️ lstDias não encontrado no DOM");
            return false;
        }

        if (!lstDiasElement.ej2_instances || !lstDiasElement.ej2_instances[0])
        {
            console.warn("⚠️ lstDias ainda não foi renderizado");
            return false;
        }

        const lstDiasObj = lstDiasElement.ej2_instances[0];

        if (lstDiasObj.dataSource && lstDiasObj.dataSource.length > 0)
        {
            console.log("ℹ️ lstDias já está populado");
            return true;
        }

        const diasDaSemana = [
            { Value: 0, Text: "Domingo" },
            { Value: 1, Text: "Segunda" },
            { Value: 2, Text: "Terça" },
            { Value: 3, Text: "Quarta" },
            { Value: 4, Text: "Quinta" },
            { Value: 5, Text: "Sexta" },
            { Value: 6, Text: "Sábado" }
        ];

        lstDiasObj.dataSource = diasDaSemana;
        lstDiasObj.dataBind();

        console.log("✅ lstDias populado com dias da semana");
        return true;

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-init.js", "inicializarLstDias", error);
        return false;
    }
};

window.inicializarTxtFinalRecorrencia = function ()
{
    try
    {
        const txtFinalRecorrenciaObj = window.getKendoDatePicker("txtFinalRecorrencia");

        if (!txtFinalRecorrenciaObj)
        {
            console.warn("⚠️ txtFinalRecorrencia não encontrado ou não inicializado (Kendo)");
            return false;
        }

        const hoje = new Date();
        if (typeof txtFinalRecorrenciaObj.min === "function")
        {
            txtFinalRecorrenciaObj.min(hoje);
        }

        console.log("✅ txtFinalRecorrencia configurado");
        return true;

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-init.js", "inicializarTxtFinalRecorrencia", error);
        return false;
    }
};

window.inicializarDropdownPeriodos = function ()
{
    try
    {
        console.log("🔧 Inicializando dropdown de períodos...");

        if (typeof ej === 'undefined' || !ej.dropdowns || !ej.dropdowns.DropDownList)
        {
            console.warn("⚠️ Syncfusion (ej.dropdowns.DropDownList) ainda não carregado. Aguardando...");

            setTimeout(window.inicializarDropdownPeriodos, 200);
            return;
        }

        const lstPeriodosElement = document.getElementById("lstPeriodos");

        if (!lstPeriodosElement)
        {
            console.error("❌ Elemento lstPeriodos não encontrado!");
            return;
        }

        if (lstPeriodosElement.ej2_instances && lstPeriodosElement.ej2_instances[0])
        {
            console.log("🗑️ Destruindo instância anterior...");
            lstPeriodosElement.ej2_instances[0].destroy();
        }

        const periodos = [
            { PeriodoId: "D", Periodo: "Diário" },
            { PeriodoId: "S", Periodo: "Semanal" },
            { PeriodoId: "Q", Periodo: "Quinzenal" },
            { PeriodoId: "M", Periodo: "Mensal" },
            { PeriodoId: "V", Periodo: "Dias Variados" }
        ];

        const dropdownPeriodos = new ej.dropdowns.DropDownList({
            dataSource: periodos,
            fields: {
                text: 'Periodo',
                value: 'PeriodoId'
            },
            placeholder: 'Selecione o período...',
            popupHeight: '200px',

            floatLabelType: 'Never',
            cssClass: 'e-outline',
            width: '100%'
        });

        dropdownPeriodos.appendTo(lstPeriodosElement);

        console.log("✅ Dropdown de períodos inicializado com sucesso!");
        console.log(" 📊 Total de períodos:", periodos.length);

    } catch (error)
    {
        console.error("❌ Erro ao inicializar dropdown de períodos:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-init.js", "inicializarDropdownPeriodos", error);
        }
    }
};

window.rebuildLstPeriodos = function ()
{
    try
    {
        console.log("🔄 Reconstruindo dropdown de períodos...");
        window.inicializarDropdownPeriodos();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-init.js", "rebuildLstPeriodos", error);
    }
};

if (typeof ej !== 'undefined' && ej.dropdowns && ej.dropdowns.DropDownList)
{
    console.log("✅ Syncfusion DropDownList disponível");

    setTimeout(() =>
    {
        try
        {
            if (document.getElementById("lstPeriodos"))
            {
                window.inicializarDropdownPeriodos();
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia-init.js", "auto-init", error);
        }
    }, 500);
}
else
{
    console.warn("⚠️ Syncfusion ainda não carregado, aguardando...");
}
```
