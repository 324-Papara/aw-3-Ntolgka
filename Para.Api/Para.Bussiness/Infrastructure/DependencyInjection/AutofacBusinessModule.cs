using System.Data;
using Autofac;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Para.Bussiness.Cqrs;
using Para.Data.Context;
using Para.Data.GenericRepository;
using Para.Data.Repository.DapperRepository;
using Para.Data.UnitOfWork;


namespace Para.Bussiness.Infrastructure.DependencyInjection;

public class AutofacBusinessModule : Module
{
    private readonly IConfiguration Configuration;

    public AutofacBusinessModule(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    //TODO - InstancePerLifetimeScope() can be used.
    protected override void Load(ContainerBuilder builder)
    {
        // Database Connection and DbContext
        // var connectionStringSql = Configuration.GetConnectionString("MsSqlConnection");
        var connectionStringPostgre = Configuration.GetConnectionString("PostgresSqlConnection");

        builder.Register(c => new ParaDbContext(new DbContextOptionsBuilder<ParaDbContext>()
            .UseNpgsql(connectionStringPostgre).Options))   // .UseSqlServer(connectionStringSql) for MSSQL
            .AsSelf()
            .InstancePerLifetimeScope();
        
        // IDbConnection for Dapper
        builder.Register(c => new NpgsqlConnection(connectionStringPostgre))  // Use SqlConnection for SQL Server
            .As<IDbConnection>()
            .InstancePerLifetimeScope();
        
        //Mapper
        var mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MapperConfig());
        });
        
        builder.RegisterInstance(mapConfig.CreateMapper())
            .As<IMapper>()
            .SingleInstance();
        
        // MediatR
        builder.RegisterAssemblyTypes(typeof(CreateCustomerCommand).Assembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
        
        builder.Register<ServiceFactory>(context =>
        {
            var c = context.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        }).InstancePerLifetimeScope();
        
        // TODO - Register FluentValidation in AutofacBusinessModel(?)
        
        // Other Services
        builder.RegisterGeneric(typeof(GenericRepository<>))
            .As(typeof(IGenericRepository<>))
            .InstancePerLifetimeScope();
        
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<CustomerRepository>()
            .As<ICustomerRepository>()
            .InstancePerLifetimeScope();

    }
}