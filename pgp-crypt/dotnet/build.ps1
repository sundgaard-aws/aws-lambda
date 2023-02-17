Write-Output "Building PGPFunctionHandler"
cd PGPFunctionHandler
dotnet publish -o publish
cd publish
#7z a -sdel ../bin/drop.zip *
7z a ../bin/drop.zip *

cd ../..