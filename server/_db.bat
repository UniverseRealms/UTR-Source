@echo off
cd storage_engine\redis 3.2.100
echo Running Redis Server...
echo Note: do not close this window to keep running this service.
redis-server.exe redis__config.conf