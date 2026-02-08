"""
aplicar_mudancas.py - Aplica mudancas do FrotiX.Site ao FrotiX.Site.2026.01 em lotes
com build automatico e rollback via git.

Uso: python aplicar_mudancas.py
"""
import os, sys, shutil, subprocess, time, datetime

# ============================================================
# CONFIGURACAO
# ============================================================
JANEIRO  = 'FrotiX.Site.2026.01'
ATUAL    = 'FrotiX.Site'
DIFFS    = 'FrotiX.Atualizado'
LOG_FILE = 'aplicar_mudancas.log'

# Arquivos que NUNCA devem ser copiados (risco de licenciamento)
NEVER_COPY = {
    'FrotiX.csproj',
    'FrotiX.sln',
    'Startup.cs',
    'Program.cs',
    'appsettings.json',
    'appsettings.Development.json',
    'appsettings.Production.json',
    'web.config',
    'package.json',
    'package-lock.json',
    'libman.json',
}

# Diretorios que nunca devem ser tocados
NEVER_DIRS = {'bin', 'obj', 'node_modules', '.git', '.vs', '.claude', '.cursor', '.gemini'}

# Arquivos com referencias Kendo/Telerik/Syncfusion - APLICAR MANUALMENTE
# (85 arquivos identificados por grep nos MDs do FrotiX.Atualizado)
TELERIK_FILES = {
    'Controllers/DashboardViagensController.cs',
    'Controllers/DashboardViagensController_ExportacaoPDF.cs',
    'Controllers/GlosaController.cs',
    'Controllers/ReportsController.cs',
    'Pages/Abastecimento/DashboardAbastecimento.cshtml',
    'Pages/Abastecimento/Index.cshtml',
    'Pages/Abastecimento/PBI.cshtml',
    'Pages/Abastecimento/RegistraCupons.cshtml',
    'Pages/Abastecimento/UpsertCupons.cshtml',
    'Pages/Administracao/AjustaCustosViagem.cshtml',
    'Pages/Administracao/GestaoRecursosNavegacao.cshtml',
    'Pages/Agenda/Index.cshtml',
    'Pages/AlertasFrotiX/Upsert.cshtml',
    'Pages/AtaRegistroPrecos/Index.cshtml',
    'Pages/AtaRegistroPrecos/Upsert.cshtml',
    'Pages/Contrato/Upsert.cshtml',
    'Pages/Manutencao/ControleLavagem.cshtml',
    'Pages/Manutencao/Glosas.cshtml',
    'Pages/Manutencao/Upsert.cshtml',
    'Pages/Motorista/UploadCNH.cshtml',
    'Pages/MovimentacaoPatrimonio/Upsert.cshtml',
    'Pages/Multa/ExibePDFAutuacao.cshtml',
    'Pages/Multa/ExibePDFComprovante.cshtml',
    'Pages/Multa/ExibePDFPenalidade.cshtml',
    'Pages/Multa/ListaAutuacao.cshtml',
    'Pages/Multa/ListaEmpenhosMulta.cshtml',
    'Pages/Multa/ListaPenalidade.cshtml',
    'Pages/Multa/UploadPDF.cshtml',
    'Pages/Multa/UpsertAutuacao.cshtml',
    'Pages/Multa/UpsertTipoMulta.cshtml',
    'Pages/Relatorio/TesteRelatorio.cshtml',
    'Pages/Requisitante/Upsert.cshtml',
    'Pages/SetorSolicitante/Index.cshtml',
    'Pages/SetorSolicitante/Upsert.cshtml',
    'Pages/Shared/_Head.cshtml',
    'Pages/Shared/_Layout.cshtml',
    'Pages/Shared/_ScriptsBasePlugins.cshtml',
    'Pages/Shared/Components/Navigation/TreeView.cshtml',
    'Pages/TaxiLeg/Importacao.cshtml',
    'Pages/Temp/Index.cshtml',
    'Pages/Unidade/LotacaoMotoristas.cshtml',
    'Pages/Unidade/VisualizaLotacoes.cshtml',
    'Pages/Uploads/UploadPDF.cshtml',
    'Pages/Viagens/FluxoPassageiros.cshtml',
    'Pages/Viagens/GestaoFluxo.cshtml',
    'Pages/Viagens/Index.cshtml',
    'Pages/Viagens/ListaEventos.cshtml',
    'Pages/Viagens/TaxiLeg.cshtml',
    'Pages/Viagens/TestGrid.cshtml',
    'Pages/Viagens/Upsert.cshtml',
    'Pages/Viagens/UpsertEvento.cshtml',
    'Pages/_ViewImports.cshtml',
    'Services/CustomReportSourceResolver.cs',
    'Startup.cs',
    'wwwroot/js/agendamento/components/dialogs.js',
    'wwwroot/js/agendamento/components/event-handlers.js',
    'wwwroot/js/agendamento/components/evento.js',
    'wwwroot/js/agendamento/components/exibe-viagem.js',
    'wwwroot/js/agendamento/components/modal-viagem-novo.js',
    'wwwroot/js/agendamento/components/recorrencia.js',
    'wwwroot/js/agendamento/components/recorrencia-init.js',
    'wwwroot/js/agendamento/components/relatorio.js',
    'wwwroot/js/agendamento/components/reportviewer-close-guard.js',
    'wwwroot/js/agendamento/components/validacao.js',
    'wwwroot/js/agendamento/main.js',
    'wwwroot/js/agendamento/utils/kendo-datetime.js',
    'wwwroot/js/agendamento/utils/kendo-editor-helper.js',
    'wwwroot/js/agendamento/utils/syncfusion.utils.js',
    'wwwroot/js/alertasfrotix/alertas_gestao.js',
    'wwwroot/js/alertasfrotix/alertas_recorrencia.js',
    'wwwroot/js/cadastros/EditarEscala.js',
    'wwwroot/js/cadastros/ListaEscala.js',
    'wwwroot/js/cadastros/ViagemIndex.js',
    'wwwroot/js/cadastros/ViagemUpsert.js',
    'wwwroot/js/cadastros/agendamento_viagem.js',
    'wwwroot/js/cadastros/ata.js',
    'wwwroot/js/cadastros/insereviagem_001.js',
    'wwwroot/js/cadastros/manutencao.js',
    'wwwroot/js/cadastros/multa.js',
    'wwwroot/js/dashboards/dashboard-eventos.js',
    'wwwroot/js/dashboards/dashboard-viagens.js',
    'wwwroot/js/kendo-error-suppressor.js',
    'wwwroot/js/localization-init.js',
    'wwwroot/js/syncfusion_tooltips.js',
}

