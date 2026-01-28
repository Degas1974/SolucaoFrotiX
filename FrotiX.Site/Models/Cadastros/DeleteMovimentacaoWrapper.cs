// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: DeleteMovimentacaoWrapper.cs                                       ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Classe Wrapper para operações de exclusão de movimentações financeiras.     ║
// ║ Agrupa ViewModels de Empenho e EmpenhoMulta para exclusão unificada.        ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • mEmpenho - ViewModel de MovimentacaoEmpenho para exclusão                 ║
// ║ • mEmpenhoMulta - ViewModel de MovimentacaoEmpenhoMulta para exclusão       ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Tela de exclusão de movimentações de empenho                              ║
// ║ • Permite excluir movimentação de empenho ou de multa em uma única ação     ║
// ║                                                                              ║
// ║ PADRÃO:                                                                       ║
// ║ • Wrapper Pattern para unificar operações em ViewModels distintos           ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
