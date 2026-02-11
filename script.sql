/*
Script: Atualiza flags de documento/cartao em Viagem
Data: 2026-02-10
Observacao: inclui rollback ao final (execute manualmente se necessario)
*/

SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRAN;

    -- Adiciona novas colunas (se ainda nao existirem)
    IF COL_LENGTH('dbo.Viagem', 'DocumentoEntregue') IS NULL
        ALTER TABLE dbo.Viagem
            ADD DocumentoEntregue bit NULL
                CONSTRAINT DF_Viagem_DocumentoEntregue DEFAULT (0);

    IF COL_LENGTH('dbo.Viagem', 'DocumentoDevolvido') IS NULL
        ALTER TABLE dbo.Viagem
            ADD DocumentoDevolvido bit NULL
                CONSTRAINT DF_Viagem_DocumentoDevolvido DEFAULT (0);

    IF COL_LENGTH('dbo.Viagem', 'CartaoAbastecimentoEntregue') IS NULL
        ALTER TABLE dbo.Viagem
            ADD CartaoAbastecimentoEntregue bit NULL
                CONSTRAINT DF_Viagem_CartaoAbastecimentoEntregue DEFAULT (0);

    IF COL_LENGTH('dbo.Viagem', 'CartaoAbastecimentoDevolvido') IS NULL
        ALTER TABLE dbo.Viagem
            ADD CartaoAbastecimentoDevolvido bit NULL
                CONSTRAINT DF_Viagem_CartaoAbastecimentoDevolvido DEFAULT (0);

    -- Backfill a partir dos campos legados (string/boolean)
    UPDATE dbo.Viagem
    SET DocumentoEntregue = CASE
            WHEN StatusDocumento IN ('Entregue', '1', 'true', 'True') THEN 1
            ELSE 0
        END,
        DocumentoDevolvido = CASE
            WHEN StatusDocumentoFinal IN ('Devolvido', '1', 'true', 'True') THEN 1
            ELSE 0
        END,
        CartaoAbastecimentoEntregue = CASE
            WHEN StatusCartaoAbastecimento IN ('Entregue', '1', 'true', 'True') THEN 1
            ELSE 0
        END,
        CartaoAbastecimentoDevolvido = CASE
            WHEN StatusCartaoAbastecimentoFinal IN ('Devolvido', '1', 'true', 'True') THEN 1
            ELSE 0
        END
    WHERE StatusDocumento IS NOT NULL
       OR StatusDocumentoFinal IS NOT NULL
       OR StatusCartaoAbastecimento IS NOT NULL
       OR StatusCartaoAbastecimentoFinal IS NOT NULL;

    -- Remove constraints defaults dos campos legados (se existirem)
    DECLARE @sql nvarchar(max);

    SELECT @sql = N'ALTER TABLE dbo.Viagem DROP CONSTRAINT ' + QUOTENAME(dc.name)
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id
       AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.Viagem')
      AND c.name = 'StatusDocumento';
    IF @sql IS NOT NULL EXEC sp_executesql @sql;

    SELECT @sql = N'ALTER TABLE dbo.Viagem DROP CONSTRAINT ' + QUOTENAME(dc.name)
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id
       AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.Viagem')
      AND c.name = 'StatusCartaoAbastecimento';
    IF @sql IS NOT NULL EXEC sp_executesql @sql;

    SELECT @sql = N'ALTER TABLE dbo.Viagem DROP CONSTRAINT ' + QUOTENAME(dc.name)
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id
       AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.Viagem')
      AND c.name = 'StatusDocumentoFinal';
    IF @sql IS NOT NULL EXEC sp_executesql @sql;

    SELECT @sql = N'ALTER TABLE dbo.Viagem DROP CONSTRAINT ' + QUOTENAME(dc.name)
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id
       AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.Viagem')
      AND c.name = 'StatusCartaoAbastecimentoFinal';
    IF @sql IS NOT NULL EXEC sp_executesql @sql;

    -- Remove colunas legadas
    IF COL_LENGTH('dbo.Viagem', 'StatusDocumento') IS NOT NULL
        ALTER TABLE dbo.Viagem DROP COLUMN StatusDocumento;
    IF COL_LENGTH('dbo.Viagem', 'StatusCartaoAbastecimento') IS NOT NULL
        ALTER TABLE dbo.Viagem DROP COLUMN StatusCartaoAbastecimento;
    IF COL_LENGTH('dbo.Viagem', 'StatusDocumentoFinal') IS NOT NULL
        ALTER TABLE dbo.Viagem DROP COLUMN StatusDocumentoFinal;
    IF COL_LENGTH('dbo.Viagem', 'StatusCartaoAbastecimentoFinal') IS NOT NULL
        ALTER TABLE dbo.Viagem DROP COLUMN StatusCartaoAbastecimentoFinal;

    COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    THROW;
END CATCH;
GO

-- View principal
CREATE OR ALTER VIEW dbo.ViewViagens
AS SELECT
        Viagem.ViagemId,
        Viagem.DataInicial,
        Viagem.DataFinal,
        Viagem.HoraInicio,
        Viagem.HoraFim,
        Viagem.Descricao,
        Viagem.Status,
        Viagem.KmInicial,
        Viagem.KmFinal,
        Viagem.CombustivelInicial,
        Viagem.CombustivelFinal,
        Viagem.MotoristaId,
        Viagem.VeiculoId,
        Viagem.ResumoOcorrencia,
        Viagem.DescricaoOcorrencia,
        Viagem.StatusOcorrencia,
        Viagem.DocumentoEntregue,
        Viagem.DocumentoDevolvido,
        Viagem.CartaoAbastecimentoEntregue,
        Viagem.CartaoAbastecimentoDevolvido,
        Viagem.StatusAgendamento,
        Viagem.NoFichaVistoria,
        Viagem.Finalidade,
        Veiculo.Placa,
        Motorista.CNH,
        Motorista.Foto,
        Motorista.Ponto,
        convert(nvarchar(36), Viagem.ViagemId) AS ViagemIDStr,
        CASE
            WHEN Requisitante.Ramal IS NULL THEN Requisitante.Nome
            ELSE Requisitante.Nome + ' - (' + CONVERT(VARCHAR, Requisitante.Ramal) + ')'
        END AS NomeRequisitante,
        CASE
            WHEN SetorSolicitante.Sigla IS NULL THEN SetorSolicitante.Nome
            ELSE SetorSolicitante.Nome + ' - (' + SetorSolicitante.Sigla + ')'
        END AS NomeSetor,
        Requisitante.RequisitanteId,
        SetorSolicitante.SetorSolicitanteId,
        Motorista.Nome AS NomeMotorista,
        '(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo,
        Veiculo.UnidadeId,
        ROW_NUMBER() OVER (ORDER BY Viagem.DataInicial, Viagem.HoraInicio) AS RowNum,
        Veiculo.CombustivelId,
        Viagem.DescricaoSolucaoOcorrencia,
        Viagem.FichaVistoria,
        Viagem.Origem,
        Viagem.Destino,
        Viagem.Minutos,
        Viagem.NomeEvento,
        Viagem.RamalRequisitante,
        Viagem.ImagemOcorrencia,
        Viagem.ItemManutencaoId,
        ROUND((Viagem.CustoCombustivel + Viagem.CustoMotorista + Viagem.CustoVeiculo + Viagem.CustoOperador + Viagem.CustoLavador), 2) AS CustoViagem,
        Viagem.EventoId,
        CorEvento = CASE
            WHEN Viagem.StatusAgendamento = 1 AND Viagem.Status <> 'Cancelada' AND Viagem.Finalidade = 'Evento' THEN '#E99B63'
            WHEN Viagem.StatusAgendamento = 1 AND Viagem.Status <> 'Cancelada' THEN '#29B3FF'
            WHEN Viagem.Status = 'Cancelada' THEN '#E34234'
            WHEN Viagem.Status = 'Aberta' AND ISNULL(Viagem.StatusAgendamento, 0) = 0 THEN '#FFD774'
            WHEN Viagem.Status = 'Realizada' AND ISNULL(Viagem.FoiAgendamento, 0) = 1 THEN '#52688F'
            WHEN Viagem.Status = 'Realizada' THEN '#75B390'
            WHEN Viagem.Finalidade = 'Evento' THEN '#E99B63'
            ELSE '#29B3FF'
        END,
        CorTexto = CASE
            WHEN (Viagem.StatusAgendamento = 1 AND Viagem.Status <> 'Cancelada')
              OR (Viagem.Status = 'Realizada')
              OR (Viagem.Finalidade = 'Evento')
              OR (Viagem.Status = 'Cancelada')
              THEN 'white'
            WHEN (Viagem.Status = 'Aberta' AND ISNULL(Viagem.StatusAgendamento, 0) = 0)
              THEN '#2B6670'
            ELSE 'white'
        END,
        DescricaoMontada =
            (
                ISNULL(Motorista.Nome, '(Motorista Nao Identificado)')
                + ' - (' + ISNULL(Veiculo.Placa, 'Sem Veiculo') + ')'
                + CASE
                    WHEN Viagem.Descricao IS NOT NULL AND LEN(LTRIM(RTRIM(Viagem.Descricao))) > 0
                    THEN ' - ' + Viagem.Descricao
                    ELSE ''
                  END
            ),
        DescricaoEvento =
            CASE
                WHEN Viagem.Finalidade = 'Evento' AND Viagem.NomeEvento IS NOT NULL AND Viagem.Status = 'Cancelada'
                    THEN 'Evento CANCELADO: ' + Viagem.NomeEvento + ' / '
                        + ISNULL(Motorista.Nome, '(Motorista Nao Identificado)')
                        + ' - (' + ISNULL(Veiculo.Placa, 'Sem Veiculo') + ')'
                        + CASE WHEN Viagem.Descricao IS NOT NULL AND LEN(LTRIM(RTRIM(Viagem.Descricao))) > 0
                            THEN ' - ' + Viagem.Descricao
                            ELSE '' END
                WHEN Viagem.Finalidade = 'Evento' AND Viagem.NomeEvento IS NOT NULL
                    THEN 'Evento: ' + Viagem.NomeEvento + ' / '
                        + ISNULL(Motorista.Nome, '(Motorista Nao Identificado)')
                        + ' - (' + ISNULL(Veiculo.Placa, 'Sem Veiculo') + ')'
                        + CASE WHEN Viagem.Descricao IS NOT NULL AND LEN(LTRIM(RTRIM(Viagem.Descricao))) > 0
                            THEN ' - ' + Viagem.Descricao
                            ELSE '' END
                ELSE NULL
            END,
        Titulo =
            CASE
                WHEN Viagem.Finalidade = 'Evento' AND Viagem.NomeEvento IS NOT NULL THEN 'Evento : ' + Viagem.NomeEvento
                ELSE Viagem.Finalidade
            END
    FROM dbo.SetorSolicitante
    RIGHT OUTER JOIN dbo.Viagem
        ON SetorSolicitante.SetorSolicitanteId = Viagem.SetorSolicitanteId
    LEFT OUTER JOIN dbo.Requisitante
        ON Requisitante.RequisitanteId = Viagem.RequisitanteId
    LEFT OUTER JOIN dbo.Motorista
        ON Motorista.MotoristaId = Viagem.MotoristaId
    LEFT OUTER JOIN dbo.Veiculo
        ON Veiculo.VeiculoId = Viagem.VeiculoId
    INNER JOIN dbo.ModeloVeiculo
        ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
    INNER JOIN dbo.MarcaVeiculo
        ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId;
