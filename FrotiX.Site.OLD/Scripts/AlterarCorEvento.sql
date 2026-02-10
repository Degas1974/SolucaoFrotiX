-- ================================================================
-- Script: Alterar cor de Eventos na Agenda
-- Descrição: Altera a cor dos agendamentos tipo "Evento" de #4C2B08 para #8C7961
-- Data: 2026-01-12
-- ================================================================

USE [FrotiX]
GO

PRINT 'Alterando VIEW ViewViagensAgenda - Cor do Evento #4C2B08 -> #8C7961'
GO

CREATE OR ALTER VIEW dbo.ViewViagensAgenda
WITH SCHEMABINDING
AS -- ================================================================
-- VIEW: ViewViagensAgenda
-- Descrição: View para alimentar o calendário FullCalendar
-- Atualizado: Cores correspondentes aos Modais do sistema
-- Última alteração: 12/01/2026 - Cor do Evento alterada para #8C7961
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
    CONVERT(
        DATETIME,
        CONVERT(VARCHAR(10), v.DataInicial, 120) + ' ' + CONVERT(VARCHAR(8), v.HoraInicio, 108),
        120
    ) AS [Start],

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
    Titulo = CASE
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL
            THEN 'Evento: ' + e.Nome
        ELSE v.Finalidade
    END,

    -- ============================================================
    -- COR DO EVENTO - Correspondente aos Headers dos Modais
    -- ============================================================
    -- Prioridade: 1. Evento, 2. Cancelada, 3. Realizada, 4. Agendada, 5. Aberta
    CorEvento = CASE
        -- EVENTO (Finalidade = 'Evento') - Bege Rosado #8C7961 (alterado em 12/01/2026)
        WHEN v.Finalidade = 'Evento'
            THEN '#8C7961'

        -- CANCELADA - Vinho #722F37
        WHEN v.Status = 'Cancelada'
            THEN '#722F37'

        -- REALIZADA - Azul Petróleo #154c62
        WHEN v.Status = 'Realizada'
            THEN '#154c62'

        -- AGENDADA (Status = 'Agendada' OU Status = 'Aberta' com StatusAgendamento = 1) - Laranja #EF6C00
        WHEN v.Status = 'Agendada'
            THEN '#EF6C00'
        WHEN v.Status = 'Aberta' AND ISNULL(CAST(v.StatusAgendamento AS INT), 0) = 1
            THEN '#EF6C00'

        -- ABERTA (viagem já transformada) - Verde Militar #476b47
        WHEN v.Status = 'Aberta' AND ISNULL(CAST(v.StatusAgendamento AS INT), 0) = 0
            THEN '#476b47'

        -- DEFAULT - Cinza
        ELSE '#6c757d'
    END,

    -- ============================================================
    -- COR DO TEXTO - Branco para todas (fundos escuros)
    -- ============================================================
    CorTexto = '#FFFFFF',

    -- ============================================================
    -- DESCRIÇÃO GENÉRICA (sem HTML)
    -- ============================================================
    DescricaoMontada =
        ISNULL(m.Nome, '(Motorista Não Informado)')
        + ' - (' + ISNULL(vec.Placa, 'Sem Veículo') + ')'
        + CASE
            WHEN v.DescricaoSemFormato IS NOT NULL
            THEN ' - ' +
                REPLACE(
                    REPLACE(
                        REPLACE(
                            CAST(v.DescricaoSemFormato AS VARCHAR(MAX)),
                            CHAR(13), ''
                        ),
                        CHAR(10), ' '
                    ),
                    CHAR(9), ' '
                )
            ELSE ''
        END,

    -- ============================================================
    -- DESCRIÇÃO ESPECIAL PARA EVENTOS
    -- ============================================================
    DescricaoEvento = CASE
        -- Evento Cancelado
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL AND v.Status = 'Cancelada'
            THEN 'Evento CANCELADO: ' + e.Nome + ' / '
                + ISNULL(m.Nome, '(Motorista Não Identificado)')
                + ' - (' + ISNULL(vec.Placa, 'Sem Veículo') + ')'
                + CASE
                    WHEN v.DescricaoSemFormato IS NOT NULL
                    THEN ' - ' +
                        REPLACE(
                            REPLACE(
                                REPLACE(
                                    CAST(v.DescricaoSemFormato AS VARCHAR(MAX)),
                                    CHAR(13), ''
                                ),
                                CHAR(10), ' '
                            ),
                            CHAR(9), ' '
                        )
                    ELSE ''
                END

        -- Evento Ativo
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL
            THEN 'Evento: ' + e.Nome + ' / '
                + ISNULL(m.Nome, '(Motorista Não Identificado)')
                + ' - (' + ISNULL(vec.Placa, 'Sem Veículo') + ')'
                + CASE
                    WHEN v.DescricaoSemFormato IS NOT NULL
                    THEN ' - ' +
                        REPLACE(
                            REPLACE(
                                REPLACE(
                                    CAST(v.DescricaoSemFormato AS VARCHAR(MAX)),
                                    CHAR(13), ''
                                ),
                                CHAR(10), ' '
                            ),
                            CHAR(9), ' '
                        )
                    ELSE ''
                END

        -- Não é evento
        ELSE NULL
    END

FROM dbo.Viagem v WITH (NOLOCK)
LEFT JOIN dbo.Motorista m WITH (NOLOCK) ON v.MotoristaId = m.MotoristaId
LEFT JOIN dbo.Veiculo vec WITH (NOLOCK) ON v.VeiculoId = vec.VeiculoId
LEFT JOIN dbo.Evento e WITH (NOLOCK) ON v.EventoId = e.EventoId

WHERE v.DataInicial IS NOT NULL
  AND v.HoraInicio IS NOT NULL;
GO

PRINT 'VIEW ViewViagensAgenda atualizada com sucesso!'
PRINT 'Cor do Evento alterada: #4C2B08 -> #8C7961'
GO
