import { Construct } from 'constructs';
import { MetaData } from './meta-data';
import { SSMHelper } from './ssm-helper';
import { ISecurityGroup, IVpc } from 'aws-cdk-lib/aws-ec2';
import { Duration, Stack, StackProps, Tags } from 'aws-cdk-lib';
import { Code, Runtime, Tracing, Function } from 'aws-cdk-lib/aws-lambda';
import { Effect, IRole, ManagedPolicy, PolicyStatement, Role, ServicePrincipal } from 'aws-cdk-lib/aws-iam';
import { HttpMethod } from 'aws-cdk-lib/aws-events';
import { HttpLambdaIntegration } from '@aws-cdk/aws-apigatewayv2-integrations-alpha';
import { HttpApi } from '@aws-cdk/aws-apigatewayv2-alpha';

export class ComputeStack extends Stack {
    private runtime:Runtime = Runtime.NODEJS_12_X;    
    private ssmHelper = new SSMHelper();
    public apiRole:IRole;

    constructor(scope: Construct, id: string, vpc: IVpc, apiSecurityGroup: ISecurityGroup, props?: StackProps) {
        super(scope, id, props);

        this.apiRole = this.buildAPIRole();
        this.createBookTradeFunction(apiSecurityGroup, vpc);
    }

    private createLambdaFunction(apiSecurityGroup: ISecurityGroup, name:string, handlerMethod:string, assetPath:string, vpc:IVpc):Function {
        var codeFromLocalZip = Code.fromAsset(assetPath);
        var lambdaFunction = new Function(this, MetaData.PREFIX+name, { 
            functionName: MetaData.PREFIX+name, vpc: vpc, code: codeFromLocalZip, handler: handlerMethod, runtime: this.runtime, memorySize: 256, 
            timeout: Duration.seconds(20), role: this.apiRole, securityGroups: [apiSecurityGroup],
            tracing: Tracing.ACTIVE,
        });
        
        var proxyIntegration = new HttpLambdaIntegration(MetaData.PREFIX+name+"-api-integration", lambdaFunction);
        const httpApi = new HttpApi(this, MetaData.PREFIX+name+"-api");
        
        httpApi.addRoutes({
        path: "/" + name,
        methods: [ HttpMethod.POST, HttpMethod.OPTIONS ],
        integration: proxyIntegration,
        });
        
        Tags.of(lambdaFunction).add(MetaData.NAME, MetaData.PREFIX+name);
        return lambdaFunction;
    } 

    private createBookTradeFunction(apiSecurityGroup: ISecurityGroup, vpc: IVpc):Function {
        return this.createLambdaFunction(apiSecurityGroup, "login-fn", "index.handler", "../java/handler-with-guice-di/target/trade-guice-di-handler-1.0-SNAPSHOT.jar", vpc);
    }
    
    private buildAPIRole(): IRole {
        var role = new Role(this, MetaData.PREFIX+"api-role", {
            description: "Lambda API Role",
            roleName: MetaData.PREFIX+"api-role",
            assumedBy: new ServicePrincipal("lambda.amazonaws.com"),
            managedPolicies: [
                ManagedPolicy.fromAwsManagedPolicyName("AWSStepFunctionsFullAccess"),
                ManagedPolicy.fromAwsManagedPolicyName("AmazonSSMFullAccess"),
                ManagedPolicy.fromManagedPolicyArn(this, "AWSLambdaSQSQueueExecutionRole", "arn:aws:iam::aws:policy/service-role/AWSLambdaSQSQueueExecutionRole"),
                ManagedPolicy.fromManagedPolicyArn(this, "AWSLambdaBasicExecutionRole", "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"),
                ManagedPolicy.fromManagedPolicyArn(this, "AWSLambdaVPCAccessExecutionRole", "arn:aws:iam::aws:policy/service-role/AWSLambdaVPCAccessExecutionRole")
            ],
        });
        role.addToPolicy(new PolicyStatement({
          effect: Effect.ALLOW,
          resources: ["*"],
          actions: ["secretsmanager:GetSecretValue","dbqms:*","rds-data:*","xray:*","dynamodb:GetItem","dynamodb:PutItem","dynamodb:UpdateItem","dynamodb:Scan","dynamodb:Query"]
        }));

        Tags.of(role).add(MetaData.NAME, MetaData.PREFIX+"api-role");
        return role;
    }      
}