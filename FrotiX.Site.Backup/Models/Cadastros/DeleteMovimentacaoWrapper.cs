/* ****************************************************************************************
 * ⚡ ARQUIVO: DeleteMovimentacaoWrapper.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Agrupar movimentações para exclusão de Empenho e EmpenhoMulta.
 *
 * 📥 ENTRADAS     : ViewModels de movimentação de empenhos e multas.
 *
 * 📤 SAÍDAS       : Wrapper utilizado em telas/fluxos de exclusão.
 *
 * 🔗 CHAMADA POR  : Controllers/Views de exclusão.
 *
 * 🔄 CHAMA        : MovimentacaoEmpenhoViewModel, MovimentacaoEmpenhoMultaViewModel.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ VIEWMODEL: DeleteMovimentacaoWrapperViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Agrupar movimentações de Empenho e EmpenhoMulta para exclusão.
     *
     * 📥 ENTRADAS     : mEmpenho e mEmpenhoMulta.
     *
     * 📤 SAÍDAS       : Wrapper para operação conjunta na UI.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de exclusão.
     *
     * 🔄 CHAMA        : MovimentacaoEmpenhoViewModel, MovimentacaoEmpenhoMultaViewModel.
     ****************************************************************************************/
    public class DeleteMovimentacaoWrapperViewModel
        {
        public MovimentacaoEmpenhoViewModel mEmpenho { get; set; }
        public MovimentacaoEmpenhoMultaViewModel mEmpenhoMulta { get; set; }
        }
    }
