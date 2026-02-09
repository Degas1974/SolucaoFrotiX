#!/bin/bash

echo "⌨️ Aplicando keybindings.json..."

KEYBINDINGS_PATH="$HOME/.config/Code - Insiders/User/keybindings.json"

mkdir -p "$(dirname "$KEYBINDINGS_PATH")"
cp keybindings.json "$KEYBINDINGS_PATH"

echo "✅ keybindings.json aplicado!"