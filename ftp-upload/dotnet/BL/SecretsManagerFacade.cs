using System;
using System.IO;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace BL
{
    public class SecretsManagerFacade
    {
        public static string GetSecret(string secretId)
        {
            string secret = null;
            MemoryStream memoryStream = new MemoryStream();
            var client = new AmazonSecretsManagerClient();
            var request = new GetSecretValueRequest();
            request.SecretId = secretId;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.
            GetSecretValueResponse response = null;
            try { response = client.GetSecretValueAsync(request).Result; }
            catch (Exception) { throw; }
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