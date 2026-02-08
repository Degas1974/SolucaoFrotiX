# wwwroot/js/cadastros/usuario-index.js

**Mudanca:** GRANDE | **+123** linhas | **-290** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/usuario-index.js
+++ ATUAL: wwwroot/js/cadastros/usuario-index.js
@@ -1,5 +1,5 @@
 (function () {
-    'use strict';
+    "use strict";
 
     let dataTableUsuarios = null;
     let dataTableRecursos = null;
@@ -8,48 +8,30 @@
 
     document.addEventListener('DOMContentLoaded', function () {
         try {
+
             inicializarDataTableUsuarios();
             inicializarModais();
             configurarEventosDelegados();
         } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'usuario-index.js',
-                'DOMContentLoaded',
-                error,
-            );
+            Alerta.TratamentoErroComLinha("usuario-index.js", "DOMContentLoaded", error);
         }
     });
 
     function inicializarModais() {
         try {
 
-            const modalControleAcessoEl = document.getElementById(
-                'modalControleAcesso',
-            );
+            const modalControleAcessoEl = document.getElementById('modalControleAcesso');
             if (modalControleAcessoEl) {
-                modalControleAcessoInstance = new bootstrap.Modal(
-                    modalControleAcessoEl,
-                );
-
-                modalControleAcessoEl.addEventListener(
-                    'hidden.bs.modal',
-                    function () {
-                        try {
-                            document.getElementById(
-                                'txtUsuarioIdRecurso',
-                            ).value = '';
-                            document.getElementById(
-                                'txtNomeUsuarioRecurso',
-                            ).textContent = '';
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'usuario-index.js',
-                                'modalControleAcesso.hidden',
-                                error,
-                            );
-                        }
-                    },
-                );
+                modalControleAcessoInstance = new bootstrap.Modal(modalControleAcessoEl);
+
+                modalControleAcessoEl.addEventListener('hidden.bs.modal', function () {
+                    try {
+                        document.getElementById('txtUsuarioIdRecurso').value = '';
+                        document.getElementById('txtNomeUsuarioRecurso').textContent = '';
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("usuario-index.js", "modalControleAcesso.hidden", error);
+                    }
+                });
             }
 
             const modalFotoEl = document.getElementById('modalFoto');
@@ -58,26 +40,16 @@
 
                 modalFotoEl.addEventListener('hidden.bs.modal', function () {
                     try {
-                        document.getElementById(
-                            'txtNomeUsuarioFoto',
-                        ).textContent = '';
-                        document.getElementById('divFotoContainer').innerHTML =
-                            '';
+                        document.getElementById('txtNomeUsuarioFoto').textContent = '';
+                        document.getElementById('divFotoContainer').innerHTML = '';
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'usuario-index.js',
-                            'modalFoto.hidden',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("usuario-index.js", "modalFoto.hidden", error);
                     }
                 });
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'usuario-index.js',
-                'inicializarModais',
-                error,
-            );
+
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("usuario-index.js", "inicializarModais", error);
         }
     }
 
@@ -89,34 +61,36 @@
             }
 
             dataTableRecursos = $('#tblRecursos').DataTable({
+
                 order: [[0, 'asc']],
                 columnDefs: [
-                    { targets: 0, className: 'text-left' },
-                    { targets: 1, className: 'text-center', width: '130px' },
+                    { targets: 0, className: "text-left" },
+                    { targets: 1, className: "text-center", width: "130px" }
                 ],
                 responsive: true,
+
                 ajax: {
-                    url: '/api/Usuario/PegaRecursosUsuario',
-                    type: 'GET',
-                    datatype: 'json',
+                    url: "/api/Usuario/PegaRecursosUsuario",
+                    type: "GET",
+                    datatype: "json",
                     data: { usuarioId: usuarioId },
                     error: function (xhr, error, code) {
-                        console.error('Erro ao carregar recursos:', error);
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar recursos do usuário',
-                            5000,
-                        );
-                    },
+                        console.error("Erro ao carregar recursos:", error);
+                        AppToast.show("Vermelho", "Erro ao carregar recursos do usuário", 5000);
+                    }
                 },
