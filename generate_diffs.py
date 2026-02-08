"""
Generate MD diff files for FrotiX.Atualizado directory.
Compares FrotiX.Site.2026.01 (base January) with FrotiX.Site (current).
Only includes files with actual code changes (not just comments/docs).
"""
import os, re, sys, difflib, time

BASE_DIR = 'FrotiX.Site.2026.01'
TARGET_DIR = 'FrotiX.Site'
OUTPUT_DIR = 'FrotiX.Atualizado'

def strip_comments(content, ext):
    """Remove comments to check for code-only changes."""
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

def has_code_changes(base_content, target_content, ext):
    """Check if files differ in actual code (not just comments)."""
    return strip_comments(base_content, ext) != strip_comments(target_content, ext)

def read_file(path):
    """Read file with fallback encoding."""
    try:
        with open(path, 'r', encoding='utf-8', errors='replace') as f:
            return f.read()
    except Exception:
        return None

def generate_unified_diff(base_content, target_content, filepath):
    """Generate a unified diff between two file contents."""
    base_lines = base_content.splitlines(keepends=True)
    target_lines = target_content.splitlines(keepends=True)
    diff = difflib.unified_diff(
        base_lines, target_lines,
        fromfile=f'ANTES (2026.01): {filepath}',
        tofile=f'DEPOIS (atual): {filepath}',
        lineterm='\n'
    )
    return ''.join(diff)

def create_md_modified(filepath, base_content, target_content):
    """Create MD for a modified file."""
    ext = os.path.splitext(filepath)[1]
    lang = {'.cs': 'csharp', '.cshtml': 'html', '.js': 'javascript', '.css': 'css', '.sql': 'sql'}.get(ext, '')
    diff_text = generate_unified_diff(base_content, target_content, filepath)

    md = f"""# {filepath}

**Status:** Modificado
**Tipo:** `{ext}`

## Diff (Antes vs Depois)

```diff
{diff_text}
```
"""
    return md

def create_md_new(filepath, content):
    """Create MD for a new file (only in current version)."""
    ext = os.path.splitext(filepath)[1]
    lang = {'.cs': 'csharp', '.cshtml': 'html', '.js': 'javascript', '.css': 'css', '.sql': 'sql'}.get(ext, '')

    # Truncate very large files
    lines = content.split('\n')
    if len(lines) > 500:
        display = '\n'.join(lines[:500]) + f'\n\n... ({len(lines) - 500} linhas adicionais omitidas)'
    else:
        display = content

    md = f"""# {filepath}

**Status:** NOVO (nao existe no 2026.01)
**Tipo:** `{ext}`

## Conteudo Completo

```{lang}
{display}
```
"""
    return md

def create_md_deleted(filepath, content):
    """Create MD for a deleted file (only in January version)."""
    ext = os.path.splitext(filepath)[1]
    lang = {'.cs': 'csharp', '.cshtml': 'html', '.js': 'javascript', '.css': 'css', '.sql': 'sql'}.get(ext, '')

    lines = content.split('\n')
    if len(lines) > 500:
        display = '\n'.join(lines[:500]) + f'\n\n... ({len(lines) - 500} linhas adicionais omitidas)'
    else:
        display = content

    md = f"""# {filepath}

**Status:** REMOVIDO (existia no 2026.01, nao existe mais)
**Tipo:** `{ext}`

## Conteudo Original (2026.01)

```{lang}
{display}
```
"""
    return md

def write_md(filepath, md_content):
    """Write the MD file to the output directory."""
    # Change extension to .md
    md_path = os.path.join(OUTPUT_DIR, filepath + '.md')
    os.makedirs(os.path.dirname(md_path), exist_ok=True)
    with open(md_path, 'w', encoding='utf-8') as f:
        f.write(md_content)

