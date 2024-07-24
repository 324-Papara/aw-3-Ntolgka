using System.Reflection;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.OpenApi.Models;
using Para.Api.Middleware;
using Para.Bussiness.Cqrs;
using FluentValidation;
using FluentValidation.AspNetCore;
using Para.Bussiness.Validation;

namespace Para.Api;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // The definition suggested for adding FluentValidation in the sources is not like this, but the methods they use are deprecate.
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
               
        services.AddControllers()
            .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Para.Api", Version = "v1" });
        });

        services.AddMediatR(typeof(CreateCustomerCommand).GetTypeInfo().Assembly);
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Para.Api v1"));
        }

        app.UseMiddleware<RequestResponseLoggerMiddleware>();
        
        app.UseMiddleware<HeartbeatMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}