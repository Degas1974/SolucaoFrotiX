# Hooks do Git

Este diretório contém hooks compartilhados para automatizar commit/push.

## Ativação

No repositório:

```bash
git config --local core.hooksPath .githooks
```

## Importante (WSL/Windows)

Se o repositório estiver em /mnt/d (NTFS), o WSL pode não permitir o bit de execução.
Para os hooks funcionarem, use uma destas opções:

1) Mover o repositório para o filesystem do Linux (ex.: ~/repos/FrotiX)
2) Habilitar metadata no WSL e remontar o drive

Sem o bit de execução, os hooks não rodam automaticamente.
