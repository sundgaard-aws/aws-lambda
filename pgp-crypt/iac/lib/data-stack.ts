import * as Core from '@aws-cdk/core';
import S3 = require('@aws-cdk/aws-s3');
import { IRole } from "@aws-cdk/aws-iam";
import { IVpc } from '@aws-cdk/aws-ec2';
import { AttributeType, BillingMode, Table } from '@aws-cdk/aws-dynamodb';
import { MetaData } from './meta-data';
import { Bucket, EventType } from '@aws-cdk/aws-s3';
import { StackProps, Tags } from '@aws-cdk/core';
import { Alias, IKey, Key } from '@aws-cdk/aws-kms';
import { v4 } from 'uuid';
import { IQueue } from '@aws-cdk/aws-sqs';
import { SqsDestination } from '@aws-cdk/aws-s3-notifications';
import { hasUncaughtExceptionCaptureCallback } from 'process';

export interface DataStackProps extends StackProps {
    key: IKey;
    sqsRequestEventTarget: IQueue;
    sqsResponseEventTarget: IQueue;
}

export class DataStack extends Core.Stack {
    private apiRole:IRole;
    keyAlias: Alias | undefined;
    private props:DataStackProps;
    constructor(scope: Core.Construct, id: string, apiRole: IRole, props?: DataStackProps) {
        super(scope, id, props);
        this.apiRole = apiRole;
        if(props==undefined) throw("Please make sure that the properties are initialized!");
        this.props=props;
        this.keyAlias=props?.key.addAlias(v4().toString());

        //this.createLoginTable();
        this.createRequestBucket();
        this.createResponseBucket();
    }

    private createRequestBucket() {
        var name = MetaData.PREFIX+"pay-req";
        var bucket = new Bucket(this, name, {
            bucketName: name, 
            encryptionKey: this.keyAlias
        });
        bucket.grantReadWrite(this.apiRole);
        bucket.addEventNotification(EventType.OBJECT_CREATED, new SqsDestination(this.props.sqsRequestEventTarget));
        Core.Tags.of(bucket).add(MetaData.NAME, name);
    }    
    
    private createResponseBucket() {
        var name = MetaData.PREFIX+"pay-res";
        var bucket = new Bucket(this, name, {
            bucketName: name, encryptionKey: this.keyAlias
        });
        bucket.grantReadWrite(this.apiRole);
        bucket.addEventNotification(EventType.OBJECT_CREATED, new SqsDestination(this.props.sqsRequestEventTarget));
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