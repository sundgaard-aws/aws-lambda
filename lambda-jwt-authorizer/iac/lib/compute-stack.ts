import * as Core from '@aws-cdk/core';
import S3 = require('@aws-cdk/aws-s3');
import Lambda = require('@aws-cdk/aws-lambda');
import { ISecurityGroup } from '@aws-cdk/aws-ec2';
import { MetaData } from './meta-data';
import { SSMHelper } from './ssm-helper';
import { HttpApi, HttpMethod } from '@aws-cdk/aws-apigatewayv2';
import { HttpLambdaIntegration } from '@aws-cdk/aws-apigatewayv2-integrations';
import * as APIGW_AUTH from '@aws-cdk/aws-apigatewayv2-authorizers';
import IAM = require("@aws-cdk/aws-iam");
import { IFunction } from '@aws-cdk/aws-lambda';
import { HttpLambdaAuthorizer, HttpLambdaResponseType } from '@aws-cdk/aws-apigatewayv2-authorizers';
import { Duration } from '@aws-cdk/core';

export class ComputeStack extends Core.Stack {
    private runtime:Lambda.Runtime = Lambda.Runtime.DOTNET_6;
    private apiRole:IAM.IRole;

    constructor(scope: Core.Construct, id: string, apiRole:IAM.IRole, props?: Core.StackProps) {
        super(scope, id, props);
        this.apiRole=apiRole;
        this.createFunctions();
    }

    private createLambdaFunction(name:string, handlerMethod:string, assetPath:string, addRoute:boolean, authorizerFunction:IFunction|undefined):IFunction {
        var codeFromLocalZip = Lambda.Code.fromAsset(assetPath);
        var lambdaFunction = new Lambda.Function(this, MetaData.PREFIX+name, { 
            functionName: MetaData.PREFIX+name, code: codeFromLocalZip, handler: handlerMethod, runtime: this.runtime, memorySize: 256, 
            timeout: Core.Duration.seconds(20), role: this.apiRole,
            tracing: Lambda.Tracing.ACTIVE
        });
        
        const lambdaIntegration = new HttpLambdaIntegration(MetaData.PREFIX+name+"-lam-int", lambdaFunction);        
        const httpApi = new HttpApi(this, MetaData.PREFIX+name+"-api");
        
        if(authorizerFunction) {
            var authorizer=this.buildAuthorizer(authorizerFunction);
            httpApi.addRoutes({
                path: "/" + name,
                methods: [ HttpMethod.POST, HttpMethod.OPTIONS ],
                integration: lambdaIntegration,
                authorizer: authorizer
            });        
        }
        else {
            httpApi.addRoutes({
                path: "/" + name,
                methods: [ HttpMethod.POST, HttpMethod.OPTIONS ],
                integration: lambdaIntegration,
                
            });   
        }     
        
        Core.Tags.of(lambdaFunction).add(MetaData.NAME, MetaData.PREFIX+name);
        return lambdaFunction;
    } 

    private buildAuthorizer(authorizerFunction: Lambda.IFunction) {
        /*new APIGW_AUTH.HttpJwtAuthorizer("", "demo-jwt-issuer", {
            
        });*/
        var authorizer=new HttpLambdaAuthorizer("MyAuthorizer", authorizerFunction, {
            authorizerName: "MyAuthorizer",
            identitySource:["$request.header.Authorization"],
            responseTypes:[HttpLambdaResponseType.IAM],
            resultsCacheTtl:Duration.seconds(15)
        });
        /*const auth = new TokenAuthorizer(this,'NewRequestAuthorizer',{
                handler:authFunc,
                identitySource:'method.request.header.AuthorizeToken'
                
        })*/
        return authorizer;
    }

    private createFunctions() {
        var authorizer=this.createLambdaFunction("LambdaAuthorizerFunction", "LambdaAuthorizerAPI::OM.AWS.Demo.API.FunctionHandler::Invoke", "../dotnet/LambdaAuthorizerAPI/publish", false, undefined);
        return this.createLambdaFunction("ProcessOrderFunction", "ProcessOrderAPI::OM.AWS.Demo.API.FunctionHandler::Invoke", "../dotnet/ProcessOrderAPI/publish/", true, authorizer);
    }

    private createLambdaCodeBucket()
    {
        var codeBucket = new S3.Bucket(this, MetaData.PREFIX+"lambda-code-bucket", {
            bucketName: MetaData.PREFIX+"lambda-code-bucket", removalPolicy: Core.RemovalPolicy.DESTROY
        });
        Core.Tags.of(codeBucket).add(MetaData.NAME, MetaData.PREFIX+"lambda-code-bucket");
        //this.ssmHelper.createSSMParameter(this, MetaData.PREFIX+"state-machine-arn", stateMachine.stateMachineArn, SSM.ParameterType.STRING);
    }    
}