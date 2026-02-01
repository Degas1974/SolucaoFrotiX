#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
SUPERVISOR DE EXTRAÇÃO DE DEPENDÊNCIAS - FrotiX
Monitora DocumentacaoIntracodigo.md e ControleExtracaoDependencias.md
Processa novos arquivos documentados e extrai suas dependências
"""

import os
import re
import json
from datetime import datetime
from pathlib import Path
import time

BASE_PATH = "/mnt/c/FrotiX/Solucao FrotiX 2026/FrotiX.Site"
DOCUMENTACAO_FILE = os.path.join(BASE_PATH, "DocumentacaoIntracodigo.md")
CONTROLE_FILE = os.path.join(BASE_PATH, "ControleExtracaoDependencias.md")
MAPEAMENTO_FILE = os.path.join(BASE_PATH, "MapeamentoDependencias.md")

class DependencyExtractor:
    def __init__(self):
        self.documentados = 0
        self.extraidos = 0
        self.loop_count = 0

    def get_documentados(self):
        """Extrai número de arquivos documentados"""
        try:
            with open(DOCUMENTACAO_FILE, 'r', encoding='utf-8') as f:
                content = f.read()
                match = re.search(r'\| Documentados \s*\|\s*(\d+)', content)
                if match:
                    return int(match.group(1))
        except Exception as e:
            print(f"Erro ao ler documentados: {e}")
        return 0

    def get_extraidos(self):
        """Extrai número de dependências já extraídas"""
        try:
            with open(CONTROLE_FILE, 'r', encoding='utf-8') as f:
                content = f.read()
                match = re.search(r'Dependências extraídas:\s*(\d+)', content)
                if match:
                    return int(match.group(1))
        except Exception as e:
            print(f"Erro ao ler extraídos: {e}")
        return 0

    def get_timestamp(self):
        """Retorna timestamp formatado HH:MM:SS"""
        return datetime.now().strftime("%H:%M:%S")

    def log(self, message):
        """Exibe mensagem com timestamp"""
        timestamp = self.get_timestamp()
        print(f"[{timestamp}] {message}")

    def run_loop(self, max_loops=999999):
        """Loop infinito de supervisão"""
        self.log("=" * 70)
        self.log("SUPERVISOR DE EXTRAÇÃO DE DEPENDÊNCIAS - FROTIX")
        self.log("=" * 70)
        self.log(f"Caminho base: {BASE_PATH}")
        self.log(f"Total esperado: 905 arquivos")
        self.log("=" * 70)
        self.log("")

        while self.loop_count < max_loops:
            self.loop_count += 1

            self.documentados = self.get_documentados()
            self.extraidos = self.get_extraidos()

            # Cálculo de progresso
            diferenca = self.documentados - self.extraidos
            percentual = (self.documentados / 905) * 100

            if self.documentados > self.extraidos:
                self.log(f"[LOTE DETECTADO] Docs: {self.documentados}/905 | Extraídos: {self.extraidos}")
                self.log(f"  → {diferenca} arquivo(s) novo(s) para processar")
                self.log(f"  → Posições: {self.extraidos + 1} até {self.documentados}")
                self.log(f"  → Progresso: {percentual:.1f}%")

            elif self.documentados == self.extraidos:
                self.log(f"Sincronizado: {self.documentados}/905 arquivos ({percentual:.1f}%)")

            elif self.documentados == 905:
                self.log("=" * 70)
                self.log("✅ PROCESSO COMPLETO!")
                self.log(f"Processados: 905/905 arquivos com sucesso")
                self.log("=" * 70)
                break

            time.sleep(2)  # Aguarda 2 segundos antes de próxima verificação

if __name__ == "__main__":
    supervisor = DependencyExtractor()
    supervisor.run_loop()
