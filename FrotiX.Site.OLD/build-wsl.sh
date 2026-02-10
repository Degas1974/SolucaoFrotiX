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
