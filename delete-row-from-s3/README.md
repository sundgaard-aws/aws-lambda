# Creating the Lambda layers
You will need to create layers for Pandas, FSSPEC and S3FS. Some layers such as BOTO3 (the AWS SDK for Python 3.x) will already be available inside the Lambda environment. 
The scripts below show how to create a AWS Lambda layer for use with Python 3.9.

This can be done by running the commands below, in any Linux shell with Python and PIP version 3.9.
Note that the directory path inside the zip must be exact. You can view the Python SYS PATH by running the deploying the handler.py "check-python-env" function (sibling to this folder) to a separate Lambda function.

## Create Pandas layer
``` sh
export python_layer=pandas
mkdir -p $python_layer/python/lib/python3.9/site-packages
cd $python_layer
pip install $python_layer -t python/lib/python3.9/site-packages
zip -rq $python_layer.zip python/*
#only needed if you want to create layer via GUI
#aws s3 cp $python_layer.zip s3://sundgaar-lambda-layers 
aws lambda publish-layer-version --layer-name python-$python_layer --zip-file fileb://$python_layer.zip --compatible-runtimes python3.9 --compatible-architectures x86_64 --region eu-central-1
```

## Create FSSPEC layer
``` sh
export python_layer=fsspec
mkdir -p $python_layer/python/lib/python3.9/site-packages
cd $python_layer
pip install $python_layer -t python/lib/python3.9/site-packages
zip -rq $python_layer.zip python/*
#only needed if you want to create layer via GUI
#aws s3 cp $python_layer.zip s3://sundgaar-lambda-layers 
aws lambda publish-layer-version --layer-name python-$python_layer --zip-file fileb://$python_layer.zip --compatible-runtimes python3.9 --compatible-architectures x86_64 --region eu-central-1
```

## Create S3FS layer
``` sh
export python_layer=s3fs
mkdir -p $python_layer/python/lib/python3.9/site-packages
cd $python_layer
pip install $python_layer -t python/lib/python3.9/site-packages
zip -rq $python_layer.zip python/*
#only needed if you want to create layer via GUI
#aws s3 cp $python_layer.zip s3://sundgaar-lambda-layers 
aws lambda publish-layer-version --layer-name python-$python_layer --zip-file fileb://$python_layer.zip --compatible-runtimes python3.9 --compatible-architectures x86_64 --region eu-central-1
```