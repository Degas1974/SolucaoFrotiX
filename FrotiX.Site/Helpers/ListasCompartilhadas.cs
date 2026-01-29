/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ListasCompartilhadas.cs                                                                  ║
   ║ 📂 CAMINHO: /Helpers                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Provedores de listas para dropdowns e componentes EJ2 (DropDownList, TreeView). Inclui:         ║
   ║    ListaFinalidade, ListaNivelCombustivel, ListaVeiculos, ListaMotorista, ListaRequisitante,       ║
   ║    ListaEvento, ListaSetores (TreeView/Flat), ListaDias, ListaPeriodos, ListaStatus.               ║
   ║    Comparadores: PtBrComparer (ignora acentos), NaturalStringComparer (ordenação natural).        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ ── Comparadores ──                                                                                  ║
   ║ 1. [PtBrComparer.Compare]        : pt-BR ignora case/acentos... (x,y) -> int                       ║
   ║ 2. [NaturalStringComparer.Compare]: Ordenação natural numérica. (x,y) -> int                       ║
   ║ ── Listas ──                                                                                       ║
   ║ 3. [ListaFinalidade.FinalidadesList]    : Lista de finalidades viagem... () -> List                ║
   ║ 4. [ListaNivelCombustivel.NivelCombustivelList]: Níveis do tanque.... () -> List                   ║
   ║ 5. [ListaVeiculos.VeiculosList]         : Veículos ativos (placa/modelo) () -> IEnumerable         ║
   ║ 6. [ListaMotorista.MotoristaList]       : Motoristas ativos com foto... () -> IEnumerable          ║
   ║ 7. [ListaRequisitante.RequisitantesList]: Requisitantes ordenados..... () -> IEnumerable           ║
   ║ 8. [ListaEvento.EventosList]            : Eventos ativos.............. () -> IEnumerable           ║
   ║ 9. [ListaSetores.SetoresList]           : Setores hierárquicos TreeView () -> List                 ║
   ║10. [ListaSetoresEvento.SetoresEventoList]: Setores flat para eventos.. () -> List                  ║
   ║11. [ListaSetoresFlat.SetoresListFlat]   : Setores indentados dropdown. () -> List                  ║
   ║12. [ListaDias.DiasList]                 : Dias da semana pt-BR........ () -> List                  ║
   ║13. [ListaPeriodos.PeriodosList]         : Períodos (D/S/Q/M)........... () -> List                 ║
   ║14. [ListaRecorrente.RecorrenteList]     : Sim/Não para recorrência.... () -> List                  ║
   ║15. [ListaStatus.StatusList]             : Status de viagem............ () -> List                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: IUnitOfWork (Repository Pattern), System.Globalization                            ║
   ║ 📅 ATUALIZAÇÃO: 29/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FrotiX.Helpers
{
    #region Comparadores

    /// <summary>
    /// Comparador pt-BR que ignora case e acentos
    /// </summary>
    internal sealed class PtBrComparer :IComparer<string>
    {
        private static readonly CompareInfo Cmp = new CultureInfo("pt-BR").CompareInfo;

        public int Compare(string x , string y)
        {
        return Cmp.Compare(
            x ?? string.Empty ,
            y ?? string.Empty ,
            CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
        );
        }
    }

    /// <summary>
    /// Comparador para ordenação natural (trata números corretamente)
    /// Exemplo: "1 Nome" < "2 Nome" < "10 Nome" < "Aaaa" < "Bbbb"
    /// Números vêm antes de letras
    /// </summary>
    public class NaturalStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int ix = 0, iy = 0;

            while (ix < x.Length && iy < y.Length)
            {
                // Se ambos começam com dígito, compara numericamente
                if (char.IsDigit(x[ix]) && char.IsDigit(y[iy]))
                {
                    // Extrai sequência numérica
                    string numX = "";
                    while (ix < x.Length && char.IsDigit(x[ix]))
                    {
                        numX += x[ix];
                        ix++;
                    }

                    string numY = "";
                    while (iy < y.Length && char.IsDigit(y[iy]))
                    {
                        numY += y[iy];
                        iy++;
                    }

                    // Compara numericamente
                    int xNum = int.Parse(numX);
                    int yNum = int.Parse(numY);

                    if (xNum != yNum)
                        return xNum.CompareTo(yNum);
                }
                else
                {
                    // Compara caracteres alfabeticamente (case-insensitive, pt-BR)
                    int charComparison = string.Compare(
                        x[ix].ToString(),
                        y[iy].ToString(),
                        new CultureInfo("pt-BR"),
                        CompareOptions.IgnoreCase
                    );

                    if (charComparison != 0)
                        return charComparison;

                    ix++;
                    iy++;
                }
            }

            // Se chegou aqui, compara tamanho
            return x.Length.CompareTo(y.Length);
        }
    }

    #endregion

    #region Lista de Finalidades

    public class ListaFinalidade
    {
        public string Descricao
        {
            get; set;
        }
        public string FinalidadeId
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaFinalidade()
        {
        }

        public ListaFinalidade(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        public List<ListaFinalidade> FinalidadesList()
        {
        try
        {
        List<ListaFinalidade> finalidades = new List<ListaFinalidade>
                {
                    new ListaFinalidade { FinalidadeId = "Transporte de Funcionários", Descricao = "Transporte de Funcionários" },
                    new ListaFinalidade { FinalidadeId = "Transporte de Convidados", Descricao = "Transporte de Convidados" },
                    new ListaFinalidade { FinalidadeId = "Transporte de Materiais/Cargas", Descricao = "Transporte de Materiais/Cargas" },
                    new ListaFinalidade { FinalidadeId = "Economildo Norte(Cefor)", Descricao = "Economildo Norte(Cefor)" },
                    new ListaFinalidade { FinalidadeId = "Economildo Sul(PGR)", Descricao = "Economildo Sul(PGR)" },
                    new ListaFinalidade { FinalidadeId = "Economildo Rodoviária", Descricao = "Economildo Rodoviária" },
                    new ListaFinalidade { FinalidadeId = "Mesa (carros pretos)", Descricao = "Mesa (carros pretos)" },
                    new ListaFinalidade { FinalidadeId = "TV/Rádio Câmara", Descricao = "TV/Rádio Câmara" },
                    new ListaFinalidade { FinalidadeId = "Aeroporto", Descricao = "Aeroporto" },
                    new ListaFinalidade { FinalidadeId = "Saída para Manutenção", Descricao = "Saída para Manutenção" },
                    new ListaFinalidade { FinalidadeId = "Chegada da Manutenção", Descricao = "Chegada da Manutenção" },
                    new ListaFinalidade { FinalidadeId = "Abastecimento", Descricao = "Abastecimento" },
                    new ListaFinalidade { FinalidadeId = "Recebimento da Locadora", Descricao = "Recebimento da Locadora" },
                    new ListaFinalidade { FinalidadeId = "Devolução à Locadora", Descricao = "Devolução à Locadora" },
                    new ListaFinalidade { FinalidadeId = "Saída Programada", Descricao = "Saída Programada" },
                    new ListaFinalidade { FinalidadeId = "Evento", Descricao = "Evento" },
                    new ListaFinalidade { FinalidadeId = "Ambulância", Descricao = "Ambulância" },
                    new ListaFinalidade { FinalidadeId = "Enviado Depol", Descricao = "Enviado Depol" },
                    new ListaFinalidade { FinalidadeId = "Demanda Política", Descricao = "Demanda Política" },
                    new ListaFinalidade { FinalidadeId = "Passaporte", Descricao = "Passaporte" },
                    new ListaFinalidade { FinalidadeId = "Aviso", Descricao = "Aviso" },
                    new ListaFinalidade { FinalidadeId = "Cursos Depol", Descricao = "Cursos Depol" }
                };

        // Ordenar alfabeticamente em pt-BR (ignora acentos e maiúsculas/minúsculas)
        return finalidades.OrderBy(f => f.Descricao , new PtBrComparer()).ToList();
        }
        catch (Exception ex)
        {
        // Log do erro (ajuste conforme seu sistema de log)
        System.Diagnostics.Debug.WriteLine($"Erro em FinalidadesList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return new List<ListaFinalidade>();
        }
        }
    }

    #endregion

    #region Lista de Nível de Combustível

    public class ListaNivelCombustivel
    {
        public string Nivel
        {
            get; set;
        }
        public string Descricao
        {
            get; set;
        }
        public string Imagem
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaNivelCombustivel()
        {
        }

        public ListaNivelCombustivel(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        public List<ListaNivelCombustivel> NivelCombustivelList()
        {
        try
        {
        return new List<ListaNivelCombustivel>
                {
                    new ListaNivelCombustivel { Nivel = "tanquevazio", Descricao = "Vazio", Imagem = "../images/tanquevazio.png" },
                    new ListaNivelCombustivel { Nivel = "tanqueumquarto", Descricao = "1/4", Imagem = "../images/tanqueumquarto.png" },
                    new ListaNivelCombustivel { Nivel = "tanquemeiotanque", Descricao = "1/2", Imagem = "../images/tanquemeiotanque.png" },
                    new ListaNivelCombustivel { Nivel = "tanquetresquartos", Descricao = "3/4", Imagem = "../images/tanquetresquartos.png" },
                    new ListaNivelCombustivel { Nivel = "tanquecheio", Descricao = "Cheio", Imagem = "../images/tanquecheio.png" }
                };
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"Erro em NivelCombustivelList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return new List<ListaNivelCombustivel>();
        }
        }
    }

    #endregion

    #region Lista de Veículos

    public class ListaVeiculos
    {
        public string Descricao
        {
            get; set;
        }
        public Guid VeiculoId
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaVeiculos()
        {
        }

        public ListaVeiculos(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        public IEnumerable<ListaVeiculos> VeiculosList()
        {
        try
        {
        var veiculos = (
            from v in _unitOfWork.Veiculo.GetAllReduced(
                includeProperties: nameof(ModeloVeiculo) + "," + nameof(MarcaVeiculo) ,
                selector: v => new
                {
                    v.VeiculoId ,
                    v.Placa ,
                    v.MarcaVeiculo.DescricaoMarca ,
                    v.ModeloVeiculo.DescricaoModelo ,
                    v.Status ,
                }
            )
            where v.Status == true
            select new ListaVeiculos
            {
                VeiculoId = v?.VeiculoId ?? Guid.Empty ,
                Descricao = $"{v.Placa} - {v.DescricaoMarca}/{v.DescricaoModelo}" ,
            }
        ).OrderBy(v => v.Descricao);

        return veiculos;
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"Erro em VeiculosList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return Enumerable.Empty<ListaVeiculos>();
        }
        }
    }

    #endregion

    #region Lista de Motoristas

    public class ListaMotorista
    {
        public Guid MotoristaId
        {
            get; set;
        }
        public string Nome
        {
            get; set;
        }
        public string FotoBase64
        {
            get; set;
        }
        public bool Status
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaMotorista()
        {
        }

        public ListaMotorista(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        public IEnumerable<ListaMotorista> MotoristaList()
        {
        try
        {
        var motoristas = _unitOfWork.ViewMotoristas.GetAllReduced(
            orderBy: m => m.OrderBy(m => m.Nome) ,
            selector: motorista => new ListaMotorista
            {
                MotoristaId = motorista.MotoristaId ,
                Nome = motorista.MotoristaCondutor ,
                FotoBase64 = motorista.Foto != null
                    ? $"data:image/jpeg;base64,{Convert.ToBase64String(motorista.Foto)}"
                    : null ,
                Status = motorista.Status ,
            }
        );

        return motoristas.Where(m => m.Status == true);
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"Erro em MotoristaList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return Enumerable.Empty<ListaMotorista>();
        }
        }
    }

    #endregion

    #region Lista de Requisitantes

    public class ListaRequisitante
    {
        public string Requisitante
        {
            get; set;
        }
        public Guid RequisitanteId
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaRequisitante()
        {
        }

        public ListaRequisitante(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        public IEnumerable<ListaRequisitante> RequisitantesList()
        {
        try
        {
        // Busca dados sem ordenação no banco (melhor performance)
        var requisitantes = _unitOfWork.ViewRequisitantes.GetAllReduced(
            selector: r => new ListaRequisitante
            {
                Requisitante = r.Requisitante ,
                RequisitanteId = (Guid)r.RequisitanteId ,
            }
        ).ToList();

        // Trim e ordena usando comparador natural (números antes de letras, case-insensitive, pt-BR)
        // Trim em memória para garantir que não há espaços em branco afetando ordenação
        return requisitantes
            .Select(r => new ListaRequisitante
            {
                Requisitante = (r.Requisitante ?? "").Trim(),
                RequisitanteId = r.RequisitanteId
            })
            .OrderBy(r => r.Requisitante ?? "", new NaturalStringComparer())
            .ToList();
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"Erro em RequisitantesList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return Enumerable.Empty<ListaRequisitante>();
        }
        }
    }

    #endregion

    #region Lista de Eventos

    public class ListaEvento
    {
        public string Evento
        {
            get; set;
        }
        public Guid EventoId
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaEvento()
        {
        }

        public ListaEvento(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        public IEnumerable<ListaEvento> EventosList()
        {
        try
        {
        var eventos = _unitOfWork.Evento.GetAllReduced(
            orderBy: n => n.OrderBy(n => n.Nome) ,
            selector: n => new ListaEvento
            {
                Evento = n.Nome ,
                EventoId = n.EventoId ,
                Status = n.Status ,
            }
        );

        return eventos.Where(e => e.Status == "1");
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"Erro em EventosList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return Enumerable.Empty<ListaEvento>();
        }
        }
    }

    #endregion

    #region Lista de Setores (TreeView)
    public class ListaSetores
    {
        public string SetorSolicitanteId
        {
            get; set;
        }
        public string SetorPaiId
        {
            get; set;
        }
        public bool HasChild
        {
            get; set;
        }
        public string Sigla
        {
            get; set;
        }
        public bool Expanded
        {
            get; set;
        }
        public bool IsSelected
        {
            get; set;
        }
        public string Nome
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaSetores()
        {
        }

        public ListaSetores(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        public List<ListaSetores> SetoresList()
        {
        try
        {
        var objSetores = _unitOfWork.ViewSetores.GetAll();

        if (objSetores == null || !objSetores.Any())
        {
        System.Diagnostics.Debug.WriteLine("⚠️ ATENÇÃO: Nenhum setor encontrado no banco de dados.");
        return new List<ListaSetores>();
        }

        List<ListaSetores> treeDataSource = new List<ListaSetores>();

        foreach (var setor in objSetores)
        {
        bool temFilho = _unitOfWork.ViewSetores.GetFirstOrDefault(u =>
            u.SetorPaiId == setor.SetorSolicitanteId
        ) != null;

        treeDataSource.Add(new ListaSetores
        {
            SetorSolicitanteId = setor.SetorSolicitanteId.ToString() ,
            SetorPaiId = setor.SetorPaiId == null || setor.SetorPaiId == Guid.Empty
                ? null  // NULL para itens raiz
                : setor.SetorPaiId.ToString() ,
            Nome = setor.Nome ,
            HasChild = temFilho
        });
        }

        System.Diagnostics.Debug.WriteLine($"✅ SetoresList carregou {treeDataSource.Count} setores");
        return treeDataSource;
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"❌ ERRO CRÍTICO em SetoresList: {ex.Message}");
        System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
        throw; // LANÇAR A EXCEÇÃO PARA IDENTIFICAR O PROBLEMA
        }
        }
    }
    #endregion

    #region Lista de Setores para Evento (Lista Plana)

    public class ListaSetoresEvento
    {
        public string SetorSolicitanteId
        {
            get; set;
        }
        public string Nome
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaSetoresEvento()
        {
        }

        public ListaSetoresEvento(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        public List<ListaSetoresEvento> SetoresEventoList()
        {
        try
        {
        var objSetores = _unitOfWork.SetorSolicitante.GetAll();

        if (objSetores == null || !objSetores.Any())
        {
        System.Diagnostics.Debug.WriteLine("Nenhum setor encontrado para eventos.");
        return new List<ListaSetoresEvento>();
        }

        List<ListaSetoresEvento> treeDataSource = new List<ListaSetoresEvento>();

        foreach (var setor in objSetores)
        {
        treeDataSource.Add(new ListaSetoresEvento
        {
            SetorSolicitanteId = setor.SetorSolicitanteId.ToString() ,
            Nome = $"{setor.Nome} ({setor.Sigla})" ,
        });
        }

        return treeDataSource.OrderBy(s => s.Nome).ToList();
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"Erro em SetoresEventoList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return new List<ListaSetoresEvento>();
        }
        }
    }

    #endregion

    #region Lista de Setores Flat (para DropDownList com Indentação)

    public class ListaSetoresFlat
    {
        public string SetorSolicitanteId
        {
            get; set;
        }
        public string Nome
        {
            get; set;
        }
        public int Nivel
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaSetoresFlat()
        {
        }

        public ListaSetoresFlat(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        // Classe auxiliar interna
        private class SetorHierarquia
        {
            public Guid SetorSolicitanteId
            {
                get; set;
            }
            public Guid SetorPaiId
            {
                get; set;
            }
            public string Nome
            {
                get; set;
            }
        }

        public List<ListaSetoresFlat> SetoresListFlat()
        {
        try
        {
        // Converte para a classe auxiliar
        var objSetores = _unitOfWork.ViewSetores.GetAllReduced(
            selector: x => new SetorHierarquia
            {
                SetorSolicitanteId = x.SetorSolicitanteId ,
                SetorPaiId = x.SetorPaiId ?? Guid.Empty ,
                Nome = x.Nome ,
            }
        ).ToList();

        if (objSetores == null || !objSetores.Any())
        {
        System.Diagnostics.Debug.WriteLine("Nenhum setor encontrado para lista flat.");
        return new List<ListaSetoresFlat>();
        }

        var resultado = objSetores.Select(setor =>
        {
        int nivel = CalcularNivel(setor.SetorSolicitanteId , setor.SetorPaiId , objSetores);
        string indentacao = new string('—' , nivel);

        return new ListaSetoresFlat
        {
            SetorSolicitanteId = setor.SetorSolicitanteId.ToString() ,
            Nome = $"{indentacao} {setor.Nome}" ,
            Nivel = nivel
        };
        })
        .OrderBy(s => s.Nome)
        .ToList();

        return resultado;
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine(
            $"Erro em SetoresListFlat - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}"
        );
        return new List<ListaSetoresFlat>();
        }
        }

        // Método agora aceita a classe auxiliar
        private int CalcularNivel(Guid setorId , Guid setorPaiId , List<SetorHierarquia> objSetores)
        {
        int nivel = 0;
        Guid paiAtual = setorPaiId;
        HashSet<Guid> visitados = new HashSet<Guid>(); // Proteção contra loops
        int maxNivel = 50; // Proteção adicional

        while (paiAtual != Guid.Empty && nivel < maxNivel)
        {
        // Verifica se já visitamos este setor (loop circular)
        if (visitados.Contains(paiAtual))
        {
        System.Diagnostics.Debug.WriteLine($"⚠️ Loop circular detectado no setor {paiAtual}");
        break;
        }

        visitados.Add(paiAtual);
        nivel++;

        var pai = objSetores.FirstOrDefault(s => s.SetorSolicitanteId == paiAtual);

        if (pai == null)
        {
        System.Diagnostics.Debug.WriteLine($"⚠️ Setor pai {paiAtual} não encontrado");
        break;
        }

        paiAtual = pai.SetorPaiId;
        }

        if (nivel >= maxNivel)
        {
        System.Diagnostics.Debug.WriteLine($"⚠️ Nível máximo atingido para setor {setorId}");
        }

        return nivel;
        }
    }

    #endregion

    #region Lista de Dias da Semana

    public class ListaDias
    {
        public string DiaId
        {
            get; set;
        }
        public string Dia
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaDias()
        {
        }

        public ListaDias(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        public List<ListaDias> DiasList()
        {
        try
        {
        return new List<ListaDias>
                {
                    new ListaDias { DiaId = "Monday", Dia = "Segunda" },
                    new ListaDias { DiaId = "Tuesday", Dia = "Terça" },
                    new ListaDias { DiaId = "Wednesday", Dia = "Quarta" },
                    new ListaDias { DiaId = "Thursday", Dia = "Quinta" },
                    new ListaDias { DiaId = "Friday", Dia = "Sexta" },
                    new ListaDias { DiaId = "Saturday", Dia = "Sábado" },
                    new ListaDias { DiaId = "Sunday", Dia = "Domingo" }
                };
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"Erro em DiasList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return new List<ListaDias>();
        }
        }
    }

    #endregion

    #region Lista de Períodos

    public class ListaPeriodos
    {
        public string PeriodoId
        {
            get; set;
        }
        public string Periodo
        {
            get; set;
        }

        public ListaPeriodos()
        {
        }

        public List<ListaPeriodos> PeriodosList()
        {
        try
        {
        return new List<ListaPeriodos>
                {
                    new ListaPeriodos { PeriodoId = "D", Periodo = "Diário" },
                    new ListaPeriodos { PeriodoId = "S", Periodo = "Semanal" },
                    new ListaPeriodos { PeriodoId = "Q", Periodo = "Quinzenal" },
                    new ListaPeriodos { PeriodoId = "M", Periodo = "Mensal" }
                };
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"Erro em PeriodosList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return new List<ListaPeriodos>();
        }
        }
    }

    #endregion

    #region Lista de Recorrente

    public class ListaRecorrente
    {
        public string RecorrenteId
        {
            get; set;
        }
        public string Descricao  // ✅ CORRIGIDO: Mudado de "Recorrente" para "Descricao"
        {
            get; set;
        }

        public ListaRecorrente()
        {
        }

        public List<ListaRecorrente> RecorrenteList()
        {
        try
        {
        return new List<ListaRecorrente>
            {
                new ListaRecorrente { RecorrenteId = "N", Descricao = "Não" },
                new ListaRecorrente { RecorrenteId = "S", Descricao = "Sim" }
            };
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"Erro em RecorrenteList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return new List<ListaRecorrente>();
        }
        }
    }

    #endregion

    #region Lista de Status

    public class ListaStatus
    {
        public string Status
        {
            get; set;
        }
        public string StatusId
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaStatus()
        {
        }

        public ListaStatus(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        public List<ListaStatus> StatusList()
        {
        try
        {
        return new List<ListaStatus>
                {
                    new ListaStatus { Status = "Todas", StatusId = "Todas" },
                    new ListaStatus { Status = "Abertas", StatusId = "Aberta" },
                    new ListaStatus { Status = "Realizadas", StatusId = "Realizada" },
                    new ListaStatus { Status = "Canceladas", StatusId = "Cancelada" }
                };
        }
        catch (Exception ex)
        {
        System.Diagnostics.Debug.WriteLine($"Erro em StatusList - Linha: {new System.Diagnostics.StackTrace(ex , true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
        return new List<ListaStatus>();
        }
        }
    }

    #endregion
}
