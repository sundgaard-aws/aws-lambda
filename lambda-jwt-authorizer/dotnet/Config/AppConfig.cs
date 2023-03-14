using Microsoft.Extensions.DependencyInjection;
using OM.AWS.Demo.BL;
using OM.AWS.Demo.SL;

namespace OM.AWS.Demo.Config
{
    public class AppConfig : IAppContextService
    {
        public static ServiceProvider Wireup() {
            var sc=new ServiceCollection();
            //sc.AddSingleton<IDatabaseService, AmazonDynamoDBService>();
            //sc.AddSingleton<ISecretsService, AWSSecretsManagerService>();
            //sc.AddSingleton<ISettingsService, AWSParameterStoreService>();
            sc.AddSingleton<IAppContextService, AppConfig>();
            sc.AddSingleton<OrderBO, OrderBO>();
            return sc.BuildServiceProvider();
        }

        public string GetAppName()
        {
            return "lam-auth-app";
        }

        public string GetAppPrefix()
        {
            return "lam-auth-";
        }
    }
}