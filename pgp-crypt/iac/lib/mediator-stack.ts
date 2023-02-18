import * as Core from '@aws-cdk/core';
import { IRole } from "@aws-cdk/aws-iam";
import { StackProps, Tags } from '@aws-cdk/core';
import { Alias, IKey, Key } from '@aws-cdk/aws-kms';
import { v4 } from 'uuid';
import { IQueue } from '@aws-cdk/aws-sqs';
import { SqsDestination } from '@aws-cdk/aws-s3-notifications';
import { EventType, IBucket } from '@aws-cdk/aws-s3';

export interface MediatorStackProps extends StackProps {
    key: IKey;
    sqsRequestEventTarget: IQueue;
    sqsResponseEventTarget: IQueue;
    paymentRequestBucket: IBucket;
    paymentResponseBucket: IBucket;
}

export class MediatorStack extends Core.Stack {
    private apiRole:IRole;
    private keyAlias: Alias | undefined;
    private props:MediatorStackProps;
    constructor(scope: Core.Construct, id: string, apiRole: IRole, props?: MediatorStackProps) {
        super(scope, id, props);
        this.apiRole = apiRole;
        if(props==undefined) throw("Please make sure that the properties are initialized!");
        this.props=props;
        this.keyAlias=props?.key.addAlias(v4().toString());
        this.addEvents();
    }

    private addEvents() {
        this.props.paymentRequestBucket.addEventNotification(EventType.OBJECT_CREATED, new SqsDestination(this.props.sqsRequestEventTarget));
        this.props.paymentResponseBucket.addEventNotification(EventType.OBJECT_CREATED, new SqsDestination(this.props.sqsRequestEventTarget));
        //Core.Tags.of(bucket).add(MetaData.NAME, name);
    }    
}