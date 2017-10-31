param(
    $buildType = "Release"
)

function clean{
    # CLEAN
    write-host "Cleaning" -foregroundcolor:blue
    dotnet clean 
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
    
    $solutionPath = "$basePath\src\$projectName.sln"
    #write-host "$msbuild $solutionPath /p:configuration=$buildType /t:Clean /t:Build /verbosity:q /nologo > $logPath\LogBuild.log"
    Invoke-expression "dotnet build $solutionPath"    
}


function xutest{
    # TESTING
    write-host "Testing"  -foregroundcolor:blue

    $testPath = "$basePath\src\$projectName.Test\"
    Invoke-expression "dotnet test $testPath"
}


function pack{
    # Packing
    write-host "Packing" -foregroundcolor:blue
    #dotnet pack .\src\$projectName\$projectName.nuspec -version $fullBuildVersion -o .\releases > $logPath\LogPacking.log     
    dotnet pack .\src\$projectName\$projectName.csproj -o $basePath\releases > $logPath\LogPacking.log     
    if($? -eq $False){
        Write-host "PACK FAILED!"  -foregroundcolor:red
        exit
    }
}

function publish{
    # DEPLOYING
    write-host "Publishing Nuget package" -foregroundcolor:blue
    $outputName = ".\releases\$projectName.$fullBuildVersion.nupkg"
    nuget push $outputName -source nuget.org
}

$basePath = Get-Location
$logPath = "$basePath\logs"
$buildVersion = Get-Content .\VERSION
$fullBuildVersion = "$buildVersion.0"
$projectName = "CliParse"


if($buildType -eq "clean"){   
    clean  
    exit
}
if($buildType -eq "pack"){
    pack
    exit
}

if($buildType -eq "publish"){
    $buildType="Release"
}

clean
build
xutest 
pack   

if($buildType -eq "publish"){
    publish
}

Write-Host Finished -foregroundcolor:blue
