using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LambdaFunction
{
    public class LambdaHandler
    {
        private ILambdaLogger logger;
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public object handleRequest(Stream inputStream, ILambdaContext context)
        {  
            if(context != null) logger = context.Logger;
            else logger = new LocalLambdaLogger();            
            var inputDataJSON = new StreamReader(inputStream).ReadToEnd();
            //logger.LogLine($"Stream contents [{inputDataJSON}]");
            var sqsMessage = JsonConvert.DeserializeObject<SQSMessage>(inputDataJSON);
            logger.LogLine($"message id [{sqsMessage.records[0].messageId}]");
            logger.LogLine($"body [{sqsMessage.records[0].body}]");
            var s3JSON = sqsMessage.records[0].body;
            var s3Message = JsonConvert.DeserializeObject<S3Message>(s3JSON);
            logger.LogLine($"bucket name [{s3Message.records[0].s3.bucket.name}]");
            logger.LogLine($"object key [{s3Message.records[0].s3.s3object.key}]");
            //FTPFacade.uploadFile();
            var response = new
            {
                body = "You just called a dotnet Lambda handler",
                statusCode = 200
            };
            return response;
        }       
    }
}
