/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewOcorrencia.cs                                                                     ║
   ║ 📂 CAMINHO: /Models/Views                                                                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: View SQL de ocorrências do sistema (resumo, status, veículo e motorista).            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: VeiculoId, ViagemId, NoFichaVistoria, DataInicial, ResumoOcorrencia, StatusOcorrencia    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations                                                       ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewOcorrencia
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de ocorrências de viagem
     *
     * 📥 ENTRADAS     : Viagem, veículo, motorista, ocorrência, item manutenção
     *
     * 📤 SAÍDAS       : Registro somente leitura para dashboards de problemas
     *
     * 🔗 CHAMADA POR  : Telas de ocorrências e relatórios
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewOcorrencia
    {
        // [DADOS] Identificador do veículo
        public Guid VeiculoId { get; set; }

        // [DADOS] Identificador da viagem
        public Guid ViagemId { get; set; }

        // [DADOS] Número da ficha de vistoria
        public int? NoFichaVistoria { get; set; }

        // [DADOS] Data inicial da viagem (formatada)
        public string? DataInicial { get; set; }

        // [DADOS] Nome do motorista
        public string? NomeMotorista { get; set; }

        // [DADOS] Descrição completa do veículo
        public string? DescricaoVeiculo { get; set; }

        // [DADOS] Resumo da ocorrência
        public string? ResumoOcorrencia { get; set; }

        // [DADOS] Status da ocorrência (aberta/resolvida/etc)
        public string? StatusOcorrencia { get; set; }

        // [DADOS] Identificador do motorista (FK)
        public Guid? MotoristaId { get; set; }

        // [DADOS] URL/blob de imagem da ocorrência
        public string? ImagemOcorrencia { get; set; }

        // [DADOS] Identificador do item de manutenção (FK)
        public Guid? ItemManutencaoId { get; set; }

        // [DADOS] Descrição detalhada da ocorrência
        public string? DescricaoOcorrencia { get; set; }

        // [DADOS] Descrição da solução implementada
        public string? DescricaoSolucaoOcorrencia { get; set; }
    }
}

