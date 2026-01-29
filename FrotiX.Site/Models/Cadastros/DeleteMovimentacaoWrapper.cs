/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: DeleteMovimentacaoWrapper.cs                                                            ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Wrapper para operações de exclusão de movimentações de Empenho e EmpenhoMulta.        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES: DeleteMovimentacaoWrapperViewModel (mEmpenho, mEmpenhoMulta)                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Validations, MovimentacaoEmpenhoViewModel, MovimentacaoEmpenhoMultaViewModel        ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
