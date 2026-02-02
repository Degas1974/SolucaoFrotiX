/* ****************************************************************************************
 * ⚡ ARQUIVO: date.utils.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Utilitários para manipulação, formatação, parsing e arredondamento
 *                   de datas e horas. Fornece funções helper para conversão entre
 *                   formatos (YYYY-MM-DD, DD/MM/YYYY, ISO local), arredondamento de
 *                   horários, parsing flexível, e operações de adição de dias. Exporta
 *                   10 funções como window globals para uso em todo o sistema.
 * 📥 ENTRADAS     : Date objects, strings de data (YYYY-MM-DD, DD/MM/YYYY, ISO), strings
 *                   de hora (HH:mm), números (dias, intervalos de minutos, ms delay)
 * 📤 SAÍDAS       : Strings formatadas (YYYY-MM-DD, DD/MM/YYYY, HH:mm, ISO local), Date
 *                   objects (sem hora), Promises (delay), null em caso de erro
 * 🔗 CHAMADA POR  : Componentes de agendamento (calendario.js, dialogs.js, main.js),
 *                   validadores (validacao.js), serviços (agendamento.service.js)
 * 🔄 CHAMA        : moment.js (arredondarHora), Date API nativa (new Date, getFullYear,
 *                   getMonth, getDate, setHours, setDate, Date.parse),
 *                   Alerta.TratamentoErroComLinha, setTimeout (delay)
 * 📦 DEPENDÊNCIAS : moment.js (apenas arredondarHora), Date API nativa, Alerta.js
 * 📝 OBSERVAÇÕES  : Todas as funções exportadas como window.* (globals). Todas têm
 *                   try-catch completo com retorno de fallback (null, "", "00:00",
 *                   Promise.resolve()). parseDate aceita múltiplos formatos (DD/MM/YYYY,
 *                   YYYY-MM-DD, Date.parse). padStart usado para zero-padding. Typo em
 *                   linha 101: "padrío" (deveria ser "padrão").
 *
 * 📋 ÍNDICE DE FUNÇÕES (10 funções globais window.*):
 *
 * ┌─ ARREDONDAMENTO E FORMATAÇÃO DE HORA ───────────────────────────────────┐
 * │ 1. window.arredondarHora(hora, intervaloMinutos = 10)                   │
 * │    → param {Date|string} hora - Data/hora a arredondar                  │
 * │    → param {number} intervaloMinutos - Intervalo (ex: 10, 15, 30, 60)   │
 * │    → returns {string} Hora arredondada "HH:mm"                          │
 * │    → Usa moment(hora), calcula resto (minutos % intervalo)              │
 * │    → Se resto≠0: add(intervalo - resto, 'minutes')                      │
 * │    → Zera seconds(0) e milliseconds(0)                                  │
 * │    → Retorna m.format("HH:mm")                                          │
 * │    → try-catch: retorna "00:00"                                         │
 * │                                                                          │
 * │ Exemplo: arredondarHora('2026-01-15 14:23:00', 10) → "14:30"            │
 * │          arredondarHora('2026-01-15 14:20:00', 10) → "14:20"            │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CONVERSÃO DATE → STRING ───────────────────────────────────────────────┐
 * │ 2. window.toDateOnlyString(d)                                           │
 * │    → param {Date|string} d - Data                                       │
 * │    → returns {string} "YYYY-MM-DD"                                      │
 * │    → Converte para Date se string: d instanceof Date ? d : new Date(d)  │
 * │    → Extrai y, m (getMonth()+1 padStart 2), dd (getDate() padStart 2)  │
 * │    → Retorna template string: `${y}-${m}-${dd}`                         │
 * │    → try-catch: retorna null                                            │
 * │                                                                          │
 * │ 3. window.fmtDateLocal(d)                                               │
 * │    → param {Date} d - Data                                              │
 * │    → returns {string} "YYYY-MM-DD"                                      │
 * │    → Idêntico a toDateOnlyString mas sempre new Date(d)                 │
 * │    → try-catch: retorna ""                                              │
 * │                                                                          │
 * │ 4. window.formatDate(dateObj)                                           │
 * │    → param {Date} dateObj - Data                                        │
 * │    → returns {string} "DD/MM/YYYY"                                      │
 * │    → day = ("0" + getDate()).slice(-2)                                  │
 * │    → month = ("0" + (getMonth()+1)).slice(-2)                           │
 * │    → year = getFullYear()                                               │
 * │    → Retorna `${day}/${month}/${year}`                                  │
 * │    → try-catch: retorna ""                                              │
 * │                                                                          │
 * │ Exemplo: formatDate(new Date('2026-01-15')) → "15/01/2026"              │
 * │          toDateOnlyString(new Date('2026-01-15')) → "2026-01-15"        │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CONVERSÃO DATE → DATE (sem hora) ──────────────────────────────────────┐
 * │ 5. window.toLocalDateOnly(date)                                         │
 * │    → param {Date|string} date - Data                                    │
 * │    → returns {Date} Data sem hora (00:00:00)                            │
 * │    → const d = new Date(date)                                           │
 * │    → return new Date(getFullYear(), getMonth(), getDate())              │
 * │    → try-catch: retorna null                                            │
 * │                                                                          │
 * │ Exemplo: toLocalDateOnly('2026-01-15T14:30:00') → Date(2026,0,15,0,0,0) │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ COMBINAÇÃO DATE + TIME → STRING ISO LOCAL ─────────────────────────────┐
 * │ 6. window.toLocalDateTimeString(date, timeStr)                          │
 * │    → param {Date} date - Data                                           │
 * │    → param {string} timeStr - Hora "HH:mm"                              │
 * │    → returns {string} "YYYY-MM-DDTHH:mm:00" ou null                     │
 * │    → Se !date: retorna null                                             │
 * │    → Split timeStr por ":" → [hh, mm].map(Number)                       │
 * │    → new Date(date), setHours(hh, mm, 0, 0)                             │
 * │    → Monta string manual: `${y}-${m}-${dd}T${hh}:${mm}:00`              │
 * │    → try-catch: retorna null                                            │
 * │                                                                          │
 * │ 7. window.makeLocalDateTime(yyyyMMdd, hhmm)                             │
 * │    → param {string} yyyyMMdd - "YYYY-MM-DD"                             │
 * │    → param {string} hhmm - "HH:mm"                                      │
 * │    → returns {string} "YYYY-MM-DDTHH:mm:00"                             │
 * │    → Split hhmm por ":" → [hh, mm] com fallback "00:00"                 │
 * │    → Retorna template: `${yyyyMMdd}T${hh.padStart(2,'0')}:${mm}:00`    │
 * │    → try-catch: retorna ""                                              │
 * │                                                                          │
 * │ Exemplo: toLocalDateTimeString(Date('2026-01-15'), "14:30")             │
 * │          → "2026-01-15T14:30:00"                                        │
 * │          makeLocalDateTime("2026-01-15", "14:30") → "2026-01-15T14:30:00"│
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ PARSING FLEXÍVEL DE DATAS ─────────────────────────────────────────────┐
 * │ 8. window.parseDate(d)                                                   │
 * │    → param {*} d - Data em qualquer formato                             │
 * │    → returns {Date|null} Data parseada ou null                          │
 * │    → Se !d: retorna null                                                │
 * │    → Se d instanceof Date && !isNaN(d): retorna d                       │
 * │    → const s = String(d).trim()                                         │
 * │    → Regex DD/MM/YYYY: /^\d{1,2}\/\d{1,2}\/\d{4}$/                      │
 * │      Split "/" → [dia, mes, ano] → new Date(ano, mes-1, dia)            │
 * │    → Regex YYYY-MM-DD: /^\d{4}-\d{1,2}-\d{1,2}$/                        │
 * │      Split "-" → [ano, mes, dia] → new Date(ano, mes-1, dia)            │
 * │    → Fallback: Date.parse(s), se !isNaN → new Date(parsed)              │
 * │    → Senão: retorna null                                                │
 * │    → try-catch: retorna null                                            │
 * │                                                                          │
 * │ Exemplo: parseDate("15/01/2026") → Date(2026, 0, 15)                    │
 * │          parseDate("2026-01-15") → Date(2026, 0, 15)                    │
 * │          parseDate("2026-01-15T14:30:00Z") → Date(...)                  │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ OPERAÇÕES DE DATA ─────────────────────────────────────────────────────┐
 * │ 9. window.addDaysLocal(dateString, days)                                │
 * │    → param {string} dateString - Data em string                         │
 * │    → param {number} days - Número de dias a adicionar                   │
 * │    → returns {string|null} "YYYY-MM-DDTHH:mm:ss" ou null                │
 * │    → Se !dateString: retorna null                                       │
 * │    → const d = new Date(dateString), se isNaN(d): retorna null          │
 * │    → setDate(getDate() + days) - valida Number.isFinite(days)           │
 * │    → Helper pad = (n) => String(n).padStart(2, '0')                     │
 * │    → Retorna string manual com todos componentes (y-m-d + h:m:s)        │
 * │    → try-catch: retorna null                                            │
 * │                                                                          │
 * │ Exemplo: addDaysLocal("2026-01-15T14:30:00", 7) → "2026-01-22T14:30:00" │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ UTILITÁRIO ASYNC ──────────────────────────────────────────────────────┐
 * │ 10. window.delay(ms)                                                    │
 * │     → param {number} ms - Milissegundos                                 │
 * │     → returns {Promise} Promise que resolve após ms                     │
 * │     → return new Promise(resolve => setTimeout(resolve, ms))            │
 * │     → try-catch: retorna Promise.resolve()                              │
 * │                                                                          │
 * │ Exemplo: await delay(1000) // aguarda 1 segundo                         │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE ARREDONDAMENTO DE HORA:
 * 1. arredondarHora(Date('2026-01-15 14:23:00'), 10)
 * 2. moment(hora) → m
 * 3. m.minutes() → 23
 * 4. resto = 23 % 10 → 3
 * 5. Se resto≠0: m.add(10 - 3, 'minutes') → m agora 14:30
 * 6. m.seconds(0).milliseconds(0)
 * 7. m.format("HH:mm") → "14:30"
 *
 * 🔄 FLUXO DE PARSING FLEXÍVEL:
 * 1. parseDate("15/01/2026")
 * 2. String(d).trim() → "15/01/2026"
 * 3. Testa regex DD/MM/YYYY: /^\d{1,2}\/\d{1,2}\/\d{4}$/ → match
 * 4. Split "/" → ["15", "01", "2026"]
 * 5. new Date(Number("2026"), Number("01")-1, Number("15")) → Date(2026, 0, 15)
 * 6. Retorna Date object
 *
 * 🔄 FLUXO DE PARSING ALTERNATIVO:
 * 1. parseDate("2026-01-15")
 * 2. Testa regex DD/MM/YYYY → não match
 * 3. Testa regex YYYY-MM-DD: /^\d{4}-\d{1,2}-\d{1,2}$/ → match
 * 4. Split "-" → ["2026", "01", "15"]
 * 5. new Date(2026, 0, 15) → Date object
 *
 * 🔄 FLUXO DE PARSING FALLBACK:
 * 1. parseDate("2026-01-15T14:30:00Z")
 * 2. Nenhuma regex match
 * 3. Date.parse("2026-01-15T14:30:00Z") → timestamp number
 * 4. Se !isNaN(parsed): new Date(parsed) → Date object
 *
 * 🔄 FLUXO DE ADIÇÃO DE DIAS:
 * 1. addDaysLocal("2026-01-15T14:30:00", 7)
 * 2. new Date("2026-01-15T14:30:00") → Date object
 * 3. d.setDate(d.getDate() + 7) → setDate(15 + 7) → setDate(22)
 * 4. Extrai componentes: year, month+1, date, hours, minutes, seconds
 * 5. Aplica pad(2,'0') a cada componente numérico
 * 6. Monta string: "2026-01-22T14:30:00"
 *
 * 📌 DIFERENÇAS ENTRE FUNÇÕES SIMILARES:
 * - toDateOnlyString vs fmtDateLocal: quase idênticas (diferem apenas em fallback: null vs "")
 * - toLocalDateTimeString vs makeLocalDateTime:
 *   - toLocalDateTimeString: recebe Date + "HH:mm", faz setHours
 *   - makeLocalDateTime: recebe "YYYY-MM-DD" + "HH:mm", apenas concatena strings
 * - formatDate (DD/MM/YYYY) vs toDateOnlyString (YYYY-MM-DD): formatos diferentes
 *
 * 📌 FUNÇÕES QUE USAM moment.js:
 * - arredondarHora: única função que usa moment (moment, format, add, seconds, milliseconds)
 * - Todas as outras usam Date API nativa
 *
 * 📌 FORMATOS ACEITOS EM parseDate:
 * - Date object: retorna direto se válido
 * - DD/MM/YYYY: regex /^\d{1,2}\/\d{1,2}\/\d{4}$/ (ex: "15/1/2026", "1/1/2026")
 * - YYYY-MM-DD: regex /^\d{4}-\d{1,2}-\d{1,2}$/ (ex: "2026-1-15", "2026-01-15")
 * - Qualquer string aceita por Date.parse (ISO, RFC, etc.)
 * - Nota: permite mês/dia com 1 ou 2 dígitos (flexível)
 *
 * 📌 VALORES DE FALLBACK EM ERRO:
 * - arredondarHora: "00:00"
 * - toDateOnlyString: null
 * - toLocalDateOnly: null
 * - toLocalDateTimeString: null
 * - formatDate: ""
 * - fmtDateLocal: ""
 * - makeLocalDateTime: ""
 * - parseDate: null
 * - addDaysLocal: null
 * - delay: Promise.resolve()
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Typo em linha 101: "padrío" deveria ser "padrão" (comentário formatDate)
 * - padStart(2, '0') ou slice(-2) para zero-padding (ambas técnicas usadas)
 * - addDaysLocal retorna datetime completo (YYYY-MM-DDTHH:mm:ss), não apenas data
 * - makeLocalDateTime sempre força seconds=00 (hardcoded)
 * - arredondarHora sempre arredonda PARA CIMA (add, nunca subtract)
 * - parseDate aceita mes-1 (zero-indexed) ao criar Date
 * - toLocalDateOnly cria novo Date sem setHours (construtor com 3 args → 00:00:00)
 * - delay útil para await em testes ou animações
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Arredonda hora para o próximo intervalo especificado
 * param {Date|string} hora - Data/hora a ser arredondada
 * param {number} intervaloMinutos - Intervalo em minutos (ex: 10, 15, 30, 60)
 * returns {string} Hora arredondada no formato "HH:mm"
 */
