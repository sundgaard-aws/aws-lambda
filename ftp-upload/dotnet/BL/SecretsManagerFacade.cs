using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace BL
{
    public class SecretsManagerFacade
    {

        /*
         *	Use this code snippet in your app.
         *	If you need more information about configurations or implementing the sample code, visit the AWS docs:
         *	https://aws.amazon.com/developers/getting-started/net/
         *	
         *	Make sure to include the following packages in your code.
         *	
         *	using System;
         *	using System.IO;
         *
         *	using Amazon;
         *	using Amazon.SecretsManager;
         *	using Amazon.SecretsManager.Model;
         *
         */

        /*
         * AWSSDK.SecretsManager version="3.3.0" targetFramework="net45"
         */
        public static string GetSecret(string secretId)
        {
            //string region = "eu-central-1";
            string secret = null;

            MemoryStream memoryStream = new MemoryStream();
            //var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
            var client = new AmazonSecretsManagerClient();
            var request = new GetSecretValueRequest();
            request.SecretId = secretId;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

            GetSecretValueResponse response = null;
            // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
            // See https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            // We rethrow the exception by default.

            try { response = client.GetSecretValueAsync(request).Result; }
            catch (Exception) { throw; }
            // Decrypts secret using the associated KMS key.
            // Depending on whether the secret is a string or binary, one of these fields will be populated.
            if (response.SecretString != null) secret = response.SecretString;
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                secret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }
            return secret;
        }
    }
}