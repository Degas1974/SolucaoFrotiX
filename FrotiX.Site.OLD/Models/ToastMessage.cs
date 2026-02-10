/* ****************************************************************************************
 * ⚡ ARQUIVO: ToastMessage.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar mensagens toast para notificações visuais.
 *
 * 📥 ENTRADAS     : Texto, cor e duração.
 *
 * 📤 SAÍDAS       : DTO para exibição de toasts.
 *
 * 🔗 CHAMADA POR  : Controllers/Views de feedback ao usuário.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : Nenhuma.
 **************************************************************************************** */

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ DTO: ToastMessage
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar dados de uma notificação toast.
     *
     * 📥 ENTRADAS     : Texto, cor e duração.
     *
     * 📤 SAÍDAS       : Mensagem pronta para UI.
     *
     * 🔗 CHAMADA POR  : Camadas de UI e serviços de feedback.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ToastMessage
    {
        // Texto exibido na notificação.
        public string Texto
        {
            get; set;
        }
        // Cor da notificação.
        public string Cor
        {
            get; set;
        }
        // Duração em ms.
        public int Duracao
        {
            get; set;
        }

        /****************************************************************************************
         * ⚡ CONSTRUTOR: ToastMessage
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar mensagem toast com valores padrão.
         *
         * 📥 ENTRADAS     : texto, cor (opcional), duracao (opcional).
         *
         * 📤 SAÍDAS       : Instância preenchida.
         *
         * 🔗 CHAMADA POR  : Controllers/Views.
         *
         * 🔄 CHAMA        : Não se aplica.
         ****************************************************************************************/
        public ToastMessage(string texto , string cor = "Verde" , int duracao = 2000)
        {
            Texto = texto;
            Cor = cor;
            Duracao = duracao;
        }
    }

    /****************************************************************************************
     * ⚡ ENUM: ToastColor
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Definir cores suportadas por toasts.
     *
     * 📥 ENTRADAS     : Seleção de cor.
     *
     * 📤 SAÍDAS       : Enum para padronização.
     *
     * 🔗 CHAMADA POR  : UI e validações.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public enum ToastColor
    {
        Verde,
        Vermelho,
        Laranja
    }
}
