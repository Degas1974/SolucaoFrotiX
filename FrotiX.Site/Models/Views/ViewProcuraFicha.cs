// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewProcuraFicha.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para busca de fichas de vistoria por critérios diversos.         ║
// ║ Usado em telas de consulta para localizar fichas existentes.                ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • MotoristaId - Filtro por motorista                                        ║
// ║ • VeiculoId - Filtro por veículo                                            ║
// ║                                                                              ║
// ║ Período:                                                                      ║
// ║ • DataInicial, DataFinal - Intervalo de datas (DateTime)                    ║
// ║ • HoraInicio, HoraFim - Horários de início e fim                            ║
// ║                                                                              ║
// ║ Resultado:                                                                    ║
// ║ • NoFichaVistoria - Número da ficha encontrada                              ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Parâmetros de busca para localização de fichas de vistoria                ║
// ║ • Retorna dados da viagem que corresponde aos critérios                     ║
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
    public class ViewProcuraFicha
        {

        public Guid MotoristaId { get; set; }

        public Guid VeiculoId { get; set; }

        public DateTime? DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public string? HoraInicio { get; set; }

        public string? HoraFim { get; set; }

        public int? NoFichaVistoria { get; set; }

        }
    }


