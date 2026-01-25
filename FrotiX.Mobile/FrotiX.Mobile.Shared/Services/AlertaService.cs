using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using FrotiX.Mobile.Shared.Services.IServices;

namespace FrotiX.Mobile.Shared.Services
{
    public class AlertaService : IAlertaService
    {
        private readonly IJSRuntime _js;

        public AlertaService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task Erro(string titulo, string mensagem)
        {
            try
            {
                await _js.InvokeVoidAsync("SweetAlertInterop.ShowError", titulo, mensagem);
            }
            catch (Exception ex)
            {
                // NÃO usamos Alerta.Erro aqui para evitar loop infinito
                await _js.InvokeVoidAsync("alert", $"⚠️ Erro ao exibir alerta original: {ex.Message}");
            }
        }
    }
}
