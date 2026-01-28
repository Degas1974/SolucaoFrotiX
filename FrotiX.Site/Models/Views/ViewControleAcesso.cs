// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewControleAcesso.cs                                              ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo para VIEW de controle de acesso usuário x recurso.                   ║
// ║ Usado na tela de gestão de permissões de usuários.                          ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - UsuarioId, RecursoId: Chaves do relacionamento                            ║
// ║ - Acesso: true = tem acesso, false = sem acesso                             ║
// ║ - Nome: Nome do recurso/menu                                                ║
// ║ - Descricao: Descrição do recurso                                           ║
// ║ - Ordem: Ordem de exibição                                                  ║
// ║ - NomeCompleto: Nome completo hierárquico                                   ║
// ║ - IDS: IDs concatenados para identificação                                  ║
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
    public class ViewControleAcesso
        {

        public string? UsuarioId { get; set; }

        public Guid RecursoId { get; set; }

        public bool Acesso { get; set; }

        public string? Nome { get; set; }

        public string? Descricao { get; set; }

        public double? Ordem { get; set; }

        public string? NomeCompleto { get; set; }

        public string? IDS { get; set; }

        }
    }


