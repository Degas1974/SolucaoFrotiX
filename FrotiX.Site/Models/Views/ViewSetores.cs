/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewSetores.cs                                                                        ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de setores solicitantes (hierarquia de setores).                             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: SetorSolicitanteId, Nome, SetorPaiId                                                     ║
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
     * ⚡ MODEL: ViewSetores
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar setores solicitantes com hierarquia
     *
     * 📥 ENTRADAS     : Setor, nome, setor pai
     *
     * 📤 SAÍDAS       : Registro somente leitura para árvores e dropdowns
     *
     * 🔗 CHAMADA POR  : Formulários de viagem e filtros
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewSetores
    {
        // [DADOS] Identificador único do setor
        public Guid SetorSolicitanteId { get; set; }

        // [DADOS] Nome do setor
        public string? Nome { get; set; }

        // [DADOS] Identificador do setor pai (FK nullable - para hierarquia)
        public Guid? SetorPaiId { get; set; }
    }
}