# ============================================================
# LOTES PRE-DEFINIDOS
# ============================================================
LOTES = [
    {
        'id': 0,
        'nome': 'Checkpoint inicial',
        'descricao': 'Commita os arquivos pendentes no Janeiro como ponto de restauracao',
        'risco': '-',
        'diretorios': [],
        'especial': 'checkpoint'
    },
    {
        'id': 1,
        'nome': 'Arquivos novos',
        'descricao': 'Copia 26 arquivos que nao existem no Janeiro',
        'risco': 'BAIXO',
        'diretorios': [],
        'especial': 'novos'
    },
    {
        'id': 2,
        'nome': 'Arquivos deletados',
        'descricao': 'Remove 14 arquivos que foram excluidos na versao atual',
        'risco': 'BAIXO',
        'diretorios': [],
        'especial': 'deletados'
    },
    {
        'id': 3,
        'nome': 'Models (todos)',
        'descricao': 'POCOs e ViewModels - sem logica complexa',
        'risco': 'BAIXO',
        'diretorios': ['Models/'],
    },
    {
        'id': 4,
        'nome': 'Repository/IRepository (interfaces)',
        'descricao': 'Interfaces do repositorio - base para implementacoes',
        'risco': 'BAIXO-MEDIO',
        'diretorios': ['Repository/IRepository/'],
    },
    {
        'id': 5,
        'nome': 'Repository (implementacoes)',
        'descricao': 'Implementacoes dos repositorios',
        'risco': 'BAIXO-MEDIO',
        'diretorios': ['Repository/'],
        'excluir': ['Repository/IRepository/'],
    },
    {
        'id': 6,
        'nome': 'Services',
        'descricao': 'Servicos - inclui ILogService (mudanca critica)',
        'risco': 'MEDIO',
        'diretorios': ['Services/'],
    },
    {
        'id': 7,
        'nome': 'Infraestrutura (Cache, Data, Helpers, Extensions, Middlewares, Hubs, Infrastructure)',
        'descricao': 'Camada de infraestrutura e helpers',
        'risco': 'MEDIO',
        'diretorios': ['Cache/', 'Data/', 'Helpers/', 'Extensions/', 'Middlewares/', 'Hubs/', 'Infrastructure/', 'EndPoints/', 'ViewComponents/', 'Settings/'],
    },
    {
        'id': 8,
        'nome': 'Controllers',
        'descricao': '85 controllers - depende de Services (ILogService removido)',
        'risco': 'MEDIO-ALTO',
        'diretorios': ['Controllers/'],
    },
    {
        'id': 9,
        'nome': 'Areas (Identity + Authorization)',
        'descricao': 'Pages de autenticacao e autorizacao',
        'risco': 'MEDIO',
        'diretorios': ['Areas/'],
    },
    {
        'id': 10,
        'nome': 'Pages (todas)',
        'descricao': 'Razor Pages - UI complexa (~120 arquivos)',
        'risco': 'ALTO',
        'diretorios': ['Pages/'],
    },
    {
        'id': 11,
        'nome': 'JavaScript (wwwroot/js)',
        'descricao': '114 arquivos JS - maior volume de mudancas',
        'risco': 'ALTO',
        'diretorios': ['wwwroot/js/'],
    },
    {
        'id': 12,
        'nome': 'CSS (wwwroot/css)',
        'descricao': 'Estilos CSS',
        'risco': 'BAIXO',
        'diretorios': ['wwwroot/css/'],
    },
    {
        'id': 13,
        'nome': 'SQL Scripts',
        'descricao': 'Scripts SQL novos e removidos',
        'risco': 'BAIXO',
        'diretorios': ['SQL/', 'Scripts/'],
    },
]

