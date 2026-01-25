// Components/SweetAlertErrorBoundary.cs
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

public sealed class SweetAlertErrorBoundary : ErrorBoundary
{
    [Inject]
    IJSRuntime JS { get; set; } = default!;

    protected override async Task OnErrorAsync(Exception exception)
    {
        // Envie informações úteis para seu modal "Unexpected"
        var classe = exception.TargetSite?.DeclaringType?.Name ?? "Componente";
        var metodo = exception.TargetSite?.Name ?? "Render/Evento";

        // Chama sua função do JS (já existente no sweetalert_interop_058.js)
        await JS.InvokeVoidAsync(
            "SweetAlertInterop.ShowErrorUnexpected",
            classe,
            metodo,
            new { message = exception.Message, stack = exception.StackTrace }
        );
    }
}
