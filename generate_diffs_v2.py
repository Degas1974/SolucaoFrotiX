"""
Generate CLEAN MD diff files for FrotiX.Atualizado.
Shows ONLY meaningful code changes (strips comments, docs, formatting noise).
Format designed for easy copy-paste into the January version.
"""
import os, re, sys, difflib, time, shutil

BASE_DIR = 'FrotiX.Site.2026.01'
TARGET_DIR = 'FrotiX.Site'
OUTPUT_DIR = 'FrotiX.Atualizado'

def strip_comments(content, ext):
    """Remove all comments/docs, keeping only executable code."""
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

def normalize_code(content):
    """Normalize whitespace/formatting to ignore trivial differences."""
    # Remove BOM
    content = content.replace('\ufeff', '')
    # Normalize line endings
    content = content.replace('\r\n', '\n').replace('\r', '\n')
    # Remove trailing whitespace per line
    lines = [line.rstrip() for line in content.split('\n')]
    # Remove consecutive blank lines (keep max 1)
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
    # Remove leading/trailing blank lines
    while result and result[0].strip() == '':
        result.pop(0)
    while result and result[-1].strip() == '':
        result.pop()
    return '\n'.join(result)

def normalize_formatting(content):
    """Normalize C# formatting quirks that don't change logic."""
    # Normalize spaces before commas: "foo , bar" -> "foo, bar"
    content = re.sub(r'\s+,', ',', content)
    # Normalize "get; set;" on multiple lines to single line
    content = re.sub(r'\{\s*get;\s*set;\s*\}', '{ get; set; }', content)
    # Normalize ": Controller" vs " :Controller" etc
    content = re.sub(r'\s*:\s*', ' : ', content)
    # Collapse multiple spaces to single
    content = re.sub(r'  +', ' ', content)
    return content

def read_file(path):
    try:
        with open(path, 'r', encoding='utf-8', errors='replace') as f:
            return f.read()
    except Exception:
        return None

def get_clean_code(content, ext, deep_normalize=False):
    """Get clean code without comments, normalized."""
    code = strip_comments(content, ext)
    code = normalize_code(code)
    if deep_normalize:
        code = normalize_formatting(code)
    return code

def generate_clean_diff(base_lines, target_lines, filepath):
    """Generate a context diff showing only meaningful changes."""
    diff = list(difflib.unified_diff(
        base_lines,
        target_lines,
        fromfile=f'JANEIRO (base): {filepath}',
        tofile=f'ATUAL (mudado): {filepath}',
        lineterm='',
        n=2  # 2 lines of context
    ))
    return '\n'.join(diff)

def extract_changes_summary(base_code, target_code, filepath, ext):
    """Extract a human-readable summary of code changes."""
    base_lines = base_code.split('\n')
    target_lines = target_code.split('\n')

    # Get the unified diff
    diff_lines = list(difflib.unified_diff(
        base_lines, target_lines,
        lineterm='', n=0  # no context for counting
    ))

    added = [l[1:] for l in diff_lines if l.startswith('+') and not l.startswith('+++')]
    removed = [l[1:] for l in diff_lines if l.startswith('-') and not l.startswith('---')]

    # Filter out empty/whitespace-only lines
    added = [l for l in added if l.strip()]
    removed = [l for l in removed if l.strip()]

    return added, removed

