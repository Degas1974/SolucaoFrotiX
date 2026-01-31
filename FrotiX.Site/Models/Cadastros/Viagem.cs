/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: Viagem.cs                                                                               ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Modelar viagens, agendamentos, ajustes e dados de finalização.                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: AgendamentoViagem, AjusteViagem, FinalizacaoViagem, Viagem, ViewModels                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core, IFormFile                                                 ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    // ==================================================================================================
    // AGENDAMENTO
    // ==================================================================================================
    // Representa dados de agendamento e recorrência de viagens.
    // ==================================================================================================
    public class AgendamentoViagem
    {
        // Combustível informado ao final da viagem.
        public string? CombustivelFinal { get; set; }
        // Combustível informado no início da viagem.
        public string? CombustivelInicial { get; set; }

        // Indica se deve criar uma viagem já fechada (uso em UI).
        [NotMapped]
        public bool CriarViagemFechada { get; set; }

        // Data do agendamento.
        public DateTime? DataAgendamento { get; set; }
        // Data de cancelamento.
        public DateTime? DataCancelamento { get; set; }
        // Data de criação do registro.
        public DateTime? DataCriacao { get; set; }

        // Datas específicas selecionadas (não mapeado).
        [NotMapped]
        public List<DateTime>? DataEspecifica { get; set; }

        // Data final da viagem.
        public DateTime? DataFinal { get; set; }
        // Data de finalização.
        public DateTime? DataFinalizacao { get; set; }
        // Data final da recorrência.
        public DateTime? DataFinalRecorrencia { get; set; }
        // Data inicial da viagem.
        public DateTime? DataInicial { get; set; }
        // Conjunto de datas selecionadas.
        public List<DateTime>? DatasSelecionadas { get; set; }
        // Descrição da viagem.
        public string? Descricao { get; set; }
        // Destino da viagem.
        public string? Destino { get; set; }
        // Dia do mês para recorrência.
        public int? DiaMesRecorrencia { get; set; }

        // Data a partir da qual editar recorrência (não mapeado).
        [NotMapped]
        public DateTime? EditarAPartirData { get; set; }

        // Indica se deve editar todas as recorrências (não mapeado).
        [NotMapped]
        public bool editarTodosRecorrentes { get; set; }

        // Evento associado.
        public Guid? EventoId { get; set; }
        // Finalidade da viagem.
        public string? Finalidade { get; set; }
        // Indica se foi criada por agendamento.
        public bool FoiAgendamento { get; set; }
        // Recorrência na sexta-feira.
        public bool? Friday { get; set; }
        // Hora de fim.
        public DateTime? HoraFim { get; set; }
        // Hora de início.
        public DateTime? HoraInicio { get; set; }
        // Intervalo de recorrência.
        public string? Intervalo { get; set; }
        // Km atual.
        public int? KmAtual { get; set; }
        // Km final.
        public int? KmFinal { get; set; }
        // Km inicial.
        public int? KmInicial { get; set; }
        // Recorrência na segunda-feira.
        public bool? Monday { get; set; }
        // Motorista associado.
        public Guid? MotoristaId { get; set; }
        // Número da ficha de vistoria.
        public int? NoFichaVistoria { get; set; }

        // Indica sucesso da operação (não mapeado).
        [NotMapped]
        public bool OperacaoBemSucedida { get; set; }

        // Origem da viagem.
        public string? Origem { get; set; }
        // Ramal do requisitante.
        public string? RamalRequisitante { get; set; }
        // Identificador da recorrência.
        public Guid? RecorrenciaViagemId { get; set; }
        // Flag de recorrência (texto).
        public string? Recorrente { get; set; }
        // Requisitante associado.
        public Guid? RequisitanteId { get; set; }
        // Recorrência no sábado.
        public bool? Saturday { get; set; }
        // Setor solicitante associado.
        public Guid? SetorSolicitanteId { get; set; }
        // Status da viagem.
        public string? Status { get; set; }
        // Status do agendamento.
        public bool StatusAgendamento { get; set; }
        // Recorrência no domingo.
        public bool? Sunday { get; set; }
        // Recorrência na quinta-feira.
        public bool? Thursday { get; set; }
        // Recorrência na terça-feira.
        public bool? Tuesday { get; set; }
        // Usuário que agendou.
        public string? UsuarioIdAgendamento { get; set; }
        // Usuário que cancelou.
        public string? UsuarioIdCancelamento { get; set; }
        // Usuário que criou.
        public string? UsuarioIdCriacao { get; set; }
        // Usuário que finalizou.
        public string? UsuarioIdFinalizacao { get; set; }
        // Veículo associado.
        public Guid? VeiculoId { get; set; }
        // Identificador da viagem.
        public Guid ViagemId { get; set; }
        // Recorrência na quarta-feira.
        public bool? Wednesday { get; set; }
    }

    // ==================================================================================================
    // AJUSTE DE VIAGEM
    // ==================================================================================================
    // Dados utilizados para ajuste de viagens após criação.
    // ==================================================================================================
    public class AjusteViagem
    {
        // Arquivo de foto anexado (não mapeado).
        [NotMapped]
        public IFormFile? ArquivoFoto { get; set; }

        // Data final ajustada.
        public DateTime? DataFinal { get; set; }
        // Data inicial ajustada.
        public DateTime? DataInicial { get; set; }
        // Evento associado.
        public Guid? EventoId { get; set; }
        // Finalidade ajustada.
        public string? Finalidade { get; set; }
        // Hora final ajustada.
        public DateTime? HoraFim { get; set; }
        // Hora inicial ajustada.
        public DateTime? HoraInicial { get; set; }
        // Km final ajustado.
        public int? KmFinal { get; set; }
        // Km inicial ajustado.
        public int? KmInicial { get; set; }
        // Motorista associado.
        public Guid? MotoristaId { get; set; }
        // Número da ficha de vistoria.
        public int? NoFichaVistoria { get; set; }
        // Setor solicitante.
        public Guid? SetorSolicitanteId { get; set; }
        // Veículo associado.
        public Guid? VeiculoId { get; set; }
        // Identificador da viagem.
        public Guid ViagemId { get; set; }
    }

    // ==================================================================================================
    // FINALIZAÇÃO DE VIAGEM
    // ==================================================================================================
    // DTO para finalização de viagem (ocorrências via tabela separada).
    // ==================================================================================================
    public class FinalizacaoViagem
    {
        // Arquivo de foto anexado (não mapeado).
        [NotMapped]
        public IFormFile? ArquivoFoto { get; set; }

        // Combustível final registrado.
        public string? CombustivelFinal { get; set; }
        // Data final da viagem.
        public DateTime? DataFinal { get; set; }
        // Descrição informada na finalização.
        public string? Descricao { get; set; }
        // Hora de fim.
        public DateTime? HoraFim { get; set; }
        // Km final registrado.
        public int? KmFinal { get; set; }
        // Status do cartão de abastecimento.
        public string? StatusCartaoAbastecimento { get; set; }
        // Status do documento.
        public string? StatusDocumento { get; set; }
        // Identificador da viagem.
        public Guid ViagemId { get; set; }

        // ✅ NOVO: Lista de ocorrências múltiplas
        // Ocorrências associadas à finalização.
        public List<OcorrenciaFinalizacaoDTO>? Ocorrencias { get; set; }
    }

    // ==================================================================================================
    // OCORRÊNCIA DE FINALIZAÇÃO
    // ==================================================================================================
    // DTO para cada ocorrência enviada na finalização.
    // ==================================================================================================
    public class OcorrenciaFinalizacaoDTO
    {
        // Resumo da ocorrência.
        public string? Resumo { get; set; }
        // Descrição detalhada.
        public string? Descricao { get; set; }
        // Imagem da ocorrência (base64 ou referência).
        public string? ImagemOcorrencia { get; set; }
    }

    // ==================================================================================================
    // PROCURA DE VIAGEM
    // ==================================================================================================
    // ViewModel para filtros de busca de viagens.
    // ==================================================================================================
    public class ProcuraViagemViewModel
    {
        // Data para busca.
        public string? Data { get; set; }
        // Hora para busca.
        public string? Hora { get; set; }
        // Número da ficha de vistoria.
        public int? NoFichaVistoria { get; set; }
        // Veículo associado.
        public Guid? VeiculoId { get; set; }
        // Entidade de viagem encontrada.
        public Viagem? Viagem { get; set; }
    }

    // ==================================================================================================
    // VIAGEM
    // ==================================================================================================
    // Entidade principal de viagens (ocorrências em tabela separada).
    // ==================================================================================================
    public class Viagem
    {
        // Arquivo de foto anexado (não mapeado).
        [NotMapped]
        public IFormFile? ArquivoFoto { get; set; }

        // Combustível final.
        [Display(Name = "Combustível Final")]
        public string? CombustivelFinal { get; set; }

        // Combustível inicial.
        [Display(Name = "Combustível Inicial")]
        public string? CombustivelInicial { get; set; }

        // Custos da viagem.
        public double? CustoCombustivel { get; set; }
        public double? CustoLavador { get; set; }
        public double? CustoMotorista { get; set; }
        public double? CustoOperador { get; set; }
        public double? CustoVeiculo { get; set; }

        // Datas de controle.
        public DateTime? DataAgendamento { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public DateTime? DataCriacao { get; set; }

        [Display(Name = "Data Final")]
        public DateTime? DataFinal { get; set; }

        public DateTime? DataFinalizacao { get; set; }
        public DateTime? DataFinalRecorrencia { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data Inicial")]
        public DateTime? DataInicial { get; set; }

        // Descrição da viagem.
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        // Descrição sem formatação.
        public string? DescricaoSemFormato { get; set; }

        // Conteúdos de descrição anexados.
        public byte[]? DescricaoViagemImagem { get; set; }
        public byte[]? DescricaoViagemWord { get; set; }

        // Destino da viagem.
        [Display(Name = "Destino")]
        public string? Destino { get; set; }

        public int? DiaMesRecorrencia { get; set; }

        // Data para edição a partir (não mapeado).
        [NotMapped]
        public DateTime? EditarAPartirData { get; set; }

        // Indica se editar todos recorrentes (não mapeado).
        [NotMapped]
        public bool? editarTodosRecorrentes { get; set; }

        // Navegação para evento.
        [ForeignKey("EventoId")]
        public virtual Evento? Evento { get; set; }

        // Evento associado.
        [Display(Name = "Evento")]
        public Guid? EventoId { get; set; }

        // Ficha de vistoria (arquivo).
        public byte[]? FichaVistoria { get; set; }

        // Finalidade da viagem.
        [Display(Name = "Finalidade")]
        public string? Finalidade { get; set; }

        // Indica se foi agendamento.
        public bool? FoiAgendamento { get; set; }
        // Recorrência na sexta-feira.
        public bool? Friday { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Hora Fim")]
        public DateTime? HoraFim { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Hora de Início")]
        public DateTime? HoraInicio { get; set; }

        // Intervalo de recorrência.
        public string? Intervalo { get; set; }

        [Display(Name = "Km Atual")]
        public int? KmAtual { get; set; }

        [Display(Name = "Km Final")]
        public int? KmFinal { get; set; }

        [Display(Name = "Km Inicial")]
        public int? KmInicial { get; set; }

        // Km rodado calculado (KmFinal - KmInicial).
        [Display(Name = "Km Rodado")]
        public int? KmRodado { get; set; }

        // Minutos calculados.
        public int? Minutos { get; set; }
        // Recorrência na segunda-feira.
        public bool? Monday { get; set; }

        // Navegação para motorista.
        [ForeignKey("MotoristaId")]
        public virtual Motorista? Motorista { get; set; }

        // Motorista associado.
        [Display(Name = "Motorista")]
        public Guid? MotoristaId { get; set; }

        [Display(Name = "Nº Ficha Vistoria")]
        public int? NoFichaVistoria { get; set; }

        [Display(Name = "Nome do Evento")]
        public string? NomeEvento { get; set; }

        // Indica sucesso da operação (não mapeado).
        [NotMapped]
        public bool? OperacaoBemSucedida { get; set; }

        [Display(Name = "Origem")]
        public string? Origem { get; set; }

        [Display(Name = "Ramal")]
        public string? RamalRequisitante { get; set; }

        // Identificador da recorrência.
        public Guid? RecorrenciaViagemId { get; set; }
        // Descrição de recorrência.
        public string? Recorrente { get; set; }

        // Navegação para requisitante.
        [ForeignKey("RequisitanteId")]
        public virtual Requisitante? Requisitante { get; set; }

        // Requisitante associado.
        [Display(Name = "Usuário Requisitante")]
        public Guid? RequisitanteId { get; set; }

        // Recorrência no sábado.
        public bool? Saturday { get; set; }

        // Navegação para setor solicitante.
        [ForeignKey("SetorSolicitanteId")]
        public virtual SetorSolicitante? SetorSolicitante { get; set; }

        // Setor solicitante associado.
        [Display(Name = "Setor Solicitante")]
        public Guid? SetorSolicitanteId { get; set; }

        // Status da viagem.
        public string? Status { get; set; }
        // Status do agendamento.
        public bool? StatusAgendamento { get; set; }
        // Status do cartão de abastecimento.
        public string? StatusCartaoAbastecimento { get; set; }
        // Status do cartão de abastecimento final.
        public string? StatusCartaoAbastecimentoFinal { get; set; }
        // Status do documento.
        public string? StatusDocumento { get; set; }
        // Status do documento final.
        public string? StatusDocumentoFinal { get; set; }

        // Recorrência no domingo.
        public bool? Sunday { get; set; }
        // Recorrência na quinta-feira.
        public bool? Thursday { get; set; }
        // Recorrência na terça-feira.
        public bool? Tuesday { get; set; }
        // Usuário de agendamento.
        public string? UsuarioIdAgendamento { get; set; }
        // Usuário de cancelamento.
        public string? UsuarioIdCancelamento { get; set; }
        // Usuário de criação.
        public string? UsuarioIdCriacao { get; set; }
        // Usuário de finalização.
        public string? UsuarioIdFinalizacao { get; set; }

        // Navegação para veículo.
        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }

        // Veículo associado.
        [Display(Name = "Veículo")]
        public Guid? VeiculoId { get; set; }

        // Identificador da viagem.
        [Key]
        public Guid ViagemId { get; set; }

        // Recorrência na quarta-feira.
        public bool? Wednesday { get; set; }

        // ===== CINTA E TABLET (TODOS BIT/BOOL) =====
        // Indica se a cinta foi entregue.
        [Display(Name = "Cinta Entregue")]
        public bool? CintaEntregue { get; set; }

        // Indica se a cinta foi devolvida.
        [Display(Name = "Cinta Devolvida")]
        public bool? CintaDevolvida { get; set; }

        // Indica se o tablet foi entregue.
        [Display(Name = "Tablet Entregue")]
        public bool? TabletEntregue { get; set; }

        // Indica se o tablet foi devolvido.
        [Display(Name = "Tablet Devolvido")]
        public bool? TabletDevolvido { get; set; }

        // ===== VISTORIADORES =====
        // Vistoriador inicial.
        [Display(Name = "Vistoriador Inicial")]
        public string? VistoriadorInicialId { get; set; }

        // Vistoriador final.
        [Display(Name = "Vistoriador Final")]
        public string? VistoriadorFinalId { get; set; }

        // Rubrica inicial.
        public string? Rubrica { get; set; }
        // Rubrica final.
        public string? RubricaFinal { get; set; }

        // ================================================================
        // CAMPOS DE NORMALIZAÇÃO (Dashboard Administração)
        // ================================================================

        // Indica se a viagem passou por normalização.
        [Display(Name = "Foi Normalizada")]
        public bool? FoiNormalizada { get; set; }

        // Tipo de normalização aplicada (DATA_INVERTIDA, KM_INVERTIDO, etc.).
        [StringLength(500)]
        [Display(Name = "Tipo de Normalização")]
        public string? TipoNormalizacao { get; set; }

        // Data em que a normalização foi aplicada.
        [Display(Name = "Data da Normalização")]
        public DateTime? DataNormalizacao { get; set; }

        // Km rodado após normalização.
        [Display(Name = "Km Rodado Normalizado")]
        public int? KmRodadoNormalizado { get; set; }

        // Data inicial após normalização.
        [Display(Name = "Data Inicial Normalizada")]
        public DateTime? DataInicialNormalizada { get; set; }

        // Data final após normalização.
        [Display(Name = "Data Final Normalizada")]
        public DateTime? DataFinalNormalizada { get; set; }

        // Hora de início normalizada (TIME no SQL Server).
        [Display(Name = "Hora Início Normalizada")]
        public TimeSpan? HoraInicioNormalizada { get; set; }

        // Hora fim normalizada (TIME no SQL Server).
        [Display(Name = "Hora Fim Normalizada")]
        public TimeSpan? HoraFimNormalizada { get; set; }

        // Minutos calculados após normalização.
        [Display(Name = "Minutos Normalizado")]
        public int? MinutosNormalizado { get; set; }

        // Km inicial após normalização.
        [Display(Name = "Km Inicial Normalizado")]
        public int? KmInicialNormalizado { get; set; }

        // Km final após normalização.
        [Display(Name = "Km Final Normalizado")]
        public int? KmFinalNormalizado { get; set; }

        // ================================================================

        // Atualiza os dados da viagem a partir do agendamento.
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
                this.SetorSolicitanteId = viagem.SetorSolicitanteId ?? Guid.Empty;
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
                this.CombustivelInicial = viagem.CombustivelInicial;
                this.CombustivelFinal = viagem.CombustivelFinal;
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
        // Identificador da viagem.
        public Guid ViagemId { get; set; }
    }

    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar metadados e entidade de viagem para exibição.
    // ==================================================================================================
    public class ViagemViewModel
    {
        // Data de cancelamento.
        public DateTime? DataCancelamento { get; set; }
        // Data de finalização (texto).
        public string? DataFinalizacao { get; set; }
        // Ficha de vistoria anexada.
        public byte[]? FichaVistoria { get; set; }
        // Hora de finalização (texto).
        public string? HoraFinalizacao { get; set; }
        // Nome do usuário do agendamento.
        public string? NomeUsuarioAgendamento { get; set; }
        // Nome do usuário do cancelamento.
        public string? NomeUsuarioCancelamento { get; set; }
        // Nome do usuário da criação.
        public string? NomeUsuarioCriacao { get; set; }
        // Nome do usuário da finalização.
        public string? NomeUsuarioFinalizacao { get; set; }
        // Usuário que cancelou.
        public string? UsuarioIdCancelamento { get; set; }
        // Entidade principal de viagem.
        public Viagem? Viagem { get; set; }
        // Identificador da viagem.
        public Guid ViagemId { get; set; }
    }
}
