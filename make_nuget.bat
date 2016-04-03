@rem Add path to MSBuild Binaries
setlocal

@IF EXIST "c:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\vsvars32.bat" SET devenv="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\vsvars32.bat"
@IF EXIST "C:\Program Files\Microsoft Visual Studio 14.0\Common7\Tools\vsvars32.bat" SET devenv="C:\Program Files\Microsoft Visual Studio 14.0\Common7\Tools\vsvars32.bat"

set cur_dir=%CD%
call %devenv% || exit /b 1


IF EXIST nuget.exe goto restore

echo Downloading nuget.exe
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile 'nuget.exe'"

set NUGETDIR=nuget

rmdir /S /Q %NUGETDIR%
mkdir %NUGETDIR%\Asn1Net.Writer\lib\net46 || exit /b 1

copy Asn1Net.Writer\*.dll %NUGETDIR%\Asn1Net.Writer\lib\net46 || exit /b 1
copy Asn1Net.Writer\*.xml %NUGETDIR%\Asn1Net.Writer\lib\net46 || exit /b 1

copy LICENSE.txt %NUGETDIR%\Asn1Net.Writer || exit /b 1
copy NOTICE.txt %NUGETDIR%\Asn1Net.Writer || exit /b 1

copy Asn1Net.Writer.nuspec %NUGETDIR%\Asn1Net.Writer || exit /b 1

nuget pack %NUGETDIR%\Asn1Net.Writer\Asn1Net.Writer.nuspec || exit /b 1

endlocal

@echo NUGET BUILD SUCCEEDED !!!
@exit /b 0
