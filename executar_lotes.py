"""Executa lotes do aplicar_mudancas de forma nao-interativa."""
import os, sys, shutil, subprocess

JANEIRO  = 'FrotiX.Site.2026.01'
ATUAL    = 'FrotiX.Site'
DIFFS    = 'FrotiX.Atualizado'

NEVER_COPY = {
    'FrotiX.csproj', 'FrotiX.sln', 'Startup.cs', 'Program.cs',
    'appsettings.json', 'appsettings.Development.json', 'appsettings.Production.json',
    'web.config', 'package.json', 'package-lock.json', 'libman.json',
}
NEVER_DIRS = {'bin', 'obj', 'node_modules', '.git', '.vs', '.claude', '.cursor', '.gemini'}

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

def scan_diffs():
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
                actual = rel[:-3]
                files.append((actual, tipo))
    return files

def is_protected(filepath):
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

def git_cmd(args):
    r = subprocess.run(['git'] + args, cwd=JANEIRO, capture_output=True, text=True, encoding='utf-8', errors='replace')
    return r

def git_checkpoint(msg):
    git_cmd(['add', '-A'])
    r = git_cmd(['commit', '--no-verify', '-m', msg, '--allow-empty'])
    if r.returncode == 0:
        print(f"  COMMIT: {msg}")
    else:
        print(f"  (nada para commitar): {msg}")

def git_rollback():
    git_cmd(['checkout', '.'])
    git_cmd(['clean', '-fd'])
    print("  ROLLBACK executado")

def dotnet_build():
    print("  Rodando dotnet build...")
    r = subprocess.run(['dotnet', 'build', '--no-restore', '-v', 'q'],
        cwd=JANEIRO, capture_output=True, text=True, encoding='utf-8', errors='replace', timeout=300)
    output = r.stdout + r.stderr
    erros = [l for l in output.split('\n') if ': error ' in l]
    warnings = [l for l in output.split('\n') if ': warning ' in l]
    if r.returncode == 0:
        print(f"  BUILD OK ({len(warnings)} warnings)")
        return True, erros
    else:
        print(f"  BUILD FALHOU ({len(erros)} erros, {len(warnings)} warnings)")
        for e in erros[:15]:
            print(f"    {e.strip()}")
        return False, erros

def aplicar(filepath, tipo):
    src = os.path.join(ATUAL, filepath)
    dst = os.path.join(JANEIRO, filepath)
    if tipo in ('NOVO', 'MODIFICADO'):
        if not os.path.exists(src):
            print(f"    SKIP (nao encontrado): {filepath}")
            return False
        os.makedirs(os.path.dirname(dst), exist_ok=True)
        shutil.copy2(src, dst)
        return True
    elif tipo == 'REMOVIDO':
        if os.path.exists(dst):
            os.remove(dst)
            return True
        else:
            print(f"    SKIP (ja removido): {filepath}")
            return False
    return False

def get_files(all_files, tipo_filter=None, dirs_filter=None, dirs_exclude=None):
    result = []
    for f, t in all_files:
        if is_protected(f):
            continue
        if tipo_filter and t != tipo_filter:
            continue
        if dirs_filter:
            if not any(f.startswith(d) for d in dirs_filter):
                continue
            if dirs_exclude and any(f.startswith(d) for d in dirs_exclude):
                continue
        result.append((f, t))
    return result

def run_lote(nome, files, all_files):
    if not files:
        print(f"\n=== {nome}: 0 arquivos - pulando ===")
        return True

    novos = sum(1 for _, t in files if t == 'NOVO')
    mods = sum(1 for _, t in files if t == 'MODIFICADO')
    dels = sum(1 for _, t in files if t == 'REMOVIDO')

    print(f"\n{'='*60}")
    print(f"=== {nome}: {len(files)} arquivos ({mods} mod, {novos} new, {dels} del) ===")
    print(f"{'='*60}")

    git_checkpoint(f"checkpoint: antes de '{nome}'")

    count = 0
    for filepath, tipo in files:
        if aplicar(filepath, tipo):
            count += 1
    print(f"  Aplicados: {count}/{len(files)}")

    ok, erros = dotnet_build()
    if ok:
        git_checkpoint(f"aplicado: {nome} ({count} arquivos)")
        print(f"  SUCESSO!")
        return True
    else:
        print(f"  FALHOU! Revertendo...")
        git_rollback()
        return False

if __name__ == '__main__':
    lote_num = int(sys.argv[1]) if len(sys.argv) > 1 else -1

    print("Escaneando diffs...")
    all_files = scan_diffs()
    print(f"  {len(all_files)} arquivos mapeados")

    lotes = {
        1: ('Lote 1: Arquivos novos', lambda: get_files(all_files, tipo_filter='NOVO')),
        2: ('Lote 2: Arquivos deletados', lambda: get_files(all_files, tipo_filter='REMOVIDO')),
        3: ('Lote 3: Models', lambda: get_files(all_files, dirs_filter=['Models/'])),
        4: ('Lote 4: Repository/IRepository', lambda: get_files(all_files, dirs_filter=['Repository/IRepository/'])),
        5: ('Lote 5: Repository (impl)', lambda: get_files(all_files, dirs_filter=['Repository/'], dirs_exclude=['Repository/IRepository/'])),
        6: ('Lote 6: Services', lambda: get_files(all_files, dirs_filter=['Services/'])),
        7: ('Lote 7: Infraestrutura', lambda: get_files(all_files, dirs_filter=['Cache/', 'Data/', 'Helpers/', 'Extensions/', 'Middlewares/', 'Hubs/', 'Infrastructure/', 'EndPoints/', 'ViewComponents/', 'Settings/'])),
        8: ('Lote 8: Controllers', lambda: get_files(all_files, dirs_filter=['Controllers/'])),
        9: ('Lote 9: Areas', lambda: get_files(all_files, dirs_filter=['Areas/'])),
        10: ('Lote 10: Pages', lambda: get_files(all_files, dirs_filter=['Pages/'])),
        11: ('Lote 11: JavaScript', lambda: get_files(all_files, dirs_filter=['wwwroot/js/'])),
        12: ('Lote 12: CSS', lambda: get_files(all_files, dirs_filter=['wwwroot/css/'])),
        13: ('Lote 13: SQL Scripts', lambda: get_files(all_files, dirs_filter=['SQL/', 'Scripts/'])),
    }

    if lote_num == -1:
        # Listar todos
        for k, (nome, fn) in sorted(lotes.items()):
            files = fn()
            novos = sum(1 for _, t in files if t == 'NOVO')
            mods = sum(1 for _, t in files if t == 'MODIFICADO')
            dels = sum(1 for _, t in files if t == 'REMOVIDO')
            print(f"  [{k:>2}] {nome:<40} {len(files):>3} arq ({mods} mod, {novos} new, {dels} del)")
    elif lote_num in lotes:
        nome, fn = lotes[lote_num]
        files = fn()
        ok = run_lote(nome, files, all_files)
        sys.exit(0 if ok else 1)
    elif lote_num == 99:
        # Listar arquivos de um lote especifico
        lid = int(sys.argv[2])
        if lid in lotes:
            nome, fn = lotes[lid]
            files = fn()
            print(f"\n{nome}: {len(files)} arquivos")
            for f, t in sorted(files):
                marker = {'NOVO': '[NEW]', 'REMOVIDO': '[DEL]', 'MODIFICADO': '[MOD]'}[t]
                print(f"  {marker} {f}")
