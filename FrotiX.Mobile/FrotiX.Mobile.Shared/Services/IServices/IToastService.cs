using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrotiX.Mobile.Shared.Services.IServices
{
    public interface IToastService
    {
        Task ShowSuccess(string message);
        Task ShowError(string message);
        Task ShowInfo(string message);
    }
}

