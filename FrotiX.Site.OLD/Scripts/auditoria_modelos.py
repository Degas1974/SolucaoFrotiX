#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Script de Auditoria: Modelos C# vs Banco de Dados SQL
Compara definições de tabelas/views SQL com modelos C#
"""

import re
import os
from pathlib import Path
from typing import Dict, List, Tuple, Set
from dataclasses import dataclass, field

@dataclass
class SqlColumn:
    """Representa uma coluna SQL"""
    name: str
    data_type: str
    is_nullable: bool
    max_length: str = ""
    default_value: str = ""

@dataclass
class CSharpProperty:
    """Representa uma propriedade C#"""
    name: str
    data_type: str
    is_nullable: bool
    max_length: str = ""
    is_not_mapped: bool = False

@dataclass
class TableDefinition:
    """Definição completa de uma tabela SQL"""
    name: str
    columns: Dict[str, SqlColumn] = field(default_factory=dict)

@dataclass
class ModelDefinition:
    """Definição completa de um modelo C#"""
    name: str
    file_path: str
    properties: Dict[str, CSharpProperty] = field(default_factory=dict)

@dataclass
class Discrepancy:
    """Representa uma discrepância encontrada"""
    model_name: str
    property_name: str
    issue_type: str
    severity: str
    csharp_definition: str
    sql_definition: str
    recommendation: str

