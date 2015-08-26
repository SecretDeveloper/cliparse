param(
    $buildType = "Release"
)


function clean{
    # CLEAN
    write-host "Cleaning" -foregroundcolor:blue
    if(!(Test-Path "$basePath\BuildOutput\"))
    {
        mkdir "$basePath\BuildOutput\"
    }
    if(!(Test-Path "$logPath"))
    {
        mkdir "$logPath"
    }
    if(!(Test-Path "$basePath\TestOutput\"))
    {
        mkdir "$basePath\TestOutput\"
    }    
    if(!(Test-Path "$basePath\releases\"))
    {
        mkdir "$basePath\releases\"
    }    
    remove-item $basePath\BuildOutput\* -recurse
    remove-item $basePath\TestOutput\* -recurse
    if((Test-Path "$basePath\TestResults\"))
    {
        remove-item $basePath\TestResults -recurse
    }
    remove-item $logPath\* -recurse
    $lastResult = $true
}

function preBuild{
   
    write-host "PreBuild"  -foregroundcolor:blue
    Try{
        # Set assembly VERSION
        Get-Content $basePath\src\$projectName\Properties\AssemblyInfo.Template.txt  -ErrorAction stop |
        Foreach-Object {$_ -replace "//{VERSION}", "[assembly: AssemblyVersion(""$buildVersion"")]"} | 
        Foreach-Object {$_ -replace "//{FILEVERSION}", "[assembly: AssemblyFileVersion(""$buildVersion"")]"} | 
        Set-Content $basePath\src\$projectName\Properties\AssemblyInfo.cs -ErrorAction stop    
    }
    Catch{
        Write-host "PREBUILD FAILED!" -foregroundcolor:red
        exit
    }
}

function build{
    
    preBuild
    
    # BUILD   
    write-host "Building"  -foregroundcolor:blue
    $msbuild = "c:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
    $solutionPath = "$basePath\src\$projectName.sln"
    #write-host "$msbuild $solutionPath /p:configuration=$buildType /t:Clean /t:Build /verbosity:q /nologo > $logPath\LogBuild.log"
    Invoke-expression "$msbuild $solutionPath /p:configuration=$buildType /t:Clean /t:Build /verbosity:q /nologo > $logPath\LogBuild.log"

    if(!$LastExitCode -eq 0){
        Write-host "BUILD FAILED!"
        exit
    }    
    
    $content = (Get-Content -Path "$logPath\LogBuild.log")
    $failedContent = ($content -match "error")
    $failedCount = $failedContent.Count
    if($failedCount -gt 0)
    {    
        Write-host "BUILDING FAILED!" -foregroundcolor:red
        $lastResult = $false
        
        Foreach ($line in $content) 
        {
            write-host $line -foregroundcolor:red
        }
    }

    if($lastResult -eq $False){    
        exit
    } 
}

function test{
    # TESTING
    write-host "Testing"  -foregroundcolor:blue

    $trxPath = "$basePath\TestOutput\AllTest.trx"
    $resultFile="/resultsfile:$trxPath"

    $testDLLs = get-childitem -path "$basePath\TestOutput\*.*" -include "*.Tests.dll"
    #write-host "get-childitem -path $basePath\TestOutput\*.* -include *.Tests.dll"
    
    $arguments = " /testcontainer:" + $testDLLs + " /TestSettings:$basePath\src\LocalTestRun.testsettings"
    #write-host "mstest $resultFile $arguments"
    Invoke-Expression "mstest $resultFile $arguments > $logPath\LogTest.log"

    if(!$LastExitCode -eq 0){
        Write-host "TESTING FAILED0!" -foregroundcolor:red
        $lastResult = $false                
    }

    $content = (Get-Content -Path "$logPath\LogTest.log")

    $failedContent = ($content -match "Failed")
    $failedCount = $failedContent.Count
    if($failedCount -gt 0)
    {    
        Write-host "TESTING FAILED1!" -foregroundcolor:red
        $lastResult = $false
    }
    Foreach ($line in $failedContent) 
    {
        write-host $line -foregroundcolor:red
    }

    $failedContent = ($content -match "Not Runnable")
    $failedCount = $failedContent.Count
    if($failedCount -gt 0)
    {    
        Write-host "TESTING FAILED2!" -foregroundcolor:red
        $lastResult = $false
    }
    Foreach ($line in $failedContent) 
    {
        write-host $line -foregroundcolor:red
    }

    if($lastResult -eq $False){    
        exit
    }
}


#vstest.console
function vstest{
    # TESTING
    write-host "Testing"  -foregroundcolor:blue

    $trxPath = "$basePath\TestOutput\AllTest.trx"
    $resultFile="/resultsfile:$trxPath"

    $testDLLs = get-childitem -path "$basePath\TestOutput\*.*" -include "*.Tests.dll"
    #write-host "get-childitem -path $basePath\TestOutput\*.* -include *.Tests.dll"
    
    $arguments = "$testDLLs"
    write-host "vstest.console.exe $arguments"
    Invoke-Expression "vstest.console.exe $arguments > $logPath\LogTest.log"

    if(!$LastExitCode -eq 0){
        Write-host "TESTING FAILED0!" -foregroundcolor:red
        $lastResult = $false                
    }

    $content = (Get-Content -Path "$logPath\LogTest.log")

    $failedContent = ($content -match "Failed: 0")
    $failedCount = $failedContent.Count    
    if($failedCount -ne 1)
    {    
        Write-host "TESTING FAILED1!" -foregroundcolor:red
        $lastResult = $false
    }
    Foreach ($line in $failedContent) 
    {
        write-host $line -foregroundcolor:blue
    }

    $failedContent = ($content -match "Not Runnable")
    $failedCount = $failedContent.Count
    if($failedCount -gt 0)
    {    
        Write-host "TESTING FAILED2!" -foregroundcolor:red
        $lastResult = $false
    }
    Foreach ($line in $failedContent) 
    {
        write-host $line -foregroundcolor:red
    }

    if($lastResult -eq $False){    
        exit
    }
}

function pack{
    # Packing
    write-host "Packing" -foregroundcolor:blue
    nuget pack .\src\$projectName\$projectName.nuspec -version $fullBuildVersion -OutputDirectory .\releases > $logPath\LogPacking.log     
    if($? -eq $False){
        Write-host "PACK FAILED!"  -foregroundcolor:red
        exit
    }
}

function createZip{
    # DEPLOYING
    write-host "Creating zip" -foregroundcolor:blue
    $outputName = $projectName+"_V"+$fullBuildVersion+"_BUILD.zip"
    zip a -tzip .\releases\$outputName -r .\src\BuildOutput\*.* >> $logPath\LogDeploy.log    

}

function publish{
    # DEPLOYING
    write-host "Publishing Nuget package" -foregroundcolor:blue
    $outputName = ".\releases\$projectName.$fullBuildVersion.nupkg"
    nuget push $outputName
}

$basePath = Get-Location
$logPath = "$basePath\logs"
$buildVersion = Get-Content .\VERSION
$fullBuildVersion = "$buildVersion.0"
$projectName = "CliParse"

if($buildType -eq "publish"){
    $buildType="Release"

    clean
    build
    test    
    pack 
    publish  

    exit
}
if($buildType -eq "clean"){
    
    clean  
    exit
}
else {
    clean
    build
    vstest 
    pack   
}
Write-Host Finished -foregroundcolor:blue