+
                 columns: [
-                    { data: 'nome' },
+
+                    { data: "nome" },
+
                     {
-                        data: 'acesso',
+                        data: "acesso",
                         render: function (data, type, row) {
                             try {
                                 const url = `/api/Usuario/UpdateStatusAcesso?IDS=${row.ids}`;
                                 if (data === true) {
+
                                     return `<a href="javascript:void(0)"
                                                class="btn btn-xs ftx-badge-status btn-verde updateStatusAcesso"
                                                data-url="${url}"
@@ -124,6 +98,7 @@
                                                 <i class="fa-duotone fa-unlock me-1"></i>Com Acesso
                                             </a>`;
                                 } else {
+
                                     return `<a href="javascript:void(0)"
                                                class="btn btn-xs ftx-badge-status fundo-cinza updateStatusAcesso"
                                                data-url="${url}"
@@ -132,65 +107,53 @@
                                             </a>`;
                                 }
                             } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'usuario-index.js',
-                                    'render.acesso',
-                                    error,
-                                );
+                                Alerta.TratamentoErroComLinha("usuario-index.js", "render.acesso", error);
                                 return '';
                             }
-                        },
-                    },
+                        }
+                    }
                 ],
+
                 language: {
-                    url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                    emptyTable: 'Nenhum recurso disponível',
-                },
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'usuario-index.js',
-                'carregarRecursosUsuario',
-                error,
-            );
+                    url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                    emptyTable: "Nenhum recurso disponível"
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("usuario-index.js", "carregarRecursosUsuario", error);
         }
     }
 
     function inicializarDataTableUsuarios() {
         try {
+
             dataTableUsuarios = $('#tblUsuario').DataTable({
+
                 order: [[0, 'asc']],
                 autoWidth: false,
                 columnDefs: [
-                    { targets: 0, className: 'text-left', width: '40%' },
-                    { targets: 1, className: 'text-center', width: '15%' },
-                    { targets: 2, className: 'text-center', width: '15%' },
-                    { targets: 3, className: 'text-center', width: '15%' },
-                    {
-                        targets: 4,
-                        className: 'text-center ftx-actions',
-                        width: '20%',
-                        orderable: false,
-                    },
+                    { targets: 0, className: "text-left", width: "40%" },
+                    { targets: 1, className: "text-center", width: "15%" },
+                    { targets: 2, className: "text-center", width: "15%" },
+                    { targets: 3, className: "text-center", width: "15%" },
+                    { targets: 4, className: "text-center ftx-actions", width: "15%", orderable: false }
                 ],
                 responsive: true,
+
                 ajax: {
-                    url: '/api/Usuario/GetAll',
-                    type: 'GET',
-                    datatype: 'json',
+                    url: "/api/Usuario/GetAll",
+                    type: "GET",
+                    datatype: "json",
                     error: function (xhr, error, code) {
-                        console.error('Erro ao carregar usuários:', error);
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar lista de usuários',
-                            5000,
-                        );
-                    },
+                        console.error("Erro ao carregar usuários:", error);
+                        AppToast.show("Vermelho", "Erro ao carregar lista de usuários", 5000);
+                    }
                 },
+
                 columns: [
+
                     {
-
-                        data: 'nomeCompleto',
+                        data: "nomeCompleto",
                         render: function (data, type, row) {
                             try {
                                 const nome = data || 'Sem Nome';
@@ -217,7 +180,7 @@
                                 } else {
 
                                     avatarHtml = `<div class="ftx-avatar" data-ejtip="Usuário sem foto">
-                                                                                                        <span class="ftx-avatar-ico"><i class="fa-duotone fa-user"></i></span>
+                                                    <span class="ftx-avatar-ico"><i class="fa-duotone fa-user"></i></span>
                                                   </div>`;
                                 }
 
@@ -226,29 +189,28 @@
                                             <span>${nome}</span>
                                         </div>`;
                             } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'usuario-index.js',
-                                    'render.nomeCompleto',
-                                    error,
-                                );
+                                Alerta.TratamentoErroComLinha("usuario-index.js", "render.nomeCompleto", error);
                                 return '';
                             }
-                        },
+                        }
                     },
+
                     {
-
-                        data: 'ponto',
+                        data: "ponto",
                         render: function (data) {
+
                             return data || '-';
-                        },
+                        }
                     },
+
                     {
-
-                        data: 'detentorCargaPatrimonial',
+                        data: "detentorCargaPatrimonial",
                         render: function (data, type, row) {
                             try {
+
                                 const url = `/api/Usuario/UpdateCargaPatrimonial?Id=${row.usuarioId}`;
                                 if (data === true) {
+
                                     return `<a href="javascript:void(0)"
                                                class="btn btn-xs ftx-badge-status btn-verde updateCargaPatrimonial"
                                                data-url="${url}"
@@ -256,6 +218,7 @@
                                                 <i class="fa-duotone fa-badge-check me-1"></i>Sim
                                             </a>`;
                                 } else {
+
                                     return `<a href="javascript:void(0)"
                                                class="btn btn-xs ftx-badge-status fundo-cinza updateCargaPatrimonial"
                                                data-url="${url}"
@@ -264,22 +227,20 @@
                                             </a>`;
                                 }
                             } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'usuario-index.js',
-                                    'render.detentorCarga',
-                                    error,
-                                );
+                                Alerta.TratamentoErroComLinha("usuario-index.js", "render.detentorCarga", error);
                                 return '';
                             }
-                        },
+                        }
                     },
+
                     {
-
-                        data: 'status',
+                        data: "status",
                         render: function (data, type, row) {
                             try {
+
                                 const url = `/api/Usuario/UpdateStatusUsuario?Id=${row.usuarioId}`;
                                 if (data === true) {
+
                                     return `<a href="javascript:void(0)"
                                                class="btn btn-xs ftx-badge-status btn-verde updateStatusUsuario"
                                                data-url="${url}"
@@ -287,6 +248,7 @@
                                                 <i class="fa-duotone fa-circle-check me-1"></i>Ativo
                                             </a>`;
                                 } else {
+
                                     return `<a href="javascript:void(0)"
                                                class="btn btn-xs ftx-badge-status fundo-cinza updateStatusUsuario"
                                                data-url="${url}"
@@ -295,17 +257,13 @@
                                             </a>`;
                                 }
                             } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'usuario-index.js',
-                                    'render.status',
-                                    error,
-                                );
+                                Alerta.TratamentoErroComLinha("usuario-index.js", "render.status", error);
                                 return '';
                             }
-                        },
+                        }
                     },
+
                     {
-
                         data: null,
                         render: function (data, type, row) {
                             try {
@@ -328,6 +286,7 @@
 
                                 let btnFoto = '';
                                 if (temFoto) {
+
                                     btnFoto = `<button type="button"
                                                       class="btn btn-foto btn-icon-28 btnFoto"
                                                       data-id="${row.usuarioId}"
@@ -348,16 +307,9 @@
                                                 </span>`;
                                 }
 
-                                const returnUrl =
-                                    encodeURIComponent('/Usuarios/Index');
-                                let btnMudarSenha = `<a href="/Usuarios/MudarSenha?returnUrl=${returnUrl}"
-                                                      class="btn btn-marrom btn-icon-28"
-                                                      data-ejtip="Mudar senha">
-                                                      <i class="fa-duotone fa-key"></i>
-                                                  </a>`;
-
                                 let btnExcluir = '';
                                 if (podeExcluir) {
+
                                     btnExcluir = `<button type="button"
                                                          class="btn btn-vinho btn-icon-28 btnExcluir"
                                                          data-id="${row.usuarioId}"
@@ -377,35 +329,22 @@
                                                    </span>`;
                                 }
 
-                                return (
-                                    btnEditar +
-                                    btnRecursos +
-                                    btnFoto +
-                                    btnMudarSenha +
-                                    btnExcluir
-                                );
+                                return btnEditar + btnRecursos + btnFoto + btnExcluir;
                             } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'usuario-index.js',
-                                    'render.acoes',
-                                    error,
-                                );
+                                Alerta.TratamentoErroComLinha("usuario-index.js", "render.acoes", error);
                                 return '';
                             }
-                        },
-                    },
+                        }
+                    }
                 ],
+
                 language: {
-                    url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                    emptyTable: 'Nenhum usuário cadastrado',
-                },
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'usuario-index.js',
-                'inicializarDataTableUsuarios',
-                error,
-            );
+                    url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                    emptyTable: "Nenhum usuário cadastrado"
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("usuario-index.js", "inicializarDataTableUsuarios", error);
         }
     }
 
@@ -422,11 +361,7 @@
 
                     abrirModalFoto(nome, foto);
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'usuario-index.js',
-                        'click.btnAbrirFoto',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.btnAbrirFoto", error);
                 }
             });
 
@@ -442,11 +377,7 @@
                         abrirModalFoto(nome, foto);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'usuario-index.js',
-                        'click.btnFoto',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.btnFoto", error);
                 }
             });
 
@@ -458,11 +389,8 @@
                     const usuarioId = $(this).data('id');
                     const nomeUsuario = $(this).data('nome');
 
-                    document.getElementById('txtUsuarioIdRecurso').value =
-                        usuarioId;
-                    document.getElementById(
-                        'txtNomeUsuarioRecurso',
-                    ).textContent = nomeUsuario;
+                    document.getElementById('txtUsuarioIdRecurso').value = usuarioId;
+                    document.getElementById('txtNomeUsuarioRecurso').textContent = nomeUsuario;
 
                     carregarRecursosUsuario(usuarioId);
 
@@ -470,11 +398,7 @@
                         modalControleAcessoInstance.show();
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'usuario-index.js',
-                        'click.btnRecursos',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.btnRecursos", error);
                 }
             });
 
@@ -482,15 +406,12 @@
                 try {
                     e.preventDefault();
                     const url = $(this).data('url');
-                    executarAcaoAjax(url, 'Status atualizado!', function () {
+
+                    executarAcaoAjax(url, "Status atualizado!", function () {
                         dataTableUsuarios.ajax.reload(null, false);
                     });
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'usuario-index.js',
-                        'click.updateStatusUsuario',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.updateStatusUsuario", error);
                 }
             });
 
@@ -498,19 +419,12 @@
                 try {
                     e.preventDefault();
                     const url = $(this).data('url');
-                    executarAcaoAjax(
-                        url,
-                        'Carga patrimonial atualizada!',
-                        function () {
-                            dataTableUsuarios.ajax.reload(null, false);
-                        },
-                    );
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'usuario-index.js',
-                        'click.updateCargaPatrimonial',
-                        error,
-                    );
+
+                    executarAcaoAjax(url, "Carga patrimonial atualizada!", function () {
+                        dataTableUsuarios.ajax.reload(null, false);
+                    });
+                } catch (error) {
+                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.updateCargaPatrimonial", error);
                 }
             });
 
@@ -518,17 +432,14 @@
                 try {
                     e.preventDefault();
                     const url = $(this).data('url');
-                    executarAcaoAjax(url, 'Acesso atualizado!', function () {
+
+                    executarAcaoAjax(url, "Acesso atualizado!", function () {
                         if (dataTableRecursos) {
                             dataTableRecursos.ajax.reload(null, false);
                         }
                     });
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'usuario-index.js',
-                        'click.updateStatusAcesso',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.updateStatusAcesso", error);
                 }
             });
 
@@ -537,35 +448,32 @@
                     e.preventDefault();
                     const usuarioId = $(this).data('id');
                     const nome = $(this).data('nome');
+
                     confirmarExclusao(usuarioId, nome);
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'usuario-index.js',
-                        'click.btnExcluir',
-                        error,
-                    );
-                }
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'usuario-index.js',
-                'configurarEventosDelegados',
-                error,
-            );
+                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.btnExcluir", error);
+                }
+            });
+
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("usuario-index.js", "configurarEventosDelegados", error);
         }
     }
 
     function abrirModalFoto(nome, foto) {
         try {
+
             document.getElementById('txtNomeUsuarioFoto').textContent = nome;
 
             const container = document.getElementById('divFotoContainer');
 
             if (foto && foto.trim() !== '') {
+
                 container.innerHTML = `<img src="data:image/jpeg;base64,${foto}"
                                             class="img-foto-usuario"
                                             alt="Foto de ${nome}" />`;
             } else {
+
                 container.innerHTML = `<div class="no-foto-placeholder">
                                            <i class="fa-duotone fa-user-slash"></i>
                                            <p>Usuário sem foto cadastrada</p>
@@ -576,132 +484,105 @@
                 modalFotoInstance.show();
             }
         } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'usuario-index.js',
-                'abrirModalFoto',
-                error,
-            );
+            Alerta.TratamentoErroComLinha("usuario-index.js", "abrirModalFoto", error);
         }
     }
 
     function executarAcaoAjax(url, mensagemSucesso, callback) {
         try {
+
             $.ajax({
                 url: url,
-                type: 'GET',
+                type: "GET",
                 success: function (response) {
                     try {
+
                         if (response.success) {
-                            AppToast.show(
-                                'Verde',
-                                response.message || mensagemSucesso,
-                                3000,
-                            );
+
+                            AppToast.show("Verde", response.message || mensagemSucesso, 3000);
+
                             if (typeof callback === 'function') {
                                 callback();
                             }
                         } else {
-                            AppToast.show(
-                                'Vermelho',
-                                response.message || 'Erro ao executar ação',
-                                5000,
-                            );
+
+                            AppToast.show("Vermelho", response.message || "Erro ao executar ação", 5000);
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'usuario-index.js',
-                            'executarAcaoAjax.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("usuario-index.js", "executarAcaoAjax.success", error);
                     }
                 },
                 error: function (xhr, status, error) {
-                    console.error('Erro AJAX:', error);
-                    AppToast.show('Vermelho', 'Erro ao executar ação', 5000);
-                },
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'usuario-index.js',
-                'executarAcaoAjax',
-                error,
-            );
+
+                    console.error("Erro AJAX:", error);
+
+                    AppToast.show("Vermelho", "Erro ao executar ação", 5000);
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("usuario-index.js", "executarAcaoAjax", error);
         }
     }
 
     function confirmarExclusao(usuarioId, nome) {
         try {
+
             Alerta.Confirmar(
-                'Confirmar Exclusão',
+                "Confirmar Exclusão",
                 `Deseja realmente excluir o usuário <strong>${nome}</strong>?<br><br>` +
-                    `<small style="color: #dc3545; font-size: 0.875rem;">⚠️ Esta ação não pode ser desfeita!</small><br><br>` +
-                    `<small style="color: #6c757d; font-size: 0.875rem;">O sistema verificará automaticamente se o usuário possui registros vinculados (Viagens, Manutenções, etc.).</small>`,
-                'Sim, Excluir',
-                'Cancelar',
+                `<small style="color: #dc3545; font-size: 0.875rem;">⚠️ Esta ação não pode ser desfeita!</small><br><br>` +
+                `<small style="color: #6c757d; font-size: 0.875rem;">O sistema verificará automaticamente se o usuário possui registros vinculados (Viagens, Manutenções, etc.).</small>`,
+                "Sim, Excluir",
+                "Cancelar"
             ).then((willDelete) => {
                 try {
+
                     if (willDelete) {
                         excluirUsuario(usuarioId);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'usuario-index.js',
-                        'confirmarExclusao.then',
-                        error,
-                    );
-                }
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'usuario-index.js',
-                'confirmarExclusao',
-                error,
-            );
+                    Alerta.TratamentoErroComLinha("usuario-index.js", "confirmarExclusao.then", error);
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("usuario-index.js", "confirmarExclusao", error);
         }
     }
 
     function excluirUsuario(usuarioId) {
         try {
+
             $.ajax({
-                url: '/api/Usuario/Delete',
-                type: 'POST',
-                contentType: 'application/json',
+                url: "/api/Usuario/Delete",
+                type: "POST",
+                contentType: "application/json",
                 data: JSON.stringify({ Id: usuarioId }),
                 success: function (response) {
                     try {
+
                         if (response.success) {
-                            AppToast.show(
-                                'Verde',
-                                response.message || 'Usuário excluído!',
-                                3000,
-                            );
+
+                            AppToast.show("Verde", response.message || "Usuário excluído!", 3000);
+
                             dataTableUsuarios.ajax.reload(null, false);
                         } else {
-                            AppToast.show(
-                                'Vermelho',
-                                response.message || 'Erro ao excluir',
-                                5000,
-                            );
+
+                            AppToast.show("Vermelho", response.message || "Erro ao excluir", 5000);
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'usuario-index.js',
-                            'excluirUsuario.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("usuario-index.js", "excluirUsuario.success", error);
                     }
                 },
                 error: function (xhr, status, error) {
-                    console.error('Erro ao excluir:', error);
-                    AppToast.show('Vermelho', 'Erro ao excluir usuário', 5000);
-                },
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'usuario-index.js',
-                'excluirUsuario',
-                error,
-            );
-        }
-    }
+
+                    console.error("Erro ao excluir:", error);
+
+                    AppToast.show("Vermelho", "Erro ao excluir usuário", 5000);
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("usuario-index.js", "excluirUsuario", error);
+        }
+    }
+
 })();
```
