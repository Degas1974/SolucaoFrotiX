# Documentacao: RegistroCupomAbastecimentoRepository.cs

> **Ultima Atualizacao**: 04/02/2026
> **Versao Atual**: 1.0

---

## Visao Geral

Repositorio para registros de cupons de abastecimento digitalizados. Mantem listagens para dropdown e atualizacao de registros associados a PDF.

## Responsabilidades

- Listar registros de cupons para UI.
- Atualizar referencias de registros de cupom.
- Registrar erros via Alerta.TratamentoErroComLinha.

## Principais Metodos

### RegistroCupomAbastecimentoRepository(FrotiXDbContext db)

Construtor com validacao do contexto.

### GetRegistroCupomAbastecimentoListForDropDown()

Projeta itens ordenados por DataRegistro e exibe RegistroPDF.

### Update(RegistroCupomAbastecimento registroCupomAbastecimento)

Atualiza a entidade e salva no banco.

## Trecho Critico (Tratamento de Erro)

```csharp
try
{
    // logica principal
}
catch (Exception erro)
{
    Alerta.TratamentoErroComLinha("RegistroCupomAbastecimentoRepository.cs", "Update", erro);
    throw;
}
```

## Dependencias

- FrotiX.Data.FrotiXDbContext
- FrotiX.Models.RegistroCupomAbastecimento
- FrotiX.Helpers.Alerta

---

## Log de Modificacoes

| Versao | Data       | Autor          | Descricao |
|--------|------------|----------------|-----------|
| 1.0    | 04/02/2026 | GitHub Copilot | Adiciona log de erro com Alerta.TratamentoErroComLinha nos catch blocks. |
