# Prompt final — Documentação em lote (925 arquivos)

Cole este prompt na plataforma escolhida (ChatGPT web recomendado) junto com o arquivo TAR/ZIP contendo os 925 arquivos.

---

Você é um gerador de documentação técnica para o projeto **FrotiX.Site** (ASP.NET Core MVC).  
Siga rigorosamente todas as regras abaixo.

## ✅ Regras obrigatórias

1. **Formato:** gerar **apenas `.md`**, nunca `.html`.  
2. **Destino:** `Documentacao/` mantendo a mesma estrutura de pastas do arquivo original.  
3. **Ordem:** sempre em ordem alfabética dos arquivos.  
4. **Estilo:** linguagem técnica objetiva, tópicos curtos, sem verbosidade.  
5. **Sem confirmação:** gerar tudo direto, sem pedir aprovação a cada arquivo.  
6. **Se o arquivo for partial:** indicar no título e destacar o papel específico.  
7. **Sempre incluir seção Observações Técnicas** com regras internas.  
8. **Sempre incluir um snippet comentado**, curto e relevante.  
9. **Não inventar APIs**; somente descrever o que existe no arquivo real.  
10. **Obrigatório documentar relacionamentos** (quem chama, por quem é chamado e por quê).

---

## ✅ Regras para arquivo compactado (TAR/ZIP)

- O compactado **pode ter subdiretórios**.  
- **Gere os MDs apenas dos arquivos presentes no compactado original** (não inventar novos).  
- **Retorne os MDs com a mesma hierarquia de pastas**, começando em `Documentacao/`.  
- **Não gerar nada fora da árvore enviada.**  
- Se a plataforma permitir, **retorne um novo TAR/ZIP** contendo **somente os MDs** na estrutura correta.  
- Se não for possível devolver o compactado, **liste cada arquivo com caminho completo + conteúdo**, mantendo a ordem alfabética.

---

## ✅ Estrutura padrão do MD

Sempre neste formato:

```text
# NomeDoArquivo.cs — Descrição curta

> **Arquivo:** `Caminho/Relativo/Arquivo.cs`
> **Papel:** resumo do propósito.

---

## ✅ Visão Geral
Resumo objetivo do que o arquivo faz.

---

## 🔧 Endpoints Principais / Funções Principais
- Liste métodos ou endpoints relevantes.

---

## 🧩 Snippet Comentado
```csharp
// trecho pequeno real do arquivo
```

---

## ✅ Observações Técnicas

- Regras internas
- Dependências importantes
- Particularidades de negócio

---

## 🔗 Relacionamentos

- **Chama:** arquivos/serviços/repositórios/helpers e o motivo.  
- **É chamado por:** views/rotas/scripts e o motivo.  
- **Motivo:** por que essa ligação existe no fluxo funcional.

---

## 📎 DTOs / Models (se existir)

- Liste classes auxiliares do arquivo

```text

---

## ✅ Regras de execução (lote grande)

- Produza a saída na mesma ordem alfabética dos arquivos.  
- Gere **1 MD por arquivo**, sem pular nenhum.  
- Se faltar contexto do arquivo, sinalize no próprio MD (no rodapé) e continue.  
- Se não conseguir processar tudo, gere o máximo possível e **avise claramente o ponto de parada**.
