import json
import pandas as pd
import datetime
import boto3
#from io import StringIO
import fsspec
import s3fs
import os
import sys


def lambda_handler(event, context):
    print("started data generation at ["+str(datetime.datetime.now())+"]...")
    
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
    
    s3 = boto3.client("s3") 
    obj = s3.get_object(Bucket=bucket, Key=file_name) 
    df = pd.read_csv(obj["Body"], index_col=0)
    print(df)
    
    # Get a few sample IDs to delete as files are auto generated and synthetic
    guidA=df.iloc[2]
    guidB=df.iloc[125000]
    guidC=df.iloc[500000]
    print("Random GUID to delete ["+str(guidA.name)+"]")
    print("Random GUID to delete ["+str(guidB.name)+"]")
    print("Random GUID to delete ["+str(guidC.name)+"]")
    
    # Delete rows. Parameter is the index value for the index column specified above
    df = df.drop(str(guidA.name))
    df = df.drop(str(guidB.name))
    df = df.drop(str(guidC.name))
    
    # Needs fsspec and s3fs import
    df.to_csv("s3://"+bucket+"/fx-trades-large-with-id-cleaned.csv",encoding="UTF-8")
    
    print("data generation ended at ["+str(datetime.datetime.now())+"].")

    return {
        'statusCode': 200,
        'body': json.dumps('Record deleted from S3.')
    }
