/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: AlertasFrotiX.cs                                                                        ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Configurar e registrar alertas do sistema (prioridades, recorrência, vínculos).       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: AlertasFrotiX                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa alertas configurados/gerados pelo sistema.
    // ==================================================================================================
    public class AlertasFrotiX
    {
        // Identificador do alerta.
        [Key]
        public Guid AlertasFrotiXId
        {
            get; set;
        }

        // Título do alerta.
        [Required(ErrorMessage = "O título do alerta é obrigatório")]
        [StringLength(200 , ErrorMessage = "O título deve ter no máximo 200 caracteres")]
        public string? Titulo
        {
            get; set;
        }

        // Descrição detalhada do alerta.
        [Required(ErrorMessage = "A descrição do alerta é obrigatória")]
        [StringLength(1000 , ErrorMessage = "A descrição deve ter no máximo 1000 caracteres")]
        public string? Descricao
        {
            get; set;
        }

        // Tipo de alerta.
        [Required(ErrorMessage = "O tipo de alerta é obrigatório")]
        public TipoAlerta TipoAlerta
        {
            get; set;
        }

        // Prioridade do alerta.
        [Required(ErrorMessage = "A prioridade é obrigatória")]
        public PrioridadeAlerta Prioridade
        {
            get; set;
        }

        // Data de inserção.
        [Required]
        public DateTime? DataInsercao
        {
            get; set;
        }

        // Data de exibição do alerta (para recorrentes: início da série).
        public DateTime? DataExibicao
        {
            get; set;
        }

        // Data de expiração do alerta (para recorrentes: fim da série).
        public DateTime? DataExpiracao
        {
            get; set;
        }

        // Data de desativação do alerta
        public DateTime? DataDesativacao
        {
            get; set;
        }

        // Usuário que desativou o alerta.
        public string? DesativadoPor
        {
            get; set;
        }

        // Motivo da desativação.
        public string? MotivoDesativacao
        {
            get; set;
        }

        // Relacionamentos opcionais
        // Viagem associada.
        public Guid? ViagemId
        {
            get; set;
        }

        // Navegação para viagem.
        [ForeignKey("ViagemId")]
        public virtual Viagem Viagem
        {
            get; set;
        }

        // Manutenção associada.
        public Guid? ManutencaoId
        {
            get; set;
        }

        // Navegação para manutenção.
        [ForeignKey("ManutencaoId")]
        public virtual Manutencao Manutencao
        {
            get; set;
        }

        // Motorista associado.
        public Guid? MotoristaId
        {
            get; set;
        }

        // Navegação para motorista.
        [ForeignKey("MotoristaId")]
        public virtual Motorista Motorista
        {
            get; set;
        }

        // Veículo associado.
        public Guid? VeiculoId
        {
            get; set;
        }

        // Navegação para veículo.
        [ForeignKey("VeiculoId")]
        public virtual Veiculo Veiculo
        {
            get; set;
        }

        // Tipo de exibição do alerta (1-3: única, 4-8: recorrente).
        public TipoExibicaoAlerta TipoExibicao
        {
            get; set;
        }

        // Horário específico para exibição.
        public TimeSpan? HorarioExibicao
        {
            get; set;
        }

        // Usuário que criou o alerta
        // Identificador do usuário criador.
        [Required]
        public string? UsuarioCriadorId
        {
            get; set;
        }

        // Status do alerta
        // Indica se o alerta está ativo.
        public bool Ativo { get; set; } = true;

        // =====================================================================
        // CAMPOS DE RECORRÊNCIA - Baseados no design da tabela Viagem
        // =====================================================================

        // Dias da semana para recorrência semanal/quinzenal.
        public bool Monday { get; set; } = false;

        public bool Tuesday { get; set; } = false;
        public bool Wednesday { get; set; } = false;
        public bool Thursday { get; set; } = false;
        public bool Friday { get; set; } = false;
        public bool Saturday { get; set; } = false;
        public bool Sunday { get; set; } = false;

        // Dia do mês para recorrência mensal (1-31).
        public int? DiaMesRecorrencia
        {
            get; set;
        }

        // Lista de datas para recorrência variada (ex.: "2025-01-15,2025-01-20").
        public string? DatasSelecionadas
        {
            get; set;
        }

        // Referência ao alerta original da série recorrente (primeiro alerta da série).
        public Guid? RecorrenciaAlertaId
        {
            get; set;
        }

        [ForeignKey("RecorrenciaAlertaId")]
        public virtual AlertasFrotiX AlertaOriginal
        {
            get; set;
        }

        // Coleção de alertas que fazem parte da mesma série recorrente
        public virtual ICollection<AlertasFrotiX> AlertasRecorrentes
        {
            get; set;
        }

        public string DiasSemana { get; set; }           // Ex: "1,2,3,4,5"

        // =====================================================================
        // FIM DOS CAMPOS DE RECORRÊNCIA
        // =====================================================================

        // Navegação para AlertasUsuario
        public virtual ICollection<AlertasUsuario> AlertasUsuarios
        {
            get; set;
        }

        // Construtor
        public AlertasFrotiX()
        {
            AlertasFrotiXId = Guid.NewGuid();
            DataInsercao = DateTime.Now;
            AlertasUsuarios = new HashSet<AlertasUsuario>();
            AlertasRecorrentes = new HashSet<AlertasFrotiX>();
        }
    }

    // ==================================================================================================
    // RELACIONAMENTO ALERTA-USUÁRIO
    // ==================================================================================================
    // Tabela de relacionamento N:N entre alertas e usuários.
    // ==================================================================================================
    public class AlertasUsuario
    {
        // Identificador do relacionamento.
        [Key]
        public Guid AlertasUsuarioId
        {
            get; set;
        }

        // Alerta associado.
        [Required]
        public Guid AlertasFrotiXId
        {
            get; set;
        }

        // Navegação para alerta.
        [ForeignKey("AlertasFrotiXId")]
        public virtual AlertasFrotiX AlertasFrotiX
        {
            get; set;
        }

        // Usuário associado.
        [Required]
        public string UsuarioId
        {
            get; set;
        }

        // Navegação para usuário.
        [ForeignKey("UsuarioId")]
        public virtual AspNetUsers Usuario
        {
            get; set;
        }

        // Indica se o alerta foi lido.
        public bool Lido { get; set; } = false;

        // Data da leitura do alerta.
        public DateTime? DataLeitura
        {
            get; set;
        }

        // Indica se foi notificado.
        public bool Notificado { get; set; } = false;

        public AlertasUsuario()
        {
            AlertasUsuarioId = Guid.NewGuid();
        }

        // Controle de exclusão sem leitura
        // Indica se o alerta foi apagado.
        public bool Apagado
        {
            get; set;
        }

        // Data de exclusão.
        public DateTime? DataApagado
        {
            get; set;
        }

        // Data de notificação enviada.
        public DateTime? DataNotificacao
        {
            get; set;
        }
    }

    // Enums
    public enum TipoAlerta
    {
        [Display(Name = "Agendamento")]
        Agendamento = 1,

        [Display(Name = "Manutenção")]
        Manutencao = 2,

        [Display(Name = "Motorista")]
        Motorista = 3,

        [Display(Name = "Veículo")]
        Veiculo = 4,

        [Display(Name = "Anúncio")]
        Anuncio = 5,

        [Display(Name = "Diversos")]
        Diversos = 6
    }

    public enum PrioridadeAlerta
    {
        [Display(Name = "Baixa")]
        Baixa = 1,

        [Display(Name = "Média")]
        Media = 2,

        [Display(Name = "Alta")]
        Alta = 3
    }

    // Tipo de exibição do alerta (1-3: única, 4-8: recorrente).
    public enum TipoExibicaoAlerta
    {
        [Display(Name = "Ao abrir o sistema")]
        AoAbrir = 1,

        [Display(Name = "Em horário específico")]
        Horario = 2,

        [Display(Name = "Em data/hora específica")]
        DataHora = 3,

        [Display(Name = "Recorrente - Diário (seg-sex)")]
        RecorrenteDiario = 4,

        [Display(Name = "Recorrente - Semanal")]
        RecorrenteSemanal = 5,

        [Display(Name = "Recorrente - Quinzenal")]
        RecorrenteQuinzenal = 6,

        [Display(Name = "Recorrente - Mensal")]
        RecorrenteMensal = 7,

        [Display(Name = "Recorrente - Dias Variados")]
        RecorrenteDiasVariados = 8
    }
}
