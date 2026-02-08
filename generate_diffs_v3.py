"""
V3 - Generate CLEAN MD diffs for FrotiX.Atualizado.
Strips comments AND normalizes formatting before diffing.
Only LOGIC changes appear in the diff.
"""
import os, re, sys, difflib, time, shutil

BASE_DIR = 'FrotiX.Site.2026.01'
TARGET_DIR = 'FrotiX.Site'
OUTPUT_DIR = 'FrotiX.Atualizado'

def strip_comments(content, ext):
    """Remove all comments/docs."""
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
    return content

def _clean_blank_lines(content):
    """Remove consecutive blank lines (keep max 1), trim leading/trailing."""
    lines = [line.rstrip() for line in content.split('\n')]
    result = []
    prev_blank = False
    for line in lines:
        if line.strip() == '':
            if not prev_blank:
                result.append('')
            prev_blank = True
        else:
            result.append(line)
            prev_blank = False
    while result and result[0].strip() == '':
        result.pop(0)
    while result and result[-1].strip() == '':
        result.pop()
    return '\n'.join(result)

def normalize_for_compare(content):
    """Aggressive normalization - used ONLY to check if files have real logic changes."""
    content = content.replace('\ufeff', '').replace('\r\n', '\n').replace('\r', '\n')
    content = re.sub(r'\{\s*get;\s*set;\s*\}', '{ get; set; }', content)
    content = re.sub(r'>\s*\n\s*where\s+', '> where ', content)
    content = re.sub(r'\s*,\s*', ', ', content)
    content = re.sub(r'\s*:\s*', ' : ', content)

    def join_parens(m):
        inner = m.group(0)
        joined = re.sub(r'\s*\n\s*', ' ', inner)
        joined = re.sub(r'  +', ' ', joined)
        return joined

    content = re.sub(r'\([^)]{0,2000}?\)', join_parens, content, flags=re.DOTALL)
    content = re.sub(r'new\s+\w+\s*\([^)]{0,3000}?\)', join_parens, content, flags=re.DOTALL)
    content = re.sub(
        r'(public|private|protected|internal)\s+(static\s+)?(readonly\s+)?(\w+[\w<>,\s\?]*?)\s+(\w+)\s*\n\s*\{\s*\n\s*get;\s*set;\s*\n\s*\}',
        r'\1 \2\3\4 \5 { get; set; }', content)
    content = re.sub(r'(\w+)\s*\n\s*\{\s*get;\s*set;\s*\}', r'\1 { get; set; }', content)
    content = re.sub(r'\n\s*\{\s*get;\s*set;\s*\}', ' { get; set; }', content)
    content = re.sub(r'\(\s*\)\s*\{\s*\}', '() { }', content)
    content = re.sub(r'  +', ' ', content)
    return _clean_blank_lines(content)

def normalize_for_display(content):
    """Light normalization - preserves indentation (4-space tabs) for readable diffs."""
    content = content.replace('\ufeff', '').replace('\r\n', '\n').replace('\r', '\n')

    # Convert tabs to 4 spaces
    content = content.expandtabs(4)

    # Normalize indentation: detect original indent unit and convert to 4 spaces
    lines = content.split('\n')
    normalized = []
    for line in lines:
        stripped = line.lstrip(' ')
        if stripped == '' or stripped == line:
            normalized.append(line)
            continue
        leading = len(line) - len(stripped)
        # Collapse inline multiple spaces (not leading)
        stripped = re.sub(r'  +', ' ', stripped)
        normalized.append(' ' * leading + stripped)
    content = '\n'.join(normalized)

    return _clean_blank_lines(content)

def get_clean_compare(content, ext):
    """Strip comments -> aggressive normalize (for equality check)."""
    return normalize_for_compare(strip_comments(content, ext))

def get_clean_display(content, ext):
    """Strip comments -> light normalize preserving indentation (for diff output)."""
    return normalize_for_display(strip_comments(content, ext))

def read_file(path):
    try:
        with open(path, 'r', encoding='utf-8', errors='replace') as f:
            return f.read()
    except Exception:
        return None

