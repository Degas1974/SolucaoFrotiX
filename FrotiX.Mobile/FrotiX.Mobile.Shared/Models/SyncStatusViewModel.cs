using System;

namespace FrotiX.Mobile.Shared.Models
{
    /// <summary>
    /// ViewModel para status de sincronização
    /// </summary>
    public class SyncStatusViewModel
    {
        public int TotalVeiculos { get; set; }
        public int TotalMotoristas { get; set; }
        public DateTime? UltimaSyncVeiculos { get; set; }
        public DateTime? UltimaSyncMotoristas { get; set; }
        public bool DadosDisponiveis { get; set; }
        public string? MensagemStatus { get; set; }
    }
}
