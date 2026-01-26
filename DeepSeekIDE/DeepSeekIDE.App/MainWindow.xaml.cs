using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DeepSeekIDE.Core.Models;
using DeepSeekIDE.Core.Services;
using Microsoft.Web.WebView2.Core;

// Resolução de ambiguidades WPF vs WinForms
using Button = System.Windows.Controls.Button;
using Color = System.Windows.Media.Color;
using Brushes = System.Windows.Media.Brushes;
using Cursors = System.Windows.Input.Cursors;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace DeepSeekIDE.App;

/// <summary>
/// Janela principal do DeepSeek IDE
/// </summary>
public partial class MainWindow : Window
{
    // Serviços
    private readonly DeepSeekService _deepSeekService;
    private readonly FileSystemService _fileSystemService;
    private readonly GitService _gitService;

    // Estado
    private string _currentWorkspacePath = string.Empty;
    private string? _currentFilePath;
    private readonly List<ChatMessage> _chatHistory = new();
    private readonly Dictionary<string, string> _openFiles = new(); // path -> content
    private bool _isEditorReady;

    // Configuração
    private const string DEEPSEEK_API_KEY = "sk-abe79be96b3347d6b07888636e5253b3";
    private const string RULES_FILE_NAME = "DEEPSEEK_RULES.md";
    private string _currentRules = "Você é um assistente de programação útil e experiente. Responda de forma clara e concisa. Quando fornecer código, use blocos de código com a linguagem especificada.";

    public MainWindow()
    {
        InitializeComponent();

        // Inicializa serviços
        _deepSeekService = new DeepSeekService(DEEPSEEK_API_KEY);
        _fileSystemService = new FileSystemService();
        _gitService = new GitService();

        // Configura eventos do DeepSeek
        _deepSeekService.OnStreamChunk += DeepSeek_OnStreamChunk;
        _deepSeekService.OnStreamComplete += DeepSeek_OnStreamComplete;
        _deepSeekService.OnError += DeepSeek_OnError;

        // Inicializa WebView2
        InitializeWebView();

        // Configura atalhos de teclado
        SetupKeyboardShortcuts();

        // Mensagem de boas-vindas no chat
        AddChatMessage("Olá! Sou o DeepSeek, seu assistente de programação. Como posso ajudar?", false);

        // Carrega regras do workspace se existir
        LoadWorkspaceRules();

        Closed += MainWindow_Closed;
    }

    #region Keyboard Shortcuts

