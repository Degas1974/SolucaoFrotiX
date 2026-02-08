"""Scan FrotiX.Atualizado to map files by batch/directory."""
import os
from collections import defaultdict

OUT = 'FrotiX.Atualizado'
files = []
for root, dirs, fnames in os.walk(OUT):
    for fn in fnames:
        if fn.endswith('.md') and fn != 'INDICE.md':
            rel = os.path.relpath(os.path.join(root, fn), OUT).replace('\\', '/')
            with open(os.path.join(root, fn), 'r', encoding='utf-8') as f:
                head = f.read(500)
            if 'ARQUIVO NOVO' in head:
                tipo = 'NOVO'
            elif 'ARQUIVO REMOVIDO' in head:
                tipo = 'REMOVIDO'
            else:
                tipo = 'MODIFICADO'
            actual = rel[:-3]  # remove .md suffix
            files.append((actual, tipo))

groups = defaultdict(list)
for fp, tp in files:
    parts = fp.split('/')
    if parts[0] == 'wwwroot':
        key = '/'.join(parts[:2])
    elif parts[0] in ('Models', 'Repository', 'Pages', 'Areas'):
        key = '/'.join(parts[:2]) if len(parts) > 1 else parts[0]
    else:
        key = parts[0]
    groups[key].append((fp, tp))

print("=" * 70)
print(f"{'DIRETORIO':<35} {'TOTAL':>5} {'MOD':>5} {'NOVO':>5} {'DEL':>5}")
print("=" * 70)
total_all = 0
for key in sorted(groups.keys()):
    items = groups[key]
    novos = sum(1 for _, t in items if t == 'NOVO')
    removidos = sum(1 for _, t in items if t == 'REMOVIDO')
    mods = sum(1 for _, t in items if t == 'MODIFICADO')
    print(f"{key:<35} {len(items):>5} {mods:>5} {novos:>5} {removidos:>5}")
    total_all += len(items)
print("=" * 70)
print(f"{'TOTAL':<35} {total_all:>5}")
