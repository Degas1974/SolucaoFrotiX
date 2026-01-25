using Microsoft.JSInterop;
using System.Threading.Tasks;

public static class JSRuntimeExtensions
{
    public static ValueTask Aviso(this IJSRuntime js, string titulo, string? mensagem = null, string botao = "OK")
    {
        return js.InvokeVoidAsync("SweetAlertInterop.ShowWarning", titulo, mensagem ?? "", botao);
    }

    public static ValueTask Erro(this IJSRuntime js, string titulo, string? mensagem = null, string botao = "Fechar")
    {
        return js.InvokeVoidAsync("SweetAlertInterop.ShowError", titulo, mensagem ?? "", botao);
    }

    public static ValueTask Sucesso(this IJSRuntime js, string titulo, string? mensagem = null, string botao = "OK")
    {
        return js.InvokeVoidAsync("SweetAlertInterop.ShowSuccess", titulo, mensagem ?? "", botao);
    }

    public static ValueTask Info(this IJSRuntime js, string titulo, string? mensagem = null, string botao = "OK")
    {
        return js.InvokeVoidAsync("SweetAlertInterop.ShowInfo", titulo, mensagem ?? "", botao);
    }
}
