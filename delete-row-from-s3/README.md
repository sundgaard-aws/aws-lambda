# Creating the Lambda layers
You will need to create layers for Pandas and SFS.

This can be done by running the commands below, in any Linux shell with Python and PIP version 3.9.
Note that the directory path inside the zip must be exact. You can view the Python SYS PATH by running the deploying the handler.py "check-python-env" function (sibling to this folder) to a separate Lambda function.

``` sh
mkdir -p pandas/python/lib/python3.9/site-packages
cd pandas
pip install pandas -t python/lib/python3.9/site-packages
zip -rq pandas.zip python/*
aws s3 cp pandas.zip s3://sundgaar-lambda-layers
```