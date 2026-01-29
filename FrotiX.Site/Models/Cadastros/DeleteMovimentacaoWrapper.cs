/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Cadastros/DeleteMovimentacaoWrapper.cs                  ║
 * ║  Descrição: Wrapper para operações de exclusão de movimentações          ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;


//Classe Wrapper para movimentação de Delete de Empenho e de EmpenhoMulta

namespace FrotiX.Models
    {
    public class DeleteMovimentacaoWrapperViewModel
        {
        public MovimentacaoEmpenhoViewModel mEmpenho { get; set; }
        public MovimentacaoEmpenhoMultaViewModel mEmpenhoMulta { get; set; }
        }
    }