window.arredondarHora = function (hora, intervaloMinutos = 10)
{
    try
    {
        const m = moment(hora);
        const minutos = m.minutes();
        const resto = minutos % intervaloMinutos;

        if (resto !== 0)
        {
            m.add(intervaloMinutos - resto, 'minutes');
        }

        m.seconds(0);
        m.milliseconds(0);

        return m.format("HH:mm");
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("date.utils.js", "arredondarHora", error);
        return "00:00";
    }
};

/**
 * Converte Date para string no formato YYYY-MM-DD
 * param {Date|string} d - Data
 * returns {string} Data no formato YYYY-MM-DD
 */
window.toDateOnlyString = function (d)
{
    try
    {
        const dt = d instanceof Date ? d : new Date(d);
        const y = dt.getFullYear();
        const m = String(dt.getMonth() + 1).padStart(2, "0");
        const dd = String(dt.getDate()).padStart(2, "0");
        return `${y}-${m}-${dd}`;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("date.utils.js", "toDateOnlyString", error);
        return null;
    }
};

/**
 * Converte para Date apenas com data (sem hora)
 * param {Date|string} date - Data
 * returns {Date} Data sem hora
 */
window.toLocalDateOnly = function (date)
{
    try
    {
        const d = new Date(date);
        return new Date(d.getFullYear(), d.getMonth(), d.getDate());
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("date.utils.js", "toLocalDateOnly", error);
        return null;
    }
};

