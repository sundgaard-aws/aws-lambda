using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using OM.AWS.Demo.Config;

namespace OM.AWS.Demo.BL.API {
    public class FunctionHandler {
        private static ServiceProvider serviceProvider;
        static FunctionHandler() {
            serviceProvider=AppConfig.Wireup();
        }

        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<Object> Invoke() {
            Console.WriteLine("Lambda Invoke() called...");
            var orderBO=serviceProvider.GetService<OrderBO>();
            if(orderBO==null) throw new Exception("Please make sure that OrderBO is initialized!");
            var order=await orderBO.ProcessOrderAsync();   
            return new { Status="Success", Order=order };
        }   
    }
}