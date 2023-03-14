#!/usr/bin/env node
import 'source-map-support/register';
import * as cdk from '@aws-cdk/core';
import { env } from 'process';
import { MetaData } from './meta-data';
import { NetworkStack } from './network-stack';
import { ComputeStack } from './compute-stack';
import { DataStack } from './data-stack';

const app = new cdk.App();
var enableGrants=true
var props = {env: {account: process.env["CDK_DEFAULT_ACCOUNT"], region: MetaData.REGION } };
var metaData = new MetaData();

var networkStack = new NetworkStack(app, MetaData.PREFIX+"network-stack", props);
var computeStack = new ComputeStack(app, MetaData.PREFIX+"compute-stack", networkStack.ApiRole, props);
var dataStack=new DataStack(app, MetaData.PREFIX+"data-stack", networkStack.ApiRole, { 
    env: {account: process.env["CDK_DEFAULT_ACCOUNT"], region: MetaData.REGION }
});
