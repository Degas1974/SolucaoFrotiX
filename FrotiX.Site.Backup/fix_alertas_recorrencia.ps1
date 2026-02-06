"# Script para corrigir a recorrencia de alertas no FrotiX

# 1. Adicionar métodos auxiliares de recorrencia
$controllerPath = "Controllers/AlertasFrotiXController.cs"
$controllerContent = Get-Content $controllerPath -Raw

# Encontrar a posição antes do método ObterTextoPorTipo
$searchPattern = 'private string ObterTextoPorTipo\\(TipoAlerta tipo\\)'
$match = [regex]::Match($controllerContent, $searchPattern)

if ($match.Success) {
    $position = $match.Index
    
    # Métodos a serem adicionados
    $newMethods = @'
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
                    Recorrente = "S", // S para recorrente
                    RecorrenciaAlertaId = recorrenciaAlertaId,
                    Intervalo = dto.TipoExibicao.ToString(), // Armazena o tipo como intervalo
                    DataFinalRecorrencia = dto.DataExpiracao,
                    Monday = dto.DiasSemana?.Contains("1") ?? false,
                    Tuesday = dto.DiasSemana?.Contains("2") ?? false,
                    Wednesday = dto.DiasSemana?.Contains("3") ?? false,
                    Thursday = dto.DiasSemana?.Contains("4") ?? false,
                    Friday = dto.DiasSemana?.Contains("5") ?? false,
                    Saturday = dto.DiasSemana?.Contains("6") ?? false,
                    Sunday = dto.DiasSemana?.Contains("0") ?? false,
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
    
    # Inserir os novos métodos
    $newContent = $controllerContent.Insert($position, $newMethods)
    
    # 2. Modificar o método Salvar para usar a nova lógica
    $salvarPattern = 'if \(dto\.TipoExibicao == 8 && !string\.IsNullOrWhiteSpace\(dto\.DatasSelecionadas\)\)'
    $salvarMatch = [regex]::Match($newContent, $salvarPattern)
    
    if ($salvarMatch.Success) {
        $salvarPosition = $salvarMatch.Index
        $endPattern = 'AlertasFrotiX alertaUnico;'
        $endMatch = [regex]::Match($newContent.Substring($salvarPosition), $endPattern)
        
        if ($endMatch.Success) {
            $sectionLength = $endMatch.Index
            $sectionToReplace = $newContent.Substring($salvarPosition, $sectionLength)
            
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
            
            $newContent = $newContent.Remove($salvarPosition, $sectionLength).Insert($salvarPosition, $newSection)
        }
    }
    
    # Salvar o arquivo modificado
    Set-Content -Path $controllerPath -Value $newContent -Encoding UTF8
    Write-Host "Controller modificado com sucesso!" -ForegroundColor Green
} else {
    Write-Host "Não foi possível encontrar o método ObterTextoPorTipo" -ForegroundColor Red
}"