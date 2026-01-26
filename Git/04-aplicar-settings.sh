#!/bin/bash

echo "⚙️ Aplicando settings.json..."

SETTINGS_PATH="$HOME/.config/Code - Insiders/User/settings.json"

mkdir -p "$(dirname "$SETTINGS_PATH")"
cp settings.json "$SETTINGS_PATH"

echo "✅ settings.json aplicado!"