# ============================================================
# FUNCOES AUXILIARES
# ============================================================
def log(msg):
    timestamp = datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    line = f"[{timestamp}] {msg}"
    print(line)
    with open(LOG_FILE, 'a', encoding='utf-8') as f:
        f.write(line + '\n')

def run_git(args, cwd=None):
    """Roda git com args no diretorio Janeiro."""
    if cwd is None:
        cwd = JANEIRO
    result = subprocess.run(
        ['git'] + args,
        cwd=cwd, capture_output=True, text=True, encoding='utf-8', errors='replace'
    )
    return result

def git_checkpoint(msg):
    """Cria commit checkpoint no Janeiro."""
    run_git(['add', '-A'])
    result = run_git(['commit', '-m', msg, '--allow-empty'])
    if result.returncode == 0:
        log(f"  Checkpoint criado: {msg}")
    else:
        log(f"  Checkpoint (nada para commitar): {msg}")
    return result

def git_rollback():
    """Reverte todas as mudancas nao-commitadas."""
    run_git(['checkout', '.'])
    # Remove arquivos novos nao-rastreados
    run_git(['clean', '-fd'])
    log("  Rollback executado (git checkout . && git clean -fd)")

def dotnet_build():
    """Roda dotnet build no Janeiro. Retorna (sucesso, output)."""
    log("  Rodando dotnet build...")
    result = subprocess.run(
        ['dotnet', 'build', '--no-restore', '-v', 'q'],
        cwd=JANEIRO, capture_output=True, text=True,
        encoding='utf-8', errors='replace', timeout=300
    )
    sucesso = result.returncode == 0
    output = result.stdout + result.stderr
    # Contar erros e warnings
    erros = [l for l in output.split('\n') if ': error ' in l]
    warnings = [l for l in output.split('\n') if ': warning ' in l]
    if sucesso:
        log(f"  BUILD OK ({len(warnings)} warnings)")
    else:
        log(f"  BUILD FALHOU ({len(erros)} erros, {len(warnings)} warnings)")
        if erros:
            log("  Primeiros erros:")
            for e in erros[:10]:
                log(f"    {e.strip()}")
    return sucesso, output, erros

def scan_diffs():
    """Escaneia FrotiX.Atualizado e retorna lista de (arquivo, tipo)."""
    files = []
    for root, dirs, fnames in os.walk(DIFFS):
        for fn in fnames:
            if fn.endswith('.md') and fn != 'INDICE.md':
                rel = os.path.relpath(os.path.join(root, fn), DIFFS).replace('\\', '/')
                with open(os.path.join(root, fn), 'r', encoding='utf-8') as f:
                    head = f.read(500)
                if 'ARQUIVO NOVO' in head:
                    tipo = 'NOVO'
                elif 'ARQUIVO REMOVIDO' in head:
                    tipo = 'REMOVIDO'
                else:
                    tipo = 'MODIFICADO'
                actual = rel[:-3]  # remove .md
                files.append((actual, tipo))
    return files