class AuditoriaModelos:
    def __init__(self, sql_file: str, models_dir: str):
        self.sql_file = sql_file
        self.models_dir = models_dir
        self.tables: Dict[str, TableDefinition] = {}
        self.views: Dict[str, TableDefinition] = {}
        self.models: Dict[str, ModelDefinition] = {}
        self.discrepancies: List[Discrepancy] = []

    def parse_sql_file(self):
        """Extrai todas as definições de tabelas e views do SQL"""
        print("Lendo arquivo SQL...")
        with open(self.sql_file, 'r', encoding='utf-8-sig') as f:
            sql_content = f.read()

        # Regex para encontrar CREATE TABLE
        table_pattern = r'CREATE TABLE dbo\.(\w+)\s*\((.*?)\)\s*ON\s+\[PRIMARY\]'
        tables_matches = re.finditer(table_pattern, sql_content, re.DOTALL | re.MULTILINE)

        for match in tables_matches:
            table_name = match.group(1)
            columns_block = match.group(2)
            table_def = TableDefinition(name=table_name)

            # Parse colunas
            column_lines = columns_block.split(',\n')
            for line in column_lines:
                line = line.strip()
                if not line or line.startswith('CONSTRAINT') or line.startswith('PRIMARY KEY') or line.startswith('UNIQUE'):
                    continue

                # Parse linha de coluna: NomeColuna tipo NULL/NOT NULL
                col_match = re.match(r'(\w+)\s+(\w+(?:\(\w+(?:,\s*\w+)?\))?)\s+(NULL|NOT NULL)?(?:\s+(?:IDENTITY|DEFAULT|CONSTRAINT))?', line)
                if col_match:
                    col_name = col_match.group(1)
                    col_type = col_match.group(2)
                    is_nullable = col_match.group(3) != 'NOT NULL' if col_match.group(3) else True

                    # Extrair max_length se houver
                    max_length = ""
                    length_match = re.search(r'\((\d+)\)', col_type)
                    if length_match:
                        max_length = length_match.group(1)

                    table_def.columns[col_name] = SqlColumn(
                        name=col_name,
                        data_type=col_type,
                        is_nullable=is_nullable,
                        max_length=max_length
                    )

            self.tables[table_name] = table_def
            print(f"[OK] Tabela: {table_name} ({len(table_def.columns)} colunas)")

        print(f"\nTotal de tabelas encontradas: {len(self.tables)}")

    def parse_csharp_models(self):
        """Extrai todas as definições de modelos C#"""
        print("\nLendo modelos C#...")
        models_path = Path(self.models_dir)

        # Buscar todos arquivos .cs
        for cs_file in models_path.rglob('*.cs'):
            if 'obj' in str(cs_file) or 'bin' in str(cs_file):
                continue

            with open(cs_file, 'r', encoding='utf-8') as f:
                content = f.read()

            # Encontrar classes públicas
            class_pattern = r'public\s+class\s+(\w+)\s*(?::\s*\w+)?\s*\{'
            class_matches = re.finditer(class_pattern, content)

            for class_match in class_matches:
                class_name = class_match.group(1)

                # Pular classes que não são entidades (ViewModel, DTO, etc.)
                if any(suffix in class_name for suffix in ['ViewModel', 'DTO', 'Dto', 'Model', 'Item']):
                    if class_name not in ['Viagem', 'Veiculo', 'Motorista', 'Abastecimento']:
                        continue

                model_def = ModelDefinition(name=class_name, file_path=str(cs_file))

                # Encontrar propriedades públicas
                prop_pattern = r'(?:\[.*?\]\s*)*public\s+(\w+\??)\s+(\w+)\s*\{\s*get;\s*set;\s*\}'
                prop_matches = re.finditer(prop_pattern, content)

                for prop_match in prop_matches:
                    prop_type = prop_match.group(1)
                    prop_name = prop_match.group(2)
                    is_nullable = '?' in prop_type
                    prop_type_clean = prop_type.replace('?', '')

                    # Verificar se tem [NotMapped]
                    prop_start = prop_match.start()
                    lines_before = content[:prop_start].split('\n')[-10:]
                    is_not_mapped = any('[NotMapped]' in line for line in lines_before)

                    # Verificar MaxLength
                    max_length = ""
                    for line in lines_before:
                        max_len_match = re.search(r'\[MaxLength\((\d+)\)\]', line)
                        if max_len_match:
                            max_length = max_len_match.group(1)

                    model_def.properties[prop_name] = CSharpProperty(
                        name=prop_name,
                        data_type=prop_type_clean,
                        is_nullable=is_nullable,
                        max_length=max_length,
                        is_not_mapped=is_not_mapped
                    )

                if model_def.properties:
                    self.models[class_name] = model_def
                    print(f"[OK] Modelo: {class_name} ({len(model_def.properties)} propriedades)")

        print(f"\nTotal de modelos encontrados: {len(self.models)}")

    def compare_models(self):
        """Compara modelos C# com tabelas SQL"""
        print("\nComparando modelos com banco de dados...")

        for model_name, model_def in self.models.items():
            # Encontrar tabela correspondente
            table_def = self.tables.get(model_name)
            if not table_def:
                # Tentar plural
                table_def = self.tables.get(model_name + 's')

            if not table_def:
                print(f"[AVISO] Modelo {model_name} nao tem tabela correspondente no SQL")
                continue

            print(f"\n[AUDITANDO] {model_name}")

            # Comparar propriedades
            for prop_name, prop in model_def.properties.items():
                if prop.is_not_mapped:
                    continue

                sql_col = table_def.columns.get(prop_name)
                if not sql_col:
                    self.discrepancies.append(Discrepancy(
                        model_name=model_name,
                        property_name=prop_name,
                        issue_type="Coluna ausente no SQL",
                        severity="🔵 INFO",
                        csharp_definition=f"public {prop.data_type}{'?' if prop.is_nullable else ''} {prop_name}",
                        sql_definition="(não existe no banco)",
                        recommendation=f"Adicionar coluna ao banco ou marcar com [NotMapped]"
                    ))
                    continue

                # Comparar nullable
                if prop.is_nullable != sql_col.is_nullable:
                    self.discrepancies.append(Discrepancy(
                        model_name=model_name,
                        property_name=prop_name,
                        issue_type="Nullable incompatível",
                        severity="🔴 CRÍTICO",
                        csharp_definition=f"{prop.data_type}{'?' if prop.is_nullable else ''} (nullable={prop.is_nullable})",
                        sql_definition=f"{sql_col.data_type} ({'NULL' if sql_col.is_nullable else 'NOT NULL'})",
                        recommendation=f"Alterar C# para: {'?' if sql_col.is_nullable else ''}"
                    ))

                # Comparar MaxLength
                if prop.max_length and sql_col.max_length:
                    if prop.max_length != sql_col.max_length:
                        self.discrepancies.append(Discrepancy(
                            model_name=model_name,
                            property_name=prop_name,
                            issue_type="MaxLength incompatível",
                            severity="🟡 ATENÇÃO",
                            csharp_definition=f"[MaxLength({prop.max_length})]",
                            sql_definition=f"({sql_col.max_length})",
                            recommendation=f"Alterar [MaxLength] para {sql_col.max_length}"
                        ))

    def generate_report(self, output_file: str):
        """Gera relatório markdown"""
        print(f"\n[RELATORIO] Gerando relatorio...")

        with open(output_file, 'w', encoding='utf-8') as f:
            f.write("# AUDITORIA COMPLETA: Modelos C# vs Banco de Dados SQL\n\n")
            f.write(f"**Data:** {Path(__file__).stat().st_mtime}\n")
            f.write("**Escopo:** Modelos principais do sistema\n\n")
            f.write("---\n\n")

            f.write("## 📊 ESTATÍSTICAS GERAIS\n\n")
            f.write(f"- **Total de tabelas SQL:** {len(self.tables)}\n")
            f.write(f"- **Total de modelos C#:** {len(self.models)}\n")
            f.write(f"- **Total de discrepâncias encontradas:** {len(self.discrepancies)}\n\n")

            f.write("---\n\n")

            # Agrupar discrepâncias por modelo
            by_model: Dict[str, List[Discrepancy]] = {}
            for disc in self.discrepancies:
                if disc.model_name not in by_model:
                    by_model[disc.model_name] = []
                by_model[disc.model_name].append(disc)

            f.write("## 🔍 ANÁLISE POR MODELO\n\n")

            for model_name in sorted(by_model.keys()):
                discs = by_model[model_name]
                f.write(f"### ⚠️ {model_name}.cs\n\n")
                f.write(f"**Status:** ⚠️ {len(discs)} discrepância(s) encontrada(s)\n\n")

                for i, disc in enumerate(discs, 1):
                    f.write(f"#### {i}. **{disc.property_name}**\n\n")
                    f.write(f"- **Problema:** {disc.issue_type}\n")
                    f.write(f"- **Severidade:** {disc.severity}\n")
                    f.write(f"- **C#:** `{disc.csharp_definition}`\n")
                    f.write(f"- **SQL:** `{disc.sql_definition}`\n")
                    f.write(f"- **Correção:** {disc.recommendation}\n\n")

                f.write("---\n\n")

        print(f"[OK] Relatorio gerado: {output_file}")

if __name__ == "__main__":
    # Caminhos
    sql_file = r"c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\FrotiX.sql"
    models_dir = r"c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models"
    output_file = r"c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Scripts\AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md"

    # Executar auditoria
    auditor = AuditoriaModelos(sql_file, models_dir)
    auditor.parse_sql_file()
    auditor.parse_csharp_models()
    auditor.compare_models()
    auditor.generate_report(output_file)

    print("\n[CONCLUIDO] Auditoria concluida!")
