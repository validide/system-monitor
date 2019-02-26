$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Definition


# comma seprated arguments need to be escaped like
$uniteTestParams = New-Object System.Collections.ArrayList
[void] $uniteTestParams.Add("/p:CollectCoverage=true")
[void] $uniteTestParams.Add("/p:Exclude='\`"[xunit.runner*]*,[xunit.runner*]*\`"'")
[void] $uniteTestParams.Add("/p:CoverletOutputFormat='\`"lcov,opencover\`"' /p:CoverletOutput=`"$scriptPath/../test/unit-test-coverage/`"")
[void] $uniteTestParams.Add("/p:ThresholdType='\`"branch,method\`"'")
[void] $uniteTestParams.Add("/p:Threshold=90")

$uniteTestParamString = $uniteTestParams -join ' '
Invoke-Expression "dotnet test `"$scriptPath/../test/SystemMonitor.UnitTests/SystemMonitor.UnitTests.csproj`" $uniteTestParamString"

if ($LASTEXITCODE -ne 0) {
  exit $LASTEXITCODE
}

$reportGeneratorParams = New-Object System.Collections.ArrayList
[void] $reportGeneratorParams.Add("$($Env:UserProfile)\.nuget\packages\reportgenerator\4.0.13.1\tools\netcoreapp2.0\ReportGenerator.dll")
[void] $reportGeneratorParams.Add("-reports:`"$scriptPath/../test/unit-test-coverage/coverage.opencover.xml`"")
[void] $reportGeneratorParams.Add("-targetdir:`"$scriptPath/../test/unit-test-coverage-report`"")

$reportGeneratorParamString = $reportGeneratorParams -join ' '
Invoke-Expression "dotnet $reportGeneratorParamString"
