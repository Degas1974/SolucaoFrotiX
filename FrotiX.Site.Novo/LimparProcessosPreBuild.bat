@echo off
REM ============================================
REM   FrotiX - Limpeza Pre-Build (Silenciosa)
REM   Executado automaticamente antes do build
REM ============================================

REM Mata apenas os processos que travam o build
taskkill /F /IM VBCSCompiler.exe 2>nul
taskkill /F /IM MSBuild.exe 2>nul

REM Aguarda 1 segundo para liberar os arquivos
timeout /t 1 /nobreak >nul

exit /b 0
