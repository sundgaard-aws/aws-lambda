import json
import datetime
import boto3
import os
import sys


def lambda_handler(event, context):
    print("started at ["+str(datetime.datetime.now())+"]...")
    
    bucket = "acc-day-glue-trade-input-bucket"
    file_name = "fx-trades-large-with-id.csv"
    cleanedFileName="fx-trades-large-with-id-cleaned.csv"
    
    print("sys.path defined as:")
    print("---------------------")
    for path in sys.path:
        print(path)
    print("---------------------")
    print("/opt/python/lib/python3.9/site-packages folder contains:")
    print("---------------------")
    for path in os.listdir("/opt/python/lib/python3.9/site-packages"):
        print(path)
    print("---------------------")
    print("PATH=["+str(os.environ['PATH'])+"]")
    #os.environ["PATH"] += os.pathsep + "/opt"
    #print("NEW PATH=["+str(os.environ['PATH'])+"]")    
    
    print("ended at ["+str(datetime.datetime.now())+"].")

    return {
        'statusCode': 200,
        'body': json.dumps('Done.')
    }
