# wwwroot/js/validacao/ValidadorFinalizacaoIA.js

**Mudanca:** GRANDE | **+88** linhas | **-225** linhas

---

```diff
--- JANEIRO: wwwroot/js/validacao/ValidadorFinalizacaoIA.js
+++ ATUAL: wwwroot/js/validacao/ValidadorFinalizacaoIA.js
@@ -8,13 +8,13 @@
             kmMaximoSemHistorico: 500,
             kmAlertaSemHistorico: 100,
             duracaoMaximaMinutos: 720,
-            duracaoMinimaMinutos: 5,
+            duracaoMinimaMinutos: 5
         };
 
         this.zScores = {
             leve: 1.5,
             moderado: 2.5,
-            severo: 3.5,
+            severo: 3.5
         };
 
         this._kmConfirmado = false;
@@ -28,12 +28,12 @@
 
     async validarDataNaoFutura(dataFinal) {
         if (!dataFinal) {
-            return { valido: false, mensagem: 'A Data Final é obrigatória.' };
+            return { valido: false, mensagem: "A Data Final é obrigatória." };
         }
 
         const data = this._parseData(dataFinal);
         if (!data || isNaN(data.getTime())) {
-            return { valido: false, mensagem: 'Data Final inválida.' };
+            return { valido: false, mensagem: "Data Final inválida." };
         }
 
         const hoje = new Date();
@@ -43,21 +43,18 @@
         if (data > hoje) {
             return {
                 valido: false,
-                mensagem:
-                    'A Data Final não pode ser superior à data de hoje.\n\nViagens só podem ser finalizadas com datas passadas ou de hoje.',
+                mensagem: "A Data Final não pode ser superior à data de hoje.\n\nViagens só podem ser finalizadas com datas passadas ou de hoje."
             };
         }
 
-        return { valido: true, mensagem: '' };
+        return { valido: true, mensagem: "" };
     }
 
     async validarDataNaoMuitoAntiga(dataFinal) {
-        if (!dataFinal)
-            return { valido: true, requerConfirmacao: false, mensagem: '' };
+        if (!dataFinal) return { valido: true, requerConfirmacao: false, mensagem: "" };
 
         const data = this._parseData(dataFinal);
-        if (!data)
-            return { valido: true, requerConfirmacao: false, mensagem: '' };
+        if (!data) return { valido: true, requerConfirmacao: false, mensagem: "" };
 
         const hoje = new Date();
         const diasAtras = Math.floor((hoje - data) / (1000 * 60 * 60 * 24));
@@ -66,42 +63,38 @@
             return {
                 valido: true,
                 requerConfirmacao: true,
-                mensagem: `Você está finalizando uma viagem de <strong>${diasAtras} dias atrás</strong>.\n\nIsso está correto?`,
+                mensagem: `Você está finalizando uma viagem de <strong>${diasAtras} dias atrás</strong>.\n\nIsso está correto?`
             };
         }
 
-        return { valido: true, requerConfirmacao: false, mensagem: '' };
+        return { valido: true, requerConfirmacao: false, mensagem: "" };
     }
 
     async analisarDatasHoras(dados) {
-        const { dataInicial, horaInicial, dataFinal, horaFinal, veiculoId } =
-            dados;
+        const { dataInicial, horaInicial, dataFinal, horaFinal, veiculoId } = dados;
 
         const dtInicial = this._parseDataHora(dataInicial, horaInicial);
         const dtFinal = this._parseDataHora(dataFinal, horaFinal);
 
         if (!dtInicial || !dtFinal) {
-            return { valido: true, nivel: 'ok', mensagem: '', detalhes: {} };
+            return { valido: true, nivel: 'ok', mensagem: "", detalhes: {} };
         }
 
         if (dtFinal <= dtInicial) {
-            const mesmodia =
-                this._parseData(dataInicial)?.getTime() ===
-                this._parseData(dataFinal)?.getTime();
+            const mesmodia = this._parseData(dataInicial)?.getTime() === this._parseData(dataFinal)?.getTime();
             if (mesmodia) {
                 return {
                     valido: false,
                     nivel: 'erro',
                     mensagem: `A hora final (${horaFinal}) é anterior ou igual à hora inicial (${horaInicial}).\n\nIsso não é possível no mesmo dia.`,
-                    detalhes: { horaInicial, horaFinal },
+                    detalhes: { horaInicial, horaFinal }
                 };
             } else {
                 return {
                     valido: false,
                     nivel: 'erro',
-                    mensagem:
-                        'A data/hora final é anterior à data/hora inicial.\n\nPor favor, corrija os valores.',
-                    detalhes: {},
+                    mensagem: "A data/hora final é anterior à data/hora inicial.\n\nPor favor, corrija os valores.",
+                    detalhes: {}
                 };
             }
         }
@@ -114,7 +107,7 @@
                 valido: false,
                 nivel: 'erro',
                 mensagem: `A duração de ${duracaoMinutos} minutos parece muito curta para uma viagem.\n\nVerifique os horários informados.`,
-                detalhes: { duracaoMinutos },
+                detalhes: { duracaoMinutos }
             };
         }
 
@@ -124,26 +117,15 @@
         }
 
         if (estatisticas && estatisticas.dadosSuficientes) {
-            const zScore =
-                (duracaoMinutos - estatisticas.duracaoMediaMinutos) /
-                estatisticas.duracaoDesvioPadraoMinutos;
+            const zScore = (duracaoMinutos - estatisticas.duracaoMediaMinutos) / estatisticas.duracaoDesvioPadraoMinutos;
 
             if (Math.abs(zScore) > this.zScores.severo) {
                 return {
                     valido: true,
                     nivel: 'severo',
                     requerConfirmacao: !this._duracaoConfirmada,
-                    mensagem: this._gerarMensagemDuracaoAnomala(
-                        duracaoMinutos,
-                        estatisticas,
-                        'severo',
-                    ),
-                    detalhes: {
-                        duracaoMinutos,
-                        duracaoHoras,
-                        zScore,
-                        estatisticas,
-                    },
+                    mensagem: this._gerarMensagemDuracaoAnomala(duracaoMinutos, estatisticas, 'severo'),
+                    detalhes: { duracaoMinutos, duracaoHoras, zScore, estatisticas }
                 };
             }
 
@@ -152,17 +134,8 @@
                     valido: true,
                     nivel: 'moderado',
                     requerConfirmacao: !this._duracaoConfirmada,
-                    mensagem: this._gerarMensagemDuracaoAnomala(
-                        duracaoMinutos,
-                        estatisticas,
-                        'moderado',
-                    ),
-                    detalhes: {
-                        duracaoMinutos,
-                        duracaoHoras,
-                        zScore,
-                        estatisticas,
-                    },
+                    mensagem: this._gerarMensagemDuracaoAnomala(duracaoMinutos, estatisticas, 'moderado'),
+                    detalhes: { duracaoMinutos, duracaoHoras, zScore, estatisticas }
                 };
             }
         } else {
@@ -172,19 +145,13 @@
                     valido: true,
                     nivel: 'moderado',
                     requerConfirmacao: !this._duracaoConfirmada,
-                    mensagem:
-                        this._gerarMensagemDuracaoSemHistorico(duracaoMinutos),
-                    detalhes: { duracaoMinutos, duracaoHoras },
-                };
-            }
-        }
-
-        return {
-            valido: true,
-            nivel: 'ok',
-            mensagem: '',
-            detalhes: { duracaoMinutos },
-        };
+                    mensagem: this._gerarMensagemDuracaoSemHistorico(duracaoMinutos),
+                    detalhes: { duracaoMinutos, duracaoHoras }
+                };
+            }
+        }
+
+        return { valido: true, nivel: 'ok', mensagem: "", detalhes: { duracaoMinutos } };
     }
 
     async analisarKm(dados) {
@@ -197,9 +164,8 @@
             return {
                 valido: false,
                 nivel: 'erro',
-                mensagem:
-                    'A quilometragem final é obrigatória e deve ser maior que zero.',
-                detalhes: {},
+                mensagem: "A quilometragem final é obrigatória e deve ser maior que zero.",
+                detalhes: {}
             };
         }
 
@@ -208,7 +174,7 @@
                 valido: false,
                 nivel: 'erro',
                 mensagem: `O Km Final (${kmFim.toLocaleString('pt-BR')}) é MENOR que o Km Inicial (${kmIni.toLocaleString('pt-BR')}).\n\nIsso significaria que o veículo andou ${(kmIni - kmFim).toLocaleString('pt-BR')} km para trás, o que é fisicamente impossível.\n\nPor favor, corrija o valor do Km Final.`,
-                detalhes: { kmInicial: kmIni, kmFinal: kmFim },
+                detalhes: { kmInicial: kmIni, kmFinal: kmFim }
             };
         }
 
@@ -218,7 +184,7 @@
                 nivel: 'alerta',
                 requerConfirmacao: true,
                 mensagem: `A quilometragem final é igual à inicial (${kmIni.toLocaleString('pt-BR')} km).\n\nIsso significa que o veículo não se deslocou.\n\nEssa viagem foi realmente sem deslocamento?`,
-                detalhes: { kmRodado: 0 },
+                detalhes: { kmRodado: 0 }
             };
         }
 
@@ -230,20 +196,15 @@
         }
 
         if (estatisticas && estatisticas.dadosSuficientes) {
-            const zScore =
-                (kmRodado - estatisticas.kmMedio) / estatisticas.kmDesvioPadrao;
+            const zScore = (kmRodado - estatisticas.kmMedio) / estatisticas.kmDesvioPadrao;
 
             if (zScore > this.zScores.severo) {
                 return {
                     valido: true,
                     nivel: 'severo',
                     requerConfirmacao: !this._kmConfirmado,
-                    mensagem: this._gerarMensagemKmAnomalo(
-                        kmRodado,
-                        estatisticas,
-                        'severo',
-                    ),
-                    detalhes: { kmRodado, zScore, estatisticas },
+                    mensagem: this._gerarMensagemKmAnomalo(kmRodado, estatisticas, 'severo'),
+                    detalhes: { kmRodado, zScore, estatisticas }
                 };
             }
 
@@ -252,20 +213,15 @@
                     valido: true,
                     nivel: 'moderado',
                     requerConfirmacao: !this._kmConfirmado,
-                    mensagem: this._gerarMensagemKmAnomalo(
-                        kmRodado,
-                        estatisticas,
-                        'moderado',
-                    ),
-                    detalhes: { kmRodado, zScore, estatisticas },
+                    mensagem: this._gerarMensagemKmAnomalo(kmRodado, estatisticas, 'moderado'),
+                    detalhes: { kmRodado, zScore, estatisticas }
                 };
             }
 
             if (zScore > this.zScores.leve) {
-                console.log(
-                    `[ValidadorIA] Km levemente acima do padrão: ${kmRodado} km (Z=${zScore.toFixed(2)})`,
-                );
-            }
+                console.log(`[ValidadorIA] Km levemente acima do padrão: ${kmRodado} km (Z=${zScore.toFixed(2)})`);
+            }
+
         } else {
 
             if (kmRodado > this.limitesPadrao.kmMaximoSemHistorico) {
@@ -274,7 +230,7 @@
                     nivel: 'moderado',
                     requerConfirmacao: !this._kmConfirmado,
                     mensagem: this._gerarMensagemKmSemHistorico(kmRodado),
-                    detalhes: { kmRodado },
+                    detalhes: { kmRodado }
                 };
             }
 
@@ -284,38 +240,22 @@
                     nivel: 'leve',
                     requerConfirmacao: !this._kmConfirmado,
                     mensagem: `Esta viagem percorreu ${kmRodado.toLocaleString('pt-BR')} km.\n\nConfirma que está correto?`,
-                    detalhes: { kmRodado },
-                };
-            }
-        }
-
-        return {
-            valido: true,
-            nivel: 'ok',
-            mensagem: '',
-            detalhes: { kmRodado },
-        };
+                    detalhes: { kmRodado }
+                };
+            }
+        }
+
+        return { valido: true, nivel: 'ok', mensagem: "", detalhes: { kmRodado } };
     }
 
     async validarFinalizacao(dados) {
-        const {
-            dataInicial,
-            horaInicial,
-            dataFinal,
-            horaFinal,
-            kmInicial,
-            kmFinal,
-            veiculoId,
-        } = dados;
+        const { dataInicial, horaInicial, dataFinal, horaFinal, kmInicial, kmFinal, veiculoId } = dados;
         const erros = [];
         const alertas = [];
 
         const validacaoData = await this.validarDataNaoFutura(dataFinal);
         if (!validacaoData.valido) {
-            erros.push({
-                tipo: 'dataFutura',
-                mensagem: validacaoData.mensagem,
-            });
+            erros.push({ tipo: 'dataFutura', mensagem: validacaoData.mensagem });
         }
 
         if (erros.length > 0) {
@@ -323,85 +263,58 @@
         }
 
         const analiseDatas = await this.analisarDatasHoras({
-            dataInicial,
-            horaInicial,
-            dataFinal,
-            horaFinal,
-            veiculoId,
+            dataInicial, horaInicial, dataFinal, horaFinal, veiculoId
         });
 
         if (!analiseDatas.valido) {
-            erros.push({
-                tipo: 'datas',
-                mensagem: analiseDatas.mensagem,
-                detalhes: analiseDatas.detalhes,
-            });
+            erros.push({ tipo: 'datas', mensagem: analiseDatas.mensagem, detalhes: analiseDatas.detalhes });
         } else if (analiseDatas.requerConfirmacao) {
             alertas.push({
                 tipo: 'duracao',
                 nivel: analiseDatas.nivel,
                 mensagem: analiseDatas.mensagem,
                 detalhes: analiseDatas.detalhes,
-                onConfirm: () => {
-                    this._duracaoConfirmada = true;
-                },
+                onConfirm: () => { this._duracaoConfirmada = true; }
             });
         }
 
-        const analiseKm = await this.analisarKm({
-            kmInicial,
-            kmFinal,
-            veiculoId,
-        });
+        const analiseKm = await this.analisarKm({ kmInicial, kmFinal, veiculoId });
 
         if (!analiseKm.valido) {
-            erros.push({
-                tipo: 'km',
-                mensagem: analiseKm.mensagem,
-                detalhes: analiseKm.detalhes,
-            });
+            erros.push({ tipo: 'km', mensagem: analiseKm.mensagem, detalhes: analiseKm.detalhes });
         } else if (analiseKm.requerConfirmacao) {
             alertas.push({
                 tipo: 'km',
                 nivel: analiseKm.nivel,
                 mensagem: analiseKm.mensagem,
                 detalhes: analiseKm.detalhes,
-                onConfirm: () => {
-                    this._kmConfirmado = true;
-                },
+                onConfirm: () => { this._kmConfirmado = true; }
             });
         }
 
-        const validacaoDataAntiga =
-            await this.validarDataNaoMuitoAntiga(dataFinal);
+        const validacaoDataAntiga = await this.validarDataNaoMuitoAntiga(dataFinal);
         if (validacaoDataAntiga.requerConfirmacao) {
             alertas.push({
                 tipo: 'dataAntiga',
                 nivel: 'leve',
                 mensagem: validacaoDataAntiga.mensagem,
-                detalhes: {},
+                detalhes: {}
             });
         }
 
         return {
             valido: erros.length === 0,
             erros,
-            alertas,
+            alertas
         };
     }
 
     _gerarMensagemKmAnomalo(kmRodado, estatisticas, nivel) {
-        const percentualAcima = (
-            (kmRodado / estatisticas.kmMedio - 1) *
-            100
-        ).toFixed(0);
+        const percentualAcima = ((kmRodado / estatisticas.kmMedio - 1) * 100).toFixed(0);
         const tempoEstimado = (kmRodado / 80).toFixed(1);
 
         let icone = nivel === 'severo' ? '🚨' : '⚠️';
-        let titulo =
-            nivel === 'severo'
-                ? 'QUILOMETRAGEM MUITO ACIMA DO PADRÃO'
-                : 'Quilometragem Acima do Padrão';
+        let titulo = nivel === 'severo' ? 'QUILOMETRAGEM MUITO ACIMA DO PADRÃO' : 'Quilometragem Acima do Padrão';
 
         return `${icone} <strong>${titulo}</strong>
 
