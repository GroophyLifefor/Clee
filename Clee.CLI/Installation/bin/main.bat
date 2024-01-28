@echo off & setlocal enabledelayedexpansion

:main 
set saveAs=%~1

	REM Good code does not need comments
	where tar >nul 2>nul
	if %errorlevel% neq 0 (
		echo [ERR] tar is not installed.
		echo [TIP] You can install in https://gnuwin32.sourceforge.net/packages/gtar.htm
		timeout /t 10
		exit /b 2
	)
	
	set "basePath=C:/tools/Clee"
	
	if not exist "%basePath%" (
		mkdir "%basePath%"
	)
	
	cd %basePath%
	
	wget https://github.com/GroophyLifefor/Clee/releases/download/release/Release.zip
	tar -xf Release.zip
	del Release.zip
	
	setx "PATH" "%PATH%;%basePath%/Clee.CLI/bin/Release" /M >nul 2>nul
	if %errorlevel% equ 0 (
		echo.
		echo [+++] The installation process is complete.
		echo [+++] You can try with "Clee.CLI --help"
	)
	if %errorlevel% equ 1 (
		echo.
		echo [ERR] Command Failed: setx PATH "%%PATH%%;%%combinedPath%%" /M
		echo [ERR] Your path not set because your PATH registery data oversized than 1024 character.
		echo [TIP] Try add to path from "Edit the system environment varriables"
		echo [TIP] You can try with "Clee.CLI --help"
		echo.
		echo path: %basePath%/Clee.CLI/bin/Release
	)
	pause
set saveAs=
GOTO :EOF