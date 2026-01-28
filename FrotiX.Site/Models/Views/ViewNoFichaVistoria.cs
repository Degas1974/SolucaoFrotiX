// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewNoFichaVistoria.cs                                             ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model simples para obtenção do próximo número de ficha de vistoria.    ║
// ║ Usado para gerar números sequenciais de fichas de vistoria de veículos.     ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • NoFichaVistoria - Próximo número sequencial de ficha disponível           ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Consulta ao banco para obter MAX(NoFichaVistoria) + 1                     ║
// ║ • Usado ao criar nova ficha de vistoria                                     ║
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
    public class ViewNoFichaVistoria
        {

        public int? NoFichaVistoria { get; set; }

        }
    }