def is_protected(filepath):
    """Verifica se arquivo esta na lista de protecao."""
    norm = filepath.replace('\\', '/')
    basename = os.path.basename(norm)
    if basename in NEVER_COPY:
        return True
    parts = norm.split('/')
    if any(d in NEVER_DIRS for d in parts):
        return True
    if norm in TELERIK_FILES:
        return True
    return False

def is_telerik(filepath):
    """Verifica se arquivo esta na lista Telerik (para contagem no menu)."""
    return filepath.replace('\\', '/') in TELERIK_FILES

def get_lote_files(lote, all_files):
    """Retorna arquivos que pertencem a este lote."""
    if lote.get('especial') == 'checkpoint':
        return []
    if lote.get('especial') == 'novos':
        return [(f, t) for f, t in all_files if t == 'NOVO' and not is_protected(f)]
    if lote.get('especial') == 'deletados':
        return [(f, t) for f, t in all_files if t == 'REMOVIDO' and not is_protected(f)]

    diretorios = lote.get('diretorios', [])
    excluir = lote.get('excluir', [])
    result = []
    for f, t in all_files:
        if is_protected(f):
            continue
        # Verificar se pertence a algum diretorio do lote
        matches = any(f.startswith(d) for d in diretorios)
        excluded = any(f.startswith(d) for d in excluir)
        if matches and not excluded:
            result.append((f, t))
    return result

def aplicar_arquivo(filepath, tipo):
    """Aplica uma mudanca individual."""
    src = os.path.join(ATUAL, filepath)
    dst = os.path.join(JANEIRO, filepath)

    if tipo in ('NOVO', 'MODIFICADO'):
        if not os.path.exists(src):
            log(f"    SKIP (nao encontrado no Atual): {filepath}")
            return False
        os.makedirs(os.path.dirname(dst), exist_ok=True)
        shutil.copy2(src, dst)
        return True
    elif tipo == 'REMOVIDO':
        if os.path.exists(dst):
            os.remove(dst)
            return True
        else:
            log(f"    SKIP (ja removido): {filepath}")
            return False
    return False

# ============================================================
# MENU INTERATIVO
# ============================================================
def mostrar_menu(all_files):
    """Mostra menu de lotes."""
    print("\n" + "=" * 70)
    print("  APLICAR MUDANCAS - FrotiX.Site -> FrotiX.Site.2026.01")
    print("=" * 70)
    print()

    for lote in LOTES:
        files = get_lote_files(lote, all_files)
        count = len(files)
        if lote.get('especial') == 'checkpoint':
            count_str = "---"
            telerik_str = ""
        else:
            novos = sum(1 for _, t in files if t == 'NOVO')
            mods = sum(1 for _, t in files if t == 'MODIFICADO')
            dels = sum(1 for _, t in files if t == 'REMOVIDO')
            parts = []
            if mods: parts.append(f"{mods} mod")
            if novos: parts.append(f"{novos} new")
            if dels: parts.append(f"{dels} del")
            count_str = f"{count} arq ({', '.join(parts)})" if parts else "0 arq"
            # Contar quantos Telerik foram excluidos deste lote
            diretorios = lote.get('diretorios', [])
            excluir = lote.get('excluir', [])
            telerik_count = 0
            for f, t in all_files:
                norm = f.replace('\\', '/')
                if norm in TELERIK_FILES:
                    if lote.get('especial') == 'novos' and t == 'NOVO':
                        telerik_count += 1
                    elif lote.get('especial') == 'deletados' and t == 'REMOVIDO':
                        telerik_count += 1
                    elif diretorios:
                        matches = any(f.startswith(d) for d in diretorios)
                        excluded = any(f.startswith(d) for d in excluir)
                        if matches and not excluded:
                            telerik_count += 1
            telerik_str = f" ({telerik_count} telerik excl.)" if telerik_count > 0 else ""

        risco = lote['risco']
        print(f"  [{lote['id']:>2}] {lote['nome']:<45} {count_str:<25} Risco: {risco}{telerik_str}")

    print()
    print(f"  [ C] Lote CUSTOMIZADO (digitar diretorio)")
    print(f"  [ L] Listar arquivos de um lote")
    print(f"  [ S] Status (ver historico git)")
    print(f"  [ R] Rollback ultimo lote")
    print(f"  [ Q] Sair")
    print()

