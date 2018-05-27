@echo off

REM begin analysis - prepare for the analysis
REM command: SonarQube.Scanner.MSBuild.exe begin /k:"unique-project-key" /n:"project-display-name" /v:"project-version"
SonarQube.Scanner.MSBuild.exe begin /k:"mysolution.dev" /n:"MySolution DEV" /v:"1.0"

REM build the project/solution
msbuild MySolution.sln /t:rebuild

REM end analysis - this will actually start the analysis!
SonarQube.Scanner.MSBuild.exe end

REM keep window open
pause