using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using OM.AWS.Demo.BL;
using OM.AWS.Demo.Config;

namespace OM.AWS.Demo
{
    public class Program {
        private static ServiceProvider serviceProvider=AppConfig.Wireup();
        public static void Main() {
            Go().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        
        async static Task Go() {
            Console.WriteLine("Started Authorizer Console...");
            var orderBO=serviceProvider.GetService<OrderBO>();
            if(orderBO==null) throw new Exception("Please make sure that OrderBO is initialized!");
            var order=await orderBO.ProcessOrderAsync();
            Console.WriteLine($"******* ORDER DETAILS ********");
            Console.WriteLine($"{JsonSerializer.Serialize(order)}");
            //var secret=secretsService.RestoreSecret<SecretDTO>("demo/secret2").ConfigureAwait(false).GetAwaiter().GetResult();
            //Console.WriteLine($"Decrypted contents are {decryptedContents}");
            Console.WriteLine("Ended Authorizer Console.");
        }
    }
}