@ECHO OFF

REM detect if BUILD_TYPE should be release or debug
if not %1!==Release! goto Debug
:RELEASE
set BUILD_TYPE=Release
goto START
:DEBUG
set BUILD_TYPE=Debug
goto START

:START
REM set vars
set SVN_ROOT=..
set PROJECTDIR=%SVN_ROOT%\InfoService
set INFOSERVICEDIR=%PROJECTDIR%\InfoService
set TARGETPATH=%INFOSERVICEDIR%\bin\x86\%BUILD_TYPE%\InfoService.dll



REM set log file
set buildlog="..\Build\build_log.txt"
set log=".\build_log.txt"

REM init log file, write dev env...
echo.
echo. > %log%
echo -= InfoService =-
echo -= InfoService =- >> %log%
echo -= build mode: %BUILD_TYPE% =-
echo -= build mode: %BUILD_TYPE% =- >> %log%
echo.
echo. >> %log%

echo. >> %log%
echo Using following environment variables: >> %log%
echo SVN_ROOT = %SVN_ROOT% >> %log%
echo TARGETPATH = %TARGETPATH% >> %log%
echo PROJECTDIR = %PROJECTDIR% >> %log%
echo INFOSERVICEDIR = %INFOSERVICEDIR% >> %log%
echo. >> %log%

REM Start buidling InfoService
echo ---- Start building InfoService ----
"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBUILD.exe" /p:DefineConstants="CODE_ANALYSIS;NET40;CODE_ANALYSIS" /v:detailed /target:InfoService:Rebuild /property:Configuration=%BUILD_TYPE%;Platform=x86 %PROJECTDIR%\InfoService.sln >> %log%

REM Copy last build
echo ---- Copy last build ----
xcopy "%TARGETPATH%" "%SVN_ROOT%\LastBuild\"  /Y /S /E
xcopy "%INFOSERVICEDIR%\GUIWindows\Language\*.*" "%SVN_ROOT%\LastBuild\Language\" /Y /S /E
xcopy "%INFOSERVICEDIR%\GUIWindows\Skin" "%SVN_ROOT%\LastBuild\Skin\" /Y /S /E
xcopy "%SVN_ROOT%\Installer\readme.txt" "%SVN_ROOT%\LastBuild\" /Y /S /E
echo ---- Generating MPE Package
for /f "tokens=*" %%i in ('..\Tools\FileVersion.exe %SVN_ROOT%\LastBuild\InfoService.dll') do set version=%%i
"%programfiles(x86)%\Team MediaPortal\MediaPortal\MPEMaker.exe" ..\Installer\MPInfoService.xmp2 /B /V=%version%
