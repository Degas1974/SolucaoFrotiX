# Controllers/AlertasFrotiXController.cs

**Mudanca:** GRANDE | **+563** linhas | **-441** linhas

---

```diff
--- JANEIRO: Controllers/AlertasFrotiXController.cs
+++ ATUAL: Controllers/AlertasFrotiXController.cs
@@ -1,5 +1,4 @@
 using FrotiX.Hubs;
-
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc;
@@ -9,8 +8,6 @@
 using System.Linq;
 using System.Security.Claims;
 using System.Threading.Tasks;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
@@ -22,24 +19,25 @@
         private readonly IUnitOfWork _unitOfWork;
         private readonly IAlertasFrotiXRepository _alertasRepo;
         private readonly IHubContext<AlertasHub> _hubContext;
-        private readonly ILogService _log;
 
         public AlertasFrotiXController(
-            IUnitOfWork unitOfWork,
-            IAlertasFrotiXRepository alertasRepo,
-            IHubContext<AlertasHub> hubContext,
-            ILogService logService)
+            IUnitOfWork unitOfWork ,
+            IAlertasFrotiXRepository alertasRepo ,
+            IHubContext<AlertasHub> hubContext)
         {
             try
             {
                 _unitOfWork = unitOfWork;
                 _alertasRepo = alertasRepo;
                 _hubContext = hubContext;
-                _log = logService;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "AlertasFrotiXController", ex);
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AlertasFrotiXController.cs" ,
+                    "AlertasFrotiXController" ,
+                    error
+                );
             }
         }
 
@@ -50,7 +48,7 @@
             {
 
                 var alerta = await _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync(
-                    a => a.AlertasFrotiXId == id,
+                    a => a.AlertasFrotiXId == id ,
                     includeProperties: "AlertasUsuarios,Viagem,Manutencao,Veiculo,Motorista"
                 );
 
@@ -58,15 +56,15 @@
                 {
                     return NotFound(new
                     {
-                        success = false,
+                        success = false ,
                         message = "Alerta não encontrado"
                     });
                 }
 
                 var debugInfo = new
                 {
-                    alertasUsuariosCount = alerta.AlertasUsuarios?.Count ?? 0,
-                    alertasUsuariosIsNull = alerta.AlertasUsuarios == null,
+                    alertasUsuariosCount = alerta.AlertasUsuarios?.Count ?? 0 ,
+                    alertasUsuariosIsNull = alerta.AlertasUsuarios == null ,
                     totalLidosNoBanco = alerta.AlertasUsuarios?.Count(au => au.Lido) ?? 0
                 };
 
@@ -80,14 +78,14 @@
 
                     usuariosDetalhes.Add(new
                     {
-                        usuarioId = au.UsuarioId,
-                        nomeUsuario = usuario?.UserName ?? "Usuário removido",
-                        email = usuario?.Email,
-                        lido = au.Lido,
-                        dataLeitura = au.DataLeitura,
-                        dataNotificacao = au.DataNotificacao,
-                        notificado = au.Notificado,
-                        apagado = au.Apagado,
+                        usuarioId = au.UsuarioId ,
+                        nomeUsuario = usuario?.UserName ?? "Usuário removido" ,
+                        email = usuario?.Email ,
+                        lido = au.Lido ,
+                        dataLeitura = au.DataLeitura ,
+                        dataNotificacao = au.DataNotificacao ,
+                        notificado = au.Notificado ,
+                        apagado = au.Apagado ,
                         dataApagado = au.DataApagado
                     });
                 }
@@ -99,7 +97,7 @@
                 var usuariosNaoLeram = alerta.AlertasUsuarios.Count(au => au.Notificado && !au.Lido && !au.Apagado);
                 var usuariosApagaram = alerta.AlertasUsuarios.Count(au => au.Apagado);
                 var percentualLeitura = totalNotificados > 0
-                    ? Math.Round((double)usuariosLeram / totalNotificados * 100, 1)
+                    ? Math.Round((double)usuariosLeram / totalNotificados * 100 , 1)
                     : 0;
 
                 var dataInicio = alerta.DataExibicao ?? alerta.DataInsercao;
@@ -147,6 +145,7 @@
 
                     if (criador != null)
                     {
+
                         if (!string.IsNullOrWhiteSpace(criador.NomeCompleto))
                         {
                             nomeCriador = criador.NomeCompleto;
@@ -173,78 +172,78 @@
 
                 return Ok(new
                 {
-                    success = true,
-                    debug = debugInfo,
+                    success = true ,
+                    debug = debugInfo ,
                     data = new
                     {
-                        alertaId = alerta.AlertasFrotiXId,
-                        titulo = alerta.Titulo,
-                        descricao = alerta.Descricao,
-                        tipoAlerta = tipoInfo.Nome,
-                        tipo = tipoInfo.Nome,
-                        prioridade = prioridadeInfo.Nome,
-                        iconeCss = tipoInfo.Icone,
-                        corBadge = tipoInfo.Cor,
-                        dataCriacao = alerta.DataInsercao,
-                        dataInsercao = alerta.DataInsercao,
-                        dataExibicao = alerta.DataExibicao,
-                        dataExpiracao = alerta.DataExpiracao,
-                        ativo = alerta.Ativo,
-                        expirado = expirado,
-                        tempoNoAr = tempoNoArFormatado,
-                        nomeCriador = nomeCriador,
-                        usuarioCriadorId = alerta.UsuarioCriadorId,
-                        totalDestinatarios = totalDestinatarios,
-                        totalNotificados = totalNotificados,
-                        aguardandoNotificacao = aguardandoNotificacao,
-                        leram = usuariosLeram,
-                        naoLeram = usuariosNaoLeram,
-                        apagaram = usuariosApagaram,
-                        percentualLeitura = percentualLeitura,
-                        usuarios = usuariosDetalhes,
-                        viagemId = alerta.ViagemId,
-                        manutencaoId = alerta.ManutencaoId,
-                        motoristaId = alerta.MotoristaId,
+                        alertaId = alerta.AlertasFrotiXId ,
+                        titulo = alerta.Titulo ,
+                        descricao = alerta.Descricao ,
+                        tipoAlerta = tipoInfo.Nome ,
+                        tipo = tipoInfo.Nome ,
+                        prioridade = prioridadeInfo.Nome ,
+                        iconeCss = tipoInfo.Icone ,
+                        corBadge = tipoInfo.Cor ,
+                        dataCriacao = alerta.DataInsercao ,
+                        dataInsercao = alerta.DataInsercao ,
+                        dataExibicao = alerta.DataExibicao ,
+                        dataExpiracao = alerta.DataExpiracao ,
+                        ativo = alerta.Ativo ,
+                        expirado = expirado ,
+                        tempoNoAr = tempoNoArFormatado ,
+                        nomeCriador = nomeCriador ,
+                        usuarioCriadorId = alerta.UsuarioCriadorId ,
+                        totalDestinatarios = totalDestinatarios ,
+                        totalNotificados = totalNotificados ,
+                        aguardandoNotificacao = aguardandoNotificacao ,
+                        leram = usuariosLeram ,
+                        naoLeram = usuariosNaoLeram ,
+                        apagaram = usuariosApagaram ,
+                        percentualLeitura = percentualLeitura ,
+                        usuarios = usuariosDetalhes ,
+                        viagemId = alerta.ViagemId ,
+                        manutencaoId = alerta.ManutencaoId ,
+                        motoristaId = alerta.MotoristaId ,
                         veiculoId = alerta.VeiculoId
                     }
                 });
             }
-            catch (Exception ex)
-            {
-
-                _log.Error($"[AlertasFrotiXController] Erro em GetDetalhesAlerta: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetDetalhesAlerta", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    mensagem = "Erro ao obter detalhes do alerta",
-                    erro = ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "AlertasFrotiXController.cs" ,
+                    "GetDetalhesAlerta" ,
+                    error
+                );
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    message = "Erro ao buscar detalhes do alerta" ,
+                    erro = error.Message
                 });
             }
         }
 
         private (string Nome, string Icone, string Cor) ObterInfoTipo(TipoAlerta tipo)
         {
-
             return tipo switch
             {
                 TipoAlerta.Agendamento => ("Agendamento", "fa-duotone fa-calendar-check", "#0ea5e9"),
-                TipoAlerta.Manutencao => ("Manutencao", "fa-duotone fa-wrench", "#f59e0b"),
+                TipoAlerta.Manutencao => ("Manutenção", "fa-duotone fa-wrench", "#f59e0b"),
                 TipoAlerta.Motorista => ("Motorista", "fa-duotone fa-user-tie", "#14b8a6"),
-                TipoAlerta.Veiculo => ("Veiculo", "fa-duotone fa-car", "#7c3aed"),
-                TipoAlerta.Anuncio => ("Anuncio", "fa-duotone fa-bullhorn", "#dc2626"),
-                TipoAlerta.Aniversario => ("Aniversario", "fa-duotone fa-cake-candles", "#ec4899"),
+                TipoAlerta.Veiculo => ("Veículo", "fa-duotone fa-car", "#7c3aed"),
+                TipoAlerta.Anuncio => ("Anúncio", "fa-duotone fa-bullhorn", "#dc2626"),
+                TipoAlerta.Diversos => ("Diversos", "fa-duotone fa-circle-info", "#6b7280"),
                 _ => ("Geral", "fa-duotone fa-bell", "#6b7280")
             };
         }
 
         private (string Nome, string Cor) ObterInfoPrioridade(PrioridadeAlerta prioridade)
         {
-
             return prioridade switch
             {
                 PrioridadeAlerta.Baixa => ("Baixa", "#0ea5e9"),
-                PrioridadeAlerta.Media => ("Media", "#f59e0b"),
+                PrioridadeAlerta.Media => ("Média", "#f59e0b"),
                 PrioridadeAlerta.Alta => ("Alta", "#dc2626"),
                 _ => ("Normal", "#6b7280")
             };
@@ -252,26 +251,28 @@
 
         private (string Nome, string Icone, string Cor) ObterInfoTipo(int tipo)
         {
+
             return tipo switch
             {
                 1 => ("Agendamento", "fa-duotone fa-calendar-check", "#0ea5e9"),
-                2 => ("Manutencao", "fa-duotone fa-wrench", "#f59e0b"),
+                2 => ("Manutenção", "fa-duotone fa-wrench", "#f59e0b"),
                 3 => ("Motorista", "fa-duotone fa-user-tie", "#14b8a6"),
-                4 => ("Veiculo", "fa-duotone fa-car", "#7c3aed"),
-                5 => ("Anuncio", "fa-duotone fa-bullhorn", "#dc2626"),
-                6 => ("Aniversario", "fa-duotone fa-cake-candles", "#ec4899"),
+                4 => ("Veículo", "fa-duotone fa-car", "#7c3aed"),
+                5 => ("Anúncio", "fa-duotone fa-bullhorn", "#dc2626"),
+                6 => ("Diversos", "fa-duotone fa-circle-info", "#6b7280"),
                 _ => ("Geral", "fa-duotone fa-bell", "#6b7280")
             };
         }
 
         private (string Nome, string Cor) ObterInfoPrioridade(int prioridade)
         {
+
             return prioridade switch
             {
                 1 => ("Baixa", "#0ea5e9"),
-                2 => ("Media", "#f59e0b"),
+                2 => ("Média", "#f59e0b"),
                 3 => ("Alta", "#dc2626"),
-                4 => ("Critica", "#991b1b"),
+                4 => ("Crítica", "#991b1b"),
                 _ => ("Normal", "#6b7280")
             };
         }
@@ -314,7 +315,6 @@
 
                 if (alertasParaNotificar.Any())
                 {
-
                     foreach (var alerta in alertasParaNotificar)
                     {
                         var alertaUsuario = alerta.AlertasUsuarios
@@ -331,32 +331,30 @@
 
                 var resultado = alertasDoUsuario.Select(a => new
                 {
-                    alertaId = a.AlertasFrotiXId,
-                    titulo = a.Titulo,
-                    descricao = a.Descricao,
-                    mensagem = a.Descricao,
-                    tipo = (int)a.TipoAlerta,
-                    prioridade = (int)a.Prioridade,
-                    dataInsercao = a.DataInsercao,
-                    usuarioCriadorId = a.UsuarioCriadorId,
-                    iconeCss = Alerta.GetIconePrioridade(a.Prioridade),
-                    corBadge = Alerta.GetCorHexPrioridade(a.Prioridade),
-                    textoBadge = a.Prioridade.ToString(),
+                    alertaId = a.AlertasFrotiXId ,
+                    titulo = a.Titulo ,
+                    descricao = a.Descricao ,
+                    mensagem = a.Descricao ,
+                    tipo = (int)a.TipoAlerta ,
+                    prioridade = (int)a.Prioridade ,
+                    dataInsercao = a.DataInsercao ,
+                    usuarioCriadorId = a.UsuarioCriadorId ,
+                    iconeCss = Alerta.GetIconePrioridade(a.Prioridade) ,
+                    corBadge = Alerta.GetCorHexPrioridade(a.Prioridade) ,
+                    textoBadge = a.Prioridade.ToString() ,
                     severidade = a.Prioridade.ToString()
                 }).ToList();
 
                 return Ok(resultado);
             }
-            catch (Exception ex)
-            {
-
-                _log.Error($"[AlertasFrotiXController] Erro em GetAlertasAtivos: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetAlertasAtivos", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    mensagem = "Erro ao obter alertas ativos",
-                    erro = ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetAlertasAtivos" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    message = "Erro ao buscar alertas ativos" ,
+                    erro = error.Message
                 });
             }
         }
@@ -366,24 +364,28 @@
         {
             try
             {
-
                 var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                 ?? User.FindFirst("sub")?.Value
                                 ?? User.Identity?.Name;
 
                 if (string.IsNullOrEmpty(usuarioId))
                 {
-                    return Ok(new { quantidade = 0 });
+                    return Ok(new
+                    {
+                        quantidade = 0
+                    });
                 }
 
                 var quantidade = await _alertasRepo.GetQuantidadeAlertasNaoLidosAsync(usuarioId);
                 return Ok(new { quantidade });
             }
-            catch (Exception ex)
-            {
-
-                _log.Error($"[AlertasFrotiXController] Erro em GetQuantidadeNaoLidos: {ex.Message}", ex);
-                return Ok(new { quantidade = 0 });
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetQuantidadeNaoLidos" , error);
+                return Ok(new
+                {
+                    quantidade = 0
+                });
             }
         }
 
@@ -398,9 +400,16 @@
                                 ?? User.FindFirstValue(ClaimTypes.Name)
                                 ?? User.Identity?.Name;
 
+                Console.WriteLine($"🔍 AlertaId: {alertaId}");
+                Console.WriteLine($"🔍 UsuarioId: {usuarioId ?? "NULL"}");
+
                 if (string.IsNullOrEmpty(usuarioId))
                 {
-                    return Unauthorized(new { success = false, message = "Usuário não autenticado" });
+                    return Unauthorized(new
+                    {
+                        success = false ,
+                        message = "Usuário não autenticado"
+                    });
                 }
 
                 var alertaUsuario = await _unitOfWork.AlertasUsuario.GetFirstOrDefaultAsync(
@@ -409,33 +418,66 @@
 
                 if (alertaUsuario == null)
                 {
+
+                    Console.WriteLine($"❌ AlertaUsuario NÃO ENCONTRADO!");
+                    Console.WriteLine($" Buscando por AlertasFrotiXId={alertaId} e UsuarioId={usuarioId}");
+
+                    var existeAlerta = await _unitOfWork.AlertasUsuario.GetFirstOrDefaultAsync(
+                        au => au.AlertasFrotiXId == alertaId
+                    );
+
+                    if (existeAlerta != null)
+                    {
+                        Console.WriteLine($"⚠️ Alerta existe, mas não para este usuário!");
+                        Console.WriteLine($"⚠️ UsuarioId no banco: {existeAlerta.UsuarioId}");
+                    }
+                    else
+                    {
+                        Console.WriteLine($"⚠️ Alerta não existe no sistema!");
+                    }
+
                     return NotFound(new
                     {
-                        success = false,
-                        message = "Alerta não encontrado para este usuário",
-                        alertaId = alertaId,
+                        success = false ,
+                        message = "Alerta não encontrado para este usuário" ,
+                        alertaId = alertaId ,
                         usuarioId = usuarioId
                     });
                 }
 
+                Console.WriteLine($"✅ AlertaUsuario ENCONTRADO!");
+                Console.WriteLine($"✅ Lido antes: {alertaUsuario.Lido}");
+
                 alertaUsuario.Lido = true;
                 alertaUsuario.DataLeitura = DateTime.Now;
 
                 _unitOfWork.AlertasUsuario.Update(alertaUsuario);
+
+                Console.WriteLine($"✅ Chamando SaveAsync...");
                 await _unitOfWork.SaveAsync();
-
-                return Ok(new { success = true, message = "Alerta marcado como lido" });
-            }
-            catch (Exception ex)
-            {
-
-                _log.Error($"[AlertasFrotiXController] Erro em MarcarComoLido: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "MarcarComoLido", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    mensagem = "Erro ao marcar alerta como lido",
-                    erro = ex.Message
+                Console.WriteLine($"✅ SaveAsync concluído!");
+
+                return Ok(new
+                {
+                    success = true ,
+                    message = "Alerta marcado como lido"
+                });
+            }
+            catch (Exception error)
+            {
+                Console.WriteLine($"❌ ERRO: {error.Message}");
+                Console.WriteLine($"❌ Stack: {error.StackTrace}");
+
+                Alerta.TratamentoErroComLinha(
+                    "AlertasFrotiXController.cs" ,
+                    "MarcarComoLido" ,
+                    error
+                );
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    message = error.Message ,
+                    innerException = error.InnerException?.Message
                 });
             }
         }
@@ -454,7 +496,7 @@
                 {
                     return Unauthorized(new
                     {
-                        success = false,
+                        success = false ,
                         message = "Usuário não identificado"
                     });
                 }
@@ -463,7 +505,7 @@
                 {
                     return BadRequest(new
                     {
-                        success = false,
+                        success = false ,
                         message = "O título é obrigatório"
                     });
                 }
@@ -472,76 +514,47 @@
                 {
                     return BadRequest(new
                     {
-                        success = false,
+                        success = false ,
                         message = "A descrição é obrigatória"
                     });
                 }
 
                 if (dto.TipoExibicao >= 4 && dto.TipoExibicao <= 8)
                 {
-
-                    var datasRecorrentes = CalcularDatasRecorrentes(dto);
-
-                    if (datasRecorrentes.Count == 0)
+                    var datasRecorrencia = GerarDatasRecorrencia(dto);
+
+                    if (datasRecorrencia == null || datasRecorrencia.Count == 0)
                     {
                         return BadRequest(new
                         {
-                            success = false,
-                            message = "Nenhuma data válida encontrada para o alerta recorrente"
+                            success = false ,
+                            message = "Não foi possível gerar datas para a recorrência informada"
                         });
                     }
 
                     var alertasCriados = new List<Guid>();
-                    var recorrenciaAlertaId = Guid.NewGuid();
-
-                    foreach (var dataExibicao in datasRecorrentes)
-                    {
-                        var alerta = new AlertasFrotiX
-                        {
-                            AlertasFrotiXId = Guid.NewGuid(),
-                            Titulo = dto.Titulo,
-                            Descricao = dto.Descricao,
-                            TipoAlerta = (TipoAlerta)dto.TipoAlerta,
-                            Prioridade = (PrioridadeAlerta)dto.Prioridade,
-                            TipoExibicao = (TipoExibicaoAlerta)dto.TipoExibicao,
-                            DataExibicao = dataExibicao,
-                            HorarioExibicao = dto.HorarioExibicao,
-                            DataExpiracao = dto.DataExpiracao,
-                            DiasSemana = dto.DiasSemana,
-                            DiaMesRecorrencia = dto.DiaMesRecorrencia,
-                            DataInsercao = DateTime.Now,
-                            UsuarioCriadorId = usuarioId,
-                            Ativo = true,
-                            ViagemId = dto.ViagemId,
-                            ManutencaoId = dto.ManutencaoId,
-                            MotoristaId = dto.MotoristaId,
-                            VeiculoId = dto.VeiculoId,
-                            RecorrenciaAlertaId = recorrenciaAlertaId,
-                            Monday = dto.DiasSemana?.Contains("1") ?? false,
-                            Tuesday = dto.DiasSemana?.Contains("2") ?? false,
-                            Wednesday = dto.DiasSemana?.Contains("3") ?? false,
-                            Thursday = dto.DiasSemana?.Contains("4") ?? false,
-                            Friday = dto.DiasSemana?.Contains("5") ?? false,
-                            Saturday = dto.DiasSemana?.Contains("6") ?? false,
-                            Sunday = dto.DiasSemana?.Contains("0") ?? false,
-                            DatasSelecionadas = dto.DatasSelecionadas
-                        };
-
-                        var usuariosParaNotificar = dto.UsuariosIds ?? new List<string>();
-                        await _alertasRepo.CriarAlertaAsync(alerta, usuariosParaNotificar);
-
+                    var recorrenciaId = Guid.NewGuid();
+                    var usuariosParaNotificar = dto.UsuariosIds ?? new List<string>();
+
+                    foreach (var dataExibicao in datasRecorrencia)
+                    {
+                        var alerta = CriarAlertaBase(dto , usuarioId);
+                        alerta.AlertasFrotiXId = alertasCriados.Count == 0 ? recorrenciaId : Guid.NewGuid();
+                        alerta.RecorrenciaAlertaId = recorrenciaId;
+                        alerta.DataExibicao = dataExibicao;
+
+                        await _alertasRepo.CriarAlertaAsync(alerta , usuariosParaNotificar);
                         alertasCriados.Add(alerta.AlertasFrotiXId);
 
-                        await NotificarUsuariosNovoAlerta(alerta, dto.UsuariosIds);
+                        await NotificarUsuariosNovoAlerta(alerta , dto.UsuariosIds);
                     }
 
                     return Ok(new
                     {
-                        success = true,
-                        message = $"{alertasCriados.Count} alertas recorrentes criados com sucesso",
-                        alertasIds = alertasCriados,
-                        quantidadeAlertas = alertasCriados.Count,
-                        recorrenciaAlertaId = recorrenciaAlertaId
+                        success = true ,
+                        message = $"{alertasCriados.Count} alertas criados com sucesso (recorrência)" ,
+                        alertasIds = alertasCriados ,
+                        quantidadeAlertas = alertasCriados.Count
                     });
                 }
 
@@ -551,7 +564,7 @@
                 if (isEdicao)
                 {
                     alertaUnico = await _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync(
-                        a => a.AlertasFrotiXId == dto.AlertasFrotiXId,
+                        a => a.AlertasFrotiXId == dto.AlertasFrotiXId ,
                         includeProperties: "AlertasUsuarios"
                     );
 
@@ -559,7 +572,7 @@
                     {
                         return NotFound(new
                         {
-                            success = false,
+                            success = false ,
                             message = "Alerta não encontrado"
                         });
                     }
@@ -572,13 +585,15 @@
                     alertaUnico.DataExibicao = dto.DataExibicao;
                     alertaUnico.HorarioExibicao = dto.HorarioExibicao;
                     alertaUnico.DataExpiracao = dto.DataExpiracao;
-                    alertaUnico.DiasSemana = dto.DiasSemana;
+                    alertaUnico.DiasSemana = ConverterDiasSemanaTexto(dto.DiasSemana);
                     alertaUnico.DiaMesRecorrencia = dto.DiaMesRecorrencia;
                     alertaUnico.ViagemId = dto.ViagemId;
                     alertaUnico.ManutencaoId = dto.ManutencaoId;
                     alertaUnico.MotoristaId = dto.MotoristaId;
                     alertaUnico.VeiculoId = dto.VeiculoId;
 
+                    AplicarDiasSemana(alertaUnico , dto);
+
                     _unitOfWork.AlertasFrotiX.Update(alertaUnico);
 
                     var associacoesAntigas = await _unitOfWork.AlertasUsuario.GetAllAsync(
@@ -597,9 +612,9 @@
                         {
                             var alertaUsuario = new AlertasUsuario
                             {
-                                AlertasFrotiXId = alertaUnico.AlertasFrotiXId,
-                                UsuarioId = uid,
-                                Lido = false,
+                                AlertasFrotiXId = alertaUnico.AlertasFrotiXId ,
+                                UsuarioId = uid ,
+                                Lido = false ,
                                 Notificado = false
                             };
                             _unitOfWork.AlertasUsuario.Add(alertaUsuario);
@@ -612,47 +627,48 @@
                 {
                     alertaUnico = new AlertasFrotiX
                     {
-                        AlertasFrotiXId = Guid.NewGuid(),
-                        Titulo = dto.Titulo,
-                        Descricao = dto.Descricao,
-                        TipoAlerta = (TipoAlerta)dto.TipoAlerta,
-                        Prioridade = (PrioridadeAlerta)dto.Prioridade,
-                        TipoExibicao = (TipoExibicaoAlerta)dto.TipoExibicao,
-                        DataExibicao = dto.DataExibicao,
-                        HorarioExibicao = dto.HorarioExibicao,
-                        DataExpiracao = dto.DataExpiracao,
-                        DiasSemana = dto.DiasSemana,
-                        DiaMesRecorrencia = dto.DiaMesRecorrencia,
-                        DataInsercao = DateTime.Now,
-                        UsuarioCriadorId = usuarioId,
-                        Ativo = true,
-                        ViagemId = dto.ViagemId,
-                        ManutencaoId = dto.ManutencaoId,
-                        MotoristaId = dto.MotoristaId,
+                        AlertasFrotiXId = Guid.NewGuid() ,
+                        Titulo = dto.Titulo ,
+                        Descricao = dto.Descricao ,
+                        TipoAlerta = (TipoAlerta)dto.TipoAlerta ,
+                        Prioridade = (PrioridadeAlerta)dto.Prioridade ,
+                        TipoExibicao = (TipoExibicaoAlerta)dto.TipoExibicao ,
+                        DataExibicao = dto.DataExibicao ,
+                        HorarioExibicao = dto.HorarioExibicao ,
+                        DataExpiracao = dto.DataExpiracao ,
+                        DiasSemana = ConverterDiasSemanaTexto(dto.DiasSemana) ,
+                        DiaMesRecorrencia = dto.DiaMesRecorrencia ,
+                        DataInsercao = DateTime.Now ,
+                        UsuarioCriadorId = usuarioId ,
+                        Ativo = true ,
+                        ViagemId = dto.ViagemId ,
+                        ManutencaoId = dto.ManutencaoId ,
+                        MotoristaId = dto.MotoristaId ,
                         VeiculoId = dto.VeiculoId
                     };
 
+                    AplicarDiasSemana(alertaUnico , dto);
+
                     var usuariosParaNotificar = dto.UsuariosIds ?? new List<string>();
-                    await _alertasRepo.CriarAlertaAsync(alertaUnico, usuariosParaNotificar);
-                }
-
-                await NotificarUsuariosNovoAlerta(alertaUnico, dto.UsuariosIds);
+                    await _alertasRepo.CriarAlertaAsync(alertaUnico , usuariosParaNotificar);
+                }
+
+                await NotificarUsuariosNovoAlerta(alertaUnico , dto.UsuariosIds);
 
                 return Ok(new
                 {
-                    success = true,
-                    message = isEdicao ? "Alerta atualizado com sucesso" : "Alerta criado com sucesso",
+                    success = true ,
+                    message = isEdicao ? "Alerta atualizado com sucesso" : "Alerta criado com sucesso" ,
                     alertaId = alertaUnico.AlertasFrotiXId
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AlertasFrotiXController] Erro em Salvar: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "Salvar", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    message = "Erro ao salvar alerta: " + ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "Salvar" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    message = "Erro ao salvar alerta: " + error.Message
                 });
             }
         }
@@ -665,52 +681,287 @@
             public int TipoAlerta { get; set; }
             public int Prioridade { get; set; }
             public int TipoExibicao { get; set; }
+
             public DateTime? DataExibicao { get; set; }
+
             public TimeSpan? HorarioExibicao { get; set; }
             public DateTime? DataExpiracao { get; set; }
-            public string DiasSemana { get; set; }
+
+            public List<int> DiasSemana { get; set; }
+
             public int? DiaMesRecorrencia { get; set; }
             public string DatasSelecionadas { get; set; }
+
             public Guid? ViagemId { get; set; }
+
             public Guid? ManutencaoId { get; set; }
             public Guid? MotoristaId { get; set; }
             public Guid? VeiculoId { get; set; }
+
             public List<string> UsuariosIds { get; set; }
         }
 
-        private async Task NotificarUsuariosNovoAlerta(AlertasFrotiX alerta, List<string> usuariosIds)
+        private AlertasFrotiX CriarAlertaBase(AlertaDto dto , string usuarioId)
+        {
+            var alerta = new AlertasFrotiX
+            {
+                AlertasFrotiXId = Guid.NewGuid() ,
+                Titulo = dto.Titulo ,
+                Descricao = dto.Descricao ,
+                TipoAlerta = (TipoAlerta)dto.TipoAlerta ,
+                Prioridade = (PrioridadeAlerta)dto.Prioridade ,
+                TipoExibicao = (TipoExibicaoAlerta)dto.TipoExibicao ,
+                DataExibicao = dto.DataExibicao ,
+                HorarioExibicao = dto.HorarioExibicao ,
+                DataExpiracao = dto.DataExpiracao ,
+                DiasSemana = ConverterDiasSemanaTexto(dto.DiasSemana) ,
+                DiaMesRecorrencia = dto.DiaMesRecorrencia ,
+                DatasSelecionadas = dto.DatasSelecionadas ,
+                DataInsercao = DateTime.Now ,
+                UsuarioCriadorId = usuarioId ,
+                Ativo = true ,
+                ViagemId = dto.ViagemId ,
+                ManutencaoId = dto.ManutencaoId ,
+                MotoristaId = dto.MotoristaId ,
+                VeiculoId = dto.VeiculoId
+            };
+
+            AplicarDiasSemana(alerta , dto);
+
+            return alerta;
+        }
+
+        private string ConverterDiasSemanaTexto(List<int> diasSemana)
+        {
+            if (diasSemana == null || diasSemana.Count == 0)
+            {
+                return string.Empty;
+            }
+
+            return string.Join("," , diasSemana.OrderBy(d => d));
+        }
+
+        private void AplicarDiasSemana(AlertasFrotiX alerta , AlertaDto dto)
+        {
+            alerta.Monday = false;
+            alerta.Tuesday = false;
+            alerta.Wednesday = false;
+            alerta.Thursday = false;
+            alerta.Friday = false;
+            alerta.Saturday = false;
+            alerta.Sunday = false;
+
+            if (dto.TipoExibicao == 4)
+            {
+                alerta.Monday = true;
+                alerta.Tuesday = true;
+                alerta.Wednesday = true;
+                alerta.Thursday = true;
+                alerta.Friday = true;
+                return;
+            }
+
+            if (dto.TipoExibicao == 5 || dto.TipoExibicao == 6)
+            {
+                if (dto.DiasSemana == null || dto.DiasSemana.Count == 0)
+                {
+                    return;
+                }
+
+                foreach (var dia in dto.DiasSemana)
+                {
+                    switch (dia)
+                    {
+                        case 0:
+                            alerta.Sunday = true;
+                            break;
+                        case 1:
+                            alerta.Monday = true;
+                            break;
+                        case 2:
+                            alerta.Tuesday = true;
+                            break;
+                        case 3:
+                            alerta.Wednesday = true;
+                            break;
+                        case 4:
+                            alerta.Thursday = true;
+                            break;
+                        case 5:
+                            alerta.Friday = true;
+                            break;
+                        case 6:
+                            alerta.Saturday = true;
+                            break;
+                    }
+                }
+            }
+        }
+
+        private List<DateTime> GerarDatasRecorrencia(AlertaDto dto)
+        {
+            if (!dto.DataExibicao.HasValue && dto.TipoExibicao != 8)
+            {
+                return new List<DateTime>();
+            }
+
+            var dataInicial = (dto.DataExibicao ?? DateTime.Today).Date;
+            var dataFinal = (dto.DataExpiracao ?? dto.DataExibicao ?? DateTime.Today).Date;
+
+            if (dataFinal < dataInicial)
+            {
+                return new List<DateTime>();
+            }
+
+            switch (dto.TipoExibicao)
+            {
+                case 4:
+                    return GerarDatasDiarias(dataInicial , dataFinal);
+                case 5:
+                    return GerarDatasSemanais(dataInicial , dataFinal , dto.DiasSemana , 1);
+                case 6:
+                    return GerarDatasSemanais(dataInicial , dataFinal , dto.DiasSemana , 2);
+                case 7:
+                    return GerarDatasMensais(dataInicial , dataFinal , dto.DiaMesRecorrencia ?? dataInicial.Day);
+                case 8:
+                    return GerarDatasVariadas(dto.DatasSelecionadas);
+                default:
+                    return new List<DateTime>();
+            }
+        }
+
+        private List<DateTime> GerarDatasDiarias(DateTime dataInicial , DateTime dataFinal)
+        {
+            var datas = new List<DateTime>();
+
+            for (var data = dataInicial; data <= dataFinal; data = data.AddDays(1))
+            {
+                if (data.DayOfWeek != DayOfWeek.Saturday && data.DayOfWeek != DayOfWeek.Sunday)
+                {
+                    datas.Add(data);
+                }
+            }
+
+            return datas;
+        }
+
+        private List<DateTime> GerarDatasSemanais(DateTime dataInicial , DateTime dataFinal , List<int> diasSemana , int intervaloSemanas)
+        {
+            if (diasSemana == null || diasSemana.Count == 0)
+            {
+                return new List<DateTime>();
+            }
+
+            var datas = new HashSet<DateTime>();
+            var dataAtual = dataInicial;
+
+            while (dataAtual <= dataFinal)
+            {
+                foreach (var dia in diasSemana)
+                {
+                    var dataDia = ProximoDiaSemana(dataAtual , (DayOfWeek)dia);
+                    if (dataDia >= dataInicial && dataDia <= dataFinal)
+                    {
+                        datas.Add(dataDia);
+                    }
+                }
+
+                dataAtual = dataAtual.AddDays(7 * intervaloSemanas);
+            }
+
+            return datas.OrderBy(d => d).ToList();
+        }
+
+        private List<DateTime> GerarDatasMensais(DateTime dataInicial , DateTime dataFinal , int diaMes)
+        {
+            var datas = new List<DateTime>();
+            var ano = dataInicial.Year;
+            var mes = dataInicial.Month;
+
+            while (new DateTime(ano , mes , 1) <= dataFinal)
+            {
+                var ultimoDiaMes = DateTime.DaysInMonth(ano , mes);
+                var dia = Math.Min(diaMes , ultimoDiaMes);
+                var data = new DateTime(ano , mes , dia);
+
+                if (data >= dataInicial && data <= dataFinal)
+                {
+                    datas.Add(data);
+                }
+
+                if (mes == 12)
+                {
+                    mes = 1;
+                    ano++;
+                }
+                else
+                {
+                    mes++;
+                }
+            }
+
+            return datas;
+        }
+
+        private List<DateTime> GerarDatasVariadas(string datasSelecionadas)
+        {
+            if (string.IsNullOrWhiteSpace(datasSelecionadas))
+            {
+                return new List<DateTime>();
+            }
+
+            var datas = new List<DateTime>();
+            var datasStr = datasSelecionadas.Split(',' , StringSplitOptions.RemoveEmptyEntries);
+
+            foreach (var dataStr in datasStr)
+            {
+                if (DateTime.TryParse(dataStr.Trim() , out DateTime data))
+                {
+                    datas.Add(data.Date);
+                }
+            }
+
+            return datas.OrderBy(d => d).ToList();
+        }
+
+        private DateTime ProximoDiaSemana(DateTime dataBase , DayOfWeek diaSemana)
+        {
+            var diff = ((int)diaSemana - (int)dataBase.DayOfWeek + 7) % 7;
+            return dataBase.AddDays(diff);
+        }
+
+        private async Task NotificarUsuariosNovoAlerta(AlertasFrotiX alerta , List<string> usuariosIds)
         {
             try
             {
                 var alertaPayload = new
                 {
-                    alertaId = alerta.AlertasFrotiXId,
-                    titulo = alerta.Titulo,
-                    descricao = alerta.Descricao,
-                    tipo = alerta.TipoAlerta,
-                    prioridade = alerta.Prioridade,
-                    iconeCss = ObterIconePorTipo(alerta.TipoAlerta),
-                    corBadge = ObterCorPorTipo(alerta.TipoAlerta),
-                    textoBadge = ObterTextoPorTipo(alerta.TipoAlerta),
+                    alertaId = alerta.AlertasFrotiXId ,
+                    titulo = alerta.Titulo ,
+                    descricao = alerta.Descricao ,
+                    tipo = alerta.TipoAlerta ,
+                    prioridade = alerta.Prioridade ,
+                    iconeCss = ObterIconePorTipo(alerta.TipoAlerta) ,
+                    corBadge = ObterCorPorTipo(alerta.TipoAlerta) ,
+                    textoBadge = ObterTextoPorTipo(alerta.TipoAlerta) ,
                     dataInsercao = alerta.DataInsercao
                 };
 
                 if (usuariosIds == null || usuariosIds.Count == 0)
                 {
-                    await _hubContext.Clients.All.SendAsync("NovoAlerta", alertaPayload);
+                    await _hubContext.Clients.All.SendAsync("NovoAlerta" , alertaPayload);
                 }
                 else
                 {
                     foreach (var usuarioId in usuariosIds)
                     {
-                        await _hubContext.Clients.User(usuarioId).SendAsync("NovoAlerta", alertaPayload);
-                    }
-                }
-            }
-            catch (Exception ex)
-            {
-                _log.Error("[AlertasFrotiXController] Erro em NotificarUsuariosNovoAlerta: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "NotificarUsuariosNovoAlerta", ex);
+                        await _hubContext.Clients.User(usuarioId).SendAsync("NovoAlerta" , alertaPayload);
+                    }
+                }
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "NotificarUsuariosNovoAlerta" , error);
             }
         }
 
@@ -730,15 +981,15 @@
 
                     return new
                     {
-                        alertaId = a.AlertasFrotiXId,
-                        titulo = a.Titulo,
-                        descricao = a.Descricao,
-                        tipo = ObterTextoPorTipo(a.TipoAlerta),
-                        prioridade = a.Prioridade.ToString(),
-                        dataInsercao = a.DataInsercao.HasValue ? a.DataInsercao.Value.ToString("dd/MM/yyyy HH:mm") : "-",
-                        dataLeitura = ultimaLeitura?.DataLeitura?.ToString("dd/MM/yyyy HH:mm") ?? "",
-                        icone = ObterIconePorTipo(a.TipoAlerta),
-                        totalLeituras = a.AlertasUsuarios.Count(au => au.Lido),
+                        alertaId = a.AlertasFrotiXId ,
+                        titulo = a.Titulo ,
+                        descricao = a.Descricao ,
+                        tipo = ObterTextoPorTipo(a.TipoAlerta) ,
+                        prioridade = a.Prioridade.ToString() ,
+                        dataInsercao = a.DataInsercao.HasValue ? a.DataInsercao.Value.ToString("dd/MM/yyyy HH:mm") : "-" ,
+                        dataLeitura = ultimaLeitura?.DataLeitura?.ToString("dd/MM/yyyy HH:mm") ?? "" ,
+                        icone = ObterIconePorTipo(a.TipoAlerta) ,
+                        totalLeituras = a.AlertasUsuarios.Count(au => au.Lido) ,
                         totalUsuarios = a.AlertasUsuarios.Count
                     };
                 }).ToList();
@@ -748,10 +999,9 @@
                     data = resultado
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AlertasFrotiXController] Erro em GetHistoricoAlertas: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetHistoricoAlertas", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetHistoricoAlertas" , error);
                 return Ok(new
                 {
                     data = new List<object>()
@@ -774,8 +1024,8 @@
 
         [HttpGet("GetAlertasFinalizados")]
         public async Task<IActionResult> GetAlertasFinalizados(
-            [FromQuery] int? dias = 30,
-            [FromQuery] int pagina = 1,
+            [FromQuery] int? dias = 30 ,
+            [FromQuery] int pagina = 1 ,
             [FromQuery] int tamanhoPagina = 20)
         {
             try
@@ -799,37 +1049,36 @@
                     .Take(tamanhoPagina)
                     .Select(a => new
                     {
-                        alertaId = a.AlertasFrotiXId,
-                        titulo = a.Titulo,
-                        descricao = a.Descricao,
-                        tipo = ObterTextoPorTipo(a.TipoAlerta),
-                        prioridade = a.Prioridade.ToString(),
-                        dataInsercao = a.DataInsercao,
-                        dataFinalizacao = a.DataDesativacao,
-                        finalizadoPor = a.DesativadoPor,
+                        alertaId = a.AlertasFrotiXId ,
+                        titulo = a.Titulo ,
+                        descricao = a.Descricao ,
+                        tipo = ObterTextoPorTipo(a.TipoAlerta) ,
+                        prioridade = a.Prioridade.ToString() ,
+                        dataInsercao = a.DataInsercao ,
+                        dataFinalizacao = a.DataDesativacao ,
+                        finalizadoPor = a.DesativadoPor ,
                         motivo = a.MotivoDesativacao
                     })
                     .ToList();
 
                 return Ok(new
                 {
-                    success = true,
-                    total = total,
-                    pagina = pagina,
-                    tamanhoPagina = tamanhoPagina,
-                    totalPaginas = (int)Math.Ceiling((double)total / tamanhoPagina),
+                    success = true ,
+                    total = total ,
+                    pagina = pagina ,
+                    tamanhoPagina = tamanhoPagina ,
+                    totalPaginas = (int)Math.Ceiling((double)total / tamanhoPagina) ,
                     dados = alertasPaginados
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AlertasFrotiXController] Erro em GetAlertasFinalizados: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetAlertasFinalizados", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    mensagem = "Erro ao buscar histórico",
-                    erro = ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetAlertasFinalizados" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    mensagem = "Erro ao buscar histórico" ,
+                    erro = error.Message
                 });
             }
         }
@@ -847,7 +1096,7 @@
                 {
                     return NotFound(new
                     {
-                        success = false,
+                        success = false ,
                         mensagem = "Alerta não encontrado"
                     });
                 }
@@ -856,7 +1105,7 @@
                 {
                     return BadRequest(new
                     {
-                        success = false,
+                        success = false ,
                         mensagem = "Este alerta já foi finalizado anteriormente"
                     });
                 }
@@ -876,22 +1125,21 @@
 
                 return Ok(new
                 {
-                    success = true,
-                    mensagem = "Baixa do alerta realizada com sucesso",
-                    alertaId = alertaId,
-                    dataFinalizacao = DateTime.Now,
+                    success = true ,
+                    mensagem = "Baixa do alerta realizada com sucesso" ,
+                    alertaId = alertaId ,
+                    dataFinalizacao = DateTime.Now ,
                     finalizadoPor = usuarioAtual
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AlertasFrotiXController] Erro em DarBaixaAlerta: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "DarBaixaAlerta", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    mensagem = "Erro interno ao processar a baixa do alerta",
-                    erro = ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "DarBaixaAlerta" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    mensagem = "Erro interno ao processar a baixa do alerta" ,
+                    erro = error.Message
                 });
             }
         }
@@ -906,8 +1154,11 @@
                                 ?? User.FindFirst(ClaimTypes.Name)?.Value
                                 ?? User.Identity?.Name;
 
+                Console.WriteLine($"GetMeusAlertas - UsuarioId: {usuarioId ?? "NULL"}");
+
                 if (string.IsNullOrEmpty(usuarioId))
                 {
+                    Console.WriteLine("UsuarioId está NULL - retornando lista vazia");
                     return Ok(new
                     {
                         data = new List<object>()
@@ -915,44 +1166,50 @@
                 }
 
                 var alertasUsuario = await _unitOfWork.AlertasUsuario.GetAllAsync(
-                    filter: au => au.UsuarioId == usuarioId,
+                    filter: au => au.UsuarioId == usuarioId ,
                     includeProperties: "AlertasFrotiX"
                 );
+
+                Console.WriteLine($"Total de alertas encontrados: {alertasUsuario.Count()}");
 
                 var resultado = alertasUsuario
                     .Where(au => au.AlertasFrotiX != null)
                     .OrderByDescending(au => au.AlertasFrotiX.DataInsercao)
                     .Select(au => new
                     {
-                        alertaId = au.AlertasFrotiXId,
-                        titulo = au.AlertasFrotiX.Titulo,
-                        descricao = au.AlertasFrotiX.Descricao,
-                        tipo = ObterTextoPorTipo(au.AlertasFrotiX.TipoAlerta),
-                        icone = ObterIconePorTipo(au.AlertasFrotiX.TipoAlerta),
-                        notificado = au.Notificado,
-                        notificadoTexto = au.Notificado ? "Sim" : "Não",
-                        dataNotificacao = au.DataNotificacao?.ToString("dd/MM/yyyy HH:mm") ?? "-",
-                        lido = au.Lido,
-                        lidoTexto = au.Lido ? "Sim" : "Não",
-                        dataLeitura = au.DataLeitura?.ToString("dd/MM/yyyy HH:mm") ?? "-",
-                        prioridade = au.AlertasFrotiX.Prioridade.ToString(),
-                        dataCriacao = au.AlertasFrotiX.DataInsercao?.ToString("dd/MM/yyyy HH:mm") ?? "-"
+                        alertaId = au.AlertasFrotiXId ,
+                        titulo = au.AlertasFrotiX.Titulo ,
+                        descricao = au.AlertasFrotiX.Descricao ,
+                        tipo = ObterTextoPorTipo(au.AlertasFrotiX.TipoAlerta) ,
+                        icone = ObterIconePorTipo(au.AlertasFrotiX.TipoAlerta) ,
+                        notificado = au.Notificado ,
+                        notificadoTexto = au.Notificado ? "Sim" : "Não" ,
+                        dataNotificacao = au.DataNotificacao?.ToString("dd/MM/yyyy HH:mm") ?? "-" ,
+                        lido = au.Lido ,
+                        lidoTexto = au.Lido ? "Sim" : "Não" ,
+                        dataLeitura = au.DataLeitura?.ToString("dd/MM/yyyy HH:mm") ?? "-" ,
+                        prioridade = au.AlertasFrotiX.Prioridade.ToString() ,
+                        dataCriacao = au.AlertasFrotiX.DataInsercao
                     })
                     .ToList();
 
+                Console.WriteLine($"Total de resultados processados: {resultado.Count}");
+
                 return Ok(new
                 {
                     data = resultado
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AlertasFrotiXController] Erro em GetMeusAlertas: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetMeusAlertas", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    message = "Erro ao buscar meus alertas: " + ex.Message,
+            catch (Exception error)
+            {
+                Console.WriteLine($"ERRO em GetMeusAlertas: {error.Message}");
+                Console.WriteLine($"Stack: {error.StackTrace}");
+
+                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetMeusAlertas" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    message = "Erro ao buscar meus alertas: " + error.Message ,
                     data = new List<object>()
                 });
             }
@@ -964,7 +1221,7 @@
             try
             {
                 var alertasInativos = await _unitOfWork.AlertasFrotiX.GetAllAsync(
-                    filter: a => !a.Ativo,
+                    filter: a => !a.Ativo ,
                     includeProperties: "AlertasUsuarios"
                 );
 
@@ -982,17 +1239,17 @@
 
                         return new
                         {
-                            alertaId = a.AlertasFrotiXId,
-                            titulo = a.Titulo,
-                            descricao = a.Descricao,
-                            tipo = ObterTextoPorTipo(a.TipoAlerta),
-                            prioridade = a.Prioridade.ToString(),
-                            dataInsercao = a.DataInsercao?.ToString("dd/MM/yyyy HH:mm"),
-                            dataDesativacao = a.DataDesativacao?.ToString("dd/MM/yyyy HH:mm") ?? "-",
-                            icone = ObterIconePorTipo(a.TipoAlerta),
-                            percentualLeitura = percentualLeitura,
-                            totalUsuarios = totalUsuarios,
-                            totalNotificados = totalNotificados,
+                            alertaId = a.AlertasFrotiXId ,
+                            titulo = a.Titulo ,
+                            descricao = a.Descricao ,
+                            tipo = ObterTextoPorTipo(a.TipoAlerta) ,
+                            prioridade = a.Prioridade.ToString() ,
+                            dataInsercao = a.DataInsercao?.ToString("dd/MM/yyyy HH:mm") ,
+                            dataDesativacao = a.DataDesativacao?.ToString("dd/MM/yyyy HH:mm") ?? "-" ,
+                            icone = ObterIconePorTipo(a.TipoAlerta) ,
+                            percentualLeitura = percentualLeitura ,
+                            totalUsuarios = totalUsuarios ,
+                            totalNotificados = totalNotificados ,
                             totalLeram = totalLeram
                         };
                     })
@@ -1003,10 +1260,9 @@
                     data = resultado
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AlertasFrotiXController] Erro em GetAlertasInativos: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetAlertasInativos", ex);
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetAlertasInativos" , error);
                 return Ok(new
                 {
                     data = new List<object>()
@@ -1020,7 +1276,7 @@
             try
             {
                 var alertasAtivos = await _unitOfWork.AlertasFrotiX.GetAllAsync(
-                    filter: a => a.Ativo,
+                    filter: a => a.Ativo ,
                     includeProperties: "AlertasUsuarios"
                 );
 
@@ -1036,34 +1292,33 @@
 
                     return new
                     {
-                        alertaId = a.AlertasFrotiXId,
-                        titulo = a.Titulo,
-                        descricao = a.Descricao,
-                        mensagem = a.Descricao,
-                        tipo = (int)a.TipoAlerta,
-                        prioridade = (int)a.Prioridade,
-                        dataInsercao = a.DataInsercao,
-                        usuarioCriadorId = a.UsuarioCriadorId,
-                        totalUsuarios = totalUsuarios,
-                        usuariosLeram = usuariosLeram,
-                        iconeCss = Alerta.GetIconePrioridade(a.Prioridade),
-                        corBadge = Alerta.GetCorHexPrioridade(a.Prioridade),
-                        textoBadge = a.Prioridade.ToString(),
+                        alertaId = a.AlertasFrotiXId ,
+                        titulo = a.Titulo ,
+                        descricao = a.Descricao ,
+                        mensagem = a.Descricao ,
+                        tipo = (int)a.TipoAlerta ,
+                        prioridade = (int)a.Prioridade ,
+                        dataInsercao = a.DataInsercao ,
+                        usuarioCriadorId = a.UsuarioCriadorId ,
+                        totalUsuarios = totalUsuarios ,
+                        usuariosLeram = usuariosLeram ,
+                        iconeCss = Alerta.GetIconePrioridade(a.Prioridade) ,
+                        corBadge = Alerta.GetCorHexPrioridade(a.Prioridade) ,
+                        textoBadge = a.Prioridade.ToString() ,
                         severidade = a.Prioridade.ToString()
                     };
                 }).ToList();
 
                 return Ok(resultado);
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AlertasFrotiXController] Erro em GetTodosAlertasAtivosGestao: {ex.Message}", ex);
-                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetTodosAlertasAtivosGestao", ex);
-                return StatusCode(500, new
-                {
-                    success = false,
-                    message = "Erro ao buscar alertas ativos para gestão",
-                    erro = ex.Message
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetTodosAlertasAtivosGestao" , error);
+                return StatusCode(500 , new
+                {
+                    success = false ,
+                    message = "Erro ao buscar alertas ativos para gestío" ,
+                    erro = error.Message
                 });
             }
         }
@@ -1091,13 +1346,12 @@
                     podeDarBaixa = podeDarBaixa
                 });
             }
-            catch (Exception ex)
-            {
-                _log.Error("[AlertasFrotiXController] Erro em VerificarPermissaoBaixa: {ex.Message}", ex);
+            catch (Exception error)
+            {
                 Alerta.TratamentoErroComLinha(
-                    "AlertasFrotiXController.cs",
-                    "VerificarPermissaoBaixa",
-                    ex
+                    "AlertasFrotiXController.cs" ,
+                    "VerificarPermissaoBaixa" ,
+                    error
                 );
                 return StatusCode(500);
             }
@@ -1116,108 +1370,6 @@
             };
         }
 
-        private List<DateTime> CalcularDatasRecorrentes(AlertaDto dto)
-        {
-            var datas = new List<DateTime>();
-
-            if (!dto.DataExibicao.HasValue)
-                return datas;
-
-            var dataBase = dto.DataExibicao.Value;
-            var dataFinal = dto.DataExpiracao ?? dataBase.AddYears(1);
-
-            switch (dto.TipoExibicao)
-            {
-                case 4:
-                    for (var data = dataBase; data <= dataFinal; data = data.AddDays(1))
-                    {
-                        datas.Add(data);
-                    }
-                    break;
-
-                case 5:
-                    var diasSemana = ParseDiasSemana(dto.DiasSemana);
-                    for (var data = dataBase; data <= dataFinal; data = data.AddDays(1))
-                    {
-                        if (diasSemana.Contains(data.DayOfWeek))
-                        {
-                            datas.Add(data);
-                        }
-                    }
-                    break;
-
-                case 6:
-                    for (var data = dataBase; data <= dataFinal; data = data.AddDays(14))
-                    {
-                        datas.Add(data);
-                    }
-                    break;
-
-                case 7:
-                    if (dto.DiaMesRecorrencia.HasValue)
-                    {
-                        var diaMes = dto.DiaMesRecorrencia.Value;
-                        for (var data = dataBase; data <= dataFinal; data = data.AddMonths(1))
-                        {
-                            var ultimoDiaMes = DateTime.DaysInMonth(data.Year, data.Month);
-                            var diaValido = Math.Min(diaMes, ultimoDiaMes);
-                            var dataRecorrente = new DateTime(data.Year, data.Month, diaValido);
-                            if (dataRecorrente >= dataBase && dataRecorrente <= dataFinal)
-                            {
-                                datas.Add(dataRecorrente);
-                            }
-                        }
-                    }
-                    else
-                    {
-
-                        for (var data = dataBase; data <= dataFinal; data = data.AddMonths(1))
-                        {
-                            datas.Add(data);
-                        }
-                    }
-                    break;
-
-                case 8:
-                    if (!string.IsNullOrWhiteSpace(dto.DatasSelecionadas))
-                    {
-                        var datasStr = dto.DatasSelecionadas.Split(',', StringSplitOptions.RemoveEmptyEntries);
-                        foreach (var dataStr in datasStr)
-                        {
-                            if (DateTime.TryParse(dataStr.Trim(), out DateTime dataExibicao))
-                            {
-                                if (dataExibicao >= dataBase && dataExibicao <= dataFinal)
-                                {
-                                    datas.Add(dataExibicao);
-                                }
-                            }
-                        }
-                    }
-                    break;
-            }
-
-            return datas.Distinct().OrderBy(d => d).ToList();
-        }
-
-        private List<DayOfWeek> ParseDiasSemana(string diasSemanaStr)
-        {
-            var dias = new List<DayOfWeek>();
-
-            if (string.IsNullOrWhiteSpace(diasSemanaStr))
-                return dias;
-
-            var diasArray = diasSemanaStr.Split(',', StringSplitOptions.RemoveEmptyEntries);
-            foreach (var dia in diasArray)
-            {
-                if (int.TryParse(dia.Trim(), out int diaNum) && diaNum >= 0 && diaNum <= 6)
-                {
-                    dias.Add((DayOfWeek)diaNum);
-                }
-            }
-
-            return dias;
-        }
-
         private string ObterTextoPorTipo(TipoAlerta tipo)
         {
             return tipo switch
@@ -1227,25 +1379,25 @@
                 TipoAlerta.Motorista => "Motorista",
                 TipoAlerta.Veiculo => "Veículo",
                 TipoAlerta.Anuncio => "Anúncio",
-                _ => "Aniversario"
+                _ => "Diversos"
             };
         }
+    }
+
+    public class ExportarDetalhesDto
+    {
+        public Guid AlertaId { get; set; }
+        public string Titulo { get; set; }
+        public List<UsuarioExportDto> Usuarios { get; set; }
+    }
+
+    public class UsuarioExportDto
+    {
+        public string NomeUsuario { get; set; }
+        public string Email { get; set; }
+        public bool Lido { get; set; }
+        public bool Apagado { get; set; }
+        public DateTime? DataNotificacao { get; set; }
+        public DateTime? DataLeitura { get; set; }
     }
 }
-
-public class ExportarDetalhesDto
-{
-    public Guid AlertaId { get; set; }
-    public string Titulo { get; set; }
-    public List<UsuarioExportDto> Usuarios { get; set; }
-}
-
-public class UsuarioExportDto
-{
-    public string NomeUsuario { get; set; }
-    public string Email { get; set; }
-    public bool Lido { get; set; }
-    public bool Apagado { get; set; }
-    public DateTime? DataNotificacao { get; set; }
-    public DateTime? DataLeitura { get; set; }
-}
```
