# Documentacao: launchSettings.json

> **Ultima Atualizacao**: 04/02/2026
> **Versao Atual**: 1.0

---

## Visao Geral

Este arquivo define os profiles de execucao do FrotiX.Site no Visual Studio, incluindo URLs de aplicacao e comportamento de abertura do navegador.

## Localizacao

- Properties/launchSettings.json

## Perfil FrotiX.Web

- commandName: Project
- launchBrowser: true
- launchUrl: ""
- applicationUrl: http://localhost:5000

## Observacoes

- O profile FrotiX.Web e o alvo principal para executar via F5/Ctrl+F5.
- O launchBrowser habilita abertura automatica do navegador.

---

## Log de Modificacoes

| Versao | Data       | Autor          | Descricao |
|--------|------------|----------------|-----------|
| 1.0    | 04/02/2026 | GitHub Copilot | Ajusta FrotiX.Web para abrir http://localhost:5000 e launchBrowser ativo. |
