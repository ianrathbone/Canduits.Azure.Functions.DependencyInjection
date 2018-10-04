# Canduits.Azure.Functions.DependencyInjection
Simplistic Microsoft.Extensions.DependencyInjection implementation exposing a binding attribute for use with Azure Functions in NetCore 2.1

![Azure Pipeline Build Status](https://ianrathbone.visualstudio.com/Open%20Source/_apis/build/status/Build%20and%20Publish%20Canduits%20Functions%20DI%20Package "Azure Pipeline Build Status")

Example usage:

### Function Declaration using a defined interface
```C#
[FunctionName("ExampleFunction")]
public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req,
[Inject] IExampleInterface example, ILogger log)
{
    example.DoSomeCoolStuff();
    return new OkResult();
}
```

### Basic Setup in Startup.cs
```C#
using System;
using Canduits.Azure.Functions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup), "Startup")]
namespace my.new.function
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddLogging();

            var myNewConfig = new MyNewConfig();
            config.GetSection("MyNewConfig").Bind(myNewConfig);
            builder.Services.Configure<MyNewConfig>(config.GetSection("MyNewConfig"));

            builder.Services.AddSingleton<IExampleInterface, ExampleImplementation>();

            builder.Services.AddSingleton<InjectBindingProvider>();
            builder.AddExtension<InjectConfigProvider>();
        }
    }
}

```