def main():
    start_time = time.time()

    # Phase 1: Process modified files
    print("=== Fase 1: Arquivos Modificados ===")
    with open('C:/tmp/code_changes_final.txt', 'r') as f:
        modified_files = [line.strip() for line in f if line.strip()]

    processed = 0
    skipped = 0

    for filepath in modified_files:
        base_path = os.path.join(BASE_DIR, filepath)
        target_path = os.path.join(TARGET_DIR, filepath)

        base_content = read_file(base_path)
        target_content = read_file(target_path)

        if base_content is None or target_content is None:
            skipped += 1
            continue

        md = create_md_modified(filepath, base_content, target_content)
        write_md(filepath, md)
        processed += 1

        if processed % 50 == 0:
            elapsed = time.time() - start_time
            print(f"  Modificados: {processed}/{len(modified_files)} ({processed*100//len(modified_files)}%) - {elapsed:.0f}s")

    print(f"  Total modificados: {processed} (skipped: {skipped})")

    # Phase 2: New files (only in FrotiX.Site)
    print("\n=== Fase 2: Arquivos Novos ===")
    new_count = 0

    # Walk FrotiX.Site and find files not in FrotiX.Site.2026.01
    for root, dirs, files in os.walk(TARGET_DIR):
        # Skip non-code directories
        rel_root = os.path.relpath(root, TARGET_DIR)
        skip_dirs = ['bin', 'obj', 'node_modules', 'Documentacao', 'Conversas', '.claude', '.cursor', '.gemini', '.continue', '.github', 'wwwroot/lib']
        should_skip = False
        for sd in skip_dirs:
            if rel_root.startswith(sd) or sd in rel_root.split(os.sep):
                should_skip = True
                break
        if should_skip:
            continue

        for fname in files:
            ext = os.path.splitext(fname)[1].lower()
            if ext not in ('.cs', '.cshtml', '.js', '.css', '.sql'):
                continue

            rel_path = os.path.relpath(os.path.join(root, fname), TARGET_DIR).replace('\\', '/')
            base_equivalent = os.path.join(BASE_DIR, rel_path)

            if not os.path.exists(base_equivalent):
                content = read_file(os.path.join(root, fname))
                if content:
                    md = create_md_new(rel_path, content)
                    write_md(rel_path, md)
                    new_count += 1

    print(f"  Total novos: {new_count}")

    # Phase 3: Deleted files (only in FrotiX.Site.2026.01)
    print("\n=== Fase 3: Arquivos Removidos ===")
    deleted_count = 0

    for root, dirs, files in os.walk(BASE_DIR):
        rel_root = os.path.relpath(root, BASE_DIR)
        skip_dirs = ['bin', 'obj', 'node_modules', 'Documentacao', 'Conversas', '.claude', '.cursor', '.gemini', '.continue', '.github', 'wwwroot/lib']
        should_skip = False
        for sd in skip_dirs:
            if rel_root.startswith(sd) or sd in rel_root.split(os.sep):
                should_skip = True
                break
        if should_skip:
            continue

        for fname in files:
            ext = os.path.splitext(fname)[1].lower()
            if ext not in ('.cs', '.cshtml', '.js', '.css', '.sql'):
                continue

            rel_path = os.path.relpath(os.path.join(root, fname), BASE_DIR).replace('\\', '/')
            target_equivalent = os.path.join(TARGET_DIR, rel_path)

            if not os.path.exists(target_equivalent):
                content = read_file(os.path.join(root, fname))
                if content:
                    md = create_md_deleted(rel_path, content)
                    write_md(rel_path, md)
                    deleted_count += 1

    print(f"  Total removidos: {deleted_count}")

    # Summary
    total = processed + new_count + deleted_count
    elapsed = time.time() - start_time
    print(f"\n{'='*60}")
    print(f"RESUMO FINAL:")
    print(f"  Modificados: {processed}")
    print(f"  Novos:       {new_count}")
    print(f"  Removidos:   {deleted_count}")
    print(f"  TOTAL:       {total}")
    print(f"  Tempo:       {elapsed:.1f}s")
    print(f"  Diretorio:   {OUTPUT_DIR}/")
    print(f"{'='*60}")

    # Generate index file
    print("\nGerando indice...")
    generate_index(processed, new_count, deleted_count, modified_files)

def generate_index(mod_count, new_count, del_count, modified_files):
    """Generate an index MD file listing all changes."""
    md_files = []
    for root, dirs, files in os.walk(OUTPUT_DIR):
        for fname in sorted(files):
            if fname.endswith('.md') and fname != 'INDICE.md':
                rel = os.path.relpath(os.path.join(root, fname), OUTPUT_DIR).replace('\\', '/')
                md_files.append(rel)

    md_files.sort()

    idx = f"""# INDICE - FrotiX.Atualizado

## Resumo
- **Modificados:** {mod_count} arquivos
- **Novos:** {new_count} arquivos
- **Removidos:** {del_count} arquivos
- **Total:** {mod_count + new_count + del_count} arquivos

## Base de Comparacao
- **ANTES:** FrotiX.Site.2026.01 (versao Janeiro limpa)
- **DEPOIS:** FrotiX.Site (versao atual com mudancas)

## Arquivos por Diretorio

"""
    current_dir = ''
    for f in md_files:
        d = os.path.dirname(f) or '(raiz)'
        if d != current_dir:
            current_dir = d
            idx += f"\n### {d}\n\n"
        idx += f"- [{os.path.basename(f)}]({f})\n"

    index_path = os.path.join(OUTPUT_DIR, 'INDICE.md')
    with open(index_path, 'w', encoding='utf-8') as fp:
        fp.write(idx)
    print(f"Indice gerado: {index_path}")

if __name__ == '__main__':
    main()
