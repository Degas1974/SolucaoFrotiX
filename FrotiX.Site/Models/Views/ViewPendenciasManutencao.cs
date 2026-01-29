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
    public class ViewPendenciasManutencao
        {

        public Guid ItemManutencaoId { get; set; }

        public Guid ManutencaoId { get; set; }

        public Guid MotoristaId { get; set; }

        public Guid ViagemId { get; set; }

        public Guid VeiculoId { get; set; }

        public string? TipoItem { get; set; }

        public string? NumFicha { get; set; }

        public string? DataItem { get; set; }

        public string? Resumo { get; set; }

        public string? Descricao { get; set; }

        public string? Status { get; set; }

        public string? Nome { get; set; }

        public string? ImagemOcorrencia { get; set; }

        }
    }