GO

-- Procedure usada por relatorios/consultas
CREATE OR ALTER PROCEDURE frotix.ViewViagens_SP
AS
SELECT [ViagemId]
          ,[DataInicial]
          ,[DataFinal]
          ,[HoraInicio]
          ,[HoraFim]
          ,[CombustivelInicial]
          ,[CombustivelFinal]
          ,[KmAtual]
          ,[KmInicial]
          ,[KmFinal]
          ,[Descricao]
          ,[RamalRequisitante]
          ,[Finalidade]
          ,[Status]
          ,[RequisitanteId]
          ,[SetorSolicitanteId]
          ,[VeiculoId]
          ,[MotoristaId]
          ,[UsuarioIdCriacao]
          ,[DataCriacao]
          ,[UsuarioIdFinalizacao]
          ,[DataFinalizacao]
          ,[ResumoOcorrencia]
          ,[DescricaoOcorrencia]
          ,[StatusOcorrencia]
          ,[DocumentoEntregue]
          ,[DocumentoDevolvido]
          ,[CartaoAbastecimentoEntregue]
          ,[CartaoAbastecimentoDevolvido]
          ,[StatusAgendamento]
          ,[Origem]
          ,[Destino]
          ,[DescricaoSemFormato]
          ,[CustoCombustivel]
          ,[CustoMotorista]
          ,[CustoVeiculo]
          ,[CustoOperador]
          ,[CustoLavador]
          ,[Minutos]
          ,[NomeEvento]
          ,[DescricaoSolucaoOcorrencia]
          ,[ImagemOcorrencia]
          ,[ItemManutencaoId]
          ,[EventoId]
          ,[NoFichaVistoria]
     INTO #Viagem FROM [Viagem] (NOLOCK);

    CREATE NONCLUSTERED INDEX #IDX_Prueba ON #Viagem (SetorSolicitanteId);
    CREATE NONCLUSTERED INDEX #IDX_VeiculoId ON #Viagem (VeiculoId);
    CREATE NONCLUSTERED INDEX #IDX_StatusOcorrencia ON #Viagem (StatusOcorrencia);
    CREATE NONCLUSTERED INDEX #IDX_Status ON #Viagem ([Status]);
    CREATE NONCLUSTERED INDEX #IDX_MotoristaId ON #Viagem ([MotoristaId]);
    CREATE NONCLUSTERED INDEX #IDX_RequisitanteID ON #Viagem (RequisitanteID);

    SELECT [MotoristaId]
          ,[Nome]
          ,[Ponto]
          ,[DataNascimento]
          ,[CPF]
          ,[CNH]
          ,[CategoriaCNH]
          ,[DataVencimentoCNH]
          ,[Celular01]
          ,[Celular02]
          ,[DataIngresso]
          ,[OrigemIndicacao]
          ,[Status]
          ,[UsuarioIdAlteracao]
          ,[DataAlteracao]
          ,[UnidadeId]
          ,[ContratoId]
          ,[CondutorId]
          ,[TipoCondutor]
          ,[EfetivoFerista]
      INTO #Motorista FROM [Motorista];

    CREATE NONCLUSTERED INDEX #IDX_Motorista ON #Motorista (MotoristaId);

    SELECT [VeiculoId]
          ,[Placa]
          ,[Quilometragem]
          ,[Renavam]
          ,[PlacaVinculada]
          ,[AnoFabricacao]
          ,[AnoModelo]
          ,[Reserva]
          ,[VeiculoProprio]
          ,[Status]
          ,[DataAlteracao]
          ,[UsuarioIdAlteracao]
          ,[UnidadeId]
          ,[MarcaId]
          ,[ModeloId]
          ,[CombustivelId]
          ,[ContratoId]
          ,[AtaId]
          ,[PlacaBronzeId]
          ,[ItemVeiculoId]
          ,[Patrimonio]
          ,[ItemVeiculoAtaId]
          ,[Categoria]
          ,[DataIngresso]
      INTO #Veiculo FROM [Veiculo];

    CREATE NONCLUSTERED INDEX #IDX_Veiculo ON #Veiculo (VeiculoId);

    SELECT
     Viagem.ViagemId
     ,CONVERT(VARCHAR, Viagem.DataInicial, 103) AS DataInicial
     ,CONVERT(VARCHAR, Viagem.DataFinal, 103) AS DataFinal
     ,FORMAT(Viagem.HoraInicio, 'HH:mm') AS HoraInicio
     ,FORMAT(Viagem.HoraFim, 'HH:mm') AS HoraFim
     ,Viagem.Descricao
     ,Viagem.Status
     ,Viagem.KmInicial
     ,Viagem.KmFinal
     ,Viagem.CombustivelInicial
     ,Viagem.CombustivelFinal
     ,Viagem.MotoristaId
     ,Viagem.VeiculoId
     ,Viagem.ResumoOcorrencia
     ,Viagem.DescricaoOcorrencia
     ,Viagem.StatusOcorrencia
     ,Viagem.DocumentoEntregue
     ,Viagem.DocumentoDevolvido
     ,Viagem.CartaoAbastecimentoEntregue
     ,Viagem.CartaoAbastecimentoDevolvido
     ,Viagem.StatusAgendamento
     ,Viagem.NoFichaVistoria
     ,Viagem.Finalidade
     ,Veiculo.Placa
     ,Motorista.Ponto
    ,convert(nvarchar(36), Viagem.ViagemId) AS ViagemIDStr
    ,CASE
        WHEN Requisitante.Ramal IS NULL THEN Requisitante.Nome
        ELSE Requisitante.Nome + ' - (' + CONVERT(VARCHAR, Requisitante.Ramal) + ')'
      END AS NomeRequisitante
     ,CASE
        WHEN SetorSolicitante.Sigla IS NULL THEN SetorSolicitante.Nome
        ELSE SetorSolicitante.Nome + ' - (' + SetorSolicitante.Sigla + ')'
      END AS NomeSetor
     ,Requisitante.RequisitanteId
     ,SetorSolicitante.SetorSolicitanteId
     ,Motorista.Nome AS NomeMotorista
     ,'(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo
     ,Veiculo.UnidadeId
     ,ROW_NUMBER() OVER (ORDER BY Viagem.DataInicial, Viagem.HoraInicio) AS RowNum
     ,Veiculo.CombustivelId
     ,Viagem.DescricaoSolucaoOcorrencia
     ,Viagem.Origem
     ,Viagem.Destino
     ,Viagem.Minutos
     ,Viagem.NomeEvento
     ,Viagem.RamalRequisitante
     ,Viagem.ImagemOcorrencia
     ,Viagem.ItemManutencaoId
    FROM dbo.SetorSolicitante
    RIGHT OUTER JOIN #Viagem Viagem
      ON SetorSolicitante.SetorSolicitanteId = Viagem.SetorSolicitanteId
    LEFT OUTER JOIN dbo.Requisitante
      ON Requisitante.RequisitanteId = Viagem.RequisitanteId
    LEFT OUTER JOIN #Motorista Motorista
      ON Motorista.MotoristaId = Viagem.MotoristaId
    LEFT OUTER JOIN #Veiculo Veiculo
      ON Veiculo.VeiculoId = Viagem.VeiculoId
    INNER JOIN dbo.ModeloVeiculo
      ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
        INNER JOIN dbo.MarcaVeiculo
            ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId;
