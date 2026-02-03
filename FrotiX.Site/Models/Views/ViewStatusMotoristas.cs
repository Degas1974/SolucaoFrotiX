/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewStatusMotoristas.cs                                                               ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de status de motoristas ativos (escala, veículo, posição).                  ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: MotoristaId, Nome, StatusAtual, DataEscala, NumeroSaidas, Placa, Veiculo                 ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Validations                                                                        ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewStatusMotoristas
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar status atual de motoristas ativos
     *
     * 📥 ENTRADAS     : Motorista, escala, veículo, status em tempo real
     *
     * 📤 SAÍDAS       : Registro somente leitura para dashboards de status
     *
     * 🔗 CHAMADA POR  : Tela de status de motoristas
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewStatusMotoristas
    {
        // [DADOS] Identificador único do motorista
        public Guid MotoristaId { get; set; }
        // [DADOS] Nome do motorista
        public string Nome { get; set; }
        // [DADOS] Ponto/matrícula do motorista
        public string? Ponto { get; set; }
        // [DADOS] Status atual (disponível/em viagem/etc)
        public string StatusAtual { get; set; }
        // [DADOS] Data da escala atual
        public DateTime? DataEscala { get; set; }
        // [DADOS] Número de saídas/expedições no dia
        public int NumeroSaidas { get; set; }
        // [DADOS] Placa do veículo designado
        public string? Placa { get; set; }
        // [DADOS] Descrição do veículo
        public string? Veiculo { get; set; }
    }
}
