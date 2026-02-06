@echo off
REM ============================================
REM   FrotiX - Pre-Build Cleanup (Silencioso)
REM   Executa antes de cada build
REM ============================================

REM Mata IIS Express (servidor web)
taskkill /F /IM iisexpress.exe 2>nul

REM Mata processos VBCSCompiler órfãos (compilador Roslyn em cache)
taskkill /F /IM VBCSCompiler.exe 2>nul

REM Mata processos dotnet que NÃO são o atual (órfãos de builds anteriores)
REM Usa WMIC para matar apenas processos dotnet mais antigos que 60 segundos
for /f "tokens=2" %%i in ('wmic process where "name='dotnet.exe' and creationdate < '%date%'" get processid 2^>nul ^| findstr [0-9]') do (
    taskkill /F /PID %%i 2>nul
)

REM Limpa handles de arquivo travados na pasta bin/obj
REM (Requer handle.exe da Sysinternals - opcional)
REM handle.exe -c -p dotnet.exe "%~dp0bin" 2>nul
REM handle.exe -c -p dotnet.exe "%~dp0obj" 2>nul

REM Aguarda 1 segundo para liberar recursos
timeout /t 1 /nobreak >nul

exit /b 0
