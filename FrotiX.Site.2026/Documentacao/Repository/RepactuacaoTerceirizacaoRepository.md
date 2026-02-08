# Documentacao: RepactuacaoTerceirizacaoRepository.cs

> **Ultima Atualizacao**: 04/02/2026
> **Versao Atual**: 1.0

---

## Visao Geral

Repositorio para repactuacoes de terceirizacao (motoristas, operadores e encarregados). Centraliza listagens e atualizacao da entidade RepactuacaoTerceirizacao.

## Responsabilidades

- Listar repactuacoes para dropdown.
- Atualizar registros de repactuacao de terceirizacao.
- Registrar erros via Alerta.TratamentoErroComLinha.

## Principais Metodos

### RepactuacaoTerceirizacaoRepository(FrotiXDbContext db)

Construtor com validacao do contexto.

### GetRepactuacaoTerceirizacaoListForDropDown()

Projeta itens usando ValorEncarregado e RepactuacaoContratoId.

### Update(RepactuacaoTerceirizacao repactuacaoTerceirizacao)

Atualiza a entidade e salva no banco.

## Trecho Critico (Tratamento de Erro)

```csharp
try
{
    // logica principal
}
catch (Exception erro)
{
    Alerta.TratamentoErroComLinha("RepactuacaoTerceirizacaoRepository.cs", "Update", erro);
    throw;
}
```

## Dependencias

- FrotiX.Data.FrotiXDbContext
- FrotiX.Models.RepactuacaoTerceirizacao
- FrotiX.Helpers.Alerta

---

## Log de Modificacoes

| Versao | Data       | Autor          | Descricao |
|--------|------------|----------------|-----------|
| 1.0    | 04/02/2026 | GitHub Copilot | Adiciona log de erro com Alerta.TratamentoErroComLinha nos catch blocks. |
