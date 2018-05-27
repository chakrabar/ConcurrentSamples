@echo off

REM Remove output directory
IF EXIST "TestOutputs" RD /S /Q TestOutputs

REM (re)create output directory
MD TestOutputs

REM begin analysis - prepare for the analysis
REM specify unit test results & opencover coverage reports
SonarQube.Scanner.MSBuild.exe begin /k:"mysolution.dev" /n:"MySolution DEV" /v:"1.0" /d:sonar.cs.nunit.reportsPaths="TestOutputs\NUnitResults.xml" /d:sonar.cs.opencover.reportsPaths="TestOutputs\opencover.xml"

REM build the project/solution
msbuild MySolution.sln /t:rebuild

REM run NUnit tests
"C:\Program Files (x86)\NUnit 2.6.4\bin\nunit-console.exe" /result=TestOutputs\NUnitResults.xml MySolution.Shared.UnitTests.dll MySolutionWeb.CoreTests.dll MySolutionWeb.ReportGeneratorTests.dll MySolutionWeb.Data.UnitTests.dll MySolutionWeb.WebTests.dll

REM run OpenCover coverage
"C:\Users\607850442\AppData\Local\Apps\OpenCover\OpenCover.Console.exe" -register:user "-target:C:\Program Files (x86)\NUnit 2.6.4\bin\nunit-console.exe" "-targetargs:MySolution.Shared.UnitTests.dll MySolutionWeb.CoreTests.dll MySolutionWeb.ReportGeneratorTests.dll MySolutionWeb.Data.UnitTests.dll MySolutionWeb.WebTests.dll /noshadow" "-output:TestOutputs\opencover.xml" "-filter:+[MySolution*]* +[MySolutionWeb*]* -[*Tests]* -[*Console]* -[*Contracts]* -[*Models]* -[*Automation]*"

REM end analysis - this will actually start the analysis!
SonarQube.Scanner.MSBuild.exe end

REM keep window open
pause