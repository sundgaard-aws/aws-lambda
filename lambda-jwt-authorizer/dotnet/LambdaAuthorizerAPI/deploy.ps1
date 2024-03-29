$functionName = "lam-auth-LambdaAuthorizerFunction"
clear
echo Compressing function code...
#zip -rq drop.zip .
#7z a -r C:\github\dotnet-lambda-function\SimpleFunctionHandler\drop.zip .\bin\debug\netcoreapp3.1\publish\*
7z a -r .\drop.zip .\bin\debug\net6.0\publish\*
echo Uploading to Lambda...
aws lambda update-function-code --region "eu-north-1" --function-name $functionName --zip-file fileb://.\drop.zip
echo Cleaning up...
rm drop.zip
echo Done.

#aws s3 cp invoke-sfn-api.zip s3://iac-demo-lambda-code-bucket
#rm invoke-sfn-api.zip

# Upload to Lambda
#s3://iac-demo-lambda-code-bucket/invoke-sfn-api.zip