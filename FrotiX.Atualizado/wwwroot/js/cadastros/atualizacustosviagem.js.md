# wwwroot/js/cadastros/atualizacustosviagem.js

**Mudanca:** GRANDE | **+246** linhas | **-418** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/atualizacustosviagem.js
+++ ATUAL: wwwroot/js/cadastros/atualizacustosviagem.js
@@ -5,337 +5,233 @@
     try {
         inicializarModais();
         initDataTable();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'document.ready',
-            error,
-        );
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "document.ready", error);
     }
 });
 
 function inicializarModais() {
     try {
 
-        const modalAjustaCustosEl =
-            document.getElementById('modalAjustaCustos');
+        const modalAjustaCustosEl = document.getElementById("modalAjustaCustos");
         if (modalAjustaCustosEl) {
             modalAjustaCustos = new bootstrap.Modal(modalAjustaCustosEl, {
                 keyboard: true,
-                backdrop: 'static',
+                backdrop: "static"
             });
 
-            modalAjustaCustosEl.addEventListener(
-                'show.bs.modal',
-                function (event) {
-                    try {
-                        const button = event.relatedTarget;
-                        if (button) {
-                            const viagemId = button.getAttribute('data-id');
-                            if (viagemId) {
-                                carregarDadosViagem(viagemId);
-                            }
-                        }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'atualizacustosviagem.js',
-                            'modalAjustaCustos.show',
-                            error,
-                        );
-                    }
-                },
-            );
-        }
-
-        const modalFichaEl = document.getElementById('modalFicha');
+            modalAjustaCustosEl.addEventListener("show.bs.modal", function (event) {
+                try {
+                    const button = event.relatedTarget;
+                    if (button) {
+                        const viagemId = button.getAttribute("data-id");
+                        if (viagemId) {
+                            carregarDadosViagem(viagemId);
+                        }
+                    }
+                }
+                catch (error) {
+                    Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "modalAjustaCustos.show", error);
+                }
+            });
+        }
+
+        const modalFichaEl = document.getElementById("modalFicha");
         if (modalFichaEl) {
             modalFicha = new bootstrap.Modal(modalFichaEl, {
                 keyboard: true,
-                backdrop: 'static',
+                backdrop: "static"
             });
 
-            modalFichaEl.addEventListener('show.bs.modal', function (event) {
+            modalFichaEl.addEventListener("show.bs.modal", function (event) {
                 try {
                     const button = event.relatedTarget;
                     if (button) {
-                        const viagemId = button.getAttribute('data-id');
+                        const viagemId = button.getAttribute("data-id");
                         if (viagemId) {
-                            document.getElementById('txtViagemId').value =
-                                viagemId;
+                            document.getElementById("txtViagemId").value = viagemId;
                             carregarFichaVistoria(viagemId, button);
                         }
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'atualizacustosviagem.js',
-                        'modalFicha.show',
-                        error,
-                    );
+                }
+                catch (error) {
+                    Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "modalFicha.show", error);
                 }
             });
         }
 
-        const btnAjustarViagem = document.getElementById('btnAjustarViagem');
+        const btnAjustarViagem = document.getElementById("btnAjustarViagem");
         if (btnAjustarViagem) {
-            btnAjustarViagem.addEventListener('click', function () {
+            btnAjustarViagem.addEventListener("click", function () {
                 gravarViagem();
             });
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'inicializarModais',
-            error,
-        );
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "inicializarModais", error);
     }
 }
 
 function mostrarLoading(mensagem) {
     try {
         if (mensagem) {
-            document.getElementById('txtLoadingMessage').textContent = mensagem;
-        }
-        const overlay = document.getElementById('loadingOverlayCustos');
+            document.getElementById("txtLoadingMessage").textContent = mensagem;
+        }
+        const overlay = document.getElementById("loadingOverlayCustos");
         if (overlay) {
-            overlay.style.display = 'flex';
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'mostrarLoading',
-            error,
-        );
+            overlay.style.display = "flex";
+        }
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "mostrarLoading", error);
     }
 }
 
 function esconderLoading() {
     try {
-        const overlay = document.getElementById('loadingOverlayCustos');
+        const overlay = document.getElementById("loadingOverlayCustos");
         if (overlay) {
-            overlay.style.display = 'none';
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'esconderLoading',
-            error,
-        );
+            overlay.style.display = "none";
+        }
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "esconderLoading", error);
     }
 }
 
 function carregarDadosViagem(viagemId) {
     try {
         $.ajax({
-            type: 'GET',
-            url: '/api/Viagem/GetViagem/' + viagemId,
+            type: "GET",
+            url: "/api/Viagem/GetViagem/" + viagemId,
             success: function (res) {
                 try {
                     if (res && res.success && res.data) {
                         const viagem = res.data;
 
-                        document.getElementById('txtId').value =
-                            viagem.viagemId;
-                        document.getElementById('txtNoFichaVistoria').value =
-                            viagem.noFichaVistoria || '';
-
-                        const lstFinalidade = document.getElementById(
-                            'lstFinalidadeAlterada',
-                        );
+                        document.getElementById("txtId").value = viagem.viagemId;
+                        document.getElementById("txtNoFichaVistoria").value = viagem.noFichaVistoria || "";
+
+                        const lstFinalidade = document.getElementById("lstFinalidadeAlterada");
                         if (lstFinalidade && lstFinalidade.ej2_instances) {
-                            lstFinalidade.ej2_instances[0].value =
-                                viagem.finalidade || null;
-                        }
-
-                        const lstEvento = document.getElementById('lstEvento');
+                            lstFinalidade.ej2_instances[0].value = viagem.finalidade || null;
+                        }
+
+                        const lstEvento = document.getElementById("lstEvento");
                         if (lstEvento && lstEvento.ej2_instances) {
-                            if (
-                                viagem.finalidade === 'Evento' &&
-                                viagem.eventoId
-                            ) {
+                            if (viagem.finalidade === "Evento" && viagem.eventoId) {
                                 lstEvento.ej2_instances[0].enabled = true;
-                                lstEvento.ej2_instances[0].value = [
-                                    viagem.eventoId.toString(),
-                                ];
-                                $('.esconde-diveventos').show();
+                                lstEvento.ej2_instances[0].value = [viagem.eventoId.toString()];
+                                $(".esconde-diveventos").show();
                             } else {
                                 lstEvento.ej2_instances[0].enabled = false;
                                 lstEvento.ej2_instances[0].value = null;
-                                $('.esconde-diveventos').hide();
+                                $(".esconde-diveventos").hide();
                             }
                         }
 
-                        document.getElementById('txtDataInicial').value =
-                            viagem.dataInicial || '';
-                        document.getElementById('txtHoraInicial').value =
-                            viagem.horaInicio || '';
-                        document.getElementById('txtDataFinal').value =
-                            viagem.dataFinal || '';
-                        document.getElementById('txtHoraFinal').value =
-                            viagem.horaFim || '';
-
-                        document.getElementById('txtKmInicial').value =
-                            viagem.kmInicial || '';
-                        document.getElementById('txtKmFinal').value =
-                            viagem.kmFinal || '';
-
-                        document.getElementById('txtRamalRequisitante').value =
-                            viagem.ramalRequisitante || '';
-
-                        setTimeout(function () {
+                        document.getElementById("txtDataInicial").value = viagem.dataInicial || "";
+                        document.getElementById("txtHoraInicial").value = viagem.horaInicio || "";
+                        document.getElementById("txtDataFinal").value = viagem.dataFinal || "";
+                        document.getElementById("txtHoraFinal").value = viagem.horaFim || "";
+
+                        document.getElementById("txtKmInicial").value = viagem.kmInicial || "";
+                        document.getElementById("txtKmFinal").value = viagem.kmFinal || "";
+
+                        document.getElementById("txtRamalRequisitante").value = viagem.ramalRequisitante || "";
+
+                        setTimeout(function() {
                             try {
 
-                                const lstMotorista = document.getElementById(
-                                    'lstMotoristaAlterado',
-                                );
-                                if (
-                                    lstMotorista &&
-                                    lstMotorista.ej2_instances &&
-                                    viagem.motoristaId
-                                ) {
-                                    lstMotorista.ej2_instances[0].value =
-                                        viagem.motoristaId;
+                                const lstMotorista = document.getElementById("lstMotoristaAlterado");
+                                if (lstMotorista && lstMotorista.ej2_instances && viagem.motoristaId) {
+                                    lstMotorista.ej2_instances[0].value = viagem.motoristaId;
                                 }
 
-                                const lstVeiculo =
-                                    document.getElementById(
-                                        'lstVeiculoAlterado',
-                                    );
-                                if (
-                                    lstVeiculo &&
-                                    lstVeiculo.ej2_instances &&
-                                    viagem.veiculoId
-                                ) {
-                                    lstVeiculo.ej2_instances[0].value =
-                                        viagem.veiculoId;
+                                const lstVeiculo = document.getElementById("lstVeiculoAlterado");
+                                if (lstVeiculo && lstVeiculo.ej2_instances && viagem.veiculoId) {
+                                    lstVeiculo.ej2_instances[0].value = viagem.veiculoId;
                                 }
 
-                                const lstRequisitante = document.getElementById(
-                                    'lstRequisitanteAlterado',
-                                );
-                                if (
-                                    lstRequisitante &&
-                                    lstRequisitante.ej2_instances &&
-                                    viagem.requisitanteId
-                                ) {
-                                    lstRequisitante.ej2_instances[0].value =
-                                        viagem.requisitanteId;
+                                const lstRequisitante = document.getElementById("lstRequisitanteAlterado");
+                                if (lstRequisitante && lstRequisitante.ej2_instances && viagem.requisitanteId) {
+                                    lstRequisitante.ej2_instances[0].value = viagem.requisitanteId;
                                 }
 
-                                const lstSetor = document.getElementById(
-                                    'lstSetorSolicitanteAlterado',
-                                );
-                                if (
-                                    lstSetor &&
-                                    lstSetor.ej2_instances &&
-                                    viagem.setorSolicitanteId
-                                ) {
-                                    lstSetor.ej2_instances[0].value = [
-                                        viagem.setorSolicitanteId,
-                                    ];
+                                const lstSetor = document.getElementById("lstSetorSolicitanteAlterado");
+                                if (lstSetor && lstSetor.ej2_instances && viagem.setorSolicitanteId) {
+                                    lstSetor.ej2_instances[0].value = [viagem.setorSolicitanteId];
                                 }
                             } catch (error) {
-                                console.error(
-                                    'Erro ao setar valores dos combos:',
-                                    error,
-                                );
+                                console.error("Erro ao setar valores dos combos:", error);
                             }
                         }, 300);
+
                     } else {
-                        AppToast.show(
-                            'Amarelo',
-                            res.message || 'Viagem não encontrada',
-                            3000,
-                        );
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'atualizacustosviagem.js',
-                        'carregarDadosViagem.success',
-                        error,
-                    );
+                        AppToast.show("Amarelo", res.message || "Viagem não encontrada", 3000);
+                    }
+                }
+                catch (error) {
+                    Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "carregarDadosViagem.success", error);
                 }
             },
             error: function (xhr, status, error) {
-                Alerta.TratamentoErroComLinha(
-                    'atualizacustosviagem.js',
-                    'carregarDadosViagem.error',
-                    error,
-                );
-            },
+                Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "carregarDadosViagem.error", error);
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'carregarDadosViagem',
-            error,
-        );
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "carregarDadosViagem", error);
     }
 }
 
 function carregarFichaVistoria(viagemId, button) {
     try {
-        const labelEl = document.getElementById('DynamicModalLabel');
-        const imgViewer = document.getElementById('imgViewer');
-
-        const fichaVistoria = $(button).closest('tr').find('td:eq(0)').text();
+        const labelEl = document.getElementById("DynamicModalLabel");
+        const imgViewer = document.getElementById("imgViewer");
+
+        const fichaVistoria = $(button).closest("tr").find("td:eq(0)").text();
 
         $.ajax({
-            type: 'GET',
-            url: '/api/Viagem/PegaFichaModal',
+            type: "GET",
+            url: "/api/Viagem/PegaFichaModal",
             data: { id: viagemId },
             success: function (res) {
                 try {
-                    imgViewer.removeAttribute('src');
-
-                    if (res === false || res === null || res === '') {
-                        labelEl.innerHTML =
-                            '<i class="fa-duotone fa-file-image me-2"></i>Viagem sem Ficha de Vistoria Digitalizada';
-                        imgViewer.src = '/Images/FichaAmarelaNova.jpg';
+                    imgViewer.removeAttribute("src");
+
+                    if (res === false || res === null || res === "") {
+                        labelEl.innerHTML = '<i class="fa-duotone fa-file-image me-2"></i>Viagem sem Ficha de Vistoria Digitalizada';
+                        imgViewer.src = "/Images/FichaAmarelaNova.jpg";
                     } else {
-                        labelEl.innerHTML =
-                            '<i class="fa-duotone fa-file-image me-2"></i>Ficha de Vistoria Nº: <b>' +
-                            fichaVistoria +
-                            '</b>';
-                        imgViewer.src = 'data:image/jpg;base64,' + res;
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'atualizacustosviagem.js',
-                        'carregarFichaVistoria.success',
-                        error,
-                    );
+                        labelEl.innerHTML = '<i class="fa-duotone fa-file-image me-2"></i>Ficha de Vistoria Nº: <b>' + fichaVistoria + '</b>';
+                        imgViewer.src = "data:image/jpg;base64," + res;
+                    }
+                }
+                catch (error) {
+                    Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "carregarFichaVistoria.success", error);
                 }
             },
             error: function (xhr, status, error) {
-                Alerta.TratamentoErroComLinha(
-                    'atualizacustosviagem.js',
-                    'carregarFichaVistoria.error',
-                    error,
-                );
-            },
+                Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "carregarFichaVistoria.error", error);
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'carregarFichaVistoria',
-            error,
-        );
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "carregarFichaVistoria", error);
     }
 }
 
 function gravarViagem() {
     try {
-        const viagemId = document.getElementById('txtId').value;
-        const noFichaVistoria =
-            document.getElementById('txtNoFichaVistoria').value;
-
-        const lstFinalidade = document.getElementById('lstFinalidadeAlterada');
-        const finalidade =
-            lstFinalidade && lstFinalidade.ej2_instances
-                ? lstFinalidade.ej2_instances[0].value
-                : null;
-
-        const lstEvento = document.getElementById('lstEvento');
+        const viagemId = document.getElementById("txtId").value;
+        const noFichaVistoria = document.getElementById("txtNoFichaVistoria").value;
+
+        const lstFinalidade = document.getElementById("lstFinalidadeAlterada");
+        const finalidade = lstFinalidade && lstFinalidade.ej2_instances ? lstFinalidade.ej2_instances[0].value : null;
+
+        const lstEvento = document.getElementById("lstEvento");
         let eventoId = null;
         if (lstEvento && lstEvento.ej2_instances) {
             const eventoValue = lstEvento.ej2_instances[0].value;
@@ -344,31 +240,21 @@
             }
         }
 
-        const dataInicial =
-            document.getElementById('txtDataInicial').value || null;
-        const horaInicial =
-            document.getElementById('txtHoraInicial').value || null;
-        const dataFinal = document.getElementById('txtDataFinal').value || null;
-        const horaFinal = document.getElementById('txtHoraFinal').value || null;
-
-        const kmInicial =
-            parseInt(document.getElementById('txtKmInicial').value) || null;
-        const kmFinal =
-            parseInt(document.getElementById('txtKmFinal').value) || null;
-
-        const lstMotorista = document.getElementById('lstMotoristaAlterado');
-        const motoristaId =
-            lstMotorista && lstMotorista.ej2_instances
-                ? lstMotorista.ej2_instances[0].value
-                : null;
-
-        const lstVeiculo = document.getElementById('lstVeiculoAlterado');
-        const veiculoId =
-            lstVeiculo && lstVeiculo.ej2_instances
-                ? lstVeiculo.ej2_instances[0].value
-                : null;
-
-        const lstSetor = document.getElementById('lstSetorSolicitanteAlterado');
+        const dataInicial = document.getElementById("txtDataInicial").value || null;
+        const horaInicial = document.getElementById("txtHoraInicial").value || null;
+        const dataFinal = document.getElementById("txtDataFinal").value || null;
+        const horaFinal = document.getElementById("txtHoraFinal").value || null;
+
+        const kmInicial = parseInt(document.getElementById("txtKmInicial").value) || null;
+        const kmFinal = parseInt(document.getElementById("txtKmFinal").value) || null;
+
+        const lstMotorista = document.getElementById("lstMotoristaAlterado");
+        const motoristaId = lstMotorista && lstMotorista.ej2_instances ? lstMotorista.ej2_instances[0].value : null;
+
+        const lstVeiculo = document.getElementById("lstVeiculoAlterado");
+        const veiculoId = lstVeiculo && lstVeiculo.ej2_instances ? lstVeiculo.ej2_instances[0].value : null;
+
+        const lstSetor = document.getElementById("lstSetorSolicitanteAlterado");
         let setorSolicitanteId = null;
         if (lstSetor && lstSetor.ej2_instances) {
             const setorValue = lstSetor.ej2_instances[0].value;
@@ -377,16 +263,10 @@
             }
         }
 
-        const lstRequisitante = document.getElementById(
-            'lstRequisitanteAlterado',
-        );
-        const requisitanteId =
-            lstRequisitante && lstRequisitante.ej2_instances
-                ? lstRequisitante.ej2_instances[0].value
-                : null;
-
-        const ramalRequisitante =
-            document.getElementById('txtRamalRequisitante').value || null;
+        const lstRequisitante = document.getElementById("lstRequisitanteAlterado");
+        const requisitanteId = lstRequisitante && lstRequisitante.ej2_instances ? lstRequisitante.ej2_instances[0].value : null;
+
+        const ramalRequisitante = document.getElementById("txtRamalRequisitante").value || null;
 
         const dados = {
             ViagemId: viagemId,
@@ -403,26 +283,26 @@
             VeiculoId: veiculoId,
             SetorSolicitanteId: setorSolicitanteId,
             RequisitanteId: requisitanteId,
-            RamalRequisitante: ramalRequisitante,
+            RamalRequisitante: ramalRequisitante
         };
 
-        const btnAjustar = document.getElementById('btnAjustarViagem');
-        const spinner = btnAjustar.querySelector('.spinner-border');
-        const btnText = btnAjustar.querySelector('.btn-text');
-        if (spinner) spinner.classList.remove('d-none');
-        if (btnText) btnText.textContent = 'Gravando...';
+        const btnAjustar = document.getElementById("btnAjustarViagem");
+        const spinner = btnAjustar.querySelector(".spinner-border");
+        const btnText = btnAjustar.querySelector(".btn-text");
+        if (spinner) spinner.classList.remove("d-none");
+        if (btnText) btnText.textContent = "Gravando...";
         btnAjustar.disabled = true;
 
         $.ajax({
-            type: 'POST',
-            url: '/api/Viagem/AtualizarDadosViagemDashboard',
-            contentType: 'application/json',
+            type: "POST",
+            url: "/api/Viagem/AtualizarDadosViagemDashboard",
+            contentType: "application/json",
             data: JSON.stringify(dados),
             success: function (res) {
                 try {
 
-                    if (spinner) spinner.classList.add('d-none');
-                    if (btnText) btnText.textContent = 'Ajustar Viagem';
+                    if (spinner) spinner.classList.add("d-none");
+                    if (btnText) btnText.textContent = "Ajustar Viagem";
                     btnAjustar.disabled = false;
 
                     if (res.success) {
@@ -431,56 +311,36 @@
                             modalAjustaCustos.hide();
                         }
 
-                        mostrarLoading('Atualizando dados...');
-
-                        $('#tblViagem')
-                            .DataTable()
-                            .ajax.reload(function () {
-                                esconderLoading();
-                                AppToast.show(
-                                    'Verde',
-                                    'Viagem atualizada com sucesso!',
-                                    3000,
-                                );
-                            }, false);
+                        mostrarLoading("Atualizando dados...");
+
+                        $("#tblViagem").DataTable().ajax.reload(function () {
+                            esconderLoading();
+                            AppToast.show("Verde", "Viagem atualizada com sucesso!", 3000);
+                        }, false);
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            res.message || 'Erro ao atualizar viagem',
-                            4000,
-                        );
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'atualizacustosviagem.js',
-                        'gravarViagem.success',
-                        error,
-                    );
+                        AppToast.show("Vermelho", res.message || "Erro ao atualizar viagem", 4000);
+                    }
+                }
+                catch (error) {
+                    Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "gravarViagem.success", error);
                 }
             },
             error: function (xhr, status, error) {
 
-                const btnAjustar = document.getElementById('btnAjustarViagem');
-                const spinner = btnAjustar.querySelector('.spinner-border');
-                const btnText = btnAjustar.querySelector('.btn-text');
-                if (spinner) spinner.classList.add('d-none');
-                if (btnText) btnText.textContent = 'Ajustar Viagem';
+                const btnAjustar = document.getElementById("btnAjustarViagem");
+                const spinner = btnAjustar.querySelector(".spinner-border");
+                const btnText = btnAjustar.querySelector(".btn-text");
+                if (spinner) spinner.classList.add("d-none");
+                if (btnText) btnText.textContent = "Ajustar Viagem";
                 btnAjustar.disabled = false;
 
-                AppToast.show('Vermelho', 'Erro ao gravar: ' + error, 4000);
-                Alerta.TratamentoErroComLinha(
-                    'atualizacustosviagem.js',
-                    'gravarViagem.error',
-                    error,
-                );
-            },
+                AppToast.show("Vermelho", "Erro ao gravar: " + error, 4000);
+                Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "gravarViagem.error", error);
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'gravarViagem',
-            error,
-        );
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "gravarViagem", error);
     }
 }
 
