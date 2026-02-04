# Documentacao: RepactuacaoContratoRepository.cs

> **Ultima Atualizacao**: 04/02/2026
> **Versao Atual**: 1.0

---

## Visao Geral

Repositorio para repactuacoes e aditivos de contratos administrativos. Fornece listagens para dropdowns e atualizacao de registros da entidade RepactuacaoContrato.

## Responsabilidades

- Listar repactuacoes de contrato para UI.
- Atualizar registros de repactuacao de contrato.
- Registrar erros via Alerta.TratamentoErroComLinha.

## Principais Metodos

### RepactuacaoContratoRepository(FrotiXDbContext db)

Construtor com validacao do contexto.

### GetRepactuacaoContratoListForDropDown()

Projeta itens ordenados por Descricao.

### Update(RepactuacaoContrato repactuacaoContrato)

Atualiza a entidade e salva no banco.

## Trecho Critico (Tratamento de Erro)

```csharp
try
{
    // logica principal
}
catch (Exception erro)
{
    Alerta.TratamentoErroComLinha("RepactuacaoContratoRepository.cs", "Update", erro);
    throw;
}
```

## Dependencias

- FrotiX.Data.FrotiXDbContext
- FrotiX.Models.RepactuacaoContrato
- FrotiX.Helpers.Alerta

---

## Log de Modificacoes

| Versao | Data       | Autor          | Descricao |
|--------|------------|----------------|-----------|
| 1.0    | 04/02/2026 | GitHub Copilot | Adiciona log de erro com Alerta.TratamentoErroComLinha nos catch blocks. |
