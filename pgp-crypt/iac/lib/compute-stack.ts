import * as Core from '@aws-cdk/core';
import EC2 = require('@aws-cdk/aws-ec2');
import S3 = require('@aws-cdk/aws-s3');
import Lambda = require('@aws-cdk/aws-lambda');
import { ISecurityGroup, IVpc } from '@aws-cdk/aws-ec2';
import { MetaData } from './meta-data';
import { SSMHelper } from './ssm-helper';
import { HttpApi, HttpMethod } from '@aws-cdk/aws-apigatewayv2';
import { HttpLambdaIntegration } from '@aws-cdk/aws-apigatewayv2-integrations';
import { IKey } from '@aws-cdk/aws-kms';
import IAM = require("@aws-cdk/aws-iam");

export class ComputeStack extends Core.Stack {
    private runtime:Lambda.Runtime = Lambda.Runtime.DOTNET_6;
    private cmk:IKey;
    private apiRole:IAM.IRole;

    constructor(scope: Core.Construct, id: string, vpc: IVpc, apiSecurityGroup: ISecurityGroup, apiRole:IAM.IRole, cmk:IKey, props?: Core.StackProps) {
        super(scope, id, props);
        this.apiRole=apiRole;
        this.createArchivePaymentRequestFunction(apiSecurityGroup, vpc);
        this.createSendExternalPaymentRequestFunction(apiSecurityGroup, vpc);
        this.createReceiveExternalPaymentResponseFunction(apiSecurityGroup, vpc);
        this.createSendPaymentResponseDownstreamFunction(apiSecurityGroup, vpc);
    }

    private createLambdaFunction(apiSecurityGroup: ISecurityGroup, name:string, handlerMethod:string, assetPath:string, vpc:EC2.IVpc):Lambda.Function {
        var codeFromLocalZip = Lambda.Code.fromAsset(assetPath);
        var lambdaFunction = new Lambda.Function(this, MetaData.PREFIX+name, { 
            functionName: MetaData.PREFIX+name, vpc: vpc, code: codeFromLocalZip, handler: handlerMethod, runtime: this.runtime, memorySize: 256, 
            timeout: Core.Duration.seconds(20), role: this.apiRole, securityGroups: [apiSecurityGroup],
            tracing: Lambda.Tracing.ACTIVE,
            environmentEncryption: this.cmk
        });
        
        const lambdaIntegration = new HttpLambdaIntegration(MetaData.PREFIX+name+"-lam-int", lambdaFunction);        
        const httpApi = new HttpApi(this, MetaData.PREFIX+name+"-api");
        
        httpApi.addRoutes({
        path: "/" + name,
        methods: [ HttpMethod.POST, HttpMethod.OPTIONS ],
        integration: lambdaIntegration,
        });
        
        Core.Tags.of(lambdaFunction).add(MetaData.NAME, MetaData.PREFIX+name);
        return lambdaFunction;
    } 

    private createArchivePaymentRequestFunction(apiSecurityGroup: ISecurityGroup, vpc: IVpc):Lambda.Function {
        return this.createLambdaFunction(apiSecurityGroup, "ArchivePaymentRequestFunction", "LambdaHandler::PGPCrypt.API.FunctionHandler::Invoke", "../dotnet/LambdaHandler/LambdaHandler.zip", vpc);
    }

    private createSendExternalPaymentRequestFunction(apiSecurityGroup: ISecurityGroup, vpc: IVpc):Lambda.Function {
        return this.createLambdaFunction(apiSecurityGroup, "SendExternalPaymentRequestFunction", "LambdaHandler::PGPCrypt.API.FunctionHandler::Invoke", "../dotnet/LambdaHandler/LambdaHandler.zip", vpc);
    }

    private createReceiveExternalPaymentResponseFunction(apiSecurityGroup: ISecurityGroup, vpc: IVpc):Lambda.Function {
        return this.createLambdaFunction(apiSecurityGroup, "ReceiveExternalPaymentResponseFunction", "LambdaHandler::PGPCrypt.API.FunctionHandler::Invoke", "../dotnet/LambdaHandler/LambdaHandler.zip", vpc);
    }

    private createSendPaymentResponseDownstreamFunction(apiSecurityGroup: EC2.ISecurityGroup, vpc: EC2.IVpc) {
        return this.createLambdaFunction(apiSecurityGroup, "SendPaymentResponseDownstreamFunction", "LambdaHandler::PGPCrypt.API.FunctionHandler::Invoke", "../dotnet/LambdaHandler/LambdaHandler.zip", vpc);
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