FOR /F "tokens=4 delims= " %%P IN ('netstat -a -n -o ^| findstr :8080') DO @ECHO TaskKill.exe /PID %%P
taskkill /f /im server.exe
FOR /F "tokens=4 delims= " %%P IN ('netstat -a -n -o ^| findstr :8080') DO @ECHO TaskKill.exe /PID %%P
taskkill /f /im server.exe
cd bin
start server.exe
exit