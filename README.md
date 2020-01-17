# CloudMQTT Client

This is a simple client to interact with the [CloudMQTT](https://www.cloudmqtt.com/) management [HTTP API](https://www.cloudmqtt.com/docs-api.html).

## Quickstart

Add the `CloudMQTT.Client` package to your project:

```bash
dotnet add package CloudMQTT.Client
```

Then use it to do a thing:

```csharp
var client = CloudMqttApi.GetInstance("username", "password");
var users = await client.GetUsers(); // Gets a list of MQTT users for the instance
```

## Building

[![Travis](https://img.shields.io/travis/thzinc/CloudMQTT.svg)](https://travis-ci.org/thzinc/CloudMQTT)
[![NuGet](https://img.shields.io/nuget/v/CloudMQTT.Client.svg)](https://www.nuget.org/packages/CloudMQTT.Client/)
[![NuGet Pre Release](https://img.shields.io/nuget/vpre/CloudMQTT.Client.svg)](https://www.nuget.org/packages/CloudMQTT.Client/)

Ensure you have [installed .NET Core](https://www.microsoft.com/net/core)

To build a local/development copy, run the following:

```bash
dotnet restore
dotnet build
```

To run the tests, you'll need a CloudMQTT instance's username and password. If you don't have an instance, you can [sign up for free](https://www.cloudmqtt.com/plans.html). Note, the username and password are usually randomly-generated. (i.e., not the same as the credentials you log in to the CloudMQTT site with.)

```bash
CLOUDMQTT_USER=ajeamalr CLOUDMQTT_PASSWORD=uwjamd3k_uma dotnet test
```

## Code of Conduct

We are committed to fostering an open and welcoming environment. Please read our [code of conduct](CODE_OF_CONDUCT.md) before participating in or contributing to this project.

## Contributing

We welcome contributions and collaboration on this project. Please read our [contributor's guide](CONTRIBUTING.md) to understand how best to work with us.

## License and Authors

[![Daniel James logo](https://secure.gravatar.com/avatar/eaeac922b9f3cc9fd18cb9629b9e79f6.png?size=16) Daniel James](https://github.com/thzinc)

[![license](https://img.shields.io/github/license/thzinc/CloudMQTT.svg)](https://github.com/thzinc/CloudMQTT/blob/master/LICENSE)
[![GitHub contributors](https://img.shields.io/github/contributors/thzinc/CloudMQTT.svg)](https://github.com/thzinc/CloudMQTT/graphs/contributors)

This software is made available by Daniel James under the MIT license.
