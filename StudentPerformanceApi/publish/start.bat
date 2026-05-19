@echo off
chcp 65001 >nul
echo Запуск Student Performance System...
echo API + Client: http://localhost:5002
echo Swagger: http://localhost:5002/swagger
echo.
StudentPerformanceApi.exe --launch-profile http
echo.
pause
