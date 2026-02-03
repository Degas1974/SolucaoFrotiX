/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewLotacaoMotorista.cs                                                                ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de lotações de motoristas (unidade, período, motivo).                        ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: UnidadeId, LotacaoMotoristaId, MotoristaId, Lotado, Motivo, DataInicial/Fim               ║
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
     * ⚡ MODEL: ViewLotacaoMotorista
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de lotações de motoristas por unidade
     *
     * 📥 ENTRADAS     : Motorista, unidade, datas, motivo de lotação
     *
     * 📤 SAÍDAS       : Registro somente leitura para controle de lotação
     *
     * 🔗 CHAMADA POR  : Telas de escala e gestão de motoristas
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewLotacaoMotorista
    {
        // [DADOS] Identificador da unidade
        public Guid UnidadeId { get; set; }

        // [DADOS] Identificador único da lotação
        public Guid LotacaoMotoristaId { get; set; }

        // [DADOS] Identificador do motorista
        public Guid MotoristaId { get; set; }

        // [DADOS] Flag indicando se motorista está lotado
        public bool Lotado { get; set; }

        // [DADOS] Motivo da lotação (férias/licença/etc)
        public string? Motivo { get; set; }

        // [DADOS] Nome da unidade
        public string? Unidade { get; set; }

        // [DADOS] Data inicial da lotação (formatada)
        public string? DataInicial { get; set; }

        // [DADOS] Data final da lotação (formatada)
        public string? DataFim { get; set; }

        // [DADOS] Motorista que cobre a lotação
        public string? MotoristaCobertura { get; set; }
    }
}


