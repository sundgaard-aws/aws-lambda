import * as Core from '@aws-cdk/core';
import { Effect, IRole, PolicyStatement, ServicePrincipal } from "@aws-cdk/aws-iam";
import { AttributeType, BillingMode, Table } from '@aws-cdk/aws-dynamodb';
import { MetaData } from './meta-data';
import { RemovalPolicy, StackProps, Tags } from '@aws-cdk/core';
import * as SSM from '@aws-cdk/aws-ssm';
import { SSMHelper } from './ssm-helper';


export interface DataStackProps extends StackProps {
}

export class DataStack extends Core.Stack {
    private ssmHelper = new SSMHelper();
    private apiRole:IRole;
    private props:DataStackProps;
    
    constructor(scope: Core.Construct, id: string, apiRole: IRole, props?: DataStackProps) {
        super(scope, id, props);
        this.apiRole = apiRole;
        if(props==undefined) throw("Please make sure that the properties are initialized!");
        this.props=props;
        //this.createOrderTable();
    }
    
    private createOrderTable() {
        var name = MetaData.PREFIX+"order";
        new Table(this, name, {
            tableName: name,
            billingMode: BillingMode.PAY_PER_REQUEST,
            partitionKey: {name: "email", type: AttributeType.STRING}
        });
    }
}