def create_md_modified(filepath, base_content, target_content):
    """Create MD with ONLY logic changes visible, preserving indentation."""
    ext = os.path.splitext(filepath)[1].lower()
    lang = {'.cs': 'csharp', '.cshtml': 'html', '.js': 'javascript',
            '.css': 'css', '.sql': 'sql'}.get(ext, '')

    # Aggressive compare to decide if there are real logic changes
    if get_clean_compare(base_content, ext) == get_clean_compare(target_content, ext):
        return None  # No logic changes

    # Display version preserves indentation
    base_display = get_clean_display(base_content, ext)
    target_display = get_clean_display(target_content, ext)

    base_lines = base_display.split('\n')
    target_lines = target_display.split('\n')

    diff = list(difflib.unified_diff(
        base_lines, target_lines,
        fromfile=f'JANEIRO: {filepath}',
        tofile=f'ATUAL: {filepath}',
        lineterm='', n=3
    ))
    diff_text = '\n'.join(diff)

    # Count real changes
    added = [l for l in diff if l.startswith('+') and not l.startswith('+++') and l[1:].strip()]
    removed = [l for l in diff if l.startswith('-') and not l.startswith('---') and l[1:].strip()]

    # Classify the change
    if len(added) + len(removed) <= 5:
        size = "PEQUENA"
    elif len(added) + len(removed) <= 30:
        size = "MEDIA"
    else:
        size = "GRANDE"

    md = f"""# {filepath}

**Mudanca:** {size} | **+{len(added)}** linhas | **-{len(removed)}** linhas

---

```diff
{diff_text}
```
"""

    # For small/medium changes, add actionable summary
    if len(added) + len(removed) <= 80:
        if removed:
            md += "\n### REMOVER do Janeiro\n\n```" + lang + "\n"
            for line in removed:
                md += line[1:] + "\n"
            md += "```\n\n"

        if added:
            md += "\n### ADICIONAR ao Janeiro\n\n```" + lang + "\n"
            for line in added:
                md += line[1:] + "\n"
            md += "```\n"

    return md

def create_md_new(filepath, content):
    ext = os.path.splitext(filepath)[1].lower()
    lang = {'.cs': 'csharp', '.cshtml': 'html', '.js': 'javascript',
            '.css': 'css', '.sql': 'sql'}.get(ext, '')
    clean = get_clean_display(content, ext)
    lines = [l for l in clean.split('\n') if l.strip()]
    if len(lines) > 500:
        display = '\n'.join(clean.split('\n')[:500]) + f'\n\n... (+{len(lines)-500} linhas)'
    else:
        display = clean
    return f"""# {filepath}

**ARQUIVO NOVO** | {len(lines)} linhas de codigo

> Copiar integralmente para o Janeiro.

---

```{lang}
{display}
```
"""

def create_md_deleted(filepath, content):
    return f"""# {filepath}

**ARQUIVO REMOVIDO**

> Existia no Janeiro, foi removido. Avaliar se deve ser deletado.
"""

def write_md(filepath, md_content):
    md_path = os.path.join(OUTPUT_DIR, filepath + '.md')
    os.makedirs(os.path.dirname(md_path), exist_ok=True)
    with open(md_path, 'w', encoding='utf-8') as f:
        f.write(md_content)

