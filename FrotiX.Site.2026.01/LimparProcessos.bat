@echo off
echo ============================================
echo   FrotiX - Limpeza de Processos Travados
echo   EXECUTAR COMO ADMINISTRADOR
echo ============================================
echo.

echo [1/3] Matando processos .NET e Visual Studio...
taskkill /F /IM dotnet.exe 2>nul
taskkill /F /IM iisexpress.exe 2>nul
taskkill /F /IM VBCSCompiler.exe 2>nul
taskkill /F /IM MSBuild.exe 2>nul
taskkill /F /IM sqlservr.exe 2>nul
echo       Processos encerrados.
echo.

echo [2/3] Limpando conexoes TCP em TIME_WAIT...
netsh int ip reset >nul 2>&1
echo       Conexoes TCP resetadas.
echo.

echo [3/3] Reiniciando servico SQL Server...
net stop MSSQLSERVER /y >nul 2>&1
net start MSSQLSERVER >nul 2>&1
echo       SQL Server reiniciado.
echo.

echo ============================================
echo   Limpeza concluida com sucesso!
echo ============================================
echo.
echo Aguarde alguns segundos antes de executar
echo o projeto novamente no Visual Studio.
echo.
pause
