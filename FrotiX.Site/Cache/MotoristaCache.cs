/* ****************************************************************************************
 * ⚡ ARQUIVO: MotoristaCache.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Manter cache em memória de motoristas com fotos codificadas em Base64,
 *                   otimizando leitura de blobs para dropdowns e listagens.
 *
 * 📥 ENTRADAS     : IUnitOfWork para acesso ao banco (ViewMotoristasViagem).
 *
 * 📤 SAÍDAS       : Lista de objetos anônimos com MotoristaId, Nome e Foto (Base64).
 *
 * 🔗 CHAMADA POR  : DI Container (Singleton) para uso em controllers e services.
 *
 * 🔄 CHAMA        : IUnitOfWork.ViewMotoristasViagem.GetAllReduced(), LoadMotoristas().
 *
 * 📦 DEPENDÊNCIAS : Entity Framework Core, IUnitOfWork, IServiceScopeFactory.
 *
 * 📝 OBSERVAÇÕES  : Usa lock para thread-safety. Cache carregado no startup (construtor).
 *                   Fotos inválidas retornam fallback "/images/barbudo.jpg".
 **************************************************************************************** */

using FrotiX.Repository;
using global::FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Cache
    {
    /****************************************************************************************
     * ⚡ CLASSE: MotoristaCache
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Cache Singleton para motoristas. Mantém lista em memória de
     *                   motoristas com fotos pré-processadas em Base64, evitando
     *                   consultas repetidas ao banco e processamento de blobs.
     *
     * 📥 ENTRADAS     : IUnitOfWork - acesso aos dados de motoristas
     *                   IServiceScopeFactory - para scopes assíncronos futuros
     *
     * 📤 SAÍDAS       : List<object> - lista de objetos {MotoristaId, Nome, Foto}
     *
     * 🔗 CHAMADA POR  : Controllers e services que precisam listar motoristas
     *
     * 🔄 CHAMA        : LoadMotoristas(), GetMotoristas()
     *
     * 📦 DEPENDÊNCIAS : IUnitOfWork, ViewMotoristasViagem
     *
     * 📝 OBSERVAÇÕES  : Thread-safe via lock. Cache inicializado no construtor (eager load).
     ****************************************************************************************/
    public class MotoristaCache
        {
        private readonly IUnitOfWork _unitOfWork;
        private List<object> _cachedMotoristas;
        private readonly object _lock = new();

        private readonly IServiceScopeFactory _scopeFactory;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: MotoristaCache
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar dependências e carregar cache de motoristas no startup.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork - acesso ao banco de dados
         *                   [IServiceScopeFactory] scopeFactory - para scopes futuros
         *
         * 📤 SAÍDAS       : Instância configurada com cache carregado
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container (configurado como Singleton)
         *
         * 🔄 CHAMA        : LoadMotoristas() - carrega cache inicial
         *
         * 📦 DEPENDÊNCIAS : IUnitOfWork, IServiceScopeFactory
         *
         * 📝 OBSERVAÇÕES  : Cache carregado no construtor (eager loading) para disponibilidade
         *                   imediata. Considerar lazy loading se a inicialização for pesada.
         ****************************************************************************************/
        public MotoristaCache(IUnitOfWork unitOfWork, IServiceScopeFactory scopeFactory)
            {
            _unitOfWork = unitOfWork;
            _scopeFactory = scopeFactory;
            LoadMotoristas(); // [DOC] Carrega cache no início para disponibilidade imediata
            }

        /****************************************************************************************
         * ⚡ MÉTODO: LoadMotoristas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Carregar lista de motoristas do banco e processar fotos para Base64,
         *                   armazenando em cache thread-safe.
         *
         * 📥 ENTRADAS     : Nenhuma (acessa diretamente ViewMotoristasViagem via UnitOfWork)
         *
         * 📤 SAÍDAS       : void - atualiza _cachedMotoristas internamente
         *
         * 🔗 CHAMADA POR  : Construtor (startup), ou métodos públicos para refresh do cache
         *
         * 🔄 CHAMA        : IUnitOfWork.ViewMotoristasViagem.GetAllReduced(),
         *                   Convert.ToBase64String()
         *
         * 📦 DEPENDÊNCIAS : IUnitOfWork, ViewMotoristasViagem, System.Convert
         *
         * 📝 OBSERVAÇÕES  : Thread-safe via lock. Converte blob de foto JPEG para Data URI
         *                   Base64. Se conversão falhar ou foto ausente, usa fallback
         *                   "/images/barbudo.jpg". Ordena por nome alfabético.
         ****************************************************************************************/
        public void LoadMotoristas()
            {
            try
            {
                lock (_lock) // [DOC] Garante thread-safety na escrita do cache
                {
                    // [DOC] Busca lista reduzida de motoristas (apenas Id, Nome, Foto) ordenada alfabeticamente
                    var motoristas = _unitOfWork.ViewMotoristasViagem.GetAllReduced(
                        selector: m => new
                            {
                            m.MotoristaId,
                            Nome = m.MotoristaCondutor,
                            m.Foto
                            },
                        orderBy: q => q.OrderBy(m => m.MotoristaCondutor)
                    ).ToList();

                    // [DOC] Converte fotos de blob (byte[]) para Data URI Base64 para uso direto em <img src="">
                    _cachedMotoristas = motoristas.Select(m =>
                    {
                        string fotoBase64;

                        // [DOC] Processa foto se existir e não for vazia
                        if (m.Foto != null && m.Foto.Length > 0)
                            {
                            try
                                {
                                // [DOC] Converte byte[] para Data URI Base64 (formato JPEG)
                                fotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(m.Foto)}";
                                }
                            catch
                                {
                                // [DOC] Falha na conversão - usa fallback
                                fotoBase64 = "/images/barbudo.jpg";
                                }
                            }
                        else
                            {
                            // [DOC] Motorista sem foto - usa fallback
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
            }
            catch (Exception ex)
            {
                // [DOC] Falha crítica no carregamento - registra erro mas não lança exceção
                // para não quebrar inicialização do cache (cache fica vazio até próximo refresh)
                Alerta.TratamentoErroComLinha("MotoristaCache.cs", "LoadMotoristas", ex);
                _cachedMotoristas = new List<object>(); // [DOC] Inicializa lista vazia em caso de erro
            }
            }


        /****************************************************************************************
         * ⚡ MÉTODO: GetMotoristas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista de motoristas em cache, garantindo que fotos vazias
         *                   usem o fallback correto.
         *
         * 📥 ENTRADAS     : Nenhuma (retorna cache interno _cachedMotoristas)
         *
         * 📤 SAÍDAS       : [List<object>] - lista de objetos {MotoristaId, Nome, Foto}
         *                   com fotos em formato Data URI Base64 ou fallback
         *
         * 🔗 CHAMADA POR  : Controllers e services que precisam listar motoristas
         *
         * 🔄 CHAMA        : _cachedMotoristas?.Select() para validação de fotos
         *
         * 📦 DEPENDÊNCIAS : Cache interno _cachedMotoristas
         *
         * 📝 OBSERVAÇÕES  : Valida fotos vazias novamente como camada adicional de segurança.
         *                   Retorna null se cache não foi inicializado (cenário de erro).
         ****************************************************************************************/
        public List<object> GetMotoristas()
            {
            try
            {
                // [DOC] Retorna lista com validação adicional de fotos vazias
                return _cachedMotoristas?.Select(m =>
                {
                    dynamic motorista = m;
                    
                    // [DOC] Se foto for null, vazia ou whitespace, usa fallback
                    if (string.IsNullOrWhiteSpace(motorista.Foto))
                        {
                        motorista.Foto = "/images/barbudo.jpg";
                        }
                    return motorista;
                }).ToList<object>();
            }
            catch (Exception ex)
            {
                // [DOC] Falha na recuperação do cache - registra erro e retorna lista vazia
                Alerta.TratamentoErroComLinha("MotoristaCache.cs", "GetMotoristas", ex);
                return new List<object>();
            }
            }
        }
    }


/****************************************************************************************
 * ⚡ CLASSE: MotoristaDto
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : DTO tipado para representar motorista com foto. Alternativa ao
 *                   objeto anônimo usado internamente no cache.
 *
 * 📥 ENTRADAS     : Propriedades setadas via inicializador ou binding
 *
 * 📤 SAÍDAS       : Instância tipada com MotoristaId, Nome e Foto
 *
 * 🔗 CHAMADA POR  : Código que prefere DTOs tipados ao invés de dynamic/object
 *
 * 🔄 CHAMA        : Nenhum método
 *
 * 📦 DEPENDÊNCIAS : System.Guid para MotoristaId
 *
 * 📝 OBSERVAÇÕES  : Atualmente não utilizado pelo MotoristaCache (que usa objetos anônimos).
 *                   Mantido para compatibilidade com código legado ou uso futuro.
 ****************************************************************************************/
public class MotoristaDto
    {
    /// <summary>
    /// Identificador único do motorista (Guid chave primária).
    /// </summary>
    public Guid MotoristaId { get; set; }
    
    /// <summary>
    /// Nome completo do motorista.
    /// </summary>
    public string Nome { get; set; }
    
    /// <summary>
    /// Foto do motorista em formato Data URI Base64 ou URL do fallback.
    /// Formato esperado: "data:image/jpeg;base64,..." ou "/images/barbudo.jpg".
    /// </summary>
    public string Foto { get; set; }
    }


