
using FrotiX.Repository;
using global::FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FrotiX.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Cache
{
    /* > ---------------------------------------------------------------------------------------
     > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
     > ---------------------------------------------------------------------------------------
     > 🆔 **Nome:** MotoristaCache.cs
     > 📍 **Local:** Cache/
     > ❓ **Por que existo?** Gerencia o cache em memória dos motoristas com suas fotos para
     >    agilizar a listagem em combos e dashboards.
     > 🔗 **Relevância:** Alta (UX e Performance)
     > --------------------------------------------------------------------------------------- */

    public class MotoristaCache
    {
        private readonly IUnitOfWork _unitOfWork;
        private List<object> _cachedMotoristas;
        private readonly object _lock = new();
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogService _log;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MotoristaCache                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o cache e dispara o primeiro carregamento de motoristas.       ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Reduz chamadas repetidas ao banco e melhora UX em listas e dashboards.    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): acesso a dados.                               ║
        /// ║    • scopeFactory (IServiceScopeFactory): fábrica de escopos de DI.          ║
        /// ║    • log (ILogService): serviço de log FrotiX.                               ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: constrói o cache e inicia carregamento.                           ║
        /// ║    • Significado: prepara a estrutura em memória.                            ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • LoadMotoristas() → carrega o cache inicial.                             ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • DI container do ASP.NET Core.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: MotoristaCache.cs                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public MotoristaCache(IUnitOfWork unitOfWork, IServiceScopeFactory scopeFactory, ILogService log)
        {
            _unitOfWork = unitOfWork;
            _scopeFactory = scopeFactory;
            _log = log;
            LoadMotoristas(); // Carrega ao iniciar
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LoadMotoristas                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Carrega ou atualiza os motoristas do banco para memória.                  ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Mantém dados prontos para uso em interfaces sem custo de consulta.        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum                                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: atualiza o cache interno.                                         ║
        /// ║    • Significado: sincroniza dados em memória com a fonte.                   ║
        /// ║    • Consumidor: métodos de leitura do cache.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewMotoristasViagem.GetAllReduced() → consulta dados.      ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Construtor do cache e rotinas de atualização.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: MotoristaCache.cs                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public void LoadMotoristas()
        {
            try
            {
                lock (_lock)
                {
                    var motoristas = _unitOfWork.ViewMotoristasViagem.GetAllReduced(
                        selector: m => new
                        {
                            m.MotoristaId,
                            Nome = m.MotoristaCondutor,
                            m.Foto
                        },
                        orderBy: q => q.OrderBy(m => m.MotoristaCondutor)
                    ).ToList();

                    _cachedMotoristas = motoristas.Select(m =>
                    {
                        string fotoBase64;

                        if (m.Foto != null && m.Foto.Length > 0)
                        {
                            try
                            {
                                fotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(m.Foto)}";
                            }
                            catch
                            {
                                fotoBase64 = "/images/barbudo.jpg";
                            }
                        }
                        else
                        {
                            fotoBase64 = "/images/barbudo.jpg";
                        }

                        return new
                        {
                            m.MotoristaId,
                            Nome = m.Nome,
                            Foto = fotoBase64
                        };
                    }).Cast<object>().ToList();
                }

                _log.Info($"Cache de Motoristas carregado com sucesso: {_cachedMotoristas?.Count} registros.");
            }
            catch (Exception ex)
            {
                _log.Error("Falha ao carregar cache de motoristas", ex);
            }
        }


        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetMotoristas                                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna a lista de motoristas em cache com fallback de foto.              ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Fornece dados rápidos para combos e telas sem acesso ao banco.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum                                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • List<object>: motoristas com foto garantida.                            ║
        /// ║    • Significado: base de dados pronta para UI.                              ║
        /// ║    • Consumidor: páginas e APIs de listagem.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _log.Error() → registra falhas.                                         ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Serviços e páginas que exigem motoristas em cache.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: MotoristaCache.cs                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public List<object> GetMotoristas()
        {
            try
            {
                return _cachedMotoristas?.Select(m =>
                {
                    dynamic motorista = m;
                    if (string.IsNullOrWhiteSpace(motorista.Foto))
                    {
                        motorista.Foto = "/images/barbudo.jpg";
                    }
                    return motorista;
                }).ToList<object>();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao recuperar motoristas do cache", ex);
                return new List<object>();
            }
        }
    }


    public class MotoristaDto
    {
        public Guid MotoristaId { get; set; }
        public string Nome { get; set; }
        public string Foto { get; set; }
    }
}


