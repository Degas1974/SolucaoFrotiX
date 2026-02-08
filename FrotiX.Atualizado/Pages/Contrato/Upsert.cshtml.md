# Pages/Contrato/Upsert.cshtml

**Mudanca:** GRANDE | **+131** linhas | **-277** linhas

---

```diff
--- JANEIRO: Pages/Contrato/Upsert.cshtml
+++ ATUAL: Pages/Contrato/Upsert.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
 
 @model FrotiX.Pages.Contrato.UpsertModel
@@ -776,23 +775,6 @@
 </form>
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * CÁLCULO DE VALORES NA GRID DE VEÍCULOS
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Calcula o valor total de cada item(quantidade x valor unitário)
-        * e atualiza o campo de total geral do contrato.
-     */
-
-    /**
-     * Callback queryCellInfo da grid Syncfusion para cálculo de valores
-     * @@description Multiplica quantidade por valor unitário e exibe na célula "Valor Total".
-     * Também soma todos os itens para atualizar o campo txtTotal.
-     * @@param { Object } args - Argumentos do evento queryCellInfo
-        * @@param { Object } args.data - Dados da linha atual
-            * @@param { Object } args.column - Informações da coluna
-                * @@param { HTMLTableCellElement } args.cell - Célula sendo renderizada
-                    */
     function calculate(args) {
         try {
             if (args === undefined) return;
@@ -842,22 +824,6 @@
         crossorigin="anonymous"></script>
 
     <script>
-                        /**
-                         * ═══════════════════════════════════════════════════════════════════════════
-                         * UTILITÁRIOS DE FORMATAÇÃO DE VALORES MONETÁRIOS
-                         * ═══════════════════════════════════════════════════════════════════════════
-                         * @@description Funções para conversão entre formato brasileiro(1.234, 56)
-            * e formato numérico JavaScript(1234.56).
-                         */
-
-                        /**
-                         * Extensão do prototype Number para formatação
-                         * @@param { number } n - Casas decimais
-            * @@param { number } x - Grupo de dígitos
-                * @@param { string } s - Separador de milhar
-                    * @@param { string } c - Separador decimal
-                        * @@returns { string } Número formatado
-                            */
         Number.prototype.format = function (n, x, s, c) {
             try {
                 var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
@@ -868,12 +834,6 @@
             }
         };
 
-                        /**
-                         * Converte valor em formato brasileiro para float
-                         * @@description Remove pontos de milhar, troca vírgula por ponto e símbolos de moeda
-            * @@param { string| number} v - Valor a converter(ex: "1.234,56" ou "R$ 1.234,56")
-                * @@returns { number } Valor numérico(ex: 1234.56)
-                    */
         function brToFloat(v) {
             try {
                 if (v == null) return 0;
@@ -887,12 +847,6 @@
             }
         }
 
-                        /**
-                         * Converte valor float para formato brasileiro
-                         * @@description Usa toLocaleString para formatar com 2 casas decimais
-            * @@param { number } v - Valor numérico(ex: 1234.56)
-                * @@returns { string } Valor formatado(ex: "1.234,56")
-                    */
         function floatToBR(v) {
             try {
                 return (isNaN(v) ? 0 : v).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
@@ -902,19 +856,9 @@
             }
         }
 
-                        /**
-                         * ═══════════════════════════════════════════════════════════════════════════
-                         * INICIALIZAÇÃO DO FORMULÁRIO DE CONTRATO
-                         * ═══════════════════════════════════════════════════════════════════════════
-                         * @@description Configura o formulário para modo criação ou edição.
-                         * Carrega repactuações, configura visibilidade de seções
-            * e handlers de eventos para checkboxes de terceirização.
-                         */
         $(document).ready(function () {
             try {
-                                /** @@type { string } ID do contrato sendo editado */
                 const CONTRATO_ID = '@Model.ContratoObj.Contrato.ContratoId';
-                                /** @@type { boolean } True se estiver em modo edição */
                 const isEdit = (CONTRATO_ID && CONTRATO_ID !== '00000000-0000-0000-0000-000000000000');
 
                 document.getElementById("divListaRepactuacoes").style.display = "none";
@@ -976,12 +920,6 @@
                     document.getElementById("chkAtivo").checked = true;
                 }
 
-                            /**
-                             * Handler de mudança do tipo de contrato
-                             * @@description Exibe / oculta seções conforme o tipo selecionado:
-                             * - Terceirização: Mostra checkboxes de funcionários
-                    * - Locação: Mostra grid de veículos
-                        */
                 $('#lstTipoContrato').on('change', function () {
                     try {
                         if (!isEdit) {
@@ -1013,10 +951,6 @@
                     }
                 });
 
-                            /**
-                             * Handler de checkbox - Encarregados
-                             * @@description Habilita / desabilita campos de custo e quantidade
-                    */
                 $('#chkencarregados').on('change', function () {
                     try {
                         if (!document.getElementById("chkencarregados").checked) {
@@ -1031,10 +965,6 @@
                     }
                 });
 
-                            /**
-                             * Handler de checkbox - Operadores
-                             * @@description Habilita / desabilita campos de custo e quantidade
-                    */
                 $('#chkoperadores').on('change', function () {
                     try {
                         if (!document.getElementById("chkoperadores").checked) {
@@ -1049,10 +979,6 @@
                     }
                 });
 
-                            /**
-                             * Handler de checkbox - Motoristas
-                             * @@description Habilita / desabilita campos de custo e quantidade
-                    */
                 $('#chkmotoristas').on('change', function () {
                     try {
                         if (!document.getElementById("chkmotoristas").checked) {
@@ -1067,10 +993,6 @@
                     }
                 });
 
-                            /**
-                             * Handler de checkbox - Lavadores
-                             * @@description Habilita / desabilita campos de custo e quantidade
-                    */
                 $('#chklavadores').on('change', function () {
                     try {
                         if (!document.getElementById("chklavadores").checked) {
@@ -1085,11 +1007,6 @@
                     }
                 });
 
-                            /**
-                             * Handler de input - Objeto do Contrato
-                             * @@description Transforma texto para CamelCase durante digitação
-                    */
-
                 $('#lstObjeto').on('input', function () {
                     try {
                         var cursorPos = this.selectionStart;
@@ -1109,10 +1026,6 @@
             }
         });
 
-                    /**
-                     * Handler do botão Adicionar
-                     * @@description Previne submit padrão e chama InsereRegistro()
-            */
         $("#btnAdiciona").click(function (event) {
             try {
                 event.preventDefault();
@@ -1122,10 +1035,6 @@
             }
         });
 
-                    /**
-                     * Handler do botão Editar
-                     * @@description Previne submit padrão e chama InsereRegistro()
-            */
         $("#btnEdita").click(function (event) {
             try {
                 event.preventDefault();
@@ -1135,23 +1044,8 @@
             }
         });
 
-                    /**
-                     * ═══════════════════════════════════════════════════════════════════════════
-                     * INSERÇÃO/EDIÇÃO DE CONTRATO
-                     * ═══════════════════════════════════════════════════════════════════════════
-                     * @@description Valida todos os campos obrigatórios e envia os dados via AJAX.
-                     * Suporta contratos de Locação(com itens) e Terceirização
-            * (com funcionários).
-                     */
-
-                    /**
-                     * Insere ou atualiza um Contrato
-                     * @@description Valida campos obrigatórios conforme tipo de contrato,
-                     * converte valores monetários e envia via AJAX para a API.
-                     */
         function InsereRegistro() {
             try {
-
                 if (document.getElementById("txtAno").value === "") {
                     Alerta.Erro("Informação Ausente", "O ano do contrato é obrigatório");
                     return;
@@ -1465,21 +1359,6 @@
     </script>
 
     <script>
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * FORMATAÇÃO MONETÁRIA DURANTE DIGITAÇÃO
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 */
-
-                /**
-                 * Formata valor monetário durante digitação
-                 * @@description Aplica máscara monetária brasileira em tempo real
-            * @@param { HTMLInputElement } a - Elemento input sendo editado
-                * @@param { string } e - Separador de milhar(geralmente '.')
-                    * @@param { string } r - Separador decimal(geralmente ',')
-                        * @@param { KeyboardEvent } t - Evento de teclado
-                            * @@returns { boolean } false para prevenir digitação inválida
-                                */
         function moeda(a, e, r, t) {
             try {
                 let n = "", h = j = 0, u = tamanho2 = 0, l = ajd2 = "",
@@ -1502,183 +1381,165 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * HANDLER DE MUDANÇA DE REPACTUAÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Carrega dados da repactuação selecionada conforme o tipo
-            * de contrato(Locação, Terceirização ou Serviços).
-             */
         document.addEventListener('DOMContentLoaded', function () {
             try {
-                    /**
-                     * Callback para ação completa na grid Syncfusion
-                     * @@param { Object } args - Argumentos do evento
-                    */
-                    function actionComplete(args) {
-                        try {
-                            if (args.requestType == "save") {
-                                var gridObj = document.getElementById('grdVeiculos').ej2_instances[0];
-                                gridObj.dataSource.shift();
-                                gridObj.dataSource.push(args.data);
-                                gridObj.refresh();
+                function actionComplete(args) {
+                    try {
+                        if (args.requestType == "save") {
+                            var gridObj = document.getElementById('grdVeiculos').ej2_instances[0];
+                            gridObj.dataSource.shift();
+                            gridObj.dataSource.push(args.data);
+                            gridObj.refresh();
+                        }
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Upsert.cshtml", "actionComplete", error);
+                    }
+                }
+
+                document.getElementById("lstRepactuacao").addEventListener("change", function () {
+                    try {
+                        let RepactuacaoContratoId = document.getElementById("lstRepactuacao").value;
+
+                        if (document.getElementById("lstTipoContrato").value === "Locação") {
+
+                            if (RepactuacaoContratoId === "-- Selecione uma Repactuação --" || RepactuacaoContratoId === "") {
+                                document.getElementById("divVeiculosEdit").style.display = "none";
+                                return;
+                            } else {
+                                document.getElementById("divVeiculosEdit").style.display = "block";
                             }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Upsert.cshtml", "actionComplete", error);
-                        }
-                    }
-
-                    /**
-                     * Handler de mudança do dropdown de repactuação
-                     * @@description Carrega itens / valores conforme o tipo de contrato:
-                     * - Locação: Carrega grid de itens
-                    * - Terceirização: Carrega valores de funcionários
-                        * - Serviços: Carrega valor do serviço
-                            */
-                    document.getElementById("lstRepactuacao").addEventListener("change", function () {
-                                try {
-                                    let RepactuacaoContratoId = document.getElementById("lstRepactuacao").value;
-
-                                    if (document.getElementById("lstTipoContrato").value === "Locação") {
-
-                                        if (RepactuacaoContratoId === "-- Selecione uma Repactuação --" || RepactuacaoContratoId === "") {
-                                            document.getElementById("divVeiculosEdit").style.display = "none";
-                                            return;
-                                        } else {
-                                            document.getElementById("divVeiculosEdit").style.display = "block";
+
+                            var grid = document.getElementById('grdVeiculos2').ej2_instances[0];
+
+                            $.ajax({
+                                url: 'api/GridContrato/DataSource',
+                                type: "GET",
+                                success: function (res) {
+                                    try {
+                                        var objItens = res || [];
+                                        objItens = objItens.filter(function (obj) {
+                                            return obj.repactuacaoId === RepactuacaoContratoId;
+                                        });
+
+                                        grid.dataSource = objItens;
+
+                                        var valortotal = 0;
+                                        for (var i = 0; i < objItens.length; i++) {
+                                            valortotal += (objItens[i].valortotal || 0);
                                         }
-
-                                        var grid = document.getElementById('grdVeiculos2').ej2_instances[0];
-
-                                        $.ajax({
-                                            url: 'api/GridContrato/DataSource',
-                                            type: "GET",
-                                            success: function (res) {
-                                                try {
-                                                    var objItens = res || [];
-                                                    objItens = objItens.filter(function (obj) {
-                                                        return obj.repactuacaoId === RepactuacaoContratoId;
-                                                    });
-
-                                                    grid.dataSource = objItens;
-
-                                                    var valortotal = 0;
-                                                    for (var i = 0; i < objItens.length; i++) {
-                                                        valortotal += (objItens[i].valortotal || 0);
-                                                    }
-                                                    document.getElementById("txtTotalEdit").value = floatToBR(valortotal);
-                                                } catch (error) {
-                                                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "DataSource.success", error);
-                                                }
-                                            },
-                                            error: function (err) {
-                                                Alerta.TratamentoErroComLinha("Upsert.cshtml", "DataSource.error", err);
-                                            }
-                                        });
+                                        document.getElementById("txtTotalEdit").value = floatToBR(valortotal);
+                                    } catch (error) {
+                                        Alerta.TratamentoErroComLinha("Upsert.cshtml", "DataSource.success", error);
                                     }
-
-                                    if (document.getElementById("lstTipoContrato").value === "Terceirização") {
-
-                                        document.getElementById("divTerceirizacao").style.display = "block";
-
-                                        if (RepactuacaoContratoId === "-- Selecione uma Repactuação --" || RepactuacaoContratoId === "") {
-                                            document.getElementById("divTerceirizacao").style.display = "none";
-                                            return;
-                                        }
-
-                                        $.ajax({
-                                            type: "GET",
-                                            url: 'api/Contrato/RecuperaRepactuacaoTerceirizacao',
-                                            data: { RepactuacaoContratoId: RepactuacaoContratoId },
-                                            contentType: "application/json",
-                                            dataType: "json",
-                                            success: function (objRep) {
-                                                try {
-                                                    var o = objRep && objRep.objRepactuacaoTerceirizacao ? objRep.objRepactuacaoTerceirizacao : null;
-                                                    if (!o) return;
-
-                                                    if (o.valorEncarregado != null && o.valorEncarregado != 0) {
-                                                        document.getElementById("chkencarregados").checked = true;
-                                                        document.getElementById("CustoMensalEncarregados").value = floatToBR(o.valorEncarregado);
-                                                        document.getElementById("txtQtdEncarregados").value = o.qtdEncarregados || "";
-                                                    } else {
-                                                        document.getElementById("chkencarregados").checked = false;
-                                                        document.getElementById("CustoMensalEncarregados").value = "";
-                                                        document.getElementById("txtQtdEncarregados").value = "";
-                                                    }
-
-                                                    if (o.valorOperador != null && o.valorOperador != 0) {
-                                                        document.getElementById("chkoperadores").checked = true;
-                                                        document.getElementById("CustoMensalOperadores").value = floatToBR(o.valorOperador);
-                                                        document.getElementById("txtQtdOperadores").value = o.qtdOperadores || "";
-                                                    } else {
-                                                        document.getElementById("chkoperadores").checked = false;
-                                                        document.getElementById("CustoMensalOperadores").value = "";
-                                                        document.getElementById("txtQtdOperadores").value = "";
-                                                    }
-
-                                                    if (o.valorMotorista != null && o.valorMotorista != 0) {
-                                                        document.getElementById("chkmotoristas").checked = true;
-                                                        document.getElementById("CustoMensalMotoristas").value = floatToBR(o.valorMotorista);
-                                                        document.getElementById("txtQtdMotoristas").value = o.qtdMotoristas || "";
-                                                    } else {
-                                                        document.getElementById("chkmotoristas").checked = false;
-                                                        document.getElementById("CustoMensalMotoristas").value = "";
-                                                        document.getElementById("txtQtdMotoristas").value = "";
-                                                    }
-
-                                                    if (o.valorLavador != null && o.valorLavador != 0) {
-                                                        document.getElementById("chklavadores").checked = true;
-                                                        document.getElementById("CustoMensalLavadores").value = floatToBR(o.valorLavador);
-                                                        document.getElementById("txtQtdLavadores").value = o.qtdLavadores || "";
-                                                    } else {
-                                                        document.getElementById("chklavadores").checked = false;
-                                                        document.getElementById("CustoMensalLavadores").value = "";
-                                                        document.getElementById("txtQtdLavadores").value = "";
-                                                    }
-
-                                                } catch (error) {
-                                                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "RecuperaTerceirizacao.success", error);
-                                                }
-                                            },
-                                            error: function (data) {
-                                                Alerta.TratamentoErroComLinha("Upsert.cshtml", "RecuperaTerceirizacao.error", data);
-                                            }
-                                        });
-
-                                    }
-
-                                    if (document.getElementById("lstTipoContrato").value === "Serviços") {
-
-                                        if (RepactuacaoContratoId === "-- Selecione uma Repactuação --" || RepactuacaoContratoId === "") {
-                                            document.getElementById("valor").value = floatToBR(0);
-                                            return;
-                                        }
-
-                                        $.ajax({
-                                            type: "GET",
-                                            url: 'api/Contrato/RecuperaRepactuacaoServicos',
-                                            data: { RepactuacaoContratoId: RepactuacaoContratoId },
-                                            contentType: "application/json",
-                                            dataType: "json",
-                                            success: function (data) {
-                                                try {
-                                                    var v = (data && data.data && data.data.valor) ? data.data.valor : 0;
-                                                    document.getElementById("valor").value = floatToBR(brToFloat(v));
-                                                } catch (error) {
-                                                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "RecuperaServicos.success", error);
-                                                }
-                                            },
-                                            error: function (data) {
-                                                Alerta.TratamentoErroComLinha("Upsert.cshtml", "RecuperaServicos.error", data);
-                                            }
-                                        });
-                                    }
-
-                                } catch (error) {
-                                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "lstRepactuacao.change", error);
+                                },
+                                error: function (err) {
+                                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "DataSource.error", err);
                                 }
                             });
+                        }
+
+                        if (document.getElementById("lstTipoContrato").value === "Terceirização") {
+
+                            document.getElementById("divTerceirizacao").style.display = "block";
+
+                            if (RepactuacaoContratoId === "-- Selecione uma Repactuação --" || RepactuacaoContratoId === "") {
+                                document.getElementById("divTerceirizacao").style.display = "none";
+                                return;
+                            }
+
+                            $.ajax({
+                                type: "GET",
+                                url: 'api/Contrato/RecuperaRepactuacaoTerceirizacao',
+                                data: { RepactuacaoContratoId: RepactuacaoContratoId },
+                                contentType: "application/json",
+                                dataType: "json",
+                                success: function (objRep) {
+                                    try {
+                                        var o = objRep && objRep.objRepactuacaoTerceirizacao ? objRep.objRepactuacaoTerceirizacao : null;
+                                        if (!o) return;
+
+                                        if (o.valorEncarregado != null && o.valorEncarregado != 0) {
+                                            document.getElementById("chkencarregados").checked = true;
+                                            document.getElementById("CustoMensalEncarregados").value = floatToBR(o.valorEncarregado);
+                                            document.getElementById("txtQtdEncarregados").value = o.qtdEncarregados || "";
+                                        } else {
+                                            document.getElementById("chkencarregados").checked = false;
+                                            document.getElementById("CustoMensalEncarregados").value = "";
+                                            document.getElementById("txtQtdEncarregados").value = "";
+                                        }
+
+                                        if (o.valorOperador != null && o.valorOperador != 0) {
+                                            document.getElementById("chkoperadores").checked = true;
+                                            document.getElementById("CustoMensalOperadores").value = floatToBR(o.valorOperador);
+                                            document.getElementById("txtQtdOperadores").value = o.qtdOperadores || "";
+                                        } else {
+                                            document.getElementById("chkoperadores").checked = false;
+                                            document.getElementById("CustoMensalOperadores").value = "";
+                                            document.getElementById("txtQtdOperadores").value = "";
+                                        }
+
+                                        if (o.valorMotorista != null && o.valorMotorista != 0) {
+                                            document.getElementById("chkmotoristas").checked = true;
+                                            document.getElementById("CustoMensalMotoristas").value = floatToBR(o.valorMotorista);
+                                            document.getElementById("txtQtdMotoristas").value = o.qtdMotoristas || "";
+                                        } else {
+                                            document.getElementById("chkmotoristas").checked = false;
+                                            document.getElementById("CustoMensalMotoristas").value = "";
+                                            document.getElementById("txtQtdMotoristas").value = "";
+                                        }
+
+                                        if (o.valorLavador != null && o.valorLavador != 0) {
+                                            document.getElementById("chklavadores").checked = true;
+                                            document.getElementById("CustoMensalLavadores").value = floatToBR(o.valorLavador);
+                                            document.getElementById("txtQtdLavadores").value = o.qtdLavadores || "";
+                                        } else {
+                                            document.getElementById("chklavadores").checked = false;
+                                            document.getElementById("CustoMensalLavadores").value = "";
+                                            document.getElementById("txtQtdLavadores").value = "";
+                                        }
+
+                                    } catch (error) {
+                                        Alerta.TratamentoErroComLinha("Upsert.cshtml", "RecuperaTerceirizacao.success", error);
+                                    }
+                                },
+                                error: function (data) {
+                                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "RecuperaTerceirizacao.error", data);
+                                }
+                            });
+
+                        }
+
+                        if (document.getElementById("lstTipoContrato").value === "Serviços") {
+
+                            if (RepactuacaoContratoId === "-- Selecione uma Repactuação --" || RepactuacaoContratoId === "") {
+                                document.getElementById("valor").value = floatToBR(0);
+                                return;
+                            }
+
+                            $.ajax({
+                                type: "GET",
+                                url: 'api/Contrato/RecuperaRepactuacaoServicos',
+                                data: { RepactuacaoContratoId: RepactuacaoContratoId },
+                                contentType: "application/json",
+                                dataType: "json",
+                                success: function (data) {
+                                    try {
+                                        var v = (data && data.data && data.data.valor) ? data.data.valor : 0;
+                                        document.getElementById("valor").value = floatToBR(brToFloat(v));
+                                    } catch (error) {
+                                        Alerta.TratamentoErroComLinha("Upsert.cshtml", "RecuperaServicos.success", error);
+                                    }
+                                },
+                                error: function (data) {
+                                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "RecuperaServicos.error", data);
+                                }
+                            });
+                        }
+
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Upsert.cshtml", "lstRepactuacao.change", error);
+                    }
+                });
             } catch (error) {
                 Alerta.TratamentoErroComLinha("Upsert.cshtml", "DOMContentLoaded", error);
             }
@@ -1686,18 +1547,9 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * ATUALIZAÇÃO DO VALOR TOTAL DO CONTRATO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Recalcula o valor total somando todos os itens da grid.
-             */
         var btnAtualiza = document.getElementById("btnAtualizaTotal");
 
         if (btnAtualiza) {
-            /**
-             * Handler do botão Atualizar Total
-             */
             btnAtualiza.addEventListener("click", function (event) {
                 try {
                     event.preventDefault();
@@ -1708,11 +1560,6 @@
             });
         }
 
-            /**
-             * Recalcula o valor total do contrato
-             * @@description Soma quantidade x valor unitário de todos os itens da grid
-            * e atualiza o campo txtTotal com o valor formatado.
-             */
         function AtualizaTotal() {
             try {
                 var gridObj = document.getElementById('grdVeiculos').ej2_instances[0];
```
