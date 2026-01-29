/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: PatrimonioController.cs                                                                 ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Gerenciar Patrimônios (ativos) e movimentações entre setores/seções. Rastreio ativos.  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Get(), GetMovimentacao(), CreateMovimentacao() - filtros marca/modelo/setor/situação     ║
   ║ 🔗 DEPS: IUnitOfWork, IMemoryCache, anti-duplicação | 📅 28/01/2026 | 👤 Copilot | 📝 v2.0          ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

//using Stimulsoft.System.Windows.Forms;
//using NPOI.SS.Formula.Functions;

namespace FrotiX.Controllers
{
    // Adicionar estes métodos ao PatrimonioController.cs existente

    [Route("api/[controller]")]
    [ApiController]
    public class PatrimonioController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        private static readonly HashSet<string> _processandoRequests = new HashSet<string>();
        private static readonly object _lockObject = new object();

        public PatrimonioController(IUnitOfWork unitOfWork , IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        // GET: api/Patrimonio/Get
        [HttpGet]
        public IActionResult Get(string marca = "" , string modelo = "" , string setor = "" , string secao = "" , string situacao = "")
        {
            try
            {
                var query = _unitOfWork.ViewPatrimonioConferencia.GetAll().AsQueryable();

                // Aplicar filtros se fornecidos
                if (!string.IsNullOrWhiteSpace(marca))
                {
                    var marcas = marca.Split(',').Select(m => m.Trim()).Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
                    if (marcas.Any())
                    {
                        query = query.Where(p => marcas.Contains(p.Marca));
                    }
                }

                if (!string.IsNullOrWhiteSpace(modelo))
                {
                    var modelos = modelo.Split(',').Select(m => m.Trim()).Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
                    if (modelos.Any())
                    {
                        query = query.Where(p => modelos.Contains(p.Modelo));
                    }
                }

                if (!string.IsNullOrWhiteSpace(setor))
                {
                    var setores = setor.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    if (setores.Any())
                    {
                        query = query.Where(p => setores.Contains(p.NomeSetor));
                    }
                }

                if (!string.IsNullOrWhiteSpace(secao))
                {
                    var secoes = secao.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    if (secoes.Any())
                    {
                        query = query.Where(p => secoes.Contains(p.NomeSecao));
                    }
                }

                if (!string.IsNullOrWhiteSpace(situacao))
                {
                    var situacoes = situacao.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    if (situacoes.Any())
                    {
                        query = query.Where(p => situacoes.Contains(p.Situacao));
                    }
                }

                var patrimonios = query.OrderBy(p => p.NPR).ToList();

                return Json(new
                {
                    success = true ,
                    data = patrimonios
                });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false ,
                        data = new List<object>() ,
                        message = $"Erro ao carregar patrimônios: {ex.Message}" ,
                    }
                );
            }
        }

        // GET: api/Patrimonio/GetMovimentacao
        [HttpGet]
        [Route("GetMovimentacao")]
        public IActionResult GetMovimentacao(Guid id)
        {
            try
            {
                var movimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(m =>
                    m.MovimentacaoPatrimonioId == id
                );

                if (movimentacao == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Movimentação não encontrada"
                    });
                }

                // Buscar dados relacionados
                var patrimonio = _unitOfWork.Patrimonio.GetFirstOrDefault(p =>
                    p.PatrimonioId == movimentacao.PatrimonioId
                );

                var setorOrigem = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(s =>
                    s.SetorId == movimentacao.SetorOrigemId
                );

                var secaoOrigem = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(s =>
                    s.SecaoId == movimentacao.SecaoOrigemId
                );

                var setorDestino = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(s =>
                    s.SetorId == movimentacao.SetorDestinoId
                );

                var secaoDestino = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(s =>
                    s.SecaoId == movimentacao.SecaoDestinoId
                );

                var result = new
                {
                    movimentacaoPatrimonioId = movimentacao.MovimentacaoPatrimonioId ,
                    patrimonioId = movimentacao.PatrimonioId ,
                    dataMovimentacao = movimentacao.DataMovimentacao ,
                    setorOrigemId = movimentacao.SetorOrigemId ,
                    secaoOrigemId = movimentacao.SecaoOrigemId ,
                    setorDestinoId = movimentacao.SetorDestinoId ,
                    secaoDestinoId = movimentacao.SecaoDestinoId ,
                    setorOrigemNome = setorOrigem?.NomeSetor ,
                    secaoOrigemNome = secaoOrigem?.NomeSecao ,
                    setorDestinoNome = setorDestino?.NomeSetor ,
                    secaoDestinoNome = secaoDestino?.NomeSecao ,
                    patrimonioNpr = patrimonio?.NPR ,
                    patrimonioDescricao = patrimonio?.Descricao ,
                    status = patrimonio?.Status ?? false ,
                };

                return Json(new
                {
                    success = true ,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false ,
                        message = $"Erro ao buscar movimentação: {ex.Message}"
                    }
                );
            }
        }

        // POST: api/Patrimonio/CreateMovimentacao
        [HttpPost]
        [Route("CreateMovimentacao")]
        public IActionResult CreateMovimentacao([FromBody] MovimentacaoPatrimonioDto dto)
        {
            var requestId = Guid.NewGuid().ToString().Substring(0 , 8);
            Console.WriteLine($"[{requestId}] === INÍCIO CreateMovimentacao ===");
            Console.WriteLine(
                $"[{requestId}] Dados recebidos: PatrimonioId={dto.PatrimonioId}, Data={dto.DataMovimentacao}"
            );

            // Criar chave única para prevenir duplicação
            var requestKey =
                $"{dto.PatrimonioId}_{dto.DataMovimentacao?.ToString("yyyyMMddHHmmss")}";

            lock (_lockObject)
            {
                if (_processandoRequests.Contains(requestKey))
                {
                    Console.WriteLine($"[{requestId}] Requisição duplicada detectada. Rejeitando.");
                    return Json(
                        new
                        {
                            success = false ,
                            message = "Requisição já está sendo processada. Aguarde." ,
                        }
                    );
                }
                _processandoRequests.Add(requestKey);
            }

            try
            {
                // Obter usuário atual
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"[{requestId}] Usuário: {currentUserID}");

                // ========== VALIDAÇÕES ==========
                Console.WriteLine($"[{requestId}] Iniciando validações...");

                if (dto.PatrimonioId == Guid.Empty)
                {
                    Console.WriteLine($"[{requestId}] Erro: Patrimônio não selecionado");
                    return Json(new
                    {
                        success = false ,
                        message = "Patrimônio não selecionado"
                    });
                }

                if (!dto.DataMovimentacao.HasValue)
                {
                    Console.WriteLine($"[{requestId}] Erro: Data não informada");
                    return Json(
                        new
                        {
                            success = false ,
                            message = "Data da movimentação não informada"
                        }
                    );
                }

                if (!dto.SetorDestinoId.HasValue || dto.SetorDestinoId == Guid.Empty)
                {
                    Console.WriteLine($"[{requestId}] Erro: Setor destino não informado");
                    return Json(
                        new
                        {
                            success = false ,
                            message = "Setor de destino não informado"
                        }
                    );
                }

                if (!dto.SecaoDestinoId.HasValue || dto.SecaoDestinoId == Guid.Empty)
                {
                    Console.WriteLine($"[{requestId}] Erro: Seção destino não informada");
                    return Json(
                        new
                        {
                            success = false ,
                            message = "Seção de destino não informada"
                        }
                    );
                }

                Console.WriteLine($"[{requestId}] Validações OK");

                // ========== BUSCAR PATRIMÔNIO ==========
                Console.WriteLine($"[{requestId}] Buscando patrimônio {dto.PatrimonioId}...");

                // Usar Find ao invés de GetFirstOrDefault para evitar problemas de concorrência
                Patrimonio patrimonio = null;
                try
                {
                    patrimonio = _unitOfWork
                        .Patrimonio.GetAll()
                        .FirstOrDefault(p => p.PatrimonioId == dto.PatrimonioId);
                }
                catch (Exception findEx)
                {
                    Console.WriteLine($"[{requestId}] Erro ao buscar patrimônio: {findEx.Message}");
                    throw;
                }

                if (patrimonio == null)
                {
                    Console.WriteLine($"[{requestId}] Patrimônio não encontrado");
                    return Json(new
                    {
                        success = false ,
                        message = "Patrimônio não encontrado"
                    });
                }

                Console.WriteLine(
                    $"[{requestId}] Patrimônio encontrado: NPR={patrimonio.NPR}, SetorAtual={patrimonio.SetorId}, SecaoAtual={patrimonio.SecaoId}"
                );

                // ========== GUARDAR VALORES ORIGINAIS ==========
                var setorOrigemId = patrimonio.SetorId;
                var secaoOrigemId = patrimonio.SecaoId;

                // ========== VALIDAR ORIGEM != DESTINO ==========
                if (dto.SecaoDestinoId == secaoOrigemId && dto.SetorDestinoId == setorOrigemId)
                {
                    Console.WriteLine($"[{requestId}] Erro: Destino igual à origem");
                    return Json(
                        new
                        {
                            success = false ,
                            message = "O destino deve ser diferente da localização atual" ,
                        }
                    );
                }

                // ========== CRIAR MOVIMENTAÇÃO ==========
                Console.WriteLine($"[{requestId}] Criando objeto movimentação...");
                var movimentacao = new MovimentacaoPatrimonio
                {
                    MovimentacaoPatrimonioId = Guid.NewGuid() ,
                    PatrimonioId = dto.PatrimonioId ,
                    DataMovimentacao = dto.DataMovimentacao.Value ,
                    SetorOrigemId = setorOrigemId ,
                    SecaoOrigemId = secaoOrigemId ,
                    SetorDestinoId = dto.SetorDestinoId.Value ,
                    SecaoDestinoId = dto.SecaoDestinoId.Value ,
                    ResponsavelMovimentacao = currentUserID ,
                };
                Console.WriteLine(
                    $"[{requestId}] Movimentação criada com ID: {movimentacao.MovimentacaoPatrimonioId}"
                );

                // ========== ATUALIZAR PATRIMÔNIO ==========
                Console.WriteLine($"[{requestId}] Atualizando patrimônio para novo destino...");
                patrimonio.SetorId = dto.SetorDestinoId.Value;
                patrimonio.SecaoId = dto.SecaoDestinoId.Value;
                patrimonio.Status = dto.StatusPatrimonio;

                // ========== PERSISTIR NO BANCO ==========
                try
                {
                    Console.WriteLine($"[{requestId}] Adicionando movimentação ao contexto...");
                    _unitOfWork.MovimentacaoPatrimonio.Add(movimentacao);

                    Console.WriteLine($"[{requestId}] Marcando patrimônio como modificado...");
                    _unitOfWork.Patrimonio.Update(patrimonio);

                    Console.WriteLine($"[{requestId}] Chamando Save() - ÚNICA VEZ");
                    _unitOfWork.Save();
                    Console.WriteLine($"[{requestId}] Save() completado com sucesso");
                }
                catch (Exception saveEx)
                {
                    Console.WriteLine($"[{requestId}] ERRO no Save(): {saveEx.Message}");
                    Console.WriteLine($"[{requestId}] StackTrace: {saveEx.StackTrace}");
                    throw;
                }

                // ========== RETORNAR SUCESSO ==========
                var response = new
                {
                    success = true ,
                    message = "Movimentação registrada com sucesso!" ,
                    data = new
                    {
                        movimentacaoId = movimentacao.MovimentacaoPatrimonioId
                    } ,
                };

                Console.WriteLine($"[{requestId}] Preparando resposta JSON de sucesso");
                Console.WriteLine($"[{requestId}] === FIM CreateMovimentacao (SUCESSO) ===");

                return Json(response);
            }
            catch (InvalidOperationException ioEx) when (ioEx.Message.Contains("second operation"))
            {
                Console.WriteLine($"[{requestId}] ERRO de concorrência: {ioEx.Message}");
                return Json(
                    new
                    {
                        success = false ,
                        message = "Erro de concorrência. Por favor, aguarde e tente novamente." ,
                    }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{requestId}] ERRO geral: {ex.Message}");
                Console.WriteLine($"[{requestId}] StackTrace: {ex.StackTrace}");
                Console.WriteLine($"[{requestId}] === FIM CreateMovimentacao (ERRO) ===");

                return Json(
                    new
                    {
                        success = false ,
                        message = "Erro ao criar movimentação. Tente novamente." ,
                    }
                );
            }
            finally
            {
                // Sempre remover da lista de processamento
                lock (_lockObject)
                {
                    _processandoRequests.Remove(requestKey);
                    Console.WriteLine($"[{requestId}] Request removido da lista de processamento");
                }
            }
        }

        // POST: api/Patrimonio/UpdateMovimentacao
        [HttpPost]
        [Route("UpdateMovimentacao")]
        public IActionResult UpdateMovimentacao([FromBody] MovimentacaoPatrimonioDto dto)
        {
            try
            {
                // Obter usuário atual
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (
                    !dto.MovimentacaoPatrimonioId.HasValue
                    || dto.MovimentacaoPatrimonioId == Guid.Empty
                )
                {
                    return Json(
                        new
                        {
                            success = false ,
                            message = "ID da movimentação não informado"
                        }
                    );
                }

                // Buscar movimentação existente
                var movimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(m =>
                    m.MovimentacaoPatrimonioId == dto.MovimentacaoPatrimonioId
                );

                if (movimentacao == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Movimentação não encontrada"
                    });
                }

                // Buscar o patrimônio
                var patrimonio = _unitOfWork.Patrimonio.GetFirstOrDefault(p =>
                    p.PatrimonioId == dto.PatrimonioId
                );

                if (patrimonio == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Patrimônio não encontrado"
                    });
                }

                // Atualizar movimentação
                movimentacao.PatrimonioId = dto.PatrimonioId;
                movimentacao.DataMovimentacao =
                    dto.DataMovimentacao ?? movimentacao.DataMovimentacao;
                movimentacao.SetorOrigemId = dto.SetorOrigemId ?? movimentacao.SetorOrigemId;
                movimentacao.SecaoOrigemId = dto.SecaoOrigemId ?? movimentacao.SecaoOrigemId;
                movimentacao.SetorDestinoId = dto.SetorDestinoId ?? movimentacao.SetorDestinoId;
                movimentacao.SecaoDestinoId = dto.SecaoDestinoId ?? movimentacao.SecaoDestinoId;
                movimentacao.ResponsavelMovimentacao = currentUserID;

                // Atualizar patrimônio se necessário
                if (dto.SetorDestinoId.HasValue && dto.SecaoDestinoId.HasValue)
                {
                    patrimonio.SetorId = dto.SetorDestinoId.Value;
                    patrimonio.SecaoId = dto.SecaoDestinoId.Value;
                    patrimonio.Status = dto.StatusPatrimonio;
                    _unitOfWork.Patrimonio.Update(patrimonio);
                }

                // Salvar alterações
                _unitOfWork.MovimentacaoPatrimonio.Update(movimentacao);
                _unitOfWork.Save();

                return Json(
                    new
                    {
                        success = true ,
                        message = "Movimentação atualizada com sucesso!"
                    }
                );
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false ,
                        message = $"Erro ao atualizar movimentação: {ex.Message}" ,
                    }
                );
            }
        }

        // DELETE: api/Patrimonio/DeleteMovimentacaoPatrimonio
        [HttpPost]
        [Route("DeleteMovimentacaoPatrimonio")]
        public IActionResult DeleteMovimentacaoPatrimonio([FromBody] DeleteMovimentacaoDto dto)
        {
            try
            {
                if (dto.MovimentacaoPatrimonioId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "ID inválido"
                    });
                }

                var movimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(m =>
                    m.MovimentacaoPatrimonioId == dto.MovimentacaoPatrimonioId
                );

                if (movimentacao == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Movimentação não encontrada"
                    });
                }

                _unitOfWork.MovimentacaoPatrimonio.Remove(movimentacao);
                _unitOfWork.Save();

                return Json(new
                {
                    success = true ,
                    message = "Movimentação excluída com sucesso!"
                });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false ,
                        message = $"Erro ao excluir movimentação: {ex.Message}"
                    }
                );
            }
        }

        // GET: api/Patrimonio/MovimentacaoPatrimonioGrid
        [HttpGet]
        [Route("MovimentacaoPatrimonioGrid")]
        public IActionResult MovimentacaoPatrimonioGrid(
            string patrimonioId = "" ,
            string dataInicio = "" ,
            string dataFim = "" ,
            string setorSecaoOrigem = "" ,
            string setorSecaoDestino = "" ,
            string responsavel = "")
        {
            try
            {
                var query = (
                    from m in _unitOfWork.MovimentacaoPatrimonio.GetAll()
                    join p in _unitOfWork.Patrimonio.GetAll()
                        on m.PatrimonioId equals p.PatrimonioId
                    join setorOrigem in _unitOfWork.SetorPatrimonial.GetAll()
                        on m.SetorOrigemId equals setorOrigem.SetorId
                        into setorOrigemGroup
                    from so in setorOrigemGroup.DefaultIfEmpty()
                    join secaoOrigem in _unitOfWork.SecaoPatrimonial.GetAll()
                        on m.SecaoOrigemId equals secaoOrigem.SecaoId
                        into secaoOrigemGroup
                    from sco in secaoOrigemGroup.DefaultIfEmpty()
                    join setorDestino in _unitOfWork.SetorPatrimonial.GetAll()
                        on m.SetorDestinoId equals setorDestino.SetorId
                        into setorDestinoGroup
                    from sd in setorDestinoGroup.DefaultIfEmpty()
                    join secaoDestino in _unitOfWork.SecaoPatrimonial.GetAll()
                        on m.SecaoDestinoId equals secaoDestino.SecaoId
                        into secaoDestinoGroup
                    from scd in secaoDestinoGroup.DefaultIfEmpty()
                    join u in _unitOfWork.AspNetUsers.GetAll()
                        on m.ResponsavelMovimentacao equals u.Id
                        into userGroup
                    from user in userGroup.DefaultIfEmpty()
                    select new
                    {
                        movimentacaoPatrimonioId = m.MovimentacaoPatrimonioId ,
                        dataMovimentacao = m.DataMovimentacao ,
                        npr = p.NPR ,
                        descricao = p.Descricao ,
                        setorOrigemNome = so != null ? so.NomeSetor : "" ,
                        secaoOrigemNome = sco != null ? sco.NomeSecao : "" ,
                        setorDestinoNome = sd != null ? sd.NomeSetor : "" ,
                        secaoDestinoNome = scd != null ? scd.NomeSecao : "" ,
                        responsavelMovimentacao = user != null ? user.NomeCompleto : "Sistema" ,
                        patrimonioId = p.PatrimonioId ,
                        setorOrigemId = m.SetorOrigemId ,
                        secaoOrigemId = m.SecaoOrigemId ,
                        setorDestinoId = m.SetorDestinoId ,
                        secaoDestinoId = m.SecaoDestinoId ,
                        responsavelId = m.ResponsavelMovimentacao
                    }
                ).AsQueryable();

                // Aplicar filtro de patrimônio
                if (!string.IsNullOrWhiteSpace(patrimonioId))
                {
                    if (Guid.TryParse(patrimonioId , out Guid patrimonioGuid))
                    {
                        query = query.Where(m => m.patrimonioId == patrimonioGuid);
                    }
                }

                // Aplicar filtro de data
                if (!string.IsNullOrWhiteSpace(dataInicio) && DateTime.TryParse(dataInicio , out DateTime dtInicio))
                {
                    query = query.Where(m => m.dataMovimentacao >= dtInicio);
                }

                if (!string.IsNullOrWhiteSpace(dataFim) && DateTime.TryParse(dataFim , out DateTime dtFim))
                {
                    query = query.Where(m => m.dataMovimentacao <= dtFim);
                }

                // Aplicar filtro de Setor/Seção Origem
                if (!string.IsNullOrWhiteSpace(setorSecaoOrigem))
                {
                    var setoresSecoesOrigem = setorSecaoOrigem.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList();

                    if (setoresSecoesOrigem.Any())
                    {
                        query = query.Where(m =>
                            (m.setorOrigemNome != null && setoresSecoesOrigem.Contains(m.setorOrigemNome)) ||
                            (m.secaoOrigemNome != null && setoresSecoesOrigem.Contains(m.secaoOrigemNome))
                        );
                    }
                }

                // Aplicar filtro de Setor/Seção Destino
                if (!string.IsNullOrWhiteSpace(setorSecaoDestino))
                {
                    var setoresSecoesDestino = setorSecaoDestino.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList();

                    if (setoresSecoesDestino.Any())
                    {
                        query = query.Where(m =>
                            (m.setorDestinoNome != null && setoresSecoesDestino.Contains(m.setorDestinoNome)) ||
                            (m.secaoDestinoNome != null && setoresSecoesDestino.Contains(m.secaoDestinoNome))
                        );
                    }
                }

                // Aplicar filtro de Responsável
                if (!string.IsNullOrWhiteSpace(responsavel))
                {
                    query = query.Where(m => m.responsavelMovimentacao == responsavel);
                }

                var movimentacoes = query
                    .OrderByDescending(m => m.dataMovimentacao)
                    .Select(m => new
                    {
                        m.movimentacaoPatrimonioId ,
                        m.dataMovimentacao ,
                        m.npr ,
                        m.descricao ,
                        m.setorOrigemNome ,
                        m.secaoOrigemNome ,
                        m.setorDestinoNome ,
                        m.secaoDestinoNome ,
                        m.responsavelMovimentacao
                    })
                    .ToList();

                return Json(new
                {
                    data = movimentacoes
                });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        data = new List<object>() ,
                        error = $"Erro ao carregar grid: {ex.Message}" ,
                    }
                );
            }
        }

        // GET: api/Patrimonio/GetResponsaveisMovimentacoes
        [HttpGet]
        [Route("GetResponsaveisMovimentacoes")]
        public IActionResult GetResponsaveisMovimentacoes()
        {
            try
            {
                var responsaveis = _unitOfWork.AspNetUsers.GetAll()
                    .Where(u => !string.IsNullOrWhiteSpace(u.NomeCompleto))
                    .OrderBy(u => u.NomeCompleto)
                    .Select(u => new
                    {
                        text = u.NomeCompleto ,
                        value = u.NomeCompleto
                    })
                    .Distinct()
                    .ToList();

                return Json(new
                {
                    success = true ,
                    data = responsaveis
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false ,
                    message = $"Erro ao carregar responsáveis: {ex.Message}"
                });
            }
        }

        // GET: api/Patrimonio/GetSetoresSecoesHierarquicos
        [HttpGet]
        [Route("GetSetoresSecoesHierarquicos")]
        public IActionResult GetSetoresSecoesHierarquicos()
        {
            try
            {
                var setores = _unitOfWork.SetorPatrimonial.GetAll()
                    .Where(s => s.Status == true)
                    .OrderBy(s => s.NomeSetor)
                    .ToList();

                var secoes = _unitOfWork.SecaoPatrimonial.GetAll()
                    .Where(s => s.Status == true)
                    .ToList();

                var hierarchicalData = setores.Select(setor => new
                {
                    id = setor.NomeSetor ,
                    name = setor.NomeSetor ,
                    hasChildren = true ,
                    children = secoes
                        .Where(sec => sec.SetorId == setor.SetorId)
                        .OrderBy(sec => sec.NomeSecao)
                        .Select(sec => new
                        {
                            id = sec.NomeSecao ,
                            name = sec.NomeSecao ,
                            hasChildren = false
                        })
                        .ToList<object>()
                })
                .ToList();

                return Json(new
                {
                    success = true ,
                    data = hierarchicalData
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false ,
                    message = $"Erro ao carregar setores e seções: {ex.Message}"
                });
            }
        }

        // Adicionar estes métodos aos controllers existentes

        // ====== PatrimonioController.cs ======

        // GET: api/Patrimonio/ListaPatrimonios
        [HttpGet]
        [Route("ListaPatrimonios")]
        public IActionResult ListaPatrimonios()
        {
            try
            {
                var patrimonios = _unitOfWork
                    .Patrimonio.GetAll()
                    .Where(p => p.Status == true) // Apenas patrimônios ativos
                    .OrderBy(p => p.NPR)
                    .Select(p => new { text = $"{p.NPR} - {p.Descricao}" , value = p.PatrimonioId })
                    .ToList();

                return Json(new
                {
                    success = true ,
                    data = patrimonios
                });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false ,
                        data = new List<object>() ,
                        message = $"Erro ao carregar patrimônios: {ex.Message}" ,
                    }
                );
            }
        }

        // GET: api/Patrimonio/GetSingle
        [HttpGet]
        [Route("GetSingle")]
        public async Task<IActionResult> GetSingle(Guid Id)
        {
            try
            {
                // Aguardar operações anteriores completarem
                await Task.Delay(100);

                var patrimonio = _unitOfWork.Patrimonio.GetFirstOrDefault(p =>
                    p.PatrimonioId == Id
                );

                if (patrimonio == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Patrimônio não encontrado"
                    });
                }

                var setor = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(s =>
                    s.SetorId == patrimonio.SetorId
                );
                var secao = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(s =>
                    s.SecaoId == patrimonio.SecaoId
                );

                var result = new
                {
                    patrimonioId = patrimonio.PatrimonioId ,
                    npr = patrimonio.NPR ,
                    descricao = patrimonio.Descricao ,
                    marca = patrimonio.Marca ,
                    modelo = patrimonio.Modelo ,
                    numeroSerie = patrimonio.NumeroSerie ,
                    status = patrimonio.Status ,
                    setorOrigemId = patrimonio.SetorId ,
                    secaoOrigemId = patrimonio.SecaoId ,
                    setorOrigemNome = setor?.NomeSetor ,
                    secaoOrigemNome = secao?.NomeSecao ,
                };

                return Json(new
                {
                    success = true ,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false ,
                        message = $"Erro ao buscar patrimônio: {ex.Message}"
                    }
                );
            }
        }

        // GET: api/Setor/ListaSetores
        [HttpGet]
        [Route("ListaSetores")]
        public IActionResult ListaSetores()
        {
            try
            {
                var setores = _unitOfWork
                    .SetorPatrimonial.GetAll()
                    .Where(s => s.Status == true) // Apenas setores ativos
                    .OrderBy(s => s.NomeSetor)
                    .Select(s => new { text = s.NomeSetor , value = s.SetorId })
                    .ToList();

                return Json(new
                {
                    success = true ,
                    data = setores
                });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false ,
                        data = new List<object>() ,
                        message = $"Erro ao carregar setores: {ex.Message}" ,
                    }
                );
            }
        }

        // GET: api/Secao/ListaSecoes
        [HttpGet]
        [Route("ListaSecoes")]
        public IActionResult ListaSecoes(Guid? setorSelecionado)
        {
            try
            {
                if (!setorSelecionado.HasValue || setorSelecionado == Guid.Empty)
                {
                    return Json(new
                    {
                        success = true ,
                        data = new List<object>()
                    });
                }

                var secoes = _unitOfWork
                    .SecaoPatrimonial.GetAll()
                    .Where(s => s.SetorId == setorSelecionado && s.Status == true)
                    .OrderBy(s => s.NomeSecao)
                    .Select(s => new { text = s.NomeSecao , value = s.SecaoId })
                    .ToList();

                return Json(new
                {
                    success = true ,
                    data = secoes
                });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false ,
                        data = new List<object>() ,
                        message = $"Erro ao carregar seções: {ex.Message}" ,
                    }
                );
            }
        }

        // GET: api/Patrimonio/ListaMarcas
        [HttpGet("ListaMarcas")]
        public IActionResult ListaMarcas()
        {
            try
            {
                var listaMarcas = _unitOfWork
                    .Patrimonio.GetAllReduced(selector: p => p.Marca)
                    .Where(m => m != null)
                    .Distinct()
                    .OrderBy(m => m)
                    .Select(m => new { text = m , value = m })
                    .ToList();

                return Json(new
                {
                    success = true ,
                    data = listaMarcas
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false ,
                    data = new List<object>() ,
                    message = $"Erro ao carregar marcas: {ex.Message}"
                });
            }
        }

        // GET: api/Patrimonio/ListaModelos
        [HttpGet("ListaModelos")]
        public IActionResult ListaModelos(string marca)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(marca))
                {
                    return Json(new
                    {
                        success = true ,
                        data = new List<object>()
                    });
                }

                var listaModelos = _unitOfWork
                    .Patrimonio.GetAll()
                    .Where(p => p.Marca == marca && p.Modelo != null)
                    .Select(p => p.Modelo)
                    .Distinct()
                    .OrderBy(m => m)
                    .Select(m => new { text = m , value = m })
                    .ToList();

                return Json(new
                {
                    success = true ,
                    data = listaModelos
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false ,
                    data = new List<object>() ,
                    message = $"Erro ao carregar modelos: {ex.Message}"
                });
            }
        }

        // GET: api/Patrimonio/ListaMarcasModelos
        [HttpGet]
        [Route("ListaMarcasModelos")]
        public IActionResult ListaMarcasModelos()
        {
            try
            {
                var marcas = _unitOfWork.Patrimonio.GetAll()
                    .Where(p => !string.IsNullOrWhiteSpace(p.Marca))
                    .GroupBy(p => p.Marca)
                    .Select(g => new
                    {
                        text = g.Key ,
                        value = g.Key ,
                        hasChildren = true
                    })
                    .OrderBy(m => m.text)
                    .ToList();

                var modelos = _unitOfWork.Patrimonio.GetAll()
                    .Where(p => !string.IsNullOrWhiteSpace(p.Marca) && !string.IsNullOrWhiteSpace(p.Modelo))
                    .GroupBy(p => new { p.Marca , p.Modelo })
                    .Select(g => new
                    {
                        text = g.Key.Modelo ,
                        value = g.Key.Modelo ,
                        parentValue = g.Key.Marca ,
                        hasChildren = false
                    })
                    .OrderBy(m => m.text)
                    .ToList();

                var resultado = marcas.Concat<object>(modelos).ToList();

                return Json(new
                {
                    success = true ,
                    data = resultado
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false ,
                    data = new List<object>() ,
                    message = $"Erro ao carregar marcas e modelos: {ex.Message}"
                });
            }
        }

        // GET: api/Patrimonio/ListaSetoresSecoes
        [HttpGet]
        [Route("ListaSetoresSecoes")]
        public IActionResult ListaSetoresSecoes()
        {
            try
            {
                var setores = _unitOfWork.SetorPatrimonial.GetAll()
                    .Where(s => s.Status == true)
                    .Select(s => new
                    {
                        text = s.NomeSetor ,
                        value = s.NomeSetor ,
                        hasChildren = true
                    })
                    .OrderBy(s => s.text)
                    .ToList();

                var secoes = (from sec in _unitOfWork.SecaoPatrimonial.GetAll()
                              join set in _unitOfWork.SetorPatrimonial.GetAll() on sec.SetorId equals set.SetorId
                              where sec.Status == true && set.Status == true
                              select new
                              {
                                  text = sec.NomeSecao ,
                                  value = sec.NomeSecao ,
                                  parentValue = set.NomeSetor ,
                                  hasChildren = false
                              })
                              .OrderBy(s => s.text)
                              .ToList();

                var resultado = setores.Concat<object>(secoes).ToList();

                return Json(new
                {
                    success = true ,
                    data = resultado
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false ,
                    data = new List<object>() ,
                    message = $"Erro ao carregar setores e seções: {ex.Message}"
                });
            }
        }

        // GET: api/Patrimonio/ListaSituacoes
        [HttpGet]
        [Route("ListaSituacoes")]
        public IActionResult ListaSituacoes()
        {
            try
            {
                var situacoes = new List<object>
                {
                    new { text = "Em Uso", value = "Em Uso" },
                    new { text = "Em Manutenção", value = "Em Manutenção" },
                    new { text = "Não Localizado", value = "Não Localizado" },
                    new { text = "Avariado/Inservível", value = "Avariado/Inservível" },
                    new { text = "Transferido (baixado)", value = "Transferido (baixado)" }
                };

                return Json(new
                {
                    success = true ,
                    data = situacoes
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false ,
                    data = new List<object>() ,
                    message = $"Erro ao carregar situações: {ex.Message}"
                });
            }
        }
    }

    // DTOs para os endpoints
    public class MovimentacaoPatrimonioDto
    {
        public Guid? MovimentacaoPatrimonioId
        {
            get; set;
        }

        public Guid PatrimonioId
        {
            get; set;
        }

        public DateTime? DataMovimentacao
        {
            get; set;
        }

        public Guid? SetorOrigemId
        {
            get; set;
        }

        public Guid? SecaoOrigemId
        {
            get; set;
        }

        public Guid? SetorDestinoId
        {
            get; set;
        }

        public Guid? SecaoDestinoId
        {
            get; set;
        }

        public bool StatusPatrimonio
        {
            get; set;
        }
    }

    public class DeleteMovimentacaoDto
    {
        public Guid MovimentacaoPatrimonioId
        {
            get; set;
        }
    }
}
