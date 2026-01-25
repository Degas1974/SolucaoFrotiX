// Services/AlertaJs.cs
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace FrotiX.Mobile.Shared.Services
{
    public sealed class AlertaJs
    {
        private readonly IJSRuntime _js;

        public AlertaJs(IJSRuntime js) => _js = js;

        public ValueTask Erro(string titulo, string texto, string confirm = "OK") =>
            _js.InvokeVoidAsync("Alerta.Erro", titulo, texto, confirm);

        public ValueTask Sucesso(string titulo, string texto, string confirm = "OK") =>
            _js.InvokeVoidAsync("Alerta.Sucesso", titulo, texto, confirm);

        public ValueTask Info(string titulo, string texto, string confirm = "OK") =>
            _js.InvokeVoidAsync("Alerta.Info", titulo, texto, confirm);

        public ValueTask Alerta(string titulo, string texto, string confirm = "OK") =>
            _js.InvokeVoidAsync("Alerta.Alerta", titulo, texto, confirm);

        public ValueTask<bool> Confirmar(
            string titulo,
            string texto,
            string confirm = "Sim",
            string cancel = "Cancelar"
        ) => _js.InvokeAsync<bool>("Alerta.Confirmar", titulo, texto, confirm, cancel);

        public ValueTask ErroNaoTratado(string classe, string metodo, object erro) =>
            _js.InvokeVoidAsync("SweetAlertInterop.ShowErrorUnexpected", classe, metodo, erro);

        /// <summary>
        /// Tratamento de erro padrão com informação de linha do código.
        /// Exibe alerta de erro com detalhes do método e linha onde ocorreu.
        /// </summary>
        public async Task TratamentoErroComLinha(
            Exception ex,
            [CallerMemberName] string metodo = "",
            [CallerLineNumber] int linha = 0,
            [CallerFilePath] string arquivo = "")
        {
            var nomeArquivo = System.IO.Path.GetFileNameWithoutExtension(arquivo);
            var mensagem = $"Erro em {nomeArquivo}.{metodo} (linha {linha}):\n{ex.Message}";
            
            await _js.InvokeVoidAsync("Alerta.Erro", "Erro", mensagem, "OK");
        }

        /// <summary>
        /// Tratamento de erro com mensagem customizada.
        /// </summary>
        public async Task TratamentoErroComLinha(
            string mensagemCustom,
            Exception ex,
            [CallerMemberName] string metodo = "",
            [CallerLineNumber] int linha = 0,
            [CallerFilePath] string arquivo = "")
        {
            var nomeArquivo = System.IO.Path.GetFileNameWithoutExtension(arquivo);
            var mensagem = $"{mensagemCustom}\n\nDetalhes: {nomeArquivo}.{metodo} (linha {linha}):\n{ex.Message}";
            
            await _js.InvokeVoidAsync("Alerta.Erro", "Erro", mensagem, "OK");
        }
    }
}
