# wwwroot/js/cadastros/veiculo_upsert.js

**Mudanca:** GRANDE | **+95** linhas | **-276** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/veiculo_upsert.js
+++ ATUAL: wwwroot/js/cadastros/veiculo_upsert.js
@@ -1,5 +1,5 @@
 (function () {
-    'use strict';
+    "use strict";
 
     $(document).ready(function () {
         try {
@@ -9,10 +9,7 @@
             setupEventListeners();
 
             var veiculoId = $('#VeiculoObj_Veiculo_VeiculoId').val();
-            if (
-                veiculoId &&
-                veiculoId !== '00000000-0000-0000-0000-000000000000'
-            ) {
+            if (veiculoId && veiculoId !== '00000000-0000-0000-0000-000000000000') {
                 var marcaId = $('#listamarca').val();
                 if (marcaId) {
                     GetModeloList(marcaId);
@@ -29,11 +26,7 @@
                 }
             }
         } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'veiculo_upsert.js',
-                'document.ready',
-                error,
-            );
+            Alerta.TratamentoErroComLinha("veiculo_upsert.js", "document.ready", error);
         }
     });
 
@@ -61,11 +54,7 @@
                 $('#lstItemVeiculoAta').show();
             }
         } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'veiculo_upsert.js',
-                'inicializarCampos',
-                error,
-            );
+            Alerta.TratamentoErroComLinha("veiculo_upsert.js", "inicializarCampos", error);
         }
     }
 
@@ -76,11 +65,7 @@
                 try {
                     $('#inputCRLV').click();
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'veiculo_upsert.js',
-                        'btnUploadCRLV.click',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("veiculo_upsert.js", "btnUploadCRLV.click", error);
                 }
             });
 
@@ -90,34 +75,19 @@
                     if (file) {
 
                         if (file.size > 10 * 1024 * 1024) {
-                            AppToast.show(
-                                'Vermelho',
-                                'Arquivo muito grande. Máximo: 10MB',
-                                3000,
-                            );
+                            AppToast.show('Vermelho', 'Arquivo muito grande. Máximo: 10MB', 3000);
                             $(this).val('');
                             return;
                         }
 
-                        var extensoesValidas = [
-                            '.pdf',
-                            '.jpg',
-                            '.jpeg',
-                            '.png',
-                        ];
+                        var extensoesValidas = ['.pdf', '.jpg', '.jpeg', '.png'];
                         var nomeArquivo = file.name.toLowerCase();
-                        var extensaoValida = extensoesValidas.some(
-                            function (ext) {
-                                return nomeArquivo.endsWith(ext);
-                            },
-                        );
+                        var extensaoValida = extensoesValidas.some(function (ext) {
+                            return nomeArquivo.endsWith(ext);
+                        });
 
                         if (!extensaoValida) {
-                            AppToast.show(
-                                'Vermelho',
-                                'Formato inválido. Use PDF, JPG ou PNG',
-                                3000,
-                            );
+                            AppToast.show('Vermelho', 'Formato inválido. Use PDF, JPG ou PNG', 3000);
                             $(this).val('');
                             return;
                         }
@@ -126,18 +96,10 @@
                         $('#infoCRLVNovo').show();
                         $('#txtBtnCRLV').text('Substituir CRLV');
 
-                        AppToast.show(
-                            'Verde',
-                            'Arquivo selecionado: ' + file.name,
-                            2000,
-                        );
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'veiculo_upsert.js',
-                        'inputCRLV.change',
-                        error,
-                    );
+                        AppToast.show('Verde', 'Arquivo selecionado: ' + file.name, 2000);
+                    }
+                } catch (error) {
+                    Alerta.TratamentoErroComLinha("veiculo_upsert.js", "inputCRLV.change", error);
                 }
             });
 
@@ -147,17 +109,11 @@
                     $('#infoCRLVNovo').hide();
 
                     var temCRLVExistente = $('#infoCRLVExistente').length > 0;
-                    $('#txtBtnCRLV').text(
-                        temCRLVExistente ? 'Substituir CRLV' : 'Upload do CRLV',
-                    );
+                    $('#txtBtnCRLV').text(temCRLVExistente ? 'Substituir CRLV' : 'Upload do CRLV');
 
                     AppToast.show('Amarelo', 'Arquivo removido', 2000);
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'veiculo_upsert.js',
-                        'btnRemoverCRLV.click',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("veiculo_upsert.js", "btnRemoverCRLV.click", error);
                 }
             });
 
