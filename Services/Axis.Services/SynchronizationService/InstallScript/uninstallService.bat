@echo off
set SERVICE_HOME=C:\Program Files\XenatiX\Axis\SynchronizationService\
set SERVICE_NAME=SynchronizationService
set SERVICE_EXE=SynchronizationService.exe
set INSTALL_UTIL_HOME=C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319

set PATH=%PATH%;%INSTALL_UTIL_HOME%

cd %SERVICE_HOME%

echo Uninstalling Service...

installutil /u /name=%SERVICE_NAME% %SERVICE_EXE%

REM cd InstallScript

echo Done.

pause