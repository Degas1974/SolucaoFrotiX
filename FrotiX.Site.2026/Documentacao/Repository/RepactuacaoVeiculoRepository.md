# Documentacao: RepactuacaoVeiculoRepository.cs

> **Ultima Atualizacao**: 04/02/2026
> **Versao Atual**: 1.0

---

## Visao Geral

Repositorio especifico para repactuacoes de valores de locacao de veiculos. Mantem listagens para dropdown e atualiza valores, observacoes e vinculos do contrato.

## Responsabilidades

- Listar repactuacoes de veiculos para UI.
- Atualizar campos de repactuacao de veiculo.
- Registrar erros via Alerta.TratamentoErroComLinha.

## Principais Metodos

### RepactuacaoVeiculoRepository(FrotiXDbContext db)

Construtor com validacao do contexto.

### GetRepactuacaoVeiculoListForDropDown()

Retorna itens ordenados por RepactuacaoVeiculoId para dropdowns.

### Update(RepactuacaoVeiculo repactuacaoVeiculo)

Atualiza Valor, Observacao, VeiculoId e RepactuacaoContratoId, salvando no banco.

## Trecho Critico (Tratamento de Erro)

```csharp
try
{
    // logica principal
}
catch (Exception erro)
{
    Alerta.TratamentoErroComLinha("RepactuacaoVeiculoRepository.cs", "Update", erro);
    throw;
}
```

## Dependencias

- FrotiX.Data.FrotiXDbContext
- FrotiX.Models.RepactuacaoVeiculo
- FrotiX.Helpers.Alerta

---

## Log de Modificacoes

| Versao | Data       | Autor          | Descricao |
|--------|------------|----------------|-----------|
| 1.0    | 04/02/2026 | GitHub Copilot | Adiciona log de erro com Alerta.TratamentoErroComLinha nos catch blocks. |
