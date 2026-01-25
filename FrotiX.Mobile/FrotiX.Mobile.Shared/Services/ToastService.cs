using System.Threading.Tasks;
using FrotiX.Mobile.Shared.Services.IServices;
using Microsoft.JSInterop;

namespace FrotiX.Mobile.Shared.Services
{
    public class ToastService : IToastService
    {
        private readonly IJSRuntime _js;

        public ToastService(IJSRuntime js) => _js = js;

        public Task ShowSuccess(string message) =>
            _js.InvokeVoidAsync("SweetAlertInterop.ShowToast", "success", message, 3000).AsTask();

        public Task ShowError(string message) =>
            _js.InvokeVoidAsync("SweetAlertInterop.ShowToast", "error", message, 4000).AsTask();

        public Task ShowInfo(string message) =>
            _js.InvokeVoidAsync("SweetAlertInterop.ShowToast", "info", message, 3000).AsTask();
    }
}
