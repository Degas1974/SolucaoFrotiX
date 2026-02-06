/* ****************************************************************************************
 * ⚡ ARQUIVO: SmartSettings.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Definir configurações de tema, recursos e app settings do FrotiX.
 *
 * 📥 ENTRADAS     : Valores de configuração de tema e features.
 *
 * 📤 SAÍDAS       : Objetos de configuração consumidos pela UI.
 *
 * 🔗 CHAMADA POR  : Startup e componentes de layout.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : Nenhuma.
 **************************************************************************************** */

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ MODEL: Theme
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Armazenar configurações de tema e branding.
     *
     * 📥 ENTRADAS     : Logo, usuário padrão e informações visuais.
     *
     * 📤 SAÍDAS       : Configuração de tema para a UI.
     *
     * 🔗 CHAMADA POR  : SmartSettings.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
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

    /****************************************************************************************
     * ⚡ MODEL: Features
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Controlar flags de recursos habilitados no app.
     *
     * 📥 ENTRADAS     : Flags de UI e integrações.
     *
     * 📤 SAÍDAS       : Configuração de recursos ativos.
     *
     * 🔗 CHAMADA POR  : SmartSettings.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
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

    /****************************************************************************************
     * ⚡ MODEL: SmartSettings
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Centralizar configurações do aplicativo.
     *
     * 📥 ENTRADAS     : Dados de versão, app e tema.
     *
     * 📤 SAÍDAS       : Configurações para uso pela aplicação.
     *
     * 🔗 CHAMADA POR  : Startup e serviços de configuração.
     *
     * 🔄 CHAMA        : Theme, Features.
     ****************************************************************************************/
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

    /****************************************************************************************
     * ⚡ MODEL: SmartError
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar erros estruturados da aplicação.
     *
     * 📥 ENTRADAS     : Lista de mensagens de erro.
     *
     * 📤 SAÍDAS       : Payload de erro para UI/serviços.
     *
     * 🔗 CHAMADA POR  : Serviços de validação e UI.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class SmartError
        {
        // Lista de erros.
        public string[][] Errors { get; set; } = { };

        /****************************************************************************************
         * ⚡ MÉTODO: Failed
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar instância de erro com mensagens.
         *
         * 📥 ENTRADAS     : errors.
         *
         * 📤 SAÍDAS       : SmartError preenchido.
         *
         * 🔗 CHAMADA POR  : Fluxos de validação.
         *
         * 🔄 CHAMA        : Não se aplica.
         ****************************************************************************************/
        public static SmartError Failed(params string[] errors) => new SmartError { Errors = new[] { errors } };
        }
    }