@@ -168,11 +124,7 @@
                         GetModeloList(id);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'veiculo_upsert.js',
-                        'listamarca.change',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("veiculo_upsert.js", "listamarca.change", error);
                 }
             });
 
@@ -198,11 +150,7 @@
                         $('#lstItemVeiculo').hide().val('');
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'veiculo_upsert.js',
-                        'lstcontratos.change',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("veiculo_upsert.js", "lstcontratos.change", error);
                 }
             });
 
@@ -228,11 +176,7 @@
                         $('#lstItemVeiculoAta').hide().val('');
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'veiculo_upsert.js',
-                        'lstatas.change',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("veiculo_upsert.js", "lstatas.change", error);
                 }
             });
 
@@ -241,11 +185,7 @@
                     var isChecked = $(this).is(':checked');
                     toggleCamposVeiculoProprio(isChecked);
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'veiculo_upsert.js',
-                        'chkVeiculoProprio.change',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("veiculo_upsert.js", "chkVeiculoProprio.change", error);
                 }
             });
 
@@ -255,10 +195,7 @@
 
                     if (placa) {
 
-                        placa = placa
-                            .replace(/\s+/g, '')
-                            .replace(/-/g, '')
-                            .toUpperCase();
+                        placa = placa.replace(/\s+/g, '').replace(/-/g, '').toUpperCase();
                         $(this).val(placa);
 
                         if (placa.length >= 4) {
@@ -267,11 +204,7 @@
                         }
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'veiculo_upsert.js',
-                        'txtPlaca.focusout',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("veiculo_upsert.js", "txtPlaca.focusout", error);
                 }
             });
 
@@ -286,21 +219,14 @@
                     console.log('Validação OK - permitindo submit');
                     return true;
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'veiculo_upsert.js',
-                        'form.submit',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("veiculo_upsert.js", "form.submit", error);
                     e.preventDefault();
                     return false;
                 }
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'veiculo_upsert.js',
-                'setupEventListeners',
-                error,
-            );
+
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("veiculo_upsert.js", "setupEventListeners", error);
         }
     }
 
@@ -329,33 +255,23 @@
                 $('#lstatas').prop('disabled', false);
             }
         } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'veiculo_upsert.js',
