# aws-ec2-switch-dotnet

## 概要


## 構成


## 詳細

### Lambdaの作成

- Runtime
    - .NET Core 2.1

### IAMロール

Lambda関数の実行ロールには、以下操作を許可するポリシーを作成・アタッチしておきます。

- ec2:StartInstances
- ec2:StopInstances
- ec2:Describe*

必要に応じて、`Resource`, `Condition`で制限してください。

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "ec2:StartInstances",
                "ec2:StopInstances"
            ],
            "Resource": "arn:aws:ec2:*:*:instance/*",
            "Condition": {
                "StringEquals": {
                    "ec2:ResourceTag/...": "..."
                }
            }
        },
        {
            "Effect": "Allow",
            "Action": "ec2:Describe*",
            "Resource": "*"
        }
    ]
}
```

### Lambdaの戻り値について

#### 参考

https://aws.amazon.com/jp/premiumsupport/knowledge-center/malformed-502-api-gateway/

#### 発生するエラー

> Execution failed due to configuration error: Malformed Lambda proxy response<br/>
> Method completed with status: 502

#### 実装

API Gatewayの統合プロキシを使用する場合、Handlerとして指定するメソッド（`FunctionHandler`, `FunctionHandlerAsync` etc.）の戻り値は上記URLに記載されている形式のJSONである必要があります。

```csharp
public class Function
{
    ...

    // NG
    public async Task<string> FunctionHandlerAsync(...)
    {
        ...

    // OK
    public async Task<ApiGatewayResponse> FunctionHandlerAsync(...)
    {
        ...
```

`ApiGatewayResponse` の定義は以下のとおりです。

```csharp
[JsonObject]
public class ApiGatewayResponse
{
    [JsonProperty("statusCode")]
    public HttpStatusCode StatusCode { get; set; }

    [JsonProperty("headers")]
    public IDictionary<string, string> Headers { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }

    public static IDictionary<string, string> DefaultHeaders => new Dictionary<string, string>();
}
```

## 各Lambdaについて

### List (DescribeInstances)

#### 実行例

以下形式のJSONをLambdaに渡します。

```json
{
    "request" : {
        "instanceIds" : [ "i-...", "i-..." ],
        "filters" : [
            {
                "name" : "tag-key",
                "values" : [ "..." ]
            },
            {
                "name" : "tag-value",
                "values" : [ "..." ]
            }
        ]
    }
}
```

##### 参考
https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/EC2/TDescribeInstancesRequest.html

### On (StartInstances)

#### 実行例

以下形式のJSONをLambdaに渡します。

```json
{
    "request" : {
        "instanceIds" : [ "i-...", "i-..." ],
    }
}
```

#### 参考
https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/EC2/TStartInstancesRequest.html

### Off (StopInstances)

#### 実行例

以下形式のJSONをLambdaに渡します。

```json
{
    "request" : {
        "instanceIds" : [ "i-...", "i-..." ],
    }
}
```

#### 参考
https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/EC2/TStopInstancesRequest.html
