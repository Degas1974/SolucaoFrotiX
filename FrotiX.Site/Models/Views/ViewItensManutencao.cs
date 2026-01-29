/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Views/ViewItensManutencao.cs                            ║
 * ║  Descrição: Modelo mapeado da View de itens de manutenção de veículos    ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
    public class ViewItensManutencao
    {
        public Guid ItemManutencaoId { get; set; }

        public Guid ManutencaoId { get; set; }

        public string? TipoItem { get; set; }

        public string? NumFicha { get; set; }

        public string? DataItem { get; set; }

        public string? Resumo { get; set; }

        public string? Descricao { get; set; }

        public string? Status { get; set; }

        public string? ImagemOcorrencia { get; set; }

        public string? NomeMotorista { get; set; }

        public Guid? MotoristaId { get; set; }

        public Guid? ViagemId { get; set; }
    }
}