-                'toggleCamposVeiculoProprio',
-                error,
-            );
+            Alerta.TratamentoErroComLinha("veiculo_upsert.js", "toggleCamposVeiculoProprio", error);
         }
     }
 
     function GetModeloList(marcaId) {
         try {
             $.ajax({
-                url: '/Veiculo/Upsert?handler=ModeloList',
-                method: 'GET',
+                url: "/Veiculo/Upsert?handler=ModeloList",
+                method: "GET",
                 data: { id: marcaId },
                 success: function (res) {
                     try {
-                        var options =
-                            '<option value="">-- Selecione um Modelo --</option>';
+                        var options = '<option value="">-- Selecione um Modelo --</option>';
 
                         if (res && res.data && res.data.length) {
                             res.data.forEach(function (obj) {
-                                options +=
-                                    '<option value="' +
-                                    obj.modeloId +
-                                    '">' +
-                                    obj.descricaoModelo +
-                                    '</option>';
+                                options += '<option value="' + obj.modeloId + '">' + obj.descricaoModelo + '</option>';
                             });
                         }
 
@@ -366,58 +282,36 @@
                             $('#ModeloId').val(modeloIdSelecionado);
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'veiculo_upsert.js',
-                            'GetModeloList.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("veiculo_upsert.js", "GetModeloList.success", error);
                     }
                 },
                 error: function (xhr) {
                     try {
-                        console.error('Erro ao carregar modelos:', xhr);
-                        AppToast.show(
-                            'Erro ao carregar modelos',
-                            'Vermelho',
-                            2000,
-                        );
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'veiculo_upsert.js',
-                            'GetModeloList.error',
-                            error,
-                        );
-                    }
-                },
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'veiculo_upsert.js',
-                'GetModeloList',
-                error,
-            );
+                        console.error("Erro ao carregar modelos:", xhr);
+                        AppToast.show('Erro ao carregar modelos', 'Vermelho', 2000);
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("veiculo_upsert.js", "GetModeloList.error", error);
+                    }
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("veiculo_upsert.js", "GetModeloList", error);
         }
     }
 
     function GetItemContratualList(contratoId) {
         try {
             $.ajax({
-                url: '/Veiculo/Upsert?handler=ItemContratual',
-                method: 'GET',
+                url: "/Veiculo/Upsert?handler=ItemContratual",
+                method: "GET",
                 data: { id: contratoId },
                 success: function (res) {
                     try {
-                        var options =
-                            '<option value="">-- Selecione um Item Contratual --</option>';
+                        var options = '<option value="">-- Selecione um Item Contratual --</option>';
 
                         if (res && res.data && res.data.length) {
                             res.data.forEach(function (obj) {
-                                options +=
-                                    '<option value="' +
-                                    obj.itemVeiculoId +
-                                    '">' +
-                                    obj.descricao +
-                                    '</option>';
+                                options += '<option value="' + obj.itemVeiculoId + '">' + obj.descricao + '</option>';
                             });
                         }
 
@@ -428,147 +322,93 @@
                             $('#lstItemVeiculo').val(itemIdSelecionado);
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'veiculo_upsert.js',
-                            'GetItemContratualList.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("veiculo_upsert.js", "GetItemContratualList.success", error);
                     }
                 },
                 error: function (xhr) {
                     try {
-                        console.error(
-                            'Erro ao carregar itens contratuais:',
-                            xhr,
-                        );
-                        AppToast.show(
-                            'Erro ao carregar itens contratuais',
-                            'Vermelho',
-                            2000,
-                        );
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'veiculo_upsert.js',
-                            'GetItemContratualList.error',
-                            error,
-                        );
-                    }
-                },
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'veiculo_upsert.js',
-                'GetItemContratualList',
-                error,
-            );
+                        console.error("Erro ao carregar itens contratuais:", xhr);
+                        AppToast.show('Erro ao carregar itens contratuais', 'Vermelho', 2000);
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("veiculo_upsert.js", "GetItemContratualList.error", error);
+                    }
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("veiculo_upsert.js", "GetItemContratualList", error);
         }
     }
 
     function GetItemAtaList(ataId) {
         try {
             $.ajax({
-                url: '/Veiculo/Upsert?handler=ItemAta',
-                method: 'GET',
+                url: "/Veiculo/Upsert?handler=ItemAta",
+                method: "GET",
                 data: { id: ataId },
                 success: function (res) {
                     try {
-                        var options =
-                            '<option value="">-- Selecione um Item da Ata --</option>';
+                        var options = '<option value="">-- Selecione um Item da Ata --</option>';
 
                         if (res && res.data && res.data.length) {
                             res.data.forEach(function (obj) {
-                                options +=
-                                    '<option value="' +
-                                    obj.itemVeiculoAtaId +
-                                    '">' +
-                                    obj.descricao +
-                                    '</option>';
+                                options += '<option value="' + obj.itemVeiculoAtaId + '">' + obj.descricao + '</option>';
                             });
                         }
 
                         $('#lstItemVeiculoAta').html(options);
 
-                        var itemAtaIdSelecionado =
-                            $('#Veiculo_ItemAtaId').val();
+                        var itemAtaIdSelecionado = $('#Veiculo_ItemAtaId').val();
                         if (itemAtaIdSelecionado) {
                             $('#lstItemVeiculoAta').val(itemAtaIdSelecionado);
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'veiculo_upsert.js',
-                            'GetItemAtaList.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("veiculo_upsert.js", "GetItemAtaList.success", error);
                     }
                 },
                 error: function (xhr) {
                     try {
-                        console.error('Erro ao carregar itens da ata:', xhr);
-                        AppToast.show(
-                            'Erro ao carregar itens da ata',
-                            'Vermelho',
-                            2000,
-                        );
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'veiculo_upsert.js',
-                            'GetItemAtaList.error',
-                            error,
-                        );
-                    }
-                },
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'veiculo_upsert.js',
-                'GetItemAtaList',
-                error,
-            );
+                        console.error("Erro ao carregar itens da ata:", xhr);
+                        AppToast.show('Erro ao carregar itens da ata', 'Vermelho', 2000);
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("veiculo_upsert.js", "GetItemAtaList.error", error);
+                    }
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("veiculo_upsert.js", "GetItemAtaList", error);
         }
     }
 
     function verificarPlacaExistente(ultimos4Digitos) {
         try {
             $.ajax({
-                url: '/Veiculo/Upsert?handler=VerificaPlaca',
-                method: 'GET',
-                datatype: 'json',
+                url: "/Veiculo/Upsert?handler=VerificaPlaca",
+                method: "GET",
+                datatype: "json",
                 data: { id: ultimos4Digitos },
                 success: function (res) {
                     try {
-                        if (res && res.data === 'Existe Placa') {
+                        if (res && res.data === "Existe Placa") {
                             Alerta.Warning(
-                                'Alerta na Placa',
-                                'Já existe uma Placa contendo esses valores!',
-                                'Ok',
+                                "Alerta na Placa",
+                                "Já existe uma Placa contendo esses valores!",
+                                "Ok"
                             );
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'veiculo_upsert.js',
-                            'verificarPlacaExistente.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("veiculo_upsert.js", "verificarPlacaExistente.success", error);
                     }
                 },
                 error: function (xhr) {
                     try {
-                        console.error('Erro ao verificar placa:', xhr);
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'veiculo_upsert.js',
-                            'verificarPlacaExistente.error',
-                            error,
-                        );
-                    }
-                },
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'veiculo_upsert.js',
-                'verificarPlacaExistente',
-                error,
-            );
+                        console.error("Erro ao verificar placa:", xhr);
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("veiculo_upsert.js", "verificarPlacaExistente.error", error);
+                    }
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("veiculo_upsert.js", "verificarPlacaExistente", error);
         }
     }
 
@@ -616,42 +456,26 @@
             var veiculoProprio = $('#chkVeiculoProprio').is(':checked');
 
             if (!contratoId && !ataId && !veiculoProprio) {
-                camposErro.push(
-                    'Contrato, Ata ou Veículo Próprio (escolha ao menos um)',
-                );
-            }
-
-            if (
-                contratoId &&
-                $('#lstItemVeiculo').is(':visible') &&
-                !$('#lstItemVeiculo').val()
-            ) {
-                camposErro.push(
-                    'Item Contratual (obrigatório quando há Contrato)',
-                );
-            }
-
-            if (
-                ataId &&
-                $('#lstItemVeiculoAta').is(':visible') &&
-                !$('#lstItemVeiculoAta').val()
-            ) {
+                camposErro.push('Contrato, Ata ou Veículo Próprio (escolha ao menos um)');
+            }
+
+            if (contratoId && $('#lstItemVeiculo').is(':visible') && !$('#lstItemVeiculo').val()) {
+                camposErro.push('Item Contratual (obrigatório quando há Contrato)');
+            }
+
+            if (ataId && $('#lstItemVeiculoAta').is(':visible') && !$('#lstItemVeiculoAta').val()) {
                 camposErro.push('Item da Ata (obrigatório quando há Ata)');
             }
 
             if (veiculoProprio && $('#txtPatrimonio').is(':visible')) {
                 var patrimonio = $('#txtPatrimonio').val();
                 if (!patrimonio || patrimonio.trim() === '') {
-                    camposErro.push(
-                        'Nº Patrimônio (obrigatório para Veículo Próprio)',
-                    );
+                    camposErro.push('Nº Patrimônio (obrigatório para Veículo Próprio)');
                 }
             }
 
             if (camposErro.length > 0) {
-                var mensagem =
-                    'Campos obrigatórios não preenchidos:\n\n• ' +
-                    camposErro.join('\n• ');
+                var mensagem = 'Campos obrigatórios não preenchidos:\n\n• ' + camposErro.join('\n• ');
                 console.log('Validação falhou. Campos:', camposErro);
                 Alerta.Warning('Validação de Campos', mensagem, 'Ok');
                 return false;
@@ -660,12 +484,9 @@
             console.log('Validação passou! Submetendo formulário...');
             return true;
         } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'veiculo_upsert.js',
-                'validarCamposObrigatorios',
-                error,
-            );
+            Alerta.TratamentoErroComLinha("veiculo_upsert.js", "validarCamposObrigatorios", error);
             return false;
         }
     }
+
 })();
```
