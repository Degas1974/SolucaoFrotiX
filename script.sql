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