GO

/*
ROLLBACK SCRIPT (execute manualmente se necessario)

SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRAN;

    -- Recria colunas legadas
    IF COL_LENGTH('dbo.Viagem', 'StatusDocumento') IS NULL
        ALTER TABLE dbo.Viagem
            ADD StatusDocumento varchar(50) NULL
                CONSTRAINT DF_Viagem_StatusDocumento DEFAULT ('');

    IF COL_LENGTH('dbo.Viagem', 'StatusCartaoAbastecimento') IS NULL
        ALTER TABLE dbo.Viagem
            ADD StatusCartaoAbastecimento varchar(50) NULL
                CONSTRAINT DF_Viagem_StatusCartaoAbastecimento DEFAULT ('');

    IF COL_LENGTH('dbo.Viagem', 'StatusDocumentoFinal') IS NULL
        ALTER TABLE dbo.Viagem
            ADD StatusDocumentoFinal varchar(50) NULL
                CONSTRAINT DF_Viagem_StatusDocumentoFinal DEFAULT ('');

    IF COL_LENGTH('dbo.Viagem', 'StatusCartaoAbastecimentoFinal') IS NULL
        ALTER TABLE dbo.Viagem
            ADD StatusCartaoAbastecimentoFinal varchar(50) NULL
                CONSTRAINT DF_Viagem_StatusCartaoAbastecimentoFinal DEFAULT ('');

    -- Backfill a partir dos novos flags
    UPDATE dbo.Viagem
    SET StatusDocumento = CASE WHEN DocumentoEntregue = 1 THEN 'Entregue' ELSE '' END,
        StatusDocumentoFinal = CASE WHEN DocumentoDevolvido = 1 THEN 'Devolvido' ELSE '' END,
        StatusCartaoAbastecimento = CASE WHEN CartaoAbastecimentoEntregue = 1 THEN 'Entregue' ELSE '' END,
        StatusCartaoAbastecimentoFinal = CASE WHEN CartaoAbastecimentoDevolvido = 1 THEN 'Devolvido' ELSE '' END
    WHERE DocumentoEntregue IS NOT NULL
       OR DocumentoDevolvido IS NOT NULL
       OR CartaoAbastecimentoEntregue IS NOT NULL
       OR CartaoAbastecimentoDevolvido IS NOT NULL;

    -- Remove defaults dos novos campos
    DECLARE @sql_rb nvarchar(max);

    SELECT @sql_rb = N'ALTER TABLE dbo.Viagem DROP CONSTRAINT ' + QUOTENAME(dc.name)
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id
       AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.Viagem')
      AND c.name = 'DocumentoEntregue';
    IF @sql_rb IS NOT NULL EXEC sp_executesql @sql_rb;

    SELECT @sql_rb = N'ALTER TABLE dbo.Viagem DROP CONSTRAINT ' + QUOTENAME(dc.name)
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id
       AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.Viagem')
      AND c.name = 'DocumentoDevolvido';
    IF @sql_rb IS NOT NULL EXEC sp_executesql @sql_rb;

    SELECT @sql_rb = N'ALTER TABLE dbo.Viagem DROP CONSTRAINT ' + QUOTENAME(dc.name)
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id
       AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.Viagem')
      AND c.name = 'CartaoAbastecimentoEntregue';
    IF @sql_rb IS NOT NULL EXEC sp_executesql @sql_rb;

    SELECT @sql_rb = N'ALTER TABLE dbo.Viagem DROP CONSTRAINT ' + QUOTENAME(dc.name)
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id
       AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.Viagem')
      AND c.name = 'CartaoAbastecimentoDevolvido';
    IF @sql_rb IS NOT NULL EXEC sp_executesql @sql_rb;

    -- Remove novos campos
    IF COL_LENGTH('dbo.Viagem', 'DocumentoEntregue') IS NOT NULL
        ALTER TABLE dbo.Viagem DROP COLUMN DocumentoEntregue;
    IF COL_LENGTH('dbo.Viagem', 'DocumentoDevolvido') IS NOT NULL
        ALTER TABLE dbo.Viagem DROP COLUMN DocumentoDevolvido;
    IF COL_LENGTH('dbo.Viagem', 'CartaoAbastecimentoEntregue') IS NOT NULL
        ALTER TABLE dbo.Viagem DROP COLUMN CartaoAbastecimentoEntregue;
    IF COL_LENGTH('dbo.Viagem', 'CartaoAbastecimentoDevolvido') IS NOT NULL
        ALTER TABLE dbo.Viagem DROP COLUMN CartaoAbastecimentoDevolvido;

    COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    THROW;
END CATCH;
GO

-- Rollback da view/procedure para colunas legadas
CREATE OR ALTER VIEW dbo.ViewViagens
AS SELECT
    Viagem.ViagemId,
    Viagem.DataInicial,
    Viagem.DataFinal,
    Viagem.HoraInicio,
    Viagem.HoraFim,
    Viagem.Descricao,
    Viagem.Status,
    Viagem.KmInicial,
    Viagem.KmFinal,
    Viagem.CombustivelInicial,
    Viagem.CombustivelFinal,
    Viagem.MotoristaId,
    Viagem.VeiculoId,
    Viagem.ResumoOcorrencia,
    Viagem.DescricaoOcorrencia,
    Viagem.StatusOcorrencia,
    Viagem.StatusDocumento,
    Viagem.StatusCartaoAbastecimento,
    Viagem.StatusAgendamento,
    Viagem.NoFichaVistoria,
    Viagem.Finalidade,
    Veiculo.Placa,
    Motorista.CNH,
    Motorista.Foto,
    Motorista.Ponto,
    convert(nvarchar(36), Viagem.ViagemId) AS ViagemIDStr,
    CASE
        WHEN Requisitante.Ramal IS NULL THEN Requisitante.Nome
        ELSE Requisitante.Nome + ' - (' + CONVERT(VARCHAR, Requisitante.Ramal) + ')'
    END AS NomeRequisitante,
    CASE
        WHEN SetorSolicitante.Sigla IS NULL THEN SetorSolicitante.Nome
        ELSE SetorSolicitante.Nome + ' - (' + SetorSolicitante.Sigla + ')'
    END AS NomeSetor,
    Requisitante.RequisitanteId,
    SetorSolicitante.SetorSolicitanteId,
    Motorista.Nome AS NomeMotorista,
    '(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo,
    Veiculo.UnidadeId,
    ROW_NUMBER() OVER (ORDER BY Viagem.DataInicial, Viagem.HoraInicio) AS RowNum,
    Veiculo.CombustivelId,
    Viagem.DescricaoSolucaoOcorrencia,
    Viagem.FichaVistoria,
    Viagem.Origem,
    Viagem.Destino,
    Viagem.Minutos,
    Viagem.NomeEvento,
    Viagem.RamalRequisitante,
    Viagem.ImagemOcorrencia,
    Viagem.ItemManutencaoId,
    ROUND((Viagem.CustoCombustivel + Viagem.CustoMotorista + Viagem.CustoVeiculo + Viagem.CustoOperador + Viagem.CustoLavador), 2) AS CustoViagem,
    Viagem.EventoId,
    CorEvento = CASE
        WHEN Viagem.StatusAgendamento = 1 AND Viagem.Status <> 'Cancelada' AND Viagem.Finalidade = 'Evento' THEN '#E99B63'
        WHEN Viagem.StatusAgendamento = 1 AND Viagem.Status <> 'Cancelada' THEN '#29B3FF'
        WHEN Viagem.Status = 'Cancelada' THEN '#E34234'
        WHEN Viagem.Status = 'Aberta' AND ISNULL(Viagem.StatusAgendamento, 0) = 0 THEN '#FFD774'
        WHEN Viagem.Status = 'Realizada' AND ISNULL(Viagem.FoiAgendamento, 0) = 1 THEN '#52688F'
        WHEN Viagem.Status = 'Realizada' THEN '#75B390'
        WHEN Viagem.Finalidade = 'Evento' THEN '#E99B63'
        ELSE '#29B3FF'
    END,
    CorTexto = CASE
        WHEN (Viagem.StatusAgendamento = 1 AND Viagem.Status <> 'Cancelada')
          OR (Viagem.Status = 'Realizada')
          OR (Viagem.Finalidade = 'Evento')
          OR (Viagem.Status = 'Cancelada')
          THEN 'white'
        WHEN (Viagem.Status = 'Aberta' AND ISNULL(Viagem.StatusAgendamento, 0) = 0)
          THEN '#2B6670'
        ELSE 'white'
    END,
    DescricaoMontada =
        (
            ISNULL(Motorista.Nome, '(Motorista Nao Identificado)')
            + ' - (' + ISNULL(Veiculo.Placa, 'Sem Veiculo') + ')'
            + CASE
                WHEN Viagem.Descricao IS NOT NULL AND LEN(LTRIM(RTRIM(Viagem.Descricao))) > 0
                THEN ' - ' + Viagem.Descricao
                ELSE ''
              END
        ),
    DescricaoEvento =
        CASE
            WHEN Viagem.Finalidade = 'Evento' AND Viagem.NomeEvento IS NOT NULL AND Viagem.Status = 'Cancelada'
                THEN 'Evento CANCELADO: ' + Viagem.NomeEvento + ' / '
                    + ISNULL(Motorista.Nome, '(Motorista Nao Identificado)')
                    + ' - (' + ISNULL(Veiculo.Placa, 'Sem Veiculo') + ')'
                    + CASE WHEN Viagem.Descricao IS NOT NULL AND LEN(LTRIM(RTRIM(Viagem.Descricao))) > 0
                        THEN ' - ' + Viagem.Descricao
                        ELSE '' END
            WHEN Viagem.Finalidade = 'Evento' AND Viagem.NomeEvento IS NOT NULL
                THEN 'Evento: ' + Viagem.NomeEvento + ' / '
                    + ISNULL(Motorista.Nome, '(Motorista Nao Identificado)')
                    + ' - (' + ISNULL(Veiculo.Placa, 'Sem Veiculo') + ')'
                    + CASE WHEN Viagem.Descricao IS NOT NULL AND LEN(LTRIM(RTRIM(Viagem.Descricao))) > 0
                        THEN ' - ' + Viagem.Descricao
                        ELSE '' END
            ELSE NULL
        END,
    Titulo =
        CASE
            WHEN Viagem.Finalidade = 'Evento' AND Viagem.NomeEvento IS NOT NULL THEN 'Evento : ' + Viagem.NomeEvento
            ELSE Viagem.Finalidade
        END
FROM dbo.SetorSolicitante
RIGHT OUTER JOIN dbo.Viagem
    ON SetorSolicitante.SetorSolicitanteId = Viagem.SetorSolicitanteId
LEFT OUTER JOIN dbo.Requisitante
    ON Requisitante.RequisitanteId = Viagem.RequisitanteId
LEFT OUTER JOIN dbo.Motorista
    ON Motorista.MotoristaId = Viagem.MotoristaId
LEFT OUTER JOIN dbo.Veiculo
    ON Veiculo.VeiculoId = Viagem.VeiculoId
INNER JOIN dbo.ModeloVeiculo
    ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
INNER JOIN dbo.MarcaVeiculo
    ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId;
GO

CREATE OR ALTER PROCEDURE frotix.ViewViagens_SP
AS
SELECT [ViagemId]
      ,[DataInicial]
      ,[DataFinal]
      ,[HoraInicio]
      ,[HoraFim]
      ,[CombustivelInicial]
      ,[CombustivelFinal]
      ,[KmAtual]
      ,[KmInicial]
      ,[KmFinal]
      ,[Descricao]
      ,[RamalRequisitante]
      ,[Finalidade]
      ,[Status]
      ,[RequisitanteId]
      ,[SetorSolicitanteId]
      ,[VeiculoId]
      ,[MotoristaId]
      ,[UsuarioIdCriacao]
      ,[DataCriacao]
      ,[UsuarioIdFinalizacao]
      ,[DataFinalizacao]
      ,[ResumoOcorrencia]
      ,[DescricaoOcorrencia]
      ,[StatusOcorrencia]
      ,[StatusDocumento]
      ,[StatusCartaoAbastecimento]
      ,[StatusAgendamento]
      ,[Origem]
      ,[Destino]
      ,[DescricaoSemFormato]
      ,[CustoCombustivel]
      ,[CustoMotorista]
      ,[CustoVeiculo]
      ,[CustoOperador]
      ,[CustoLavador]
      ,[Minutos]
      ,[NomeEvento]
      ,[DescricaoSolucaoOcorrencia]
      ,[ImagemOcorrencia]
      ,[ItemManutencaoId]
      ,[EventoId]
      ,[NoFichaVistoria]
 INTO #Viagem FROM [Viagem] (NOLOCK);

CREATE NONCLUSTERED INDEX #IDX_Prueba ON #Viagem (SetorSolicitanteId);
CREATE NONCLUSTERED INDEX #IDX_VeiculoId ON #Viagem (VeiculoId);
CREATE NONCLUSTERED INDEX #IDX_StatusOcorrencia ON #Viagem (StatusOcorrencia);
CREATE NONCLUSTERED INDEX #IDX_Status ON #Viagem ([Status]);
CREATE NONCLUSTERED INDEX #IDX_MotoristaId ON #Viagem ([MotoristaId]);
CREATE NONCLUSTERED INDEX #IDX_RequisitanteID ON #Viagem (RequisitanteID);

SELECT [MotoristaId]
      ,[Nome]
      ,[Ponto]
      ,[DataNascimento]
      ,[CPF]
      ,[CNH]
      ,[CategoriaCNH]
      ,[DataVencimentoCNH]
      ,[Celular01]
      ,[Celular02]
      ,[DataIngresso]
      ,[OrigemIndicacao]
      ,[Status]
      ,[UsuarioIdAlteracao]
      ,[DataAlteracao]
      ,[UnidadeId]
      ,[ContratoId]
      ,[CondutorId]
      ,[TipoCondutor]
      ,[EfetivoFerista]
  INTO #Motorista FROM [Motorista];

CREATE NONCLUSTERED INDEX #IDX_Motorista ON #Motorista (MotoristaId);

SELECT [VeiculoId]
      ,[Placa]
      ,[Quilometragem]
      ,[Renavam]
      ,[PlacaVinculada]
      ,[AnoFabricacao]
      ,[AnoModelo]
      ,[Reserva]
      ,[VeiculoProprio]
      ,[Status]
      ,[DataAlteracao]
      ,[UsuarioIdAlteracao]
      ,[UnidadeId]
      ,[MarcaId]
      ,[ModeloId]
      ,[CombustivelId]
      ,[ContratoId]
      ,[AtaId]
      ,[PlacaBronzeId]
      ,[ItemVeiculoId]
      ,[Patrimonio]
      ,[ItemVeiculoAtaId]
      ,[Categoria]
      ,[DataIngresso]
  INTO #Veiculo FROM [Veiculo];

CREATE NONCLUSTERED INDEX #IDX_Veiculo ON #Veiculo (VeiculoId);

SELECT
 Viagem.ViagemId
 ,CONVERT(VARCHAR, Viagem.DataInicial, 103) AS DataInicial
 ,CONVERT(VARCHAR, Viagem.DataFinal, 103) AS DataFinal
 ,FORMAT(Viagem.HoraInicio, 'HH:mm') AS HoraInicio
 ,FORMAT(Viagem.HoraFim, 'HH:mm') AS HoraFim
 ,Viagem.Descricao
 ,Viagem.Status
 ,Viagem.KmInicial
 ,Viagem.KmFinal
 ,Viagem.CombustivelInicial
 ,Viagem.CombustivelFinal
 ,Viagem.MotoristaId
 ,Viagem.VeiculoId
 ,Viagem.ResumoOcorrencia
 ,Viagem.DescricaoOcorrencia
 ,Viagem.StatusOcorrencia
 ,Viagem.StatusDocumento
 ,Viagem.StatusCartaoAbastecimento
 ,Viagem.StatusAgendamento
 ,Viagem.NoFichaVistoria
 ,Viagem.Finalidade
 ,Veiculo.Placa
 ,Motorista.Ponto
,convert(nvarchar(36), Viagem.ViagemId) AS ViagemIDStr
,CASE
    WHEN Requisitante.Ramal IS NULL THEN Requisitante.Nome
    ELSE Requisitante.Nome + ' - (' + CONVERT(VARCHAR, Requisitante.Ramal) + ')'
  END AS NomeRequisitante
 ,CASE
    WHEN SetorSolicitante.Sigla IS NULL THEN SetorSolicitante.Nome
    ELSE SetorSolicitante.Nome + ' - (' + SetorSolicitante.Sigla + ')'
  END AS NomeSetor
 ,Requisitante.RequisitanteId
 ,SetorSolicitante.SetorSolicitanteId
 ,Motorista.Nome AS NomeMotorista
 ,'(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo
 ,Veiculo.UnidadeId
 ,ROW_NUMBER() OVER (ORDER BY Viagem.DataInicial, Viagem.HoraInicio) AS RowNum
 ,Veiculo.CombustivelId
 ,Viagem.DescricaoSolucaoOcorrencia
 ,Viagem.Origem
 ,Viagem.Destino
 ,Viagem.Minutos
 ,Viagem.NomeEvento
 ,Viagem.RamalRequisitante
 ,Viagem.ImagemOcorrencia
 ,Viagem.ItemManutencaoId
FROM dbo.SetorSolicitante
RIGHT OUTER JOIN #Viagem Viagem
  ON SetorSolicitante.SetorSolicitanteId = Viagem.SetorSolicitanteId
LEFT OUTER JOIN dbo.Requisitante
  ON Requisitante.RequisitanteId = Viagem.RequisitanteId
LEFT OUTER JOIN #Motorista Motorista
  ON Motorista.MotoristaId = Viagem.MotoristaId
LEFT OUTER JOIN #Veiculo Veiculo
  ON Veiculo.VeiculoId = Viagem.VeiculoId
INNER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
INNER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId;
GO
*/

