/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewLotacoes.cs                                                                       ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de lotações (setores/unidades) para listagens.                               ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: LotacaoMotoristaId, MotoristaId, UnidadeId, Unidade, DataInicio, Lotado                  ║
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
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewLotacoes
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de lotações consolidadas
     *
     * 📥 ENTRADAS     : Motorista, unidade, categoria, data e status
     *
     * 📤 SAÍDAS       : Registro somente leitura para listagens
     *
     * 🔗 CHAMADA POR  : Consultas de lotação e dashboards
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewLotacoes
    {
        // [DADOS] Identificador único da lotação
        public Guid LotacaoMotoristaId { get; set; }

        // [DADOS] Identificador do motorista
        public Guid MotoristaId { get; set; }

        // [DADOS] Identificador da unidade
        public Guid UnidadeId { get; set; }

        // [DADOS] Nome da categoria do motorista
        public string? NomeCategoria { get; set; }

        // [DADOS] Nome da unidade
        public string? Unidade { get; set; }

        // [DADOS] Nome do motorista
        public string? Motorista { get; set; }

        // [DADOS] Data de início da lotação (formatada)
        public string? DataInicio { get; set; }

        // [DADOS] Flag indicando se motorista está lotado
        public bool Lotado { get; set; }
    }
}


