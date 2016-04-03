@rem Add path to MSBuild Binaries
setlocal

@rem preparing environment

@IF EXIST "c:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\vsvars32.bat" SET devenv="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\vsvars32.bat"
@IF EXIST "C:\Program Files\Microsoft Visual Studio 14.0\Common7\Tools\vsvars32.bat" SET devenv="C:\Program Files\Microsoft Visual Studio 14.0\Common7\Tools\vsvars32.bat"


set cur_dir=%CD%
call %devenv% || exit /b 1

set SLNPATH=src\Asn1Net.Writer.sln

IF EXIST .nuget\nuget.exe goto restore

echo Downloading nuget.exe
md .nuget
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile '.nuget\nuget.exe'"

:restore
IF EXIST packages goto run
.nuget\NuGet.exe restore %SLNPATH%

:run
@rem cleanin sln
msbuild %SLNPATH% /p:Configuration=Release /target:Clean || exit /b 1
@rem build version
msbuild %SLNPATH% /p:Configuration=Release /target:ReBuild || exit /b 1


@rem set variables
set OUTDIR=build\Asn1Net.Writer\
set SRCDIR=src\Asn1Net.Writer\bin\Release

@rem prepare output directory
rmdir /S /Q %OUTDIR%
mkdir %OUTDIR% || exit /b 1

@rem copy files to output directory
copy %SRCDIR%\Asn1Net.Writer.dll %OUTDIR% || exit /b 1
copy %SRCDIR%\Asn1Net.Writer.XML %OUTDIR% || exit /b 1

set BUILDDIR=build
@rem copy license and notice to output directory
copy %SRCDIR%\LICENSE.txt %BUILDDIR% || exit /b 1
copy %SRCDIR%\NOTICE.txt %BUILDDIR% || exit /b 1


@rem copy make_nuget.bat and nuspec file
copy make_nuget.bat %BUILDDIR% || exit /b 1
copy Asn1Net.Writer.nuspec %BUILDDIR% || exit /b 1

endlocal

@echo BUILD SUCCEEDED !!!
@exit /b 0