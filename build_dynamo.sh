aws dynamodb create-table \
    --cli-input-json file://src/Stubias.SpeedTest.Api/dynamodb.Development.schema.json \
    --endpoint-url http://127.0.0.1:8000
