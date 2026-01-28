// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ToastMessage.cs                                                    ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo para mensagens toast (notificações visuais temporárias).             ║
// ║ Usado pelo ToastService e AppToast para feedback ao usuário.                ║
// ║                                                                              ║
// ║ CLASSE ToastMessage:                                                         ║
// ║ - Texto: Mensagem a ser exibida                                             ║
// ║ - Cor: Cor do toast (Verde, Vermelho, Laranja)                              ║
// ║ - Duracao: Tempo de exibição em ms (padrão: 2000)                           ║
// ║                                                                              ║
// ║ ENUM ToastColor: Verde (sucesso), Vermelho (erro), Laranja (aviso)          ║
// ║                                                                              ║
// ║ CONSTRUTOR: ToastMessage(texto, cor = "Verde", duracao = 2000)              ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

namespace FrotiX.Models
{
    public class ToastMessage
    {
        public string Texto
        {
            get; set;
        }
        public string Cor
        {
            get; set;
        }
        public int Duracao
        {
            get; set;
        }

        public ToastMessage(string texto , string cor = "Verde" , int duracao = 2000)
        {
            Texto = texto;
            Cor = cor;
            Duracao = duracao;
        }
    }

    public enum ToastColor
    {
        Verde,
        Vermelho,
        Laranja
    }
}