    private void SetupKeyboardShortcuts()
    {
        // Ctrl+S - Salvar arquivo
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => SaveCurrentFile()), new KeyGesture(Key.S, ModifierKeys.Control)));

        // Ctrl+O - Abrir pasta
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => OpenFolder_Click(this, new RoutedEventArgs())), new KeyGesture(Key.O, ModifierKeys.Control)));

        // Ctrl+Shift+E - Explorador
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => ShowExplorer()), new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift)));

        // Ctrl+Shift+F - Busca
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => ShowSearch()), new KeyGesture(Key.F, ModifierKeys.Control | ModifierKeys.Shift)));

        // Ctrl+Shift+G - Git
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => ShowGit()), new KeyGesture(Key.G, ModifierKeys.Control | ModifierKeys.Shift)));

        // Ctrl+Shift+I - Chat IA
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => FocusChat()), new KeyGesture(Key.I, ModifierKeys.Control | ModifierKeys.Shift)));

        // Ctrl+W - Fechar tab atual
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => CloseCurrentTab()), new KeyGesture(Key.W, ModifierKeys.Control)));

        // Ctrl+Tab - Próxima tab
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => NextTab()), new KeyGesture(Key.Tab, ModifierKeys.Control)));

        // Ctrl+Shift+Tab - Tab anterior
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => PreviousTab()), new KeyGesture(Key.Tab, ModifierKeys.Control | ModifierKeys.Shift)));

        // F5 - Atualizar Git
        InputBindings.Add(new KeyBinding(new RelayCommand(_ => RefreshGitStatus()), new KeyGesture(Key.F5)));

        // Ctrl+Enter - Enviar mensagem no chat (quando focado no input)
        InputBindings.Add(new KeyBinding(new RelayCommand(async _ => await SendChatMessage()), new KeyGesture(Key.Enter, ModifierKeys.Control)));
    }

    private void ShowExplorer()
    {
        btnExplorer.Tag = "Active";
        btnSearch.Tag = null;
        btnGit.Tag = null;
        pnlExplorer.Visibility = Visibility.Visible;
        pnlSearch.Visibility = Visibility.Collapsed;
        pnlGit.Visibility = Visibility.Collapsed;
    }

    private void ShowSearch()
    {
        btnExplorer.Tag = null;
        btnSearch.Tag = "Active";
        btnGit.Tag = null;
        pnlExplorer.Visibility = Visibility.Collapsed;
        pnlSearch.Visibility = Visibility.Visible;
        pnlGit.Visibility = Visibility.Collapsed;
        txtSearch.Focus();
    }

    private void ShowGit()
    {
        btnExplorer.Tag = null;
        btnSearch.Tag = null;
        btnGit.Tag = "Active";
        pnlExplorer.Visibility = Visibility.Collapsed;
        pnlSearch.Visibility = Visibility.Collapsed;
        pnlGit.Visibility = Visibility.Visible;
        RefreshGitStatus();
    }

    private void FocusChat()
    {
        txtChatInput.Focus();
    }

    private async void CloseCurrentTab()
    {
        if (_currentFilePath != null)
        {
            _openFiles.Remove(_currentFilePath);

            var tabToRemove = pnlTabs.Children.Cast<Border>().FirstOrDefault(t => t.Tag as string == _currentFilePath);
            if (tabToRemove != null)
            {
                pnlTabs.Children.Remove(tabToRemove);
            }

            _currentFilePath = _openFiles.Keys.FirstOrDefault();
            if (_currentFilePath != null)
            {
                await OpenFile(_currentFilePath);
            }
            else
            {
                await SetEditorContent("// Nenhum arquivo aberto", "plaintext");
                txtCurrentFile.Text = "Bem-vindo";
            }
        }
    }

    private async void NextTab()
    {
        var tabs = pnlTabs.Children.Cast<Border>().ToList();
        if (tabs.Count <= 1) return;

        var currentIndex = tabs.FindIndex(t => t.Tag as string == _currentFilePath);
        var nextIndex = (currentIndex + 1) % tabs.Count;
        var nextPath = tabs[nextIndex].Tag as string;
        if (nextPath != null)
        {
            await OpenFile(nextPath);
        }
    }

    private async void PreviousTab()
    {
        var tabs = pnlTabs.Children.Cast<Border>().ToList();
        if (tabs.Count <= 1) return;

        var currentIndex = tabs.FindIndex(t => t.Tag as string == _currentFilePath);
        var prevIndex = (currentIndex - 1 + tabs.Count) % tabs.Count;
        var prevPath = tabs[prevIndex].Tag as string;
        if (prevPath != null)
        {
            await OpenFile(prevPath);
        }
    }

    #endregion

    /// <summary>
    /// Carrega regras do arquivo DEEPSEEK_RULES.md se existir no workspace
    /// </summary>
    private void LoadWorkspaceRules()
    {
        if (string.IsNullOrEmpty(_currentWorkspacePath)) return;

        var rulesPath = Path.Combine(_currentWorkspacePath, RULES_FILE_NAME);
        if (File.Exists(rulesPath))
        {
            try
            {
                _currentRules = File.ReadAllText(rulesPath);
                txtRules.Text = _currentRules;
                AddChatMessage($"Regras carregadas de {RULES_FILE_NAME}", false);
            }
            catch { }
        }
    }

    #region WebView2 / Monaco Editor

    private async void InitializeWebView()
    {
        try
        {
            await webEditor.EnsureCoreWebView2Async();
            webEditor.CoreWebView2.Settings.IsScriptEnabled = true;
            webEditor.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;

            // Carrega o Monaco Editor
            var monacoHtml = GetMonacoEditorHtml();
            webEditor.NavigateToString(monacoHtml);

            webEditor.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            webEditor.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao inicializar editor: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        _isEditorReady = true;
    }

    private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            var message = System.Text.Json.JsonSerializer.Deserialize<EditorMessage>(e.WebMessageAsJson);
            if (message == null) return;

            Dispatcher.Invoke(() =>
            {
                switch (message.Type)
                {
                    case "cursorPosition":
                        txtStatusLine.Text = $"Ln {message.Line}, Col {message.Column}";
                        break;
                    case "contentChanged":
                        if (_currentFilePath != null && _openFiles.ContainsKey(_currentFilePath))
                        {
                            _openFiles[_currentFilePath] = message.Content ?? string.Empty;
                            MarkFileAsModified(_currentFilePath);
                        }
                        break;
                    case "save":
                        SaveCurrentFile();
                        break;
                }
            });
        }
        catch { }
    }

    private string GetMonacoEditorHtml()
    {
        return @"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <style>
        html, body { margin: 0; padding: 0; width: 100%; height: 100%; overflow: hidden; }
        #editor { width: 100%; height: 100%; }
    </style>
</head>
<body>
    <div id=""editor""></div>
    <script src=""https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs/loader.min.js""></script>
    <script>
        let editor;
        let currentLanguage = 'plaintext';

        require.config({ paths: { 'vs': 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs' }});

        require(['vs/editor/editor.main'], function() {
            editor = monaco.editor.create(document.getElementById('editor'), {
                value: '// Bem-vindo ao DeepSeek IDE!\n// Abra uma pasta ou arquivo para começar.',
                language: 'javascript',
                theme: 'vs-dark',
                fontSize: 14,
                fontFamily: 'Consolas, Menlo, monospace',
                minimap: { enabled: true },
                automaticLayout: true,
                scrollBeyondLastLine: false,
                renderWhitespace: 'selection',
                tabSize: 4,
                insertSpaces: true,
                wordWrap: 'off',
                lineNumbers: 'on',
                glyphMargin: true,
                folding: true,
                bracketPairColorization: { enabled: true }
            });

            // Eventos
            editor.onDidChangeCursorPosition(function(e) {
                window.chrome.webview.postMessage({
                    type: 'cursorPosition',
                    line: e.position.lineNumber,
                    column: e.position.column
                });
            });

            editor.onDidChangeModelContent(function(e) {
                window.chrome.webview.postMessage({
                    type: 'contentChanged',
                    content: editor.getValue()
                });
            });

            // Atalhos
            editor.addCommand(monaco.KeyMod.CtrlCmd | monaco.KeyCode.KeyS, function() {
                window.chrome.webview.postMessage({ type: 'save' });
            });
        });

        // Funções expostas para C#
        function setContent(content, language) {
            if (editor) {
                const model = monaco.editor.createModel(content, language);
                editor.setModel(model);
                currentLanguage = language;
            }
        }

        function getContent() {
            return editor ? editor.getValue() : '';
        }

        function setLanguage(language) {
            if (editor && editor.getModel()) {
                monaco.editor.setModelLanguage(editor.getModel(), language);
                currentLanguage = language;
            }
        }

        function insertText(text) {
            if (editor) {
                const selection = editor.getSelection();
                const id = { major: 1, minor: 1 };
                const op = { identifier: id, range: selection, text: text, forceMoveMarkers: true };
                editor.executeEdits('insertText', [op]);
            }
        }

        function getSelectedText() {
            if (editor) {
                const selection = editor.getSelection();
                return editor.getModel().getValueInRange(selection);
            }
            return '';
        }

        function goToLine(line, column) {
            if (editor) {
                editor.setPosition({ lineNumber: line, column: column || 1 });
                editor.revealLineInCenter(line);
                editor.focus();
            }
        }
    </script>
</body>
</html>";
    }

    private async Task SetEditorContent(string content, string language)
    {
        if (!_isEditorReady) return;

        var escapedContent = content.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r");
        await webEditor.ExecuteScriptAsync($"setContent('{escapedContent}', '{language}')");
    }

    private async Task<string> GetEditorContent()
    {
        if (!_isEditorReady) return string.Empty;

        var result = await webEditor.ExecuteScriptAsync("getContent()");
        return result.Trim('"').Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t").Replace("\\\\", "\\");
    }

    private async Task<string> GetSelectedText()
    {
        if (!_isEditorReady) return string.Empty;

        var result = await webEditor.ExecuteScriptAsync("getSelectedText()");
        return result.Trim('"').Replace("\\n", "\n").Replace("\\r", "\r");
    }

    #endregion

    #region Window Controls

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            Maximize_Click(sender, e);
        }
        else
        {
            DragMove();
        }
    }

    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void Maximize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();

    #endregion

    #region Activity Bar / Sidebar

    private void ActivityBar_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button == null) return;

        // Reset all buttons
        btnExplorer.Tag = null;
        btnSearch.Tag = null;
        btnGit.Tag = null;
        btnChat.Tag = null;

        // Hide all panels
        pnlExplorer.Visibility = Visibility.Collapsed;
        pnlSearch.Visibility = Visibility.Collapsed;
        pnlGit.Visibility = Visibility.Collapsed;

        // Activate selected
        button.Tag = "Active";

        if (button == btnExplorer)
        {
            pnlExplorer.Visibility = Visibility.Visible;
        }
        else if (button == btnSearch)
        {
            pnlSearch.Visibility = Visibility.Visible;
            txtSearch.Focus();
        }
        else if (button == btnGit)
        {
            pnlGit.Visibility = Visibility.Visible;
            RefreshGitStatus();
        }
    }

    #endregion

    #region File Explorer

    private void OpenFolder_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new System.Windows.Forms.FolderBrowserDialog
        {
            Description = "Selecione a pasta do projeto",
            ShowNewFolderButton = false
        };

        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            OpenWorkspace(dialog.SelectedPath);
        }
    }

    private void OpenWorkspace(string path)
    {
        _currentWorkspacePath = path;

        // Carrega estrutura de diretórios
        var rootItem = _fileSystemService.GetDirectoryStructure(path, maxDepth: 4);
        treeExplorer.ItemsSource = new[] { rootItem };

        // Abre Git se for repositório
        if (GitService.IsGitRepository(path))
        {
            _gitService.OpenRepository(path);
            RefreshGitStatus();
        }

        // Carrega regras do workspace se existir
        LoadWorkspaceRules();

        Title = $"DeepSeek IDE - {Path.GetFileName(path)}";
    }

    private async void TreeExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is FileSystemItem item && !item.IsDirectory)
        {
            await OpenFile(item.FullPath);
        }
    }

    private async Task OpenFile(string filePath)
    {
        try
        {
            // Lê o arquivo
            var content = await _fileSystemService.ReadFileAsync(filePath);
            var fileItem = _fileSystemService.GetFileInfo(filePath);

            // Adiciona à lista de arquivos abertos
            if (!_openFiles.ContainsKey(filePath))
            {
                _openFiles[filePath] = content;
                AddTab(filePath, fileItem.Name);
            }

            // Define como arquivo atual
            _currentFilePath = filePath;

            // Atualiza editor
            await SetEditorContent(content, fileItem.Language);

            // Atualiza status bar
            txtCurrentFile.Text = fileItem.Name;
            txtStatusLanguage.Text = fileItem.Language;
            txtStatusEncoding.Text = "UTF-8";

            // Atualiza tabs
            UpdateTabSelection(filePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao abrir arquivo: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void AddTab(string filePath, string fileName)
    {
        var tab = new Border
        {
            Background = new SolidColorBrush(Color.FromRgb(0x1e, 0x1e, 0x1e)),
            BorderThickness = new Thickness(0, 0, 1, 0),
            BorderBrush = new SolidColorBrush(Color.FromRgb(0x3c, 0x3c, 0x3c)),
            Padding = new Thickness(10, 8, 10, 8),
            Tag = filePath,
            Cursor = Cursors.Hand
        };

        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        var text = new TextBlock
        {
            Text = fileName,
            Foreground = new SolidColorBrush(Color.FromRgb(0xcc, 0xcc, 0xcc)),
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(text, 0);

        var closeBtn = new Button
        {
            Content = "×",
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Foreground = new SolidColorBrush(Color.FromRgb(0x80, 0x80, 0x80)),
            Margin = new Thickness(8, 0, 0, 0),
            Padding = new Thickness(0),
            Width = 16,
            Height = 16,
            FontSize = 14,
            Tag = filePath
        };
        closeBtn.Click += CloseTab_Click;
        Grid.SetColumn(closeBtn, 1);

        grid.Children.Add(text);
        grid.Children.Add(closeBtn);
        tab.Child = grid;

        tab.MouseLeftButtonDown += (s, e) =>
        {
            _ = OpenFile(filePath);
        };

        pnlTabs.Children.Add(tab);
    }

    private async void CloseTab_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string filePath)
        {
            // Remove da lista
            _openFiles.Remove(filePath);

            // Remove tab
            var tabToRemove = pnlTabs.Children.Cast<Border>().FirstOrDefault(t => t.Tag as string == filePath);
            if (tabToRemove != null)
            {
                pnlTabs.Children.Remove(tabToRemove);
            }

            // Se era o arquivo atual, abre outro ou limpa
            if (_currentFilePath == filePath)
            {
                _currentFilePath = _openFiles.Keys.FirstOrDefault();
                if (_currentFilePath != null)
                {
                    await OpenFile(_currentFilePath);
                }
                else
                {
                    await SetEditorContent("// Nenhum arquivo aberto", "plaintext");
                    txtCurrentFile.Text = "Bem-vindo";
                }
            }
        }
    }

    private void UpdateTabSelection(string filePath)
    {
        foreach (Border tab in pnlTabs.Children)
        {
            var isSelected = tab.Tag as string == filePath;
            tab.Background = new SolidColorBrush(isSelected
                ? Color.FromRgb(0x1e, 0x1e, 0x1e)
                : Color.FromRgb(0x2d, 0x2d, 0x2d));
        }
    }

    private void MarkFileAsModified(string filePath)
    {
        var tab = pnlTabs.Children.Cast<Border>().FirstOrDefault(t => t.Tag as string == filePath);
        if (tab?.Child is Grid grid && grid.Children[0] is TextBlock text)
        {
            if (!text.Text.EndsWith(" •"))
            {
                text.Text += " •";
            }
        }
    }

    private async void SaveCurrentFile()
    {
        if (string.IsNullOrEmpty(_currentFilePath)) return;

        try
        {
            var content = await GetEditorContent();
            await _fileSystemService.SaveFileAsync(_currentFilePath, content);
            _openFiles[_currentFilePath] = content;

            // Remove marcador de modificado
            var tab = pnlTabs.Children.Cast<Border>().FirstOrDefault(t => t.Tag as string == _currentFilePath);
            if (tab?.Child is Grid grid && grid.Children[0] is TextBlock text)
            {
                text.Text = text.Text.TrimEnd(' ', '•');
            }

            // Atualiza Git se aplicável
            RefreshGitStatus();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Search

    private async void TxtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(txtSearch.Text))
        {
            await PerformSearch();
        }
    }

    private async Task PerformSearch()
    {
        if (string.IsNullOrEmpty(_currentWorkspacePath)) return;

        lstSearchResults.Items.Clear();

        var results = await _fileSystemService.SearchInFilesAsync(
            _currentWorkspacePath,
            txtSearch.Text,
            chkCaseSensitive.IsChecked == true,
            chkRegex.IsChecked == true);

        foreach (var result in results.Take(100))
        {
            var item = new ListBoxItem
            {
                Content = $"{result.FileName}:{result.LineNumber} - {result.LineContent.Trim()}",
                Tag = result,
                Foreground = new SolidColorBrush(Color.FromRgb(0xcc, 0xcc, 0xcc))
            };
            lstSearchResults.Items.Add(item);
        }
    }

    private async void SearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lstSearchResults.SelectedItem is ListBoxItem item && item.Tag is SearchResult result)
        {
            await OpenFile(result.FilePath);
            await webEditor.ExecuteScriptAsync($"goToLine({result.LineNumber}, {result.MatchStart + 1})");
        }
    }

    #endregion

    #region Git

    private void RefreshGitStatus()
    {
        var status = _gitService.GetStatus();
        if (status == null)
        {
            txtGitBranch.Text = "Não é um repositório Git";
            txtGitStatus.Text = "";
            lstGitChanges.Items.Clear();
            return;
        }

        txtGitBranch.Text = $"Branch: {status.CurrentBranch}";
        txtStatusGit.Text = status.CurrentBranch;

        var changesCount = status.Files.Count;
        txtGitStatus.Text = changesCount == 0
            ? "Nenhuma alteração"
            : $"{changesCount} arquivo(s) alterado(s)";

        lstGitChanges.Items.Clear();
        foreach (var file in status.Files)
        {
            var item = new ListBoxItem
            {
                Content = $"{file.State.ToString()[0]} {file.FileName}",
                Tag = file,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(file.Color))
            };
            lstGitChanges.Items.Add(item);
        }
    }

    private async void Commit_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtCommitMessage.Text))
        {
            MessageBox.Show("Digite uma mensagem de commit", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            _gitService.StageAll();
            var commit = _gitService.Commit(txtCommitMessage.Text, "DeepSeek IDE User", "user@deepseekide.local");

            if (commit != null)
            {
                MessageBox.Show($"Commit criado: {commit.ShortSha}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCommitMessage.Text = string.Empty;
                RefreshGitStatus();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro no commit: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void Push_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            txtGitStatus.Text = "Enviando...";
            var result = await _gitService.PushAsync();

            if (result)
            {
                MessageBox.Show("Push realizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Falha ao fazer push. Verifique suas credenciais e se existe um remote configurado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RefreshGitStatus();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro no push: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            RefreshGitStatus();
        }
    }

    private async void Pull_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            txtGitStatus.Text = "Baixando...";
            var result = await _gitService.PullAsync("DeepSeek IDE User", "user@deepseekide.local");

            if (result)
            {
                MessageBox.Show("Pull realizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Falha ao fazer pull. Verifique suas credenciais e se existe um remote configurado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RefreshGitStatus();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro no pull: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            RefreshGitStatus();
        }
    }

    private async void Fetch_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            txtGitStatus.Text = "Buscando...";
            var result = await _gitService.FetchAsync();

            if (result)
            {
                MessageBox.Show("Fetch realizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Falha ao fazer fetch. Verifique se existe um remote configurado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RefreshGitStatus();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro no fetch: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            RefreshGitStatus();
        }
    }

    private void RefreshGit_Click(object sender, RoutedEventArgs e)
    {
        RefreshGitStatus();
    }

    #endregion

    #region Chat / IA

    private void AddChatMessage(string message, bool isUser)
    {
        var border = new Border
        {
            Background = new SolidColorBrush(isUser
                ? Color.FromRgb(0x3c, 0x3c, 0x3c)
                : Color.FromRgb(0x2d, 0x2d, 0x2d)),
            CornerRadius = new CornerRadius(8),
            Padding = new Thickness(12),
            Margin = new Thickness(0, 0, 0, 8),
            HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
            MaxWidth = 300
        };

        var text = new TextBlock
        {
            Text = message,
            Foreground = new SolidColorBrush(Color.FromRgb(0xcc, 0xcc, 0xcc)),
            TextWrapping = TextWrapping.Wrap
        };

        border.Child = text;
        pnlChatMessages.Children.Add(border);
        scrollChat.ScrollToEnd();
    }

    /// <summary>
    /// Adiciona uma notificação do sistema (mais sutil que mensagem normal)
    /// </summary>
    private void AddSystemNotification(string message)
    {
        var text = new TextBlock
        {
            Text = $"[{message}]",
            Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
            FontSize = 10,
            FontStyle = FontStyles.Italic,
            Margin = new Thickness(0, 0, 0, 4),
            HorizontalAlignment = HorizontalAlignment.Center
        };

        pnlChatMessages.Children.Add(text);
        scrollChat.ScrollToEnd();
    }

    /// <summary>
    /// Limpa o histórico do chat
    /// </summary>
    private void ClearChat_Click(object sender, RoutedEventArgs e)
    {
        _chatHistory.Clear();
        pnlChatMessages.Children.Clear();
        AddChatMessage("Chat limpo. Como posso ajudar?", false);
    }

    private Border? _currentStreamingMessage;
    private StringBuilder _currentStreamingContent = new();

    private void StartStreamingMessage()
    {
        _currentStreamingContent.Clear();

        var border = new Border
        {
            Background = new SolidColorBrush(Color.FromRgb(0x2d, 0x2d, 0x2d)),
            CornerRadius = new CornerRadius(8),
            Padding = new Thickness(12),
            Margin = new Thickness(0, 0, 0, 8),
            HorizontalAlignment = HorizontalAlignment.Left,
            MaxWidth = 300
        };

        var text = new TextBlock
        {
            Text = "...",
            Foreground = new SolidColorBrush(Color.FromRgb(0xcc, 0xcc, 0xcc)),
            TextWrapping = TextWrapping.Wrap
        };

        border.Child = text;
        _currentStreamingMessage = border;
        pnlChatMessages.Children.Add(border);
        scrollChat.ScrollToEnd();
    }

    private void DeepSeek_OnStreamChunk(object? sender, string chunk)
    {
        Dispatcher.Invoke(() =>
        {
            _currentStreamingContent.Append(chunk);
            if (_currentStreamingMessage?.Child is TextBlock text)
            {
                text.Text = _currentStreamingContent.ToString();
            }
            scrollChat.ScrollToEnd();
        });
    }

    private void DeepSeek_OnStreamComplete(object? sender, string fullMessage)
    {
        Dispatcher.Invoke(() =>
        {
            _chatHistory.Add(ChatMessage.Assistant(fullMessage));
            _currentStreamingMessage = null;
        });
    }

    private void DeepSeek_OnError(object? sender, Exception ex)
    {
        Dispatcher.Invoke(() =>
        {
            if (_currentStreamingMessage?.Child is TextBlock text)
            {
                text.Text = $"Erro: {ex.Message}";
                text.Foreground = new SolidColorBrush(Colors.Red);
            }
        });
    }

    private async void TxtChatInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
        {
            await SendChatMessage();
            e.Handled = true;
        }
    }

    private async void SendChat_Click(object sender, RoutedEventArgs e)
    {
        await SendChatMessage();
    }

    private async Task SendChatMessage()
    {
        var message = txtChatInput.Text.Trim();
        if (string.IsNullOrEmpty(message)) return;

        txtChatInput.Text = string.Empty;

        // Detecta e carrega arquivos mencionados na mensagem
        var (processedMessage, loadedFiles) = await ProcessFileReferences(message);

        // Mostra mensagem do usuário
        AddChatMessage(message, true);

        // Se carregou arquivos, mostra notificação sutil
        if (loadedFiles.Count > 0)
        {
            var filesLoaded = string.Join(", ", loadedFiles.Select(f => Path.GetFileName(f.Key)));
            AddSystemNotification($"Arquivos carregados: {filesLoaded}");
        }

        _chatHistory.Add(ChatMessage.User(processedMessage));

        // Inicia resposta da IA
        StartStreamingMessage();

        try
        {
            var model = (cmbModel.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "deepseek-chat";
            var messages = new List<ChatMessage>
            {
                ChatMessage.System(_currentRules) // Usa as regras personalizadas
            };
            messages.AddRange(_chatHistory.TakeLast(20)); // Últimas 20 mensagens para contexto

            await _deepSeekService.SendMessageStreamAsync(messages, model);
        }
        catch (Exception ex)
        {
            if (_currentStreamingMessage?.Child is TextBlock text)
            {
                text.Text = $"Erro: {ex.Message}";
                text.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }

    /// <summary>
    /// Processa referências de arquivos na mensagem e carrega seus conteúdos
    /// Sintaxes suportadas:
    /// - @arquivo.cs ou @pasta/arquivo.cs (com @)
    /// - #arquivo.cs (com #)
    /// - [arquivo.cs] (entre colchetes)
    /// - `arquivo.cs` (entre crases)
    /// </summary>
    private async Task<(string processedMessage, Dictionary<string, string> loadedFiles)> ProcessFileReferences(string message)
    {
        var loadedFiles = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(_currentWorkspacePath))
            return (message, loadedFiles);

        // Padrões para detectar referências de arquivos
        var patterns = new[]
        {
            @"@([\w\-./\\]+\.\w+)",           // @arquivo.cs ou @pasta/arquivo.cs
            @"#([\w\-./\\]+\.\w+)",           // #arquivo.cs
            @"\[([\w\-./\\]+\.\w+)\]",        // [arquivo.cs]
            @"`([\w\-./\\]+\.\w+)`"           // `arquivo.cs`
        };

        var allMatches = new HashSet<string>();

        foreach (var pattern in patterns)
        {
            var regex = new System.Text.RegularExpressions.Regex(pattern);
            var matches = regex.Matches(message);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    allMatches.Add(match.Groups[1].Value);
                }
            }
        }

        // Carrega cada arquivo encontrado
        var fileContents = new StringBuilder();
        foreach (var fileName in allMatches)
        {
            var filePath = FindFileInWorkspace(fileName);
            if (filePath != null && !loadedFiles.ContainsKey(filePath))
            {
                try
                {
                    var content = await _fileSystemService.ReadFileAsync(filePath);
                    var fileInfo = _fileSystemService.GetFileInfo(filePath);
                    loadedFiles[filePath] = content;

                    fileContents.AppendLine($"\n\n--- ARQUIVO: {Path.GetFileName(filePath)} ({filePath}) ---");
                    fileContents.AppendLine($"```{fileInfo.Language}");
                    fileContents.AppendLine(content);
                    fileContents.AppendLine("```");
                }
                catch
                {
                    // Arquivo não encontrado ou erro ao ler, ignora silenciosamente
                }
            }
        }

        // Se encontrou arquivos, anexa o conteúdo à mensagem
        if (loadedFiles.Count > 0)
        {
            var enhancedMessage = message + "\n\n" + fileContents.ToString();
            return (enhancedMessage, loadedFiles);
        }

        return (message, loadedFiles);
    }

    private async void AnalyzeCode_Click(object sender, RoutedEventArgs e)
    {
        var selectedText = await GetSelectedText();
        var code = string.IsNullOrEmpty(selectedText) ? await GetEditorContent() : selectedText;

        if (string.IsNullOrWhiteSpace(code))
        {
            AddChatMessage("Nenhum código selecionado ou aberto para analisar.", false);
            return;
        }

        var language = txtStatusLanguage.Text;
        AddChatMessage($"Analisando código {language}...", true);
        StartStreamingMessage();

        try
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.System(@"Você é um revisor de código experiente. Analise o código fornecido e:
1. Identifique possíveis bugs ou problemas
2. Sugira melhorias de performance
3. Verifique boas práticas e padrões
4. Proponha refatorações quando apropriado

Seja objetivo e forneça exemplos de código quando sugerir mudanças."),
                ChatMessage.User($"Analise este código {language}:\n\n```{language}\n{code}\n```")
            };

            await _deepSeekService.SendMessageStreamAsync(messages);
        }
        catch (Exception ex)
        {
            if (_currentStreamingMessage?.Child is TextBlock text)
            {
                text.Text = $"Erro: {ex.Message}";
                text.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }

    #region Regras do Chat

    private void EditRules_Click(object sender, RoutedEventArgs e)
    {
        // Toggle visibilidade do painel de regras
        pnlRules.Visibility = pnlRules.Visibility == Visibility.Visible
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    private void SaveRules_Click(object sender, RoutedEventArgs e)
    {
        _currentRules = txtRules.Text;
        pnlRules.Visibility = Visibility.Collapsed;
        AddChatMessage("Regras atualizadas! As novas regras serão usadas nas próximas mensagens.", false);

        // Salva no workspace se existir
        if (!string.IsNullOrEmpty(_currentWorkspacePath))
        {
            try
            {
                var rulesPath = Path.Combine(_currentWorkspacePath, RULES_FILE_NAME);
                File.WriteAllText(rulesPath, _currentRules);
                AddChatMessage($"Regras salvas em {RULES_FILE_NAME}", false);
            }
            catch (Exception ex)
            {
                AddChatMessage($"Erro ao salvar regras: {ex.Message}", false);
            }
        }
    }

    private void LoadRulesFromFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Arquivos Markdown (*.md)|*.md|Arquivos de Texto (*.txt)|*.txt|Todos os arquivos (*.*)|*.*",
            Title = "Selecione o arquivo de regras"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                _currentRules = File.ReadAllText(dialog.FileName);
                txtRules.Text = _currentRules;
                AddChatMessage($"Regras carregadas de {Path.GetFileName(dialog.FileName)}", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar arquivo: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    #endregion

    #region Busca de Arquivos no Workspace

    /// <summary>
    /// Busca um arquivo no workspace pelo nome (parcial ou completo)
    /// </summary>
    private string? FindFileInWorkspace(string searchName)
    {
        if (string.IsNullOrEmpty(_currentWorkspacePath)) return null;

        // Se é um caminho relativo ou absoluto que existe
        if (File.Exists(searchName))
            return searchName;

        var fullPath = Path.Combine(_currentWorkspacePath, searchName);
        if (File.Exists(fullPath))
            return fullPath;

        // Busca recursiva por nome parcial
        try
        {
            var files = Directory.EnumerateFiles(_currentWorkspacePath, $"*{searchName}*", SearchOption.AllDirectories)
                .Where(f => !f.Contains("\\node_modules\\") && !f.Contains("\\bin\\") && !f.Contains("\\obj\\") && !f.Contains("\\.git\\"))
                .ToList();

            if (files.Count == 1)
                return files[0];

            if (files.Count > 1)
            {
                // Prioriza match exato do nome
                var exactMatch = files.FirstOrDefault(f => Path.GetFileName(f).Equals(searchName, StringComparison.OrdinalIgnoreCase));
                if (exactMatch != null)
                    return exactMatch;

                // Retorna o primeiro arquivo encontrado (mais curto caminho)
                return files.OrderBy(f => f.Length).First();
            }
        }
        catch { }

        return null;
    }

    #endregion

    #endregion

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        _deepSeekService.Dispose();
        _gitService.Dispose();
    }
}

/// <summary>
/// Mensagem do editor Monaco para o C#
/// </summary>
public class EditorMessage
{
    public string Type { get; set; } = string.Empty;
    public int Line { get; set; }
    public int Column { get; set; }
    public string? Content { get; set; }
}

/// <summary>
/// Comando relay simples para bindings de teclado
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => _execute(parameter);
}
