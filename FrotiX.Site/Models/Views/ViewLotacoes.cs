// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewLotacoes.cs                                                    ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para listagem de lotações de motoristas em unidades.             ║
// ║ Exibe histórico de alocações com status atual de lotação.                   ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • LotacaoMotoristaId - Identificador único da lotação                       ║
// ║ • MotoristaId - Referência ao motorista                                     ║
// ║ • UnidadeId - Referência à unidade de lotação                               ║
// ║ • NomeCategoria - Categoria do motorista (titular, reserva, etc)            ║
// ║ • Unidade - Nome da unidade para exibição                                   ║
// ║ • Motorista - Nome do motorista para exibição                               ║
// ║ • DataInicio - Data de início da lotação (formatada)                        ║
// ║ • Lotado - Flag indicando se está atualmente lotado                         ║
// ║                                                                              ║
// ║ RELACIONAMENTOS:                                                             ║
// ║ • LotacaoMotorista - Entidade de origem dos dados                           ║
// ║ • Motorista, Unidade - Entidades relacionadas                               ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
    public class ViewLotacoes
        {

        public Guid LotacaoMotoristaId { get; set; }

        public Guid MotoristaId { get; set; }

        public Guid UnidadeId { get; set; }

        public string? NomeCategoria { get; set; }

        public string? Unidade { get; set; }

        public string? Motorista { get; set; }

        public string? DataInicio { get; set; }

        public bool Lotado { get; set; }
        }
    }


