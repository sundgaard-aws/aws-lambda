using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using OM.AWS.Demo.Config;
using static Amazon.Lambda.APIGatewayEvents.APIGatewayCustomAuthorizerPolicy;

namespace OM.AWS.Demo.BL.API {
    public class FunctionHandler {
        private static ServiceProvider serviceProvider;
        static FunctionHandler() {
            serviceProvider=AppConfig.Wireup();
        }

        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<APIGatewayCustomAuthorizerResponse> Invoke(APIGatewayCustomAuthorizerRequest apiGatewayCustomAuthorizerRequest) {
            Console.WriteLine("Lambda Authorizer Invoke() called...");
            var effect="Deny";
            if(apiGatewayCustomAuthorizerRequest.AuthorizationToken=="demo123") effect="Allow";
            return new APIGatewayCustomAuthorizerResponse {
                PolicyDocument=new APIGatewayCustomAuthorizerPolicy {
                    Statement=new List<IAMPolicyStatement> {
                        new IAMPolicyStatement{
                            Action=new HashSet<string>{"execute-api:Invoke"},
                            Effect=effect,
                            Resource=new HashSet<string>{"arn:aws:execute-api:*"}
                        }
                    }
                }
            };
        }
    }
}