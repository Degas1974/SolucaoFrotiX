-- ================================================================
-- SCRIPT: Correção da VIEW ViewViagensAgenda
-- Data: 20/01/2026
-- Motivo: Remover SCHEMABINDING que causa erro com VARCHAR(MAX)
-- ================================================================

USE FrotiX
GO

-- Drop da view existente
IF OBJECT_ID('dbo.ViewViagensAgenda', 'V') IS NOT NULL
    DROP VIEW dbo.ViewViagensAgenda
GO

-- Recriar VIEW SEM SCHEMABINDING (necessário para VARCHAR(MAX))
CREATE VIEW dbo.ViewViagensAgenda
AS
-- ================================================================
-- VIEW: ViewViagensAgenda
-- Descrição: View para alimentar o calendário FullCalendar
-- Atualizado: Cores correspondentes aos Modais do sistema
-- Última alteração: 20/01/2026 - Removido SCHEMABINDING
-- ================================================================

SELECT
    -- ============================================================
    -- IDENTIFICADORES
    -- ============================================================
    v.ViagemId,
    v.MotoristaId,
    v.VeiculoId,
    v.EventoId,

    -- ============================================================
    -- DATAS E HORÁRIOS
    -- ============================================================
    v.DataInicial,
    v.HoraInicio,
    v.DataFinal,
    v.HoraFim,

    -- Campo Start para FullCalendar (DATETIME combinando data + hora)
    CASE
        WHEN v.DataInicial IS NOT NULL AND v.HoraInicio IS NOT NULL
        THEN DATEADD(
            SECOND,
            DATEDIFF(SECOND, 0, CAST(v.HoraInicio AS TIME)),
            CAST(v.DataInicial AS DATETIME)
        )
        ELSE v.DataInicial
    END AS [Start],

    -- Campo End para FullCalendar
    v.HoraFim AS [End],

    -- ============================================================
    -- STATUS E FLAGS
    -- ============================================================
    v.Status,
    v.StatusAgendamento,
    v.FoiAgendamento,
    v.Finalidade,

    -- ============================================================
    -- INFORMAÇÕES DO EVENTO
    -- ============================================================
    v.NomeEvento,
    e.Nome AS NomeEventoFull,

    -- ============================================================
    -- INFORMAÇÕES DE MOTORISTA E VEÍCULO
    -- ============================================================
    m.Nome AS NomeMotorista,
    vec.Placa,

    -- ============================================================
    -- TÍTULO DO EVENTO NA AGENDA
    -- ============================================================
    CASE
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL
            THEN 'Evento: ' + e.Nome
        ELSE v.Finalidade
    END AS Titulo,

    -- ============================================================
    -- COR DO EVENTO - Correspondente aos Headers dos Modais
    -- ============================================================
    CASE
        -- EVENTO (Finalidade = 'Evento') - Bege claro #A39481
        WHEN v.Finalidade = 'Evento'
            THEN '#A39481'

        -- CANCELADA - Vinho #722F37
        WHEN v.Status = 'Cancelada'
            THEN '#722F37'

        -- REALIZADA - Azul Petróleo #154c62
        WHEN v.Status = 'Realizada'
            THEN '#154c62'

        -- AGENDADA - Laranja #FFA726
        WHEN v.Status = 'Agendada'
            THEN '#FFA726'
        WHEN v.Status = 'Aberta' AND ISNULL(CAST(v.StatusAgendamento AS INT), 0) = 1
            THEN '#FFA726'

        -- ABERTA - Verde Militar #476b47
        WHEN v.Status = 'Aberta' AND ISNULL(CAST(v.StatusAgendamento AS INT), 0) = 0
            THEN '#476b47'

        -- DEFAULT - Cinza
        ELSE '#6c757d'
    END AS CorEvento,

    -- ============================================================
    -- COR DO TEXTO - Branco para todas (fundos escuros)
    -- ============================================================
    '#FFFFFF' AS CorTexto,

    -- ============================================================
    -- DESCRIÇÃO GENÉRICA (sem HTML)
    -- ============================================================
    ISNULL(m.Nome, '(Motorista Não Informado)')
        + ' - (' + ISNULL(vec.Placa, 'Sem Veículo') + ')'
        + CASE
            WHEN v.DescricaoSemFormato IS NOT NULL AND LEN(CAST(v.DescricaoSemFormato AS NVARCHAR(MAX))) > 0
            THEN ' - ' + CAST(v.DescricaoSemFormato AS NVARCHAR(500))
            ELSE ''
        END AS DescricaoMontada,

    -- ============================================================
    -- DESCRIÇÃO ESPECIAL PARA EVENTOS
    -- ============================================================
    CASE
        -- Evento Cancelado
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL AND v.Status = 'Cancelada'
            THEN 'Evento CANCELADO: ' + e.Nome + ' / '
                + ISNULL(m.Nome, '(Motorista Não Identificado)')
                + ' - (' + ISNULL(vec.Placa, 'Sem Veículo') + ')'

        -- Evento Ativo
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL
            THEN 'Evento: ' + e.Nome + ' / '
                + ISNULL(m.Nome, '(Motorista Não Identificado)')
                + ' - (' + ISNULL(vec.Placa, 'Sem Veículo') + ')'

        -- Não é evento
        ELSE NULL
    END AS DescricaoEvento,

    -- ============================================================
    -- DESCRIÇÃO PURA (para tooltips customizadas)
    -- ============================================================
    CASE
        WHEN v.DescricaoSemFormato IS NOT NULL AND LEN(CAST(v.DescricaoSemFormato AS NVARCHAR(MAX))) > 0
        THEN CAST(v.DescricaoSemFormato AS NVARCHAR(500))
        ELSE NULL
    END AS Descricao

FROM dbo.Viagem v
LEFT JOIN dbo.Motorista m ON v.MotoristaId = m.MotoristaId
LEFT JOIN dbo.Veiculo vec ON v.VeiculoId = vec.VeiculoId
LEFT JOIN dbo.Evento e ON v.EventoId = e.EventoId

WHERE v.DataInicial IS NOT NULL
  AND v.HoraInicio IS NOT NULL
GO

PRINT 'VIEW ViewViagensAgenda recriada com sucesso (sem SCHEMABINDING)!'
GO
