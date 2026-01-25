using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Mobile.Shared.Models
{
    public class AgendamentoViagem
    {
        public string? CombustivelFinal { get; set; }
        public string? CombustivelInicial { get; set; }

        [NotMapped]
        public bool? CriarViagemFechada { get; set; }

        public DateTime? DataAgendamento { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public DateTime? DataCriacao { get; set; }

        [NotMapped]
        public List<DateTime>? DataEspecifica { get; set; }

        public DateTime? DataFinal { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public DateTime? DataFinalRecorrencia { get; set; }
        public DateTime? DataInicial { get; set; }
        public List<DateTime>? DatasSelecionadas { get; set; }
        public string? Descricao { get; set; }
        public string? Destino { get; set; }
        public int? DiaMesRecorrencia { get; set; }

        [NotMapped]
        public DateTime? EditarAPartirData { get; set; }

        [NotMapped]
        public bool? editarTodosRecorrentes { get; set; }

        public Guid? EventoId { get; set; }
        public string? Finalidade { get; set; }
        public bool? FoiAgendamento { get; set; }
        public bool? Friday { get; set; }
        public DateTime? HoraFim { get; set; }
        public DateTime? HoraInicio { get; set; }
        public string? Intervalo { get; set; }
        public int? KmAtual { get; set; }
        public int? KmFinal { get; set; }
        public int? KmInicial { get; set; }
        public bool? Monday { get; set; }
        public Guid? MotoristaId { get; set; }
        public int? NoFichaVistoria { get; set; }

        [NotMapped]
        public bool? OperacaoBemSucedida { get; set; }

        public string? Origem { get; set; }
        public string? RamalRequisitante { get; set; }
        public Guid? RecorrenciaViagemId { get; set; }
        public string? Recorrente { get; set; }
        public Guid? RequisitanteId { get; set; }
        public bool? Saturday { get; set; }
        public Guid? SetorSolicitanteId { get; set; }
        public string? Status { get; set; }
        public bool? StatusAgendamento { get; set; }
        public bool? Sunday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Tuesday { get; set; }
        public string? UsuarioIdAgendamento { get; set; }
        public string? UsuarioIdCancelamento { get; set; }
        public string? UsuarioIdCriacao { get; set; }
        public string? UsuarioIdFinalizacao { get; set; }
        public Guid? VeiculoId { get; set; }
        public Guid ViagemId { get; set; }
        public bool? Wednesday { get; set; }
    }

    public class AjusteViagem
    {
        [NotMapped]
        public byte[]? ArquivoFotoBytes { get; set; }

        public DateTime? DataFinal { get; set; }
        public DateTime? DataInicial { get; set; }
        public Guid? EventoId { get; set; }
        public string? Finalidade { get; set; }
        public DateTime? HoraFim { get; set; }
        public DateTime? HoraInicial { get; set; }
        public int? KmFinal { get; set; }
        public int? KmInicial { get; set; }
        public Guid? MotoristaId { get; set; }
        public int? NoFichaVistoria { get; set; }
        public Guid? SetorSolicitanteId { get; set; }
        public Guid? VeiculoId { get; set; }
        public Guid ViagemId { get; set; }
    }

    public class FinalizacaoViagem
    {
        [NotMapped]
        public byte[]? ArquivoFotoBytes { get; set; }

        public string? CombustivelFinal { get; set; }
        public DateTime? DataFinal { get; set; }
        public string? Descricao { get; set; }
        public string? DescricaoOcorrencia { get; set; }
        public DateTime? HoraFim { get; set; }
        public string? ImagemOcorrencia { get; set; }
        public int? KmFinal { get; set; }
        public string? ResumoOcorrencia { get; set; }
        public string? SolucaoOcorrencia { get; set; }
        public string? StatusCartaoAbastecimento { get; set; }
        public string? StatusDocumento { get; set; }
        public string? StatusOcorrencia { get; set; }
        public Guid ViagemId { get; set; }
    }

    public class ProcuraViagemViewModel
    {
        public string? Data { get; set; }
        public string? Hora { get; set; }
        public int? NoFichaVistoria { get; set; }
        public Guid? VeiculoId { get; set; }
        public Viagem? Viagem { get; set; }
    }

    public class Viagem
    {
        [Key]
        public Guid ViagemId { get; set; }

        // ===== RELACIONAMENTOS (apenas IDs - sem navegação para classes inexistentes) =====
        [Display(Name = "Veículo")]
        public Guid? VeiculoId { get; set; }
        [ForeignKey(nameof(VeiculoId))]
        public virtual Veiculo? Veiculo { get; set; }

        [Display(Name = "Setor Solicitante")]
        public Guid? SetorSolicitanteId { get; set; }
        [ForeignKey(nameof(SetorSolicitanteId))]
        public virtual SetorSolicitante? SetorSolicitante { get; set; }

        [Display(Name = "Usuário Requisitante")]
        public Guid? RequisitanteId { get; set; }
        [ForeignKey(nameof(RequisitanteId))]
        public virtual Requisitante? Requisitante { get; set; }

        [Display(Name = "Motorista")]
        public Guid? MotoristaId { get; set; }
        [ForeignKey(nameof(MotoristaId))]
        public virtual Motorista? Motorista { get; set; }

        [Display(Name = "Evento")]
        public Guid? EventoId { get; set; }
        // Removido: virtual Evento? Evento (não existe no Mobile)

        [Display(Name = "Item Manutenção")]
        public Guid? ItemManutencaoId { get; set; }
        // Removido: virtual ItensManutencao? ItensManutencao (não existe no Mobile)

        // ===== DADOS BÁSICOS =====
        [Display(Name = "Origem")]
        public string? Origem { get; set; }

        [Display(Name = "Destino")]
        public string? Destino { get; set; }

        [Display(Name = "Ramal")]
        public string? RamalRequisitante { get; set; }

        [Display(Name = "Finalidade")]
        public string? Finalidade { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public string? DescricaoSemFormato { get; set; }

        [Display(Name = "Nome do Evento")]
        public string? NomeEvento { get; set; }

        // ===== DATAS E HORAS =====
        [DataType(DataType.DateTime)]
        [Display(Name = "Data Inicial")]
        public DateTime? DataInicial { get; set; }

        [Display(Name = "Data Final")]
        public DateTime? DataFinal { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Hora de Início")]
        public DateTime? HoraInicio { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Hora Fim")]
        public DateTime? HoraFim { get; set; }

        public DateTime? DataAgendamento { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public DateTime? DataFinalRecorrencia { get; set; }

        public int? Minutos { get; set; }

        // ===== QUILOMETRAGEM =====
        [Column("KmAtual")]
        [Display(Name = "Km Atual")]
        public int? KmAtual { get; set; }

        [Column("KmInicial")]
        [Display(Name = "Km Inicial")]
        public int? KmInicial { get; set; }

        [Column("KmFinal")]
        [Display(Name = "Km Final")]
        public int? KmFinal { get; set; }

        // ===== FICHA VISTORIA =====
        [Column("NoFichaVistoria")]
        [Display(Name = "Nº Ficha Vistoria")]
        public int? NoFichaVistoria { get; set; }

        public byte[]? FichaVistoria { get; set; }

        // ===== CUSTOS =====
        [Column("CustoCombustivel")]
        public double? CustoCombustivel { get; set; }

        public double? CustoLavador { get; set; }

        [Column("CustoMotorista")]
        public double? CustoMotorista { get; set; }

        [Column("CustoVeiculo")]
        public double? CustoVeiculo { get; set; }

        [Column("CustoOperador")]
        public double? CustoOperador { get; set; }

        // ===== STATUS =====
        public string? Status { get; set; }
        public bool? StatusAgendamento { get; set; }

        [Column("StatusCartaoAbastecimento")]
        public string? StatusCartaoAbastecimento { get; set; }

        public string? StatusCartaoAbastecimentoFinal { get; set; }
        public string? StatusDocumento { get; set; }
        public string? StatusDocumentoFinal { get; set; }
        public string? StatusOcorrencia { get; set; }

        // ===== CINTA E TABLET (TODOS BIT/BOOL) =====
        [Display(Name = "Cinta Entregue")]
        public bool? CintaEntregue { get; set; }

        [Display(Name = "Cinta Devolvida")]
        public bool? CintaDevolvida { get; set; }

        [Display(Name = "Tablet Entregue")]
        public bool? TabletEntregue { get; set; }

        [Display(Name = "Tablet Devolvido")]
        public bool? TabletDevolvido { get; set; }

        // ===== VISTORIADORES =====
        [Display(Name = "Vistoriador Inicial")]
        public string? VistoriadorInicialId { get; set; }

        [Display(Name = "Vistoriador Final")]
        public string? VistoriadorFinalId { get; set; }

        // ===== VISTORIA INICIAL - CAMPOS =====
        public string? Rubrica { get; set; }
        public string? DanoAvaria { get; set; }

        [Column("CombustivelInicial")]
        [Display(Name = "Combustível Inicial")]
        public string? NivelCombustivelInicial { get; set; } = "Cheio";

        [Column("FotosBase64", TypeName = "varbinary(max)")]
        public byte[]? FotosBase64 { get; set; }

        [Column("VideosBase64", TypeName = "varbinary(max)")]
        public byte[]? VideosBase64 { get; set; }

        // ===== VISTORIA FINAL - CAMPOS =====
        public string? RubricaFinal { get; set; }
        public string? DanoAvariaFinal { get; set; }

        [Column("CombustivelFinal")]
        [Display(Name = "Combustível Final")]
        public string? NivelCombustivelFinal { get; set; } = "Cheio";

        [Column("FotosFinaisBase64", TypeName = "varbinary(max)")]
        public byte[]? FotosFinaisBase64 { get; set; }

        [Column("VideosFinaisBase64", TypeName = "varbinary(max)")]
        public byte[]? VideosFinaisBase64 { get; set; }

        // ===== OCORRÊNCIAS =====
        public string? DescricaoOcorrencia { get; set; }
        public string? DescricaoSolucaoOcorrencia { get; set; }
        public string? ResumoOcorrencia { get; set; }
        public string? ImagemOcorrencia { get; set; }

        // ===== ARQUIVOS/IMAGENS =====
        public byte[]? DescricaoViagemImagem { get; set; }
        public byte[]? DescricaoViagemWord { get; set; }

        // ===== RECORRÊNCIA =====
        public string? Recorrente { get; set; }
        public Guid? RecorrenciaViagemId { get; set; }
        public string? Intervalo { get; set; }
        public int? DiaMesRecorrencia { get; set; }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }
        public bool? Sunday { get; set; }

        // ===== USUÁRIOS =====
        public string? UsuarioIdAgendamento { get; set; }
        public string? UsuarioIdCancelamento { get; set; }
        public string? UsuarioIdCriacao { get; set; }
        public string? UsuarioIdFinalizacao { get; set; }

        // ===== NOMES DE USUÁRIO (preenchidos pela API) =====
        [NotMapped]
        public string? NomeUsuarioCriacao { get; set; }

        [NotMapped]
        public string? NomeUsuarioAgendamento { get; set; }

        [NotMapped]
        public string? NomeUsuarioCancelamento { get; set; }

        [NotMapped]
        public string? NomeUsuarioFinalizacao { get; set; }

        // ===== CONTROLE =====
        public bool? FoiAgendamento { get; set; }

        // ===== NOT MAPPED (Não vão para o banco) =====
        [NotMapped]
        public byte[]? ArquivoFotoBytes { get; set; }

        [NotMapped]
        public DateTime? EditarAPartirData { get; set; }

        [NotMapped]
        public bool? editarTodosRecorrentes { get; set; }

        [NotMapped]
        public bool? OperacaoBemSucedida { get; set; }

        [NotMapped]
        public DateTime? DataViagem { get; set; }

        [NotMapped]
        public DateTime DataInicialOrdenavel { get; set; }

        [NotMapped]
        public string NomeMotorista { get; set; } = string.Empty;

        [NotMapped]
        public string NomeVeiculo { get; set; } = string.Empty;

        [NotMapped]
        public string? FotosBase64Json { get; set; }

        [NotMapped]
        public string? VideosBase64Json { get; set; }

        [NotMapped]
        public string? FotosFinaisBase64Json { get; set; }

        [NotMapped]
        public string? VideosFinaisBase64Json { get; set; }

        // ===== MÉTODO AUXILIAR =====
        public void AtualizarDados(AgendamentoViagem? viagem)
        {
            if (viagem != null)
            {
                this.DataInicial = viagem.DataInicial;
                this.HoraInicio = viagem.HoraInicio;
                this.Finalidade = viagem.Finalidade;
                this.Origem = viagem.Origem;
                this.Destino = viagem.Destino;
                this.MotoristaId = viagem.MotoristaId;
                this.VeiculoId = viagem.VeiculoId;
                this.RequisitanteId = viagem.RequisitanteId;
                this.RamalRequisitante = viagem.RamalRequisitante;
                this.SetorSolicitanteId = viagem.SetorSolicitanteId;
                this.Descricao = viagem.Descricao;
                this.StatusAgendamento = viagem.StatusAgendamento;
                this.FoiAgendamento = viagem.FoiAgendamento;
                this.Status = viagem.Status;
                this.DataFinal = viagem.DataFinal;
                this.HoraFim = viagem.HoraFim;
                this.NoFichaVistoria = viagem.NoFichaVistoria;
                this.EventoId = viagem.EventoId;
                this.KmAtual = viagem.KmAtual ?? 0;
                this.KmInicial = viagem.KmInicial ?? 0;
                this.KmFinal = viagem.KmFinal ?? 0;
                this.NivelCombustivelInicial = viagem.CombustivelInicial;
                this.NivelCombustivelFinal = viagem.CombustivelFinal;
                this.UsuarioIdCriacao = viagem.UsuarioIdCriacao;
                this.DataCriacao = viagem.DataCriacao;
                this.UsuarioIdFinalizacao = viagem.UsuarioIdFinalizacao;
                this.DataFinalizacao = viagem.DataFinalizacao;
                this.Recorrente = viagem.Recorrente;
                this.RecorrenciaViagemId = viagem.RecorrenciaViagemId;
                this.Intervalo = viagem.Intervalo;
                this.DataFinalRecorrencia = viagem.DataFinalRecorrencia;
                this.Monday = viagem.Monday;
                this.Tuesday = viagem.Tuesday;
                this.Wednesday = viagem.Wednesday;
                this.Thursday = viagem.Thursday;
                this.Friday = viagem.Friday;
                this.Saturday = viagem.Saturday;
                this.Sunday = viagem.Sunday;
                this.DiaMesRecorrencia = viagem.DiaMesRecorrencia;
                this.editarTodosRecorrentes = viagem.editarTodosRecorrentes;
                this.EditarAPartirData = viagem.EditarAPartirData;
            }
        }
    }

    public class ViagemID
    {
        public Guid ViagemId { get; set; }
    }

    public class ViagemViewModel
    {
        public DateTime? DataCancelamento { get; set; }
        public string? DataFinalizacao { get; set; }
        public byte[]? FichaVistoria { get; set; }
        public string? HoraFinalizacao { get; set; }
        public string? NomeUsuarioAgendamento { get; set; }
        public string? NomeUsuarioCancelamento { get; set; }
        public string? NomeUsuarioCriacao { get; set; }
        public string? NomeUsuarioFinalizacao { get; set; }
        public string? UsuarioIdCancelamento { get; set; }
        public Viagem? Viagem { get; set; }
        public Guid ViagemId { get; set; }
    }
}