def create_md_modified(filepath, base_content, target_content):
    """Create a CLEAN MD showing only code changes."""
    ext = os.path.splitext(filepath)[1].lower()
    lang = {'.cs': 'csharp', '.cshtml': 'html', '.js': 'javascript', '.css': 'css', '.sql': 'sql'}.get(ext, '')

    # Get clean code (no comments)
    base_clean = get_clean_code(base_content, ext)
    target_clean = get_clean_code(target_content, ext)

    # Also get deeply normalized for detecting formatting-only changes
    base_deep = get_clean_code(base_content, ext, deep_normalize=True)
    target_deep = get_clean_code(target_content, ext, deep_normalize=True)

    if base_deep == target_deep:
        return None  # Only formatting differences, skip

    base_lines = base_clean.split('\n')
    target_lines = target_clean.split('\n')

    # Get summary
    added, removed = extract_changes_summary(base_clean, target_clean, filepath, ext)

    # Generate context diff
    diff_text = generate_clean_diff(base_lines, target_lines, filepath)

    # Build the MD
    md = f"""# {filepath}

**Status:** Modificado | **Linhas adicionadas:** {len(added)} | **Linhas removidas:** {len(removed)}

---

## Mudancas de Codigo (sem comentarios/docs)

```diff
{diff_text}
```

"""

    # If changes are small enough, also show specific "what to do" instructions
    if len(added) + len(removed) <= 60:
        if removed:
            md += "## REMOVER do Janeiro (linhas que foram excluidas)\n\n```" + lang + "\n"
            for line in removed:
                md += line + "\n"
            md += "```\n\n"

        if added:
            md += "## ADICIONAR ao Janeiro (linhas novas)\n\n```" + lang + "\n"
            for line in added:
                md += line + "\n"
            md += "```\n\n"

    return md

def create_md_new(filepath, content):
    """Create MD for a new file."""
    ext = os.path.splitext(filepath)[1]
    lang = {'.cs': 'csharp', '.cshtml': 'html', '.js': 'javascript', '.css': 'css', '.sql': 'sql'}.get(ext, '')

    clean = get_clean_code(content, ext)
    lines = clean.split('\n')
    line_count = len([l for l in lines if l.strip()])

    if len(lines) > 500:
        display = '\n'.join(lines[:500]) + f'\n\n... ({len(lines) - 500} linhas omitidas)'
    else:
        display = clean

    md = f"""# {filepath}

**Status:** ARQUIVO NOVO | **Linhas de codigo:** {line_count}

> Este arquivo NAO existe no Janeiro. Copie ele integralmente para o projeto.

---

## Codigo (sem comentarios/docs)

```{lang}
{display}
```
"""
    return md

def create_md_deleted(filepath, content):
    """Create MD for a deleted file."""
    ext = os.path.splitext(filepath)[1]

    md = f"""# {filepath}

**Status:** REMOVIDO

> Este arquivo existia no Janeiro mas foi REMOVIDO na versao atual.
> Acao: Avaliar se deve ser deletado do Janeiro tambem.
"""
    return md

def write_md(filepath, md_content):
    md_path = os.path.join(OUTPUT_DIR, filepath + '.md')
    os.makedirs(os.path.dirname(md_path), exist_ok=True)
    with open(md_path, 'w', encoding='utf-8') as f:
        f.write(md_content)

