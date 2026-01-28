// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewSetores.cs                                                     ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para listagem hierárquica de setores solicitantes.               ║
// ║ Suporta estrutura de árvore com setores pai-filho.                          ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • SetorSolicitanteId - Identificador único do setor                         ║
// ║ • Nome - Nome do setor para exibição                                        ║
// ║ • SetorPaiId - Referência ao setor pai (hierarquia)                         ║
// ║                                                                              ║
// ║ REGRAS DE NEGÓCIO:                                                           ║
// ║ • SetorPaiId null indica setor raiz                                         ║
// ║ • Permite construção de árvore de setores (TreeView)                        ║
// ║ • Usado em filtros e relatórios por estrutura organizacional                ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
    public class ViewSetores
    {
        public Guid SetorSolicitanteId { get; set; }

        public string? Nome { get; set; }

        public Guid? SetorPaiId { get; set; }
    }
}
