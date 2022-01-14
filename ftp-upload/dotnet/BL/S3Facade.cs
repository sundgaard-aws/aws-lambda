using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace BL
{
    public class S3Facade
    {
        public static async Task<string> GetS3ObjectContents(string bucketName, string objectKey)
        {
            AmazonS3Client client = new AmazonS3Client();
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = objectKey
            };

            string contents = null;
            using (GetObjectResponse response = await client.GetObjectAsync(request))
            {
                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    contents = reader.ReadToEnd();
                }
            }
            return contents;
        }       
    }
}