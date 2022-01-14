using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using DTL;
using BL;
using System.Threading.Tasks;

namespace LambdaFunction
{
    public class LambdaHandler
    {
        private ILambdaLogger logger;
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<object> handleRequest(Stream inputStream, ILambdaContext context)
        {  
            if(context != null) logger = context.Logger;
            else logger = new LocalLambdaLogger();            
            var inputDataJSON = new StreamReader(inputStream).ReadToEnd();
            //logger.LogLine($"Stream contents [{inputDataJSON}]");
            var sqsMessage = JsonConvert.DeserializeObject<SQSMessage>(inputDataJSON);
            logger.LogLine($"message id [{sqsMessage.records[0].messageId}]");
            var s3JSON = sqsMessage.records[0].body;
            var s3Message = JsonConvert.DeserializeObject<S3Message>(s3JSON);
            var bucketName = s3Message.records[0].s3.bucket.name;
            var objectKey = s3Message.records[0].s3.s3object.key;
            logger.LogLine($"bucket name [{bucketName}]");
            logger.LogLine($"object key [{objectKey}]");
            string s3ObjectContents = await S3Facade.GetS3ObjectContents(bucketName, objectKey);
            logger.LogLine($"object contents:\n{s3ObjectContents}");
            var ftpSecretId = System.Environment.GetEnvironmentVariable("ftpSecretId");
            if (string.IsNullOrEmpty(ftpSecretId)) throw new System.Exception("Please define an environment variable for [ftpSecretId]");
            var ftpTargetFileName = System.Environment.GetEnvironmentVariable("ftpTargetFileName");
            if (string.IsNullOrEmpty(ftpTargetFileName)) throw new System.Exception("Please define an environment variable for [ftpTargetFileName]");
            var rawSecret = SecretsManagerFacade.GetSecret(ftpSecretId);            
            logger.LogLine($"raw secret:\n{rawSecret}");
            var ftpSecret = JsonConvert.DeserializeObject<FTPSecret>(rawSecret);
            logger.LogLine($"ftp login [{ftpSecret.Login}]");
            FTPFacade.uploadData(s3ObjectContents, ftpTargetFileName, ftpSecret);
            var response = new
            {
                body = "You just called a dotnet Lambda handler",
                statusCode = 200
            };
            return response;
        }       
    }
}
