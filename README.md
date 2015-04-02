# ElasticBeanstalkMiddleware
A middleware for a ASP.NET vNext worker in AWS Elastic Beanstalk

[![Build Status](https://travis-ci.org/sklose/ElasticBeanstalkMiddleware.svg?branch=master)](https://travis-ci.org/sklose/ElasticBeanstalkMiddleware)
[![Build status](https://ci.appveyor.com/api/projects/status/dxtl9kx5qyuyy4bd?svg=true)](https://ci.appveyor.com/project/sklose/elasticbeanstalkmiddleware)

# Quick Start
Add NuGet package ElasticBeanstalkMiddleware to your ASP.NET vNext application. A simple implementation:

```csharp
public class Startup
{
    public void Configure(IBuilder app)
    {
        app.RunElasticBeanstalkWorker(
            new EbOptions { Dispatcher = new MessageDispatcher() });
    }
}

public class MessageDispatcher : IMessageDispatcher
{
    public void Dispatch(SqsMessage message)
    {
        Console.WriteLine("Received a new message to process");
    }
}
```