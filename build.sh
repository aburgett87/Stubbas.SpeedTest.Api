dotnet restore
dotnet test ./test/Stubias.SpeedTest.Api/Stubias.SpeedTest.Api.Tests.csproj
dotnet test ./test/Stubias.SpeedTest.Api.Integration/Stubias.SpeedTest.Api.Integration.csproj
cd ./src/Stubias.SpeedTest.Api
dotnet lambda package \
    --disable-version-check true \
    --configuration release \
    --framework netcoreapp2.1 \
    --output-package bin/local/release/netcoreapp2.1/deploy-package.zip
cd ../..