def listar_arquivos_lote(lote_id, all_files):
    """Lista arquivos de um lote especifico."""
    if lote_id < 0 or lote_id >= len(LOTES):
        print("  Lote invalido.")
        return
    lote = LOTES[lote_id]
    files = get_lote_files(lote, all_files)
    print(f"\n  Lote {lote_id}: {lote['nome']} ({len(files)} arquivos)")
    print(f"  {'-' * 60}")
    for f, t in sorted(files):
        marker = {'NOVO': '[NEW]', 'REMOVIDO': '[DEL]', 'MODIFICADO': '[MOD]'}[t]
        print(f"    {marker} {f}")

def aplicar_lote_interativo(lote, all_files):
    """Aplica um lote com confirmacao, build e rollback."""
    files = get_lote_files(lote, all_files)

    if not files and lote.get('especial') != 'checkpoint':
        log(f"Lote '{lote['nome']}': nenhum arquivo para aplicar.")
        return

    # Checkpoint especial
    if lote.get('especial') == 'checkpoint':
        log(f"=== Lote 0: Checkpoint inicial ===")
        git_checkpoint("checkpoint: estado original antes de aplicar mudancas")
        # Tambem fazer dotnet restore
        log("  Rodando dotnet restore...")
        subprocess.run(
            ['dotnet', 'restore'],
            cwd=JANEIRO, capture_output=True, text=True,
            encoding='utf-8', errors='replace', timeout=300
        )
        return

    novos = sum(1 for _, t in files if t == 'NOVO')
    mods = sum(1 for _, t in files if t == 'MODIFICADO')
    dels = sum(1 for _, t in files if t == 'REMOVIDO')

    print(f"\n  Lote: {lote['nome']}")
    print(f"  Risco: {lote['risco']}")
    print(f"  Arquivos: {len(files)} ({mods} mod, {novos} new, {dels} del)")
    print(f"  {lote['descricao']}")

    # Verificar protegidos
    protegidos = [f for f, _ in files if is_protected(f)]
    if protegidos:
        print(f"\n  PROTEGIDOS (serao ignorados): {len(protegidos)}")
        for p in protegidos:
            print(f"    {p}")

    resp = input(f"\n  Aplicar? (s/n): ").strip().lower()
    if resp != 's':
        print("  Cancelado.")
        return

    log(f"\n=== Aplicando lote: {lote['nome']} ({len(files)} arquivos) ===")

    # 1. Checkpoint git
    git_checkpoint(f"checkpoint: antes de '{lote['nome']}'")

    # 2. Aplicar arquivos
    aplicados = 0
    erros_copia = 0
    start = time.time()
    for filepath, tipo in files:
        try:
            if aplicar_arquivo(filepath, tipo):
                aplicados += 1
        except Exception as e:
            log(f"    ERRO ao aplicar {filepath}: {e}")
            erros_copia += 1

    elapsed = time.time() - start
    log(f"  Copiados: {aplicados} | Erros: {erros_copia} | Tempo: {elapsed:.1f}s")

    # 3. Build
    sucesso, output, build_erros = dotnet_build()

    # 4. Resultado
    if sucesso:
        log(f"  BUILD OK! Commitando lote...")
        git_checkpoint(f"aplicado: {lote['nome']} ({aplicados} arquivos)")
        print(f"\n  >>> LOTE APLICADO COM SUCESSO! <<<")
    else:
        print(f"\n  >>> BUILD FALHOU! ({len(build_erros)} erros) <<<")
        print(f"  Primeiros erros:")
        for e in build_erros[:10]:
            print(f"    {e.strip()}")

        resp = input(f"\n  Reverter mudancas? (s/n): ").strip().lower()
        if resp == 's':
            git_rollback()
            log(f"  Lote '{lote['nome']}' REVERTIDO.")
            print("  Revertido com sucesso.")
        else:
            print("  Mantendo mudancas (build quebrado). Use [R] para reverter depois.")

