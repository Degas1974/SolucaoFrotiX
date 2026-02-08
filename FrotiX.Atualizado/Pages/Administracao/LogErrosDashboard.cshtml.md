# Pages/Administracao/LogErrosDashboard.cshtml

**ARQUIVO NOVO** | 2096 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```html
@page
@model FrotiX.Pages.Administracao.LogErrosDashboardModel

@{
    ViewData["Title"] = "Dashboard de Logs";
    ViewData["PageName"] = "logerros_dashboard";
    ViewData["Heading"] = "<i class='fa-duotone fa-chart-mixed'></i> Administração: <span class='fw-300'>Dashboard de Logs</span>";
    ViewData["Category1"] = "Administração";
    ViewData["PageIcon"] = "fa-duotone fa-chart-mixed";
}

@section HeadBlock {
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Outfit:wght@400;600;700;900&display=swap" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script src="https://cdn.jsdelivr.net/npm/marked/marked.min.js"></script>

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/styles/github-dark.min.css">
    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/core.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/csharp.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/javascript.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/json.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/sql.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/xml.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/bash.min.js"></script>

    <script src="https://cdn.jsdelivr.net/npm/marked-highlight/lib/index.umd.js"></script>
    <style>
        /* ====== HEADER AZUL PADRÃO FROTIX ====== */
        .ftx-card-header {
            background-color: #3D5771;
            padding: 1rem 1.5rem;
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 1rem;
            border-radius: 8px 8px 0 0;
            position: relative;
            overflow: hidden;
        }

        .ftx-card-header::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.1), transparent);
            animation: shimmer 3s infinite;
        }

        @@keyframes shimmer {
            0% { left: -100%; }
            100% { left: 100%; }
        }

        .ftx-card-header .titulo-paginas {
            color: #fff !important;
            font-family: 'Outfit', sans-serif !important;
            font-weight: 900 !important;
            font-size: 1.5rem;
            margin: 0;
            display: flex;
            align-items: center;
            gap: 0.5rem;
            text-shadow: 0 2px 8px rgba(0, 0, 0, 0.3);
            position: relative;
            z-index: 1;
        }

        .ftx-card-header .titulo-paginas i.fa-duotone {
            font-size: 1.75rem;
            filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.3));
        }

        /* ====== CARDS DE ESTATÍSTICAS ====== */
        .stats-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(160px, 1fr));
            gap: 1rem;
            margin-bottom: 1.5rem;
        }

        .stat-card {
            background: #fff;
            border-radius: 12px;
            padding: 1.25rem;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            border-left: 4px solid #3D5771;
            transition: all 0.3s ease;
            position: relative;
            overflow: hidden;
        }

        .stat-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 8px 24px rgba(0,0,0,0.12);
        }

        .stat-card::after {
            content: '';
            position: absolute;
            top: 0;
            right: 0;
            width: 60px;
            height: 60px;
            background: linear-gradient(135deg, transparent 50%, rgba(0,0,0,0.03) 50%);
        }

        .stat-card.total { border-left-color: #3D5771; }
        .stat-card.erros { border-left-color: #dc3545; }
        .stat-card.warnings { border-left-color: #ffc107; }
        .stat-card.info { border-left-color: #17a2b8; }
        .stat-card.js-errors { border-left-color: #6f42c1; }
        .stat-card.http-errors { border-left-color: #fd7e14; }
        .stat-card.console { border-left-color: #9333ea; }
        .stat-card.servidor { border-left-color: #3b82f6; }
        .stat-card.cliente { border-left-color: #8b5cf6; }

        /* ====== CARDS CLICÁVEIS E SELECIONÁVEIS ====== */
        .stat-card.filterable {
            cursor: pointer;
            user-select: none;
        }

        .stat-card.filterable::before {
            content: '';
            position: absolute;
            top: 8px;
            right: 8px;
            width: 20px;
            height: 20px;
            border: 2px solid #d1d5db;
            border-radius: 4px;
            background: #fff;
            transition: all 0.2s ease;
            z-index: 2;
        }

        .stat-card.filterable.selected::before {
            background: #3D5771;
            border-color: #3D5771;
        }

        .stat-card.filterable.selected::after {
            content: '\f00c';
            font-family: 'Font Awesome 6 Pro', 'Font Awesome 5 Pro', 'Font Awesome 6 Free', 'FontAwesome';
            font-weight: 900;
            position: absolute;
            top: 10px;
            right: 12px;
            width: 16px;
            height: 16px;
            color: #fff;
            font-size: 12px;
            z-index: 3;
        }

        .stat-card.filterable.selected {
            border-left-width: 6px;
            box-shadow: 0 4px 16px rgba(61, 87, 113, 0.25);
            background: linear-gradient(to right, rgba(61, 87, 113, 0.05), #fff);
        }

        .stat-card.erros.selected { box-shadow: 0 4px 16px rgba(220, 53, 69, 0.25); background: linear-gradient(to right, rgba(220, 53, 69, 0.08), #fff); }
        .stat-card.warnings.selected { box-shadow: 0 4px 16px rgba(255, 193, 7, 0.25); background: linear-gradient(to right, rgba(255, 193, 7, 0.08), #fff); }
        .stat-card.js-errors.selected { box-shadow: 0 4px 16px rgba(111, 66, 193, 0.25); background: linear-gradient(to right, rgba(111, 66, 193, 0.08), #fff); }
        .stat-card.http-errors.selected { box-shadow: 0 4px 16px rgba(253, 126, 20, 0.25); background: linear-gradient(to right, rgba(253, 126, 20, 0.08), #fff); }
        .stat-card.console.selected { box-shadow: 0 4px 16px rgba(147, 51, 234, 0.25); background: linear-gradient(to right, rgba(147, 51, 234, 0.08), #fff); }
        .stat-card.servidor.selected { box-shadow: 0 4px 16px rgba(59, 130, 246, 0.25); background: linear-gradient(to right, rgba(59, 130, 246, 0.08), #fff); }
        .stat-card.cliente.selected { box-shadow: 0 4px 16px rgba(139, 92, 246, 0.25); background: linear-gradient(to right, rgba(139, 92, 246, 0.08), #fff); }

        /* Indicador de filtros ativos */
        .active-filters-bar {
            background: linear-gradient(135deg, #3D5771, #4a6a8a);
            border-radius: 8px;
            padding: 0.75rem 1rem;
            margin-bottom: 1rem;
            display: none;
            align-items: center;
            gap: 0.75rem;
            color: #fff;
            font-size: 0.85rem;
            animation: slideDown 0.3s ease;
        }

        .active-filters-bar.show {
            display: flex;
        }

        @@keyframes slideDown {
            from { opacity: 0; transform: translateY(-10px); }
            to { opacity: 1; transform: translateY(0); }
        }

        .active-filters-bar .filter-tag {
            background: rgba(255,255,255,0.2);
            padding: 0.25rem 0.75rem;
            border-radius: 20px;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }

        .active-filters-bar .filter-tag .remove-filter {
            cursor: pointer;
            opacity: 0.7;
            transition: opacity 0.2s;
        }

        .active-filters-bar .filter-tag .remove-filter:hover {
            opacity: 1;
        }

        .active-filters-bar .clear-all-filters {
            margin-left: auto;
            background: rgba(255,255,255,0.15);
            border: 1px solid rgba(255,255,255,0.3);
            color: #fff;
            padding: 0.25rem 0.75rem;
            border-radius: 4px;
            font-size: 0.75rem;
            cursor: pointer;
            transition: background 0.2s;
        }

        .active-filters-bar .clear-all-filters:hover {
            background: rgba(255,255,255,0.25);
        }

        .stat-icon {
            font-size: 2rem;
            opacity: 0.15;
            position: absolute;
            right: 1rem;
            top: 50%;
            transform: translateY(-50%);
        }

        .stat-value {
            font-size: 2.5rem;
            font-weight: 800;
            line-height: 1;
            color: #2d3748;
            font-family: 'Outfit', sans-serif;
        }

        .stat-card.erros .stat-value { color: #dc3545; }
        .stat-card.warnings .stat-value { color: #b8860b; }
        .stat-card.js-errors .stat-value { color: #6f42c1; }
        .stat-card.http-errors .stat-value { color: #fd7e14; }
        .stat-card.console .stat-value { color: #9333ea; }

        .stat-label {
            font-size: 0.75rem;
            color: #718096;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            margin-top: 0.5rem;
            font-weight: 600;
        }

        .stat-trend {
            font-size: 0.7rem;
            margin-top: 0.25rem;
            display: flex;
            align-items: center;
            gap: 0.25rem;
        }

        .stat-trend.up { color: #dc3545; }
        .stat-trend.down { color: #10b981; }
        .stat-trend.stable { color: #6b7280; }

        /* ====== CARDS DE GRÁFICOS ====== */
        .chart-card {
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            margin-bottom: 1.5rem;
            overflow: hidden;
        }

        .chart-header {
            background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%);
            padding: 1rem 1.5rem;
            border-bottom: 1px solid #e2e8f0;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .chart-title {
            font-size: 1rem;
            font-weight: 700;
            color: #2d3748;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }

        .chart-title i {
            color: #3D5771;
        }

        .chart-body {
            padding: 1.5rem;
        }

        .chart-container {
            position: relative;
            height: 300px;
        }

        /* ====== RANKINGS ====== */
        .ranking-list {
            list-style: none;
            padding: 0;
            margin: 0;
        }

        .ranking-item {
            display: flex;
            align-items: center;
            padding: 0.75rem 0;
            border-bottom: 1px solid #f1f5f9;
            transition: background 0.2s;
        }

        .ranking-item:hover {
            background: #f8fafc;
            margin: 0 -1rem;
            padding-left: 1rem;
            padding-right: 1rem;
        }

        .ranking-item:last-child {
            border-bottom: none;
        }

        .ranking-position {
            width: 28px;
            height: 28px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: 700;
            font-size: 0.75rem;
            margin-right: 0.75rem;
            flex-shrink: 0;
        }

        .ranking-position.top-1 { background: linear-gradient(135deg, #fbbf24, #f59e0b); color: #fff; }
        .ranking-position.top-2 { background: linear-gradient(135deg, #9ca3af, #6b7280); color: #fff; }
        .ranking-position.top-3 { background: linear-gradient(135deg, #cd7f32, #a0522d); color: #fff; }
        .ranking-position.other { background: #f1f5f9; color: #64748b; }

        .ranking-content {
            flex: 1;
            min-width: 0;
        }

        .ranking-label {
            font-size: 0.875rem;
            font-weight: 600;
            color: #2d3748;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .ranking-sublabel {
            font-size: 0.7rem;
            color: #94a3b8;
        }

        .ranking-count {
            font-size: 1rem;
            font-weight: 700;
            color: #3D5771;
            margin-left: 0.5rem;
        }

        .ranking-bar {
            height: 4px;
            background: #e2e8f0;
            border-radius: 2px;
            margin-top: 0.25rem;
            overflow: hidden;
        }

        .ranking-bar-fill {
            height: 100%;
            background: linear-gradient(90deg, #3D5771, #5a7fa3);
            border-radius: 2px;
            transition: width 0.5s ease;
        }

        /* ====== PEAK HOURS ====== */
        .peak-hours-grid {
            display: grid;
            grid-template-columns: repeat(24, 1fr);
            gap: 2px;
        }

        .peak-hour {
            aspect-ratio: 1;
            border-radius: 4px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 0.6rem;
            font-weight: 600;
            transition: transform 0.2s;
            cursor: pointer;
        }

        .peak-hour:hover {
            transform: scale(1.2);
            z-index: 10;
        }

        .peak-hour.level-0 { background: #f1f5f9; color: #94a3b8; }
        .peak-hour.level-1 { background: #dcfce7; color: #166534; }
        .peak-hour.level-2 { background: #fef3c7; color: #92400e; }
        .peak-hour.level-3 { background: #fed7aa; color: #9a3412; }
        .peak-hour.level-4 { background: #fecaca; color: #991b1b; }
        .peak-hour.level-5 { background: #dc3545; color: #fff; }

        /* ====== ANOMALIAS ====== */
        .anomaly-card {
            background: linear-gradient(135deg, #fef2f2 0%, #fee2e2 100%);
            border: 1px solid #fecaca;
            border-radius: 8px;
            padding: 1rem;
            margin-bottom: 0.75rem;
        }

        .anomaly-card.high {
            background: linear-gradient(135deg, #fef2f2 0%, #fecdd3 100%);
            border-color: #f87171;
        }

        .anomaly-header {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            margin-bottom: 0.5rem;
        }

        .anomaly-icon {
            width: 32px;
            height: 32px;
            border-radius: 50%;
            background: #dc3545;
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .anomaly-title {
            font-weight: 700;
            color: #991b1b;
        }

        .anomaly-details {
            font-size: 0.8rem;
            color: #7f1d1d;
        }

        /* ====== FILTROS ====== */
        .filter-bar {
            background: #fff;
            border-radius: 8px;
            padding: 1rem;
            margin-bottom: 1.5rem;
            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
            display: flex;
            align-items: center;
            gap: 1rem;
            flex-wrap: wrap;
        }

        .filter-label {
            font-size: 0.875rem;
            font-weight: 600;
            color: #475569;
        }

        .filter-buttons {
            display: flex;
            gap: 0.5rem;
        }


... (+1596 linhas)
```
