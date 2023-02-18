import * as Core from '@aws-cdk/core';
import { MetaData } from './meta-data';
import { IQueue, Queue, QueueEncryption } from '@aws-cdk/aws-sqs';
import { SSMHelper } from './ssm-helper';
import * as SSM from '@aws-cdk/aws-ssm';
import { IKey } from '@aws-cdk/aws-kms';
import { StackProps } from '@aws-cdk/core';
import { v4 } from 'uuid';

export interface MessageStackProps extends StackProps {
    key: IKey;
}

export class MessageStack extends Core.Stack {
    private ssmHelper = new SSMHelper();
    public PaymentRequestQueue: IQueue;
    public PaymentResponseQueue: IQueue;
    private keyAlias: IKey | undefined;
    constructor(scope: Core.Construct, id: string, props?: MessageStackProps) {
        super(scope, id, props);
        this.keyAlias=props?.key.addAlias(v4().toString());
        this.PaymentRequestQueue=this.createPaymentRequestSQSQueue();
        this.PaymentResponseQueue=this.createPaymentResponseSQSQueue();
    }

    private createPaymentRequestSQSQueue():IQueue
    {
        var queue=this.createSQSQueue("payment-request");
        return queue;
    }

    private createPaymentResponseSQSQueue():IQueue
    {
        var queue=this.createSQSQueue("payment-response");
        return queue;
    }

    private createSQSQueue(prefix:string):IQueue
    {
        var dlqName=MetaData.PREFIX+prefix+"-dlq-sqs";
        var qName=MetaData.PREFIX+prefix+"-sqs";
        var ssmQueueURLParameterName=MetaData.PREFIX+prefix+"-sqs-queue-url";
        var deadLetterqueue = new Queue(this, dlqName, {
            queueName: dlqName, visibilityTimeout: Core.Duration.seconds(4), retentionPeriod: Core.Duration.days(14)
        });
        Core.Tags.of(deadLetterqueue).add(MetaData.NAME, dlqName);
        
        var queue = new Queue(this, qName, {
            queueName: qName, visibilityTimeout: Core.Duration.seconds(4), retentionPeriod: Core.Duration.days(14), 
            deadLetterQueue: {queue: deadLetterqueue, maxReceiveCount: 5}, encryption: QueueEncryption.KMS, encryptionMasterKey:this.keyAlias
            //,fifo: false
        });
        Core.Tags.of(queue).add(MetaData.NAME, qName);
        this.ssmHelper.createSSMParameter(this, ssmQueueURLParameterName, queue.queueUrl, SSM.ParameterType.STRING);
        return queue;
    }
}