@pushd %~dp0
@cd Backstop
@IF EXIST “%~dp0\node.exe” (
“%~dp0\node.exe” “C:\Users\ranjeet.rattanpal\source\repos\TestProject\mycliindex.js” _report — compareConfigFile=” C:\\BackstopExample\\BackstopTests\\BackstopJS\\backstop_data\\BSCompareConfig.json”
) ELSE (
@SETLOCAL
@SET PATHEXT=%PATHEXT:;.JS;=;%
node “C:\Users\ranjeet.rattanpal\source\repos\TestProject\mycliindex.js” _report — compareConfigFile=” C:\\BackstopExample\\BackstopTests\\BackstopJS\\backstop_data\\BSCompareConfig.json”
)
@popd