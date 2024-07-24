using Autofac;
using Autofac.Extensions.DependencyInjection;
using Para.Bussiness.Infrastructure.DependencyInjection;
using Serilog;

namespace Para.Api;

public class Program
{
    public static void Main(string[] args)
    {
        // Log to the console can be added, I didn't need it.
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("Logs/app.log") 
            .CreateLogger();

        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly...");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
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