# wwwroot/js/agendamento/utils/kendo-datetime.js

**ARQUIVO NOVO** | 192 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```javascript
(function ()
{
    function getElementById(id)
    {
        return document.getElementById(id);
    }

    function getKendoDatePicker(id)
    {
        try
        {
            const el = getElementById(id);
            if (!el || !window.jQuery) return null;
            return window.jQuery(el).data("kendoDatePicker") || null;
        } catch (error)
        {
            return null;
        }
    }

    function getKendoTimePicker(id)
    {
        try
        {
            const el = getElementById(id);
            if (!el || !window.jQuery) return null;
            return window.jQuery(el).data("kendoTimePicker") || null;
        } catch (error)
        {
            return null;
        }
    }

    function parseDateValue(value)
    {
        if (!value) return null;

        if (value instanceof Date && !isNaN(value))
        {
            return value;
        }

        if (typeof window.parseDate === "function")
        {
            return window.parseDate(value);
        }

        const parsed = new Date(value);
        return isNaN(parsed) ? null : parsed;
    }

    function formatDateValue(date)
    {
        if (!date) return "";
        if (typeof window.formatDate === "function")
        {
            return window.formatDate(date);
        }
        const pad = (n) => String(n).padStart(2, "0");
        return `${pad(date.getDate())}/${pad(date.getMonth() + 1)}/${date.getFullYear()}`;
    }

    function formatTimeValue(date)
    {
        if (!date) return "";
        const pad = (n) => String(n).padStart(2, "0");
        return `${pad(date.getHours())}:${pad(date.getMinutes())}`;
    }

    function parseTimeValue(value)
    {
        if (!value) return null;
        if (value instanceof Date && !isNaN(value)) return value;

        if (typeof value === "string" && value.includes(":"))
        {
            const parts = value.split(":");
            const h = parseInt(parts[0], 10);
            const m = parseInt(parts[1], 10);
            if (isNaN(h) || isNaN(m)) return null;
            const d = new Date();
            d.setHours(h, m, 0, 0);
            return d;
        }

        return null;
    }

    window.getKendoDatePicker = function (id)
    {
        return getKendoDatePicker(id);
    };

    window.getKendoTimePicker = function (id)
    {
        return getKendoTimePicker(id);
    };

    window.getKendoDateValue = function (id)
    {
        const picker = getKendoDatePicker(id);
        if (picker) return picker.value();

        const el = getElementById(id);
        if (el && el.value)
        {
            return parseDateValue(el.value);
        }

        return null;
    };

    window.setKendoDateValue = function (id, value, triggerChange = false)
    {
        const date = parseDateValue(value);
        const picker = getKendoDatePicker(id);

        if (picker)
        {
            picker.value(date);
            if (triggerChange && typeof picker.trigger === "function")
            {
                picker.trigger("change");
            }
            return;
        }

        const el = getElementById(id);
        if (el)
        {
            el.value = date ? formatDateValue(date) : "";
        }
    };

    window.enableKendoDatePicker = function (id, enabled)
    {
        const picker = getKendoDatePicker(id);
        if (picker && typeof picker.enable === "function")
        {
            picker.enable(enabled !== false);
            return;
        }

        const el = getElementById(id);
        if (el) el.disabled = enabled === false;
    };

    window.showKendoDatePicker = function (id, show)
    {
        const picker = getKendoDatePicker(id);
        if (picker && picker.wrapper)
        {
            if (show === false) picker.wrapper.hide();
            else picker.wrapper.show();
            return;
        }

        const el = getElementById(id);
        if (el) el.style.display = show === false ? "none" : "";
    };

    window.getKendoTimeValue = function (id)
    {
        const picker = getKendoTimePicker(id);
        if (picker)
        {
            return formatTimeValue(picker.value());
        }

        const el = getElementById(id);
        return el ? (el.value || "") : "";
    };

    window.setKendoTimeValue = function (id, value)
    {
        const time = parseTimeValue(value);
        const picker = getKendoTimePicker(id);

        if (picker)
        {
            picker.value(time);
            return;
        }

        const el = getElementById(id);
        if (el)
        {
            if (typeof value === "string")
            {
                el.value = value;
            } else
            {
                el.value = time ? formatTimeValue(time) : "";
            }
        }
    };

    window.enableKendoTimePicker = function (id, enabled)
    {
        const picker = getKendoTimePicker(id);
        if (picker && typeof picker.enable === "function")
        {
            picker.enable(enabled !== false);
            return;
        }

        const el = getElementById(id);
        if (el) el.disabled = enabled === false;
    };

    window.showKendoTimePicker = function (id, show)
    {
        const picker = getKendoTimePicker(id);
        if (picker && picker.wrapper)
        {
            if (show === false) picker.wrapper.hide();
            else picker.wrapper.show();
            return;
        }

        const el = getElementById(id);
        if (el) el.style.display = show === false ? "none" : "";
    };
})();
```
