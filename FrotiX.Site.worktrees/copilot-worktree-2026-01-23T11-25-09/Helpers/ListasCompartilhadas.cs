using FrotiX.Models;
using FrotiX.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace FrotiX.Helpers
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║                                                                              ║
    /// ║  📋 ARQUIVO: ListasCompartilhadas.cs (Helper Collection Classes)            ║
    /// ║                                                                              ║
    /// ║  DESCRIÇÃO:                                                                  ║
    /// ║  Coleção de helpers para listas compartilhadas em dropdowns/selects.        ║
    /// ║  Centraliza todas as listas utilizadas em formulários do sistema.           ║
    /// ║                                                                              ║
    /// ║  ESTRUTURA:                                                                  ║
    /// ║  1. Comparadores (PtBrComparer, NaturalStringComparer).                     ║
    /// ║  2. Listas de Domínio (Finalidade, NivelCombustivel, etc.).                 ║
    /// ║  3. Listas de Entidades (Veículos, Motoristas, Setores, etc.).              ║
    /// ║                                                                              ║
    /// ║  CLASSES PRINCIPAIS:                                                         ║
    /// ║  - ListaFinalidade: Finalidades de viagens (hardcoded).                     ║
    /// ║  - ListaNivelCombustivel: Níveis de combustível com imagens.                ║
    /// ║  - ListaVeiculos: Veículos ativos para dropdowns.                           ║
    /// ║  - ListaMotorista: Motoristas com fotos para seleção.                       ║
    /// ║  - ListaSetores: Setores/Departamentos do sistema.                          ║
    /// ║  - ListaDependencias: Dependências/Locais de destino.                       ║
    /// ║                                                                              ║
    /// ║  PADRÃO DE ORDENAÇÃO:                                                        ║
    /// ║  - pt-BR (PtBrComparer): Ignora acentos e case.                             ║
    /// ║  - Natural (NaturalStringComparer): Ordena números corretamente.            ║
    /// ║                                                                              ║
    /// ║  ÚLTIMA ATUALIZAÇÃO: 19/01/2026                                              ║
    /// ║                                                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    /// </summary>

    #region Comparadores

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: PtBrComparer (Comparador pt-BR)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Comparador personalizado para ordenação em português brasileiro.
    /// │    Ignora case e acentuação (CompareOptions.IgnoreCase | IgnoreNonSpace).
    /// │
    /// │ COMPORTAMENTO:
    /// │    - "Ação" == "acao" == "ACAO"
    /// │    - "Água" == "agua" == "AGUA"
    /// │    - Ordenação alfabética correta em pt-BR.
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    Usado em OrderBy() de listas para garantir ordenação pt-BR consistente.
    /// │    Ex: finalidades.OrderBy(f => f.Descricao, new PtBrComparer())
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
    internal sealed class PtBrComparer : IComparer<string>
    {
        private static readonly CompareInfo Cmp = new CultureInfo("pt-BR").CompareInfo;

        public int Compare(string x, string y)
        {
            return Cmp.Compare(
                x ?? string.Empty,
                y ?? string.Empty,
                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
            );
        }
    }

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: NaturalStringComparer (Ordenação Natural/Alfanumérica)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Comparador que ordena strings alfanuméricas naturalmente.
    /// │    Trata números como inteiros, não caracteres.
    /// │
    /// │ COMPORTAMENTO:
    /// │    - Ordenação natural: "1 Nome" < "2 Nome" < "10 Nome" < "Aaaa" < "Bbbb"
    /// │    - Números VÊM ANTES de letras.
    /// │    - Compara números numericamente (não lexicograficamente).
    /// │
    /// │ EXEMPLO PRÁTICO:
    /// │    Ordenação lexicográfica: "1", "10", "2", "20"  (ERRADO)
    /// │    Ordenação natural:       "1", "2", "10", "20"  (CORRETO)
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    Usado para ordenar listas com identificadores alfanuméricos:
    /// │    - Placas de veículos: "AAA-1B20" < "AAA-2A10" < "AAA-10C50"
    /// │    - Códigos de setores: "Setor 1" < "Setor 2" < "Setor 10"
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
    public class NaturalStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            // [ETAPA 1] - Validação de nulls
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int ix = 0, iy = 0;

            // [ETAPA 2] - Percorre ambas strings caractere a caractere
            while (ix < x.Length && iy < y.Length)
            {
                // [ETAPA 2.1] - Se ambos começam com dígito, compara numericamente
                if (char.IsDigit(x[ix]) && char.IsDigit(y[iy]))
                {
                    // Extrai sequência numérica completa de x
                    string numX = "";
                    while (ix < x.Length && char.IsDigit(x[ix]))
                    {
                        numX += x[ix];
                        ix++;
                    }

                    // Extrai sequência numérica completa de y
                    string numY = "";
                    while (iy < y.Length && char.IsDigit(y[iy]))
                    {
                        numY += y[iy];
                        iy++;
                    }

                    // Compara numericamente (2 < 10, não "2" > "10")
                    int xNum = int.Parse(numX);
                    int yNum = int.Parse(numY);

                    if (xNum != yNum)
                        return xNum.CompareTo(yNum);
                }
                // [ETAPA 2.2] - Compara caracteres alfabeticamente (pt-BR, case-insensitive)
                else
                {
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

            // [ETAPA 3] - Se strings iguais até aqui, compara tamanho (mais curta vem primeiro)
            return x.Length.CompareTo(y.Length);
        }
    }

    #endregion

    #region Lista de Finalidades

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaFinalidade (Lista de Finalidades de Viagens)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista hardcoded de finalidades de viagens (não vem do banco de dados).
    /// │    Usada em dropdowns de seleção de finalidade em formulários de viagem.
    /// │
    /// │ PROPRIEDADES:
    /// │    - FinalidadeId: Identificador único (string) da finalidade.
    /// │    - Descricao: Descrição exibida no dropdown.
    /// │
    /// │ FINALIDADES DISPONÍVEIS (23 itens):
    /// │    - Transporte de Funcionários, Convidados, Materiais
    /// │    - Economildo (Norte/Sul/Rodoviária)
    /// │    - Mesa (carros pretos), TV/Rádio Câmara
    /// │    - Aeroporto, Manutenção, Abastecimento
    /// │    - Locadora (Recebimento/Devolução)
    /// │    - Saída Programada, Evento, Ambulância
    /// │    - Depol, Demanda Política, Passaporte, Aviso, Cursos
    /// │
    /// │ ORDENAÇÃO:
    /// │    Alfabética em pt-BR (PtBrComparer) - Ignora acentos e case.
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    Usada em formulários de criação/edição de viagens.
    /// │    Ex: @Html.DropDownListFor(m => m.Finalidade, new SelectList(...))
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Lista hardcoded de 23 finalidades
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

                // [ETAPA 2] - Ordenar alfabeticamente em pt-BR (ignora acentos e maiúsculas/minúsculas)
                return finalidades.OrderBy(f => f.Descricao, new PtBrComparer()).ToList();
            }
            catch (Exception ex)
            {
                // Log do erro (ajuste conforme seu sistema de log)
                System.Diagnostics.Debug.WriteLine($"Erro em FinalidadesList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return new List<ListaFinalidade>();
            }
        }
    }

    #endregion

    #region Lista de Nível de Combustível

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaNivelCombustivel (Níveis de Combustível com Ícones)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista hardcoded de níveis de combustível com ícones visuais.
    /// │    Usada em formulários de abastecimento/viagens para indicar nível do tanque.
    /// │
    /// │ PROPRIEDADES:
    /// │    - Nivel: ID do nível (usado como value no dropdown).
    /// │    - Descricao: Texto exibido ao usuário (Vazio, 1/4, 1/2, 3/4, Cheio).
    /// │    - Imagem: Path para ícone visual do tanque (PNG em wwwroot/images/).
    /// │
    /// │ NÍVEIS DISPONÍVEIS (5 itens):
    /// │    - tanquevazio      → "Vazio"  → tanquevazio.png
    /// │    - tanqueumquarto   → "1/4"    → tanqueumquarto.png
    /// │    - tanquemeiotanque → "1/2"    → tanquemeiotanque.png
    /// │    - tanquetresquartos→ "3/4"    → tanquetresquartos.png
    /// │    - tanquecheio      → "Cheio"  → tanquecheio.png
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulário de Abastecimento: Registro do nível antes/depois do abastecimento.
    /// │    - Formulário de Viagem: Nível ao iniciar/finalizar a viagem.
    /// │    - Dashboards: Indicador visual do nível de combustível dos veículos.
    /// │
    /// │ ÍCONES:
    /// │    Os ícones PNG mostram representação visual do tanque preenchido.
    /// │    Facilita identificação rápida pelo usuário (UX).
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Lista hardcoded de 5 níveis com ícones PNG
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
                System.Diagnostics.Debug.WriteLine($"Erro em NivelCombustivelList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return new List<ListaNivelCombustivel>();
            }
        }
    }

    #endregion

    #region Lista de Veículos

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaVeiculos (Veículos para Dropdowns - Ativos e Inativos)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista de TODOS os veículos (ativos e inativos) buscados do banco de dados.
    /// │    Formato: "PLACA - MARCA/MODELO" (ex: "ABC-1234 - VW/Gol").
    /// │
    /// │ PROPRIEDADES:
    /// │    - VeiculoId: Guid do veículo (usado como value no dropdown).
    /// │    - Descricao: Texto formatado "Placa - Marca/Modelo".
    /// │
    /// │ QUERY DATABASE:
    /// │    - Tabela: Veiculo (com JOIN em MarcaVeiculo e ModeloVeiculo).
    /// │    - Filtro: NENHUM (retorna todos os veículos independente do Status).
    /// │    - Include: ModeloVeiculo, MarcaVeiculo (navegação EF Core).
    /// │    - Ordenação: Alfabética por Descricao (Placa - Marca/Modelo).
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulário de Viagem: Seleção do veículo utilizado.
    /// │    - Formulário de Manutenção: Veículo em manutenção.
    /// │    - Formulário de Abastecimento: Veículo abastecido.
    /// │    - Escalas: Vinculação de veículo a motorista.
    /// │
    /// │ EXEMPLO DE SAÍDA:
    /// │    - "ABC-1234 - Volkswagen/Gol"
    /// │    - "XYZ-9876 - Fiat/Uno"
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Query EF Core com Include de Marca e Modelo
                var veiculos = (
                    from v in _unitOfWork.Veiculo.GetAllReduced(
                        includeProperties: nameof(ModeloVeiculo) + "," + nameof(MarcaVeiculo),
                        selector: v => new
                        {
                            v.VeiculoId,
                            v.Placa,
                            v.MarcaVeiculo.DescricaoMarca,
                            v.ModeloVeiculo.DescricaoModelo,
                            v.Status,
                        }
                    )
                    // [ETAPA 2] - Projeta para ListaVeiculos com formato "Placa - Marca/Modelo"
                    // NOTA: Removido filtro de Status para incluir veículos ativos E inativos
                    select new ListaVeiculos
                    {
                        VeiculoId = v?.VeiculoId ?? Guid.Empty,
                        Descricao = $"{v.Placa} - {v.DescricaoMarca}/{v.DescricaoModelo}",
                    }
                // [ETAPA 3] - Ordena alfabeticamente pela descrição formatada
                ).OrderBy(v => v.Descricao);

                return veiculos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em VeiculosList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return Enumerable.Empty<ListaVeiculos>();
            }
        }
    }

    #endregion

    #region Lista de Motoristas

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaMotorista (Motoristas com Fotos Base64 - Ativos e Inativos)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista de TODOS os motoristas (ativos e inativos) com fotos em Base64.
    /// │    Usada em dropdowns avançados com avatar do motorista.
    /// │
    /// │ PROPRIEDADES:
    /// │    - MotoristaId: Guid do motorista (value no dropdown).
    /// │    - Nome: Nome completo do motorista/condutor.
    /// │    - FotoBase64: Data URI da foto em Base64 (data:image/jpeg;base64,...).
    /// │    - Status: Indica se motorista está ativo (true) ou inativo (false).
    /// │
    /// │ QUERY DATABASE:
    /// │    - Tabela: ViewMotoristas (View materializada com dados consolidados).
    /// │    - Filtro: NENHUM (retorna todos os motoristas independente do Status).
    /// │    - Ordenação: Alfabética por Nome.
    /// │    - Foto: Convertida de byte[] para Base64 Data URI (JPEG).
    /// │
    /// │ CONVERSÃO BASE64:
    /// │    - Se motorista tem foto (byte[] não-null):
    /// │      → Convert.ToBase64String(motorista.Foto)
    /// │      → Prefixo: "data:image/jpeg;base64,"
    /// │    - Se não tem foto:
    /// │      → FotoBase64 = null (UI deve exibir avatar padrão).
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulário de Viagem: Seleção do motorista com preview da foto.
    /// │    - Formulário de Escala: Vinculação de motorista a veículo.
    /// │    - Dashboards: Exibição de escalas com avatares dos motoristas.
    /// │
    /// │ UX ENHANCEMENT:
    /// │    FotoBase64 permite dropdown com imagens (Syncfusion ComboBox suporta templates).
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Query ViewMotoristas com ordenação alfabética por Nome
                var motoristas = _unitOfWork.ViewMotoristas.GetAllReduced(
                    orderBy: m => m.OrderBy(m => m.Nome),
                    selector: motorista => new ListaMotorista
                    {
                        MotoristaId = motorista.MotoristaId,
                        Nome = motorista.MotoristaCondutor,
                        // [ETAPA 2] - Converte foto de byte[] para Base64 Data URI (se existir)
                        FotoBase64 = motorista.Foto != null
                            ? $"data:image/jpeg;base64,{Convert.ToBase64String(motorista.Foto)}"
                            : null,
                        Status = motorista.Status,
                    }
                );

                // [ETAPA 3] - Retorna TODOS os motoristas (ativos e inativos)
                // NOTA: Removido filtro de Status para incluir motoristas ativos E inativos
                return motoristas;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em MotoristaList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return Enumerable.Empty<ListaMotorista>();
            }
        }
    }

    #endregion

    #region Lista de Requisitantes

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaRequisitante (Requisitantes de Viagens)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista de requisitantes (pessoas que solicitam viagens) da ViewRequisitantes.
    /// │    Ordenação natural (números antes de letras, alfanumérica inteligente).
    /// │
    /// │ PROPRIEDADES:
    /// │    - RequisitanteId: Guid do requisitante (value no dropdown).
    /// │    - Requisitante: Nome completo do requisitante.
    /// │
    /// │ QUERY DATABASE:
    /// │    - Tabela: ViewRequisitantes (View consolidada).
    /// │    - Sem filtro de Status (todos os requisitantes são retornados).
    /// │    - Ordenação: MEMÓRIA (NaturalStringComparer) - Melhor performance.
    /// │
    /// │ ESTRATÉGIA DE ORDENAÇÃO:
    /// │    1. Query DB SEM OrderBy (melhor performance no SQL Server).
    /// │    2. ToList() carrega dados em memória.
    /// │    3. Trim() remove espaços em branco (ex: "João  " → "João").
    /// │    4. OrderBy(NaturalStringComparer) ordena alfanumericamente.
    /// │
    /// │ ORDENAÇÃO NATURAL:
    /// │    - Números VÊM ANTES de letras.
    /// │    - Números ordenados numericamente: "1 João" < "2 Maria" < "10 Pedro".
    /// │    - Case-insensitive, pt-BR (ignora acentos).
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulário de Viagem: Seleção do requisitante da viagem.
    /// │    - Relatórios: Filtro por requisitante.
    /// │
    /// │ PERFORMANCE:
    /// │    Ordenar em memória (C#) é mais rápido que ORDER BY no SQL Server
    /// │    para comparadores customizados (NaturalStringComparer).
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Busca dados SEM ordenação no banco (melhor performance)
                var requisitantes = _unitOfWork.ViewRequisitantes.GetAllReduced(
                    selector: r => new ListaRequisitante
                    {
                        Requisitante = r.Requisitante,
                        RequisitanteId = (Guid)r.RequisitanteId,
                    }
                ).ToList();

                // [ETAPA 2] - Trim e ordena usando NaturalStringComparer em memória
                // (números antes de letras, case-insensitive, pt-BR)
                return requisitantes
                    .Select(r => new ListaRequisitante
                    {
                        // [ETAPA 2.1] - Remove espaços em branco de início/fim
                        Requisitante = (r.Requisitante ?? "").Trim(),
                        RequisitanteId = r.RequisitanteId
                    })
                    // [ETAPA 2.2] - Ordenação natural alfanumérica
                    .OrderBy(r => r.Requisitante ?? "", new NaturalStringComparer())
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em RequisitantesList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return Enumerable.Empty<ListaRequisitante>();
            }
        }
    }

    #endregion

    #region Lista de Eventos

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaEvento (Eventos do Sistema - Ativos e Inativos)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista de TODOS os eventos (ativos e inativos) buscados da tabela Evento.
    /// │    Usada em dropdowns de seleção de eventos (reuniões, conferências, etc.).
    /// │
    /// │ PROPRIEDADES:
    /// │    - EventoId: Guid do evento (value no dropdown).
    /// │    - Evento: Nome do evento (texto exibido).
    /// │    - Status: "1" = Ativo, "0" = Inativo (string no banco).
    /// │
    /// │ QUERY DATABASE:
    /// │    - Tabela: Evento.
    /// │    - Filtro: NENHUM (retorna todos os eventos independente do Status).
    /// │    - Ordenação: Alfabética por Nome (OrderBy no banco).
    /// │
    /// │ STATUS STRING:
    /// │    - Banco usa string: "1" (ativo), "0" (inativo).
    /// │    - NÃO é booleano (manter string na comparação).
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulário de Viagem: Vincular viagem a evento específico.
    /// │    - Formulário de Evento: Seleção de eventos para relatórios.
    /// │    - Dashboards: Filtro por eventos ativos.
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Query Evento com ordenação alfabética por Nome (no banco)
                var eventos = _unitOfWork.Evento.GetAllReduced(
                    orderBy: n => n.OrderBy(n => n.Nome),
                    selector: n => new ListaEvento
                    {
                        Evento = n.Nome,
                        EventoId = n.EventoId,
                        Status = n.Status,
                    }
                );

                // [ETAPA 2] - Retorna TODOS os eventos (ativos e inativos)
                // NOTA: Removido filtro de Status para incluir eventos ativos E inativos
                return eventos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em EventosList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return Enumerable.Empty<ListaEvento>();
            }
        }
    }

    #endregion

    #region Lista de Setores (TreeView)
    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaSetores (Setores Hierárquicos para TreeView)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista de setores em estrutura hierárquica para componente TreeView.
    /// │    Suporta múltiplos níveis (Pai → Filho → Neto → ...).
    /// │
    /// │ PROPRIEDADES:
    /// │    - SetorSolicitanteId: Guid do setor (string).
    /// │    - SetorPaiId: Guid do setor pai (string) ou NULL (raiz da árvore).
    /// │    - Nome: Nome do setor (exibido no TreeView).
    /// │    - HasChild: Indica se setor tem filhos (seta expansão no TreeView).
    /// │    - Sigla: Sigla do setor (opcional, não usada no TreeView).
    /// │    - Expanded: Define se nó inicia expandido (default: false).
    /// │    - IsSelected: Define se nó inicia selecionado (default: false).
    /// │
    /// │ QUERY DATABASE:
    /// │    - Tabela: ViewSetores (View com hierarquia de setores).
    /// │    - Hierarquia: SetorPaiId → SetorSolicitanteId (recursivo).
    /// │    - HasChild: Query adicional (GetFirstOrDefault) para verificar filhos.
    /// │
    /// │ ESTRUTURA HIERÁRQUICA:
    /// │    - Raiz: SetorPaiId == NULL ou Guid.Empty.
    /// │    - Filhos: SetorPaiId aponta para o SetorSolicitanteId do pai.
    /// │    - Exemplo:
    /// │      ├── Diretoria Geral (SetorPaiId = NULL)
    /// │      │   ├── TI (SetorPaiId = GUID_Diretoria)
    /// │      │   │   └── Infraestrutura (SetorPaiId = GUID_TI)
    /// │      │   └── RH (SetorPaiId = GUID_Diretoria)
    /// │
    /// │ DETECÇÃO DE FILHOS:
    /// │    Para cada setor, verifica se EXISTE outro setor com SetorPaiId == SetorSolicitanteId.
    /// │    Se existir, HasChild = true (TreeView exibe seta de expansão).
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulário de Requisitante: Seleção do setor solicitante em TreeView.
    /// │    - Relatórios: Filtro hierárquico por setor.
    /// │    - Dashboards: Visualização hierárquica de setores.
    /// │
    /// │ COMPONENTE UI:
    /// │    - Syncfusion TreeView (EJ2): Suporta hierarquia com SetorPaiId.
    /// │    - Configuração: DataSource → SetoresList(), ParentIdMapping → SetorPaiId.
    /// │
    /// │ VALIDAÇÃO:
    /// │    - Se ViewSetores retornar vazio, loga aviso e retorna lista vazia.
    /// │    - Em caso de erro, LANÇA EXCEÇÃO (throw) para debugging.
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Busca todos os setores da ViewSetores
                var objSetores = _unitOfWork.ViewSetores.GetAll();

                // [VALIDAÇÃO] - Se nenhum setor encontrado, retorna lista vazia com aviso
                if (objSetores == null || !objSetores.Any())
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ ATENÇÃO: Nenhum setor encontrado no banco de dados.");
                    return new List<ListaSetores>();
                }

                List<ListaSetores> treeDataSource = new List<ListaSetores>();

                // [ETAPA 2] - Itera sobre cada setor e detecta filhos
                foreach (var setor in objSetores)
                {
                    // [ETAPA 2.1] - Verifica se setor tem filhos (existe outro setor com SetorPaiId == SetorSolicitanteId)
                    bool temFilho = _unitOfWork.ViewSetores.GetFirstOrDefault(u =>
                        u.SetorPaiId == setor.SetorSolicitanteId
                    ) != null;

                    // [ETAPA 2.2] - Adiciona setor à lista hierárquica
                    treeDataSource.Add(new ListaSetores
                    {
                        SetorSolicitanteId = setor.SetorSolicitanteId.ToString(),
                        // [ETAPA 2.3] - SetorPaiId = NULL para raiz, ou Guid.ToString() para filhos
                        SetorPaiId = setor.SetorPaiId == null || setor.SetorPaiId == Guid.Empty
                            ? null  // NULL indica item raiz do TreeView
                            : setor.SetorPaiId.ToString(),
                        Nome = setor.Nome,
                        HasChild = temFilho // Controla exibição da seta de expansão
                    });
                }

                System.Diagnostics.Debug.WriteLine($"✅ SetoresList carregou {treeDataSource.Count} setores");
                return treeDataSource;
            }
            catch (Exception ex)
            {
                // [ERRO CRÍTICO] - Lança exceção para debugging (não silencia erro)
                System.Diagnostics.Debug.WriteLine($"❌ ERRO CRÍTICO em SetoresList: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw; // LANÇAR A EXCEÇÃO PARA IDENTIFICAR O PROBLEMA
            }
        }
    }
    #endregion

    #region Lista de Setores para Evento (Lista Plana)

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaSetoresEvento (Setores PLANOS para Dropdown Simples)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista PLANA de setores (sem hierarquia) para dropdown simples.
    /// │    Formato: "Nome do Setor (SIGLA)" (ex: "Tecnologia da Informação (TI)").
    /// │
    /// │ DIFERENÇA DE ListaSetores:
    /// │    - ListaSetores: Hierárquica (TreeView) com SetorPaiId.
    /// │    - ListaSetoresEvento: PLANA (DropDownList) sem SetorPaiId.
    /// │
    /// │ PROPRIEDADES:
    /// │    - SetorSolicitanteId: Guid do setor (string).
    /// │    - Nome: "Nome Completo (SIGLA)" (formato concatenado).
    /// │
    /// │ QUERY DATABASE:
    /// │    - Tabela: SetorSolicitante (tabela base, não View).
    /// │    - Ordenação: Alfabética por Nome (após concatenação).
    /// │
    /// │ FORMATO DO NOME:
    /// │    - Template: "{Nome} ({Sigla})"
    /// │    - Exemplo: "Diretoria Geral (DG)", "Recursos Humanos (RH)"
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulário de Evento: Seleção do setor organizador (dropdown simples).
    /// │    - Relatórios: Filtro simples por setor (sem hierarquia).
    /// │
    /// │ USO:
    /// │    Quando NÃO é necessário navegação hierárquica (pai/filho).
    /// │    Apenas seleção direta de um setor.
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Busca todos os setores da tabela base (não View)
                var objSetores = _unitOfWork.SetorSolicitante.GetAll();

                // [VALIDAÇÃO] - Se nenhum setor encontrado, retorna lista vazia
                if (objSetores == null || !objSetores.Any())
                {
                    System.Diagnostics.Debug.WriteLine("Nenhum setor encontrado para eventos.");
                    return new List<ListaSetoresEvento>();
                }

                List<ListaSetoresEvento> treeDataSource = new List<ListaSetoresEvento>();

                // [ETAPA 2] - Projeta para lista plana com formato "Nome (SIGLA)"
                foreach (var setor in objSetores)
                {
                    treeDataSource.Add(new ListaSetoresEvento
                    {
                        SetorSolicitanteId = setor.SetorSolicitanteId.ToString(),
                        // [ETAPA 2.1] - Concatena "Nome (Sigla)" para exibição
                        Nome = $"{setor.Nome} ({setor.Sigla})",
                    });
                }

                // [ETAPA 3] - Ordena alfabeticamente pelo Nome concatenado
                return treeDataSource.OrderBy(s => s.Nome).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em SetoresEventoList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return new List<ListaSetoresEvento>();
            }
        }
    }

    #endregion

    #region Lista de Setores Flat (para DropDownList com Indentação)

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaSetoresFlat (Setores Hierárquicos para DropDownList com Indentação)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista FLAT de setores hierárquicos com INDENTAÇÃO visual.
    /// │    Mostra hierarquia em dropdown simples usando "—" para nível.
    /// │
    /// │ DIFERENÇA DE OUTRAS LISTAS:
    /// │    - ListaSetores: TreeView (expandir/colapsar).
    /// │    - ListaSetoresEvento: Dropdown simples (sem hierarquia).
    /// │    - ListaSetoresFlat: Dropdown COM hierarquia visual (indentação).
    /// │
    /// │ PROPRIEDADES:
    /// │    - SetorSolicitanteId: Guid do setor (string).
    /// │    - Nome: "——— Nome" (indentação por travessões).
    /// │    - Nivel: Profundidade na hierarquia (0 = raiz, 1 = filho, 2 = neto, ...).
    /// │
    /// │ QUERY DATABASE:
    /// │    - Tabela: ViewSetores (View com hierarquia).
    /// │    - Projeção: SetorHierarquia (classe auxiliar interna).
    /// │
    /// │ CÁLCULO DE NÍVEL:
    /// │    - Método CalcularNivel(): Percorre SetorPaiId recursivamente até raiz (Guid.Empty).
    /// │    - Proteção contra loops circulares: HashSet<Guid> visitados.
    /// │    - Proteção contra overflow: maxNivel = 50.
    /// │
    /// │ INDENTAÇÃO VISUAL:
    /// │    - 0 níveis: "Diretoria Geral"
    /// │    - 1 nível:  "— TI"
    /// │    - 2 níveis: "—— Infraestrutura"
    /// │    - 3 níveis: "——— Redes"
    /// │
    /// │ ORDENAÇÃO:
    /// │    Alfabética pelo Nome COM indentação (mantém ordem alfabética dentro de cada nível).
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulários com dropdown de setores que precisam exibir hierarquia.
    /// │    - Relatórios: Filtro por setor com visualização hierárquica.
    /// │    - Onde TreeView é pesado demais, mas hierarquia é importante.
    /// │
    /// │ CLASSE AUXILIAR:
    /// │    SetorHierarquia: DTO interno para cálculo de nível (não exposto fora da classe).
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ CLASSE AUXILIAR INTERNA: SetorHierarquia (DTO para Cálculo de Nível)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    DTO interno usado no cálculo de nível hierárquico.
        /// │    NÃO exposto fora da classe ListaSetoresFlat.
        /// │
        /// │ PROPRIEDADES:
        /// │    - SetorSolicitanteId: Guid do setor.
        /// │    - SetorPaiId: Guid do setor pai (Guid.Empty se raiz).
        /// │    - Nome: Nome do setor (para debugging).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
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
                // [ETAPA 1] - Query ViewSetores e converte para classe auxiliar
                var objSetores = _unitOfWork.ViewSetores.GetAllReduced(
                    selector: x => new SetorHierarquia
                    {
                        SetorSolicitanteId = x.SetorSolicitanteId,
                        SetorPaiId = x.SetorPaiId ?? Guid.Empty, // NULL → Guid.Empty (raiz)
                        Nome = x.Nome,
                    }
                ).ToList();

                // [VALIDAÇÃO] - Se nenhum setor encontrado, retorna lista vazia
                if (objSetores == null || !objSetores.Any())
                {
                    System.Diagnostics.Debug.WriteLine("Nenhum setor encontrado para lista flat.");
                    return new List<ListaSetoresFlat>();
                }

                // [ETAPA 2] - Calcula nível de cada setor e adiciona indentação
                var resultado = objSetores.Select(setor =>
                {
                    // [ETAPA 2.1] - Calcula nível hierárquico (0 = raiz, 1 = filho, 2 = neto, ...)
                    int nivel = CalcularNivel(setor.SetorSolicitanteId, setor.SetorPaiId, objSetores);

                    // [ETAPA 2.2] - Gera indentação visual (travessões "—")
                    string indentacao = new string('—', nivel);

                    return new ListaSetoresFlat
                    {
                        SetorSolicitanteId = setor.SetorSolicitanteId.ToString(),
                        // [ETAPA 2.3] - Concatena indentação + Nome
                        Nome = $"{indentacao} {setor.Nome}",
                        Nivel = nivel
                    };
                })
                // [ETAPA 3] - Ordena alfabeticamente pelo Nome indentado
                .OrderBy(s => s.Nome)
                .ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Erro em SetoresListFlat - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}"
                );
                return new List<ListaSetoresFlat>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: CalcularNivel (Cálculo Recursivo de Profundidade Hierárquica)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Percorre SetorPaiId recursivamente até raiz (Guid.Empty) para calcular nível.
        /// │
        /// │ PARÂMETROS:
        /// │    - setorId: Guid do setor atual (para debugging).
        /// │    - setorPaiId: Guid do setor pai (inicial).
        /// │    - objSetores: Lista completa de setores (para buscar pais).
        /// │
        /// │ LÓGICA:
        /// │    1. Inicia com nivel = 0, paiAtual = setorPaiId.
        /// │    2. Enquanto paiAtual != Guid.Empty (não é raiz):
        /// │       - Verifica se já visitou (proteção loop circular).
        /// │       - Incrementa nivel++.
        /// │       - Busca setor pai na lista.
        /// │       - Atualiza paiAtual = pai.SetorPaiId (sobe na hierarquia).
        /// │    3. Retorna nivel final (0 = raiz, 1 = filho, 2 = neto, ...).
        /// │
        /// │ PROTEÇÕES:
        /// │    - HashSet<Guid> visitados: Evita loops circulares (A → B → A).
        /// │    - maxNivel = 50: Evita overflow em hierarquias profundas.
        /// │    - Loga avisos se detectar problemas (loop, pai não encontrado, overflow).
        /// │
        /// │ EXEMPLO:
        /// │    - Raiz (SetorPaiId = Guid.Empty): nivel = 0
        /// │    - Filho (SetorPaiId = Guid_Raiz): nivel = 1
        /// │    - Neto (SetorPaiId = Guid_Filho): nivel = 2
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        private int CalcularNivel(Guid setorId, Guid setorPaiId, List<SetorHierarquia> objSetores)
        {
            int nivel = 0;
            Guid paiAtual = setorPaiId;
            HashSet<Guid> visitados = new HashSet<Guid>(); // Proteção contra loops circulares
            int maxNivel = 50; // Proteção contra overflow (hierarquias muito profundas)

            // [ETAPA 1] - Loop até raiz (paiAtual == Guid.Empty)
            while (paiAtual != Guid.Empty && nivel < maxNivel)
            {
                // [ETAPA 1.1] - Verifica se já visitamos este setor (loop circular)
                if (visitados.Contains(paiAtual))
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Loop circular detectado no setor {paiAtual}");
                    break;
                }

                visitados.Add(paiAtual);
                nivel++;

                // [ETAPA 1.2] - Busca setor pai na lista
                var pai = objSetores.FirstOrDefault(s => s.SetorSolicitanteId == paiAtual);

                // [VALIDAÇÃO] - Se pai não encontrado, para busca (possível inconsistência no banco)
                if (pai == null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Setor pai {paiAtual} não encontrado");
                    break;
                }

                // [ETAPA 1.3] - Sobe na hierarquia (vai para o pai do pai)
                paiAtual = pai.SetorPaiId;
            }

            // [VALIDAÇÃO] - Se atingiu maxNivel, loga aviso (possível loop infinito)
            if (nivel >= maxNivel)
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ Nível máximo atingido para setor {setorId}");
            }

            return nivel;
        }
    }

    #endregion

    #region Lista de Dias da Semana

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaDias (Dias da Semana - Inglês/Português)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista hardcoded de dias da semana com ID em inglês (DayOfWeek enum) e
    /// │    descrição em português (exibição para usuário).
    /// │
    /// │ PROPRIEDADES:
    /// │    - DiaId: ID em inglês (Monday, Tuesday, ..., Sunday) - value do dropdown.
    /// │    - Dia: Nome em português (Segunda, Terça, ..., Domingo) - texto exibido.
    /// │
    /// │ MAPEAMENTO DayOfWeek:
    /// │    - DiaId usa NOME do enum DayOfWeek (Monday, Tuesday, ...).
    /// │    - Compatível com DateTime.DayOfWeek.ToString().
    /// │    - Permite conversão direta: Enum.Parse<DayOfWeek>(DiaId).
    /// │
    /// │ DIAS DISPONÍVEIS (7 itens):
    /// │    - Monday    → "Segunda"
    /// │    - Tuesday   → "Terça"
    /// │    - Wednesday → "Quarta"
    /// │    - Thursday  → "Quinta"
    /// │    - Friday    → "Sexta"
    /// │    - Saturday  → "Sábado"
    /// │    - Sunday    → "Domingo"
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulário de Escala: Seleção de dias da semana para escalas recorrentes.
    /// │    - Formulário de Evento: Dias de funcionamento do evento.
    /// │    - Relatórios: Filtro por dia da semana.
    /// │
    /// │ ATENÇÃO:
    /// │    IUnitOfWork é injetado mas NÃO utilizado (lista hardcoded).
    /// │    Manter injeção para consistência com outras classes.
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Lista hardcoded de 7 dias (ID em inglês, Descrição em pt-BR)
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
                System.Diagnostics.Debug.WriteLine($"Erro em DiasList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return new List<ListaDias>();
            }
        }
    }

    #endregion

    #region Lista de Períodos

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaPeriodos (Períodos de Recorrência)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista hardcoded de períodos de recorrência para eventos/escalas.
    /// │    Define frequência de repetição (Diário, Semanal, Quinzenal, Mensal).
    /// │
    /// │ PROPRIEDADES:
    /// │    - PeriodoId: Código do período (D, S, Q, M) - value do dropdown.
    /// │    - Periodo: Descrição em português (Diário, Semanal, ...) - texto exibido.
    /// │
    /// │ PERÍODOS DISPONÍVEIS (4 itens):
    /// │    - D → "Diário"     (repete todos os dias)
    /// │    - S → "Semanal"    (repete uma vez por semana)
    /// │    - Q → "Quinzenal"  (repete a cada 15 dias)
    /// │    - M → "Mensal"     (repete uma vez por mês)
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulário de Escala: Define frequência de repetição da escala.
    /// │    - Formulário de Evento: Define periodicidade do evento.
    /// │    - Sistema de Recorrência: Usado para calcular próximas ocorrências.
    /// │
    /// │ LÓGICA DE RECORRÊNCIA:
    /// │    - Backend usa PeriodoId para adicionar dias/semanas/meses à DataInicial.
    /// │    - Diário (D): +1 dia
    /// │    - Semanal (S): +7 dias
    /// │    - Quinzenal (Q): +15 dias
    /// │    - Mensal (M): +1 mês (AddMonths)
    /// │
    /// │ ATENÇÃO:
    /// │    IUnitOfWork NÃO injetado (lista hardcoded não precisa de DB).
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Lista hardcoded de 4 períodos de recorrência
                return new List<ListaPeriodos>
                {
                    new ListaPeriodos { PeriodoId = "D", Periodo = "Diário" },    // +1 dia
                    new ListaPeriodos { PeriodoId = "S", Periodo = "Semanal" },   // +7 dias
                    new ListaPeriodos { PeriodoId = "Q", Periodo = "Quinzenal" }, // +15 dias
                    new ListaPeriodos { PeriodoId = "M", Periodo = "Mensal" }     // +1 mês (AddMonths)
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em PeriodosList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return new List<ListaPeriodos>();
            }
        }
    }

    #endregion

    #region Lista de Recorrente

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaRecorrente (Flag Sim/Não para Recorrência)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista hardcoded de flag Sim/Não para indicar se evento/escala é recorrente.
    /// │    Usado em conjunto com ListaPeriodos (se "Sim", exibe dropdown de períodos).
    /// │
    /// │ PROPRIEDADES:
    /// │    - RecorrenteId: Código (N = Não, S = Sim) - value do dropdown.
    /// │    - Descricao: Texto exibido ("Não" ou "Sim") - texto no dropdown.
    /// │
    /// │ VALORES DISPONÍVEIS (2 itens):
    /// │    - N → "Não"  (evento/escala ocorre UMA VEZ apenas)
    /// │    - S → "Sim"  (evento/escala REPETE conforme período)
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Formulário de Escala: Define se escala repete ou não.
    /// │    - Formulário de Evento: Define se evento repete ou não.
    /// │
    /// │ FLUXO CONDICIONAL:
    /// │    1. Usuário seleciona "Sim" (S) → Exibe dropdown de ListaPeriodos (D/S/Q/M).
    /// │    2. Usuário seleciona "Não" (N) → Oculta dropdown de períodos (evento único).
    /// │
    /// │ ATENÇÃO:
    /// │    - Propriedade CORRIGIDA: "Recorrente" → "Descricao" (padronização com outras listas).
    /// │    - IUnitOfWork NÃO injetado (lista hardcoded).
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Lista hardcoded de 2 valores (Sim/Não)
                return new List<ListaRecorrente>
            {
                new ListaRecorrente { RecorrenteId = "N", Descricao = "Não" },  // Evento único
                new ListaRecorrente { RecorrenteId = "S", Descricao = "Sim" }   // Evento recorrente
            };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em RecorrenteList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return new List<ListaRecorrente>();
            }
        }
    }

    #endregion

    #region Lista de Status

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: ListaStatus (Status de Viagens para Filtros)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Lista hardcoded de status de viagens para filtros em dashboards/relatórios.
    /// │    Inclui opção "Todas" para não filtrar por status.
    /// │
    /// │ PROPRIEDADES:
    /// │    - StatusId: ID do status (usado na query WHERE) - value do dropdown.
    /// │    - Status: Descrição exibida ao usuário (plural) - texto no dropdown.
    /// │
    /// │ STATUS DISPONÍVEIS (4 itens):
    /// │    - "Todas"       → StatusId = "Todas"     (sem filtro, retorna tudo)
    /// │    - "Abertas"     → StatusId = "Aberta"    (viagens aguardando motorista)
    /// │    - "Realizadas"  → StatusId = "Realizada" (viagens finalizadas)
    /// │    - "Canceladas"  → StatusId = "Cancelada" (viagens canceladas)
    /// │
    /// │ ATENÇÃO - SINGULAR/PLURAL:
    /// │    - Status (exibido): PLURAL ("Abertas", "Realizadas", "Canceladas").
    /// │    - StatusId (query):  SINGULAR ("Aberta", "Realizada", "Cancelada").
    /// │    - Banco usa SINGULAR no campo Status da tabela Viagem.
    /// │
    /// │ APLICAÇÃO NO SISTEMA:
    /// │    - Dashboard de Viagens: Filtro de status (Abertas/Realizadas/Canceladas/Todas).
    /// │    - Relatórios: Filtro de viagens por status.
    /// │    - Grid de Viagens: Dropdown de filtro rápido.
    /// │
    /// │ LÓGICA DE FILTRO:
    /// │    - Se StatusId == "Todas": Query SEM WHERE (retorna todos os status).
    /// │    - Senão: WHERE v.Status == StatusId (filtra por status específico).
    /// │
    /// │ ATENÇÃO:
    /// │    IUnitOfWork é injetado mas NÃO utilizado (lista hardcoded).
    /// │    Manter injeção para consistência com outras classes.
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
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
                // [ETAPA 1] - Lista hardcoded de 4 status (1 sem filtro + 3 com filtro)
                return new List<ListaStatus>
                {
                    new ListaStatus { Status = "Todas", StatusId = "Todas" },              // Sem filtro (retorna tudo)
                    new ListaStatus { Status = "Abertas", StatusId = "Aberta" },           // Viagens pendentes
                    new ListaStatus { Status = "Realizadas", StatusId = "Realizada" },     // Viagens finalizadas
                    new ListaStatus { Status = "Canceladas", StatusId = "Cancelada" }      // Viagens canceladas
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em StatusList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return new List<ListaStatus>();
            }
        }
    }

    #endregion

    #region Lista de TipoServico

    public class ListaTipoServico
    {
        public Guid TipoServicoId
        {
            get; set;
        }
        public string NomeServico
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaTipoServico()
        {
        }
        public ListaTipoServico(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ListaTipoServico> ServicoList()
        {
            try
            {
                var servicos = _unitOfWork.ViewEscalasCompletas.GetAllReduced(
                    orderBy: ts => ts.OrderBy(ts => ts.NomeServico),
                    selector: servicos => new ListaTipoServico
                    {
                        TipoServicoId = servicos.TipoServicoId ?? Guid.Empty,
                        NomeServico = servicos.NomeServico,
                    }
                );

                return servicos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em TipoServicoList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return Enumerable.Empty<ListaTipoServico>();
            }
        }

    }
    #endregion

    #region Lista de Turno
    public class ListaTurno
    {
        public Guid TurnoId { get; set; }

        public string NomeTurno { get; set; }

        public TurnoEnum? TurnoTipo { get; set; }

        private readonly IUnitOfWork _unitOfWork;

        public ListaTurno()
        {
        }

        public ListaTurno(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ListaTurno> TurnoList()
        {
            try
            {
                var turnos = _unitOfWork.ViewEscalasCompletas.GetAllReduced(
                    orderBy: tr => tr.OrderBy(tr => tr.NomeTurno),
                    selector: turnos => new ListaTurno
                    {
                        TurnoId = turnos.TurnoId ?? Guid.Empty,
                        NomeTurno = turnos.NomeTurno ?? "",
                        TurnoTipo = MapearEnum(turnos.NomeTurno)
                    }
                );
                return turnos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em TurnoList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return Enumerable.Empty<ListaTurno>();
            }
        }
        private static TurnoEnum? MapearEnum(string? nomeTurno)
        {
            if (string.IsNullOrWhiteSpace(nomeTurno))
                return null;


            return nomeTurno switch
            {
                "Matutino" => TurnoEnum.Matutino,
                "Vespertino" => TurnoEnum.Vespertino,
                "Noturno" => TurnoEnum.Noturno,
                _ => null
            };
        }

        public string ObterNomeDisplay()
        {
            if (!TurnoTipo.HasValue)
                return NomeTurno;

            var field = typeof(TurnoEnum).GetField(TurnoTipo.Value.ToString());
            var displayAttr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;

            return displayAttr?.Name ?? NomeTurno;
        }

        public bool EhTurnoValido()
        {
            return TurnoTipo.HasValue;
        }
    }
    #endregion

    #region Lista de Lotacao

    public class ListaLotacao
    {
        public string? Lotacao
        {
            get; set;
        }

        public LotacaoEnum? TipoLotacao { get; set; }

        private readonly IUnitOfWork _unitOfWork;

        public ListaLotacao()
        {
        }

        public ListaLotacao(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ListaLotacao> LotacaoList()
        {
            try
            {
                var lotacoes = _unitOfWork.ViewEscalasCompletas.GetAllReduced(
                    orderBy: lt => lt.OrderBy(lt => lt.Lotacao),
                    selector: lotacoes => new ListaLotacao
                    {
                        Lotacao = lotacoes.Lotacao ?? "",
                        TipoLotacao = MapearEnum(lotacoes.Lotacao),
                    }
                );

                return lotacoes;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em LotacaoList - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return Enumerable.Empty<ListaLotacao>();
            }
        }

        private static LotacaoEnum? MapearEnum(string? lotacao)
        {
            if (string.IsNullOrWhiteSpace(lotacao))
                return null;

            return lotacao switch
            {
                "Aeroporto" => LotacaoEnum.Aeroporto,
                "PGR" => LotacaoEnum.PGR,
                "Rodoviária" => LotacaoEnum.Rodoviaria,
                "Setor de Obras" => LotacaoEnum.SetorObras,
                "CEFOR" => LotacaoEnum.Cefor,
                "Outros" => LotacaoEnum.Outros,
                _ => null
            };
        }

        public string ObterNomeDisplay()
        {
            if (!TipoLotacao.HasValue)
                return Lotacao;

            var field = typeof(LotacaoEnum).GetField(TipoLotacao.Value.ToString());
            var displayAttr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;

            return displayAttr?.Name ?? Lotacao;
        }

    }
    #endregion

    #region Lista de StatusMotorista

    public class ListaStatusMotorista
    {
        public string? StatusMotorista
        {
            get; set;
        }

        public StatusMotoristaEnum? TipoStatusM { get; set; }

        private readonly IUnitOfWork _unitOfWork;

        public ListaStatusMotorista()
        {
        }

        public ListaStatusMotorista(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ListaStatusMotorista> StatusMList()
        {
            try
            {
                var statusmotorista = _unitOfWork.ViewEscalasCompletas.GetAllReduced(
                    orderBy: sm => sm.OrderBy(sm => sm.StatusMotorista),
                    selector: statusmotorista => new ListaStatusMotorista
                    {
                        StatusMotorista = statusmotorista.StatusMotorista ?? "",
                        TipoStatusM = MapearEnum(statusmotorista.StatusMotorista),
                    }
                );

                return statusmotorista;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro em ListaStatusMotorista - Linha: {new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber()} - {ex.Message}");
                return Enumerable.Empty<ListaStatusMotorista>();
            }
        }

        private static StatusMotoristaEnum? MapearEnum(string? statusM)
        {
            if (string.IsNullOrWhiteSpace(statusM))
                return null;

            return statusM switch
            {
                "Disponível" => StatusMotoristaEnum.Disponivel,
                "Em Viagem" => StatusMotoristaEnum.EmViagem,
                "Em Serviço" => StatusMotoristaEnum.EmServico,
                "Indisponível" => StatusMotoristaEnum.Indisponivel,
                "Economildo" => StatusMotoristaEnum.Economildo,
                _ => null
            };
        }

        public string ObterNomeDisplay()
        {
            if (!TipoStatusM.HasValue)
                return StatusMotorista;

            var field = typeof(StatusMotoristaEnum).GetField(TipoStatusM.Value.ToString());
            var displayAttr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;

            return displayAttr?.Name ?? StatusMotorista;
        }

    }
    #endregion
}