def main():
    start_time = time.time()

    # Clean output directory
    if os.path.exists(OUTPUT_DIR):
        shutil.rmtree(OUTPUT_DIR)
    os.makedirs(OUTPUT_DIR)

    # Phase 1: Modified files
    print("=== Fase 1: Arquivos Modificados ===")
    with open('C:/tmp/code_changes_final.txt', 'r') as f:
        modified_files = [line.strip() for line in f if line.strip()]

    processed = 0
    skipped_formatting = 0
    skipped_missing = 0

    for filepath in modified_files:
        base_content = read_file(os.path.join(BASE_DIR, filepath))
        target_content = read_file(os.path.join(TARGET_DIR, filepath))

        if base_content is None or target_content is None:
            skipped_missing += 1
            continue

        md = create_md_modified(filepath, base_content, target_content)
        if md is None:
            skipped_formatting += 1
            continue

        write_md(filepath, md)
        processed += 1

        if processed % 50 == 0:
            elapsed = time.time() - start_time
            print(f"  {processed} processados ({elapsed:.0f}s)")

    print(f"  Modificados com mudancas reais: {processed}")
    print(f"  Ignorados (so formatacao): {skipped_formatting}")
    print(f"  Ignorados (arquivo faltando): {skipped_missing}")

    # Phase 2: New files
    print("\n=== Fase 2: Arquivos Novos ===")
    new_count = 0
    skip_dirs = {'bin', 'obj', 'node_modules', 'Documentacao', 'Conversas',
                 '.claude', '.cursor', '.gemini', '.continue', '.github'}

    for root, dirs, files in os.walk(TARGET_DIR):
        rel_root = os.path.relpath(root, TARGET_DIR).replace('\\', '/')
        if any(sd in rel_root.split('/') for sd in skip_dirs):
            continue
        for fname in files:
            ext = os.path.splitext(fname)[1].lower()
            if ext not in ('.cs', '.cshtml', '.js', '.css', '.sql'):
                continue
            rel_path = os.path.relpath(os.path.join(root, fname), TARGET_DIR).replace('\\', '/')
            if not os.path.exists(os.path.join(BASE_DIR, rel_path)):
                content = read_file(os.path.join(root, fname))
                if content:
                    md = create_md_new(rel_path, content)
                    write_md(rel_path, md)
                    new_count += 1

    print(f"  Novos: {new_count}")

    # Phase 3: Deleted files
    print("\n=== Fase 3: Arquivos Removidos ===")
    deleted_count = 0
    for root, dirs, files in os.walk(BASE_DIR):
        rel_root = os.path.relpath(root, BASE_DIR).replace('\\', '/')
        if any(sd in rel_root.split('/') for sd in skip_dirs):
            continue
        for fname in files:
            ext = os.path.splitext(fname)[1].lower()
            if ext not in ('.cs', '.cshtml', '.js', '.css', '.sql'):
                continue
            rel_path = os.path.relpath(os.path.join(root, fname), BASE_DIR).replace('\\', '/')
            if not os.path.exists(os.path.join(TARGET_DIR, rel_path)):
                content = read_file(os.path.join(root, fname))
                if content:
                    md = create_md_deleted(rel_path, content)
                    write_md(rel_path, md)
                    deleted_count += 1

    print(f"  Removidos: {deleted_count}")

    # Summary
    total = processed + new_count + deleted_count
    elapsed = time.time() - start_time
    print(f"\n{'='*60}")
    print(f"RESUMO FINAL:")
    print(f"  Modificados (codigo real): {processed}")
    print(f"  Novos:                     {new_count}")
    print(f"  Removidos:                 {deleted_count}")
    print(f"  TOTAL MDs gerados:         {total}")
    print(f"  Ignorados (so formatacao): {skipped_formatting}")
    print(f"  Tempo:                     {elapsed:.1f}s")
    print(f"{'='*60}")

    # Generate index
    generate_index(processed, new_count, deleted_count, skipped_formatting)

def generate_index(mod_count, new_count, del_count, fmt_count):
    """Generate index file."""
    md_files = []
    for root, dirs, files in os.walk(OUTPUT_DIR):
        for fname in sorted(files):
            if fname.endswith('.md') and fname != 'INDICE.md':
                rel = os.path.relpath(os.path.join(root, fname), OUTPUT_DIR).replace('\\', '/')
                md_files.append(rel)
    md_files.sort()

    idx = f"""# INDICE - FrotiX.Atualizado

## O que e isto?
Este diretorio contem as mudancas de **CODIGO** entre:
- **BASE:** `FrotiX.Site.2026.01` (versao Janeiro limpa)
- **ATUAL:** `FrotiX.Site` (versao atual com modificacoes)

Comentarios, documentacao e mudancas de formatacao foram **filtrados**.
Voce ve apenas o que mudou no codigo executavel.

## Resumo
| Tipo | Qtd |
|------|-----|
| Modificados (codigo real) | {mod_count} |
| Arquivos novos | {new_count} |
| Arquivos removidos | {del_count} |
| **Total** | **{mod_count + new_count + del_count}** |
| Ignorados (so formatacao) | {fmt_count} |

## Como usar
1. Abra cada arquivo .md
2. **Modificados**: Veja o diff limpo. Secoes "REMOVER" e "ADICIONAR" indicam o que fazer
3. **Novos**: Copie o arquivo inteiro para o Janeiro
4. **Removidos**: Avalie se deve deletar do Janeiro tambem

## Arquivos por Diretorio

"""
    current_dir = ''
    for f in md_files:
        d = os.path.dirname(f) or '(raiz)'
        if d != current_dir:
            current_dir = d
            idx += f"\n### {d}\n\n"
        idx += f"- [{os.path.basename(f)}]({f})\n"

    with open(os.path.join(OUTPUT_DIR, 'INDICE.md'), 'w', encoding='utf-8') as fp:
        fp.write(idx)
    print("Indice gerado.")

if __name__ == '__main__':
    main()
