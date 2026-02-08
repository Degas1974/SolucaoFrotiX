"""Aplica TODOS os arquivos C# de uma vez (exceto Telerik e protegidos)."""
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

# Diretorios C# (tudo exceto wwwroot e Pages cshtml)
CS_DIRS = [
    'Models/', 'Repository/', 'Services/', 'Controllers/',
    'Cache/', 'Data/', 'Helpers/', 'Extensions/', 'Middlewares/',
    'Hubs/', 'Infrastructure/', 'EndPoints/', 'ViewComponents/', 'Settings/',
    'Areas/',
]

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
    return subprocess.run(['git'] + args, cwd=JANEIRO, capture_output=True, text=True, encoding='utf-8', errors='replace')

def git_checkpoint(msg):
    git_cmd(['add', '-A'])
    r = git_cmd(['commit', '--no-verify', '-m', msg, '--allow-empty'])
    if r.returncode == 0:
        print(f"  COMMIT: {msg}")

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
        for e in erros[:20]:
            print(f"    {e.strip()}")
        return False, erros

def aplicar(filepath, tipo):
    src = os.path.join(ATUAL, filepath)
    dst = os.path.join(JANEIRO, filepath)
    if tipo in ('NOVO', 'MODIFICADO'):
        if not os.path.exists(src):
            return False
        os.makedirs(os.path.dirname(dst), exist_ok=True)
        shutil.copy2(src, dst)
        return True
    elif tipo == 'REMOVIDO':
        if os.path.exists(dst):
            os.remove(dst)
            return True
    return False

def main():
    mode = sys.argv[1] if len(sys.argv) > 1 else 'csharp'

    print("Escaneando diffs...")
    all_files = scan_diffs()
    print(f"  {len(all_files)} arquivos mapeados")

    if mode == 'csharp':
        # Todos os .cs + Areas + Pages (cshtml + cshtml.cs) - tudo exceto wwwroot
        files = []
        for f, t in all_files:
            if is_protected(f):
                continue
            norm = f.replace('\\', '/')
            # Incluir se esta em um dir C# OU Pages OU se e .cs na raiz
            in_cs_dir = any(norm.startswith(d) for d in CS_DIRS)
            is_cs_file = norm.endswith('.cs')
            is_page = norm.startswith('Pages/')
            if in_cs_dir or is_page or (is_cs_file and '/' not in norm):
                files.append((f, t))
        nome = "Mega-lote C# + Pages"

    elif mode == 'pages':
        # Todas as Pages .cshtml (sem code-behind, que ja foi no csharp)
        files = []
        for f, t in all_files:
            if is_protected(f):
                continue
            norm = f.replace('\\', '/')
            if norm.startswith('Pages/') and norm.endswith('.cshtml') and not norm.endswith('.cshtml.cs'):
                files.append((f, t))
        nome = "Lote Pages (.cshtml)"

    elif mode == 'js':
        files = [(f, t) for f, t in all_files if not is_protected(f) and f.replace('\\', '/').startswith('wwwroot/js/')]
        nome = "Lote JavaScript"

    elif mode == 'css':
        files = [(f, t) for f, t in all_files if not is_protected(f) and f.replace('\\', '/').startswith('wwwroot/css/')]
        nome = "Lote CSS"

    elif mode == 'sql':
        files = [(f, t) for f, t in all_files if not is_protected(f) and (f.replace('\\', '/').startswith('SQL/') or f.replace('\\', '/').startswith('Scripts/'))]
        nome = "Lote SQL"

    elif mode == 'rest':
        # Tudo que sobrou (nao C#, nao Pages, nao JS, nao CSS, nao SQL)
        already = set()
        for f, t in all_files:
            norm = f.replace('\\', '/')
            in_cs_dir = any(norm.startswith(d) for d in CS_DIRS)
            if in_cs_dir or norm.endswith('.cs') or norm.startswith('Pages/') or norm.startswith('wwwroot/'):
                already.add(f)
            if norm.startswith('SQL/') or norm.startswith('Scripts/'):
                already.add(f)
        files = [(f, t) for f, t in all_files if not is_protected(f) and f not in already]
        nome = "Lote Restante"

    else:
        print(f"Modo desconhecido: {mode}")
        print("Modos: csharp, pages, js, css, sql, rest")
        sys.exit(1)

    novos = sum(1 for _, t in files if t == 'NOVO')
    mods = sum(1 for _, t in files if t == 'MODIFICADO')
    dels = sum(1 for _, t in files if t == 'REMOVIDO')
    telerik_skip = sum(1 for f, _ in all_files if f.replace('\\', '/') in TELERIK_FILES)

    print(f"\n{'='*60}")
    print(f"  {nome}")
    print(f"  {len(files)} arquivos ({mods} mod, {novos} new, {dels} del)")
    print(f"  {telerik_skip} arquivos Telerik excluidos globalmente")
    print(f"{'='*60}")

    if '--list' in sys.argv:
        for f, t in sorted(files):
            marker = {'NOVO': '[NEW]', 'REMOVIDO': '[DEL]', 'MODIFICADO': '[MOD]'}[t]
            print(f"  {marker} {f}")
        return

    if not files:
        print("  Nenhum arquivo para aplicar.")
        return

    git_checkpoint(f"checkpoint: antes de '{nome}'")

    count = 0
    skip = 0
    for filepath, tipo in files:
        if aplicar(filepath, tipo):
            count += 1
        else:
            skip += 1
    print(f"  Aplicados: {count} | Skipped: {skip}")

    ok, erros = dotnet_build()
    if ok:
        git_checkpoint(f"aplicado: {nome} ({count} arquivos)")
        print(f"\n  >>> SUCESSO! <<<")
    else:
        print(f"\n  >>> FALHOU! Revertendo... <<<")
        git_rollback()
        sys.exit(1)

if __name__ == '__main__':
    main()
