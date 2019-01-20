Set-Location .\src\Stubias.SpeedTest.Api
dotnet restore
dotnet lambda package `
    --disable-version-check true `
    --configuration release `
    --framework netcoreapp2.1 `
    --output-package bin/local/release/netcoreapp2.1/deploy-package.zip
Set-Location ..\..
