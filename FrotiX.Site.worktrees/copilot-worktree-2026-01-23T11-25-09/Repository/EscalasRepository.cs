// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║                                                                                                   ║
// ║   ███████╗███████╗ ██████╗ █████╗ ██╗      █████╗ ███████╗    ██████╗ ███████╗██████╗  ██████╗  ║
// ║   ██╔════╝██╔════╝██╔════╝██╔══██╗██║     ██╔══██╗██╔════╝    ██╔══██╗██╔════╝██╔══██╗██╔═══██╗ ║
// ║   █████╗  ███████╗██║     ███████║██║     ███████║███████╗    ██████╔╝█████╗  ██████╔╝██║   ██║ ║
// ║   ██╔══╝  ╚════██║██║     ██╔══██║██║     ██╔══██║╚════██║    ██╔══██╗██╔══╝  ██╔═══╝ ██║   ██║ ║
// ║   ███████╗███████║╚██████╗██║  ██║███████╗██║  ██║███████║    ██║  ██║███████╗██║     ╚██████╔╝ ║
// ║   ╚══════╝╚══════╝ ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝    ╚═╝  ╚═╝╚══════╝╚═╝      ╚═════╝  ║
// ║                                                                                                   ║
// ║   📋 ARQUIVO: EscalasRepository.cs                                                                ║
// ║   📂 LOCALIZAÇÃO: Repository/                                                                     ║
// ║   📅 DOCUMENTADO EM: 2026-01-14                                                                   ║
// ║   👤 AUTOR: GitHub Copilot (Documentação INTRA-CODE)                                              ║
// ║   ⚙️ TECNOLOGIAS: C#, .NET 10, EF Core, Repository Pattern                                       ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 📖 DESCRIÇÃO GERAL                                                                                ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ Arquivo consolidado contendo 11 REPOSITÓRIOS relacionados ao sistema de ESCALAS e GESTÃO DE      ║
// ║ MOTORISTAS. Este arquivo concentra toda a lógica de acesso a dados para o módulo de escalas      ║
// ║ (scheduling) do sistema FrotiX, incluindo:                                                        ║
// ║                                                                                                   ║
// ║ 1. TipoServicoRepository        - Tipos de serviço (Escolta, Atendimento, Emergência...)         ║
// ║ 2. TurnoRepository              - Turnos de trabalho (Diurno, Noturno, Integral...)              ║
// ║ 3. VAssociadoRepository         - Associações Motorista ↔ Veículo ativas/históricas              ║
// ║ 4. EscalaDiariaRepository       - CORE: Escalas diárias dos motoristas (COMPLEXO)                ║
// ║ 5. FolgaRecessoRepository       - Folgas e recessos dos motoristas                               ║
// ║ 6. FeriasRepository             - Períodos de férias dos motoristas                              ║
// ║ 7. CoberturaFolgaRepository     - Substituições de motoristas em folga                           ║
// ║ 8. ObservacoesEscalaRepository  - Observações gerais nas escalas diárias                         ║
// ║ 9. ViewEscalasCompletasRepository   - View consolidada (leitura) de escalas completas            ║
// ║ 10. ViewMotoristasVezRepository     - View "Motoristas da Vez" (menor nº viagens)                ║
// ║ 11. ViewStatusMotoristasRepository  - View status atual de todos motoristas                      ║
// ║                                                                                                   ║
// ║ Este arquivo implementa o padrão AGGREGATE ROOT: EscalaDiaria é o coração do sistema de escalas, ║
// ║ coordenando todos os demais conceitos (Turno, TipoServico, FolgaRecesso, Ferias, Coberturas).    ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 🎯 FUNCIONALIDADES PRINCIPAIS                                                                     ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ ✅ Gestão de Escalas Diárias                                                                      ║
// ║    • Criar/Editar/Excluir escalas para motoristas                                                ║
// ║    • Validação de conflitos de horários (motorista/veículo)                                      ║
// ║    • Controle de status (Disponível/Em Serviço/Encerrado)                                        ║
// ║    • Consultas complexas com múltiplos JOINs (ViewEscalasCompletas)                              ║
// ║                                                                                                   ║
// ║ ✅ Sistema de "Motoristas da Vez"                                                                 ║
// ║    • Listar motoristas disponíveis no momento (turno ativo)                                      ║
// ║    • Ordenação por menor número de viagens (justiça distributiva)                                ║
// ║    • Integração com tabela Viagem para contagem em tempo real                                    ║
// ║                                                                                                   ║
// ║ ✅ Controle de Folgas, Férias e Coberturas                                                        ║
// ║    • Folgas/Recessos programados                                                                 ║
// ║    • Períodos de férias com validação de conflitos                                               ║
// ║    • Sistema de cobertura (motorista substituto)                                                 ║
// ║                                                                                                   ║
// ║ ✅ Validações de Negócio                                                                          ║
// ║    • Motorista disponível (sem conflito + não em férias/folga)                                   ║
// ║    • Veículo disponível (sem conflito de horários)                                               ║
// ║    • Turno existe e está ativo                                                                   ║
// ║    • Validação de sobreposição de horários                                                       ║
// ║                                                                                                   ║
// ║ ✅ Consultas de Visualização (Views)                                                              ║
// ║    • ViewEscalasCompletas: JOIN completo com Motorista, Veículo, TipoServico, Turno             ║
// ║    • ViewMotoristasVez: Próximos motoristas disponíveis (menor fila)                             ║
// ║    • ViewStatusMotoristas: Status consolidado (Escala/Férias/Folga/Sem Escala)                   ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 📊 ARQUITETURA DO SISTEMA DE ESCALAS                                                              ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║  ┌────────────────────────────────────────────────────────────────────────────────┐              ║
// ║  │                          ESCALA DIÁRIA (Core)                                  │              ║
// ║  │  - Data, Hora Início/Fim, Intervalo                                            │              ║
// ║  │  - Status (Disponível, Em Serviço, Encerrado)                                  │              ║
// ║  │  - NumeroSaidas (contador de viagens realizadas)                               │              ║
// ║  └─────────┬────────────────────────────────────────────────────────┬─────────────┘              ║
// ║            │                                                        │                            ║
// ║    ┌───────▼───────┐                                      ┌─────────▼──────────┐                ║
// ║    │  ASSOCIAÇÃO   │                                      │   TIPO SERVIÇO     │                ║
// ║    │ Motorista <-> │                                      │   + TURNO          │                ║
// ║    │    Veículo    │                                      │   + REQUISITANTE   │                ║
// ║    └───────┬───────┘                                      └────────────────────┘                ║
// ║            │                                                                                     ║
// ║    ┌───────▼────────────────────────────────────────────────────┐                               ║
// ║    │        RESTRIÇÕES (Férias, Folgas, Coberturas)             │                               ║
// ║    │  - FeriasRepository: Período de férias                     │                               ║
// ║    │  - FolgaRecessoRepository: Folgas programadas              │                               ║
// ║    │  - CoberturaFolgaRepository: Motorista substituto          │                               ║
// ║    └────────────────────────────────────────────────────────────┘                               ║
// ║                                                                                                   ║
// ║  FLUXO DE VALIDAÇÃO:                                                                             ║
// ║  1. MotoristaDisponivelAsync() → Verifica conflitos de horário + férias + folgas                 ║
// ║  2. VeiculoDisponivelAsync() → Verifica conflito de horário do veículo                          ║
// ║  3. ExisteEscalaConflitanteAsync() → Valida sobreposição com outras escalas                     ║
// ║  4. Se OK, cria EscalaDiaria                                                                     ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ ⚠️ PADRÕES E OBSERVAÇÕES                                                                          ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ ✅ BOA PRÁTICA: Update() NÃO chama SaveChanges()                                                 ║
// ║    → Respeita o padrão Unit of Work (diferente de outros repositórios)                           ║
// ║    → Transações devem ser gerenciadas pelo Controller/Service                                    ║
// ║                                                                                                   ║
// ║ ✅ BOA PRÁTICA: Uso correto de AsTracking()                                                      ║
// ║    → Apenas em Update() para rastreamento de mudanças                                            ║
// ║    → Consultas usam AsNoTracking() implícito (performance)                                       ║
// ║                                                                                                   ║
// ║ ✅ BOA PRÁTICA: Validação antes de persistência                                                  ║
// ║    → ExisteNomeServicoAsync(excludeId) permite validação em edição                               ║
// ║    → VerificarConflitoHorarioAsync() impede sobreposições                                        ║
// ║                                                                                                   ║
// ║ 🔍 OBSERVAÇÃO: GetMotoristasVezAsync() - Lógica Complexa                                         ║
// ║    • Busca escalas ativas no momento (DataEscala = Hoje, HoraInicio <= Agora, HoraFim >= Agora) ║
// ║    • Conta viagens realizadas HOJE na tabela Viagem                                              ║
// ║    • Ordena por MENOR número de viagens (justiça distributiva)                                   ║
// ║    • Retorna TOP N motoristas (default = 5)                                                      ║
// ║    → Usado em tela "Despachante" para alocar viagens                                             ║
// ║                                                                                                   ║
// ║ 🔍 OBSERVAÇÃO: GetStatusMotoristasAsync() - Status Consolidado                                   ║
// ║    • JOIN com FolgaRecesso, Ferias, EscalaDiaria                                                 ║
// ║    • Prioridade: Férias > Folga > Escala > "Sem Escala"                                          ║
// ║    • Usado em dashboards e telas de gestão                                                       ║
// ║                                                                                                   ║
// ║ 🔍 OBSERVAÇÃO: IncrementarContadorViagemAsync() - Método Depreciado                              ║
// ║    • Comentário indica que contador agora vem da tabela Viagem                                   ║
// ║    • Mantido por compatibilidade (retorna true sempre)                                           ║
// ║    → Refatorar: Remover método e atualizar chamadas                                              ║
// ║                                                                                                   ║
// ║ 📊 OBSERVAÇÃO: Views (Read-Only Repositories)                                                     ║
// ║    • ViewEscalasCompletasRepository: Escala completa (todos relacionamentos)                     ║
// ║    • ViewMotoristasVezRepository: Fila de motoristas disponíveis                                 ║
// ║    • ViewStatusMotoristasRepository: Status atual de todos motoristas                            ║
// ║    → Views são mapeadas para entidades EF (sem tabela física, baseadas em queries)               ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 🔗 RELACIONAMENTOS                                                                                ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ EscalaDiaria (1) ──────────── (1) VAssociado (Motorista + Veículo)                               ║
// ║ EscalaDiaria (N) ──────────── (1) TipoServico                                                    ║
// ║ EscalaDiaria (N) ──────────── (1) Turno                                                          ║
// ║ EscalaDiaria (N) ──────────── (1) Requisitante [Opcional]                                        ║
// ║                                                                                                   ║
// ║ FolgaRecesso (N) ──────────── (1) Motorista                                                      ║
// ║ Ferias       (N) ──────────── (1) Motorista                                                      ║
// ║                                                                                                   ║
// ║ CoberturaFolga (1) ─────────── (1) Motorista (MotoristaFolgaId)                                  ║
// ║ CoberturaFolga (1) ─────────── (1) Motorista (MotoristaCoberturaId) [Substituto]                 ║
// ║                                                                                                   ║
// ║ ObservacoesEscala ────────────── [DataEscala] (associação lógica por data)                       ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 📌 ESTRUTURA DO ARQUIVO (11 Repositórios)                                                         ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ 1️⃣ TipoServicoRepository (Linhas ~15-60)           → Tipos de serviço (dropdown + validação)    ║
// ║ 2️⃣ TurnoRepository (Linhas ~62-119)                 → Turnos de trabalho (com validação horário) ║
// ║ 3️⃣ VAssociadoRepository (Linhas ~120-186)          → Associações Motorista<->Veículo            ║
// ║ 4️⃣ EscalaDiariaRepository (Linhas ~187-621) ⭐     → CORE - Escalas diárias (COMPLEXO)          ║
// ║ 5️⃣ FolgaRecessoRepository (Linhas ~622-664)        → Folgas e recessos                          ║
// ║ 6️⃣ FeriasRepository (Linhas ~665-712)              → Férias dos motoristas                       ║
// ║ 7️⃣ CoberturaFolgaRepository (Linhas ~713-759)      → Substituições de folga                      ║
// ║ 8️⃣ ObservacoesEscalaRepository (Linhas ~760-797)   → Observações nas escalas                     ║
// ║ 9️⃣ ViewEscalasCompletasRepository (Linhas ~798-825) → View consolidada (leitura)                 ║
// ║ 🔟 ViewMotoristasVezRepository (Linhas ~826-838)    → View "motoristas da vez"                    ║
// ║ 1️⃣1️⃣ ViewStatusMotoristasRepository (Linhas ~839-854) → View status atual                          ║
// ║                                                                                                   ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
{
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // [REPOSITÓRIO 1/11] TIPO SERVIÇO
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // Gerencia os tipos de serviço que podem ser atribuídos às escalas (Escolta, Atendimento, Emergência, etc.)
    // Métodos: GetListForDropDown (ativos), Update, ExisteNomeServicoAsync (validação unicidade)
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    public class TipoServicoRepository : Repository<TipoServico>, ITipoServicoRepository
    {
        private readonly FrotiXDbContext _db;

        public TipoServicoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetTipoServicoListForDropDown()
        {
            return _db.Set<TipoServico>()
                .Where(x => x.Ativo)
                .Select(i => new SelectListItem()
                {
                    Text = i.NomeServico,
                    Value = i.TipoServicoId.ToString()
                });
        }

        public void Update(TipoServico tipoServico)
        {
            var objFromDb = _db.Set<TipoServico>().AsTracking().FirstOrDefault(s => s.TipoServicoId == tipoServico.TipoServicoId);
            if (objFromDb != null)
            {
                objFromDb.NomeServico = tipoServico.NomeServico;
                objFromDb.Descricao = tipoServico.Descricao;
                objFromDb.Ativo = tipoServico.Ativo;
                objFromDb.DataAlteracao = DateTime.Now;
                objFromDb.UsuarioIdAlteracao = tipoServico.UsuarioIdAlteracao;
            }
        }

        public async Task<bool> ExisteNomeServicoAsync(string nomeServico, Guid? excludeId = null)
        {
            var query = _db.Set<TipoServico>().Where(x => x.NomeServico == nomeServico && x.Ativo);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.TipoServicoId != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // [REPOSITÓRIO 2/11] TURNO
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // Gerencia os turnos de trabalho (Diurno, Noturno, Integral, etc.) com controle de horários
    // Métodos: GetListForDropDown, Update, GetTurnoByNomeAsync, VerificarConflitoHorarioAsync
    // Validação complexa: Detecta sobreposição de horários entre turnos
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    public class TurnoRepository : Repository<Turno>, ITurnoRepository
    {
        private readonly FrotiXDbContext _db;

        public TurnoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Busca turnos ATIVOS para popular dropdown
        // Retorna SelectListItem com TurnoId (GUID) e NomeTurno
        public IEnumerable<SelectListItem> GetTurnoListForDropDown()
        {
            return _db.Set<Turno>()
                .Where(x => x.Ativo)
                .Select(i => new SelectListItem()
                {
                    Text = i.NomeTurno,
                    Value = i.TurnoId.ToString()
                });
        }

        // [ETAPA] Atualiza turno existente
        // Atualiza campos de horário (HoraInicio, HoraFim) + campos de controle
        // NÃO chama SaveChanges() (respeita Unit of Work)
        public void Update(Turno turno)
        {
            var objFromDb = _db.Set<Turno>().AsTracking().FirstOrDefault(s => s.TurnoId == turno.TurnoId);
            if (objFromDb != null)
            {
                objFromDb.NomeTurno = turno.NomeTurno;
                objFromDb.HoraInicio = turno.HoraInicio;
                objFromDb.HoraFim = turno.HoraFim;
                objFromDb.Ativo = turno.Ativo;
                objFromDb.DataAlteracao = DateTime.Now;
                objFromDb.UsuarioIdAlteracao = turno.UsuarioIdAlteracao;
            }
        }

        // [ETAPA] Busca turno por nome (apenas ativos)
        // Usado para validação de nomes duplicados ou busca rápida
        public async Task<Turno> GetTurnoByNomeAsync(string nomeTurno)
        {
            return await _db.Set<Turno>()
                .FirstOrDefaultAsync(x => x.NomeTurno == nomeTurno && x.Ativo);
        }

        // [ETAPA] Verifica se há conflito de horário entre turnos
        // Lógica complexa: Detecta sobreposição de intervalos de tempo
        // Casos: Início dentro do turno OU Fim dentro do turno OU Envolvendo completamente
        // Parâmetro excludeId: Ignora o próprio turno (validação em edição)
        public async Task<bool> VerificarConflitoHorarioAsync(TimeSpan horaInicio, TimeSpan horaFim, Guid? excludeId = null)
        {
            var query = _db.Set<Turno>().Where(x => x.Ativo);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.TurnoId != excludeId.Value);
            }

            return await query.AnyAsync(x =>
                (horaInicio >= x.HoraInicio && horaInicio < x.HoraFim) ||
                (horaFim > x.HoraInicio && horaFim <= x.HoraFim) ||
                (horaInicio <= x.HoraInicio && horaFim >= x.HoraFim)
            );
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // [REPOSITÓRIO 3/11] VASSOCIADO (Associação Motorista ↔ Veículo)
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // Gerencia associações entre motoristas e veículos (ativas e históricas)
    // Métodos: Update, GetAssociacaoAtivaAsync, GetHistoricoAssociacoesAsync, MotoristaTemVeiculoAsync
    // Controla DataInicio, DataFim, Ativo para rastreamento temporal das associações
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    public class VAssociadoRepository : Repository<VAssociado>, IVAssociadoRepository
    {
        private readonly FrotiXDbContext _db;

        public VAssociadoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Atualiza associação Motorista-Veículo
        // Campos: MotoristaId, VeiculoId, DataInicio, DataFim, Observacoes, Ativo
        // NÃO chama SaveChanges() (respeita Unit of Work)
        public void Update(VAssociado vAssociado)
        {
            var objFromDb = _db.Set<VAssociado>().AsTracking().FirstOrDefault(s => s.AssociacaoId == vAssociado.AssociacaoId);
            if (objFromDb != null)
            {
                objFromDb.MotoristaId = vAssociado.MotoristaId;
                objFromDb.VeiculoId = vAssociado.VeiculoId;
                objFromDb.DataInicio = vAssociado.DataInicio;
                objFromDb.DataFim = vAssociado.DataFim;
                objFromDb.Observacoes = vAssociado.Observacoes;
                objFromDb.Ativo = vAssociado.Ativo;
                objFromDb.DataAlteracao = DateTime.Now;
                objFromDb.UsuarioIdAlteracao = vAssociado.UsuarioIdAlteracao;
            }
        }

        // [ETAPA] Busca associação ATIVA de um motorista
        // Include: Motorista + Veiculo (eager loading)
        // Filtros: Ativo=true, DataFim=null OU DataFim > Agora (associação vigente)
        public async Task<VAssociado> GetAssociacaoAtivaAsync(Guid motoristaId)
        {
            return await _db.Set<VAssociado>()
                .Include(x => x.Motorista)
                .Include(x => x.Veiculo)
                .FirstOrDefaultAsync(x => x.MotoristaId == motoristaId &&
                                         x.Ativo &&
                                         (x.DataFim == null || x.DataFim > DateTime.Now));
        }

        // [ETAPA] Busca HISTÓRICO completo de associações de um motorista
        // Include: Motorista + Veiculo
        // Ordenação: DataInicio DESC (mais recentes primeiro)
        public async Task<List<VAssociado>> GetHistoricoAssociacoesAsync(Guid motoristaId)
        {
            return await _db.Set<VAssociado>()
                .Include(x => x.Motorista)
                .Include(x => x.Veiculo)
                .Where(x => x.MotoristaId == motoristaId)
                .OrderByDescending(x => x.DataInicio)
                .ToListAsync();
        }

        // [ETAPA] Verifica se motorista tem veículo associado em uma data específica
        // Validação: Ativo=true, DataInicio <= data, DataFim=null OU DataFim > data
        // Retorna bool (usado em validações de negócio)
        public async Task<bool> MotoristaTemVeiculoAsync(Guid motoristaId, DateTime data)
        {
            return await _db.Set<VAssociado>()
                .AnyAsync(x => x.MotoristaId == motoristaId &&
                              x.Ativo &&
                              x.DataInicio <= data &&
                              (x.DataFim == null || x.DataFim > data));
        }

        // [ETAPA] Busca associação VIGENTE em uma data específica
        // Similar a GetAssociacaoAtivaAsync, mas permite consultar datas históricas
        // Include: Motorista + Veiculo
        public async Task<VAssociado> GetAssociacaoPorDataAsync(Guid motoristaId, DateTime data)
        {
            return await _db.Set<VAssociado>()
                .Include(x => x.Motorista)
                .Include(x => x.Veiculo)
                .FirstOrDefaultAsync(x => x.MotoristaId == motoristaId &&
                                         x.Ativo &&
                                         x.DataInicio <= data &&
                                         (x.DataFim == null || x.DataFim > data));
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // [REPOSITÓRIO 4/11] ESCALA DIÁRIA ⭐ CORE DO SISTEMA
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // ⚠️ REPOSITÓRIO MAIS COMPLEXO DO ARQUIVO (435 LINHAS)
    // Gerencia as escalas diárias dos motoristas - coração do sistema de scheduling
    //
    // MÉTODOS PRINCIPAIS:
    // • Update() - Atualização de escala
    // • GetEscalasCompletasAsync() - Escalas do dia com JOIN completo (Motorista+Veículo+TipoServico+Turno)
    // • GetEscalaCompletaByIdAsync() - Escala individual completa
    // • GetEscalasPorFiltroAsync() - Busca avançada com múltiplos filtros
    // • GetMotoristasVezAsync() - ALGORITMO DE FILA: Motoristas disponíveis ordenados por menor nº viagens
    // • GetStatusMotoristasAsync() - Status consolidado (JOIN com Folgas, Férias, Escalas)
    // • AtualizarStatusMotoristaAsync() - Altera status (Disponível/Em Serviço/Encerrado)
    // • MotoristaDisponivelAsync() - Valida disponibilidade (horário + férias + folgas)
    // • VeiculoDisponivelAsync() - Valida disponibilidade do veículo
    // • ExisteEscalaConflitanteAsync() - Detecta conflitos de horário
    // • GetEscalasPorPeriodoAsync() - Consulta por intervalo de datas
    // • GetEscalasMotoristaAsync() - Escalas de um motorista específico
    //
    // REGRAS DE NEGÓCIO:
    // 1. Motorista só pode ter 1 escala ativa por horário (sem sobreposição)
    // 2. Veículo só pode ter 1 escala ativa por horário
    // 3. Motorista em férias/folga não pode ter escala
    // 4. Status: "Disponível" → "Em Serviço" → "Encerrado"
    // 5. NumeroSaidas: Contador de viagens realizadas (vem da tabela Viagem)
    //
    // VIEWS CONSTRUÍDAS:
    // • ViewEscalasCompletas: JOIN Motorista+Veículo+TipoServico+Turno+Requisitante
    // • ViewMotoristasVez: Algoritmo de fila (menor NumeroSaidas + HoraInicio)
    // • ViewStatusMotoristas: Status consolidado com prioridade (Férias > Folga > Escala)
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    public class EscalaDiariaRepository : Repository<EscalaDiaria>, IEscalaDiariaRepository
    {
        private readonly FrotiXDbContext _db;

        public EscalaDiariaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Atualiza escala diária existente
        // Campos atualizados: Associação, Tipo Serviço, Turno, Horários (Início/Fim/Intervalo),
        //                     Lotação, NumeroSaidas, Status, Requisitante, Observações
        // NÃO chama SaveChanges() (respeita Unit of Work)
        public void Update(EscalaDiaria escalaDiaria)
        {
            var objFromDb = _db.Set<EscalaDiaria>().AsTracking().FirstOrDefault(s => s.EscalaDiaId == escalaDiaria.EscalaDiaId);
            if (objFromDb != null)
            {
                objFromDb.AssociacaoId = escalaDiaria.AssociacaoId;
                objFromDb.TipoServicoId = escalaDiaria.TipoServicoId;
                objFromDb.TurnoId = escalaDiaria.TurnoId;
                objFromDb.DataEscala = escalaDiaria.DataEscala;
                objFromDb.HoraInicio = escalaDiaria.HoraInicio;
                objFromDb.HoraFim = escalaDiaria.HoraFim;
                objFromDb.HoraIntervaloInicio = escalaDiaria.HoraIntervaloInicio;
                objFromDb.HoraIntervaloFim = escalaDiaria.HoraIntervaloFim;
                objFromDb.Lotacao = escalaDiaria.Lotacao;
                objFromDb.NumeroSaidas = escalaDiaria.NumeroSaidas;
                objFromDb.StatusMotorista = escalaDiaria.StatusMotorista;
                objFromDb.RequisitanteId = escalaDiaria.RequisitanteId;
                objFromDb.Observacoes = escalaDiaria.Observacoes;
                objFromDb.DataAlteracao = DateTime.Now;
                objFromDb.UsuarioIdAlteracao = escalaDiaria.UsuarioIdAlteracao;
            }
        }

        // [ETAPA] Busca escalas completas do dia com JOIN COMPLETO
        // JOIN: EscalaDiaria → VAssociado → Motorista + ViewVeiculos + TipoServico + Turno + Requisitante
        // Constrói ViewModel ViewEscalasCompletas com TODOS os dados necessários para exibição
        // Filtro: data (default = Hoje), Ativo = true
        // Ordenação: HoraInicio ASC
        // Formatação: TimeSpan → string "hh:mm"
        public async Task<List<ViewEscalasCompletas>> GetEscalasCompletasAsync(DateTime? data = null)
        {
            var dataEscala = data ?? DateTime.Today;

            var query = from ed in _db.Set<EscalaDiaria>()
                        join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
                        from va in vaLeft.DefaultIfEmpty()
                        join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
                        from m in mLeft.DefaultIfEmpty()
                        join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
                        from v in vLeft.DefaultIfEmpty()
                        join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
                        join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
                        join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
                        from r in rLeft.DefaultIfEmpty()
                        where ed.DataEscala == dataEscala && ed.Ativo
                        orderby ed.HoraInicio
                        select new ViewEscalasCompletas
                        {
                            EscalaDiaId = ed.EscalaDiaId,
                            DataEscala = ed.DataEscala,
                            HoraInicio = ed.HoraInicio.ToString(@"hh\:mm"),
                            HoraFim = ed.HoraFim.ToString(@"hh\:mm"),
                            HoraIntervaloInicio = ed.HoraIntervaloInicio.HasValue ?
                                ed.HoraIntervaloInicio.Value.ToString(@"hh\:mm") : null,
                            HoraIntervaloFim = ed.HoraIntervaloFim.HasValue ?
                                ed.HoraIntervaloFim.Value.ToString(@"hh\:mm") : null,
                            NumeroSaidas = ed.NumeroSaidas,
                            StatusMotorista = ed.StatusMotorista,
                            Lotacao = ed.Lotacao,
                            Observacoes = ed.Observacoes,
                            MotoristaId = m.MotoristaId,
                            NomeMotorista = m.Nome,
                            Ponto = m.Ponto,
                            CPF = m.CPF,
                            CNH = m.CNH,
                            Celular01 = m.Celular01,
                            Foto = m.Foto,
                            VeiculoId = v.VeiculoId,
                            VeiculoDescricao = v.Descricao,
                            Placa = v.Placa,
                            Modelo = v.MarcaModelo,
                            TipoServicoId = ts.TipoServicoId,
                            NomeServico = ts.NomeServico,
                            TurnoId = t.TurnoId,
                            NomeTurno = t.NomeTurno,
                            RequisitanteId = r.RequisitanteId,
                            NomeRequisitante = r.Nome
                        };

            return await query.ToListAsync();
        }

        public async Task<ViewEscalasCompletas> GetEscalaCompletaByIdAsync(Guid id)
        {
            var query = from ed in _db.Set<EscalaDiaria>()
                        join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
                        from va in vaLeft.DefaultIfEmpty()
                        join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
                        from m in mLeft.DefaultIfEmpty()
                        join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
                        from v in vLeft.DefaultIfEmpty()
                        join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
                        join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
                        join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
                        from r in rLeft.DefaultIfEmpty()
                        where ed.EscalaDiaId == id && ed.Ativo
                        select new ViewEscalasCompletas
                        {
                            EscalaDiaId = ed.EscalaDiaId,
                            DataEscala = ed.DataEscala,
                            HoraInicio = ed.HoraInicio.ToString(@"hh\:mm"),
                            HoraFim = ed.HoraFim.ToString(@"hh\:mm"),
                            HoraIntervaloInicio = ed.HoraIntervaloInicio.HasValue ?
                                ed.HoraIntervaloInicio.Value.ToString(@"hh\:mm") : null,
                            HoraIntervaloFim = ed.HoraIntervaloFim.HasValue ?
                                ed.HoraIntervaloFim.Value.ToString(@"hh\:mm") : null,
                            NumeroSaidas = ed.NumeroSaidas,
                            StatusMotorista = ed.StatusMotorista,
                            Lotacao = ed.Lotacao,
                            Observacoes = ed.Observacoes,
                            MotoristaId = m.MotoristaId,
                            NomeMotorista = m.Nome,
                            Ponto = m.Ponto,
                            CPF = m.CPF,
                            CNH = m.CNH,
                            Celular01 = m.Celular01,
                            Foto = m.Foto,
                            VeiculoId = v.VeiculoId,
                            VeiculoDescricao = v.Descricao,
                            Placa = v.Placa,
                            Modelo = v.MarcaModelo,
                            TipoServicoId = ts.TipoServicoId,
                            NomeServico = ts.NomeServico,
                            TurnoId = t.TurnoId,
                            NomeTurno = t.NomeTurno,
                            RequisitanteId = r.RequisitanteId,
                            NomeRequisitante = r.Nome
                        };

            return await query.FirstOrDefaultAsync();
        }

        // [ETAPA] Busca escalas por FILTROS AVANÇADOS
        // Filtros possíveis: DataFiltro, TipoServicoId, Lotacao, VeiculoId, MotoristaId, StatusMotorista, TurnoId, TextoPesquisa
        // TextoPesquisa busca em: Nome Motorista, Placa Veículo, Observações
        // JOIN completo: EscalaDiaria → VAssociado → Motorista + ViewVeiculos + TipoServico + Turno + Requisitante
        // Ordenação: DataEscala ASC, HoraInicio ASC
        public async Task<List<ViewEscalasCompletas>> GetEscalasPorFiltroAsync(FiltroEscalaViewModel filtro)
        {
            var query = from ed in _db.Set<EscalaDiaria>()
                        join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId into vaLeft
                        from va in vaLeft.DefaultIfEmpty()
                        join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId into mLeft
                        from m in mLeft.DefaultIfEmpty()
                        join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
                        from v in vLeft.DefaultIfEmpty()
                        join ts in _db.Set<TipoServico>() on ed.TipoServicoId equals ts.TipoServicoId
                        join t in _db.Set<Turno>() on ed.TurnoId equals t.TurnoId
                        join r in _db.Set<Requisitante>() on ed.RequisitanteId equals r.RequisitanteId into rLeft
                        from r in rLeft.DefaultIfEmpty()
                        where ed.Ativo
                        select new { ed, va, m, v, ts, t, r };

            // Aplicar filtros
            if (filtro.DataFiltro.HasValue)
                query = query.Where(x => x.ed.DataEscala == filtro.DataFiltro.Value);

            if (filtro.TipoServicoId.HasValue)
                query = query.Where(x => x.ed.TipoServicoId == filtro.TipoServicoId.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Lotacao))
                query = query.Where(x => x.ed.Lotacao == filtro.Lotacao);

            if (filtro.VeiculoId.HasValue)
                query = query.Where(x => x.v != null && x.v.VeiculoId == filtro.VeiculoId.Value);

            if (filtro.MotoristaId.HasValue)
                query = query.Where(x => x.m != null && x.m.MotoristaId == filtro.MotoristaId.Value);

            if (!string.IsNullOrWhiteSpace(filtro.StatusMotorista))
                query = query.Where(x => x.ed.StatusMotorista == filtro.StatusMotorista);

            if (filtro.TurnoId.HasValue)
                query = query.Where(x => x.ed.TurnoId == filtro.TurnoId.Value);

            if (!string.IsNullOrWhiteSpace(filtro.TextoPesquisa))
            {
                query = query.Where(x =>
                    (x.m != null && x.m.Nome.Contains(filtro.TextoPesquisa)) ||
                    (x.v != null && x.v.Placa.Contains(filtro.TextoPesquisa)) ||
                    (x.ed.Observacoes != null && x.ed.Observacoes.Contains(filtro.TextoPesquisa))
                );
            }

            var result = await query.OrderBy(x => x.ed.DataEscala)
                                   .ThenBy(x => x.ed.HoraInicio)
                                   .Select(x => new ViewEscalasCompletas
                                   {
                                       EscalaDiaId = x.ed.EscalaDiaId,
                                       DataEscala = x.ed.DataEscala,
                                       HoraInicio = x.ed.HoraInicio.ToString(@"hh\:mm"),
                                       HoraFim = x.ed.HoraFim.ToString(@"hh\:mm"),
                                       NumeroSaidas = x.ed.NumeroSaidas,
                                       StatusMotorista = x.ed.StatusMotorista,
                                       Lotacao = x.ed.Lotacao,
                                       Observacoes = x.ed.Observacoes,
                                       MotoristaId = x.m.MotoristaId,
                                       NomeMotorista = x.m.Nome,
                                       Ponto = x.m.Ponto,
                                       VeiculoId = x.v.VeiculoId,
                                       VeiculoDescricao = x.v.Descricao,
                                       Placa = x.v.Placa,
                                       NomeServico = x.ts.NomeServico,
                                       NomeTurno = x.t.NomeTurno,
                                       NomeRequisitante = x.r.Nome
                                   }).ToListAsync();

            return result;
        }

        // [ETAPA] ⭐ ALGORITMO DE FILA: Busca motoristas disponíveis no momento
        // LÓGICA COMPLEXA:
        // 1. Busca escalas do DIA de hoje com Status = "Disponível"
        // 2. Filtra apenas motoristas cujo turno está ATIVO no momento (HoraInicio <= Agora <= HoraFim)
        // 3. Conta viagens realizadas HOJE na tabela Viagem (Status = "Realizada", DataFinalizacao = Hoje)
        // 4. Ordena por MENOR número de viagens (justiça distributiva)
        // 5. Desempate: HoraInicio ASC (quem começou primeiro)
        // 6. Retorna TOP N motoristas (default = 5)
        // Usado em: Tela Despachante para alocar viagens
        public async Task<List<ViewMotoristasVez>> GetMotoristasVezAsync(int quantidade = 5)
        {
            var hoje = DateTime.Today;
            var agora = DateTime.Now.TimeOfDay;

            // Primeiro, buscar contagem de viagens realizadas hoje
            var viagensHoje = await _db.Set<Viagem>()
                .Where(v => v.DataFinalizacao == hoje && v.Status == "Realizada")
                .GroupBy(v => v.MotoristaId)
                .Select(g => new
                {
                    MotoristaId = g.Key,
                    NumeroViagens = g.Count()
                })
                .ToListAsync();

            var query = from ed in _db.Set<EscalaDiaria>()
                        join va in _db.Set<VAssociado>() on ed.AssociacaoId equals va.AssociacaoId
                        join m in _db.Set<Motorista>() on va.MotoristaId equals m.MotoristaId
                        join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
                        from v in vLeft.DefaultIfEmpty()
                        where ed.DataEscala == hoje &&
                              ed.Ativo &&
                              ed.StatusMotorista == "Disponível" &&
                              ed.HoraInicio <= agora &&
                              ed.HoraFim >= agora
                        select new
                        {
                            ed,
                            m,
                            v
                        };

            var escalas = await query.ToListAsync();

            var resultado = escalas
                .Select(x => new ViewMotoristasVez
                {
                    MotoristaId = x.m.MotoristaId,
                    NomeMotorista = x.m.Nome,
                    Ponto = x.m.Ponto,
                    Foto = x.m.Foto,
                    DataEscala = x.ed.DataEscala,
                    NumeroSaidas = viagensHoje.FirstOrDefault(vh => vh.MotoristaId == x.m.MotoristaId)?.NumeroViagens ?? 0,
                    StatusMotorista = x.ed.StatusMotorista,
                    Lotacao = x.ed.Lotacao,
                    VeiculoDescricao = x.v?.Descricao,
                    Placa = x.v?.Placa,
                    HoraInicio = x.ed.HoraInicio.ToString(@"hh\:mm"),
                    HoraFim = x.ed.HoraFim.ToString(@"hh\:mm")
                })
                .OrderBy(x => x.NumeroSaidas)
                .ThenBy(x => x.HoraInicio)
                .Take(quantidade)
                .ToList();

            return resultado;
        }

        // [ETAPA] ⭐ STATUS CONSOLIDADO: Busca status atual de TODOS os motoristas
        // JOIN COMPLEXO com priorização de status:
        // 1. Motorista ativo (Status = true)
        // 2. JOIN LEFT com VAssociado ativo (veículo atual)
        // 3. JOIN LEFT com ViewVeiculos (dados do veículo)
        // 4. JOIN LEFT com EscalaDiaria do DIA (DataEscala = Hoje, Ativo = true)
        // 5. JOIN LEFT com FolgaRecesso (Hoje >= DataInicio AND Hoje <= DataFim, Ativo = true)
        // 6. JOIN LEFT com Ferias (Hoje >= DataInicio AND Hoje <= DataFim, Ativo = true)
        //
        // PRIORIZAÇÃO DE STATUS (lógica ternária aninhada):
        // Férias > FolgaRecesso.Tipo > EscalaDiaria.StatusMotorista > "Sem Escala"
        //
        // Usado em: Dashboards, telas de gestão, relatórios
        public async Task<List<ViewStatusMotoristas>> GetStatusMotoristasAsync()
        {
            var hoje = DateTime.Today;

            var query = from m in _db.Set<Motorista>()
                        join va in _db.Set<VAssociado>() on m.MotoristaId equals va.MotoristaId into vaLeft
                        from va in vaLeft.Where(x => x.Ativo).DefaultIfEmpty()
                        join v in _db.Set<ViewVeiculos>() on va.VeiculoId equals v.VeiculoId into vLeft
                        from v in vLeft.DefaultIfEmpty()
                        join ed in _db.Set<EscalaDiaria>() on va.AssociacaoId equals ed.AssociacaoId into edLeft
                        from ed in edLeft.Where(x => x.DataEscala == hoje && x.Ativo).DefaultIfEmpty()
                        join fr in _db.Set<FolgaRecesso>() on m.MotoristaId equals fr.MotoristaId into frLeft
                        from fr in frLeft.Where(x => hoje >= x.DataInicio && hoje <= x.DataFim && x.Ativo).DefaultIfEmpty()
                        join f in _db.Set<Ferias>() on m.MotoristaId equals f.MotoristaId into fLeft
                        from f in fLeft.Where(x => hoje >= x.DataInicio && hoje <= x.DataFim && x.Ativo).DefaultIfEmpty()
                        where m.Status == true
                        select new ViewStatusMotoristas
                        {
                            MotoristaId = m.MotoristaId,
                            Nome = m.Nome,
                            Ponto = m.Ponto,
                            StatusAtual = f != null ? "Férias" :
                                        fr != null ? fr.Tipo :
                                        ed != null ? ed.StatusMotorista :
                                        "Sem Escala",
                            DataEscala = ed.DataEscala,
                            NumeroSaidas = ed != null ? ed.NumeroSaidas : 0,
                            Placa = v.Placa,
                            Veiculo = v.Descricao
                        };

            return await query.ToListAsync();
        }

        // [ETAPA] Atualiza status de um motorista em sua escala do dia
        // Busca escala por MotoristaId (via Associacao) + Data + Ativo = true
        // Altera StatusMotorista (ex: "Disponível" → "Em Serviço" → "Encerrado")
        // ⚠️ USA .Update() E SaveChanges (EXCEÇÃO ao padrão Unit of Work)
        // Retorna: true se encontrou e atualizou, false se não encontrou escala
        public async Task<bool> AtualizarStatusMotoristaAsync(Guid motoristaId, string novoStatus, DateTime? data = null)
        {
            var dataEscala = data ?? DateTime.Today;

            var escala = await _db.Set<EscalaDiaria>().AsTracking()
                .AsTracking().FirstOrDefaultAsync(ed =>
                    ed.Associacao.MotoristaId == motoristaId &&
                    ed.DataEscala == dataEscala &&
                    ed.Ativo);

            if (escala != null)
            {
                escala.StatusMotorista = novoStatus;
                escala.DataAlteracao = DateTime.Now;
                _db.Update(escala);
                return true;
            }

            return false;
        }

        // [ETAPA] ⚠️ MÉTODO DEPRECIADO - Incrementa contador de viagens
        // OBSERVAÇÃO NO CÓDIGO: "Este método não é mais necessário pois o contador agora vem da tabela Viagem"
        // Mantido por compatibilidade (retorna true sempre)
        // TODO: Remover método e atualizar chamadas no código
        public async Task<bool> IncrementarContadorViagemAsync(Guid motoristaId, DateTime data)
        {
            // Este método não é mais necessário pois o contador agora vem da tabela Viagem
            // Mantido por compatibilidade
            return await Task.FromResult(true);
        }

        // [ETAPA] ⭐ VALIDAÇÃO COMPLEXA: Verifica se motorista está disponível para escala
        // TRÊS VERIFICAÇÕES:
        // 1. Não há conflito de horário com outras escalas do motorista (sobreposição)
        // 2. Motorista não está em Folga/Recesso no dia
        // 3. Motorista não está em Férias no dia
        // Retorna: true se disponível, false se indisponível
        public async Task<bool> MotoristaDisponivelAsync(Guid motoristaId, DateTime data, TimeSpan horaInicio, TimeSpan horaFim)
        {
            // Verificar se não há conflito com outras escalas
            var temConflito = await _db.Set<EscalaDiaria>()
                .AnyAsync(ed =>
                    ed.Associacao.MotoristaId == motoristaId &&
                    ed.DataEscala == data &&
                    ed.Ativo &&
                    ((horaInicio >= ed.HoraInicio && horaInicio < ed.HoraFim) ||
                     (horaFim > ed.HoraInicio && horaFim <= ed.HoraFim) ||
                     (horaInicio <= ed.HoraInicio && horaFim >= ed.HoraFim))
                );

            // Verificar se não está em folga/férias
            var estaEmFolga = await _db.Set<FolgaRecesso>()
                .AnyAsync(f => f.MotoristaId == motoristaId &&
                              f.DataInicio <= data &&
                              f.DataFim >= data &&
                              f.Ativo);

            var estaEmFerias = await _db.Set<Ferias>()
                .AnyAsync(f => f.MotoristaId == motoristaId &&
                               f.DataInicio <= data &&
                               f.DataFim >= data &&
                               f.Ativo);

            return !temConflito && !estaEmFolga && !estaEmFerias;
        }

        // [ETAPA] Verifica se VEÍCULO está disponível no horário
        // Lógica de sobreposição: Mesma lógica de Turno
        // - horaInicio dentro do período existente
        // - horaFim dentro do período existente
        // - Novo período envolve completamente o existente
        // Retorna: true se disponível, false se conflito
        public async Task<bool> VeiculoDisponivelAsync(Guid veiculoId, DateTime data, TimeSpan horaInicio, TimeSpan horaFim)
        {
            var temConflito = await _db.Set<EscalaDiaria>()
                .AnyAsync(ed =>
                    ed.Associacao.VeiculoId == veiculoId &&
                    ed.DataEscala == data &&
                    ed.Ativo &&
                    ((horaInicio >= ed.HoraInicio && horaInicio < ed.HoraFim) ||
                     (horaFim > ed.HoraInicio && horaFim <= ed.HoraFim) ||
                     (horaInicio <= ed.HoraInicio && horaFim >= ed.HoraFim))
                );

            return !temConflito;
        }

        // [ETAPA] Verifica se existe conflito de horário para um motorista em uma data
        // Similar a MotoristaDisponivelAsync, mas foca apenas em conflito de escalas (sem férias/folgas)
        // Parâmetro excludeId: Ignora a própria escala (validação em edição)
        // Usado em: Validação antes de criar/editar escala
        public async Task<bool> ExisteEscalaConflitanteAsync(Guid motoristaId, DateTime data, TimeSpan horaInicio, TimeSpan horaFim, Guid? excludeId = null)
        {
            var query = _db.Set<EscalaDiaria>()
                .Where(ed =>
                    ed.Associacao.MotoristaId == motoristaId &&
                    ed.DataEscala == data &&
                    ed.Ativo);

            if (excludeId.HasValue)
            {
                query = query.Where(ed => ed.EscalaDiaId != excludeId.Value);
            }

            return await query.AnyAsync(ed =>
                (horaInicio >= ed.HoraInicio && horaInicio < ed.HoraFim) ||
                (horaFim > ed.HoraInicio && horaFim <= ed.HoraFim) ||
                (horaInicio <= ed.HoraInicio && horaFim >= ed.HoraFim)
            );
        }

        // [ETAPA] Busca escalas em um intervalo de datas
        // Include: Associacao → Motorista + Veiculo, TipoServico, Turno, Requisitante
        // Filtros: DataEscala >= dataInicio AND DataEscala <= dataFim, Ativo = true
        // Ordenação: DataEscala ASC, HoraInicio ASC
        // Usado em: Relatórios, exportações, consultas de histórico
        public async Task<List<EscalaDiaria>> GetEscalasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _db.Set<EscalaDiaria>()
                .Include(x => x.Associacao)
                    .ThenInclude(a => a.Motorista)
                .Include(x => x.Associacao)
                    .ThenInclude(a => a.Veiculo)
                .Include(x => x.TipoServico)
                .Include(x => x.Turno)
                .Include(x => x.Requisitante)
                .Where(x => x.DataEscala >= dataInicio &&
                           x.DataEscala <= dataFim &&
                           x.Ativo)
                .OrderBy(x => x.DataEscala)
                .ThenBy(x => x.HoraInicio)
                .ToListAsync();
        }

        // [ETAPA] Busca escalas de um motorista específico (todas ou de uma data)
        // Include: Associacao → Motorista + Veiculo, TipoServico, Turno, Requisitante
        // Filtro: MotoristaId (via Associacao), Data (opcional), Ativo = true
        // Ordenação: DataEscala ASC, HoraInicio ASC
        public async Task<List<EscalaDiaria>> GetEscalasMotoristaAsync(Guid motoristaId, DateTime? data = null)
        {
            var query = _db.Set<EscalaDiaria>()
                .Include(x => x.Associacao)
                    .ThenInclude(a => a.Motorista)
                .Include(x => x.Associacao)
                    .ThenInclude(a => a.Veiculo)
                .Include(x => x.TipoServico)
                .Include(x => x.Turno)
                .Include(x => x.Requisitante)
                .Where(x => x.Associacao.MotoristaId == motoristaId && x.Ativo);

            if (data.HasValue)
            {
                query = query.Where(x => x.DataEscala == data.Value);
            }

            return await query.OrderBy(x => x.DataEscala)
                             .ThenBy(x => x.HoraInicio)
                             .ToListAsync();
        }
    }

    public class FolgaRecessoRepository : Repository<FrotiX.Models.FolgaRecesso>, IFolgaRecessoRepository
    {
        private readonly FrotiXDbContext _db;
        public FolgaRecessoRepository(FrotiXDbContext db) : base(db) => _db = db;

        // [ETAPA] Atualiza folga/recesso existente
        // Campos: MotoristaId, Tipo, DataInicio, DataFim, Ativo, Observacoes
        // NÃO chama SaveChanges() (respeita Unit of Work)
        public void Update(FrotiX.Models.FolgaRecesso folgaRecesso)
        {
            var set = _db.Set<FrotiX.Models.FolgaRecesso>();
            var obj = set.AsTracking().FirstOrDefault(x => x.FolgaId == folgaRecesso.FolgaId);
            if (obj != null)
            {
                obj.MotoristaId = folgaRecesso.MotoristaId;
                obj.Tipo = folgaRecesso.Tipo;
                obj.DataInicio = folgaRecesso.DataInicio;
                obj.DataFim = folgaRecesso.DataFim;
                obj.Ativo = folgaRecesso.Ativo;
                obj.Observacoes = folgaRecesso.Observacoes;
                obj.DataAlteracao = DateTime.Now;
                obj.UsuarioIdAlteracao = folgaRecesso.UsuarioIdAlteracao;
            }
        }

        // [ETAPA] Busca HISTÓRICO de folgas/recessos de um motorista
        // Ordenação: DataInicio DESC (mais recentes primeiro)
        public Task<List<FrotiX.Models.FolgaRecesso>> GetFolgasPorMotoristaAsync(Guid motoristaId)
            => _db.Set<FrotiX.Models.FolgaRecesso>()
                  .Where(x => x.MotoristaId == motoristaId)
                  .OrderByDescending(x => x.DataInicio)
                  .ToListAsync();

        // [ETAPA] Busca folgas/recessos ATIVOS em uma data específica
        // Filtro: Ativo = true, DataInicio <= data <= DataFim
        public Task<List<FrotiX.Models.FolgaRecesso>> GetFolgasAtivasAsync(DateTime data)
            => _db.Set<FrotiX.Models.FolgaRecesso>()
                  .Where(x => x.Ativo && x.DataInicio <= data && x.DataFim >= data)
                  .ToListAsync();

        // [ETAPA] Verifica se motorista está em folga/recesso em uma data
        // Validação usada em: Criação de escalas (motorista em folga não pode ter escala)
        public Task<bool> MotoristaEstaEmFolgaAsync(Guid motoristaId, DateTime data)
            => _db.Set<FrotiX.Models.FolgaRecesso>()
                  .AnyAsync(x => x.MotoristaId == motoristaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        // [ETAPA] Busca a folga/recesso ATIVA de um motorista em uma data
        // Retorna: Objeto FolgaRecesso ou null
        public Task<FrotiX.Models.FolgaRecesso> GetFolgaAtivaMotoristaAsync(Guid motoristaId, DateTime data)
            => _db.Set<FrotiX.Models.FolgaRecesso>()
                  .FirstOrDefaultAsync(x => x.MotoristaId == motoristaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);
    }

    public class FeriasRepository : Repository<FrotiX.Models.Ferias>, IFeriasRepository
    {
        private readonly FrotiXDbContext _db;
        public FeriasRepository(FrotiXDbContext db) : base(db) => _db = db;

        // [ETAPA] Atualiza período de férias existente
        // Campos: MotoristaId, DataInicio, DataFim, Ativo, Observacoes
        // NÃO chama SaveChanges() (respeita Unit of Work)
        public void Update(FrotiX.Models.Ferias ferias)
        {
            var set = _db.Set<FrotiX.Models.Ferias>();
            var obj = set.AsTracking().FirstOrDefault(x => x.FeriasId == ferias.FeriasId);
            if (obj != null)
            {
                obj.MotoristaId = ferias.MotoristaId;
                obj.DataInicio = ferias.DataInicio;
                obj.DataFim = ferias.DataFim;
                obj.Ativo = ferias.Ativo;
                obj.Observacoes = ferias.Observacoes;
                obj.DataAlteracao = DateTime.Now;
                obj.UsuarioIdAlteracao = ferias.UsuarioIdAlteracao;
            }
        }

        public Task<List<FrotiX.Models.Ferias>> GetFeriasPorMotoristaAsync(Guid motoristaId)
            => _db.Set<FrotiX.Models.Ferias>()
                  .Where(x => x.MotoristaId == motoristaId)
                  .OrderByDescending(x => x.DataInicio)
                  .ToListAsync();

        public Task<List<FrotiX.Models.Ferias>> GetFeriasAtivasAsync(DateTime data)
            => _db.Set<FrotiX.Models.Ferias>()
                  .Where(x => x.Ativo && x.DataInicio <= data && x.DataFim >= data)
                  .ToListAsync();

        public Task<bool> MotoristaEstaEmFeriasAsync(Guid motoristaId, DateTime data)
            => _db.Set<FrotiX.Models.Ferias>()
                  .AnyAsync(x => x.MotoristaId == motoristaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        public Task<FrotiX.Models.Ferias> GetFeriasAtivaMotoristaAsync(Guid motoristaId, DateTime data)
            => _db.Set<FrotiX.Models.Ferias>()
                  .FirstOrDefaultAsync(x => x.MotoristaId == motoristaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        // Sem a coluna de substituto no modelo, retornamos as férias ativas na data.
        public Task<List<FrotiX.Models.Ferias>> GetFeriasSemSubstitutoAsync(DateTime data)
            => _db.Set<FrotiX.Models.Ferias>()
                  .Where(x => x.Ativo && x.DataInicio <= data && x.DataFim >= data)
                  .ToListAsync();
    }

    public class CoberturaFolgaRepository : Repository<FrotiX.Models.CoberturaFolga>, ICoberturaFolgaRepository
    {
        private readonly FrotiXDbContext _db;
        public CoberturaFolgaRepository(FrotiXDbContext db) : base(db) => _db = db;

        public void Update(FrotiX.Models.CoberturaFolga coberturaFolga)
        {
            var set = _db.Set<FrotiX.Models.CoberturaFolga>();
            var obj = set.AsTracking().FirstOrDefault(x => x.CoberturaId == coberturaFolga.CoberturaId);
            if (obj != null)
            {
                obj.MotoristaFolgaId = coberturaFolga.MotoristaFolgaId;
                obj.MotoristaCoberturaId = coberturaFolga.MotoristaCoberturaId;
                obj.DataInicio = coberturaFolga.DataInicio;
                obj.DataFim = coberturaFolga.DataFim;
                obj.Ativo = coberturaFolga.Ativo;
                obj.Observacoes = coberturaFolga.Observacoes;
                obj.DataAlteracao = DateTime.Now;
                obj.UsuarioIdAlteracao = coberturaFolga.UsuarioIdAlteracao;
            }
        }

        public Task<List<FrotiX.Models.CoberturaFolga>> GetCoberturasAtivasAsync(DateTime data)
            => _db.Set<FrotiX.Models.CoberturaFolga>()
                  .Where(x => x.Ativo && x.DataInicio <= data && x.DataFim >= data)
                  .ToListAsync();

        public Task<FrotiX.Models.CoberturaFolga> GetCoberturaPorMotoristaAsync(Guid motoristaCoberturaId, DateTime data)
            => _db.Set<FrotiX.Models.CoberturaFolga>()
                  .FirstOrDefaultAsync(x => x.MotoristaCoberturaId == motoristaCoberturaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        public Task<bool> MotoristaEstaCobridoAsync(Guid motoristaFolgaId, DateTime data)
            => _db.Set<FrotiX.Models.CoberturaFolga>()
                  .AnyAsync(x => x.MotoristaFolgaId == motoristaFolgaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        public Task<FrotiX.Models.CoberturaFolga> GetCoberturaMotoristaAsync(Guid motoristaFolgaId, DateTime data)
            => _db.Set<FrotiX.Models.CoberturaFolga>()
                  .FirstOrDefaultAsync(x => x.MotoristaFolgaId == motoristaFolgaId && x.Ativo && x.DataInicio <= data && x.DataFim >= data);

        public Task<List<FrotiX.Models.CoberturaFolga>> GetHistoricoCoberturas(Guid motoristaId)
            => _db.Set<FrotiX.Models.CoberturaFolga>()
                  .Where(x => x.MotoristaFolgaId == motoristaId || x.MotoristaCoberturaId == motoristaId)
                  .OrderByDescending(x => x.DataInicio)
                  .ToListAsync();
    }

    public class ObservacoesEscalaRepository : Repository<FrotiX.Models.ObservacoesEscala>, IObservacoesEscalaRepository
    {
        private readonly FrotiXDbContext _db;
        public ObservacoesEscalaRepository(FrotiXDbContext db) : base(db) => _db = db;

        public void Update(FrotiX.Models.ObservacoesEscala observacaoEscala)
        {
            var set = _db.Set<FrotiX.Models.ObservacoesEscala>();
            var obj = set.AsTracking().FirstOrDefault(x => x.ObservacaoId == observacaoEscala.ObservacaoId);
            if (obj != null)
            {
                obj.DataEscala = observacaoEscala.DataEscala;
                obj.Titulo = observacaoEscala.Titulo;
                obj.Descricao = observacaoEscala.Descricao;
                obj.Ativo = observacaoEscala.Ativo;
                obj.DataAlteracao = DateTime.Now;
                obj.UsuarioIdAlteracao = observacaoEscala.UsuarioIdAlteracao;
            }
        }

        public Task<List<FrotiX.Models.ObservacoesEscala>> GetObservacoesAtivasAsync(DateTime data)
            => _db.Set<FrotiX.Models.ObservacoesEscala>()
                  .Where(x => x.Ativo && x.DataEscala.Date == data.Date)
                  .OrderBy(x => x.DataEscala)
                  .ToListAsync();

        public Task<List<FrotiX.Models.ObservacoesEscala>> GetObservacoesPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
            => _db.Set<FrotiX.Models.ObservacoesEscala>()
                  .Where(x => x.DataEscala.Date >= dataInicio.Date && x.DataEscala.Date <= dataFim.Date)
                  .OrderBy(x => x.DataEscala)
                  .ToListAsync();

        public Task<bool> ExisteObservacaoAsync(DateTime data, string titulo)
            => _db.Set<FrotiX.Models.ObservacoesEscala>()
                  .AnyAsync(x => x.DataEscala.Date == data.Date && x.Titulo == titulo);
    }

    public class ViewEscalasCompletasRepository : Repository<FrotiX.Models.ViewEscalasCompletas>, IViewEscalasCompletasRepository
    {
        private readonly FrotiXDbContext _db;
        public ViewEscalasCompletasRepository(FrotiXDbContext db) : base(db) => _db = db;

        // [ETAPA] Busca TODAS as escalas completas (view materializada)
        // Ordenação: DataEscala ASC, HoraInicio ASC
        public Task<List<FrotiX.Models.ViewEscalasCompletas>> GetAllAsync()
            => _db.Set<FrotiX.Models.ViewEscalasCompletas>()
                  .OrderBy(x => x.DataEscala)
                  .ThenBy(x => x.HoraInicio)
                  .ToListAsync();

        // [ETAPA] Busca escalas completas com PAGINAÇÃO e filtro genérico
        // Retorna: (Items, TotalCount) para construir paginação no frontend
        public async Task<(List<FrotiX.Models.ViewEscalasCompletas> Items, int TotalCount)> GetPaginatedAsync(
            Expression<Func<FrotiX.Models.ViewEscalasCompletas, bool>> filter,
            int page,
            int pageSize)
        {
            var query = _db.Set<FrotiX.Models.ViewEscalasCompletas>().Where(filter);
            var total = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.DataEscala)
                .ThenBy(x => x.HoraInicio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, total);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // [REPOSITÓRIO 10/11] VIEW MOTORISTAS VEZ (Read-Only)
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // View: Motoristas disponíveis ordenados por menor número de viagens (fila de atendimento)
    // Método: GetTopMotoristasAsync (retorna TOP N)
    // Ordenação: NumeroSaidas ASC, HoraInicio ASC
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    public class ViewMotoristasVezRepository : Repository<FrotiX.Models.ViewMotoristasVez>, IViewMotoristasVezRepository
    {
        private readonly FrotiXDbContext _db;
        public ViewMotoristasVezRepository(FrotiXDbContext db) : base(db) => _db = db;

        // [ETAPA] Busca TOP N motoristas da vez (menor número de viagens)
        // Usado em: Tela Despachante para distribuir viagens
        public Task<List<FrotiX.Models.ViewMotoristasVez>> GetTopMotoristasAsync(int quantidade = 5)
            => _db.Set<FrotiX.Models.ViewMotoristasVez>()
                  .OrderBy(x => x.NumeroSaidas)
                  .ThenBy(x => x.HoraInicio)
                  .Take(quantidade)
                  .ToListAsync();
    }

    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // [REPOSITÓRIO 11/11] VIEW STATUS MOTORISTAS (Read-Only)
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    // View: Status consolidado de todos motoristas (Férias/Folga/Escala/Sem Escala)
    // Métodos: GetStatusAtualizadoAsync, GetStatusMotoristaAsync (individual)
    // Priorização de status: Férias > Folga > Escala > "Sem Escala"
    // ═══════════════════════════════════════════════════════════════════════════════════════════════════════
    public class ViewStatusMotoristasRepository : Repository<FrotiX.Models.ViewStatusMotoristas>, IViewStatusMotoristasRepository
    {
        private readonly FrotiXDbContext _db;
        public ViewStatusMotoristasRepository(FrotiXDbContext db) : base(db) => _db = db;

        // [ETAPA] Busca status atualizado de TODOS os motoristas
        // Usado em: Dashboards, telas de gestão
        // Ordenação: Nome ASC
        public Task<List<FrotiX.Models.ViewStatusMotoristas>> GetStatusAtualizadoAsync()
            => _db.Set<FrotiX.Models.ViewStatusMotoristas>()
                  .OrderBy(x => x.Nome)
                  .ToListAsync();

        // [ETAPA] Busca status de UM motorista específico
        // Retorna: ViewStatusMotoristas ou null
        public Task<FrotiX.Models.ViewStatusMotoristas> GetStatusMotoristaAsync(Guid motoristaId)
            => _db.Set<FrotiX.Models.ViewStatusMotoristas>()
                  .FirstOrDefaultAsync(x => x.MotoristaId == motoristaId);
    }
}
