# Documentação: AbastecimentoController.cs

> **Última Atualização**: 15/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Métodos](#métodos)
4. [Dependências](#dependências)
5. [Propriedades](#propriedades)
6. [Interconexões](#interconexões)
7. [Troubleshooting](#troubleshooting)

---

## Visão Geral

O `AbastecimentoController.cs` atua como o principal orquestrador para todas as operações relacionadas ao controle de combustível no ecossistema FrotiX. Localizado no namespace `FrotiX.Controllers`, este controlador gerencia desde fluxos simples de visualização e listagem até processos complexos de integração e processamento assíncrono.

A arquitetura deste módulo é robusta, fundamentada no padrão **Unit of Work** e **Repository**, garantindo que as interações com o banco de dados SQL Server sejam seguras e transacionais. Além disso, o controlador utiliza o **SignalR** (via `ImportacaoHub`) para fornecer feedback em tempo real aos usuários durante processos de longa duração, como a importação massiva de cupons de abastecimento.

Com mais de 800 linhas de código, ele é um dos pilares do sistema, integrando-se profundamente com as visões de dados (`ViewAbastecimentos`) para performance otimizada em relatórios e dashboards.

---

## Métodos e Lógica de Negócio

### 1. Listagem Principal (`Get`)

Este método é a porta de entrada para a visualização dos dados de abastecimento. Ele não apenas recupera os registros, mas garante que a experiência do usuário seja fluida ao ordenar as informações cronologicamente de forma decrescente.

**Explicação Técnica do Código:**

```csharp
[HttpGet]
public IActionResult Get()
{
    try
    {
        // Acesso à View otimizada via Unit Of Work
        var dados = _unitOfWork
            .ViewAbastecimentos.GetAll()
            .OrderByDescending(va => va.DataHora)
            .ToList();

        // Retorno padronizado para consumo em DataTables ou Grids
        return Ok(new
        {
            data = dados
        });
    }
    catch (Exception error)
    {
        // Tratamento centralizado seguindo as Regras de Desenvolvimento FrotiX
        Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "Get" , error);
        return StatusCode(500);
    }
}
```

- **Fluxo de Dados:** O método invoca `_unitOfWork.ViewAbastecimentos.GetAll()`. Aqui, o uso de uma **View** em vez da tabela direta permite que consultas complexas (envolvendo joins de Veículos, Motoristas e Combustíveis) sejam executadas com performance máxima no SQL Server.
- **Ordenação:** O `OrderByDescending(va => va.DataHora)` é crítico para que os registros mais recentes apareçam no topo da lista.
- **Segurança:** Todo o corpo está envolvido em um `try-catch`. Caso ocorra qualquer falha (perda de conexão, erro de mapeamento), o helper `Alerta.TratamentoErroComLinha` registra a falha exata, facilitando a depuração.

---

### 2. Gestão de Quilometragem (`AtualizaQuilometragem`)

Este endpoint foi projetado para ser leve e específico. Diferente de um update completo de entidade, ele recebe um payload dinâmico para atualizar apenas o hodômetro de um registro específico.

**Explicação Técnica do Código:**

```csharp
[HttpPost]
[Route("AtualizaQuilometragem")]
public IActionResult AtualizaQuilometragem([FromBody] Dictionary<string, object> payload)
{
    try
    {
        // 1. Extração segura do ID do abastecimento
        if (!payload.TryGetValue("AbastecimentoId", out var abastecimentoIdObj))
        {
            return BadRequest(new { success = false, message = "AbastecimentoId é obrigatório" });
        }

        var abastecimentoId = Guid.Parse(abastecimentoIdObj.ToString());

        // 2. Busca do registro original no banco
        var objAbastecimento = _unitOfWork.Abastecimento.GetFirstOrDefault(a =>
            a.AbastecimentoId == abastecimentoId
        );

        if (objAbastecimento == null)
        {
            return NotFound(new { success = false, message = "Abastecimento não encontrado" });
        }

        // 3. Atualização condicional do KmRodado
        if (payload.TryGetValue("KmRodado", out var kmRodadoObj) && kmRodadoObj != null)
        {
            if (int.TryParse(kmRodadoObj.ToString(), out var km))
            {
                objAbastecimento.KmRodado = km;
            }
        }

        // 4. Persistência e Salvaguarda
        _unitOfWork.Abastecimento.Update(objAbastecimento);
        _unitOfWork.Save();

        return Ok(new { success = true, message = "Quilometragem atualizada!" });
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AtualizaQuilometragem", error);
        return StatusCode(500);
    }
}
```

- **Padrão de Payload:** O uso de `Dictionary<string, object>` permite flexibilidade no recebimento de dados via AJAX, evitando problemas de validação de modelo (ModelState) que ocorreriam se passássemos a entidade completa com campos nulos.
- **Validação em Camadas:** O código primeiro verifica a existência do ID, depois busca a entidade e, por fim, tenta converter o valor da quilometragem. Isso evita exceções de tipo durante a execução.
- **Persistência:** O comando `_unitOfWork.Save()` é o responsável por disparar a transação final para o banco de dados.

--- .OrderByDescending(va => va.DataHora)
.ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "Get" , error);
                return StatusCode(500);
            }
        }

````

**Métodos Chamados**:

- `_unitOfWork
                    .ViewAbastecimentos.GetAll`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `AbastecimentoVeiculos`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Parâmetros**:

- `Guid Id`

**Código**:

```csharp
{
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.VeiculoId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoVeiculos" ,
                    error
                );
                return StatusCode(500);
            }
        }
````

**Métodos Chamados**:

- `_unitOfWork
                  .ViewAbastecimentos.GetAll`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `AbastecimentoCombustivel`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Parâmetros**:

- `Guid Id`

**Código**:

```csharp
{
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.CombustivelId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoCombustivel" ,
                    error
                );
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork
                  .ViewAbastecimentos.GetAll`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `AbastecimentoUnidade`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Parâmetros**:

- `Guid Id`

**Código**:

```csharp
{
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.UnidadeId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoUnidade" ,
                    error
                );
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork
                  .ViewAbastecimentos.GetAll`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `AbastecimentoMotorista`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Parâmetros**:

- `Guid Id`

**Código**:

```csharp
{
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.MotoristaId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoMotorista" ,
                    error
                );
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork
                  .ViewAbastecimentos.GetAll`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `AbastecimentoData`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Parâmetros**:

- `string dataAbastecimento`

**Código**:

```csharp
{
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.Data == dataAbastecimento)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoData" ,
                    error
                );
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork
                  .ViewAbastecimentos.GetAll`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `Import`

**Retorno**: `ActionResult`

**HTTP**: `POST` ``

**Código**:

```csharp
{
            try
            {
                IFormFile file = Request.Form.Files[0];
                string folderName = "DadosEditaveis/UploadExcel";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath , folderName);
                StringBuilder sb = new StringBuilder();

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath , file.FileName);

                    using (var stream = new FileStream(fullPath , FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;

                        if (...
```

**Métodos Chamados**:

- `Path.Combine`
- `Directory.Exists`
- `Directory.CreateDirectory`
- `Path.GetExtension`
- `file.CopyTo`
- `hssfwb.GetSheetAt`
- `sheet.GetRow`
- `sb.Append`
- `headerRow.GetCell`
- `cell.ToString`

---

### `MotoristaList`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Código**:

```csharp
{
            try
            {
                var result = _unitOfWork.ViewMotoristas.GetAll().OrderBy(vm => vm.Nome).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "MotoristaList" , error);
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork.ViewMotoristas.GetAll`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `UnidadeList`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Código**:

```csharp
{
            try
            {
                var result = _unitOfWork.Unidade.GetAll().OrderBy(u => u.Descricao).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "UnidadeList" , error);
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork.Unidade.GetAll`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `CombustivelList`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Código**:

```csharp
{
            try
            {
                var result = _unitOfWork.Combustivel.GetAll().OrderBy(u => u.Descricao).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "CombustivelList" ,
                    error
                );
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork.Combustivel.GetAll`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `VeiculoList`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Código**:

```csharp
{
            try
            {
                var result = (
                    from v in _unitOfWork.Veiculo.GetAll()
                    join m in _unitOfWork.ModeloVeiculo.GetAll() on v.ModeloId equals m.ModeloId
                    join ma in _unitOfWork.MarcaVeiculo.GetAll() on v.MarcaId equals ma.MarcaId
                    orderby v.Placa
                    select new
                    {
                        v.VeiculoId ,
                        PlacaMarcaModelo = v.Placa
                            + " - "
                            + ma.DescricaoMarca
                            + "/"
                            + m.DescricaoModelo ,
                    }
                )
                    .OrderBy(v => v.PlacaMarcaModelo)
                    .ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Ale...
```

**Métodos Chamados**:

- `_unitOfWork.Veiculo.GetAll`
- `_unitOfWork.ModeloVeiculo.GetAll`
- `_unitOfWork.MarcaVeiculo.GetAll`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `AtualizaQuilometragem`

**Descrição**: Atualiza apenas a quilometragem de um abastecimento
Endpoint novo que não valida o modelo completo

**Retorno**: `IActionResult`

**HTTP**: `POST` ``

**Parâmetros**:

- `Dictionary<string, object> payload` [FromBody]

**Código**:

```csharp
{
            try
            {
                // Extrai os valores do payload dinâmico
                if (!payload.TryGetValue("AbastecimentoId", out var abastecimentoIdObj))
                {
                    return BadRequest(new { success = false, message = "AbastecimentoId é obrigatório" });
                }

                var abastecimentoId = Guid.Parse(abastecimentoIdObj.ToString());

                var objAbastecimento = _unitOfWork.Abastecimento.GetFirstOrDefault(a =>
                    a.AbastecimentoId == abastecimentoId
                );

                if (objAbastecimento == null)
                {
                    return NotFound(new { success = false, message = "Abastecimento não encontrado" });
                }

                // Extrai KmRodado
                if (payload.TryGetValue("KmRodado", out var kmRodadoObj) && kmRodadoObj != null)
                {
                    if (int.TryParse(kmRodadoObj.ToString(), out var km...
```

**Métodos Chamados**:

- `payload.TryGetValue`
- `BadRequest`
- `Guid.Parse`
- `abastecimentoIdObj.ToString`
- `_unitOfWork.Abastecimento.GetFirstOrDefault`
- `NotFound`
- `int.TryParse`
- `kmRodadoObj.ToString`
- `_unitOfWork.Abastecimento.Update`
- `_unitOfWork.Save`

---

### `EditaKm`

**Descrição**: Endpoint antigo - mantido para compatibilidade

**Retorno**: `IActionResult`

**HTTP**: `POST` ``

**Parâmetros**:

- `Dictionary<string, object> payload` [FromBody]

**Código**:

```csharp
{
            // Redireciona para o novo endpoint
            return AtualizaQuilometragem(payload);
        }
```

**Métodos Chamados**:

- `AtualizaQuilometragem`

---

### `ListaRegistroCupons`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Parâmetros**:

- `string IDapi`

**Código**:

```csharp
{
            try
            {
                var result = (
                    from rc in _unitOfWork.RegistroCupomAbastecimento.GetAll()
                    orderby rc.DataRegistro descending
                    select new
                    {
                        DataRegistro = rc.DataRegistro?.ToShortDateString() ,
                        rc.RegistroCupomId ,
                    }
                ).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "ListaRegistroCupons" ,
                    error
                );
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork.RegistroCupomAbastecimento.GetAll`
- `.ToShortDateString`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `PegaRegistroCupons`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Parâmetros**:

- `string IDapi`

**Código**:

```csharp
{
            try
            {
                var objRegistro = _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(rc =>
                    rc.RegistroCupomId == Guid.Parse(IDapi)
                );

                return Ok(new
                {
                    RegistroPDF = objRegistro.RegistroPDF
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "PegaRegistroCupons" ,
                    error
                );
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault`
- `Guid.Parse`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `PegaRegistroCuponsData`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Parâmetros**:

- `string id`

**Código**:

```csharp
{
            try
            {
                var result = (
                    from rc in _unitOfWork.RegistroCupomAbastecimento.GetAll()
                    where rc.DataRegistro == DateTime.Parse(id)
                    orderby rc.DataRegistro descending
                    select new
                    {
                        DataRegistro = rc.DataRegistro?.ToShortDateString() ,
                        rc.RegistroCupomId ,
                    }
                ).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "PegaRegistroCuponsData" ,
                    error
                );
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork.RegistroCupomAbastecimento.GetAll`
- `DateTime.Parse`
- `.ToShortDateString`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

### `DeleteRegistro`

**Retorno**: `IActionResult`

**HTTP**: `GET` ``

**Parâmetros**:

- `string IDapi`

**Código**:

```csharp
{
            try
            {
                var objRegistro = _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(rc =>
                    rc.RegistroCupomId == Guid.Parse(IDapi)
                );

                _unitOfWork.RegistroCupomAbastecimento.Remove(objRegistro);
                _unitOfWork.Save();

                return Ok(new
                {
                    success = true ,
                    message = "Registro excluído com sucesso!"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "DeleteRegistro" ,
                    error
                );
                return StatusCode(500);
            }
        }
```

**Métodos Chamados**:

- `_unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault`
- `Guid.Parse`
- `_unitOfWork.RegistroCupomAbastecimento.Remove`
- `_unitOfWork.Save`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `StatusCode`

---

## Dependências

| Interface                          | Campo                 | Descrição               |
| ---------------------------------- | --------------------- | ----------------------- |
| `ILogger<AbastecimentoController>` | `_logger`             | Injetado via construtor |
| `IWebHostEnvironment`              | `_hostingEnvironment` | Injetado via construtor |
| `IUnitOfWork`                      | `_unitOfWork`         | Injetado via construtor |
| `IHubContext<ImportacaoHub>`       | `_hubContext`         | Injetado via construtor |
| `FrotiXDbContext`                  | `_context`            | Injetado via construtor |

## Propriedades

| Nome               | Tipo                   | Get | Set | Atributos |
| ------------------ | ---------------------- | --- | --- | --------- |
| `AbastecimentoObj` | `Models.Abastecimento` | ✅  | ✅  |           |

## Interconexões

### Quem Chama Este Arquivo

_(Análise manual necessária)_

### O Que Este Arquivo Chama

- `ILogger<AbastecimentoController>` → Injetado via construtor
- `IWebHostEnvironment` → Injetado via construtor
- `IUnitOfWork` → Injetado via construtor
- `IHubContext<ImportacaoHub>` → Injetado via construtor
- `FrotiXDbContext` → Injetado via construtor

## Troubleshooting

### Problemas Comuns

#### Erro: Exceção não tratada

**Sintoma**: Erro 500 ao acessar a funcionalidade

**Solução**: Verificar logs em `/Documentacao/Logs/` e consultar `Alerta.TratamentoErroComLinha()`

#### Erro: Dados não persistidos

**Sintoma**: Alterações não salvas no banco

**Solução**: Verificar se `_unitOfWork.SaveChanges()` está sendo chamado

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [15/01/2026 07:22] - Documentação inicial gerada automaticamente

**Descrição**: Documentação gerada automaticamente pelo DocGenerator FrotiX.

**Arquivos Afetados**:

- `Controllers\AbastecimentoController.cs`

**Status**: ✅ **Concluído**