@@ -444,9 +357,7 @@
         const mediaMinutos = Math.round(estatisticas.duracaoMediaMinutos % 60);
         const mediaFormatada = `${mediaHoras}h ${mediaMinutos}min`;
 
-        const vezes = (
-            duracaoMinutos / estatisticas.duracaoMediaMinutos
-        ).toFixed(1);
+        const vezes = (duracaoMinutos / estatisticas.duracaoMediaMinutos).toFixed(1);
 
         let icone = nivel === 'severo' ? '🕐' : '⏰';
 
@@ -477,21 +388,19 @@
 
         const cacheKey = veiculoId.toString();
         const cached = this.cacheEstatisticas.get(cacheKey);
-        if (cached && Date.now() - cached.timestamp < this.cacheDuracao) {
+        if (cached && (Date.now() - cached.timestamp) < this.cacheDuracao) {
             return cached.data;
         }
 
         try {
-            const response = await fetch(
-                `/api/Viagem/EstatisticasVeiculo?veiculoId=${veiculoId}`,
-            );
+            const response = await fetch(`/api/Viagem/EstatisticasVeiculo?veiculoId=${veiculoId}`);
             const result = await response.json();
 
             if (result.success && result.data) {
 
                 this.cacheEstatisticas.set(cacheKey, {
                     data: result.data,
-                    timestamp: Date.now(),
+                    timestamp: Date.now()
                 });
                 return result.data;
             }
@@ -535,31 +444,19 @@
 
 async function mostrarErroValidacaoIA(mensagem) {
 
-    await Alerta.Erro('Erro de Validação', mensagem);
+    await Alerta.Erro("Erro de Validação", mensagem);
 }
 
 async function mostrarConfirmacaoValidacaoIA(mensagem, nivel) {
-    const botaoConfirma =
-        nivel === 'severo' ? 'Sim, confirmo!' : 'Está correto';
-    const botaoCancela = nivel === 'severo' ? 'Deixa eu corrigir' : 'Corrigir';
-    const titulo =
-        nivel === 'severo' ? 'Atenção - Análise IA' : 'Verificação Inteligente';
+    const botaoConfirma = nivel === 'severo' ? "Sim, confirmo!" : "Está correto";
+    const botaoCancela = nivel === 'severo' ? "Deixa eu corrigir" : "Corrigir";
+    const titulo = nivel === 'severo' ? "Atenção - Análise IA" : "Verificação Inteligente";
 
     if (window.Alerta?.ValidacaoIAConfirmar) {
-        return await Alerta.ValidacaoIAConfirmar(
-            titulo,
-            mensagem,
-            botaoConfirma,
-            botaoCancela,
-        );
+        return await Alerta.ValidacaoIAConfirmar(titulo, mensagem, botaoConfirma, botaoCancela);
     } else {
 
-        return await Alerta.Confirmar(
-            titulo,
-            mensagem,
-            botaoConfirma,
-            botaoCancela,
-        );
+        return await Alerta.Confirmar(titulo, mensagem, botaoConfirma, botaoCancela);
     }
 }
 
@@ -576,10 +473,7 @@
         }
 
         for (const alerta of resultado.alertas) {
-            const confirmou = await mostrarConfirmacaoValidacaoIA(
-                alerta.mensagem,
-                alerta.nivel,
-            );
+            const confirmou = await mostrarConfirmacaoValidacaoIA(alerta.mensagem, alerta.nivel);
             if (!confirmou) {
                 return false;
             }
@@ -609,40 +503,27 @@
         let temErros = false;
 
         if (dados.dataFinal) {
-            const resultadoDataFutura = await validador.validarDataNaoFutura(
-                dados.dataFinal,
-            );
+            const resultadoDataFutura = await validador.validarDataNaoFutura(dados.dataFinal);
             if (!resultadoDataFutura.valido) {
-                await Alerta.Erro(
-                    'Data Inválida',
-                    resultadoDataFutura.mensagem,
-                );
+                await Alerta.Erro("Data Inválida", resultadoDataFutura.mensagem);
                 return false;
             }
         }
 
-        if (
-            dados.dataInicial &&
-            dados.horaInicial &&
-            dados.dataFinal &&
-            dados.horaFinal
-        ) {
+        if (dados.dataInicial && dados.horaInicial && dados.dataFinal && dados.horaFinal) {
             const resultadoDatas = await validador.analisarDatasHoras(dados);
 
             if (!resultadoDatas.valido && resultadoDatas.nivel === 'erro') {
-                await Alerta.Erro('Erro de Data/Hora', resultadoDatas.mensagem);
+                await Alerta.Erro("Erro de Data/Hora", resultadoDatas.mensagem);
                 return false;
             }
 
-            if (
-                resultadoDatas.nivel === 'moderado' ||
-                resultadoDatas.nivel === 'severo'
-            ) {
+            if (resultadoDatas.nivel === 'moderado' || resultadoDatas.nivel === 'severo') {
                 alertasPendentes.push({
                     tipo: 'duracao',
                     nivel: resultadoDatas.nivel,
                     titulo: '⏱️ DURAÇÃO DA VIAGEM',
-                    mensagem: resultadoDatas.mensagem,
+                    mensagem: resultadoDatas.mensagem
                 });
             }
         }
@@ -652,46 +533,33 @@
             const kmFinal = parseInt(dados.kmFinal) || 0;
 
             if (kmFinal <= kmInicial) {
-                await Alerta.Erro(
-                    'Erro de Quilometragem',
-                    'A quilometragem final deve ser maior que a inicial.',
-                );
+                await Alerta.Erro("Erro de Quilometragem", "A quilometragem final deve ser maior que a inicial.");
                 return false;
             }
 
             const resultadoKm = await validador.analisarKm({
                 kmInicial: kmInicial,
                 kmFinal: kmFinal,
-                veiculoId: dados.veiculoId,
+                veiculoId: dados.veiculoId
             });
 
             if (!resultadoKm.valido && resultadoKm.nivel === 'erro') {
-                await Alerta.Erro(
-                    'Erro de Quilometragem',
-                    resultadoKm.mensagem,
-                );
+                await Alerta.Erro("Erro de Quilometragem", resultadoKm.mensagem);
                 return false;
             }
 
-            if (
-                resultadoKm.nivel === 'moderado' ||
-                resultadoKm.nivel === 'severo'
-            ) {
+            if (resultadoKm.nivel === 'moderado' || resultadoKm.nivel === 'severo') {
                 alertasPendentes.push({
                     tipo: 'km',
                     nivel: resultadoKm.nivel,
                     titulo: '🚗 QUILOMETRAGEM',
-                    mensagem: resultadoKm.mensagem,
+                    mensagem: resultadoKm.mensagem
                 });
             }
         }
 
         if (alertasPendentes.length > 0) {
-            const nivelMaisAlto = alertasPendentes.some(
-                (a) => a.nivel === 'severo',
-            )
-                ? 'severo'
-                : 'moderado';
+            const nivelMaisAlto = alertasPendentes.some(a => a.nivel === 'severo') ? 'severo' : 'moderado';
 
             let mensagemConsolidada = '';
 
@@ -700,28 +568,24 @@
                 mensagemConsolidada = alertasPendentes[0].mensagem;
             } else {
 
-                mensagemConsolidada =
-                    '<strong>A Análise Inteligente identificou os seguintes pontos:</strong>\n\n';
+                mensagemConsolidada = '<strong>A Análise Inteligente identificou os seguintes pontos:</strong>\n\n';
 
                 for (const alerta of alertasPendentes) {
                     mensagemConsolidada += `<strong>${alerta.titulo}</strong>\n`;
                     mensagemConsolidada += alerta.mensagem + '\n\n';
                 }
 
-                mensagemConsolidada +=
-                    '<strong>Deseja prosseguir mesmo assim?</strong>';
-            }
-
-            const botaoConfirma =
-                nivelMaisAlto === 'severo' ? 'Sim, confirmo!' : 'Está correto';
-            const botaoCancela =
-                nivelMaisAlto === 'severo' ? 'Deixa eu corrigir' : 'Corrigir';
+                mensagemConsolidada += '<strong>Deseja prosseguir mesmo assim?</strong>';
+            }
+
+            const botaoConfirma = nivelMaisAlto === 'severo' ? "Sim, confirmo!" : "Está correto";
+            const botaoCancela = nivelMaisAlto === 'severo' ? "Deixa eu corrigir" : "Corrigir";
 
             const confirmou = await Alerta.ValidacaoIAConfirmar(
-                'Verificação Final',
+                "Verificação Final",
                 mensagemConsolidada,
                 botaoConfirma,
-                botaoCancela,
+                botaoCancela
             );
 
             if (!confirmou) {
```
