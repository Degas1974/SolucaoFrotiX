/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Escalas.cs                                                                              ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidades para tipos de serviço, turnos e escalas de trabalho de motoristas.          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES: TipoServico, Turno, EscalaDiaria, ViewModels para escalas de trabalho                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    public class TipoServico
    {
        [Key]
        public Guid TipoServicoId { get; set; }

        [Required(ErrorMessage = "O nome do serviço é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome do serviço deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome do Serviço")]
        public string? NomeServico { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioIdAlteracao { get; set; }

        // NavegaÃ§Ã£o
        public virtual ICollection<EscalaDiaria> EscalasDiarias { get; set; }

        public TipoServico()
        {
            TipoServicoId = Guid.NewGuid();
            DataCriacao = DateTime.Now;
            EscalasDiarias = new HashSet<EscalaDiaria>();
        }
    }

    public class Turno
    {
        [Key]
        public Guid TurnoId { get; set; }

        [Required(ErrorMessage = "O nome do turno Ã© obrigatório")]
        [StringLength(50, ErrorMessage = "O nome do turno deve ter no máximo 50 caracteres")]
        [Display(Name = "Nome do Turno")]
        public string? NomeTurno { get; set; }

        [Required(ErrorMessage = "A hora de início é obrigatória")]
        [Display(Name = "Hora Início")]
        [DataType(DataType.Time)]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "A hora de fim é obrigatória")]
        [Display(Name = "Hora Fim")]
        [DataType(DataType.Time)]
        public TimeSpan HoraFim { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioIdAlteracao { get; set; }

        // NavegaÃ§Ã£o
        public virtual ICollection<EscalaDiaria> EscalasDiarias { get; set; }

        public Turno()
        {
            TurnoId = Guid.NewGuid();
            DataCriacao = DateTime.Now;
            EscalasDiarias = new HashSet<EscalaDiaria>();
        }
    }

    public class VAssociado
    {
        [Key]
        public Guid AssociacaoId { get; set; }

        [Required(ErrorMessage = "O motorista é obrigatório")]
        [Display(Name = "Motorista")]
        public Guid MotoristaId { get; set; }

        [Display(Name = "Veículo")]
        public Guid? VeiculoId { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatório")]
        [Display(Name = "Data Iní­cio")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data Fim")]
        [DataType(DataType.Date)]
        public DateTime? DataFim { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string? Observacoes { get; set; }

        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioIdAlteracao { get; set; }

        // Foreign Keys
        [ForeignKey("MotoristaId")]
        public virtual Motorista Motorista { get; set; }

        [ForeignKey("VeiculoId")]
        public virtual Veiculo Veiculo { get; set; }

        // NavegaÃ§Ã£o
        public virtual ICollection<EscalaDiaria> EscalasDiarias { get; set; }

        public VAssociado()
        {
            AssociacaoId = Guid.NewGuid();
            DataCriacao = DateTime.Now;
            EscalasDiarias = new HashSet<EscalaDiaria>();
        }
    }

    public class EscalaDiaria
    {
        [Key]
        public Guid EscalaDiaId { get; set; }

        [Display(Name = "Associação Motorista/Veí­culo")]
        public Guid? AssociacaoId { get; set; }

        [Required(ErrorMessage = "O tipo de serviço é obrigatório")]
        [Display(Name = "Tipo de Serviço")]
        public Guid TipoServicoId { get; set; }

        [Required(ErrorMessage = "O turno é obrigatório")]
        [Display(Name = "Turno")]
        public Guid TurnoId { get; set; }

        [Required(ErrorMessage = "A data da escala é obrigatória")]
        [Display(Name = "Data da Escala")]
        [DataType(DataType.Date)]
        public DateTime DataEscala { get; set; }

        [Required(ErrorMessage = "A hora de iní­cio é obrigatória")]
        [Display(Name = "Hora Início")]
        [DataType(DataType.Time)]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "A hora de fim é obrigatória")]
        [Display(Name = "Hora Fim")]
        [DataType(DataType.Time)]
        public TimeSpan HoraFim { get; set; }

        [Display(Name = "Início do Intervalo")]
        [DataType(DataType.Time)]
        public TimeSpan? HoraIntervaloInicio { get; set; }

        [Display(Name = "Fim do Intervalo")]
        [DataType(DataType.Time)]
        public TimeSpan? HoraIntervaloFim { get; set; }

        [StringLength(100, ErrorMessage = "A lotação deve ter no máximo 100 caracteres")]
        [Display(Name = "Lotação")]
        public string? Lotacao { get; set; }

        [Display(Name = "Número de Saídas")]
        public int NumeroSaidas { get; set; } = 0;

        [Required(ErrorMessage = "O status do motorista é obrigatório")]
        [StringLength(20)]
        [Display(Name = "Status")]
        public string StatusMotorista { get; set; } = "Disponí­vel";

        [Display(Name = "Requisitante")]
        public Guid? RequisitanteId { get; set; }

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string? Observacoes { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioIdAlteracao { get; set; }

        // Foreign Keys
        [ForeignKey("AssociacaoId")]
        public virtual VAssociado? Associacao { get; set; }

        [ForeignKey("TipoServicoId")]
        public virtual TipoServico TipoServico { get; set; }

        [ForeignKey("TurnoId")]
        public virtual Turno Turno { get; set; }

        [ForeignKey("RequisitanteId")]
        public virtual Requisitante? Requisitante { get; set; }

        public EscalaDiaria()
        {
            EscalaDiaId = Guid.NewGuid();
            DataCriacao = DateTime.Now;
            DataEscala = DateTime.Today;
        }
    }

    public class FolgaRecesso
    {
        [Key]
        public Guid FolgaId { get; set; }

        [Required(ErrorMessage = "O motorista é obrigatório")]
        [Display(Name = "Motorista")]
        public Guid MotoristaId { get; set; }

        [Required(ErrorMessage = "A data de inicio é obrigatória")]
        [Display(Name = "Data Iní­cio")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de fim é obrigatória")]
        [Display(Name = "Data Fim")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        [Required(ErrorMessage = "O tipo é obrigatório")]
        [StringLength(20)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } // Folga, Recesso

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string? Observacoes { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioIdAlteracao { get; set; }

        // Foreign Keys
        [ForeignKey("MotoristaId")]
        public virtual Motorista Motorista { get; set; }

        public FolgaRecesso()
        {
            FolgaId = Guid.NewGuid();
            DataCriacao = DateTime.Now;
        }
    }

    public class Ferias
    {
        [Key]
        public Guid FeriasId { get; set; }

        [Required(ErrorMessage = "O motorista titular é obrigatório")]
        [Display(Name = "Motorista Titular")]
        public Guid MotoristaId { get; set; }

        [Display(Name = "Motorista Substituto")]
        public Guid? MotoristaSubId { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória")]
        [Display(Name = "Data Início")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de fim é obrigatória")]
        [Display(Name = "Data Fim")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string? Observacoes { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioIdAlteracao { get; set; }

        // Foreign Keys
        [ForeignKey("MotoristaId")]
        public virtual Motorista MotoristaTitular { get; set; }

        [ForeignKey("MotoristaSubId")]
        public virtual Motorista? MotoristaSubstituto { get; set; }

        public Ferias()
        {
            FeriasId = Guid.NewGuid();
            DataCriacao = DateTime.Now;
        }
    }

    public class CoberturaFolga
    {
        [Key]
        public Guid CoberturaId { get; set; }

        [Required(ErrorMessage = "O motorista em folga é obrigatório")]
        [Display(Name = "Motorista em Folga")]
        public Guid MotoristaFolgaId { get; set; }

        [Required(ErrorMessage = "O motorista cobertor é obrigatório")]
        [Display(Name = "Motorista Cobertor")]
        public Guid MotoristaCoberturaId { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória")]
        [Display(Name = "Data Início")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de fim é obrigatória")]
        [Display(Name = "Data Fim")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        [StringLength(50)]
        [Display(Name = "Motivo")]
        public string? Motivo { get; set; } // Folga, FÃ©rias, Recesso, Atestado

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string? Observacoes { get; set; }

        public string? StatusOriginal { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioIdAlteracao { get; set; }

        // Foreign Keys
        [ForeignKey("MotoristaFolgaId")]
        public virtual Motorista MotoristaEmFolga { get; set; }

        [ForeignKey("MotoristaCoberturaId")]
        public virtual Motorista MotoristaCobrindo { get; set; }

        public CoberturaFolga()
        {
            CoberturaId = Guid.NewGuid();
            DataCriacao = DateTime.Now;
        }
    }

    public class ObservacoesEscala
    {
        [Key]
        public Guid ObservacaoId { get; set; }

        [Required(ErrorMessage = "A data da escala é obrigatória")]
        [Display(Name = "Data da Escala")]
        [DataType(DataType.Date)]
        public DateTime DataEscala { get; set; }

        [StringLength(200)]
        [Display(Name = "TÃ­tulo")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [Display(Name = "Descrição")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        [StringLength(20)]
        [Display(Name = "Prioridade")]
        public string Prioridade { get; set; } = "Normal"; // Baixa, Normal, Alta

        [Required(ErrorMessage = "A data de início de exibição é obrigatória")]
        [Display(Name = "Exibir De")]
        [DataType(DataType.Date)]
        public DateTime ExibirDe { get; set; }

        [Required(ErrorMessage = "A data de fim de exibição é obrigatória")]
        [Display(Name = "Exibir Até")]
        [DataType(DataType.Date)]
        public DateTime ExibirAte { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioIdAlteracao { get; set; }

        public ObservacoesEscala()
        {
            ObservacaoId = Guid.NewGuid();
            DataCriacao = DateTime.Now;
            DataEscala = DateTime.Today;
            ExibirDe = DateTime.Today;
            ExibirAte = DateTime.Today.AddDays(7);
        }
    }

    // Enum para Status do Motorista
    public enum StatusMotoristaEnum
    {
        Disponivel,
        EmViagem,
        Indisponivel,
        EmServico,
        Economildo
    }

    // Enum para Tipo de Turno
    public enum TurnoEnum
    {
        Matutino,
        Vespertino,
        Noturno
    }

    // Enum para LotaÃ§Ã£o
    public enum LotacaoEnum
    {
        Aeroporto,  
        Rodoviaria,      
        PGR,  
        Cefor,
        SetorObras,
        Outros
    }


}
