# Setup WSL - Build FrotiX.Site

## Objetivo

Este documento descreve o fluxo de build do FrotiX.Site em WSL, usando um NuGet config separado para nao afetar o ambiente Windows. O foco e manter o repo no WSL, evitar alteracoes de EOL, e garantir restore/build com fontes locais de pacotes.

## Estrutura de Arquivos

- FrotiX.Site/NuGet.WSL.config
- FrotiX.Site/build-wsl.sh
- /home/degas/FrotiX/NuGets (espelho de pacotes locais)

## Passo a Passo

1. Copiar os pacotes locais para o WSL em /home/degas/FrotiX/NuGets.
2. Verificar se os caminhos do NuGet.WSL.config apontam para as pastas corretas.
3. Executar o script build-wsl.sh a partir do FrotiX.Site.

## Node modules (bs5-patcher)

O build depende de assets em wwwroot/js/bs5-patcher/node_modules. Se o build falhar com erro de package.json ausente, copie o node_modules do Windows para o WSL:

```bash
cp -a "/mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/wwwroot/js/bs5-patcher/node_modules/." "/home/degas/FrotiX/Solucao FrotiX 2026/FrotiX.Site/wwwroot/js/bs5-patcher/node_modules/"
```

## Build Release

Para validar Release no WSL:

```bash
cd "/home/degas/FrotiX/Solucao FrotiX 2026/FrotiX.Site"
dotnet build FrotiX.sln -c Release --no-restore
```

## NuGet.WSL.config

O arquivo abaixo mantem as mesmas sources do Windows, mas com caminhos Linux:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="NuGet" value="https://api.nuget.org/v3/index.json" />
    <add key="Stimulsoft Reports 2023.1.1" value="/home/degas/FrotiX/NuGets/Stimulsoft Reports 2023.1.1" />
    <add key="TeleriK Reporting 18.1.24.514" value="/home/degas/FrotiX/NuGets/Telerik.Reporting.Nupkg.18.1.24.514" />
    <add key="Telerik 2023 R1 SP2" value="/home/degas/FrotiX/NuGets/Telerik.Nupkg.2023R1SP2" />
    <add key="Telerik 2025" value="/home/degas/FrotiX/NuGets/Telerik Collection for .NET 2025 Q2" allowInsecureConnections="True" />
  </packageSources>
  <fallbackPackageFolders>
    <clear />
  </fallbackPackageFolders>
</configuration>
```

## build-wsl.sh

Script para restore/build usando o NuGet config do WSL:

```bash
#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
solution_path="$script_dir/FrotiX.sln"
config_path="$script_dir/NuGet.WSL.config"

if [[ ! -f "$config_path" ]]; then
  echo "NuGet.WSL.config not found: $config_path" >&2
  exit 1
fi

dotnet restore "$solution_path" --configfile "$config_path"
dotnet build "$solution_path" -c Debug --no-restore
```

## Observacoes

- O NuGet.config padrao do Windows permanece intacto.
- O NuGet.WSL.config deve permanecer ignorado no git.
- O espelho local e necessario para pacotes Stimulsoft/Telerik.
- Warning TKL002 (Telerik/Kendo) indica ausencia de license file no WSL. Coloque o arquivo em /home/degas/.telerik/telerik-license.txt ou defina TELERIK_LICENSE_PATH.
- Warnings CS0168/CS0414 e ASP0000 aparecem no build e nao impedem a compilacao.

## Log de Modificacoes

| Versao | Data       | Autor           | Descricao |
|--------|------------|-----------------|-----------|
| 1.0    | 04/02/2026 | GitHub Copilot  | Cria setup WSL com NuGet config separado e script de build. |
| 1.1    | 04/02/2026 | GitHub Copilot  | Documenta node_modules do bs5-patcher, build Release e warnings do build. |
