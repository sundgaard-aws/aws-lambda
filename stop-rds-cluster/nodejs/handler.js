var AWS = require('aws-sdk');

exports.handler = function(event) {
    // aws rds stop-db-instance db-instance-identifier test-instance
    var rds = new AWS.RDS({apiVersion: '2014-10-31'});
    //await usingAwait();
    
    const response = {
        statusCode: 200,
        body: JSON.stringify('RDS cluster stop initiated for all DEV databases!'),
    }
     var params = { DBClusterIdentifier: 'aurora-db-cluster1' /* required */ };
     rds.stopDBCluster(params, function(err, data) {
        if (err) { console.log(err, err.stack); console.log("err="+err); }
        else     { console.log(data); console.log("data="+data); }
    });
    
};

