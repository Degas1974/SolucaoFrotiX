import os

# Caminho do arquivo bruto
input_file = 'todos_arquivos_projeto.txt'
output_file = 'plans/arquivos_pendentes.md'

# Arquivos já processados (caminhos relativos normalizados)
processed_files = {
    'Areas/Authorization/Pages/Roles.cshtml.cs',
    'Areas/Authorization/Pages/Users.cshtml.cs',
    'Areas/Authorization/Pages/Usuarios.cshtml.cs',
    'Areas/Identity/Pages/Account/ConfirmEmail.cshtml.cs',
    'Areas/Identity/Pages/Account/ConfirmEmailChange.cshtml.cs',
    'Areas/Identity/Pages/Account/ForgotPassword.cshtml.cs',
    'Areas/Identity/Pages/Account/ForgotPasswordConfirmation.cshtml.cs',
    'Areas/Identity/Pages/Account/Lockout.cshtml.cs',
    'Areas/Identity/Pages/Account/Login.cshtml.cs',
    'Areas/Identity/Pages/ConfirmarSenha.cshtml.cs'
}

# Padrões a ignorar (Libs externas, minificados, etc)
ignore_patterns = [
    '.min.js', '.min.css',
    '/node_modules/', '/bin/', '/obj/',
    'jquery', 'bootstrap', 'fontawesome', 'modernizr',
    'ej2', 'syncfusion', 'toastr', 'sweetalert2', 'fullcalendar',
    'chartist', 'chartjs', 'c3', 'dygraph', 'dropify',
    'summernote', 'select2', 'smartwizard', 'nouislider',
    'ion-rangeslider', 'dropzone', 'cropperjs', 'jqvmap',
    'lightgallery', 'spinkit', 'microtip', 'flatpickr',
    'bundle.js', 'bundle.css',
    'designer.cs', 'g.cs', 'assemblyinfo.cs'
]

# Exceções que DEVEMOS incluir mesmo parecendo libs (se houver)
keep_patterns = [
    'sweetalert_interop.js',
    'frotix',
    'alerta'
]

def should_process(file_path):
    # Normalizar separadores
    path = file_path.replace('\\', '/')
    filename = os.path.basename(path).lower()
    
    # Se for C#, processamos quase tudo (exceto designers e gerados)
    if path.endswith('.cs'):
        if '.g.cs' in filename or '.designer.cs' in filename:
            return False
        return True
        
    # Para JS e CSS, aplicamos filtros de libs
    is_asset = path.endswith('.js') or path.endswith('.css')
    if is_asset:
        # Se for um arquivo que queremos manter explicitamente
        for keep in keep_patterns:
            if keep in filename:
                return True
                
        # Verificar padrões de ignorar
        for pattern in ignore_patterns:
            if pattern in path.lower():
                return False
        return True
        
    return False

def main():
    with open(input_file, 'r', encoding='utf-8') as f:
        lines = f.readlines()

    pending_files = []
    prefix_to_remove = r'C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site\\'

    for line in lines:
        # Limpar linha e remover prefixo absoluto
        path = line.strip()
        if path.startswith(prefix_to_remove):
            path = path[len(prefix_to_remove):]
        
        # Normalizar para verificar
        path_normalized = path.replace('\\', '/')
        
        # Verificar se já foi processado
        if path_normalized in processed_files:
            continue
            
        # Verificar critérios de filtro
        if should_process(path_normalized):
            pending_files.append(path_normalized)

    # Ordenar alfabeticamente
    pending_files.sort()

    # Escrever no arquivo de plano
    with open(output_file, 'w', encoding='utf-8') as f:
        f.write("# Lista Completa de Arquivos Pendentes\n")
        f.write("# Gerada automaticamente com filtros de libs externas\n\n")
        
        for path in pending_files:
            f.write(f"- {path}\n")

if __name__ == '__main__':
    main()
