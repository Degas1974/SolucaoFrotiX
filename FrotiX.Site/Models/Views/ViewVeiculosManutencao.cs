/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewVeiculosManutencao.cs                                                             ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de veículos em manutenção (descrição e identificador).                       ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: Descricao, VeiculoId                                                                     ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: System                                                                                     ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;

namespace FrotiX.Models
    {
    public class ViewVeiculosManutencao
        {
        public String? Descricao { get; set; }
        public Guid VeiculoId { get; set; }
        }
    }

