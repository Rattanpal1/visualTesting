@pushd %~dp0
@cd Backstop
@IF EXIST “%~dp0\node.exe” (
“%~dp0\node.exe” “C:\BackstopExample\BackstopTests\BackstopJS\mycliindex.js” _report — compareConfigFile=” C:\\BackstopExample\\BackstopTests\\BackstopJS\\backstop_data\\BSCompareConfig.json”
) ELSE (
@SETLOCAL
@SET PATHEXT=%PATHEXT:;.JS;=;%
node “C:\BackstopExample\BackstopTests\BackstopJS\mycliindex.js” _report — compareConfigFile=” C:\\BackstopExample\\BackstopTests\\BackstopJS\\backstop_data\\BSCompareConfig.json”
)
@popd