const AWS = require('aws-sdk');
//AWS.config.update({region: 'eu-central-1'});
const sqs = new AWS.SQS({apiVersion: '2012-11-05'});
const { randomUUID } = require('crypto');

exports.handler = async (event) => {
    logInput(event);
    var targetQueueURL =  process.env.targetQueueURL;    
    var guid = randomUUID();
    const groupId = "GroupA";

    console.log("guid=["+guid+"]");
    var params = {
        MessageBody: JSON.stringify(event)
        ,MessageDeduplicationId: guid  // Required for FIFO queues
        ,MessageGroupId: groupId  // Required for FIFO queues
        ,QueueUrl: targetQueueURL
    };
    
    const promise = new Promise(async function(resolve, reject) {
      await sqs.sendMessage(params, function(err, data) {
        if (err) { console.log(err); reject(Error(err)); }
        else { console.log("Success", data.MessageId); resolve("Message put on FIFO queue"); }
      });
    });
    return promise;
};

function logInput(event) {
    console.log("EVENT\n=================================");
    console.log(JSON.stringify(event));
    console.log("=================================");
    console.log("Input message=["+event.Records[0].body+"]");
}