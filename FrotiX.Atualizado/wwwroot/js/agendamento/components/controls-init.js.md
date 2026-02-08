# wwwroot/js/agendamento/components/controls-init.js

**ARQUIVO NOVO** | 157 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```javascript
window.inicializarEventHandlersControles = function () {
    try {
        console.log('🎯 Inicializando event handlers dos controles...');

        const lstFinalidade = document.getElementById('lstFinalidade');
        if (lstFinalidade && lstFinalidade.ej2_instances && lstFinalidade.ej2_instances[0]) {
            const finalidadeObj = lstFinalidade.ej2_instances[0];

            finalidadeObj.change = null;

            finalidadeObj.change = function (args) {
                if (window.lstFinalidade_Change) {
                    window.lstFinalidade_Change(args);
                }
            };

            console.log('✅ lstFinalidade: change event configurado');
        }

        const lstMotorista = document.getElementById('lstMotorista');
        if (lstMotorista && lstMotorista.ej2_instances && lstMotorista.ej2_instances[0]) {
            const motoristaObj = lstMotorista.ej2_instances[0];

            console.log('🔧 Inicializando lstMotorista...');

            motoristaObj.created = function () {
                if (window.onLstMotoristaCreated) {
                    window.onLstMotoristaCreated();
                }
            };

            motoristaObj.change = function (args) {
                if (window.MotoristaValueChange) {
                    window.MotoristaValueChange(args);
                }
            };

            motoristaObj.itemTemplate = function (data) {
                if (!data) return '';

                let imgSrc = (data.FotoBase64 && data.FotoBase64.startsWith('data:image'))
                    ? data.FotoBase64
                    : '/images/barbudo.jpg';

                return `
            <div class="d-flex align-items-center">
                <img src="${imgSrc}"
                     alt="Foto"
                     style="height:40px; width:40px; border-radius:50%; margin-right:10px; object-fit: cover;"
                     onerror="this.src='/images/barbudo.jpg';" />
                <span>${data.Nome || data.MotoristaCondutor || ''}</span>
            </div>`;
            };

            motoristaObj.valueTemplate = function (data) {
                if (!data) return '';

                let imgSrc = (data.FotoBase64 && data.FotoBase64.startsWith('data:image'))
                    ? data.FotoBase64
                    : '/images/barbudo.jpg';

                return `
            <div class="d-flex align-items-center">
                <img src="${imgSrc}"
                     alt="Foto"
                     style="height:30px; width:30px; border-radius:50%; margin-right:10px; object-fit: cover;"
                     onerror="this.src='/images/barbudo.jpg';" />
                <span>${data.Nome || data.MotoristaCondutor || ''}</span>
            </div>`;
            };

            if (window.onLstMotoristaCreated) {
                window.onLstMotoristaCreated();
            }

            console.log('✅ lstMotorista configurado');
        }

        const lstVeiculo = document.getElementById('lstVeiculo');
        if (lstVeiculo && lstVeiculo.ej2_instances && lstVeiculo.ej2_instances[0]) {
            const veiculoObj = lstVeiculo.ej2_instances[0];

            veiculoObj.change = null;
            veiculoObj.change = function (args) {
                if (window.VeiculoValueChange) {
                    window.VeiculoValueChange(args);
                }
            };

            console.log('✅ lstVeiculo: change event configurado');
        }

        const lstRequisitante = document.getElementById('lstRequisitante');
        if (lstRequisitante && lstRequisitante.ej2_instances && lstRequisitante.ej2_instances[0]) {
            const requisitanteObj = lstRequisitante.ej2_instances[0];

            console.log('🔧 Configurando eventos do lstRequisitante...');
            console.log(' Antes - select:', requisitanteObj.select);
            console.log(' Antes - change:', requisitanteObj.change);

            requisitanteObj.select = null;
            requisitanteObj.select = function (args) {
                if (window.onSelectRequisitante) {
                    window.onSelectRequisitante(args);
                }
            };

            requisitanteObj.change = null;
            requisitanteObj.change = function (args) {
                if (window.RequisitanteValueChange) {
                    window.RequisitanteValueChange(args);
                }
            };

            console.log(' Depois - select:', requisitanteObj.select);
            console.log(' Depois - change:', requisitanteObj.change);
            console.log('✅ lstRequisitante: select e change events configurados');
        }

        const lstRequisitanteEvento = document.getElementById('lstRequisitanteEvento');
        if (lstRequisitanteEvento && lstRequisitanteEvento.ej2_instances && lstRequisitanteEvento.ej2_instances[0]) {
            const requisitanteEventoObj = lstRequisitanteEvento.ej2_instances[0];

            requisitanteEventoObj.change = null;
            requisitanteEventoObj.change = function (args) {
                if (window.RequisitanteEventoValueChange) {
                    window.RequisitanteEventoValueChange(args);
                }
            };

            console.log('✅ lstRequisitanteEvento: change event configurado');
        }

        const lstDias = document.getElementById('lstDias');
        if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0]) {
            const diasObj = lstDias.ej2_instances[0];

            diasObj.blur = null;
            diasObj.blur = function (args) {
                if (window.onBlurLstDias) {
                    window.onBlurLstDias(args);
                }
            };

            console.log('✅ lstDias: blur event configurado');
        }

        const rteDescricao = document.getElementById('rteDescricao');
        if (rteDescricao && rteDescricao.ej2_instances && rteDescricao.ej2_instances[0]) {
            const rteObj = rteDescricao.ej2_instances[0];

            if (window.onCreate) {
                rteObj.created = function () {
                    window.onCreate();
                };
            }

            if (window.toolbarClick) {
                rteObj.toolbarClick = function (args) {
                    window.toolbarClick(args);
                };
            }

            console.log('✅ rteDescricao: created e toolbarClick events configurados');
        }

        const lstRecorrente = document.getElementById('lstRecorrente');
        if (lstRecorrente && lstRecorrente.ej2_instances && lstRecorrente.ej2_instances[0]) {
            const recorrenteObj = lstRecorrente.ej2_instances[0];

            recorrenteObj.change = null;
            recorrenteObj.change = function (args) {
                if (window.RecorrenteValueChange) {
                    window.RecorrenteValueChange(args);
                }
            };

            console.log('✅ lstRecorrente: change event configurado');
        }

        const lstPeriodos = document.getElementById('lstPeriodos');
        if (lstPeriodos && lstPeriodos.ej2_instances && lstPeriodos.ej2_instances[0]) {
            const periodosObj = lstPeriodos.ej2_instances[0];

            if (window.PeriodosValueChange) {
                periodosObj.change = function (args) {
                    window.PeriodosValueChange(args);
                };
                console.log('✅ lstPeriodos: change event configurado');
            }
        }

        console.log('✅ Todos os event handlers foram configurados!');

    } catch (error) {
        Alerta.TratamentoErroComLinha("controls-init.js", "inicializarEventHandlersControles", error);
    }
};
```
