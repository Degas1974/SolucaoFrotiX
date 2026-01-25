using System.Threading.Tasks;

namespace FrotiX.Mobile.Shared.Services.IServices
{
    public interface IAlertaService
    {
        Task Erro(string titulo, string mensagem);
    }
}