-- ============================================================================
-- 2026-02-11 - Schema sync (models) - Tabelas e Views faltantes
-- ============================================================================

SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRAN;

    IF OBJECT_ID('dbo.TipoServico', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.TipoServico (
            TipoServicoId uniqueidentifier NOT NULL,
            NomeServico nvarchar(100) NOT NULL,
            Descricao nvarchar(500) NULL,
            Ativo bit NOT NULL DEFAULT (1),
            DataCriacao datetime NULL,
            DataAlteracao datetime NULL,
            UsuarioIdAlteracao nvarchar(450) NULL,
            CONSTRAINT PK_TipoServico PRIMARY KEY CLUSTERED (TipoServicoId)
        );
    END

    IF OBJECT_ID('dbo.Turno', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.Turno (
            TurnoId uniqueidentifier NOT NULL,
            NomeTurno nvarchar(50) NOT NULL,
            HoraInicio time NOT NULL,
            HoraFim time NOT NULL,
            Ativo bit NOT NULL DEFAULT (1),
            DataCriacao datetime NULL,
            DataAlteracao datetime NULL,
            UsuarioIdAlteracao nvarchar(450) NULL,
            CONSTRAINT PK_Turno PRIMARY KEY CLUSTERED (TurnoId)
        );
    END

    IF OBJECT_ID('dbo.VAssociado', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.VAssociado (
            AssociacaoId uniqueidentifier NOT NULL,
            MotoristaId uniqueidentifier NOT NULL,
            VeiculoId uniqueidentifier NULL,
            DataInicio date NOT NULL,
            DataFim date NULL,
            Ativo bit NOT NULL DEFAULT (1),
            Observacoes nvarchar(max) NULL,
            DataCriacao datetime NULL,
            DataAlteracao datetime NULL,
            UsuarioIdAlteracao nvarchar(450) NULL,
            CONSTRAINT PK_VAssociado PRIMARY KEY CLUSTERED (AssociacaoId)
        );
    END

    IF OBJECT_ID('dbo.EscalaDiaria', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.EscalaDiaria (
            EscalaDiaId uniqueidentifier NOT NULL,
            AssociacaoId uniqueidentifier NULL,
            TipoServicoId uniqueidentifier NOT NULL,
            TurnoId uniqueidentifier NOT NULL,
            DataEscala date NOT NULL,
            HoraInicio time NOT NULL,
            HoraFim time NOT NULL,
            HoraIntervaloInicio time NULL,
            HoraIntervaloFim time NULL,
            Lotacao nvarchar(100) NULL,
            NumeroSaidas int NOT NULL DEFAULT (0),
            StatusMotorista nvarchar(20) NOT NULL DEFAULT (N'Disponivel'),
            RequisitanteId uniqueidentifier NULL,
            Observacoes nvarchar(max) NULL,
            Ativo bit NOT NULL DEFAULT (1),
            DataCriacao datetime NULL,
            DataAlteracao datetime NULL,
            UsuarioIdAlteracao nvarchar(450) NULL,
            CONSTRAINT PK_EscalaDiaria PRIMARY KEY CLUSTERED (EscalaDiaId)
        );
    END

    IF OBJECT_ID('dbo.FolgaRecesso', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.FolgaRecesso (
            FolgaId uniqueidentifier NOT NULL,
            MotoristaId uniqueidentifier NOT NULL,
            DataInicio date NOT NULL,
            DataFim date NOT NULL,
            Tipo nvarchar(20) NOT NULL,
            Observacoes nvarchar(max) NULL,
            Ativo bit NOT NULL DEFAULT (1),
            DataCriacao datetime NULL,
            DataAlteracao datetime NULL,
            UsuarioIdAlteracao nvarchar(450) NULL,
            CONSTRAINT PK_FolgaRecesso PRIMARY KEY CLUSTERED (FolgaId)
        );
    END

    IF OBJECT_ID('dbo.Ferias', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.Ferias (
            FeriasId uniqueidentifier NOT NULL,
            MotoristaId uniqueidentifier NOT NULL,
            MotoristaSubId uniqueidentifier NULL,
            DataInicio date NOT NULL,
            DataFim date NOT NULL,
            Observacoes nvarchar(max) NULL,
            Ativo bit NOT NULL DEFAULT (1),
            DataCriacao datetime NULL,
            DataAlteracao datetime NULL,
            UsuarioIdAlteracao nvarchar(450) NULL,
            CONSTRAINT PK_Ferias PRIMARY KEY CLUSTERED (FeriasId)
        );
    END

    IF OBJECT_ID('dbo.CoberturaFolga', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.CoberturaFolga (
            CoberturaId uniqueidentifier NOT NULL,
            MotoristaFolgaId uniqueidentifier NOT NULL,
            MotoristaCoberturaId uniqueidentifier NOT NULL,
            DataInicio date NOT NULL,
            DataFim date NOT NULL,
            Motivo nvarchar(50) NULL,
            Observacoes nvarchar(max) NULL,
            StatusOriginal nvarchar(50) NULL,
            Ativo bit NOT NULL DEFAULT (1),
            DataCriacao datetime NULL,
            DataAlteracao datetime NULL,
            UsuarioIdAlteracao nvarchar(450) NULL,
            CONSTRAINT PK_CoberturaFolga PRIMARY KEY CLUSTERED (CoberturaId)
        );
    END

    IF OBJECT_ID('dbo.ObservacoesEscala', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.ObservacoesEscala (
            ObservacaoId uniqueidentifier NOT NULL,
            DataEscala date NOT NULL,
            Titulo nvarchar(200) NULL,
            Descricao nvarchar(max) NOT NULL,
            Prioridade nvarchar(20) NOT NULL DEFAULT (N'Normal'),
            ExibirDe date NOT NULL,
            ExibirAte date NOT NULL,
            Ativo bit NOT NULL DEFAULT (1),
            DataCriacao datetime NULL,
            DataAlteracao datetime NULL,
            UsuarioIdAlteracao nvarchar(450) NULL,
            CONSTRAINT PK_ObservacoesEscala PRIMARY KEY CLUSTERED (ObservacaoId)
        );
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.LogErros (
            LogErroId bigint IDENTITY(1, 1) NOT NULL,
            DataHora datetime2(3) NOT NULL DEFAULT (getdate()),
            Tipo nvarchar(50) NOT NULL,
            Origem nvarchar(20) NOT NULL,
            Nivel nvarchar(20) NULL,
            Categoria nvarchar(100) NULL,
            Mensagem nvarchar(max) NOT NULL,
            MensagemCurta AS (
                CASE
                    WHEN LEN(Mensagem) > 200 THEN LEFT(Mensagem, 200) + '...'
                    ELSE Mensagem
                END
            ) PERSISTED,
            Arquivo nvarchar(500) NULL,
            Metodo nvarchar(200) NULL,
            Linha int NULL,
            Coluna int NULL,
            ExceptionType nvarchar(200) NULL,
            ExceptionMessage nvarchar(max) NULL,
            StackTrace nvarchar(max) NULL,
            InnerException nvarchar(max) NULL,
            Url nvarchar(1000) NULL,
            HttpMethod nvarchar(10) NULL,
            StatusCode int NULL,
            UserAgent nvarchar(500) NULL,
            IpAddress nvarchar(45) NULL,
            Usuario nvarchar(100) NULL,
            SessionId nvarchar(100) NULL,
            DadosAdicionais nvarchar(max) NULL,
            Resolvido bit NOT NULL DEFAULT (0),
            DataResolucao datetime2(3) NULL,
            ResolvidoPor nvarchar(100) NULL,
            Observacoes nvarchar(max) NULL,
            HashErro AS (
                CONVERT(nvarchar(64),
                    HASHBYTES('SHA2_256',
                        CONCAT(
                            ISNULL(Tipo, ''), '|',
                            ISNULL(Arquivo, ''), '|',
                            ISNULL(CAST(Linha AS nvarchar(10)), '0'), '|',
                            LEFT(ISNULL(Mensagem, ''), 200)
                        )
                    ), 2)
            ) PERSISTED,
            CriadoEm datetime2(3) NOT NULL DEFAULT (getdate()),
            CONSTRAINT PK_LogErros PRIMARY KEY CLUSTERED (LogErroId DESC)
        );
    END

    IF OBJECT_ID('dbo.VAssociado', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_VAssociado_Motorista')
    BEGIN
        ALTER TABLE dbo.VAssociado
          ADD CONSTRAINT FK_VAssociado_Motorista FOREIGN KEY (MotoristaId) REFERENCES dbo.Motorista (MotoristaId);
    END

    IF OBJECT_ID('dbo.VAssociado', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_VAssociado_Veiculo')
    BEGIN
        ALTER TABLE dbo.VAssociado
          ADD CONSTRAINT FK_VAssociado_Veiculo FOREIGN KEY (VeiculoId) REFERENCES dbo.Veiculo (VeiculoId);
    END

    IF OBJECT_ID('dbo.EscalaDiaria', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_EscalaDiaria_Associacao')
    BEGIN
        ALTER TABLE dbo.EscalaDiaria
          ADD CONSTRAINT FK_EscalaDiaria_Associacao FOREIGN KEY (AssociacaoId) REFERENCES dbo.VAssociado (AssociacaoId);
    END

    IF OBJECT_ID('dbo.EscalaDiaria', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_EscalaDiaria_TipoServico')
    BEGIN
        ALTER TABLE dbo.EscalaDiaria
          ADD CONSTRAINT FK_EscalaDiaria_TipoServico FOREIGN KEY (TipoServicoId) REFERENCES dbo.TipoServico (TipoServicoId);
    END

    IF OBJECT_ID('dbo.EscalaDiaria', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_EscalaDiaria_Turno')
    BEGIN
        ALTER TABLE dbo.EscalaDiaria
          ADD CONSTRAINT FK_EscalaDiaria_Turno FOREIGN KEY (TurnoId) REFERENCES dbo.Turno (TurnoId);
    END

    IF OBJECT_ID('dbo.EscalaDiaria', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_EscalaDiaria_Requisitante')
    BEGIN
        ALTER TABLE dbo.EscalaDiaria
          ADD CONSTRAINT FK_EscalaDiaria_Requisitante FOREIGN KEY (RequisitanteId) REFERENCES dbo.Requisitante (RequisitanteId);
    END

    IF OBJECT_ID('dbo.FolgaRecesso', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_FolgaRecesso_Motorista')
    BEGIN
        ALTER TABLE dbo.FolgaRecesso
          ADD CONSTRAINT FK_FolgaRecesso_Motorista FOREIGN KEY (MotoristaId) REFERENCES dbo.Motorista (MotoristaId);
    END

    IF OBJECT_ID('dbo.Ferias', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Ferias_Motorista')
    BEGIN
        ALTER TABLE dbo.Ferias
          ADD CONSTRAINT FK_Ferias_Motorista FOREIGN KEY (MotoristaId) REFERENCES dbo.Motorista (MotoristaId);
    END

    IF OBJECT_ID('dbo.Ferias', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Ferias_MotoristaSub')
    BEGIN
        ALTER TABLE dbo.Ferias
          ADD CONSTRAINT FK_Ferias_MotoristaSub FOREIGN KEY (MotoristaSubId) REFERENCES dbo.Motorista (MotoristaId);
    END

    IF OBJECT_ID('dbo.CoberturaFolga', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_CoberturaFolga_MotoristaFolga')
    BEGIN
        ALTER TABLE dbo.CoberturaFolga
          ADD CONSTRAINT FK_CoberturaFolga_MotoristaFolga FOREIGN KEY (MotoristaFolgaId) REFERENCES dbo.Motorista (MotoristaId);
    END

    IF OBJECT_ID('dbo.CoberturaFolga', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_CoberturaFolga_MotoristaCobertura')
    BEGIN
        ALTER TABLE dbo.CoberturaFolga
          ADD CONSTRAINT FK_CoberturaFolga_MotoristaCobertura FOREIGN KEY (MotoristaCoberturaId) REFERENCES dbo.Motorista (MotoristaId);
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogErros_DataHora' AND object_id = OBJECT_ID('dbo.LogErros'))
    BEGIN
        CREATE INDEX IX_LogErros_DataHora
          ON dbo.LogErros (DataHora DESC)
          INCLUDE (Tipo, Origem, Nivel, MensagemCurta, Usuario);
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogErros_Tipo' AND object_id = OBJECT_ID('dbo.LogErros'))
    BEGIN
        CREATE INDEX IX_LogErros_Tipo
          ON dbo.LogErros (Tipo, DataHora DESC);
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogErros_Origem' AND object_id = OBJECT_ID('dbo.LogErros'))
    BEGIN
        CREATE INDEX IX_LogErros_Origem
          ON dbo.LogErros (Origem, DataHora DESC);
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogErros_Usuario' AND object_id = OBJECT_ID('dbo.LogErros'))
    BEGIN
        CREATE INDEX IX_LogErros_Usuario
          ON dbo.LogErros (Usuario, DataHora DESC)
          WHERE (Usuario IS NOT NULL);
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogErros_Url' AND object_id = OBJECT_ID('dbo.LogErros'))
    BEGIN
        CREATE INDEX IX_LogErros_Url
          ON dbo.LogErros (Url, Tipo)
          INCLUDE (DataHora)
          WHERE (Url IS NOT NULL);
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogErros_HashErro' AND object_id = OBJECT_ID('dbo.LogErros'))
    BEGIN
        CREATE INDEX IX_LogErros_HashErro
          ON dbo.LogErros (HashErro, DataHora DESC)
          INCLUDE (Mensagem, Arquivo, Linha);
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogErros_Resolvido' AND object_id = OBJECT_ID('dbo.LogErros'))
    BEGIN
        CREATE INDEX IX_LogErros_Resolvido
          ON dbo.LogErros (Resolvido, Tipo, DataHora DESC)
          WHERE (Resolvido = 0);
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LogErros_Dashboard' AND object_id = OBJECT_ID('dbo.LogErros'))
    BEGIN
        CREATE INDEX IX_LogErros_Dashboard
          ON dbo.LogErros (Tipo, Origem, DataHora DESC)
          INCLUDE (Usuario, Url);
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.stats WHERE name = 'STAT_LogErros_TipoOrigem' AND object_id = OBJECT_ID('dbo.LogErros'))
    BEGIN
        CREATE STATISTICS STAT_LogErros_TipoOrigem
          ON dbo.LogErros (Tipo, Origem);
    END

    IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.stats WHERE name = 'STAT_LogErros_DataHoraTipo' AND object_id = OBJECT_ID('dbo.LogErros'))
    BEGIN
        CREATE STATISTICS STAT_LogErros_DataHoraTipo
          ON dbo.LogErros (DataHora, Tipo);
    END

    COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    THROW;
END CATCH;
GO

-- Views (idempotente)
CREATE OR ALTER VIEW dbo.ViewVeiculosManutencaoReserva
AS SELECT TOP 1000
    v.VeiculoId,
    (v.Placa + ' - ' + ma.DescricaoMarca + '/' + m.DescricaoModelo) AS Descricao
FROM dbo.Veiculo v
INNER JOIN dbo.ModeloVeiculo m ON v.ModeloId = m.ModeloId
INNER JOIN dbo.MarcaVeiculo ma ON v.MarcaId = ma.MarcaId
WHERE v.Status = 1 AND v.Reserva = 1
ORDER BY Descricao;
GO

CREATE OR ALTER VIEW dbo.ViewVeiculosManutencao
AS SELECT TOP 1000
    v.VeiculoId,
    (v.Placa + ' - ' + ma.DescricaoMarca + '/' + m.DescricaoModelo) AS Descricao
FROM dbo.Veiculo v
INNER JOIN dbo.ModeloVeiculo m ON v.ModeloId = m.ModeloId
INNER JOIN dbo.MarcaVeiculo ma ON v.MarcaId = ma.MarcaId
WHERE v.Status = 1
ORDER BY Descricao;
GO

CREATE OR ALTER VIEW dbo.ViewMotoristasViagem
AS SELECT TOP 10000
    m.MotoristaId,
    m.Nome,
    m.TipoCondutor,
    m.Status,
    m.Foto,
    CAST(m.Nome + ' (' + m.TipoCondutor + ')' AS nvarchar(300)) AS MotoristaCondutor
FROM dbo.Motorista AS m
WHERE m.Status = 1
ORDER BY m.Nome;
GO

CREATE OR ALTER VIEW dbo.ViewOcorrenciasViagem
AS SELECT
    oc.OcorrenciaViagemId,
    oc.ViagemId,
    oc.VeiculoId,
    oc.MotoristaId,
    ISNULL(oc.Resumo, '') AS Resumo,
    ISNULL(oc.Descricao, '') AS Descricao,
    ISNULL(oc.ImagemOcorrencia, '') AS ImagemOcorrencia,
    ISNULL(oc.Status, 'Aberta') AS Status,
    oc.DataCriacao,
    oc.DataBaixa,
    ISNULL(oc.UsuarioCriacao, '') AS UsuarioCriacao,
    ISNULL(oc.UsuarioBaixa, '') AS UsuarioBaixa,
    oc.ItemManutencaoId,
    ISNULL(oc.Observacoes, '') AS Observacoes,
    vi.DataInicial,
    vi.DataFinal,
    vi.HoraInicio,
    vi.HoraFim,
    vi.NoFichaVistoria,
    ISNULL(vi.Origem, '') AS Origem,
    ISNULL(vi.Destino, '') AS Destino,
    ISNULL(vi.Finalidade, '') AS FinalidadeViagem,
    ISNULL(vi.Status, '') AS StatusViagem,
    ISNULL(ve.Placa, '') AS Placa,
    ISNULL(ma.DescricaoMarca, '') AS DescricaoMarca,
    ISNULL(mo.DescricaoModelo, '') AS DescricaoModelo,
    ISNULL(CONCAT(ve.Placa, ' - ', ma.DescricaoMarca, '/', mo.DescricaoModelo), '') AS VeiculoCompleto,
    ISNULL(CONCAT(ma.DescricaoMarca, '/', mo.DescricaoModelo), '') AS MarcaModelo,
    ISNULL(mt.Nome, '') AS NomeMotorista,
    CAST('' AS varchar(1)) AS FotoMotorista,
    DATEDIFF(DAY, oc.DataCriacao, GETDATE()) AS DiasEmAberto,
    CASE
        WHEN oc.Status = 'Baixada' THEN 'Resolvida'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 30 THEN 'Critica'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 15 THEN 'Alta'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 7 THEN 'Media'
        ELSE 'Normal'
    END AS Urgencia,
    CASE
        WHEN oc.Status = 'Baixada' THEN '#28a745'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 30 THEN '#dc3545'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 15 THEN '#ffc107'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 7 THEN '#17a2b8'
        ELSE '#6c757d'
    END AS CorUrgencia
FROM dbo.OcorrenciaViagem oc
LEFT JOIN dbo.Viagem vi ON oc.ViagemId = vi.ViagemId
LEFT JOIN dbo.Veiculo ve ON oc.VeiculoId = ve.VeiculoId
LEFT JOIN dbo.MarcaVeiculo ma ON ve.MarcaId = ma.MarcaId
LEFT JOIN dbo.ModeloVeiculo mo ON ve.ModeloId = mo.ModeloId
LEFT JOIN dbo.Motorista mt ON oc.MotoristaId = mt.MotoristaId;
GO

CREATE OR ALTER VIEW dbo.ViewOcorrenciasAbertasVeiculo
AS
SELECT
    oc.OcorrenciaViagemId,
    oc.ViagemId,
    oc.VeiculoId,
    oc.MotoristaId,
    oc.Resumo,
    oc.Descricao,
    oc.ImagemOcorrencia,
    oc.DataCriacao,
    oc.UsuarioCriacao,
    ve.Placa,
    ma.DescricaoMarca,
    mo.DescricaoModelo,
    CONCAT(ve.Placa, ' - ', ma.DescricaoMarca, '/', mo.DescricaoModelo) AS VeiculoCompleto,
    vi.DataInicial AS DataViagem,
    vi.NoFichaVistoria,
    mt.Nome AS NomeMotorista,
    DATEDIFF(DAY, oc.DataCriacao, GETDATE()) AS DiasEmAberto,
    CASE
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 30 THEN 'Critica'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 15 THEN 'Alta'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 7 THEN 'Media'
        ELSE 'Normal'
    END AS Urgencia,
    CASE
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 30 THEN '#dc3545'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 15 THEN '#ffc107'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 7 THEN '#17a2b8'
        ELSE '#6c757d'
    END AS CorUrgencia
FROM dbo.OcorrenciaViagem oc
INNER JOIN dbo.Viagem vi ON oc.ViagemId = vi.ViagemId
INNER JOIN dbo.Veiculo ve ON oc.VeiculoId = ve.VeiculoId
INNER JOIN dbo.MarcaVeiculo ma ON ve.MarcaId = ma.MarcaId
INNER JOIN dbo.ModeloVeiculo mo ON ve.ModeloId = mo.ModeloId
LEFT JOIN dbo.Motorista mt ON oc.MotoristaId = mt.MotoristaId
WHERE oc.Status = 'Aberta';
GO

CREATE OR ALTER VIEW dbo.ViewManutencao
AS SELECT
    ISNULL(v.Placa, '') + ' - ' + ISNULL(ma.DescricaoMarca, '') + '/' + ISNULL(m.DescricaoModelo, '') AS PlacaDescricao,
    ISNULL(v.Placa, '') + ' - (' + ISNULL(ma.DescricaoMarca, '') + '/' + ISNULL(m.DescricaoModelo, '') + ')' AS Veiculo,
    vc.ContratoId,
    b.ManutencaoId,
    b.NumOS,
    b.ResumoOS,
    CONVERT(char(10), b.DataSolicitacao, 103) AS DataSolicitacao,
    CONVERT(char(10), b.DataDisponibilidade, 103) AS DataDisponibilidade,
    CONVERT(char(10), b.DataRecolhimento, 103) AS DataRecolhimento,
    CONVERT(char(10), b.DataRecebimentoReserva, 103) AS DataRecebimentoReserva,
    CONVERT(char(10), b.DataDevolucaoReserva, 103) AS DataDevolucaoReserva,
    CONVERT(char(10), b.DataEntrega, 103) AS DataEntrega,
    CONVERT(char(10), b.DataDevolucao, 103) AS DataDevolucao,
    b.DataSolicitacao AS DataSolicitacaoRaw,
    b.DataDevolucao AS DataDevolucaoRaw,
    b.StatusOS,
    b.VeiculoId,
    b.ReservaEnviado,
    ma.DescricaoMarca + '/' + m.DescricaoModelo AS DescricaoVeiculo,
    u.Sigla AS Sigla,
    c.Descricao AS CombustivelDescricao,
    v.Placa AS Placa,
    CASE
        WHEN b.ReservaEnviado = 1 THEN 'Enviado'
        WHEN b.ReservaEnviado = 0 THEN 'Ausente'
        ELSE ''
    END AS Reserva,
    ivc.Descricao,
    ivc.Quantidade,
    ivc.ValorUnitario,
    d.DiasCalc AS DiasGlosa,
    CAST(d.DiasCalc * COALESCE(CAST(ivc.ValorUnitario AS decimal(18, 2)), 0) AS decimal(18, 2)) AS ValorGlosa,
    CASE
        WHEN b.DataDevolucao IS NULL THEN 0
        ELSE DATEDIFF(DAY, b.DataSolicitacao, b.DataDevolucao)
    END AS Dias,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN '' ELSE 'data-toggle=''modal'' data-target=''#modalManutencao''' END AS Habilitado,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'fa-regular fa-lock' ELSE 'far fa-flag-checkered' END AS Icon,
    ivc.NumItem,
    CASE
        WHEN b.VeiculoReservaId IS NULL THEN NULL
        ELSE ISNULL(v.Placa, '') + ' - (' + ISNULL(ma.DescricaoMarca, '') + '/' + ISNULL(m.DescricaoModelo, '') + ')'
    END AS CarroReserva,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'disabled' ELSE '' END AS HabilitadoEditar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END AS OpacityEditar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'Visualizar Manutencao' ELSE 'Edita a Ordem de Servico!' END AS OpacityTooltipEditarEditar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'disabled' ELSE '' END AS HabilitadoBaixar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN '' ELSE 'data-toggle=''modal'' data-target=''#modalManutencao''' END AS ModalBaixarAttrs,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END AS OpacityBaixar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'Desabilitado' ELSE 'Fecha a Ordem de Servico!' END AS Tooltip,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'disabled' ELSE '' END AS HabilitadoCancelar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END AS OpacityCancelar,
    CASE
        WHEN b.StatusOS = 'Cancelada' THEN 'Manutencao Cancelada'
        WHEN b.StatusOS = 'Fechada' THEN 'OS Fechada/Baixada'
        ELSE 'Cancelar Manutencao'
    END AS TooltipCancelar
FROM dbo.Manutencao AS b
LEFT JOIN dbo.Veiculo AS v ON v.VeiculoId = b.VeiculoId
LEFT JOIN dbo.VeiculoContrato AS vc ON vc.VeiculoId = v.VeiculoId
LEFT JOIN dbo.ModeloVeiculo AS m ON m.ModeloId = v.ModeloId
LEFT JOIN dbo.MarcaVeiculo AS ma ON ma.MarcaId = v.MarcaId
LEFT JOIN dbo.Combustivel AS c ON c.CombustivelId = v.CombustivelId
LEFT JOIN dbo.Unidade AS u ON u.UnidadeId = v.UnidadeId
LEFT JOIN dbo.ItemVeiculoContrato AS ivc ON ivc.ItemVeiculoId = v.ItemVeiculoId
CROSS APPLY (
  SELECT DiasCalc =
    CASE
      WHEN b.DataDevolucao IS NULL THEN 1
      WHEN b.DataEntrega IS NOT NULL THEN DATEDIFF(DAY, b.DataEntrega, b.DataDevolucao)
      ELSE DATEDIFF(DAY, b.DataSolicitacao, b.DataDevolucao)
    END
) AS d;
GO

CREATE OR ALTER VIEW dbo.ViewGlosa
AS SELECT
    ISNULL(v.Placa, '') + ' - ' + ISNULL(ma.DescricaoMarca, '') + '/' + ISNULL(m.DescricaoModelo, '') AS PlacaDescricao,
    vc.ContratoId,
    b.ManutencaoId,
    b.NumOS,
    b.ResumoOS,
    CONVERT(char(10), b.DataSolicitacao, 103) AS DataSolicitacao,
    CONVERT(char(10), b.DataDisponibilidade, 103) AS DataDisponibilidade,
    CONVERT(char(10), b.DataRecolhimento, 103) AS DataRecolhimento,
    CONVERT(char(10), b.DataRecebimentoReserva, 103) AS DataRecebimentoReserva,
    CONVERT(char(10), b.DataDevolucaoReserva, 103) AS DataDevolucaoReserva,
    CONVERT(char(10), b.DataEntrega, 103) AS DataEntrega,
    CONVERT(char(10), b.DataDevolucao, 103) AS DataDevolucao,
    b.DataSolicitacao AS DataSolicitacaoRaw,
    b.DataDisponibilidade AS DataDisponibilidadeRaw,
    b.DataDevolucao AS DataDevolucaoRaw,
    b.StatusOS,
    b.VeiculoId,
    ma.DescricaoMarca + '/' + m.DescricaoModelo AS DescricaoVeiculo,
    u.Sigla AS Sigla,
    c.Descricao AS CombustivelDescricao,
    v.Placa AS Placa,
    CASE
        WHEN b.ReservaEnviado = 1 THEN 'Enviado'
        WHEN b.ReservaEnviado = 0 THEN 'Ausente'
        ELSE ''
    END AS Reserva,
    ivc.Descricao,
    ivc.Quantidade,
    ivc.ValorUnitario,
    d.DiasCalc AS DiasGlosa,
    CAST(d.DiasCalc * COALESCE(CAST(ivc.ValorUnitario AS decimal(18, 2)), 0) AS decimal(18, 2)) AS ValorGlosa,
    CASE
        WHEN b.DataDevolucao IS NULL THEN 0
        ELSE DATEDIFF(DAY, b.DataSolicitacao, b.DataDevolucao)
    END AS Dias,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN '' ELSE 'data-toggle=''modal'' data-target=''#modalManutencao''' END AS Habilitado,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'fa-regular fa-lock' ELSE 'far fa-flag-checkered' END AS Icon,
    ivc.NumItem,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'disabled' ELSE '' END AS HabilitadoEditar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END AS OpacityEditar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'Visualizar Manutencao' ELSE 'Edita a Ordem de Servico!' END AS OpacityTooltipEditarEditar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'disabled' ELSE '' END AS HabilitadoBaixar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN '' ELSE 'data-toggle=''modal'' data-target=''#modalManutencao''' END AS ModalBaixarAttrs,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END AS OpacityBaixar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'Desabilitado' ELSE 'Fecha a Ordem de Servico!' END AS Tooltip,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'disabled' ELSE '' END AS HabilitadoCancelar,
    CASE WHEN b.StatusOS IN ('Fechada', 'Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END AS OpacityCancelar,
    CASE
        WHEN b.StatusOS = 'Cancelada' THEN 'Manutencao Cancelada'
        WHEN b.StatusOS = 'Fechada' THEN 'OS Fechada/Baixada'
        ELSE 'Cancelar Manutencao'
    END AS TooltipCancelar
FROM dbo.Manutencao AS b
LEFT JOIN dbo.Veiculo AS v ON v.VeiculoId = b.VeiculoId
LEFT JOIN dbo.VeiculoContrato AS vc ON vc.VeiculoId = v.VeiculoId
LEFT JOIN dbo.ModeloVeiculo AS m ON m.ModeloId = v.ModeloId
LEFT JOIN dbo.MarcaVeiculo AS ma ON ma.MarcaId = v.MarcaId
LEFT JOIN dbo.Combustivel AS c ON c.CombustivelId = v.CombustivelId
LEFT JOIN dbo.Unidade AS u ON u.UnidadeId = v.UnidadeId
LEFT JOIN dbo.ItemVeiculoContrato AS ivc ON ivc.ItemVeiculoId = v.ItemVeiculoId
CROSS APPLY (
  SELECT DiasCalc =
    CASE
      WHEN b.DataDevolucao IS NULL THEN 1
      WHEN b.DataEntrega IS NOT NULL THEN DATEDIFF(DAY, b.DataEntrega, b.DataDevolucao)
      ELSE DATEDIFF(DAY, b.DataSolicitacao, b.DataDevolucao)
    END
) AS d;
GO

CREATE OR ALTER VIEW dbo.ViewFluxoEconomildoData
AS SELECT
    ViewFluxoEconomildo.VeiculoId,
    ViewFluxoEconomildo.VIagemEconomildoId AS ViagemEconomildoId,
    ViewFluxoEconomildo.MotoristaId,
    ViewFluxoEconomildo.TipoCondutor,
    ViewFluxoEconomildo.Data,
    ViewFluxoEconomildo.MOB,
    ViewFluxoEconomildo.HoraInicio,
    ViewFluxoEconomildo.HoraFim,
    ViewFluxoEconomildo.QtdPassageiros,
    ViewFluxoEconomildo.NomeMotorista,
    ViewFluxoEconomildo.DescricaoVeiculo
FROM dbo.ViewFluxoEconomildo;
GO

CREATE OR ALTER VIEW dbo.ViewEscalasCompletas
AS SELECT
    ed.EscalaDiaId,
    ed.DataEscala,
    CONVERT(varchar(5), ed.HoraInicio, 108) AS HoraInicio,
    CONVERT(varchar(5), ed.HoraFim, 108) AS HoraFim,
    CONVERT(varchar(5), ed.HoraIntervaloInicio, 108) AS HoraIntervaloInicio,
    CONVERT(varchar(5), ed.HoraIntervaloFim, 108) AS HoraIntervaloFim,
    ed.NumeroSaidas,
    ed.StatusMotorista,
    ed.Lotacao,
    ed.Observacoes,
    m.MotoristaId,
    m.Nome AS NomeMotorista,
    m.Ponto,
    m.CPF,
    m.CNH,
    m.Celular01,
    m.Foto,
    v.VeiculoId,
    v.Placa,
    mv.DescricaoModelo AS Modelo,
    CASE
        WHEN v.VeiculoId IS NULL THEN NULL
        ELSE '(' + v.Placa + ') - ' + ma.DescricaoMarca + '/' + mv.DescricaoModelo
    END AS VeiculoDescricao,
    ts.TipoServicoId,
    ts.NomeServico,
    t.TurnoId,
    t.NomeTurno,
    r.RequisitanteId,
    r.Nome AS NomeRequisitante,
    cf.CoberturaId,
    cf.MotoristaCoberturaId,
    cf.MotoristaFolgaId,
    cf.DataInicio,
    cf.DataFim,
    cf.Motivo AS MotivoCobertura,
    mc.Nome AS NomeMotoristaCobertor,
    mf.Nome AS NomeMotoristaTitular
FROM dbo.EscalaDiaria ed
LEFT JOIN dbo.VAssociado va ON ed.AssociacaoId = va.AssociacaoId
LEFT JOIN dbo.Motorista m ON va.MotoristaId = m.MotoristaId
LEFT JOIN dbo.Veiculo v ON va.VeiculoId = v.VeiculoId
LEFT JOIN dbo.ModeloVeiculo mv ON v.ModeloId = mv.ModeloId
LEFT JOIN dbo.MarcaVeiculo ma ON mv.MarcaId = ma.MarcaId
LEFT JOIN dbo.TipoServico ts ON ed.TipoServicoId = ts.TipoServicoId
LEFT JOIN dbo.Turno t ON ed.TurnoId = t.TurnoId
LEFT JOIN dbo.Requisitante r ON ed.RequisitanteId = r.RequisitanteId
LEFT JOIN dbo.CoberturaFolga cf
  ON cf.MotoristaFolgaId = m.MotoristaId
 AND ed.DataEscala BETWEEN cf.DataInicio AND cf.DataFim
LEFT JOIN dbo.Motorista mc ON cf.MotoristaCoberturaId = mc.MotoristaId
LEFT JOIN dbo.Motorista mf ON cf.MotoristaFolgaId = mf.MotoristaId;
GO

CREATE OR ALTER VIEW dbo.ViewMotoristasVez
AS SELECT
    m.MotoristaId,
    m.Nome AS NomeMotorista,
    m.Ponto,
    m.Foto,
    ed.DataEscala,
    ed.NumeroSaidas,
    ed.StatusMotorista,
    ed.Lotacao,
    CASE
        WHEN v.VeiculoId IS NULL THEN NULL
        ELSE v.Placa + ' - ' + ma.DescricaoMarca + '/' + mv.DescricaoModelo
    END AS VeiculoDescricao,
    v.Placa,
    CONVERT(varchar(5), ed.HoraInicio, 108) AS HoraInicio,
    CONVERT(varchar(5), ed.HoraFim, 108) AS HoraFim
FROM dbo.EscalaDiaria ed
LEFT JOIN dbo.VAssociado va ON ed.AssociacaoId = va.AssociacaoId
LEFT JOIN dbo.Motorista m ON va.MotoristaId = m.MotoristaId
LEFT JOIN dbo.Veiculo v ON va.VeiculoId = v.VeiculoId
LEFT JOIN dbo.ModeloVeiculo mv ON v.ModeloId = mv.ModeloId
LEFT JOIN dbo.MarcaVeiculo ma ON mv.MarcaId = ma.MarcaId
WHERE ed.DataEscala = CAST(GETDATE() AS date);
GO

CREATE OR ALTER VIEW dbo.ViewStatusMotoristas
AS SELECT
    m.MotoristaId,
    m.Nome,
    m.Ponto,
    ed.StatusMotorista AS StatusAtual,
    ed.DataEscala,
    ed.NumeroSaidas,
    v.Placa,
    CASE
        WHEN v.VeiculoId IS NULL THEN NULL
        ELSE v.Placa + ' - ' + ma.DescricaoMarca + '/' + mv.DescricaoModelo
    END AS Veiculo
FROM dbo.EscalaDiaria ed
LEFT JOIN dbo.VAssociado va ON ed.AssociacaoId = va.AssociacaoId
LEFT JOIN dbo.Motorista m ON va.MotoristaId = m.MotoristaId
LEFT JOIN dbo.Veiculo v ON va.VeiculoId = v.VeiculoId
LEFT JOIN dbo.ModeloVeiculo mv ON v.ModeloId = mv.ModeloId
LEFT JOIN dbo.MarcaVeiculo ma ON mv.MarcaId = ma.MarcaId
WHERE ed.DataEscala = CAST(GETDATE() AS date);
GO
