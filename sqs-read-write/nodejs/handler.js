var AWS = require('aws-sdk');
AWS.config.update({region: 'eu-central-1'});
var sqs = new AWS.SQS({apiVersion: '2012-11-05'});
const { randomUUID } = require('crypto');

exports.handler = async (event) => {
    // TODO implement
    console.log(JSON.stringify(event));
    console.log("Input message=["+event.Records[0].body+"]");
    const response = {
        statusCode: 200,
        body: JSON.stringify('Reading from non FIFO queue and writing to FIFO queue...')
    }
    
    var targetQueueURL = "https://sqs.eu-central-1.amazonaws.com/299199322523/erp-sqs-read-write.fifo";
    //var targetQueueURL = "https://sqs.eu-central-1.amazonaws.com/299199322523/erp-target-sqs";
    var guid = randomUUID();
    console.log("guid=["+guid+"]");
    var params = {
        MessageBody: JSON.stringify(event)
        ,MessageDeduplicationId: guid  // Required for FIFO queues
        ,MessageGroupId: "GroupA"  // Required for FIFO queues
        ,QueueUrl: targetQueueURL
    };
    
    const promise = new Promise(async function(resolve, reject) {
      await sqs.sendMessage(params, function(err, data) {
        if (err) { console.log(err); reject(Error(err)); }
        else { console.log("Success", data.MessageId); resolve("Message put on FIFO queue"); }
      });
    });
    return promise;
        
    //return sqs.sendMessage(params).promise();
    
    /*await sqs.sendMessage(params, function(err, data) {
      if (err) {
        console.log("Error", err);
        throw err;
      } else {
        console.log("Success", data.MessageId);
        return response;
      }
    });*/
};