def lote_customizado(all_files):
    """Permite ao usuario digitar um diretorio para aplicar."""
    diretorio = input("  Diretorio (ex: Pages/Multa): ").strip().rstrip('/')
    if not diretorio:
        return

    # Filtrar arquivos que comecam com esse diretorio
    files = [(f, t) for f, t in all_files if f.startswith(diretorio + '/') or f.startswith(diretorio + '\\')]
    files = [(f, t) for f, t in files if not is_protected(f)]

    if not files:
        print(f"  Nenhum arquivo encontrado em '{diretorio}'")
        return

    lote = {
        'id': 99,
        'nome': f'Custom: {diretorio}',
        'descricao': f'Lote customizado para {diretorio}',
        'risco': 'CUSTOMIZADO',
        'diretorios': [diretorio + '/'],
    }
    # Override get_lote_files for this case
    lote['_files'] = files

    novos = sum(1 for _, t in files if t == 'NOVO')
    mods = sum(1 for _, t in files if t == 'MODIFICADO')
    dels = sum(1 for _, t in files if t == 'REMOVIDO')

    print(f"\n  Lote customizado: {diretorio}")
    print(f"  Arquivos: {len(files)} ({mods} mod, {novos} new, {dels} del)")
    for f, t in sorted(files):
        marker = {'NOVO': '[NEW]', 'REMOVIDO': '[DEL]', 'MODIFICADO': '[MOD]'}[t]
        print(f"    {marker} {f}")

    resp = input(f"\n  Aplicar? (s/n): ").strip().lower()
    if resp != 's':
        print("  Cancelado.")
        return

    log(f"\n=== Aplicando lote customizado: {diretorio} ({len(files)} arquivos) ===")
    git_checkpoint(f"checkpoint: antes de custom '{diretorio}'")

    aplicados = 0
    for filepath, tipo in files:
        try:
            if aplicar_arquivo(filepath, tipo):
                aplicados += 1
        except Exception as e:
            log(f"    ERRO ao aplicar {filepath}: {e}")

    log(f"  Copiados: {aplicados}")

    sucesso, output, build_erros = dotnet_build()
    if sucesso:
        git_checkpoint(f"aplicado custom: {diretorio} ({aplicados} arquivos)")
        print(f"\n  >>> LOTE CUSTOMIZADO APLICADO COM SUCESSO! <<<")
    else:
        print(f"\n  >>> BUILD FALHOU! ({len(build_erros)} erros) <<<")
        for e in build_erros[:10]:
            print(f"    {e.strip()}")
        resp = input(f"\n  Reverter mudancas? (s/n): ").strip().lower()
        if resp == 's':
            git_rollback()
            log(f"  Lote custom '{diretorio}' REVERTIDO.")

def mostrar_status():
    """Mostra historico de commits."""
    result = run_git(['log', '--oneline', '-20'])
    print(f"\n  Ultimos 20 commits no Janeiro:")
    print(f"  {'-' * 50}")
    for line in result.stdout.strip().split('\n'):
        print(f"    {line}")

def rollback_ultimo():
    """Reverte o ultimo lote aplicado."""
    result = run_git(['log', '--oneline', '-5'])
    print(f"\n  Ultimos 5 commits:")
    for line in result.stdout.strip().split('\n'):
        print(f"    {line}")

    resp = input(f"\n  Reverter para o commit anterior (git reset --hard HEAD~1)? (s/n): ").strip().lower()
    if resp == 's':
        result = run_git(['reset', '--hard', 'HEAD~1'])
        if result.returncode == 0:
            log("  Rollback: revertido para commit anterior (HEAD~1)")
            print("  Revertido com sucesso.")
        else:
            print(f"  Erro: {result.stderr}")

# ============================================================
# MAIN
# ============================================================
def main():
    print("Escaneando FrotiX.Atualizado...")
    all_files = scan_diffs()
    print(f"  {len(all_files)} arquivos mapeados.")

    # Verificar que diretorios existem
    for d in [JANEIRO, ATUAL, DIFFS]:
        if not os.path.isdir(d):
            print(f"ERRO: Diretorio nao encontrado: {d}")
            sys.exit(1)

    # Verificar git no Janeiro
    if not os.path.isdir(os.path.join(JANEIRO, '.git')):
        print(f"ERRO: {JANEIRO} nao e um repositorio git. Rode: cd {JANEIRO} && git init && git add -A && git commit -m 'estado inicial'")
        sys.exit(1)

    while True:
        mostrar_menu(all_files)
        escolha = input("  Escolha: ").strip().upper()

        if escolha == 'Q':
            print("  Ate logo!")
            break
        elif escolha == 'S':
            mostrar_status()
        elif escolha == 'R':
            rollback_ultimo()
        elif escolha == 'L':
            try:
                lid = int(input("  Numero do lote: ").strip())
                listar_arquivos_lote(lid, all_files)
            except ValueError:
                print("  Numero invalido.")
        elif escolha == 'C':
            lote_customizado(all_files)
        else:
            try:
                lid = int(escolha)
                if 0 <= lid < len(LOTES):
                    aplicar_lote_interativo(LOTES[lid], all_files)
                else:
                    print("  Lote invalido.")
            except ValueError:
                print("  Opcao invalida.")

        input("\n  [Enter para continuar...]")

if __name__ == '__main__':
    main()
