/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewItensManutencao.cs                                                                ║
   ║ 📂 CAMINHO: /Models/Views                                                                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: View SQL de itens de manutenção de veículos (OS, status, imagens).                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: ItemManutencaoId, ManutencaoId, TipoItem, NumFicha, Status, ImagemOcorrencia              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations                                                        ║
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
     * ⚡ MODEL: ViewItensManutencao
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de itens de manutenção com status
     *
     * 📥 ENTRADAS     : Manutenção, viagem, motorista, imagens e status
     *
     * 📤 SAÍDAS       : Registro somente leitura para listagens de pendências
     *
     * 🔗 CHAMADA POR  : Telas de manutenção e ocorrências
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewItensManutencao
    {
        // [DADOS] Identificador único do item de manutenção
        public Guid ItemManutencaoId { get; set; }

        // [DADOS] Identificador da manutenção pai
        public Guid ManutencaoId { get; set; }

        // [DADOS] Tipo de item (ocorrência/defeito/etc)
        public string? TipoItem { get; set; }

        // [DADOS] Número de ficha de vistoria
        public string? NumFicha { get; set; }

        // [DADOS] Data do item (formatada)
        public string? DataItem { get; set; }

        // [DADOS] Resumo do problema
        public string? Resumo { get; set; }

        // [DADOS] Descrição detalhada
        public string? Descricao { get; set; }

        // [DADOS] Status atual (pendente/resolvido/etc)
        public string? Status { get; set; }

        // [DADOS] URL/blob de imagem da ocorrência
        public string? ImagemOcorrencia { get; set; }

        // [DADOS] Nome do motorista que registrou
        public string? NomeMotorista { get; set; }

        // [DADOS] Identificador do motorista (FK)
        public Guid? MotoristaId { get; set; }

        // [DADOS] Identificador da viagem (FK)
        public Guid? ViagemId { get; set; }
    }
}
