import os, re, sys

BASE = 'FrotiX.Site.2026.01'
TARGET = 'FrotiX.Site'

def strip_comments(content, ext):
    if ext in ('.cs', '.js'):
        content = re.sub(r'/\*[\s\S]*?\*/', '', content)
        content = re.sub(r'^\s*///.*$', '', content, flags=re.MULTILINE)
        content = re.sub(r'(?<!:)//(?!/)[^\n]*', '', content)
        content = re.sub(r'^\s*#(region|endregion).*$', '', content, flags=re.MULTILINE)
    elif ext == '.cshtml':
        content = re.sub(r'@\*[\s\S]*?\*@', '', content)
        content = re.sub(r'<!--[\s\S]*?-->', '', content)
        content = re.sub(r'^\s*///.*$', '', content, flags=re.MULTILINE)
        content = re.sub(r'(?<!:)//(?!/)[^\n]*', '', content)
    elif ext == '.css':
        content = re.sub(r'/\*[\s\S]*?\*/', '', content)
    lines = [line.rstrip() for line in content.split('\n') if line.strip()]
    return '\n'.join(lines)

def has_code_changes(filepath):
    ext = os.path.splitext(filepath)[1].lower()
    base_path = os.path.join(BASE, filepath)
    target_path = os.path.join(TARGET, filepath)
    if not os.path.exists(base_path) or not os.path.exists(target_path):
        return True
    try:
        with open(base_path, 'r', encoding='utf-8', errors='replace') as f:
            base_content = f.read()
        with open(target_path, 'r', encoding='utf-8', errors='replace') as f:
            target_content = f.read()
    except Exception:
        return True
    base_stripped = strip_comments(base_content, ext)
    target_stripped = strip_comments(target_content, ext)
    return base_stripped != target_stripped

with open('C:/tmp/all_diff_files.txt', 'r') as f:
    files = [line.strip() for line in f if line.strip()]

code_changes = []
docs_only = []
for i, fp in enumerate(files):
    if has_code_changes(fp):
        code_changes.append(fp)
    else:
        docs_only.append(fp)
    if (i + 1) % 200 == 0:
        print(f'  {i + 1}/{len(files)}...', file=sys.stderr)

with open('C:/tmp/code_changes_final.txt', 'w') as f:
    f.write('\n'.join(code_changes))
with open('C:/tmp/docs_only_final.txt', 'w') as f:
    f.write('\n'.join(docs_only))
print(f'CODE changes: {len(code_changes)}')
print(f'DOCS-only: {len(docs_only)}')
