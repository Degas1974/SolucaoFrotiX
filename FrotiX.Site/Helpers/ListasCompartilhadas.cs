/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ListasCompartilhadas.cs                                                               ║
   ║ 📂 CAMINHO: Helpers/                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Prover listas para dropdowns e componentes EJ2 (DropDownList, TreeView) e                       ║
   ║    comparadores de ordenação pt-BR/natural.                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • PtBrComparer.Compare(string x, string y)                                                     ║
   ║    • NaturalStringComparer.Compare(string x, string y)                                            ║
   ║    • ListaFinalidade.FinalidadesList()                                                            ║
   ║    • ListaNivelCombustivel.NivelCombustivelList()                                                 ║
   ║    • ListaVeiculos.VeiculosList()                                                                 ║
   ║    • ListaMotorista.MotoristaList()                                                               ║
   ║    • ListaRequisitante.RequisitantesList()                                                        ║
   ║    • ListaEvento.EventosList()                                                                    ║
   ║    • ListaSetores.SetoresList()                                                                   ║
   ║    • ListaSetoresEvento.SetoresEventoList()                                                       ║
   ║    • ListaSetoresFlat.SetoresListFlat()                                                           ║
   ║    • ListaDias.DiasList()                                                                         ║
   ║    • ListaPeriodos.PeriodosList()                                                                 ║
   ║    • ListaRecorrente.RecorrenteList()                                                             ║
   ║    • ListaStatus.StatusList()                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: IUnitOfWork (Repository Pattern), System.Globalization                            ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: PtBrComparer                                                                      │
    // │ 📦 TIPO: Interna                                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Comparar strings em pt-BR ignorando maiúsculas/minúsculas e acentuação.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Ordenações internas de listas
    // ➡️ CHAMA       : CompareInfo.Compare()
    
    
    internal sealed class PtBrComparer :IComparer<string>
    {
        private static readonly CompareInfo Cmp = new CultureInfo("pt-BR").CompareInfo;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Compare                                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : OrderBy/Sort                                                        │
        // │    ➡️ CHAMA       : CompareInfo.Compare()                                               │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Comparar duas strings usando cultura pt-BR, ignorando case e acentos.
        
        
        
        // 📥 PARÂMETROS:
        // x - Primeiro texto
        // y - Segundo texto
        
        
        
        // 📤 RETORNO:
        // int - Resultado da comparação (menor, igual ou maior).
        
        
        // Param x: Primeiro texto.
        // Param y: Segundo texto.
        // Returns: Resultado da comparação.
        public int Compare(string x , string y)
        {
        return Cmp.Compare(
            x ?? string.Empty ,
            y ?? string.Empty ,
            CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
        );
        }
    }

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: NaturalStringComparer                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Implementar ordenação natural de strings, tratando sequências numéricas corretamente.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Ordenações de listas no backend
    // ➡️ CHAMA       : string.Compare(), int.Parse()
    
    
    public class NaturalStringComparer : IComparer<string>
    {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Compare                                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : OrderBy/Sort                                                        │
        // │    ➡️ CHAMA       : string.Compare(), int.Parse()                                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Comparar duas strings com ordenação natural (números antes de letras).
        
        
        
        // 📥 PARÂMETROS:
        // x - Primeiro texto
        // y - Segundo texto
        
        
        
        // 📤 RETORNO:
        // int - Resultado da comparação (menor, igual ou maior).
        
        
        // Param x: Primeiro texto.
        // Param y: Segundo texto.
        // Returns: Resultado da comparação.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaFinalidade                                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Disponibilizar lista de finalidades de viagem para dropdowns.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views, Controllers e Services
    // ➡️ CHAMA       : PtBrComparer (ordenação)
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaFinalidade (ctor)                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências (lista estática).
        
        
        public ListaFinalidade()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaFinalidade (ctor)                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância com acesso ao UnitOfWork quando necessário.
        
        
        
        // 📥 PARÂMETROS:
        // unitOfWork - Unidade de trabalho para acesso a repositórios.
        
        
        // Param unitOfWork: Unidade de trabalho para acesso a repositórios.
        public ListaFinalidade(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: FinalidadesList                                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, Controllers                                               │
        // │    ➡️ CHAMA       : PtBrComparer, OrderBy()                                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar lista de finalidades ordenadas em pt-BR.
        
        
        
        // 📤 RETORNO:
        // List&lt;ListaFinalidade&gt; - Lista ordenada de finalidades.
        
        
        // Returns: Lista ordenada de finalidades.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaNivelCombustivel                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista de níveis de combustível para seleção visual.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views e componentes de formulário
    // ➡️ CHAMA       : (lista estática)
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaNivelCombustivel (ctor)                                              │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências (lista estática).
        
        
        public ListaNivelCombustivel()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaNivelCombustivel (ctor)                                              │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância com acesso ao UnitOfWork quando necessário.
        
        
        
        // 📥 PARÂMETROS:
        // unitOfWork - Unidade de trabalho para acesso a repositórios.
        
        
        // Param unitOfWork: Unidade de trabalho para acesso a repositórios.
        public ListaNivelCombustivel(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: NivelCombustivelList                                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, Controllers                                               │
        // │    ➡️ CHAMA       : (lista estática)                                                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar lista fixa de níveis de combustível.
        
        
        
        // 📤 RETORNO:
        // List&lt;ListaNivelCombustivel&gt; - Itens com nível, descrição e imagem.
        
        
        // Returns: Lista fixa de níveis de combustível.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaVeiculos                                                                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista de veículos ativos para seleção em dropdowns.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views, Controllers e Services
    // ➡️ CHAMA       : IUnitOfWork.Veiculo.GetAllReduced()
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaVeiculos (ctor)                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências.
        
        
        public ListaVeiculos()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaVeiculos (ctor)                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância com UnitOfWork para acesso ao repositório de veículos.
        
        
        
        // 📥 PARÂMETROS:
        // unitOfWork - Unidade de trabalho para acesso aos dados.
        
        
        // Param unitOfWork: Unidade de trabalho para acesso aos dados.
        public ListaVeiculos(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: VeiculosList                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, Controllers                                               │
        // │    ➡️ CHAMA       : IUnitOfWork.Veiculo.GetAllReduced(), OrderBy()                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar veículos ativos ordenados por descrição (placa/modelo).
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;ListaVeiculos&gt; - Veículos ativos.
        
        
        // Returns: Veículos ativos ordenados por descrição.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaMotorista                                                                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista de motoristas ativos com foto em base64.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views, Controllers e Services
    // ➡️ CHAMA       : IUnitOfWork.ViewMotoristas.GetAllReduced()
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaMotorista (ctor)                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências.
        
        
        public ListaMotorista()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaMotorista (ctor)                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância com UnitOfWork para acesso à view de motoristas.
        
        
        
        // 📥 PARÂMETROS:
        // unitOfWork - Unidade de trabalho para acesso aos dados.
        
        
        // Param unitOfWork: Unidade de trabalho para acesso aos dados.
        public ListaMotorista(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MotoristaList                                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, Controllers                                               │
        // │    ➡️ CHAMA       : IUnitOfWork.ViewMotoristas.GetAllReduced()                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar motoristas ativos, com foto em base64 quando disponível.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;ListaMotorista&gt; - Motoristas ativos.
        
        
        // Returns: Motoristas ativos ordenados por nome.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaRequisitante                                                                 │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista de requisitantes com ordenação natural.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views, Controllers e Services
    // ➡️ CHAMA       : IUnitOfWork.ViewRequisitantes.GetAllReduced(), NaturalStringComparer
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaRequisitante (ctor)                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências.
        
        
        public ListaRequisitante()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaRequisitante (ctor)                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância com UnitOfWork para acesso a requisitantes.
        
        
        
        // 📥 PARÂMETROS:
        // unitOfWork - Unidade de trabalho para acesso aos dados.
        
        
        // Param unitOfWork: Unidade de trabalho para acesso aos dados.
        public ListaRequisitante(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RequisitantesList                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, Controllers                                               │
        // │    ➡️ CHAMA       : IUnitOfWork.ViewRequisitantes.GetAllReduced(), NaturalStringComparer │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar requisitantes ordenados naturalmente (números antes de letras).
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;ListaRequisitante&gt; - Lista ordenada de requisitantes.
        
        
        // Returns: Lista ordenada de requisitantes.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaEvento                                                                       │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista de eventos ativos para seleção.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views, Controllers e Services
    // ➡️ CHAMA       : IUnitOfWork.Evento.GetAllReduced()
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaEvento (ctor)                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências.
        
        
        public ListaEvento()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaEvento (ctor)                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância com UnitOfWork para acesso a eventos.
        
        
        
        // 📥 PARÂMETROS:
        // unitOfWork - Unidade de trabalho para acesso aos dados.
        
        
        // Param unitOfWork: Unidade de trabalho para acesso aos dados.
        public ListaEvento(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: EventosList                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, Controllers                                               │
        // │    ➡️ CHAMA       : IUnitOfWork.Evento.GetAllReduced(), OrderBy()                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar eventos ativos ordenados por nome.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;ListaEvento&gt; - Eventos ativos.
        
        
        // Returns: Eventos ativos ordenados por nome.
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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaSetores                                                                      │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista hierárquica de setores para TreeView.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views e componentes TreeView
    // ➡️ CHAMA       : IUnitOfWork.ViewSetores.GetAll(), IUnitOfWork.ViewSetores.GetFirstOrDefault()
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaSetores (ctor)                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências.
        
        
        public ListaSetores()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaSetores (ctor)                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância com UnitOfWork para acesso aos setores.
        
        
        
        // 📥 PARÂMETROS:
        // unitOfWork - Unidade de trabalho para acesso aos dados.
        
        
        // Param unitOfWork: Unidade de trabalho para acesso aos dados.
        public ListaSetores(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: SetoresList                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, TreeView                                                   │
        // │    ➡️ CHAMA       : IUnitOfWork.ViewSetores.GetAll(), GetFirstOrDefault()                │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Montar lista hierárquica de setores com indicação de filhos.
        
        
        
        // 📤 RETORNO:
        // List&lt;ListaSetores&gt; - Lista hierárquica para TreeView.
        
        
        // Returns: Lista hierárquica para TreeView.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaSetoresEvento                                                                │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista plana de setores para seleção em eventos.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views e formulários de eventos
    // ➡️ CHAMA       : IUnitOfWork.SetorSolicitante.GetAll()
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaSetoresEvento (ctor)                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências.
        
        
        public ListaSetoresEvento()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaSetoresEvento (ctor)                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância com UnitOfWork para acesso aos setores.
        
        
        
        // 📥 PARÂMETROS:
        // unitOfWork - Unidade de trabalho para acesso aos dados.
        
        
        // Param unitOfWork: Unidade de trabalho para acesso aos dados.
        public ListaSetoresEvento(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: SetoresEventoList                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, formulários de eventos                                   │
        // │    ➡️ CHAMA       : IUnitOfWork.SetorSolicitante.GetAll(), OrderBy()                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar setores em lista plana ordenada por nome.
        
        
        
        // 📤 RETORNO:
        // List&lt;ListaSetoresEvento&gt; - Lista ordenada de setores.
        
        
        // Returns: Lista ordenada de setores.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaSetoresFlat                                                                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista plana de setores com indentação para dropdowns.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views e formulários com DropDownList
    // ➡️ CHAMA       : IUnitOfWork.ViewSetores.GetAllReduced(), CalcularNivel()
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaSetoresFlat (ctor)                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências.
        
        
        public ListaSetoresFlat()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaSetoresFlat (ctor)                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância com UnitOfWork para acesso aos setores.
        
        
        
        // 📥 PARÂMETROS:
        // unitOfWork - Unidade de trabalho para acesso aos dados.
        
        
        // Param unitOfWork: Unidade de trabalho para acesso aos dados.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: SetoresListFlat                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, DropDownList                                              │
        // │    ➡️ CHAMA       : IUnitOfWork.ViewSetores.GetAllReduced(), CalcularNivel()            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Montar lista plana de setores com indentação hierárquica.
        
        
        
        // 📤 RETORNO:
        // List&lt;ListaSetoresFlat&gt; - Lista plana com nível calculado.
        
        
        // Returns: Lista plana com nível calculado.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaDias                                                                        │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista de dias da semana em pt-BR.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views e formulários de agenda
    // ➡️ CHAMA       : (lista estática)
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaDias (ctor)                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências.
        
        
        public ListaDias()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaDias (ctor)                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (injeção de dependências)                                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância com UnitOfWork (quando necessário).
        
        
        
        // 📥 PARÂMETROS:
        // unitOfWork - Unidade de trabalho para acesso aos dados.
        
        
        // Param unitOfWork: Unidade de trabalho para acesso aos dados.
        public ListaDias(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: DiasList                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, formulários de agenda                                   │
        // │    ➡️ CHAMA       : (lista estática)                                                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar dias da semana em pt-BR.
        
        
        
        // 📤 RETORNO:
        // List&lt;ListaDias&gt; - Lista dos dias da semana.
        
        
        // Returns: Lista dos dias da semana.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaPeriodos                                                                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista de períodos (D/S/Q/M) para seleção.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views e formulários de recorrência
    // ➡️ CHAMA       : (lista estática)
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaPeriodos (ctor)                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências.
        
        
        public ListaPeriodos()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: PeriodosList                                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, formulários de recorrência                               │
        // │    ➡️ CHAMA       : (lista estática)                                                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar lista de períodos de recorrência.
        
        
        
        // 📤 RETORNO:
        // List&lt;ListaPeriodos&gt; - Lista de períodos.
        
        
        // Returns: Lista de períodos.
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

    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ListaRecorrente                                                                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Fornecer lista Sim/Não para recorrência.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : UI/Views e formulários de recorrência
    // ➡️ CHAMA       : (lista estática)
    
    
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ListaRecorrente (ctor)                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : DI / Instanciação manual                                            │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Criar instância sem dependências.
        
        
        public ListaRecorrente()
        {
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RecorrenteList                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views, formulários de recorrência                               │
        // │    ➡️ CHAMA       : (lista estática)                                                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar lista de opções de recorrência (Sim/Não).
        
        
        
        // 📤 RETORNO:
        // List&lt;ListaRecorrente&gt; - Lista de opções de recorrência.
        
        
        // Returns: Lista de opções de recorrência.
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
