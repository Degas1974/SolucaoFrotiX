# Documentacao: RepactuacaoServicosRepository.cs

> **Ultima Atualizacao**: 04/02/2026
> **Versao Atual**: 1.0

---

## Visao Geral

Repositorio especifico para repactuacoes de servicos em contratos administrativos. Ele centraliza listagens para UI (dropdown) e a atualizacao de registros da entidade RepactuacaoServicos.

## Responsabilidades

- Fornecer lista de repactuacoes de servicos para dropdowns.
- Persistir atualizacoes de valores e vinculos de contrato.
- Garantir tratamento de erro padrao com Alerta.TratamentoErroComLinha.

## Principais Metodos

### RepactuacaoServicosRepository(FrotiXDbContext db)

Construtor que injeta o contexto e valida nulidade.

### GetRepactuacaoServicosListForDropDown()

Projeta itens para dropdown usando Valor como texto e RepactuacaoContratoId como chave.

### Update(RepactuacaoServicos repactuacaoServicos)

Atualiza a entidade e salva mudancas no banco.

## Trecho Critico (Tratamento de Erro)

```csharp
try
{
    // logica principal
}
catch (Exception erro)
{
    Alerta.TratamentoErroComLinha("RepactuacaoServicosRepository.cs", "Update", erro);
    throw;
}
```

## Dependencias

- FrotiX.Data.FrotiXDbContext
- FrotiX.Models.RepactuacaoServicos
- FrotiX.Helpers.Alerta

---

## Log de Modificacoes

| Versao | Data       | Autor          | Descricao |
|--------|------------|----------------|-----------|
| 1.0    | 04/02/2026 | GitHub Copilot | Adiciona log de erro com Alerta.TratamentoErroComLinha nos catch blocks. |
