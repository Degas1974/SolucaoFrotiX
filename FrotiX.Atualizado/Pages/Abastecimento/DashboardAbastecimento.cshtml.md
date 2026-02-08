# Pages/Abastecimento/DashboardAbastecimento.cshtml

**Mudanca:** GRANDE | **+1282** linhas | **-1499** linhas

---

```diff
--- JANEIRO: Pages/Abastecimento/DashboardAbastecimento.cshtml
+++ ATUAL: Pages/Abastecimento/DashboardAbastecimento.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Abastecimento.DashboardAbastecimentoModel
 @{
     ViewData["Title"] = "Dashboard de Abastecimentos";
@@ -10,815 +9,750 @@
 }
 
 @section HeadBlock {
-    <style>
-        /* ========================================
-               PALETA DE CORES - TEMA CARAMELO SUAVE
-               ======================================== */
-        :root {
-            --abast-primary: #a8784c;
-            --abast-secondary: #c4956a;
-            --abast-accent: #d4a574;
-            --abast-dark: #8b5e3c;
-            --abast-darker: #6d472c;
-            --abast-light: #faf6f1;
-            --abast-cream: #f5ebe0;
-            --abast-gradient: linear-gradient(135deg, #a8784c 0%, #c4956a 100%);
-        }
-
-        /* Header do Dashboard */
-        .header-dashboard-abast {
-            background: var(--abast-gradient);
-            color: white;
-            padding: 20px 24px;
-            border-radius: 10px;
-            margin-bottom: 24px;
-            box-shadow: 0 4px 15px rgba(168, 120, 76, 0.35);
-        }
-
-        .header-dashboard-titulo-abast {
-            margin: 0;
-            font-family: 'Outfit', sans-serif;
-            font-weight: 800;
-            font-size: 1.75rem;
-            display: flex;
-            align-items: center;
-            gap: 12px;
-            text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
-            position: relative;
-            z-index: 1;
-        }
-
-        .header-dashboard-titulo-abast i {
-            font-size: 1.75rem;
-            filter: drop-shadow(0 2px 3px rgba(0, 0, 0, 0.25));
-        }
-
-        /* Navegação de Abas - Atualizado 15/01/2026 */
-        .dash-tabs {
-            background: #f5ebe0 !important;
-            /* var(--abast-cream) forçado */
-            border-radius: 12px;
-            padding: 6px;
-            margin-bottom: 24px;
-            box-shadow: 0 2px 8px rgba(168, 120, 76, 0.15);
-            display: flex;
-            gap: 6px;
-        }
-
-        .dash-tab {
-            flex: 1;
-            padding: 12px 20px;
-            border: none;
-            background: transparent;
-            border-radius: 8px;
-            font-weight: 600;
-            font-size: 0.9rem;
-            color: #8b5e3c !important;
-            /* var(--abast-dark) forçado */
-            cursor: pointer;
-            transition: all 0.25s ease;
-            display: flex;
-            align-items: center;
-            justify-content: center;
-            gap: 8px;
-        }
-
-        .dash-tab:hover {
-            background: rgba(168, 120, 76, 0.15);
-            color: var(--abast-darker);
-        }
-
-        .dash-tab.active {
-            background: var(--abast-gradient);
-            color: white;
-            box-shadow: 0 4px 12px rgba(168, 120, 76, 0.35);
-        }
-
-        .dash-tab i {
-            font-size: 1.1rem;
-        }
-
-        .dash-content {
-            display: none !important;
-            visibility: hidden;
-            position: absolute;
-            left: -9999px;
-        }
-
-        .dash-content.active {
-            display: block !important;
-            visibility: visible;
-            position: relative;
-            left: 0;
-            animation: fadeIn 0.3s ease;
-        }
-
-        @@keyframes fadeIn {
-            from {
-                opacity: 0;
-                transform: translateY(10px);
-            }
-
-            to {
-                opacity: 1;
-                transform: translateY(0);
-            }
-        }
-
-        /* Container de Filtros */
-        .filter-card-abast {
-            background: var(--abast-gradient);
-            color: white;
-            border-radius: 12px;
-            padding: 20px 24px;
-            margin-bottom: 24px;
-            box-shadow: 0 4px 12px rgba(168, 120, 76, 0.25);
-        }
-
-        .filter-card-abast .form-label {
-            color: white;
-            font-weight: 600;
-            font-size: 0.85rem;
-            margin-bottom: 6px;
-        }
-
-        .filter-card-abast .form-control,
-        .filter-card-abast .form-select {
-            background: rgba(255, 255, 255, 0.18);
-            border: 1px solid rgba(255, 255, 255, 0.35);
-            color: white;
-            border-radius: 8px;
-            font-size: 0.9rem;
-            padding: 8px 12px;
-        }
-
-        .filter-card-abast .form-control::placeholder {
-            color: rgba(255, 255, 255, 0.7);
-        }
-
-        .filter-card-abast .form-control:focus,
-        .filter-card-abast .form-select:focus {
-            background: rgba(255, 255, 255, 0.28);
-            border-color: rgba(255, 255, 255, 0.6);
-            box-shadow: 0 0 0 3px rgba(255, 255, 255, 0.2);
-            color: white;
-        }
-
-        .filter-card-abast .form-select option {
-            background: var(--abast-dark);
-            color: white;
-        }
-
-        /* Botões de Filtro - Base comum */
-        .btn-filtrar-abast,
-        .btn-limpar-abast {
-            padding: 8px 20px !important;
-            border-radius: 8px !important;
-            font-weight: 600 !important;
-            font-size: 0.875rem !important;
-            line-height: 1.5 !important;
-            transition: all 0.3s ease !important;
-            box-sizing: border-box !important;
-        }
-
-        /* Botão Filtrar */
-        .btn-filtrar-abast {
-            background: rgba(255, 255, 255, 0.2) !important;
-            border: 2px solid rgba(255, 255, 255, 0.5) !important;
-            color: white !important;
-        }
-
-        .btn-filtrar-abast:hover {
-            background: rgba(255, 255, 255, 0.35) !important;
-            border-color: white !important;
-            transform: translateY(-1px) !important;
-        }
-
-        /* Botão Limpar Filtro */
-        .btn-limpar-abast {
-            background: linear-gradient(135deg, #B28767 0%, #9a7254 100%) !important;
-            border: 2px solid rgba(255, 255, 255, 0.6) !important;
-            color: white !important;
-            box-shadow: 0 2px 8px rgba(178, 135, 103, 0.4);
-        }
-
-        .btn-limpar-abast:hover {
-            background: linear-gradient(135deg, #9a7254 0%, #825d43 100%) !important;
-            border-color: rgba(255, 255, 255, 0.9) !important;
-            color: white !important;
-            transform: translateY(-1px);
-            box-shadow: 0 4px 12px rgba(75, 85, 99, 0.5);
-        }
-
-        /* Botões de Período Rápido */
-        .btn-period-abast,
-        .btn-period-abast-veiculo {
-            border-radius: 20px;
-            padding: 8px 16px;
-            font-weight: 600;
-            font-size: 0.85rem;
-            transition: all 0.2s;
-            border: 2px solid rgba(255, 255, 255, 0.3);
-            background: rgba(255, 255, 255, 0.15);
-            color: white;
-        }
-
-        .btn-period-abast:hover,
-        .btn-period-abast-veiculo:hover {
-            transform: scale(1.05);
-            background: rgba(255, 255, 255, 0.25);
-            border-color: rgba(255, 255, 255, 0.5);
-            color: white;
-        }
-
-        .btn-period-abast.active,
-        .btn-period-abast-veiculo.active {
-            background: white;
-            color: var(--abast-primary);
-            border-color: white;
-        }
-
-        /* Indicador de Período */
-        .periodo-atual-abast {
-            background: rgba(255, 255, 255, 0.2);
-            border: 1px solid rgba(255, 255, 255, 0.3);
-            border-radius: 8px;
-            padding: 10px 16px;
-            color: white;
-            font-size: 0.9rem;
-        }
-
-        /* Cards de Estatísticas */
-        .card-estatistica-abast {
-            background: white;
-            border-radius: 10px;
-            padding: 16px 20px;
-            border-left: 4px solid;
-            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
-            transition: all 0.3s ease;
-            height: 100%;
-            min-height: 90px;
-        }
-
-        .card-estatistica-abast:hover {
-            box-shadow: 0 6px 16px rgba(0, 0, 0, 0.1);
-            transform: translateY(-3px);
-        }
-
-        .card-estatistica-abast.amber {
-            border-left-color: var(--abast-primary);
-        }
-
-        .card-estatistica-abast.orange {
-            border-left-color: #ea580c;
-        }
-
-        .card-estatistica-abast.yellow {
-            border-left-color: var(--abast-accent);
-        }
-
-        .card-estatistica-abast.brown {
-            border-left-color: var(--abast-darker);
-        }
-
-        .card-estatistica-abast.gold {
-            border-left-color: #ca8a04;
-        }
-
-        .icone-card-abast {
-            font-size: 1.4rem;
-            margin-bottom: 8px;
-        }
-
-        .card-estatistica-abast.amber .icone-card-abast {
-            color: var(--abast-primary);
-        }
-
-        .card-estatistica-abast.orange .icone-card-abast {
-            color: #ea580c;
-        }
-
-        .card-estatistica-abast.yellow .icone-card-abast {
-            color: var(--abast-accent);
-        }
-
-        .card-estatistica-abast.brown .icone-card-abast {
-            color: var(--abast-darker);
-        }
-
-        .card-estatistica-abast.gold .icone-card-abast {
-            color: #ca8a04;
-        }
-
-        .texto-metrica-abast {
-            font-size: 0.7rem;
-            color: #6B7280;
-            font-weight: 600;
-            text-transform: uppercase;
-            letter-spacing: 0.5px;
-            margin-bottom: 6px;
-        }
-
-        .valor-metrica-abast {
-            font-size: 1.5rem;
-            font-weight: 800;
-            color: #111827;
-            line-height: 1.1;
-        }
-
-        /* Cards de Gráficos */
-        .chart-container-abast {
-            background: white;
-            border-radius: 12px;
-            padding: 20px;
-            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
-            transition: box-shadow 0.3s ease;
-            height: 100%;
-            border: none;
-        }
-
-        .chart-container-abast:hover {
-            box-shadow: 0 6px 16px rgba(0, 0, 0, 0.1);
-        }
-
-        .chart-title-abast {
-            font-size: 0.95rem;
-            font-weight: 700;
-            margin-bottom: 16px;
-            color: #111827;
-            display: flex;
-            align-items: center;
-            gap: 10px;
-            padding-bottom: 12px;
-            border-bottom: 2px solid var(--abast-cream);
-        }
-
-        .chart-title-abast i {
-            color: var(--abast-primary);
-            font-size: 1.1rem;
-        }
-
-        .chart-subtitle {
-            font-size: 0.7rem;
-            color: #9ca3af;
-            font-weight: 400;
-            margin-left: auto;
-        }
-
-        /* Card Mensal Grande */
-        .stat-card-mensal-abast {
-            background: white;
-            border-radius: 12px;
-            padding: 20px;
-            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
-            height: 100%;
-            display: flex;
-            flex-direction: column;
-        }
-
-        .stat-card-mensal-abast .card-header-custom {
-            display: flex;
-            align-items: center;
-            gap: 10px;
-            margin-bottom: 12px;
-            padding-bottom: 10px;
-            border-bottom: 2px solid var(--abast-cream);
-        }
-
-        .stat-card-mensal-abast .card-header-custom i {
-            color: var(--abast-primary);
-            font-size: 1.1rem;
-        }
-
-        .stat-card-mensal-abast .card-header-custom span {
-            font-size: 0.9rem;
-            font-weight: 700;
-            color: #111827;
-        }
-
-        .stat-card-mensal-abast .big-value {
-            font-size: 2rem;
-            font-weight: 800;
-            color: var(--abast-dark);
-            text-align: center;
-            margin-top: auto;
-            margin-bottom: auto;
-        }
-
-        /* =====================================================
-               GRID-TABELAS - Usando DIVs para evitar CSS global de table
-               ===================================================== */
-        .ftx-grid-tabela {
-            width: 100%;
-            font-size: 0.8rem;
-        }
-
-        .ftx-grid-tabela .grid-header {
-            display: grid;
-            background: linear-gradient(135deg, #a8784c 0%, #c4956a 100%);
-            border-radius: 6px 6px 0 0;
-        }
-
-        .ftx-grid-tabela .grid-header-cell {
-            color: #ffffff;
-            font-weight: 600;
-            font-size: 0.75rem;
-            text-transform: uppercase;
-            letter-spacing: 0.3px;
-            padding: 10px 12px;
-        }
-
-        .ftx-grid-tabela .grid-body {
-            max-height: 280px;
-            overflow-y: auto;
-        }
-
-        .ftx-grid-tabela .grid-row {
-            display: grid;
-            border-bottom: 1px solid #f3f4f6;
-            transition: background-color 0.15s;
-        }
-
-        .ftx-grid-tabela .grid-row:hover {
-            background-color: var(--abast-light);
-        }
-
-        .ftx-grid-tabela .grid-cell {
-            padding: 10px 12px;
-        }
-
-        .ftx-grid-tabela .text-end {
-            text-align: right;
-        }
-
-        /* Layout 2 colunas (Resumo por Ano, Top por Tipo) */
-        .ftx-grid-tabela.cols-2 .grid-header,
-        .ftx-grid-tabela.cols-2 .grid-row {
-            grid-template-columns: 1fr auto;
-        }
-
-        /* Layout 3 colunas (Top por Placa) */
-        .ftx-grid-tabela.cols-3 .grid-header,
-        .ftx-grid-tabela.cols-3 .grid-row {
-            grid-template-columns: auto 1fr auto;
-        }
-
-        .ftx-grid-tabela .grid-row-total {
-            background: var(--abast-cream);
-            font-weight: 700;
-        }
-
-        /* Linha clicável - abre detalhes */
-        .ftx-grid-tabela .grid-row-clicavel {
-            cursor: pointer;
-        }
-
-        .ftx-grid-tabela .grid-row-clicavel:hover {
-            background-color: #e8f4e8;
-            box-shadow: inset 3px 0 0 var(--abast-primary);
-        }
-
-        .ftx-grid-tabela .grid-row-clicavel:active {
-            background-color: #d4ecd4;
-        }
-
-        /* Badge de Ranking */
-        .badge-rank-abast {
-            display: inline-flex;
-            align-items: center;
-            justify-content: center;
-            width: 24px;
-            height: 24px;
-            border-radius: 50%;
-            background: var(--abast-secondary);
-            color: white;
-            font-size: 0.7rem;
-            font-weight: 700;
-            margin-right: 8px;
-        }
-
-        .badge-rank-abast.top3 {
-            background: var(--abast-primary);
-            box-shadow: 0 2px 6px rgba(168, 120, 76, 0.4);
-        }
-
-        /* Tabela Média Litro */
-        .table-media-litro-abast {
-            font-size: 0.8rem;
-            margin: 0;
-        }
-
-        .table-media-litro-abast tbody tr {
-            border-bottom: 1px solid #f3f4f6;
-        }
-
-        .table-media-litro-abast tbody td {
-            padding: 8px 10px;
-        }
-
-        /* Loading - Usa padrão FrotiX global (ftx-spin-overlay) */
-
-        /* ================================================================
-               MODAL DE DETALHES - Tema Caramelo/Creme
-               ================================================================ */
-        #modalDetalhesAbast .modal-dialog {
-            max-width: 900px;
-        }
-
-        #modalDetalhesAbast .modal-content {
-            border: none;
-            border-radius: 12px;
-            overflow: hidden;
-            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.25);
-        }
-
-        #modalDetalhesAbast .modal-header {
-            background: linear-gradient(135deg, #a8784c 0%, #c4956a 100%);
-            border-bottom: none;
-            padding: 1rem 1.5rem;
-        }
-
-        #modalDetalhesAbast .modal-title {
-            font-family: 'Outfit', sans-serif;
-            font-weight: 700;
-            font-size: 1.1rem;
-            color: #fff;
-            display: flex;
-            align-items: center;
-            gap: 0.5rem;
-        }
-
-        #modalDetalhesAbast .modal-title i {
-            font-size: 1.2rem;
-        }
-
-        #modalDetalhesAbast .btn-close {
-            filter: brightness(0) invert(1);
-            opacity: 0.8;
-        }
-
-        #modalDetalhesAbast .btn-close:hover {
-            opacity: 1;
-        }
-
-        #modalDetalhesAbast .modal-body {
-            background: var(--abast-cream);
-            padding: 1.5rem;
-            max-height: 500px;
-            overflow-y: auto;
-        }
-
-        #modalDetalhesAbast .detalhes-resumo {
-            background: white;
-            border-radius: 8px;
-            padding: 1rem;
-            margin-bottom: 1rem;
-            display: flex;
-            gap: 2rem;
-            flex-wrap: wrap;
-        }
-
-        #modalDetalhesAbast .detalhes-resumo-item {
-            text-align: center;
-        }
-
-        #modalDetalhesAbast .detalhes-resumo-label {
-            font-size: 0.7rem;
-            color: #6c757d;
-            text-transform: uppercase;
-            font-weight: 600;
-        }
-
-        #modalDetalhesAbast .detalhes-resumo-valor {
-            font-size: 1.2rem;
-            font-weight: 700;
-            color: var(--abast-dark);
-        }
-
-        #modalDetalhesAbast .detalhes-grid {
-            display: grid;
-            grid-template-columns: repeat(5, 1fr);
-            gap: 0;
-            background: white;
-            border-radius: 8px;
-            overflow: hidden;
-        }
-
-        #modalDetalhesAbast .detalhes-grid-header {
-            background: linear-gradient(135deg, #a8784c 0%, #c4956a 100%);
-            color: white;
-            font-weight: 600;
-            font-size: 0.75rem;
-            text-transform: uppercase;
-            padding: 10px 12px;
-        }
-
-        #modalDetalhesAbast .detalhes-grid-cell {
-            padding: 8px 12px;
-            font-size: 0.8rem;
-            border-bottom: 1px solid #f3f4f6;
-        }
-
-        #modalDetalhesAbast .detalhes-grid-cell:nth-child(10n+6),
-        #modalDetalhesAbast .detalhes-grid-cell:nth-child(10n+7),
-        #modalDetalhesAbast .detalhes-grid-cell:nth-child(10n+8),
-        #modalDetalhesAbast .detalhes-grid-cell:nth-child(10n+9),
-        #modalDetalhesAbast .detalhes-grid-cell:nth-child(10n+10) {
-            background: var(--abast-light);
-        }
-
-        #modalDetalhesAbast .modal-footer {
-            background: var(--abast-cream);
-            border-top: 1px solid rgba(168, 120, 76, 0.2);
-            padding: 1rem 1.5rem;
-        }
-
-        #modalDetalhesAbast .btn-fechar-modal {
-            background: var(--abast-primary);
-            border: none;
-            color: white;
-            padding: 8px 24px;
-            border-radius: 6px;
-            font-weight: 600;
-        }
-
-        #modalDetalhesAbast .btn-fechar-modal:hover {
-            background: var(--abast-dark);
-        }
-
-        /* ================================================================
-               MAPA DE CALOR
-               ================================================================ */
-        .heatmap-container {
-            background: white;
-            border-radius: 8px;
-            padding: 10px;
-        }
-
-        /* Legenda customizada com faixas de valores */
-        .heatmap-legenda-custom {
-            display: flex;
-            justify-content: center;
-            align-items: center;
-            gap: 8px;
-            margin-top: 12px;
-            padding: 10px;
-            background: #f8f9fa;
-            border-radius: 6px;
-            flex-wrap: wrap;
-        }
-
-        .heatmap-legenda-item {
-            display: flex;
-            align-items: center;
-            gap: 4px;
-            font-size: 0.72rem;
-            color: #555;
-        }
-
-        .heatmap-legenda-cor {
-            width: 16px;
-            height: 16px;
-            border-radius: 3px;
-            border: 1px solid #ddd;
-        }
-
-        .heatmap-legenda-range {
-            font-family: 'Outfit', sans-serif;
-            white-space: nowrap;
-        }
-
-        /* Responsividade */
-        @@media (max-width: 992px) {
-            .valor-metrica-abast {
-                font-size: 1.3rem;
-            }
-
-            .chart-container-abast {
-                margin-bottom: 1rem;
-            }
-
-            .dash-tab {
-                font-size: 0.85rem;
-                padding: 10px 16px;
-            }
-
-            .header-dashboard-titulo-abast {
-                font-size: 1.4rem;
-            }
-        }
-
-        @@media (max-width: 768px) {
-            .dash-tabs {
-                flex-direction: column;
-            }
-
-            .card-estatistica-abast {
-                margin-bottom: 12px;
-            }
-
-            .header-dashboard-abast {
-                padding: 16px 20px;
-            }
-        }
-
-        /* ========== BOTÃO EXPORTAR PDF ========== */
-        .abast-btn-pdf {
-            display: inline-flex;
-            align-items: center;
-            gap: 4px;
-            padding: 4px 8px;
-            background: linear-gradient(135deg, var(--abast-primary), var(--abast-dark));
-            color: white;
-            border: none;
-            border-radius: 4px;
-            font-size: 0.65rem;
-            font-weight: 600;
-            cursor: pointer;
-            transition: all 0.2s ease;
-            opacity: 0.85;
-            margin-left: auto;
-        }
-
-        .abast-btn-pdf:hover {
-            opacity: 1;
-            transform: translateY(-1px);
-            box-shadow: 0 3px 10px rgba(168, 120, 76, 0.4);
-        }
-
-        .abast-btn-pdf i {
-            font-size: 10px;
-        }
-
-        .chart-title-abast {
-            display: flex;
-            align-items: center;
-            gap: 8px;
-        }
-
-        /* ========================================
-               SELECT2 - Estilo para filtro de Placas
-               ======================================== */
-        /* Garantir que o container da Placa seja em bloco com label em cima */
-        .filtro-placa-container {
-            display: flex;
-            flex-direction: column;
-        }
-
-        .filtro-placa-container .form-label {
-            display: block;
-            margin-bottom: 6px;
-        }
-
-        #filtroPlacaVeiculo+.select2-container {
-            min-width: 220px !important;
-            display: block !important;
-        }
-
-        #filtroPlacaVeiculo+.select2-container .select2-selection--single {
-            height: 38px;
-            border: 1px solid #ced4da;
-            border-radius: 6px;
-            background: linear-gradient(135deg, #fff 0%, #f8f9fa 100%);
-        }
-
-        #filtroPlacaVeiculo+.select2-container .select2-selection--single .select2-selection__rendered {
-            line-height: 36px;
-            padding-left: 12px;
-            color: #495057;
-        }
-
-        #filtroPlacaVeiculo+.select2-container .select2-selection--single .select2-selection__arrow {
-            height: 36px;
-        }
-
-        #filtroPlacaVeiculo+.select2-container--focus .select2-selection--single {
-            border-color: var(--abast-accent);
-            box-shadow: 0 0 0 0.2rem rgba(168, 120, 76, 0.25);
-        }
-
-        /* Dropdown do Select2 */
-        .select2-dropdown {
-            border-color: var(--abast-accent);
-            border-radius: 6px;
-        }
-
-        .select2-results__option--highlighted[aria-selected] {
-            background-color: var(--abast-accent) !important;
-            color: white !important;
-        }
-
-        .select2-search--dropdown .select2-search__field {
-            border: 1px solid var(--abast-secondary);
-            border-radius: 4px;
-            padding: 6px 10px;
-        }
-
-        .select2-search--dropdown .select2-search__field:focus {
-            border-color: var(--abast-primary);
-            outline: none;
-        }
-    </style>
+<style>
+    /* ========================================
+       PALETA DE CORES - TEMA CARAMELO SUAVE
+       ======================================== */
+    :root {
+        --abast-primary: #a8784c;
+        --abast-secondary: #c4956a;
+        --abast-accent: #d4a574;
+        --abast-dark: #8b5e3c;
+        --abast-darker: #6d472c;
+        --abast-light: #faf6f1;
+        --abast-cream: #f5ebe0;
+        --abast-gradient: linear-gradient(135deg, #a8784c 0%, #c4956a 100%);
+    }
+
+    /* Header do Dashboard */
+    .header-dashboard-abast {
+        background: var(--abast-gradient);
+        color: white;
+        padding: 20px 24px;
+        border-radius: 10px;
+        margin-bottom: 24px;
+        box-shadow: 0 4px 15px rgba(168, 120, 76, 0.35);
+    }
+
+    .header-dashboard-titulo-abast {
+        margin: 0;
+        font-family: 'Outfit', sans-serif;
+        font-weight: 800;
+        font-size: 1.75rem;
+        display: flex;
+        align-items: center;
+        gap: 12px;
+        text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
+        position: relative;
+        z-index: 1;
+    }
+
+    .header-dashboard-titulo-abast i {
+        font-size: 1.75rem;
+        filter: drop-shadow(0 2px 3px rgba(0, 0, 0, 0.25));
+    }
+
+    /* Navegação de Abas - Atualizado 15/01/2026 */
+    .dash-tabs {
+        background: #f5ebe0 !important; /* var(--abast-cream) forçado */
+        border-radius: 12px;
+        padding: 6px;
+        margin-bottom: 24px;
+        box-shadow: 0 2px 8px rgba(168, 120, 76, 0.15);
+        display: flex;
+        gap: 6px;
+    }
+
+    .dash-tab {
+        flex: 1;
+        padding: 12px 20px;
+        border: none;
+        background: transparent;
+        border-radius: 8px;
+        font-weight: 600;
+        font-size: 0.9rem;
+        color: #8b5e3c !important; /* var(--abast-dark) forçado */
+        cursor: pointer;
+        transition: all 0.25s ease;
+        display: flex;
+        align-items: center;
+        justify-content: center;
+        gap: 8px;
+    }
+
+    .dash-tab:hover {
+        background: rgba(168, 120, 76, 0.15);
+        color: var(--abast-darker);
+    }
+
+    .dash-tab.active {
+        background: var(--abast-gradient);
+        color: white;
+        box-shadow: 0 4px 12px rgba(168, 120, 76, 0.35);
+    }
+
+    .dash-tab i { font-size: 1.1rem; }
+
+    .dash-content {
+        display: none !important;
+        visibility: hidden;
+        position: absolute;
+        left: -9999px;
+    }
+
+    .dash-content.active {
+        display: block !important;
+        visibility: visible;
+        position: relative;
+        left: 0;
+        animation: fadeIn 0.3s ease;
+    }
+
+    @@keyframes fadeIn {
+        from { opacity: 0; transform: translateY(10px); }
+        to { opacity: 1; transform: translateY(0); }
+    }
+
+    /* Container de Filtros */
+    .filter-card-abast {
+        background: var(--abast-gradient);
+        color: white;
+        border-radius: 12px;
+        padding: 20px 24px;
+        margin-bottom: 24px;
+        box-shadow: 0 4px 12px rgba(168, 120, 76, 0.25);
+    }
+
+    .filter-card-abast .form-label {
+        color: white;
+        font-weight: 600;
+        font-size: 0.85rem;
+        margin-bottom: 6px;
+    }
+
+    .filter-card-abast .form-control,
+    .filter-card-abast .form-select {
+        background: rgba(255,255,255,0.18);
+        border: 1px solid rgba(255,255,255,0.35);
+        color: white;
+        border-radius: 8px;
+        font-size: 0.9rem;
+        padding: 8px 12px;
+    }
+
+    .filter-card-abast .form-control::placeholder { color: rgba(255,255,255,0.7); }
+
+    .filter-card-abast .form-control:focus,
+    .filter-card-abast .form-select:focus {
+        background: rgba(255,255,255,0.28);
+        border-color: rgba(255,255,255,0.6);
+        box-shadow: 0 0 0 3px rgba(255,255,255,0.2);
+        color: white;
+    }
+
+    .filter-card-abast .form-select option {
+        background: var(--abast-dark);
+        color: white;
+    }
+
+    /* Botões de Filtro - Base comum */
+    .btn-filtrar-abast, .btn-limpar-abast {
+        padding: 8px 20px !important;
+        border-radius: 8px !important;
+        font-weight: 600 !important;
+        font-size: 0.875rem !important;
+        line-height: 1.5 !important;
+        transition: all 0.3s ease !important;
+        box-sizing: border-box !important;
+    }
+
+    /* Botão Filtrar */
+    .btn-filtrar-abast {
+        background: rgba(255,255,255,0.2) !important;
+        border: 2px solid rgba(255,255,255,0.5) !important;
+        color: white !important;
+    }
+
+    .btn-filtrar-abast:hover {
+        background: rgba(255,255,255,0.35) !important;
+        border-color: white !important;
+        transform: translateY(-1px) !important;
+    }
+
+    /* Botão Limpar Filtro */
+    .btn-limpar-abast {
+        background: linear-gradient(135deg, #B28767 0%, #9a7254 100%) !important;
+        border: 2px solid rgba(255, 255, 255, 0.6) !important;
+        color: white !important;
+        box-shadow: 0 2px 8px rgba(178, 135, 103, 0.4);
+    }
+
+    .btn-limpar-abast:hover {
+        background: linear-gradient(135deg, #9a7254 0%, #825d43 100%) !important;
+        border-color: rgba(255, 255, 255, 0.9) !important;
+        color: white !important;
+        transform: translateY(-1px);
+        box-shadow: 0 4px 12px rgba(75, 85, 99, 0.5);
+    }
+
+    /* Botões de Período Rápido */
+    .btn-period-abast, .btn-period-abast-veiculo {
+        border-radius: 20px;
+        padding: 8px 16px;
+        font-weight: 600;
+        font-size: 0.85rem;
+        transition: all 0.2s;
+        border: 2px solid rgba(255,255,255,0.3);
+        background: rgba(255,255,255,0.15);
+        color: white;
+    }
+
+    .btn-period-abast:hover, .btn-period-abast-veiculo:hover {
+        transform: scale(1.05);
+        background: rgba(255,255,255,0.25);
+        border-color: rgba(255,255,255,0.5);
+        color: white;
+    }
+
+    .btn-period-abast.active, .btn-period-abast-veiculo.active {
+        background: white;
+        color: var(--abast-primary);
+        border-color: white;
+    }
+
+    /* Indicador de Período */
+    .periodo-atual-abast {
+        background: rgba(255,255,255,0.2);
+        border: 1px solid rgba(255,255,255,0.3);
+        border-radius: 8px;
+        padding: 10px 16px;
+        color: white;
+        font-size: 0.9rem;
+    }
+
+    /* Cards de Estatísticas */
+    .card-estatistica-abast {
+        background: white;
+        border-radius: 10px;
+        padding: 16px 20px;
+        border-left: 4px solid;
+        box-shadow: 0 2px 8px rgba(0,0,0,0.06);
+        transition: all 0.3s ease;
+        height: 100%;
+        min-height: 90px;
+    }
+
+    .card-estatistica-abast:hover {
+        box-shadow: 0 6px 16px rgba(0,0,0,0.1);
+        transform: translateY(-3px);
+    }
+
+    .card-estatistica-abast.amber { border-left-color: var(--abast-primary); }
+    .card-estatistica-abast.orange { border-left-color: #ea580c; }
+    .card-estatistica-abast.yellow { border-left-color: var(--abast-accent); }
+    .card-estatistica-abast.brown { border-left-color: var(--abast-darker); }
+    .card-estatistica-abast.gold { border-left-color: #ca8a04; }
+
+    .icone-card-abast {
+        font-size: 1.4rem;
+        margin-bottom: 8px;
+    }
+
+    .card-estatistica-abast.amber .icone-card-abast { color: var(--abast-primary); }
+    .card-estatistica-abast.orange .icone-card-abast { color: #ea580c; }
+    .card-estatistica-abast.yellow .icone-card-abast { color: var(--abast-accent); }
+    .card-estatistica-abast.brown .icone-card-abast { color: var(--abast-darker); }
+    .card-estatistica-abast.gold .icone-card-abast { color: #ca8a04; }
+
+    .texto-metrica-abast {
+        font-size: 0.7rem;
+        color: #6B7280;
+        font-weight: 600;
+        text-transform: uppercase;
+        letter-spacing: 0.5px;
+        margin-bottom: 6px;
+    }
+
+    .valor-metrica-abast {
+        font-size: 1.5rem;
+        font-weight: 800;
+        color: #111827;
+        line-height: 1.1;
+    }
+
+    /* Cards de Gráficos */
+    .chart-container-abast {
+        background: white;
+        border-radius: 12px;
+        padding: 20px;
+        box-shadow: 0 2px 8px rgba(0,0,0,0.06);
+        transition: box-shadow 0.3s ease;
+        height: 100%;
+        border: none;
+    }
+
+    .chart-container-abast:hover {
+        box-shadow: 0 6px 16px rgba(0,0,0,0.1);
+    }
+
+    .chart-title-abast {
+        font-size: 0.95rem;
+        font-weight: 700;
+        margin-bottom: 16px;
+        color: #111827;
+        display: flex;
+        align-items: center;
+        gap: 10px;
+        padding-bottom: 12px;
+        border-bottom: 2px solid var(--abast-cream);
+    }
+
+    .chart-title-abast i {
+        color: var(--abast-primary);
+        font-size: 1.1rem;
+    }
+
+    .chart-subtitle {
+        font-size: 0.7rem;
+        color: #9ca3af;
+        font-weight: 400;
+        margin-left: auto;
+    }
+
+    /* Card Mensal Grande */
+    .stat-card-mensal-abast {
+        background: white;
+        border-radius: 12px;
+        padding: 20px;
+        box-shadow: 0 2px 8px rgba(0,0,0,0.06);
+        height: 100%;
+        display: flex;
+        flex-direction: column;
+    }
+
+    .stat-card-mensal-abast .card-header-custom {
+        display: flex;
+        align-items: center;
+        gap: 10px;
+        margin-bottom: 12px;
+        padding-bottom: 10px;
+        border-bottom: 2px solid var(--abast-cream);
+    }
+
+    .stat-card-mensal-abast .card-header-custom i {
+        color: var(--abast-primary);
+        font-size: 1.1rem;
+    }
+
+    .stat-card-mensal-abast .card-header-custom span {
+        font-size: 0.9rem;
+        font-weight: 700;
+        color: #111827;
+    }
+
+    .stat-card-mensal-abast .big-value {
+        font-size: 2rem;
+        font-weight: 800;
+        color: var(--abast-dark);
+        text-align: center;
+        margin-top: auto;
+        margin-bottom: auto;
+    }
+
+    /* =====================================================
+       GRID-TABELAS - Usando DIVs para evitar CSS global de table
+       ===================================================== */
+    .ftx-grid-tabela {
+        width: 100%;
+        font-size: 0.8rem;
+    }
+
+    .ftx-grid-tabela .grid-header {
+        display: grid;
+        background: linear-gradient(135deg, #a8784c 0%, #c4956a 100%);
+        border-radius: 6px 6px 0 0;
+    }
+
+    .ftx-grid-tabela .grid-header-cell {
+        color: #ffffff;
+        font-weight: 600;
+        font-size: 0.75rem;
+        text-transform: uppercase;
+        letter-spacing: 0.3px;
+        padding: 10px 12px;
+    }
+
+    .ftx-grid-tabela .grid-body {
+        max-height: 280px;
+        overflow-y: auto;
+    }
+
+    .ftx-grid-tabela .grid-row {
+        display: grid;
+        border-bottom: 1px solid #f3f4f6;
+        transition: background-color 0.15s;
+    }
+
+    .ftx-grid-tabela .grid-row:hover {
+        background-color: var(--abast-light);
+    }
+
+    .ftx-grid-tabela .grid-cell {
+        padding: 10px 12px;
+    }
+
+    .ftx-grid-tabela .text-end {
+        text-align: right;
+    }
+
+    /* Layout 2 colunas (Resumo por Ano, Top por Tipo) */
+    .ftx-grid-tabela.cols-2 .grid-header,
+    .ftx-grid-tabela.cols-2 .grid-row {
+        grid-template-columns: 1fr auto;
+    }
+
+    /* Layout 3 colunas (Top por Placa) */
+    .ftx-grid-tabela.cols-3 .grid-header,
+    .ftx-grid-tabela.cols-3 .grid-row {
+        grid-template-columns: auto 1fr auto;
+    }
+
+    .ftx-grid-tabela .grid-row-total {
+        background: var(--abast-cream);
+        font-weight: 700;
+    }
+
+    /* Linha clicável - abre detalhes */
+    .ftx-grid-tabela .grid-row-clicavel {
+        cursor: pointer;
+    }
+
+    .ftx-grid-tabela .grid-row-clicavel:hover {
+        background-color: #e8f4e8;
+        box-shadow: inset 3px 0 0 var(--abast-primary);
+    }
+
+    .ftx-grid-tabela .grid-row-clicavel:active {
+        background-color: #d4ecd4;
+    }
+
+    /* Badge de Ranking */
+    .badge-rank-abast {
+        display: inline-flex;
+        align-items: center;
+        justify-content: center;
+        width: 24px;
+        height: 24px;
+        border-radius: 50%;
+        background: var(--abast-secondary);
+        color: white;
+        font-size: 0.7rem;
+        font-weight: 700;
+        margin-right: 8px;
+    }
+
+    .badge-rank-abast.top3 {
+        background: var(--abast-primary);
+        box-shadow: 0 2px 6px rgba(168, 120, 76, 0.4);
+    }
+
+    /* Tabela Média Litro */
+    .table-media-litro-abast {
+        font-size: 0.8rem;
+        margin: 0;
+    }
+
+    .table-media-litro-abast tbody tr {
+        border-bottom: 1px solid #f3f4f6;
+    }
+
+    .table-media-litro-abast tbody td {
+        padding: 8px 10px;
+    }
+
+    /* Loading - Usa padrão FrotiX global (ftx-spin-overlay) */
+
+    /* ================================================================
+       MODAL DE DETALHES - Tema Caramelo/Creme
+       ================================================================ */
+    #modalDetalhesAbast .modal-dialog {
+        max-width: 900px;
+    }
+
+    #modalDetalhesAbast .modal-content {
+        border: none;
+        border-radius: 12px;
+        overflow: hidden;
+        box-shadow: 0 10px 40px rgba(0,0,0,0.25);
+    }
+
+    #modalDetalhesAbast .modal-header {
+        background: linear-gradient(135deg, #a8784c 0%, #c4956a 100%);
+        border-bottom: none;
+        padding: 1rem 1.5rem;
+    }
+
+    #modalDetalhesAbast .modal-title {
+        font-family: 'Outfit', sans-serif;
+        font-weight: 700;
+        font-size: 1.1rem;
+        color: #fff;
+        display: flex;
+        align-items: center;
+        gap: 0.5rem;
+    }
+
+    #modalDetalhesAbast .modal-title i {
+        font-size: 1.2rem;
+    }
+
+    #modalDetalhesAbast .btn-close {
+        filter: brightness(0) invert(1);
+        opacity: 0.8;
+    }
+
+    #modalDetalhesAbast .btn-close:hover {
+        opacity: 1;
+    }
+
+    #modalDetalhesAbast .modal-body {
+        background: var(--abast-cream);
+        padding: 1.5rem;
+        max-height: 500px;
+        overflow-y: auto;
+    }
+
+    #modalDetalhesAbast .detalhes-resumo {
+        background: white;
+        border-radius: 8px;
+        padding: 1rem;
+        margin-bottom: 1rem;
+        display: flex;
+        gap: 2rem;
+        flex-wrap: wrap;
+    }
+
+    #modalDetalhesAbast .detalhes-resumo-item {
+        text-align: center;
+    }
+
+    #modalDetalhesAbast .detalhes-resumo-label {
+        font-size: 0.7rem;
+        color: #6c757d;
+        text-transform: uppercase;
+        font-weight: 600;
+    }
+
+    #modalDetalhesAbast .detalhes-resumo-valor {
+        font-size: 1.2rem;
+        font-weight: 700;
+        color: var(--abast-dark);
+    }
+
+    #modalDetalhesAbast .detalhes-grid {
+        display: grid;
+        grid-template-columns: repeat(5, 1fr);
+        gap: 0;
+        background: white;
+        border-radius: 8px;
+        overflow: hidden;
+    }
+
+    #modalDetalhesAbast .detalhes-grid-header {
+        background: linear-gradient(135deg, #a8784c 0%, #c4956a 100%);
+        color: white;
+        font-weight: 600;
+        font-size: 0.75rem;
+        text-transform: uppercase;
+        padding: 10px 12px;
+    }
+
+    #modalDetalhesAbast .detalhes-grid-cell {
+        padding: 8px 12px;
+        font-size: 0.8rem;
+        border-bottom: 1px solid #f3f4f6;
+    }
+
+    #modalDetalhesAbast .detalhes-grid-cell:nth-child(10n+6),
+    #modalDetalhesAbast .detalhes-grid-cell:nth-child(10n+7),
+    #modalDetalhesAbast .detalhes-grid-cell:nth-child(10n+8),
+    #modalDetalhesAbast .detalhes-grid-cell:nth-child(10n+9),
+    #modalDetalhesAbast .detalhes-grid-cell:nth-child(10n+10) {
+        background: var(--abast-light);
+    }
+
+    #modalDetalhesAbast .modal-footer {
+        background: var(--abast-cream);
+        border-top: 1px solid rgba(168, 120, 76, 0.2);
+        padding: 1rem 1.5rem;
+    }
+
+    #modalDetalhesAbast .btn-fechar-modal {
+        background: var(--abast-primary);
+        border: none;
+        color: white;
+        padding: 8px 24px;
+        border-radius: 6px;
+        font-weight: 600;
+    }
+
+    #modalDetalhesAbast .btn-fechar-modal:hover {
+        background: var(--abast-dark);
+    }
+
+    /* ================================================================
+       MAPA DE CALOR
+       ================================================================ */
+    .heatmap-container {
+        background: white;
+        border-radius: 8px;
+        padding: 10px;
+    }
+
+    /* Legenda customizada com faixas de valores */
+    .heatmap-legenda-custom {
+        display: flex;
+        justify-content: center;
+        align-items: center;
+        gap: 8px;
+        margin-top: 12px;
+        padding: 10px;
+        background: #f8f9fa;
+        border-radius: 6px;
+        flex-wrap: wrap;
+    }
+
+    .heatmap-legenda-item {
+        display: flex;
+        align-items: center;
+        gap: 4px;
+        font-size: 0.72rem;
+        color: #555;
+    }
+
+    .heatmap-legenda-cor {
+        width: 16px;
+        height: 16px;
+        border-radius: 3px;
+        border: 1px solid #ddd;
+    }
+
+    .heatmap-legenda-range {
+        font-family: 'Outfit', sans-serif;
+        white-space: nowrap;
+    }
+
+    /* Responsividade */
+    @@media (max-width: 992px) {
+        .valor-metrica-abast { font-size: 1.3rem; }
+        .chart-container-abast { margin-bottom: 1rem; }
+        .dash-tab { font-size: 0.85rem; padding: 10px 16px; }
+        .header-dashboard-titulo-abast { font-size: 1.4rem; }
+    }
+
+    @@media (max-width: 768px) {
+        .dash-tabs { flex-direction: column; }
+        .card-estatistica-abast { margin-bottom: 12px; }
+        .header-dashboard-abast { padding: 16px 20px; }
+    }
+
+    /* ========== BOTÃO EXPORTAR PDF ========== */
+    .abast-btn-pdf {
+        display: inline-flex;
+        align-items: center;
+        gap: 4px;
+        padding: 4px 8px;
+        background: linear-gradient(135deg, var(--abast-primary), var(--abast-dark));
+        color: white;
+        border: none;
+        border-radius: 4px;
+        font-size: 0.65rem;
+        font-weight: 600;
+        cursor: pointer;
+        transition: all 0.2s ease;
+        opacity: 0.85;
+        margin-left: auto;
+    }
+
+    .abast-btn-pdf:hover {
+        opacity: 1;
+        transform: translateY(-1px);
+        box-shadow: 0 3px 10px rgba(168, 120, 76, 0.4);
+    }
+
+    .abast-btn-pdf i {
+        font-size: 10px;
+    }
+
+    .chart-title-abast {
+        display: flex;
+        align-items: center;
+        gap: 8px;
+    }
+
+    /* ========================================
+       SELECT2 - Estilo para filtro de Placas
+       ======================================== */
+    /* Garantir que o container da Placa seja em bloco com label em cima */
+    .filtro-placa-container {
+        display: flex;
+        flex-direction: column;
+    }
+
+    .filtro-placa-container .form-label {
+        display: block;
+        margin-bottom: 6px;
+    }
+
+    #filtroPlacaVeiculo + .select2-container {
+        min-width: 220px !important;
+        display: block !important;
+    }
+
+    #filtroPlacaVeiculo + .select2-container .select2-selection--single {
+        height: 38px;
+        border: 1px solid #ced4da;
+        border-radius: 6px;
+        background: linear-gradient(135deg, #fff 0%, #f8f9fa 100%);
+    }
+
+    #filtroPlacaVeiculo + .select2-container .select2-selection--single .select2-selection__rendered {
+        line-height: 36px;
+        padding-left: 12px;
+        color: #495057;
+    }
+
+    #filtroPlacaVeiculo + .select2-container .select2-selection--single .select2-selection__arrow {
+        height: 36px;
+    }
+
+    #filtroPlacaVeiculo + .select2-container--focus .select2-selection--single {
+        border-color: var(--abast-accent);
+        box-shadow: 0 0 0 0.2rem rgba(168, 120, 76, 0.25);
+    }
+
+    /* Dropdown do Select2 */
+    .select2-dropdown {
+        border-color: var(--abast-accent);
+        border-radius: 6px;
+    }
+
+    .select2-results__option--highlighted[aria-selected] {
+        background-color: var(--abast-accent) !important;
+        color: white !important;
+    }
+
+    .select2-search--dropdown .select2-search__field {
+        border: 1px solid var(--abast-secondary);
+        border-radius: 4px;
+        padding: 6px 10px;
+    }
+
+    .select2-search--dropdown .select2-search__field:focus {
+        border-color: var(--abast-primary);
+        outline: none;
+    }
+</style>
 }
 
 <div id="dashboardAbastecimento" class="container-fluid">
@@ -847,621 +781,591 @@
 
     <div class="dash-tabs-container">
 
-        <div id="tab-consumo-geral" class="dash-content active">
-            <div class="filter-card-abast">
-
-                <div class="row align-items-end g-3 mb-3">
-                    <div class="col-auto">
-                        <label class="form-label">
-                            <i class="fa-duotone fa-calendar me-1"></i> Ano
-                        </label>
-                        <select id="filtroAnoGeral" class="form-select" style="min-width: 120px;">
-                            <option value="">&lt;Todos os Anos&gt;</option>
-                        </select>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">
-                            <i class="fa-duotone fa-calendar-day me-1"></i> Mês
-                        </label>
-                        <select id="filtroMesGeral" class="form-select" style="min-width: 150px;">
-                            <option value="">&lt;Todos os Meses&gt;</option>
-                        </select>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">&nbsp;</label>
-                        <button type="button" class="btn btn-filtrar-abast d-block" id="btnFiltrarAnoMesGeral">
-                            <i class="fa-duotone fa-magnifying-glass me-1"></i> Filtrar
+    <div id="tab-consumo-geral" class="dash-content active">
+        <div class="filter-card-abast">
+
+            <div class="row align-items-end g-3 mb-3">
+                <div class="col-auto">
+                    <label class="form-label">
+                        <i class="fa-duotone fa-calendar me-1"></i> Ano
+                    </label>
+                    <select id="filtroAnoGeral" class="form-select" style="min-width: 120px;">
+                        <option value="">&lt;Todos os Anos&gt;</option>
+                    </select>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">
+                        <i class="fa-duotone fa-calendar-day me-1"></i> Mês
+                    </label>
+                    <select id="filtroMesGeral" class="form-select" style="min-width: 150px;">
+                        <option value="">&lt;Todos os Meses&gt;</option>
+                    </select>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">&nbsp;</label>
+                    <button type="button" class="btn btn-filtrar-abast d-block" id="btnFiltrarAnoMesGeral">
+                        <i class="fa-duotone fa-magnifying-glass me-1"></i> Filtrar
+                    </button>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">&nbsp;</label>
+                    <button type="button" class="btn btn-limpar-abast d-block" id="btnLimparAnoMesGeral">
+                        <i class="fa-duotone fa-eraser me-1"></i> Limpar
+                    </button>
+                </div>
+            </div>
+
+            <div class="row align-items-end g-3">
+                <div class="col-auto">
+                    <label class="form-label">
+                        <i class="fa-duotone fa-calendar-range me-1"></i> Período Personalizado
+                    </label>
+                    <div class="d-flex gap-2 align-items-center">
+                        <div>
+                            <label class="form-label small mb-1">De:</label>
+                            <input type="date" id="dataInicioGeral" class="form-control" style="width: 140px;" />
+                        </div>
+                        <div>
+                            <label class="form-label small mb-1">Até:</label>
+                            <input type="date" id="dataFimGeral" class="form-control" style="width: 140px;" />
+                        </div>
+                    </div>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">&nbsp;</label>
+                    <button type="button" class="btn btn-filtrar-abast d-block" id="btnFiltrarPeriodoGeral">
+                        <i class="fa-duotone fa-magnifying-glass me-1"></i> Filtrar
+                    </button>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">&nbsp;</label>
+                    <button type="button" class="btn btn-limpar-abast d-block" id="btnLimparPeriodoGeral">
+                        <i class="fa-duotone fa-eraser me-1"></i> Limpar
+                    </button>
+                </div>
+                <div class="col">
+                    <label class="form-label">Períodos Rápidos</label>
+                    <div class="d-flex gap-2 flex-wrap">
+                        <button type="button" class="btn-period-abast" data-dias="7">7 dias</button>
+                        <button type="button" class="btn-period-abast" data-dias="15">15 dias</button>
+                        <button type="button" class="btn-period-abast" data-dias="30">30 dias</button>
+                        <button type="button" class="btn-period-abast" data-dias="60">60 dias</button>
+                        <button type="button" class="btn-period-abast" data-dias="90">90 dias</button>
+                        <button type="button" class="btn-period-abast" data-dias="180">180 dias</button>
+                        <button type="button" class="btn-period-abast" data-dias="365">1 ano</button>
+                    </div>
+                </div>
+            </div>
+
+            <div class="row mt-3">
+                <div class="col-12">
+                    <div class="periodo-atual-abast d-flex align-items-center">
+                        <i class="fa-duotone fa-info-circle me-2"></i>
+                        <span id="periodoAtualLabelGeral">Exibindo todos os dados</span>
+                    </div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-2">
+            <div class="col-lg col-md-6">
+                <div class="card-estatistica-abast amber">
+                    <div class="icone-card-abast"><i class="fa-duotone fa-sack-dollar"></i></div>
+                    <div class="texto-metrica-abast">Valor Total</div>
+                    <div class="valor-metrica-abast" id="valorTotalGeral">R$ 0</div>
+                </div>
+            </div>
+            <div class="col-lg col-md-6">
+                <div class="card-estatistica-abast orange">
+                    <div class="icone-card-abast"><i class="fa-duotone fa-gas-pump"></i></div>
+                    <div class="texto-metrica-abast">Total de Litros</div>
+                    <div class="valor-metrica-abast" id="litrosTotalGeral">0</div>
+                </div>
+            </div>
+            <div class="col-lg col-md-6">
+                <div class="card-estatistica-abast yellow">
+                    <div class="icone-card-abast"><i class="fa-duotone fa-gauge-high"></i></div>
+                    <div class="texto-metrica-abast">Abastecimentos</div>
+                    <div class="valor-metrica-abast" id="qtdAbastecimentosGeral">0</div>
+                </div>
+            </div>
+            <div class="col-lg col-md-6">
+                <div class="card-estatistica-abast brown">
+                    <div class="icone-card-abast"><i class="fa-duotone fa-droplet"></i></div>
+                    <div class="texto-metrica-abast">Diesel S-10</div>
+                    <div class="valor-metrica-abast" id="mediaDieselGeral">R$ 0</div>
+                </div>
+            </div>
+            <div class="col-lg col-md-6">
+                <div class="card-estatistica-abast gold">
+                    <div class="icone-card-abast"><i class="fa-duotone fa-droplet"></i></div>
+                    <div class="texto-metrica-abast">Gasolina</div>
+                    <div class="valor-metrica-abast" id="mediaGasolinaGeral">R$ 0</div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3 mt-1">
+            <div class="col-lg-3">
+                <div class="chart-container-abast" style="min-height: 360px;">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-calendar-lines"></i>
+                        Resumo por Ano
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('tabelaResumoPorAno', 'Resumo por Ano')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
                         </button>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">&nbsp;</label>
-                        <button type="button" class="btn btn-limpar-abast d-block" id="btnLimparAnoMesGeral">
-                            <i class="fa-duotone fa-eraser me-1"></i> Limpar
-                        </button>
-                    </div>
-                </div>
-
-                <div class="row align-items-end g-3">
-                    <div class="col-auto">
-                        <label class="form-label">
-                            <i class="fa-duotone fa-calendar-range me-1"></i> Período Personalizado
-                        </label>
-                        <div class="d-flex gap-2 align-items-center">
-                            <div>
-                                <label class="form-label small mb-1">De:</label>
-                                <input type="date" id="dataInicioGeral" class="form-control" style="width: 140px;" />
-                            </div>
-                            <div>
-                                <label class="form-label small mb-1">Até:</label>
-                                <input type="date" id="dataFimGeral" class="form-control" style="width: 140px;" />
+                    </h5>
+                    <div class="ftx-grid-tabela cols-2">
+                        <div class="grid-header">
+                            <div class="grid-header-cell">Ano</div>
+                            <div class="grid-header-cell text-end">Valor (R$)</div>
+                        </div>
+                        <div class="grid-body" id="tabelaResumoPorAno">
+                            <div class="grid-row" style="justify-content: center; padding: 20px;">
+                                <i class="fa-duotone fa-spinner-third fa-spin"></i>
                             </div>
                         </div>
                     </div>
-                    <div class="col-auto">
-                        <label class="form-label">&nbsp;</label>
-                        <button type="button" class="btn btn-filtrar-abast d-block" id="btnFiltrarPeriodoGeral">
-                            <i class="fa-duotone fa-magnifying-glass me-1"></i> Filtrar
+                </div>
+            </div>
+            <div class="col-lg-5">
+                <div class="chart-container-abast" style="min-height: 360px;">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-chart-bar"></i>
+                        Valor Total por Categoria
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('chartValorCategoria', 'Valor Total por Categoria')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
                         </button>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">&nbsp;</label>
-                        <button type="button" class="btn btn-limpar-abast d-block" id="btnLimparPeriodoGeral">
-                            <i class="fa-duotone fa-eraser me-1"></i> Limpar
+                    </h5>
+                    <div id="chartValorCategoria" style="height: 300px;"></div>
+                </div>
+            </div>
+            <div class="col-lg-4">
+                <div class="chart-container-abast" style="min-height: 360px;">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-chart-line"></i>
+                        Valor do Litro por Mês
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('chartValorLitro', 'Valor do Litro por Mes')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
                         </button>
-                    </div>
-                    <div class="col">
-                        <label class="form-label">Períodos Rápidos</label>
-                        <div class="d-flex gap-2 flex-wrap">
-                            <button type="button" class="btn-period-abast" data-dias="7">7 dias</button>
-                            <button type="button" class="btn-period-abast" data-dias="15">15 dias</button>
-                            <button type="button" class="btn-period-abast" data-dias="30">30 dias</button>
-                            <button type="button" class="btn-period-abast" data-dias="60">60 dias</button>
-                            <button type="button" class="btn-period-abast" data-dias="90">90 dias</button>
-                            <button type="button" class="btn-period-abast" data-dias="180">180 dias</button>
-                            <button type="button" class="btn-period-abast" data-dias="365">1 ano</button>
+                    </h5>
+                    <div id="chartValorLitro" style="height: 300px;"></div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3 mt-1">
+            <div class="col-lg-6">
+                <div class="chart-container-abast" style="min-height: 340px;">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-chart-area"></i>
+                        Litros por Mês
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('chartLitrosMes', 'Litros por Mes')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
+                        </button>
+                    </h5>
+                    <div id="chartLitrosMes" style="height: 280px;"></div>
+                </div>
+            </div>
+            <div class="col-lg-6">
+                <div class="chart-container-abast" style="min-height: 340px;">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-chart-column"></i>
+                        Consumo Geral por Mês
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('chartConsumoMes', 'Consumo Geral por Mes')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
+                        </button>
+                    </h5>
+                    <div id="chartConsumoMes" style="height: 280px;"></div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3 mt-2">
+            <div class="col-12">
+                <div class="chart-container-abast">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-grid-2"></i>
+                        Mapa de Calor - Consumo por Dia/Hora
+                        <span class="chart-subtitle">Visualize quando ocorrem mais abastecimentos (clique para ver detalhes)</span>
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('heatmapDiaHora', 'Mapa de Calor - Consumo por Dia-Hora')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
+                        </button>
+                    </h5>
+                    <div class="heatmap-container">
+                        <div id="heatmapDiaHora" style="height: 450px;"></div>
+                        <div id="legendaHeatmapDiaHora" class="heatmap-legenda-custom"></div>
+                    </div>
+                </div>
+            </div>
+        </div>
+    </div>
+
+    <div id="tab-consumo-mensal" class="dash-content">
+
+        <div class="filter-card-abast">
+            <div class="row align-items-end g-3">
+                <div class="col-md-3">
+                    <label class="form-label">
+                        <i class="fa-duotone fa-calendar me-1"></i> Ano
+                    </label>
+                    <select id="filtroAnoMensal" class="form-select"></select>
+                </div>
+                <div class="col-md-4">
+                    <label class="form-label">
+                        <i class="fa-duotone fa-calendar-day me-1"></i> Mês
+                    </label>
+                    <select id="filtroMesMensal" class="form-select">
+                        <option value="">&lt;Todos os Meses&gt;</option>
+                    </select>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">&nbsp;</label>
+                    <button type="button" class="btn btn-filtrar-abast d-block" id="btnFiltrarMensal">
+                        <i class="fa-duotone fa-magnifying-glass me-1"></i> Filtrar
+                    </button>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">&nbsp;</label>
+                    <button type="button" class="btn btn-limpar-abast d-block" id="btnLimparMensal">
+                        <i class="fa-duotone fa-eraser me-1"></i> Limpar
+                    </button>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3">
+
+            <div class="col-lg-3 col-md-6">
+                <div class="stat-card-mensal-abast">
+                    <div class="card-header-custom">
+                        <i class="fa-duotone fa-sack-dollar"></i>
+                        <span>Valor Total</span>
+                    </div>
+                    <div class="big-value" id="valorTotalMensal">R$ 0</div>
+                </div>
+            </div>
+
+            <div class="col-lg-3 col-md-6">
+                <div class="stat-card-mensal-abast">
+                    <div class="card-header-custom">
+                        <i class="fa-duotone fa-gas-pump"></i>
+                        <span>Total de Litros</span>
+                    </div>
+                    <div class="big-value" id="totalLitrosMensal">0</div>
+                </div>
+            </div>
+
+            <div class="col-lg-3 col-md-6">
+                <div class="stat-card-mensal-abast">
+                    <div class="card-header-custom">
+                        <i class="fa-duotone fa-dollar-sign"></i>
+                        <span>Média do Litro</span>
+                    </div>
+                    <div class="table-responsive" style="flex: 1;">
+                        <table class="table table-media-litro-abast mb-0">
+                            <tbody id="tabelaMediaLitroMensal">
+                                <tr><td colspan="2" class="text-center py-3"><i class="fa-duotone fa-spinner-third fa-spin"></i></td></tr>
+                            </tbody>
+                        </table>
+                    </div>
+                </div>
+            </div>
+
+            <div class="col-lg-3 col-md-6">
+                <div class="stat-card-mensal-abast" style="min-height: 220px;">
+                    <div class="card-header-custom">
+                        <i class="fa-duotone fa-chart-pie"></i>
+                        <span>Combustíveis</span>
+                    </div>
+                    <div id="chartPizzaCombustivel" style="height: 180px;"></div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3 mt-2">
+            <div class="col-12">
+                <div class="chart-container-abast">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-chart-area"></i>
+                        Litros por Dia
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('chartLitrosDia', 'Litros por Dia')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
+                        </button>
+                    </h5>
+                    <div id="chartLitrosDia" style="height: 220px;"></div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3 mt-2">
+
+            <div class="col-lg-6">
+                <div class="chart-container-abast" style="min-height: 480px;">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-building"></i>
+                        Top 15 por Unidade
+                        <span class="chart-subtitle">Soma de todos os abastecimentos por unidade</span>
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('tabelaValorPorUnidade', 'Top 15 por Unidade', 'portrait')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
+                        </button>
+                    </h5>
+                    <div class="ftx-grid-tabela cols-2">
+                        <div class="grid-header">
+                            <div class="grid-header-cell">Unidade</div>
+                            <div class="grid-header-cell text-end">Valor</div>
                         </div>
-                    </div>
-                </div>
-
-                <div class="row mt-3">
-                    <div class="col-12">
-                        <div class="periodo-atual-abast d-flex align-items-center">
-                            <i class="fa-duotone fa-info-circle me-2"></i>
-                            <span id="periodoAtualLabelGeral">Exibindo todos os dados</span>
-                        </div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-2">
-                <div class="col-lg col-md-6">
-                    <div class="card-estatistica-abast amber">
-                        <div class="icone-card-abast"><i class="fa-duotone fa-sack-dollar"></i></div>
-                        <div class="texto-metrica-abast">Valor Total</div>
-                        <div class="valor-metrica-abast" id="valorTotalGeral">R$ 0</div>
-                    </div>
-                </div>
-                <div class="col-lg col-md-6">
-                    <div class="card-estatistica-abast orange">
-                        <div class="icone-card-abast"><i class="fa-duotone fa-gas-pump"></i></div>
-                        <div class="texto-metrica-abast">Total de Litros</div>
-                        <div class="valor-metrica-abast" id="litrosTotalGeral">0</div>
-                    </div>
-                </div>
-                <div class="col-lg col-md-6">
-                    <div class="card-estatistica-abast yellow">
-                        <div class="icone-card-abast"><i class="fa-duotone fa-gauge-high"></i></div>
-                        <div class="texto-metrica-abast">Abastecimentos</div>
-                        <div class="valor-metrica-abast" id="qtdAbastecimentosGeral">0</div>
-                    </div>
-                </div>
-                <div class="col-lg col-md-6">
-                    <div class="card-estatistica-abast brown">
-                        <div class="icone-card-abast"><i class="fa-duotone fa-droplet"></i></div>
-                        <div class="texto-metrica-abast">Diesel S-10</div>
-                        <div class="valor-metrica-abast" id="mediaDieselGeral">R$ 0</div>
-                    </div>
-                </div>
-                <div class="col-lg col-md-6">
-                    <div class="card-estatistica-abast gold">
-                        <div class="icone-card-abast"><i class="fa-duotone fa-droplet"></i></div>
-                        <div class="texto-metrica-abast">Gasolina</div>
-                        <div class="valor-metrica-abast" id="mediaGasolinaGeral">R$ 0</div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3 mt-1">
-                <div class="col-lg-3">
-                    <div class="chart-container-abast" style="min-height: 360px;">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-calendar-lines"></i>
-                            Resumo por Ano
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('tabelaResumoPorAno', 'Resumo por Ano')"
-                                title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div class="ftx-grid-tabela cols-2">
-                            <div class="grid-header">
-                                <div class="grid-header-cell">Ano</div>
-                                <div class="grid-header-cell text-end">Valor (R$)</div>
-                            </div>
-                            <div class="grid-body" id="tabelaResumoPorAno">
-                                <div class="grid-row" style="justify-content: center; padding: 20px;">
-                                    <i class="fa-duotone fa-spinner-third fa-spin"></i>
-                                </div>
+                        <div class="grid-body" id="tabelaValorPorUnidade" style="max-height: 380px;">
+                            <div class="grid-row" style="justify-content: center; padding: 20px;">
+                                <i class="fa-duotone fa-spinner-third fa-spin"></i>
                             </div>
                         </div>
                     </div>
                 </div>
-                <div class="col-lg-5">
-                    <div class="chart-container-abast" style="min-height: 360px;">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-chart-bar"></i>
-                            Valor Total por Categoria
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('chartValorCategoria', 'Valor Total por Categoria')"
-                                title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div id="chartValorCategoria" style="height: 300px;"></div>
-                    </div>
-                </div>
-                <div class="col-lg-4">
-                    <div class="chart-container-abast" style="min-height: 360px;">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-chart-line"></i>
-                            Valor do Litro por Mês
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('chartValorLitro', 'Valor do Litro por Mes')"
-                                title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div id="chartValorLitro" style="height: 300px;"></div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3 mt-1">
-                <div class="col-lg-6">
-                    <div class="chart-container-abast" style="min-height: 340px;">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-chart-area"></i>
-                            Litros por Mês
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('chartLitrosMes', 'Litros por Mes')" title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div id="chartLitrosMes" style="height: 280px;"></div>
-                    </div>
-                </div>
-                <div class="col-lg-6">
-                    <div class="chart-container-abast" style="min-height: 340px;">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-chart-column"></i>
-                            Consumo Geral por Mês
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('chartConsumoMes', 'Consumo Geral por Mes')"
-                                title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div id="chartConsumoMes" style="height: 280px;"></div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3 mt-2">
-                <div class="col-12">
-                    <div class="chart-container-abast">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-grid-2"></i>
-                            Mapa de Calor - Consumo por Dia/Hora
-                            <span class="chart-subtitle">Visualize quando ocorrem mais abastecimentos (clique para ver
-                                detalhes)</span>
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('heatmapDiaHora', 'Mapa de Calor - Consumo por Dia-Hora')"
-                                title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div class="heatmap-container">
-                            <div id="heatmapDiaHora" style="height: 450px;"></div>
-                            <div id="legendaHeatmapDiaHora" class="heatmap-legenda-custom"></div>
+            </div>
+
+            <div class="col-lg-6">
+                <div class="chart-container-abast" style="min-height: 480px;">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-rectangle-barcode"></i>
+                        Top 15 por Placa Individual
+                        <span class="chart-subtitle">Cada veículo separadamente</span>
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('tabelaValorPorPlaca', 'Top 15 por Placa Individual', 'portrait')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
+                        </button>
+                    </h5>
+                    <div class="ftx-grid-tabela cols-3">
+                        <div class="grid-header">
+                            <div class="grid-header-cell">Placa</div>
+                            <div class="grid-header-cell">Tipo</div>
+                            <div class="grid-header-cell text-end">Valor</div>
                         </div>
-                    </div>
-                </div>
-            </div>
-        </div>
-
-        <div id="tab-consumo-mensal" class="dash-content">
-
-            <div class="filter-card-abast">
-                <div class="row align-items-end g-3">
-                    <div class="col-md-3">
-                        <label class="form-label">
-                            <i class="fa-duotone fa-calendar me-1"></i> Ano
-                        </label>
-                        <select id="filtroAnoMensal" class="form-select"></select>
-                    </div>
-                    <div class="col-md-4">
-                        <label class="form-label">
-                            <i class="fa-duotone fa-calendar-day me-1"></i> Mês
-                        </label>
-                        <select id="filtroMesMensal" class="form-select">
-                            <option value="">&lt;Todos os Meses&gt;</option>
-                        </select>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">&nbsp;</label>
-                        <button type="button" class="btn btn-filtrar-abast d-block" id="btnFiltrarMensal">
-                            <i class="fa-duotone fa-magnifying-glass me-1"></i> Filtrar
-                        </button>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">&nbsp;</label>
-                        <button type="button" class="btn btn-limpar-abast d-block" id="btnLimparMensal">
-                            <i class="fa-duotone fa-eraser me-1"></i> Limpar
-                        </button>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3">
-
-                <div class="col-lg-3 col-md-6">
-                    <div class="stat-card-mensal-abast">
-                        <div class="card-header-custom">
-                            <i class="fa-duotone fa-sack-dollar"></i>
-                            <span>Valor Total</span>
-                        </div>
-                        <div class="big-value" id="valorTotalMensal">R$ 0</div>
-                    </div>
-                </div>
-
-                <div class="col-lg-3 col-md-6">
-                    <div class="stat-card-mensal-abast">
-                        <div class="card-header-custom">
-                            <i class="fa-duotone fa-gas-pump"></i>
-                            <span>Total de Litros</span>
-                        </div>
-                        <div class="big-value" id="totalLitrosMensal">0</div>
-                    </div>
-                </div>
-
-                <div class="col-lg-3 col-md-6">
-                    <div class="stat-card-mensal-abast">
-                        <div class="card-header-custom">
-                            <i class="fa-duotone fa-dollar-sign"></i>
-                            <span>Média do Litro</span>
-                        </div>
-                        <div class="table-responsive" style="flex: 1;">
-                            <table class="table table-media-litro-abast mb-0">
-                                <tbody id="tabelaMediaLitroMensal">
-                                    <tr>
-                                        <td colspan="2" class="text-center py-3"><i
-                                                class="fa-duotone fa-spinner-third fa-spin"></i></td>
-                                    </tr>
-                                </tbody>
-                            </table>
-                        </div>
-                    </div>
-                </div>
-
-                <div class="col-lg-3 col-md-6">
-                    <div class="stat-card-mensal-abast" style="min-height: 220px;">
-                        <div class="card-header-custom">
-                            <i class="fa-duotone fa-chart-pie"></i>
-                            <span>Combustíveis</span>
-                        </div>
-                        <div id="chartPizzaCombustivel" style="height: 180px;"></div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3 mt-2">
-                <div class="col-12">
-                    <div class="chart-container-abast">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-chart-area"></i>
-                            Litros por Dia
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('chartLitrosDia', 'Litros por Dia')" title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div id="chartLitrosDia" style="height: 220px;"></div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3 mt-2">
-
-                <div class="col-lg-6">
-                    <div class="chart-container-abast" style="min-height: 480px;">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-building"></i>
-                            Top 15 por Unidade
-                            <span class="chart-subtitle">Soma de todos os abastecimentos por unidade</span>
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('tabelaValorPorUnidade', 'Top 15 por Unidade', 'portrait')"
-                                title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div class="ftx-grid-tabela cols-2">
-                            <div class="grid-header">
-                                <div class="grid-header-cell">Unidade</div>
-                                <div class="grid-header-cell text-end">Valor</div>
-                            </div>
-                            <div class="grid-body" id="tabelaValorPorUnidade" style="max-height: 380px;">
-                                <div class="grid-row" style="justify-content: center; padding: 20px;">
-                                    <i class="fa-duotone fa-spinner-third fa-spin"></i>
-                                </div>
+                        <div class="grid-body" id="tabelaValorPorPlaca" style="max-height: 380px;">
+                            <div class="grid-row" style="justify-content: center; padding: 20px;">
+                                <i class="fa-duotone fa-spinner-third fa-spin"></i>
                             </div>
                         </div>
                     </div>
                 </div>
-
-                <div class="col-lg-6">
-                    <div class="chart-container-abast" style="min-height: 480px;">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-rectangle-barcode"></i>
-                            Top 15 por Placa Individual
-                            <span class="chart-subtitle">Cada veículo separadamente</span>
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('tabelaValorPorPlaca', 'Top 15 por Placa Individual', 'portrait')"
-                                title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div class="ftx-grid-tabela cols-3">
-                            <div class="grid-header">
-                                <div class="grid-header-cell">Placa</div>
-                                <div class="grid-header-cell">Tipo</div>
-                                <div class="grid-header-cell text-end">Valor</div>
-                            </div>
-                            <div class="grid-body" id="tabelaValorPorPlaca" style="max-height: 380px;">
-                                <div class="grid-row" style="justify-content: center; padding: 20px;">
-                                    <i class="fa-duotone fa-spinner-third fa-spin"></i>
-                                </div>
-                            </div>
-                        </div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3 mt-2">
-                <div class="col-12">
-                    <div class="chart-container-abast">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-tags"></i>
-                            Consumo por Categoria
-                            <span class="chart-subtitle">Ambulância, Carga Leve, Carga Pesada, Coletivos Pequenos,
-                                Depol, Mesa, Ônibus/Microônibus, Passeio</span>
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('chartConsumoCategoria', 'Consumo por Categoria')"
-                                title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div id="chartConsumoCategoria" style="height: 280px;"></div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3 mt-2">
-                <div class="col-12">
-                    <div class="chart-container-abast">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-grid-2"></i>
-                            Mapa de Calor - Consumo por Categoria
-                            <span class="chart-subtitle">Visualize o consumo por categoria ao longo do ano</span>
-                            <button type="button" class="abast-btn-pdf"
-                                onclick="exportarGraficoPdf('heatmapCategoria', 'Mapa de Calor - Consumo por Categoria')"
-                                title="Exportar PDF">
-                                <i class="fa-duotone fa-file-pdf"></i> PDF
-                            </button>
-                        </h5>
-                        <div class="heatmap-container">
-                            <div id="heatmapCategoria" style="height: 320px;"></div>
-                            <div id="legendaHeatmapCategoria" class="heatmap-legenda-custom"></div>
-                        </div>
-                    </div>
-                </div>
             </div>
         </div>
 
-        <div id="tab-consumo-veiculo" class="dash-content">
-            <div class="filter-card-abast">
-
-                <div class="row align-items-end g-3 mb-3">
-                    <div class="col-auto">
-                        <label class="form-label">
-                            <i class="fa-duotone fa-calendar me-1"></i> Ano
-                        </label>
-                        <select id="filtroAnoVeiculo" class="form-select" style="min-width: 120px;"></select>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">
-                            <i class="fa-duotone fa-calendar-day me-1"></i> Mês
-                        </label>
-                        <select id="filtroMesVeiculo" class="form-select" style="min-width: 150px;">
-                            <option value="">&lt;Todos os Meses&gt;</option>
-                        </select>
-                    </div>
-                    <div class="col-auto filtro-placa-container">
-                        <label class="form-label">
-                            <i class="fa-duotone fa-rectangle-barcode me-1"></i> Placa
-                        </label>
-                        <select id="filtroPlacaVeiculo" class="form-select" style="min-width: 200px;">
-                            <option value="">Todas</option>
-                        </select>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">&nbsp;</label>
-                        <button type="button" class="btn btn-filtrar-abast d-block" id="btnFiltrarAnoMesVeiculo"
-                            data-ejtip="Selecione os Parâmetros para a Pesquisa">
-                            <i class="fa-duotone fa-filter me-1"></i> Filtrar
+        <div class="row g-3 mt-2">
+            <div class="col-12">
+                <div class="chart-container-abast">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-tags"></i>
+                        Consumo por Categoria
+                        <span class="chart-subtitle">Ambulância, Carga Leve, Carga Pesada, Coletivos Pequenos, Depol, Mesa, Ônibus/Microônibus, Passeio</span>
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('chartConsumoCategoria', 'Consumo por Categoria')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
                         </button>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">&nbsp;</label>
-                        <button type="button" class="btn btn-limpar-abast d-block" id="btnLimparAnoMesVeiculo">
-                            <i class="fa-duotone fa-eraser me-1"></i> Limpar
+                    </h5>
+                    <div id="chartConsumoCategoria" style="height: 280px;"></div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3 mt-2">
+            <div class="col-12">
+                <div class="chart-container-abast">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-grid-2"></i>
+                        Mapa de Calor - Consumo por Categoria
+                        <span class="chart-subtitle">Visualize o consumo por categoria ao longo do ano</span>
+                        <button type="button" class="abast-btn-pdf" onclick="exportarGraficoPdf('heatmapCategoria', 'Mapa de Calor - Consumo por Categoria')" title="Exportar PDF">
+                            <i class="fa-duotone fa-file-pdf"></i> PDF
                         </button>
-                    </div>
-                </div>
-
-                <div class="row align-items-end g-3">
-                    <div class="col-auto">
-                        <label class="form-label">
-                            <i class="fa-duotone fa-calendar-range me-1"></i> Período Personalizado
-                        </label>
-                        <div class="d-flex gap-2 align-items-center">
-                            <div>
-                                <label class="form-label small mb-1">De:</label>
-                                <input type="date" id="dataInicioVeiculo" class="form-control" style="width: 140px;" />
-                            </div>
-                            <div>
-                                <label class="form-label small mb-1">Até:</label>
-                                <input type="date" id="dataFimVeiculo" class="form-control" style="width: 140px;" />
-                            </div>
-                        </div>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">&nbsp;</label>
-                        <button type="button" class="btn btn-filtrar-abast d-block" id="btnFiltrarPeriodoVeiculo">
-                            <i class="fa-duotone fa-magnifying-glass me-1"></i> Filtrar
-                        </button>
-                    </div>
-                    <div class="col-auto">
-                        <label class="form-label">&nbsp;</label>
-                        <button type="button" class="btn btn-limpar-abast d-block" id="btnLimparPeriodoVeiculo">
-                            <i class="fa-duotone fa-eraser me-1"></i> Limpar
-                        </button>
-                    </div>
-                    <div class="col">
-                        <label class="form-label">Períodos Rápidos</label>
-                        <div class="d-flex gap-2 flex-wrap">
-                            <button type="button" class="btn-period-abast-veiculo" data-dias="7">7 dias</button>
-                            <button type="button" class="btn-period-abast-veiculo" data-dias="15">15 dias</button>
-                            <button type="button" class="btn-period-abast-veiculo" data-dias="30">30 dias</button>
-                            <button type="button" class="btn-period-abast-veiculo" data-dias="60">60 dias</button>
-                            <button type="button" class="btn-period-abast-veiculo" data-dias="90">90 dias</button>
-                            <button type="button" class="btn-period-abast-veiculo" data-dias="180">180 dias</button>
-                            <button type="button" class="btn-period-abast-veiculo" data-dias="365">1 ano</button>
-                        </div>
-                    </div>
-                </div>
-
-                <div class="row mt-3">
-                    <div class="col-12">
-                        <div class="periodo-atual-abast d-flex align-items-center">
-                            <i class="fa-duotone fa-info-circle me-2"></i>
-                            <span id="periodoAtualLabelVeiculo">Exibindo todos os dados</span>
-                        </div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3">
-                <div class="col-lg-12">
-                    <div class="chart-container-abast mb-3">
-                        <div class="text-center py-2">
-                            <div style="font-size: 0.75rem; color: #64748b;">Veículo Selecionado</div>
-                            <h3 id="descricaoVeiculoSelecionado"
-                                style="font-size: 1.3rem; font-weight: 700; color: var(--abast-dark); margin: 0.4rem 0;">
-                                Selecione um veículo
-                            </h3>
-                            <div style="font-size: 0.75rem; color: #64748b;">Categoria</div>
-                            <h4 id="categoriaVeiculoSelecionado"
-                                style="font-size: 1rem; font-weight: 600; color: var(--abast-primary); margin: 0.2rem 0;">
-                                -</h4>
-                        </div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3">
-                <div class="col-lg-3 col-md-6">
-                    <div class="card-estatistica-abast amber">
-                        <div class="icone-card-abast"><i class="fa-duotone fa-sack-dollar"></i></div>
-                        <div class="texto-metrica-abast">Valor Total</div>
-                        <div class="valor-metrica-abast" id="valorTotalVeiculo">R$ 0</div>
-                    </div>
-                </div>
-                <div class="col-lg-3 col-md-6">
-                    <div class="card-estatistica-abast orange">
-                        <div class="icone-card-abast"><i class="fa-duotone fa-gas-pump"></i></div>
-                        <div class="texto-metrica-abast">Total de Litros</div>
-                        <div class="valor-metrica-abast" id="litrosTotalVeiculo">0</div>
-                    </div>
-                </div>
-                <div class="col-lg-6">
-                    <div class="chart-container-abast" style="min-height: 120px;">
-                        <h5 class="chart-title-abast" style="font-size: 0.8rem;">
-                            <i class="fa-duotone fa-chart-line"></i>
-                            Consumo Mensal de Litros
-                        </h5>
-                        <div id="chartConsumoMensalVeiculo" style="height: 160px;"></div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3 mt-1">
-                <div class="col-lg-6">
-                    <div class="chart-container-abast" style="min-height: 340px;">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-chart-bar"></i>
-                            Valor Total Mensal
-                        </h5>
-                        <div id="chartValorMensalVeiculo" style="height: 280px;"></div>
-                    </div>
-                </div>
-                <div class="col-lg-6">
-                    <div class="chart-container-abast" style="min-height: 340px;">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-ranking-star" id="iconRankingVeiculos"></i>
-                            <span id="tituloRankingVeiculos">Ranking de Veículos (Top 10)</span>
-                            <span class="chart-subtitle" id="subtituloRankingVeiculos">Por placa individual</span>
-                        </h5>
-                        <div id="chartRankingVeiculos" style="height: 280px;"></div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row g-3 mt-1">
-                <div class="col-lg-12">
-                    <div class="chart-container-abast">
-                        <h5 class="chart-title-abast">
-                            <i class="fa-duotone fa-fire-flame-simple"></i>
-                            Mapa de Calor - Dia da Semana x Hora
-                            <span class="chart-subtitle">Padrão de abastecimento do veículo selecionado</span>
-                        </h5>
-                        <div class="heatmap-container">
-                            <div id="heatmapVeiculo" style="height: 380px;"></div>
-                            <div id="legendaHeatmapVeiculo" class="heatmap-legenda-custom"></div>
-                        </div>
-                        <div id="heatmapVeiculoVazio" class="text-center text-muted py-4" style="display: none;">
-                            Selecione um veículo para visualizar o padrão de abastecimento
-                        </div>
+                    </h5>
+                    <div class="heatmap-container">
+                        <div id="heatmapCategoria" style="height: 320px;"></div>
+                        <div id="legendaHeatmapCategoria" class="heatmap-legenda-custom"></div>
                     </div>
                 </div>
             </div>
         </div>
     </div>
+
+    <div id="tab-consumo-veiculo" class="dash-content">
+        <div class="filter-card-abast">
+
+            <div class="row align-items-end g-3 mb-3">
+                <div class="col-auto">
+                    <label class="form-label">
+                        <i class="fa-duotone fa-calendar me-1"></i> Ano
+                    </label>
+                    <select id="filtroAnoVeiculo" class="form-select" style="min-width: 120px;"></select>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">
+                        <i class="fa-duotone fa-calendar-day me-1"></i> Mês
+                    </label>
+                    <select id="filtroMesVeiculo" class="form-select" style="min-width: 150px;">
+                        <option value="">&lt;Todos os Meses&gt;</option>
+                    </select>
+                </div>
+                <div class="col-auto filtro-placa-container">
+                    <label class="form-label">
+                        <i class="fa-duotone fa-rectangle-barcode me-1"></i> Placa
+                    </label>
+                    <select id="filtroPlacaVeiculo" class="form-select" style="min-width: 200px;">
+                        <option value="">Todas</option>
+                    </select>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">&nbsp;</label>
+                    <button type="button" class="btn btn-filtrar-abast d-block" id="btnFiltrarAnoMesVeiculo" data-ejtip="Selecione os Parâmetros para a Pesquisa">
+                        <i class="fa-duotone fa-filter me-1"></i> Filtrar
+                    </button>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">&nbsp;</label>
+                    <button type="button" class="btn btn-limpar-abast d-block" id="btnLimparAnoMesVeiculo">
+                        <i class="fa-duotone fa-eraser me-1"></i> Limpar
+                    </button>
+                </div>
+            </div>
+
+            <div class="row align-items-end g-3">
+                <div class="col-auto">
+                    <label class="form-label">
+                        <i class="fa-duotone fa-calendar-range me-1"></i> Período Personalizado
+                    </label>
+                    <div class="d-flex gap-2 align-items-center">
+                        <div>
+                            <label class="form-label small mb-1">De:</label>
+                            <input type="date" id="dataInicioVeiculo" class="form-control" style="width: 140px;" />
+                        </div>
+                        <div>
+                            <label class="form-label small mb-1">Até:</label>
+                            <input type="date" id="dataFimVeiculo" class="form-control" style="width: 140px;" />
+                        </div>
+                    </div>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">&nbsp;</label>
+                    <button type="button" class="btn btn-filtrar-abast d-block" id="btnFiltrarPeriodoVeiculo">
+                        <i class="fa-duotone fa-magnifying-glass me-1"></i> Filtrar
+                    </button>
+                </div>
+                <div class="col-auto">
+                    <label class="form-label">&nbsp;</label>
+                    <button type="button" class="btn btn-limpar-abast d-block" id="btnLimparPeriodoVeiculo">
+                        <i class="fa-duotone fa-eraser me-1"></i> Limpar
+                    </button>
+                </div>
+                <div class="col">
+                    <label class="form-label">Períodos Rápidos</label>
+                    <div class="d-flex gap-2 flex-wrap">
+                        <button type="button" class="btn-period-abast-veiculo" data-dias="7">7 dias</button>
+                        <button type="button" class="btn-period-abast-veiculo" data-dias="15">15 dias</button>
+                        <button type="button" class="btn-period-abast-veiculo" data-dias="30">30 dias</button>
+                        <button type="button" class="btn-period-abast-veiculo" data-dias="60">60 dias</button>
+                        <button type="button" class="btn-period-abast-veiculo" data-dias="90">90 dias</button>
+                        <button type="button" class="btn-period-abast-veiculo" data-dias="180">180 dias</button>
+                        <button type="button" class="btn-period-abast-veiculo" data-dias="365">1 ano</button>
+                    </div>
+                </div>
+            </div>
+
+            <div class="row mt-3">
+                <div class="col-12">
+                    <div class="periodo-atual-abast d-flex align-items-center">
+                        <i class="fa-duotone fa-info-circle me-2"></i>
+                        <span id="periodoAtualLabelVeiculo">Exibindo todos os dados</span>
+                    </div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3">
+            <div class="col-lg-12">
+                <div class="chart-container-abast mb-3">
+                    <div class="text-center py-2">
+                        <div style="font-size: 0.75rem; color: #64748b;">Veículo Selecionado</div>
+                        <h3 id="descricaoVeiculoSelecionado" style="font-size: 1.3rem; font-weight: 700; color: var(--abast-dark); margin: 0.4rem 0;">
+                            Selecione um veículo
+                        </h3>
+                        <div style="font-size: 0.75rem; color: #64748b;">Categoria</div>
+                        <h4 id="categoriaVeiculoSelecionado" style="font-size: 1rem; font-weight: 600; color: var(--abast-primary); margin: 0.2rem 0;">-</h4>
+                    </div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3">
+            <div class="col-lg-3 col-md-6">
+                <div class="card-estatistica-abast amber">
+                    <div class="icone-card-abast"><i class="fa-duotone fa-sack-dollar"></i></div>
+                    <div class="texto-metrica-abast">Valor Total</div>
+                    <div class="valor-metrica-abast" id="valorTotalVeiculo">R$ 0</div>
+                </div>
+            </div>
+            <div class="col-lg-3 col-md-6">
+                <div class="card-estatistica-abast orange">
+                    <div class="icone-card-abast"><i class="fa-duotone fa-gas-pump"></i></div>
+                    <div class="texto-metrica-abast">Total de Litros</div>
+                    <div class="valor-metrica-abast" id="litrosTotalVeiculo">0</div>
+                </div>
+            </div>
+            <div class="col-lg-6">
+                <div class="chart-container-abast" style="min-height: 120px;">
+                    <h5 class="chart-title-abast" style="font-size: 0.8rem;">
+                        <i class="fa-duotone fa-chart-line"></i>
+                        Consumo Mensal de Litros
+                    </h5>
+                    <div id="chartConsumoMensalVeiculo" style="height: 160px;"></div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3 mt-1">
+            <div class="col-lg-6">
+                <div class="chart-container-abast" style="min-height: 340px;">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-chart-bar"></i>
+                        Valor Total Mensal
+                    </h5>
+                    <div id="chartValorMensalVeiculo" style="height: 280px;"></div>
+                </div>
+            </div>
+            <div class="col-lg-6">
+                <div class="chart-container-abast" style="min-height: 340px;">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-ranking-star" id="iconRankingVeiculos"></i>
+                        <span id="tituloRankingVeiculos">Ranking de Veículos (Top 10)</span>
+                        <span class="chart-subtitle" id="subtituloRankingVeiculos">Por placa individual</span>
+                    </h5>
+                    <div id="chartRankingVeiculos" style="height: 280px;"></div>
+                </div>
+            </div>
+        </div>
+
+        <div class="row g-3 mt-1">
+            <div class="col-lg-12">
+                <div class="chart-container-abast">
+                    <h5 class="chart-title-abast">
+                        <i class="fa-duotone fa-fire-flame-simple"></i>
+                        Mapa de Calor - Dia da Semana x Hora
+                        <span class="chart-subtitle">Padrão de abastecimento do veículo selecionado</span>
+                    </h5>
+                    <div class="heatmap-container">
+                        <div id="heatmapVeiculo" style="height: 380px;"></div>
+                        <div id="legendaHeatmapVeiculo" class="heatmap-legenda-custom"></div>
+                    </div>
+                    <div id="heatmapVeiculoVazio" class="text-center text-muted py-4" style="display: none;">
+                        Selecione um veículo para visualizar o padrão de abastecimento
+                    </div>
+                </div>
+            </div>
+        </div>
+    </div>
+    </div>
 </div>
 
 <div id="loadingOverlayAbast" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text">Carregando Dashboard de Abastecimento</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
@@ -1515,75 +1419,57 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalViagensVeiculo" tabindex="-1" aria-labelledby="modalViagensVeiculoLabel"
-    aria-hidden="true">
+<div class="modal fade" id="modalViagensVeiculo" tabindex="-1" aria-labelledby="modalViagensVeiculoLabel" aria-hidden="true">
     <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
         <div class="modal-content" style="border-radius: 12px; overflow: hidden;">
-            <div class="modal-header"
-                style="background: linear-gradient(135deg, #325d88 0%, #4a7c59 100%); color: white; border: none;">
+            <div class="modal-header" style="background: linear-gradient(135deg, #325d88 0%, #4a7c59 100%); color: white; border: none;">
                 <h5 class="modal-title" id="modalViagensVeiculoLabel">
                     <i class="fa-duotone fa-route me-2"></i>
                     Viagens do Veículo
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body" style="background: #f8f9fa; padding: 0;">
 
-                <div class="viagem-modal-header"
-                    style="background: white; padding: 20px; border-bottom: 1px solid #dee2e6;">
+                <div class="viagem-modal-header" style="background: white; padding: 20px; border-bottom: 1px solid #dee2e6;">
                     <div class="row align-items-center">
                         <div class="col-md-4">
                             <div class="d-flex align-items-center gap-3">
-                                <div class="viagem-placa-badge"
-                                    style="background: #325d88; color: white; padding: 10px 20px; border-radius: 8px; font-size: 1.3rem; font-weight: 700; letter-spacing: 2px;">
+                                <div class="viagem-placa-badge" style="background: #325d88; color: white; padding: 10px 20px; border-radius: 8px; font-size: 1.3rem; font-weight: 700; letter-spacing: 2px;">
                                     <i class="fa-duotone fa-rectangle-barcode me-2"></i>
                                     <span id="modalVeiculoPlaca">---</span>
                                 </div>
                                 <div>
                                     <div style="font-weight: 600; color: #333;" id="modalVeiculoModelo">---</div>
-                                    <div style="font-size: 0.85rem; color: #6c757d;" id="modalVeiculoCategoria">---
-                                    </div>
+                                    <div style="font-size: 0.85rem; color: #6c757d;" id="modalVeiculoCategoria">---</div>
                                 </div>
                             </div>
                         </div>
                         <div class="col-md-8 text-md-end mt-3 mt-md-0">
                             <div class="d-flex justify-content-md-end gap-3 flex-wrap">
                                 <div class="text-center px-2">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Período
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #325d88;" id="modalPeriodo">
-                                        ---</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Período</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #325d88;" id="modalPeriodo">---</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Viagens
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #4a7c59;"
-                                        id="modalTotalViagens">0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Viagens</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #4a7c59;" id="modalTotalViagens">0</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Km
-                                        Rodados</div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #722f37;" id="modalTotalKm">0
-                                    </div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Km Rodados</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #722f37;" id="modalTotalKm">0</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Abastec.
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #f59e0b;"
-                                        id="modalTotalAbastecimentos">0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Abastec.</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #f59e0b;" id="modalTotalAbastecimentos">0</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Litros
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #0ea5e9;"
-                                        id="modalTotalLitros">0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Litros</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #0ea5e9;" id="modalTotalLitros">0</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Valor
-                                        Total</div>
-                                    <div style="font-size: 1rem; font-weight: 700; color: #dc2626;"
-                                        id="modalTotalValor">R$ 0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Valor Total</div>
+                                    <div style="font-size: 1rem; font-weight: 700; color: #dc2626;" id="modalTotalValor">R$ 0</div>
                                 </div>
                             </div>
                         </div>
@@ -1592,15 +1478,13 @@
 
                 <ul class="nav nav-tabs" style="padding: 0 20px; background: white; border-bottom: none;">
                     <li class="nav-item">
-                        <button class="nav-link active" id="tabViagens" data-bs-toggle="tab"
-                            data-bs-target="#painelViagens" type="button">
+                        <button class="nav-link active" id="tabViagens" data-bs-toggle="tab" data-bs-target="#painelViagens" type="button">
                             <i class="fa-duotone fa-route me-1"></i> Viagens
                             <span class="badge bg-success ms-1" id="badgeViagens">0</span>
                         </button>
                     </li>
                     <li class="nav-item">
-                        <button class="nav-link" id="tabAbastecimentos" data-bs-toggle="tab"
-                            data-bs-target="#painelAbastecimentos" type="button">
+                        <button class="nav-link" id="tabAbastecimentos" data-bs-toggle="tab" data-bs-target="#painelAbastecimentos" type="button">
                             <i class="fa-duotone fa-gas-pump me-1"></i> Abastecimentos
                             <span class="badge bg-warning text-dark ms-1" id="badgeAbastecimentos">0</span>
                         </button>
@@ -1610,41 +1494,32 @@
                 <div class="tab-content" style="padding: 20px;">
 
                     <div class="tab-pane fade show active" id="painelViagens">
-                        <div class="table-responsive"
-                            style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
+                        <div class="table-responsive" style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
                             <table class="table table-hover mb-0" id="tabelaViagensModal">
                                 <thead style="background: #f1f3f5; position: sticky; top: 0;">
                                     <tr>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-file-lines me-1"></i> Ficha
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-calendar me-1"></i> Data Inicial
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-clock me-1"></i> Hora
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-calendar me-1"></i> Data Final
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-clock me-1"></i> Hora
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-gauge me-1"></i> Km Ini
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-gauge-max me-1"></i> Km Fin
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-road me-1"></i> Rodado
                                         </th>
                                     </tr>
@@ -1662,33 +1537,26 @@
                     </div>
 
                     <div class="tab-pane fade" id="painelAbastecimentos">
-                        <div class="table-responsive"
-                            style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
+                        <div class="table-responsive" style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
                             <table class="table table-hover mb-0" id="tabelaAbastecimentosModal">
                                 <thead style="background: #fff8e1; position: sticky; top: 0;">
                                     <tr>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-calendar me-1"></i> Data
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-fire-flame me-1"></i> Combustível
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-droplet me-1"></i> Litros
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-dollar-sign me-1"></i> R$/Litro
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-coins me-1"></i> Total
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-road me-1"></i> Km Rodado
                                         </th>
                                     </tr>
@@ -1715,28 +1583,23 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalAbastecimentosUnidade" tabindex="-1" aria-labelledby="modalAbastecimentosUnidadeLabel"
-    aria-hidden="true">
+<div class="modal fade" id="modalAbastecimentosUnidade" tabindex="-1" aria-labelledby="modalAbastecimentosUnidadeLabel" aria-hidden="true">
     <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
         <div class="modal-content" style="border-radius: 12px; overflow: hidden;">
-            <div class="modal-header"
-                style="background: linear-gradient(135deg, #a8784c 0%, #c4956a 100%); color: white; border: none;">
+            <div class="modal-header" style="background: linear-gradient(135deg, #a8784c 0%, #c4956a 100%); color: white; border: none;">
                 <h5 class="modal-title" id="modalAbastecimentosUnidadeLabel">
                     <i class="fa-duotone fa-building me-2"></i>
                     Abastecimentos da Unidade
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body" style="background: #f8f9fa; padding: 0;">
 
-                <div class="unidade-modal-header"
-                    style="background: white; padding: 20px; border-bottom: 1px solid #dee2e6;">
+                <div class="unidade-modal-header" style="background: white; padding: 20px; border-bottom: 1px solid #dee2e6;">
                     <div class="row align-items-center">
                         <div class="col-md-4">
                             <div class="d-flex align-items-center gap-3">
-                                <div class="unidade-badge"
-                                    style="background: #a8784c; color: white; padding: 10px 20px; border-radius: 8px; font-size: 1.3rem; font-weight: 700; letter-spacing: 2px;">
+                                <div class="unidade-badge" style="background: #a8784c; color: white; padding: 10px 20px; border-radius: 8px; font-size: 1.3rem; font-weight: 700; letter-spacing: 2px;">
                                     <i class="fa-duotone fa-building me-2"></i>
                                     <span id="modalUnidadeNome">---</span>
                                 </div>
@@ -1745,28 +1608,20 @@
                         <div class="col-md-8 text-md-end mt-3 mt-md-0">
                             <div class="d-flex justify-content-md-end gap-3 flex-wrap">
                                 <div class="text-center px-2">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Período
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #a8784c;"
-                                        id="modalUnidadePeriodo">---</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Período</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #a8784c;" id="modalUnidadePeriodo">---</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Abastec.
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #f59e0b;"
-                                        id="modalUnidadeTotalAbastecimentos">0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Abastec.</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #f59e0b;" id="modalUnidadeTotalAbastecimentos">0</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Litros
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #0ea5e9;"
-                                        id="modalUnidadeTotalLitros">0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Litros</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #0ea5e9;" id="modalUnidadeTotalLitros">0</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Valor
-                                        Total</div>
-                                    <div style="font-size: 1rem; font-weight: 700; color: #dc2626;"
-                                        id="modalUnidadeTotalValor">R$ 0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Valor Total</div>
+                                    <div style="font-size: 1rem; font-weight: 700; color: #dc2626;" id="modalUnidadeTotalValor">R$ 0</div>
                                 </div>
                             </div>
                         </div>
@@ -1775,15 +1630,13 @@
 
                 <ul class="nav nav-tabs" style="padding: 0 20px; background: white; border-bottom: none;">
                     <li class="nav-item">
-                        <button class="nav-link active" id="tabAbastecimentosUnidade" data-bs-toggle="tab"
-                            data-bs-target="#painelAbastecimentosUnidade" type="button">
+                        <button class="nav-link active" id="tabAbastecimentosUnidade" data-bs-toggle="tab" data-bs-target="#painelAbastecimentosUnidade" type="button">
                             <i class="fa-duotone fa-gas-pump me-1"></i> Abastecimentos
                             <span class="badge bg-warning text-dark ms-1" id="badgeAbastecimentosUnidade">0</span>
                         </button>
                     </li>
                     <li class="nav-item">
-                        <button class="nav-link" id="tabResumoVeiculos" data-bs-toggle="tab"
-                            data-bs-target="#painelResumoVeiculos" type="button">
+                        <button class="nav-link" id="tabResumoVeiculos" data-bs-toggle="tab" data-bs-target="#painelResumoVeiculos" type="button">
                             <i class="fa-duotone fa-cars me-1"></i> Resumo por Veículo
                             <span class="badge bg-info text-dark ms-1" id="badgeResumoVeiculos">0</span>
                         </button>
@@ -1793,37 +1646,29 @@
                 <div class="tab-content" style="padding: 20px;">
 
                     <div class="tab-pane fade show active" id="painelAbastecimentosUnidade">
-                        <div class="table-responsive"
-                            style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
+                        <div class="table-responsive" style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
                             <table class="table table-hover mb-0" id="tabelaAbastecimentosUnidadeModal">
                                 <thead style="background: #fff8e1; position: sticky; top: 0;">
                                     <tr>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-calendar me-1"></i> Data
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-rectangle-barcode me-1"></i> Placa
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-car me-1"></i> Veículo
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-fire-flame me-1"></i> Combustível
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-droplet me-1"></i> Litros
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-dollar-sign me-1"></i> R$/Litro
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-coins me-1"></i> Total
                                         </th>
                                     </tr>
@@ -1841,29 +1686,23 @@
                     </div>
 
                     <div class="tab-pane fade" id="painelResumoVeiculos">
-                        <div class="table-responsive"
-                            style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
+                        <div class="table-responsive" style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
                             <table class="table table-hover mb-0" id="tabelaResumoVeiculosModal">
                                 <thead style="background: #e0f7fa; position: sticky; top: 0;">
                                     <tr>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-rectangle-barcode me-1"></i> Placa
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-car me-1"></i> Tipo Veículo
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-gas-pump me-1"></i> Qtd Abast.
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-droplet me-1"></i> Litros
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-coins me-1"></i> Valor Total
                                         </th>
                                     </tr>
@@ -1890,28 +1729,23 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalAbastecimentosCategoria" tabindex="-1"
-    aria-labelledby="modalAbastecimentosCategoriaLabel" aria-hidden="true">
+<div class="modal fade" id="modalAbastecimentosCategoria" tabindex="-1" aria-labelledby="modalAbastecimentosCategoriaLabel" aria-hidden="true">
     <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
         <div class="modal-content" style="border-radius: 12px; overflow: hidden;">
-            <div class="modal-header"
-                style="background: linear-gradient(135deg, #4a7c59 0%, #6b8e23 100%); color: white; border: none;">
+            <div class="modal-header" style="background: linear-gradient(135deg, #4a7c59 0%, #6b8e23 100%); color: white; border: none;">
                 <h5 class="modal-title" id="modalAbastecimentosCategoriaLabel">
                     <i class="fa-duotone fa-layer-group me-2"></i>
                     Abastecimentos por Categoria
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body" style="background: #f8f9fa; padding: 0;">
 
-                <div class="categoria-modal-header"
-                    style="background: white; padding: 20px; border-bottom: 1px solid #dee2e6;">
+                <div class="categoria-modal-header" style="background: white; padding: 20px; border-bottom: 1px solid #dee2e6;">
                     <div class="row align-items-center">
                         <div class="col-md-4">
                             <div class="d-flex align-items-center gap-3">
-                                <div class="categoria-badge"
-                                    style="background: #4a7c59; color: white; padding: 10px 20px; border-radius: 8px; font-size: 1.1rem; font-weight: 700;">
+                                <div class="categoria-badge" style="background: #4a7c59; color: white; padding: 10px 20px; border-radius: 8px; font-size: 1.1rem; font-weight: 700;">
                                     <i class="fa-duotone fa-layer-group me-2"></i>
                                     <span id="modalCategoriaNome">---</span>
                                 </div>
@@ -1920,34 +1754,24 @@
                         <div class="col-md-8 text-md-end mt-3 mt-md-0">
                             <div class="d-flex justify-content-md-end gap-3 flex-wrap">
                                 <div class="text-center px-2">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Período
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #4a7c59;"
-                                        id="modalCategoriaPeriodo">---</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Período</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #4a7c59;" id="modalCategoriaPeriodo">---</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Veículos
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #325d88;"
-                                        id="modalCategoriaTotalVeiculos">0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Veículos</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #325d88;" id="modalCategoriaTotalVeiculos">0</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Abastec.
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #f59e0b;"
-                                        id="modalCategoriaTotalAbastecimentos">0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Abastec.</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #f59e0b;" id="modalCategoriaTotalAbastecimentos">0</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Litros
-                                    </div>
-                                    <div style="font-size: 1rem; font-weight: 600; color: #0ea5e9;"
-                                        id="modalCategoriaTotalLitros">0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Litros</div>
+                                    <div style="font-size: 1rem; font-weight: 600; color: #0ea5e9;" id="modalCategoriaTotalLitros">0</div>
                                 </div>
                                 <div class="text-center px-2" style="border-left: 1px solid #dee2e6;">
-                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Valor
-                                        Total</div>
-                                    <div style="font-size: 1rem; font-weight: 700; color: #dc2626;"
-                                        id="modalCategoriaTotalValor">R$ 0</div>
+                                    <div style="font-size: 0.7rem; color: #6c757d; text-transform: uppercase;">Valor Total</div>
+                                    <div style="font-size: 1rem; font-weight: 700; color: #dc2626;" id="modalCategoriaTotalValor">R$ 0</div>
                                 </div>
                             </div>
                         </div>
@@ -1956,15 +1780,13 @@
 
                 <ul class="nav nav-tabs" style="padding: 0 20px; background: white; border-bottom: none;">
                     <li class="nav-item">
-                        <button class="nav-link active" id="tabVeiculosCategoria" data-bs-toggle="tab"
-                            data-bs-target="#painelVeiculosCategoria" type="button">
+                        <button class="nav-link active" id="tabVeiculosCategoria" data-bs-toggle="tab" data-bs-target="#painelVeiculosCategoria" type="button">
                             <i class="fa-duotone fa-cars me-1"></i> Veículos da Categoria
                             <span class="badge bg-info text-dark ms-1" id="badgeVeiculosCategoria">0</span>
                         </button>
                     </li>
                     <li class="nav-item">
-                        <button class="nav-link" id="tabAbastecimentosCategoria" data-bs-toggle="tab"
-                            data-bs-target="#painelAbastecimentosCategoria" type="button">
+                        <button class="nav-link" id="tabAbastecimentosCategoria" data-bs-toggle="tab" data-bs-target="#painelAbastecimentosCategoria" type="button">
                             <i class="fa-duotone fa-gas-pump me-1"></i> Todos Abastecimentos
                             <span class="badge bg-warning text-dark ms-1" id="badgeAbastecimentosCategoria">0</span>
                         </button>
@@ -1974,29 +1796,23 @@
                 <div class="tab-content" style="padding: 20px;">
 
                     <div class="tab-pane fade show active" id="painelVeiculosCategoria">
-                        <div class="table-responsive"
-                            style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
+                        <div class="table-responsive" style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
                             <table class="table table-hover mb-0" id="tabelaVeiculosCategoriaModal">
                                 <thead style="background: #e8f5e9; position: sticky; top: 0;">
                                     <tr>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-rectangle-barcode me-1"></i> Placa
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-car me-1"></i> Tipo Veículo
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-gas-pump me-1"></i> Qtd Abast.
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-droplet me-1"></i> Litros
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-coins me-1"></i> Valor Total
                                         </th>
                                     </tr>
@@ -2014,37 +1830,29 @@
                     </div>
 
                     <div class="tab-pane fade" id="painelAbastecimentosCategoria">
-                        <div class="table-responsive"
-                            style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
+                        <div class="table-responsive" style="background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); max-height: 400px;">
                             <table class="table table-hover mb-0" id="tabelaAbastecimentosCategoriaModal">
                                 <thead style="background: #fff8e1; position: sticky; top: 0;">
                                     <tr>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-calendar me-1"></i> Data
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-rectangle-barcode me-1"></i> Placa
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-car me-1"></i> Veículo
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; white-space: nowrap;">
                                             <i class="fa-duotone fa-fire-flame me-1"></i> Combustível
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-droplet me-1"></i> Litros
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-dollar-sign me-1"></i> R$/Litro
                                         </th>
-                                        <th
-                                            style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
+                                        <th style="font-weight: 600; color: #495057; padding: 12px 15px; text-align: right; white-space: nowrap;">
                                             <i class="fa-duotone fa-coins me-1"></i> Total
                                         </th>
                                     </tr>
@@ -2076,22 +1884,14 @@
     <script src="https://cdn.jsdelivr.net/npm/html2canvas@1.4.1/dist/html2canvas.min.js"></script>
     <script src="https://cdn.jsdelivr.net/npm/jspdf@2.5.1/dist/jspdf.umd.min.js"></script>
 
-    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-base/dist/global/ej2-base.min.js"
-        asp-append-version="true"></script>
-    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-data/dist/global/ej2-data.min.js"
-        asp-append-version="true"></script>
-    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-svg-base/dist/global/ej2-svg-base.min.js"
-        asp-append-version="true"></script>
-    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-pdf-export/dist/global/ej2-pdf-export.min.js"
-        asp-append-version="true"></script>
-    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-file-utils/dist/global/ej2-file-utils.min.js"
-        asp-append-version="true"></script>
-    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-compression/dist/global/ej2-compression.min.js"
-        asp-append-version="true"></script>
-    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-charts/dist/global/ej2-charts.min.js"
-        asp-append-version="true"></script>
-    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-heatmap/dist/global/ej2-heatmap.min.js"
-        asp-append-version="true"></script>
+    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-base/dist/global/ej2-base.min.js" asp-append-version="true"></script>
+    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-data/dist/global/ej2-data.min.js" asp-append-version="true"></script>
+    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-svg-base/dist/global/ej2-svg-base.min.js" asp-append-version="true"></script>
+    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-pdf-export/dist/global/ej2-pdf-export.min.js" asp-append-version="true"></script>
+    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-file-utils/dist/global/ej2-file-utils.min.js" asp-append-version="true"></script>
+    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-compression/dist/global/ej2-compression.min.js" asp-append-version="true"></script>
+    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-charts/dist/global/ej2-charts.min.js" asp-append-version="true"></script>
+    <script src="https://cdn.syncfusion.com/ej2/23.1.36/ej2-heatmap/dist/global/ej2-heatmap.min.js" asp-append-version="true"></script>
     <script src="~/js/dashboards/dashboard-abastecimento.js" asp-append-version="true"></script>
 
     <script>
@@ -2162,15 +1962,6 @@
             }
         };
 
-        /**
-         * Renderiza ícone FontAwesome Duotone em canvas para uso em exportação PDF
-         * @@description Cria canvas de alta resolução, desenha camadas primary/secondary com opacidade e retorna dataURL
-         * @@param {Object} icon - Objeto contendo paths SVG {primary, secondary}
-         * @@param {number} tamanho - Tamanho em pixels do ícone
-         * @@param {number[]} corPrimaria - Cor RGB [r, g, b] da camada primária
-         * @@param {number[]} corSecundaria - Cor RGB [r, g, b] da camada secundária (40% opacidade)
-         * @@returns {Promise<string|null>} DataURL da imagem PNG ou null em caso de erro
-         */
         async function renderizarIconeDuotone(icon, tamanho, corPrimaria, corSecundaria) {
             return new Promise((resolve) => {
                 try {
@@ -2199,14 +1990,6 @@
             });
         }
 
-        /**
-         * Exporta gráfico do dashboard para PDF profissional com layout customizado
-         * @@description Captura elemento via html2canvas, gera PDF com jsPDF incluindo header com logo, barra de ícones, marca d'água e rodapé institucional
-         * @@param {string} elementId - ID do elemento HTML a ser exportado
-         * @@param {string} titulo - Título do gráfico exibido no subtítulo do PDF
-         * @@param {string|null} [forcarOrientacao=null] - Orientação forçada: 'portrait', 'landscape' ou null (auto)
-         * @@returns {Promise<void>}
-         */
         async function exportarGraficoPdf(elementId, titulo, forcarOrientacao = null) {
             try {
                 AppToast.show('orange', 'Gerando PDF profissional com ícones Duotone...', 5000);
@@ -2223,21 +2006,15 @@
 
                 const elementosExpandidos = [];
 
-                /**
-                 * Expande elemento e seus pais que tenham scroll para captura completa
-                 * @@description Remove limitações de altura/overflow temporariamente, salvando estado para restauração
-                 * @@param {HTMLElement} el - Elemento a ser expandido
-                 * @@returns {void}
-                 */
                 function expandirElemento(el) {
                     if (!el || el === document.body || el === document.documentElement) return;
 
                     const computedStyle = window.getComputedStyle(el);
                     const hasScroll = computedStyle.overflow === 'auto' ||
-                        computedStyle.overflow === 'scroll' ||
-                        computedStyle.overflowY === 'auto' ||
-                        computedStyle.overflowY === 'scroll' ||
-                        el.scrollHeight > el.clientHeight;
+                                      computedStyle.overflow === 'scroll' ||
+                                      computedStyle.overflowY === 'auto' ||
+                                      computedStyle.overflowY === 'scroll' ||
+                                      el.scrollHeight > el.clientHeight;
 
                     if (hasScroll || el.style.maxHeight || el.style.height) {
                         elementosExpandidos.push({
@@ -2317,15 +2094,6 @@
                 const pageHeight = pdf.internal.pageSize.getHeight();
                 const margin = 10;
 
-                /**
-                 * Renderiza texto com fonte Outfit em canvas para inserção em PDF
-                 * @@description Cria canvas temporário, renderiza texto com fonte customizada e retorna dataURL
-                 * @@param {string} texto - Texto a ser renderizado
-                 * @@param {number} tamanhoFonte - Tamanho da fonte em pixels
-                 * @@param {number[]} cor - Cor RGB [r, g, b]
-                 * @@param {boolean} [negrito=true] - Se true usa weight 800, senão 600
-                 * @@returns {Promise<{dataUrl: string, width: number, height: number}>} Objeto com dataURL e dimensões
-                 */
                 async function renderizarTextoOutfit(texto, tamanhoFonte, cor, negrito = true) {
                     return new Promise((resolve) => {
                         const tempCanvas = document.createElement('canvas');
@@ -2413,16 +2181,16 @@
                 pdf.setTextColor(...PDF_CORES.cream);
                 pdf.setFontSize(7);
                 pdf.setFont('helvetica', 'bold');
-                pdf.text('GERADO EM', badgeX + badgeWidth / 2, badgeY + 8, { align: 'center' });
+                pdf.text('GERADO EM', badgeX + badgeWidth/2, badgeY + 8, { align: 'center' });
 
                 pdf.setTextColor(...PDF_CORES.white);
                 pdf.setFontSize(11);
                 pdf.setFont('helvetica', 'bold');
-                pdf.text(dataFormatada, badgeX + badgeWidth / 2, badgeY + 18, { align: 'center' });
+                pdf.text(dataFormatada, badgeX + badgeWidth/2, badgeY + 18, { align: 'center' });
 
                 pdf.setFontSize(10);
                 pdf.setFont('helvetica', 'bold');
-                pdf.text(horaFormatada, badgeX + badgeWidth / 2, badgeY + 27, { align: 'center' });
+                pdf.text(horaFormatada, badgeX + badgeWidth/2, badgeY + 27, { align: 'center' });
 
                 const infoBarY = margin + headerHeight + 4;
                 const infoBarHeight = 12;
@@ -2435,7 +2203,7 @@
                 const iconSpacing = (pageWidth - (margin * 2) - 20) / (iconsBar.length + 1);
                 iconsBar.forEach((icon, index) => {
                     if (icon) {
-                        pdf.addImage(icon, 'PNG', margin + 10 + (iconSpacing * (index + 1)) - (iconSize / 2), infoBarY + (infoBarHeight - iconSize) / 2, iconSize, iconSize);
+                        pdf.addImage(icon, 'PNG', margin + 10 + (iconSpacing * (index + 1)) - (iconSize/2), infoBarY + (infoBarHeight - iconSize)/2, iconSize, iconSize);
                     }
                 });
 
@@ -2513,8 +2281,8 @@
                 pdf.setGState(new pdf.GState({ opacity: 1 }));
 
                 const nomeArquivo = 'FrotiX_Abastecimento_' +
-                    titulo.replace(/[^a-zA-Z0-9áéíóúãõâêîôûàèìòùçÁÉÍÓÚÃÕÂÊÎÔÛÀÈÌÒÙÇ ]/g, '').replace(/ /g, '_') + '_' +
-                    dataGeracao.toISOString().slice(0, 10) + '.pdf';
+                                   titulo.replace(/[^a-zA-Z0-9áéíóúãõâêîôûàèìòùçÁÉÍÓÚÃÕÂÊÎÔÛÀÈÌÒÙÇ ]/g, '').replace(/ /g, '_') + '_' +
+                                   dataGeracao.toISOString().slice(0,10) + '.pdf';
                 pdf.save(nomeArquivo);
 
                 AppToast.show('green', 'PDF profissional gerado com sucesso!', 4000);
```
