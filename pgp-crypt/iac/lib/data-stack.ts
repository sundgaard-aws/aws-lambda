import * as Core from '@aws-cdk/core';
import S3 = require('@aws-cdk/aws-s3');
import { IRole } from "@aws-cdk/aws-iam";
import { IVpc } from '@aws-cdk/aws-ec2';
import { AttributeType, BillingMode, Table } from '@aws-cdk/aws-dynamodb';
import { MetaData } from './meta-data';
import { Bucket } from '@aws-cdk/aws-s3';
import { Tags } from '@aws-cdk/core';
import { IKey } from '@aws-cdk/aws-kms';

export class DataStack extends Core.Stack {
    private apiRole:IRole;
    private cmk:IKey;
    //constructor(scope: Core.Construct, id: string, apiRole: IRole, cmk:IKey, props?: Core.StackProps) {
    constructor(scope: Core.Construct, id: string, props?: Core.StackProps) {
        super(scope, id, props);
        //this.cmk=cmk;
        //this.apiRole = apiRole;
        
        //this.createLoginTable();
        //this.createRequestBucket();
        //this.createResponseBucket();
    }

    private createRequestBucket() {
        var name = MetaData.PREFIX+"pay-req";
        var bucket = new Bucket(this, name, {
            bucketName: name, encryptionKey: this.cmk
        });
        bucket.grantReadWrite(this.apiRole);
        Core.Tags.of(bucket).add(MetaData.NAME, name);
    }    
    
    private createResponseBucket() {
        var name = MetaData.PREFIX+"pay-res";
        var bucket = new Bucket(this, name, {
            bucketName: name, encryptionKey: this.cmk
        });
        bucket.grantReadWrite(this.apiRole);
        Core.Tags.of(bucket).add(MetaData.NAME, name);
    }    
    
    private createLoginTable() {
        var name = MetaData.PREFIX+"login";
        new Table(this, name, {
            tableName: name,
            billingMode: BillingMode.PAY_PER_REQUEST,
            partitionKey: {name: "email", type: AttributeType.STRING}
        });
    }
}