using Autofac;
using Autofac.Extensions.DependencyInjection;
using Para.Bussiness.Infrastructure.DependencyInjection;

namespace Para.Api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((context, builder)=>
            {
                builder.RegisterModule(new AutofacBusinessModule(context.Configuration));
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}