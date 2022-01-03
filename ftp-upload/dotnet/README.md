# Running locally
Simply run the command below in a command or PS prompt
``` ps
cd Console
dotnet run
```

# Deploy to Lambda
Simply run the command below in a command or PS prompt
``` ps
cd SL
deploy.ps1
```

# Required environment parameters
Obviously you can choose to hardcode the below or use another configuration manager.

``` ps
$Env:localFilePath="../local/<file-to-upload>.csv"
$Env:targetFTPPath="ftp://<hostname or ip>:21/<file-to-upload>.csv"
$Env:ftpUsername="<ftp-user>"
$Env:ftpPassword="<ftp-pw>"
$Env:usePassiveFTP="<true or false>"
```

# Install dotnet on Linux
sudo rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm
sudo yum install dotnet-sdk-3.1

# Diagnostics
https://github.com/dotnet/diagnostics/blob/main/documentation/diagnostics-client-library-instructions.md