@@ -490,12 +350,9 @@
         if (element && element.ej2_instances) {
             element.ej2_instances[0].value = value || 0;
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'setNumericValue',
-            error,
-        );
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "setNumericValue", error);
     }
 }
 
@@ -506,126 +363,107 @@
             return element.ej2_instances[0].value || 0;
         }
         return 0;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'getNumericValue',
-            error,
-        );
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "getNumericValue", error);
         return 0;
     }
 }
 
 function formatarDataParaInput(dataStr) {
     try {
-        if (!dataStr) return '';
-
-        if (dataStr.includes('-') && dataStr.length === 10) {
+        if (!dataStr) return "";
+
+        if (dataStr.includes("-") && dataStr.length === 10) {
             return dataStr;
         }
 
-        if (dataStr.includes('/')) {
-            const partes = dataStr.split('/');
+        if (dataStr.includes("/")) {
+            const partes = dataStr.split("/");
             if (partes.length === 3) {
                 return `${partes[2]}-${partes[1].padStart(2, '0')}-${partes[0].padStart(2, '0')}`;
             }
         }
 
         return dataStr;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'formatarDataParaInput',
-            error,
-        );
-        return '';
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "formatarDataParaInput", error);
+        return "";
     }
 }
 
 function initDataTable() {
     try {
 
-        mostrarLoading('Carregando Dados de Viagens...');
-
-        $('#tblViagem').DataTable({
+        mostrarLoading("Carregando Dados de Viagens...");
+
+        $("#tblViagem").DataTable({
             processing: false,
             serverSide: false,
             paging: true,
             searching: true,
             ordering: true,
-            order: [[1, 'desc']],
+            order: [[1, "desc"]],
             ajax: {
-                url: '/api/custosviagem',
-                type: 'GET',
+                url: "/api/custosviagem",
+                type: "GET",
                 dataSrc: function (json) {
                     try {
 
                         esconderLoading();
                         return json.data || [];
-                    } catch (error) {
+                    }
+                    catch (error) {
                         esconderLoading();
-                        Alerta.TratamentoErroComLinha(
-                            'atualizacustosviagem.js',
-                            'ajax.dataSrc',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "ajax.dataSrc", error);
                         return [];
                     }
                 },
                 error: function (xhr, status, error) {
                     esconderLoading();
-                    Alerta.TratamentoErroComLinha(
-                        'atualizacustosviagem.js',
-                        'ajax.error',
-                        error,
-                    );
-                },
+                    Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "ajax.error", error);
+                }
             },
             columns: [
-                { data: 'noFichaVistoria' },
-                { data: 'dataInicial' },
-                { data: 'dataFinal' },
-                { data: 'horaInicio' },
-                { data: 'horaFim' },
-                { data: 'finalidade' },
-                { data: 'nomeMotorista' },
-                { data: 'descricaoVeiculo' },
+                { data: "noFichaVistoria" },
+                { data: "dataInicial" },
+                { data: "dataFinal" },
+                { data: "horaInicio" },
+                { data: "horaFim" },
+                { data: "finalidade" },
+                { data: "nomeMotorista" },
+                { data: "descricaoVeiculo" },
                 {
-                    data: 'kmInicial',
+                    data: "kmInicial",
                     render: function (data) {
                         try {
-                            return data ? data.toLocaleString('pt-BR') : '-';
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'atualizacustosviagem.js',
-                                'render.kmInicial',
-                                error,
-                            );
-                            return '-';
-                        }
-                    },
+                            return data ? data.toLocaleString("pt-BR") : "-";
+                        }
+                        catch (error) {
+                            Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "render.kmInicial", error);
+                            return "-";
+                        }
+                    }
                 },
                 {
-                    data: 'kmFinal',
+                    data: "kmFinal",
                     render: function (data) {
                         try {
-                            return data ? data.toLocaleString('pt-BR') : '-';
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'atualizacustosviagem.js',
-                                'render.kmFinal',
-                                error,
-                            );
-                            return '-';
-                        }
-                    },
+                            return data ? data.toLocaleString("pt-BR") : "-";
+                        }
+                        catch (error) {
+                            Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "render.kmFinal", error);
+                            return "-";
+                        }
+                    }
                 },
 
                 {
-                    data: 'viagemId',
+                    data: "viagemId",
                     orderable: false,
                     searchable: false,
-                    className: 'ftx-actions text-center',
+                    className: "ftx-actions text-center",
                     render: function (data, type, row, meta) {
                         try {
                             return `<div class="d-flex justify-content-center gap-1">
@@ -642,60 +480,51 @@
                                     <i class="fa-duotone fa-file-image"></i>
                                 </button>
                             </div>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'atualizacustosviagem.js',
-                                'columns.render.acao',
-                                error,
-                            );
-                            return '';
-                        }
-                    },
+                        }
+                        catch (error) {
+                            Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "columns.render.acao", error);
+                            return "";
+                        }
+                    }
                 },
 
                 {
-                    data: 'viagemId',
+                    data: "viagemId",
                     visible: false,
                     render: function (data, type, row, meta) {
                         try {
                             return meta.row + meta.settings._iDisplayStart + 1;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'atualizacustosviagem.js',
-                                'columns.render.rowNumber',
-                                error,
-                            );
-                            return '';
-                        }
-                    },
-                },
+                        }
+                        catch (error) {
+                            Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "columns.render.rowNumber", error);
+                            return "";
+                        }
+                    }
+                }
             ],
             language: {
-                emptyTable: 'Nenhum registro encontrado',
-                info: 'Mostrando de _START_ até _END_ de _TOTAL_ registros',
-                infoEmpty: 'Mostrando 0 até 0 de 0 registros',
-                infoFiltered: '(Filtrados de _MAX_ registros)',
-                infoThousands: '.',
-                lengthMenu: '_MENU_ resultados por página',
-                loadingRecords: 'Carregando...',
-                processing: 'Processando...',
-                search: 'Pesquisar:',
-                zeroRecords: 'Nenhum registro encontrado',
+                emptyTable: "Nenhum registro encontrado",
+                info: "Mostrando de _START_ até _END_ de _TOTAL_ registros",
+                infoEmpty: "Mostrando 0 até 0 de 0 registros",
+                infoFiltered: "(Filtrados de _MAX_ registros)",
+                infoThousands: ".",
+                lengthMenu: "_MENU_ resultados por página",
+                loadingRecords: "Carregando...",
+                processing: "Processando...",
+                search: "Pesquisar:",
+                zeroRecords: "Nenhum registro encontrado",
                 paginate: {
-                    first: 'Primeiro',
-                    last: 'Último',
-                    next: 'Próximo',
-                    previous: 'Anterior',
-                },
+                    first: "Primeiro",
+                    last: "Último",
+                    next: "Próximo",
+                    previous: "Anterior"
+                }
             },
             dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip',
-            responsive: true,
+            responsive: true
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'atualizacustosviagem.js',
-            'initDataTable',
-            error,
-        );
-    }
-}
+    }
+    catch (error) {
+        Alerta.TratamentoErroComLinha("atualizacustosviagem.js", "initDataTable", error);
+    }
+}
```
