/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: SmartSettings.cs                                                                        ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Definir configurações de tema, recursos e app settings do FrotiX.                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: Theme, Features, SmartSettings, SmartError                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Nenhuma                                                                            ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

namespace FrotiX.Models
    {
    // ==================================================================================================
    // TEMA
    // ==================================================================================================
    public class Theme
        {
        // Versão do tema.
        public string ThemeVersion { get; set; }
        // Prefixo de ícones (FontAwesome).
        public string IconPrefix { get; set; }
        // Logo do sistema.
        public string Logo { get; set; }
        // Nome do usuário padrão.
        public string User { get; set; }
        // Perfil padrão.
        public string Role { get; set; } = "Administrator";
        // Email padrão.
        public string Email { get; set; }
        // Twitter/contato social.
        public string Twitter { get; set; }
        // Avatar do usuário.
        public string Avatar { get; set; }
        }

    // ==================================================================================================
    // FEATURES
    // ==================================================================================================
    public class Features
        {
        // Habilita sidebar.
        public bool AppSidebar { get; set; }
        // Habilita header.
        public bool AppHeader { get; set; }
        // Atalho de layout.
        public bool AppLayoutShortcut { get; set; }
        // Habilita footer.
        public bool AppFooter { get; set; }
        // Atalho de menu.
        public bool ShortcutMenu { get; set; }
        // Google Analytics.
        public bool GoogleAnalytics { get; set; }
        // Interface de chat.
        public bool ChatInterface { get; set; }
        // Configurações de layout.
        public bool LayoutSettings { get; set; }
        }

    // ==================================================================================================
    // SETTINGS
    // ==================================================================================================
    public class SmartSettings
        {
        // Nome da seção no appsettings.
        public const string SectionName = nameof(SmartSettings);

        // Versão do app.
        public string Version { get; set; }
        // Identificador do app.
        public string App { get; set; }
        // Nome exibido.
        public string AppName { get; set; }
        // Flavor do app.
        public string AppFlavor { get; set; }
        // Subscript do flavor.
        public string AppFlavorSubscript { get; set; }
        // Configurações de tema.
        public Theme Theme { get; set; }
        // Configurações de features.
        public Features Features { get; set; }
        }

    // ==================================================================================================
    // ERRO
    // ==================================================================================================
    public class SmartError
        {
        // Lista de erros.
        public string[][] Errors { get; set; } = { };

        // Factory para retorno de erro simples.
        public static SmartError Failed(params string[] errors) => new SmartError { Errors = new[] { errors } };
        }
    }