/**
 * Converte data e hora para string ISO local
 * param {Date} date - Data
 * param {string} timeStr - Hora no formato "HH:mm"
 * returns {string} Data/hora no formato ISO local
 */
window.toLocalDateTimeString = function (date, timeStr)
{
    try
    {
        if (!date) return null;
        const [hh, mm] = (timeStr || "").split(":").map(Number);
        const d = new Date(date);
        if (!isNaN(hh) && !isNaN(mm))
        {
            d.setHours(hh, mm, 0, 0);
            return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}-${String(d.getDate()).padStart(2, "0")}T${String(hh).padStart(2, "0")}:${String(mm).padStart(2, "0")}:00`;
        }
        return null;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("date.utils.js", "toLocalDateTimeString", error);
        return null;
    }
};

/**
 * Formata data no padrío DD/MM/YYYY
 * param {Date} dateObj - Data
 * returns {string} Data formatada
 */
window.formatDate = function (dateObj)
{
    try
    {
        const day = ("0" + dateObj.getDate()).slice(-2);
        const month = ("0" + (dateObj.getMonth() + 1)).slice(-2);
        const year = dateObj.getFullYear();
        return `${day}/${month}/${year}`;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("date.utils.js", "formatDate", error);
        return "";
    }
};

/**
 * Formata data local
 * param {Date} d - Data
 * returns {string} Data formatada
 */
window.fmtDateLocal = function (d)
{
    try
    {
        const dt = new Date(d);
        const y = dt.getFullYear();
        const m = String(dt.getMonth() + 1).padStart(2, "0");
        const day = String(dt.getDate()).padStart(2, "0");
        return `${y}-${m}-${day}`;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("date.utils.js", "fmtDateLocal", error);
        return "";
    }
};

/**
 * Cria string de data/hora local
 * param {string} yyyyMMdd - Data no formato YYYY-MM-DD
 * param {string} hhmm - Hora no formato HH:mm
 * returns {string} DateTime local
 */
window.makeLocalDateTime = function (yyyyMMdd, hhmm)
{
    try
    {
        const [hh, mm] = String(hhmm || "00:00").split(":");
        return `${yyyyMMdd}T${String(hh).padStart(2, "0")}:${String(mm).padStart(2, "0")}:00`;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("date.utils.js", "makeLocalDateTime", error);
        return "";
    }
};

/**
 * Parseia data em vários formatos
 * param {*} d - Data em qualquer formato
 * returns {Date|null} Data parseada ou null
 */
window.parseDate = function (d)
{
    try
    {
        if (!d) return null;

        if (d instanceof Date && !isNaN(d))
        {
            return d;
        }

        const s = String(d).trim();

        // DD/MM/YYYY
        if (/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(s))
        {
            const [dia, mes, ano] = s.split("/");
            return new Date(Number(ano), Number(mes) - 1, Number(dia));
        }

        // YYYY-MM-DD
        if (/^\d{4}-\d{1,2}-\d{1,2}$/.test(s))
        {
            const [ano, mes, dia] = s.split("-");
            return new Date(Number(ano), Number(mes) - 1, Number(dia));
        }

        const parsed = Date.parse(s);
        if (!isNaN(parsed))
        {
            return new Date(parsed);
        }

        return null;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("date.utils.js", "parseDate", error);
        return null;
    }
};

/**
 * Adiciona dias a uma data
 * param {string} dateString - Data em string
 * param {number} days - Número de dias
 * returns {string|null} Nova data ou null
 */
window.addDaysLocal = function (dateString, days)
{
    try
    {
        if (!dateString) return null;
        const d = new Date(dateString);
        if (isNaN(d)) return null;
        d.setDate(d.getDate() + (Number.isFinite(days) ? days : 0));
        const pad = (n) => String(n).padStart(2, '0');
        return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()) + 'T' + pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds());
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("date.utils.js", "addDaysLocal", error);
        return null;
    }
};

/**
 * Delay assíncrono
 * param {number} ms - Milissegundos
 * returns {Promise}
 */
window.delay = function (ms)
{
    try
    {
        return new Promise(resolve => setTimeout(resolve, ms));
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("date.utils.js", "delay", error);
        return Promise.resolve();
    }
};
