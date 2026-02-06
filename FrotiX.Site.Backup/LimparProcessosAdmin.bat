@echo off
:: ============================================
:: FrotiX - Limpeza SuperAgressiva (Auto-Admin)
:: Clique duplo ou botão do Visual Studio
:: ============================================

:: Verifica se já está como Admin
net session >nul 2>&1
if %errorlevel% neq 0 (
    :: Não é admin - reinicia como admin
    powershell -Command "Start-Process '%~f0' -Verb RunAs"
    exit /b
)

:: Já é admin - executa limpeza
echo ============================================
echo   FrotiX - Limpeza SuperAgressiva
echo   Executando como Administrador
echo ============================================
echo.

echo [1/4] Parando IIS Express...
taskkill /F /IM iisexpress.exe 2>nul
taskkill /F /IM iisexpresstray.exe 2>nul
echo       OK
echo.

echo [2/4] Parando processos .NET orfaos...
taskkill /F /IM dotnet.exe 2>nul
taskkill /F /IM VBCSCompiler.exe 2>nul
taskkill /F /IM ServiceHub.Host.dotnet.x64.exe 2>nul
taskkill /F /IM ServiceHub.Host.CLR.x64.exe 2>nul
taskkill /F /IM ServiceHub.IdentityHost.exe 2>nul
taskkill /F /IM ServiceHub.VSDetouredHost.exe 2>nul
echo       OK
echo.

echo [3/4] Limpando conexoes TCP...
netsh int ip reset >nul 2>&1
echo       OK
echo.

echo [4/4] Reiniciando SQL Server...
net stop MSSQLSERVER /y >nul 2>&1
timeout /t 2 /nobreak >nul
net start MSSQLSERVER >nul 2>&1
echo       OK
echo.

echo ============================================
echo   LIMPEZA CONCLUIDA!
echo   Pode compilar no Visual Studio agora.
echo ============================================
echo.
timeout /t 3
exit
