dotnet restore
dotnet test ./test/Stubias.SpeedTest.Api/Stubias.SpeedTest.Api.Tests.csproj
dotnet test ./test/Stubias.SpeedTest.Api.Integration/Stubias.SpeedTest.Api.Integration.csproj
dotnet lambda package \
    --disable-version-check true \
    --configuration release \
    --framework netcoreapp2.1 \
    --output-package src/Stubias.SpeedTest.Api/bin/local/release/netcoreapp2.1/deploy-package.zip \
    --project-location ./src/Stubias.SpeedTest.Api/