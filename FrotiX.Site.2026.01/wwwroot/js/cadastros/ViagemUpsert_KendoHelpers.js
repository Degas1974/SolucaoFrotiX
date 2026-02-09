/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                     ║
 * ║ Arquivo: ViagemUpsert_KendoHelpers.js                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                 ║
 * ║ Funções auxiliares para manipulação de controles Telerik Kendo UI       ║
 * ║ (DatePicker e TimePicker) usados no formulário de Viagens.              ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

/**
 * Obtém o valor de um DatePicker Kendo como string no formato YYYY-MM-DD
 * @param {string} id - ID do elemento DatePicker
 * @returns {string} Data no formato YYYY-MM-DD ou string vazia
 */
function getKendoDateValue(id) {
    try {
        const picker = $(`#${id}`).data("kendoDatePicker");
        if (!picker) return "";
        
        const value = picker.value();
        if (!value) return "";
        
        // Retornar no formato YYYY-MM-DD
        const year = value.getFullYear();
        const month = String(value.getMonth() + 1).padStart(2, '0');
        const day = String(value.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    } catch (error) {
        Alerta.TratamentoErroComLinha("ViagemUpsert_KendoHelpers.js", "getKendoDateValue", error);
        return "";
    }
}

/**
 * Define o valor de um DatePicker Kendo
 * @param {string} id - ID do elemento DatePicker
 * @param {string|Date|null} value - Data a ser definida (string YYYY-MM-DD, Date ou null para limpar)
 */
function setKendoDateValue(id, value) {
    try {
        const picker = $(`#${id}`).data("kendoDatePicker");
        if (!picker) return;
        
        if (!value || value === "" || value === null) {
            picker.value(null);
            return;
        }
        
        // Se for string, converter para Date
        if (typeof value === "string") {
            picker.value(new Date(value));
        } else {
            picker.value(value);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("ViagemUpsert_KendoHelpers.js", "setKendoDateValue", error);
    }
}

/**
 * Obtém o valor de um TimePicker Kendo como string no formato HH:mm
 * @param {string} id - ID do elemento TimePicker
 * @returns {string} Hora no formato HH:mm ou string vazia
 */
function getKendoTimeValue(id) {
    try {
        const picker = $(`#${id}`).data("kendoTimePicker");
        if (!picker) return "";
        
        const value = picker.value();
        if (!value) return "";
        
        // Retornar no formato HH:mm
        const hours = String(value.getHours()).padStart(2, '0');
        const minutes = String(value.getMinutes()).padStart(2, '0');
        return `${hours}:${minutes}`;
    } catch (error) {
        Alerta.TratamentoErroComLinha("ViagemUpsert_KendoHelpers.js", "getKendoTimeValue", error);
        return "";
    }
}

/**
 * Define o valor deum TimePicker Kendo
 * @param {string} id - ID do elemento TimePicker
 * @param {string|Date|null} value - Hora a ser definida (string HH:mm, Date ou null para limpar)
 */
function setKendoTimeValue(id, value) {
    try {
        const picker = $(`#${id}`).data("kendoTimePicker");
        if (!picker) return;
        
        if (!value || value === "" || value === null) {
            picker.value(null);
            return;
        }
        
        // Se for string HH:mm, converter para Date
        if (typeof value === "string" && value.includes(":")) {
            const [hours, minutes] = value.split(":").map(Number);
            const date = new Date();
            date.setHours(hours, minutes, 0, 0);
            picker.value(date);
        } else {
            picker.value(value);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("ViagemUpsert_KendoHelpers.js", "setKendoTimeValue", error);
    }
}
