"# Script para aplicar correções no AlertasFrotiXController

# 1. Ler o arquivo
$file = "Controllers/AlertasFrotiXController.cs"
$content = [System.IO.File]::ReadAllText($file)

# 2. Adicionar métodos auxiliares antes de ObterTextoPorTipo
$methodsToAdd = @'

        /// <summary>
        /// Calcula as datas recorrentes com base no tipo de exibição e parâmetros
        /// </summary>
        private List<DateTime> CalcularDatasRecorrentes(AlertaDto dto)
        {
            var datas = new List<DateTime>();
            
            if (!dto.DataExibicao.HasValue)
                return datas;

            var dataBase = dto.DataExibicao.Value;
            var dataFinal = dto.DataExpiracao ?? dataBase.AddYears(1); // Default: 1 ano

            // TipoExibicao: 4=Diário, 5=Semanal, 6=Quinzenal, 7=Mensal, 8=Dias Variados
            switch (dto.TipoExibicao)
            {
                case 4: // Diário
                    for (var data = dataBase; data <= dataFinal; data = data.AddDays(1))
                    {
                        datas.Add(data);
                    }
                    break;

                case 5: // Semanal
                    var diasSemana = ParseDiasSemana(dto.DiasSemana);
                    for (var data = dataBase; data <= dataFinal; data = data.AddDays(1))
                    {
                        if (diasSemana.Contains(data.DayOfWeek))
                        {
                            datas.Add(data);
                        }
                    }
                    break;

                case 6: // Quinzenal
                    for (var data = dataBase; data <= dataFinal; data = data.AddDays(14))
                    {
                        datas.Add(data);
                    }
                    break;

                case 7: // Mensal
                    if (dto.DiaMesRecorrencia.HasValue)
                    {
                        var diaMes = dto.DiaMesRecorrencia.Value;
                        for (var data = dataBase; data <= dataFinal; data = data.AddMonths(1))
                        {
                            var ultimoDiaMes = DateTime.DaysInMonth(data.Year, data.Month);
                            var diaValido = Math.Min(diaMes, ultimoDiaMes);
                            var dataRecorrente = new DateTime(data.Year, data.Month, diaValido);
                            if (dataRecorrente >= dataBase && dataRecorrente <= dataFinal)
                            {
                                datas.Add(dataRecorrente);
                            }
                        }
                    }
                    else
                    {
                        // Usa o mesmo dia do mês da data base
                        for (var data = dataBase; data <= dataFinal; data = data.AddMonths(1))
                        {
                            datas.Add(data);
                        }
                    }
                    break;

                case 8: // Dias Variados
                    if (!string.IsNullOrWhiteSpace(dto.DatasSelecionadas))
                    {
                        var datasStr = dto.DatasSelecionadas.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var dataStr in datasStr)
                        {
                            if (DateTime.TryParse(dataStr.Trim(), out DateTime dataExibicao))
                            {
                                if (dataExibicao >= dataBase && dataExibicao <= dataFinal)
                                {
                                    datas.Add(dataExibicao);
                                }
                            }
                        }
                    }
                    break;
            }

            return datas.Distinct().OrderBy(d => d).ToList();
        }

        /// <summary>
        /// Converte string de dias da semana para lista de DayOfWeek
        /// </summary>
        private List<DayOfWeek> ParseDiasSemana(string diasSemanaStr)
        {
            var dias = new List<DayOfWeek>();
            
            if (string.IsNullOrWhiteSpace(diasSemanaStr))
                return dias;

            var diasArray = diasSemanaStr.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var dia in diasArray)
            {
                if (int.TryParse(dia.Trim(), out int diaNum) && diaNum >= 0 && diaNum <= 6)
                {
                    dias.Add((DayOfWeek)diaNum);
                }
            }

            return dias;
        }

        /// <summary>
        /// Cria múltiplos alertas para datas recorrentes
        /// </summary>
        private async Task<List<Guid>> CriarAlertasRecorrentesAsync(AlertaDto dto, string usuarioId, List<DateTime> datasRecorrentes)
        {
            var alertasCriados = new List<Guid>();
            var recorrenciaAlertaId = Guid.NewGuid();

            foreach (var dataExibicao in datasRecorrentes)
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

            return alertasCriados;
        }
'@

# Encontrar a posição do método ObterTextoPorTipo
$pattern = '\s*private string ObterTextoPorTipo\\(TipoAlerta tipo\\)'
if ($content -match $pattern) {
    $position = $content.IndexOf($matches[0])
    
    # Inserir os métodos antes
    $newContent = $content.Insert($position, $methodsToAdd)
    
    # 3. Modificar a lógica do método Salvar
    # Encontrar a seção do tipo 8
    $tipo8Pattern = 'if \\(dto\\.TipoExibicao == 8 && !string\\.IsNullOrWhiteSpace\\(dto\\.DatasSelecionadas\\)\\)'
    if ($newContent -match $tipo8Pattern) {
        $tipo8Position = $newContent.IndexOf($matches[0])
        
        # Encontrar o fim dessa seção (até 'AlertasFrotiX alertaUnico;')
        $endPattern = 'AlertasFrotiX alertaUnico;'
        $endPosition = $newContent.IndexOf($endPattern, $tipo8Position)
        
        if ($endPosition -gt $tipo8Position) {
            $sectionLength = $endPosition - $tipo8Position
            $section = $newContent.Substring($tipo8Position, $sectionLength)
            
            # Nova seção com suporte completo
            $newSection = @'
                // Verifica se é um alerta recorrente (tipos 4-8)
                if (dto.TipoExibicao >= 4 && dto.TipoExibicao <= 8)
                {
                    // Calcula datas recorrentes
                    var datasRecorrentes = CalcularDatasRecorrentes(dto);
                    
                    if (datasRecorrentes.Count == 0)
                    {
                        return BadRequest(new
                        {
                            success = false,
                            message = "Nenhuma data válida encontrada para o alerta recorrente"
                        });
                    }

                    var alertasCriados = await CriarAlertasRecorrentesAsync(dto, usuarioId, datasRecorrentes);

                    return Ok(new
                    {
                        success = true,
                        message = $"{alertasCriados.Count} alertas recorrentes criados com sucesso",
                        alertasIds = alertasCriados,
                        quantidadeAlertas = alertasCriados.Count,
                        recorrenciaAlertaId = alertasCriados.Count > 0 ? "Gerado automaticamente" : null
                    });
                }
'@
            
            # Substituir a seção
            $newContent = $newContent.Remove($tipo8Position, $sectionLength).Insert($tipo8Position, $newSection)
            
            # Salvar o arquivo
            [System.IO.File]::WriteAllText($file, $newContent, [System.Text.Encoding]::UTF8)
            Write-Host "Correções aplicadas com sucesso!" -ForegroundColor Green
        } else {
            Write-Host "Não foi possível encontrar o fim da seção do tipo 8" -ForegroundColor Red
        }
    } else {
        Write-Host "Não foi encontrar a seção do tipo 8" -ForegroundColor Red
    }
} else {
    Write-Host "Não foi possível encontrar o método ObterTextoPorTipo" -ForegroundColor Red
}"