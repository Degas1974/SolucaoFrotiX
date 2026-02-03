/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewPendenciasManutencao.cs                                                           ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de pendências de manutenção (itens, veículo, viagem).                       ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: ItemManutencaoId, ManutencaoId, VeiculoId, ViagemId, TipoItem, DataItem, Status          ║
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
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewPendenciasManutencao
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar pendências de manutenção não resolvidas
     *
     * 📥 ENTRADAS     : Manutenção, viagem, veículo, status pendente
     *
     * 📤 SAÍDAS       : Registro somente leitura para dashboards de pendências
     *
     * 🔗 CHAMADA POR  : Telas de pendências e alertas
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewPendenciasManutencao
    {
        // [DADOS] Identificador único do item
        public Guid ItemManutencaoId { get; set; }

        // [DADOS] Identificador da manutenção pai
        public Guid ManutencaoId { get; set; }

        // [DADOS] Identificador do motorista que registrou
        public Guid MotoristaId { get; set; }

        // [DADOS] Identificador da viagem associada
        public Guid ViagemId { get; set; }

        // [DADOS] Identificador do veículo
        public Guid VeiculoId { get; set; }

        // [DADOS] Tipo de item (defeito/ocorrência/etc)
        public string? TipoItem { get; set; }

        // [DADOS] Número de ficha de vistoria
        public string? NumFicha { get; set; }

        // [DADOS] Data do registro (formatada)
        public string? DataItem { get; set; }

        // [DADOS] Resumo do problema
        public string? Resumo { get; set; }

        // [DADOS] Descrição detalhada
        public string? Descricao { get; set; }

        // [DADOS] Status (pendente/aguardando/etc)
        public string? Status { get; set; }

        // [DADOS] Nome do motorista/responsável
        public string? Nome { get; set; }

        // [DADOS] URL/blob de imagem
        public string? ImagemOcorrencia { get; set; }
    }
}


