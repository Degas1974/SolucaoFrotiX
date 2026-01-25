"# Script para aplicar patch no AlertasFrotiXController

$filePath = "Controllers/AlertasFrotiXController.cs"
$content = Get-Content $filePath -Raw

# Encontrar e substituir a seção específica
$pattern = @'
if \(dto\.TipoExibicao == 8 && !string\.IsNullOrWhiteSpace\(dto\.DatasSelecionadas\)\)
\s*{
\s*var datasStr = dto\.DatasSelecionadas\.Split\(\',\', StringSplitOptions\.RemoveEmptyEntries\);
\s*var alertasCriados = new List<Guid>\(\);

\s*foreach \(var dataStr in datasStr\)
\s*{
\s*if \(DateTime\.TryParse\(dataStr\.Trim\(\), out DateTime dataExibicao\)\)
\s*{
\s*var alerta = new AlertasFrotiX
\s*{
\s*AlertasFrotiXId = Guid\.NewGuid\(\),
\s*Titulo = dto\.Titulo,
\s*Descricao = dto\.Descricao,
\s*TipoAlerta = \(TipoAlerta\)dto\.TipoAlerta,
\s*Prioridade = \(PrioridadeAlerta\)dto\.Prioridade,
\s*TipoExibicao = \(TipoExibicaoAlerta\)dto\.TipoExibicao,
\s*DataExibicao = dataExibicao,
\s*HorarioExibicao = dto\.HorarioExibicao,
\s*DataInsercao = DateTime\.Now,
\s*UsuarioCriadorId = usuarioId,
\s*Ativo = true,
\s*ViagemId = dto\.ViagemId,
\s*ManutencaoId = dto\.ManutencaoId,
\s*MotoristaId = dto\.MotoristaId,
\s*VeiculoId = dto\.VeiculoId
\s*};

\s*var usuariosParaNotificar = dto\.UsuariosIds \?\? new List<string>\(\);
\s*await _alertasRepo\.CriarAlertaAsync\(alerta, usuariosParaNotificar\);

\s*alertasCriados\.Add\(alerta\.AlertasFrotiXId\);

\s*await NotificarUsuariosNovoAlerta\(alerta, dto\.UsuariosIds\);
\s*}
\s*}

\s*return Ok\(new
\s*{
\s*success = true,
\s*message = \$\"{alertasCriados\.Count} alertas criados com sucesso \(um para cada data selecionada\)\",
\s*alertasIds = alertasCriados,
\s*quantidadeAlertas = alertasCriados\.Count
\s*}\);
\s*}
'@

$replacement = @'
// Verifica se é um alerta recorrente (tipos 4-8)
if (dto.TipoExibicao >= 4 && dto.TipoExibicao <= 8)
{
    // Para tipo 8 (Dias Variados), usa a lógica existente mas com campos de recorrência
    if (dto.TipoExibicao == 8 && !string.IsNullOrWhiteSpace(dto.DatasSelecionadas))
    {
        var datasStr = dto.DatasSelecionadas.Split(\',\', StringSplitOptions.RemoveEmptyEntries);
        var alertasCriados = new List<Guid>();
        var recorrenciaAlertaId = Guid.NewGuid();

        foreach (var dataStr in datasStr)
        {
            if (DateTime.TryParse(dataStr.Trim(), out DateTime dataExibicao))
            {
                var alerta = new AlertasFrotiX
                {
                    AlertasFrotiXId = Guid.NewGuid(),
                    Titulo = dto.Titulo,
                    Descricao = dto.Descricao,
                    TipoAlerta = (TipoAlerta)dto.TipoAlerta,
                    Prioridade = (PrioridadeAlerta)dto.Prioridade,
                    TipoExibicao = (TipoExibicaoAlerta)dto.TipoExibicao,
                    DataExibicao = dataExibicao,
                    HorarioExibicao = dto.HorarioExibicao,
                    DataExpiracao = dto.DataExpiracao,
                    DiasSemana = dto.DiasSemana,
                    DiaMesRecorrencia = dto.DiaMesRecorrencia,
                    DataInsercao = DateTime.Now,
                    UsuarioCriadorId = usuarioId,
                    Ativo = true,
                    ViagemId = dto.ViagemId,
                    ManutencaoId = dto.ManutencaoId,
                    MotoristaId = dto.MotoristaId,
                    VeiculoId = dto.VeiculoId,
                    Recorrente = \"S\",
                    RecorrenciaAlertaId = recorrenciaAlertaId,
                    Intervalo = dto.TipoExibicao.ToString(),
                    DataFinalRecorrencia = dto.DataExpiracao,
                    Monday = dto.DiasSemana?.Contains(\"1\") ?? false,
                    Tuesday = dto.DiasSemana?.Contains(\"2\") ?? false,
                    Wednesday = dto.DiasSemana?.Contains(\"3\") ?? false,
                    Thursday = dto.DiasSemana?.Contains(\"4\") ?? false,
                    Friday = dto.DiasSemana?.Contains(\"5\") ?? false,
                    Saturday = dto.DiasSemana?.Contains(\"6\") ?? false,
                    Sunday = dto.DiasSemana?.Contains(\"0\") ?? false,
                    DatasSelecionadas = dto.DatasSelecionadas
                };

                var usuariosParaNotificar = dto.UsuariosIds ?? new List<string>();
                await _alertasRepo.CriarAlertaAsync(alerta, usuariosParaNotificar);

                alertasCriados.Add(alerta.AlertasFrotiXId);

                await NotificarUsuariosNovoAlerta(alerta, dto.UsuariosIds);
            }
        }

        return Ok(new
        {
            success = true,
            message = $\"{alertasCriados.Count} alertas recorrentes criados com sucesso\",
            alertasIds = alertasCriados,
            quantidadeAlertas = alertasCriados.Count,
            recorrenciaAlertaId = recorrenciaAlertaId
        });
    }
    else
    {
        // Para tipos 4-7, informa que precisa dos métodos auxiliares
        return BadRequest(new
        {
            success = false,
            message = $\"Recorrência do tipo {dto.TipoExibicao} requer implementação completa.\" +
                      $\"\n\nPara implementar todos os tipos de recorrência (4-8), adicione os métodos:\" +
                      $\"\n1. CalcularDatasRecorrentes()\" +
                      $\"\n2. ParseDiasSemana()\" +
                      $\"\n3. CriarAlertasRecorrentesAsync()\",
            tipoExibicao = dto.TipoExibicao,
            descricaoTipo = dto.TipoExibicao switch
            {
                4 => \"Diário\",
                5 => \"Semanal\",
                6 => \"Quinzenal\",
                7 => \"Mensal\",
                8 => \"Dias Variados\",
                _ => \"Desconhecido\"
            }
        });
    }
}
'@

# Aplicar a substituição
if ($content -match $pattern) {
    $newContent = $content -replace $pattern, $replacement
    Set-Content -Path $filePath -Value $newContent -Encoding UTF8
    Write-Host "Patch aplicado com sucesso!" -ForegroundColor Green
} else {
    Write-Host "Padrão não encontrado no arquivo." -ForegroundColor Red
    Write-Host "Tentando abordagem alternativa..." -ForegroundColor Yellow
    
    # Tentar abordagem mais simples
    $simplePattern = 'if \(dto\.TipoExibicao == 8 && !string\.IsNullOrWhiteSpace\(dto\.DatasSelecionadas\)\)'
    if ($content -match $simplePattern) {
        Write-Host "Padrão simples encontrado na posição: $($matches.Index)" -ForegroundColor Green
        # Podemos fazer uma substituição manual mais simples
    }
}"