using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers
{
/*
 * =======================================================================================
 * (IA) 📄 CARD DE IDENTIDADE DO ARQUIVO
 * =======================================================================================
 * 🆔 Nome: AtaRegistroPrecosController.cs
 * 📍 Local: Controllers
 * ❓ Por que existo? API para gestão de Atas de Registro de Preços, Repactuações e
 *                      Itens vinculados (Veículos).
 * 🔗 Relevância: Alta (Gestão de Contratos/Atas)
 * =======================================================================================
 */

    [Route("api/[controller]")]
    [ApiController]
    public partial class AtaRegistroPrecosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AtaRegistroPrecosController (Constructor)                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de Atas com UnitOfWork e serviço de log.         ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita operações de gestão de atas com rastreabilidade.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): acesso a repositórios.                         ║
        /// ║    • log (ILogService): log centralizado.                                    ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Tipo: N/A                                                               ║
        /// ║    • Significado: N/A                                                        ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Injeção de dependência ao instanciar o controller.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Program.cs                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public AtaRegistroPrecosController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs", "AtaRegistroPrecosController", ex);
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get                                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista Atas com fornecedores e contagem de itens/veículos relacionados.    ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Fornece base para grid administrativo de atas.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON formatado para grid.                                ║
        /// ║    • Consumidor: UI de Atas de Registro de Preços.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.AtaRegistroPrecos.GetAll()                                   ║
        /// ║    • _unitOfWork.Fornecedor.GetAll()                                          ║
        /// ║    • _unitOfWork.ItemVeiculoAta.GetAll()                                      ║
        /// ║    • _unitOfWork.VeiculoAta.GetAll()                                          ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/AtaRegistroPrecos                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Contratos/Atas                                           ║
        /// ║    • Arquivos relacionados: Pages/AtaRegistroPrecos/*.cshtml                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Consulta e consolidação de atas
                var result = (
                    from a in _unitOfWork.AtaRegistroPrecos.GetAll()
                    join f in _unitOfWork.Fornecedor.GetAll()
                        on a.FornecedorId equals f.FornecedorId
                    orderby a.AnoAta descending
                    select new
                    {
                        AtaCompleta = a.AnoAta + "/" + a.NumeroAta,
                        ProcessoCompleto = a.NumeroProcesso
                            + "/"
                            + a.AnoProcesso.ToString().Substring(2, 2),
                        a.Objeto,
                        f.DescricaoFornecedor,
                        Periodo = a.DataInicio?.ToString("dd/MM/yy")
                            + " a "
                            + a.DataFim?.ToString("dd/MM/yy"),
                        ValorFormatado = a.Valor?.ToString("C"),
                        a.Status,
                        a.AtaId,
                        depItens = _unitOfWork.ItemVeiculoAta.GetAll(i => i.RepactuacaoAta.AtaId == a.AtaId).Count(),
                        depVeiculos = _unitOfWork.VeiculoAta.GetAll(v => v.AtaId == a.AtaId).Count()
                    }
                ).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AtaRegistroPrecosController] Erro em Get: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs", "Get", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete                                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove uma Ata e suas dependências, se não houver veículos associados.    ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Preserva integridade referencial e evita exclusões inválidas.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (AtaRegistroPrecosViewModel): contém o ID da Ata.                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: status da exclusão.                                      ║
        /// ║    • Consumidor: UI de Atas de Registro de Preços.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault()                        ║
        /// ║    • _unitOfWork.VeiculoAta.GetFirstOrDefault()                               ║
        /// ║    • _unitOfWork.RepactuacaoAta.GetAll()                                      ║
        /// ║    • _unitOfWork.ItemVeiculoAta.GetAll()                                      ║
        /// ║    • _unitOfWork.Save()                                                      ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/AtaRegistroPrecos/Delete                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Contratos/Atas                                           ║
        /// ║    • Arquivos relacionados: Pages/AtaRegistroPrecos/*.cshtml                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(AtaRegistroPrecosViewModel model)
        {
            try
            {
                // [REGRA] Valida modelo e ID
                if (model != null && model.AtaId != Guid.Empty)
                {
                    // [DADOS] Carrega Ata
                    var objFromDb = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u =>
                        u.AtaId == model.AtaId
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Impede exclusão se houver veículos vinculados
                        var veiculo = _unitOfWork.VeiculoAta.GetFirstOrDefault(u =>
                            u.AtaId == model.AtaId
                        );
                        if (veiculo != null)
                        {
                            return Ok(
                                new
                                {
                                    success = false,
                                    message = "Existem veículos associados a essa Ata",
                                }
                            );
                        }

                        var objRepactuacao = _unitOfWork.RepactuacaoAta.GetAll(riv =>
                            riv.AtaId == model.AtaId
                        );
                        foreach (var repactuacao in objRepactuacao)
                        {
                            // [DADOS] Remove itens da repactuação
                            var objItemRepactuacao = _unitOfWork.ItemVeiculoAta.GetAll(iva =>
                                iva.RepactuacaoAtaId == repactuacao.RepactuacaoAtaId
                            );
                            foreach (var itemveiculo in objItemRepactuacao)
                            {
                                _unitOfWork.ItemVeiculoAta.Remove(itemveiculo);
                            }
                            _unitOfWork.RepactuacaoAta.Remove(repactuacao);
                        }

                        _unitOfWork.AtaRegistroPrecos.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Ok(new
                        {
                            success = true,
                            message = "Ata removida com sucesso"
                        });
                    }
                }
                return Ok(new
                {
                    success = false,
                    message = "Erro ao apagar Ata"
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AtaRegistroPrecosController] Erro em Delete: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs", "Delete", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusAta                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna o status (Ativo/Inativo) de uma Ata de Registro de Preços.        ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite ativar/desativar atas sem exclusão física.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): identificador único da Ata.                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: status com descrição da mudança.                          ║
        /// ║    • Consumidor: UI de Atas de Registro de Preços.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault()                        ║
        /// ║    • _unitOfWork.AtaRegistroPrecos.Update()                                   ║
        /// ║    • _unitOfWork.Save()                                                      ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/AtaRegistroPrecos/UpdateStatusAta                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Contratos/Atas                                           ║
        /// ║    • Arquivos relacionados: Pages/AtaRegistroPrecos/*.cshtml                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [Route("UpdateStatusAta")]
        [HttpPost]
        public IActionResult UpdateStatusAta(Guid Id)
        {
            try
            {
                // [REGRA] Valida ID
                if (Id != Guid.Empty)
                {
                    // [DADOS] Busca Ata
                    var objFromDb = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u =>
                        u.AtaId == Id
                    );
                    string Description = string.Empty;
                    int type = 0;

                    if (objFromDb != null)
                    {
                        // [LOGICA] Alterna status
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status da Ata [Número: {0}] (Inativo)",
                                objFromDb.AnoAta + "/" + objFromDb.NumeroAta
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status da Ata  [Número: {0}] (Ativo)",
                                objFromDb.AnoAta + "/" + objFromDb.NumeroAta
                            );
                            type = 0;
                        }

                        _unitOfWork.AtaRegistroPrecos.Update(objFromDb);
                        _unitOfWork.Save();
                    }
                    return Ok(
                        new
                        {
                            success = true,
                            message = Description,
                            type,
                        }
                    );
                }
                return Ok(new
                {
                    success = false
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AtaRegistroPrecosController] Erro em UpdateStatusAta: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha(
                    "AtaRegistroPrecosController.cs",
                    "UpdateStatusAta",
                    ex
                );
                return StatusCode(500);
            }
        }

        /// <summary>
        /// (IA) Insere uma nova Ata no sistema e cria automaticamente uma repactuação inicial (Valor Inicial).
        /// </summary>
        /// <param name="ata">Objeto contendo os dados da nova Ata.</param>
        /// <returns>ID da repactuação criada.</returns>
        [Route("InsereAta")]
        [HttpPost]
        public IActionResult InsereAta(AtaRegistroPrecos ata)
        {
            try
            {
                var existeAta = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u =>
                    (u.AnoAta == ata.AnoAta) && (u.NumeroAta == ata.NumeroAta)
                );
                if (existeAta != null && existeAta.AtaId != ata.AtaId)
                {
                    return Ok(
                        new
                        {
                            success = false,
                            data = "00000000-0000-0000-0000-000000000000",
                            message = "Já existe uma ata com esse número!",
                        }
                    );
                }

                _unitOfWork.AtaRegistroPrecos.Add(ata);

                var objRepactuacao = new RepactuacaoAta();
                objRepactuacao.DataRepactuacao = ata.DataInicio;
                objRepactuacao.Descricao = "Valor Inicial";
                objRepactuacao.AtaId = ata.AtaId;
                _unitOfWork.RepactuacaoAta.Add(objRepactuacao);

                _unitOfWork.Save();

                return Ok(
                    new
                    {
                        data = objRepactuacao.RepactuacaoAtaId,
                        message = "Ata Adicionada com Sucesso",
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("[AtaRegistroPrecosController] Erro em InsereAta: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs", "InsereAta", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// (IA) Atualiza as informações cadastrais de uma Ata de Registro de Preços existente.
        /// </summary>
        /// <param name="ata">Objeto com os dados atualizados.</param>
        /// <returns>Dados da Ata atualizada.</returns>
        [Route("EditaAta")]
        [HttpPost]
        public IActionResult EditaAta(AtaRegistroPrecos ata)
        {
            try
            {
                var existeAta = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u =>
                    (u.AnoAta == ata.AnoAta) && (u.NumeroAta == ata.NumeroAta)
                );
                if (existeAta != null && existeAta.AtaId != ata.AtaId)
                {
                    return Ok(
                        new
                        {
                            data = "00000000-0000-0000-0000-000000000000",
                            message = "Já existe uma Ata com esse número",
                        }
                    );
                }

                _unitOfWork.AtaRegistroPrecos.Update(ata);
                _unitOfWork.Save();

                return Ok(new
                {
                    data = ata,
                    message = "Ata Atualizada com Sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AtaRegistroPrecosController] Erro em EditaAta: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AtaRegistroPrecosController.cs", "EditaAta", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// (IA) Insere um novo item de veículo/serviço vinculado a uma Ata e sua respectiva repactuação.
        /// </summary>
        /// <param name="itemveiculo">Objeto do item de veículo.</param>
        /// <returns>ID do item de veículo criado.</returns>
        [Route("InsereItemAta")]
        [HttpPost]
        public IActionResult InsereItemAta(ItemVeiculoAta itemveiculo)
        {
            try
            {
                _unitOfWork.ItemVeiculoAta.Add(itemveiculo);
                _unitOfWork.Save();

                return Ok(
                    new
                    {
                        data = itemveiculo.ItemVeiculoAtaId,
                        message = "Item Veiculo Ata adicionado com sucesso",
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("[AtaRegistroPrecosController] Erro em InsereItemAta: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha(
                    "AtaRegistroPrecosController.cs",
                    "InsereItemAta",
                    ex
                );
                return StatusCode(500);
            }
        }

        /// <summary>
        /// (IA) Obtém a lista de repactuações (ajustes de valores) associadas a uma Ata específica.
        /// </summary>
        /// <param name="id">ID da Ata.</param>
        /// <returns>Lista de repactuações formatadas.</returns>
        [Route("RepactuacaoList")]
        [HttpGet]
        public IActionResult RepactuacaoList(Guid id)
        {
            try
            {
                var RepactuacoList = (
                    from r in _unitOfWork.RepactuacaoAta.GetAll()
                    where r.AtaId == id
                    orderby r.DataRepactuacao
                    select new
                    {
                        r.RepactuacaoAtaId,
                        Repactuacao = "("
                            + r.DataRepactuacao?.ToString("dd/MM/yy")
                            + ") "
                            + r.Descricao,
                    }
                ).ToList();

                return Ok(new
                {
                    data = RepactuacoList
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AtaRegistroPrecosController] Erro em RepactuacaoList: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha(
                    "AtaRegistroPrecosController.cs",
                    "RepactuacaoList",
                    ex
                );
                return StatusCode(500);
            }
        }

        /// <summary>
        /// (IA) Retorna a lista de Atas filtradas por ano para preenchimento de componentes DropDown (Select2).
        /// </summary>
        /// <param name="id">Ano da Ata.</param>
        /// <returns>Lista de objetos para DropDown.</returns>
        [Route("ListaAtas")]
        [HttpGet]
        public IActionResult OnGetListaAtas(string id)
        {
            try
            {
                var AtaList = _unitOfWork.AtaRegistroPrecos.GetAtaListForDropDown(
                    Convert.ToInt32(id)
                );
                return Ok(new
                {
                    data = AtaList
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AtaRegistroPrecosController] Erro em OnGetListaAtas: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha(
                    "AtaRegistroPrecosController.cs",
                    "OnGetListaAtas",
                    ex
                );
                return StatusCode(500);
            }
        }
    }
}