def main():
    start = time.time()
    if os.path.exists(OUTPUT_DIR):
        shutil.rmtree(OUTPUT_DIR)
    os.makedirs(OUTPUT_DIR)

    # Modified files
    print("=== Modificados ===")
    with open('C:/tmp/code_changes_final.txt', 'r') as f:
        files = [l.strip() for l in f if l.strip()]

    processed = 0
    skipped = 0
    for fp in files:
        bc = read_file(os.path.join(BASE_DIR, fp))
        tc = read_file(os.path.join(TARGET_DIR, fp))
        if not bc or not tc:
            skipped += 1
            continue
        md = create_md_modified(fp, bc, tc)
        if md is None:
            skipped += 1
            continue
        write_md(fp, md)
        processed += 1
        if processed % 100 == 0:
            print(f"  {processed} ({time.time()-start:.0f}s)")

    print(f"  Processados: {processed} | Ignorados: {skipped}")

    # New files
    print("=== Novos ===")
    new_count = 0
    skip_dirs = {'bin', 'obj', 'node_modules', 'Documentacao', 'Conversas',
                 '.claude', '.cursor', '.gemini', '.continue', '.github'}
    for root, dirs, fnames in os.walk(TARGET_DIR):
        rel_root = os.path.relpath(root, TARGET_DIR).replace('\\', '/')
        if any(sd in rel_root.split('/') for sd in skip_dirs):
            continue
        for fn in fnames:
            ext = os.path.splitext(fn)[1].lower()
            if ext not in ('.cs', '.cshtml', '.js', '.css', '.sql'):
                continue
            rp = os.path.relpath(os.path.join(root, fn), TARGET_DIR).replace('\\', '/')
            if not os.path.exists(os.path.join(BASE_DIR, rp)):
                c = read_file(os.path.join(root, fn))
                if c:
                    write_md(rp, create_md_new(rp, c))
                    new_count += 1
    print(f"  Novos: {new_count}")

    # Deleted files
    print("=== Removidos ===")
    del_count = 0
    for root, dirs, fnames in os.walk(BASE_DIR):
        rel_root = os.path.relpath(root, BASE_DIR).replace('\\', '/')
        if any(sd in rel_root.split('/') for sd in skip_dirs):
            continue
        for fn in fnames:
            ext = os.path.splitext(fn)[1].lower()
            if ext not in ('.cs', '.cshtml', '.js', '.css', '.sql'):
                continue
            rp = os.path.relpath(os.path.join(root, fn), BASE_DIR).replace('\\', '/')
            if not os.path.exists(os.path.join(TARGET_DIR, rp)):
                c = read_file(os.path.join(root, fn))
                if c:
                    write_md(rp, create_md_deleted(rp, c))
                    del_count += 1
    print(f"  Removidos: {del_count}")

    total = processed + new_count + del_count
    print(f"\n{'='*50}")
    print(f"TOTAL: {total} MDs ({processed} mod + {new_count} new + {del_count} del)")
    print(f"Ignorados (sem mudanca logica): {skipped}")
    print(f"Tempo: {time.time()-start:.1f}s")
    print(f"{'='*50}")

    # Index
    md_files = []
    for root, dirs, fnames in os.walk(OUTPUT_DIR):
        for fn in sorted(fnames):
            if fn.endswith('.md') and fn != 'INDICE.md':
                md_files.append(os.path.relpath(os.path.join(root, fn), OUTPUT_DIR).replace('\\', '/'))
    md_files.sort()

    idx = f"""# INDICE - FrotiX.Atualizado (v3 - so logica)

## O que e isto?
Mudancas de **LOGICA** entre `FrotiX.Site.2026.01` (Janeiro) e `FrotiX.Site` (atual).

Filtrados: comentarios, documentacao, formatacao (espacos, quebras de linha, {{ get; set; }}).
Voce ve **apenas codigo que mudou de verdade**.

| Tipo | Qtd |
|------|-----|
| Modificados | {processed} |
| Novos | {new_count} |
| Removidos | {del_count} |
| **Total** | **{total}** |

## Como usar
1. Abra cada .md
2. **diff**: `-` = remover do Janeiro, `+` = adicionar ao Janeiro
3. Secoes "REMOVER" e "ADICIONAR" para mudancas pequenas/medias
4. **NOVO**: Copiar arquivo inteiro
5. **REMOVIDO**: Avaliar se deletar

## Arquivos

"""
    cur_dir = ''
    for f in md_files:
        d = os.path.dirname(f) or '(raiz)'
        if d != cur_dir:
            cur_dir = d
            idx += f"\n### {d}\n\n"
        idx += f"- [{os.path.basename(f)}]({f})\n"

    with open(os.path.join(OUTPUT_DIR, 'INDICE.md'), 'w', encoding='utf-8') as fp:
        fp.write(idx)
    print("Indice OK.")

if __name__ == '__main__':
    main()
