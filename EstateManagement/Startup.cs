namespace EstateManagement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Common;
    using BusinessLogic.EventHandling;
    using BusinessLogic.Manger;
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using BusinessLogic.Services;
    using Common;
    using Controllers;
    using EstateReporting.Database;
    using EventStore.Client;
    using HealthChecks.UI.Client;
    using IdentityModel.AspNetCore.OAuth2Introspection;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Models.Factories;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NLog.Extensions.Logging;
    using Repository;
    using SecurityService.Client;
    using Shared.DomainDrivenDesign.CommandHandling;
    using Shared.EntityFramework;
    using Shared.EntityFramework.ConnectionStringConfiguration;
    using Shared.EventStore.EventStore;
    using Shared.Extensions;
    using Shared.General;
    using Shared.Logger;
    using Shared.Repositories;
    using Swashbuckle.AspNetCore.Filters;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using AuthenticationFailedContext = Microsoft.AspNetCore.Authentication.JwtBearer.AuthenticationFailedContext;
    using ConnectionStringType = Shared.Repositories.ConnectionStringType;
    using ILogger = Microsoft.Extensions.Logging.ILogger;
    using TokenValidatedContext = Microsoft.AspNetCore.Authentication.JwtBearer.TokenValidatedContext;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="webHostEnvironment">The web host environment.</param>
        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(webHostEnvironment.ContentRootPath)
                                                                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                                      .AddJsonFile($"appsettings.{webHostEnvironment.EnvironmentName}.json", optional: true).AddEnvironmentVariables();

            Startup.Configuration = builder.Build();
            Startup.WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Gets or sets the web host environment.
        /// </summary>
        /// <value>
        /// The web host environment.
        /// </value>
        public static IWebHostEnvironment WebHostEnvironment { get; set; }

        private static EventStoreClientSettings EventStoreClientSettings;

        private static void ConfigureEventStoreSettings(EventStoreClientSettings settings = null)
        {
            if (settings == null)
            {
                settings = new EventStoreClientSettings();
            }

            settings.CreateHttpMessageHandler = () => new SocketsHttpHandler
                                                      {
                                                          SslOptions =
                                                          {
                                                              RemoteCertificateValidationCallback = (sender,
                                                                                                     certificate,
                                                                                                     chain,
                                                                                                     errors) => true,
                                                          }
                                                      };
            settings.ConnectionName = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionName");
            settings.ConnectivitySettings = new EventStoreClientConnectivitySettings
                                            {
                                                Address = new Uri(Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionString")),
                                            };

            settings.DefaultCredentials = new UserCredentials(Startup.Configuration.GetValue<String>("EventStoreSettings:UserName"),
                                                              Startup.Configuration.GetValue<String>("EventStoreSettings:Password"));
            Startup.EventStoreClientSettings = settings;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigurationReader.Initialise(Startup.Configuration);

            Startup.ConfigureEventStoreSettings();

            this.ConfigureMiddlewareServices(services);

            services.AddTransient<IMediator, Mediator>();
            services.AddSingleton<IEstateManagementManager, EstateManagementManager>();
            
            Boolean useConnectionStringConfig = Boolean.Parse(ConfigurationReader.GetValue("AppSettings", "UseConnectionStringConfig"));
            
            if (useConnectionStringConfig)
            {
                String connectionStringConfigurationConnString = ConfigurationReader.GetConnectionString("ConnectionStringConfiguration");
                services.AddSingleton<IConnectionStringConfigurationRepository, ConnectionStringConfigurationRepository>();
                services.AddTransient<ConnectionStringConfigurationContext>(c =>
                                                                            {
                                                                                return new ConnectionStringConfigurationContext(connectionStringConfigurationConnString);
                                                                            });

                // TODO: Read this from a the database and set
            }
            else
            {
                services.AddEventStoreClient(Startup.ConfigureEventStoreSettings);
                services.AddEventStoreProjectionManagerClient(Startup.ConfigureEventStoreSettings);

                services.AddSingleton<IConnectionStringConfigurationRepository, ConfigurationReaderConnectionStringRepository>();
            }

            services.AddTransient<IEventStoreContext, EventStoreContext>();
            services.AddSingleton<IEstateManagementRepository, EstateManagementRepository>();
            services.AddSingleton<IDbContextFactory<EstateReportingContext>, DbContextFactory<EstateReportingContext>>();

            Dictionary<String, String[]> handlerEventTypesToSilentlyHandle = new Dictionary<String, String[]>();

            if (Startup.Configuration != null)
            {
                IConfigurationSection section = Startup.Configuration.GetSection("AppSettings:HandlerEventTypesToSilentlyHandle");

                if (section != null)
                {
                    Startup.Configuration.GetSection("AppSettings:HandlerEventTypesToSilentlyHandle").Bind(handlerEventTypesToSilentlyHandle);
                }
            }

            services.AddSingleton<Func<String, EstateReportingContext>>(cont => (connectionString) => { return new EstateReportingContext(connectionString); });

            DomainEventTypesToSilentlyHandle eventTypesToSilentlyHandle = new DomainEventTypesToSilentlyHandle(handlerEventTypesToSilentlyHandle);
            services.AddTransient<IEventStoreContext, EventStoreContext>();
            services.AddSingleton<IAggregateRepository<EstateAggregate.EstateAggregate>, AggregateRepository<EstateAggregate.EstateAggregate>>();
            services.AddSingleton<IAggregateRepository<MerchantAggregate.MerchantAggregate>, AggregateRepository<MerchantAggregate.MerchantAggregate>>();
            services.AddSingleton<IAggregateRepository<ContractAggregate.ContractAggregate>, AggregateRepository<ContractAggregate.ContractAggregate>>();
            services.AddSingleton<IEstateDomainService, EstateDomainService>();
            services.AddSingleton<IMerchantDomainService, MerchantDomainService>();
            services.AddSingleton<IContractDomainService, ContractDomainService>();
            services.AddSingleton<IModelFactory, ModelFactory>();
            services.AddSingleton<Factories.IModelFactory, Factories.ModelFactory>();
            services.AddSingleton<ISecurityServiceClient, SecurityServiceClient>();

            //// request & notification handlers
            services.AddTransient<ServiceFactory>(context =>
            {
                return t => context.GetService(t);
            });

            services.AddSingleton<IRequestHandler<CreateEstateRequest, String>, EstateRequestHandler>();
            services.AddSingleton<IRequestHandler<CreateEstateUserRequest, Guid>, EstateRequestHandler>();
            services.AddSingleton<IRequestHandler<AddOperatorToEstateRequest, String>, EstateRequestHandler>();

            services.AddSingleton<IRequestHandler<CreateMerchantRequest, String>, MerchantRequestHandler>();
            services.AddSingleton<IRequestHandler<AssignOperatorToMerchantRequest, String>, MerchantRequestHandler>();
            services.AddSingleton<IRequestHandler<CreateMerchantUserRequest, Guid>, MerchantRequestHandler>();
            services.AddSingleton<IRequestHandler<AddMerchantDeviceRequest, String>, MerchantRequestHandler>();
            services.AddSingleton<IRequestHandler<MakeMerchantDepositRequest, Guid>, MerchantRequestHandler>();

            services.AddSingleton<IRequestHandler<CreateContractRequest, String>, ContractRequestHandler>();
            services.AddSingleton<IRequestHandler<AddProductToContractRequest, String>, ContractRequestHandler>();
            services.AddSingleton<IRequestHandler<AddTransactionFeeForProductToContractRequest, String>, ContractRequestHandler>();

            services.AddSingleton<Func<String, String>>(container => (serviceName) =>
            {
                return ConfigurationReader.GetBaseServerUri(serviceName).OriginalString;
            });
            services.AddSingleton<HttpClient>();
        }
        
        /// <summary>
        /// Configures the middleware services.
        /// </summary>
        /// <param name="services">The services.</param>
        private void ConfigureMiddlewareServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddSqlServer(connectionString:ConfigurationReader.GetConnectionString("HealthCheck"),
                                  healthQuery:"SELECT 1;",
                                  name:"Read Model Server",
                                  failureStatus:HealthStatus.Degraded,
                                  tags:new string[] {"db", "sql", "sqlserver"})
                    .AddEventStore(Startup.EventStoreClientSettings,
                                   userCredentials: Startup.EventStoreClientSettings.DefaultCredentials,
                                   name: "Eventstore",
                                     failureStatus: HealthStatus.Unhealthy,
                                     tags: new string[] { "db", "eventstore" })
                    .AddUrlGroup(new Uri($"{ConfigurationReader.GetValue("SecurityConfiguration", "Authority")}/.well-known/openid-configuration"),
                         name:"Security Service",
                         httpMethod:HttpMethod.Get,
                         failureStatus:HealthStatus.Unhealthy,
                         tags: new string[] { "security", "authorisation" });


            services.AddApiVersioning(
                                      options =>
                                      {
                                          // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                                          options.ReportApiVersions = true;
                                          options.DefaultApiVersion = new ApiVersion(1, 0);
                                          options.AssumeDefaultVersionWhenUnspecified = true;
                                          options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                                      });

            services.AddVersionedApiExplorer(
                                             options =>
                                             {
                                                 // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                                                 // note: the specified format code will format the version as "'v'major[.minor][-status]"
                                                 options.GroupNameFormat = "'v'VVV";

                                                 // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                                                 // can also be used to control the format of the API version in route templates
                                                 options.SubstituteApiVersionInUrl = true;
                                             });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                // add a custom operation filter which sets default values
                c.OperationFilter<SwaggerDefaultValues>();
                c.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblyOf<SwaggerJsonConverter>();

            services.AddAuthentication(options =>
                                       {
                                           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                           options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                                       })
                .AddJwtBearer(options =>
                              {
                                  //options.SaveToken = true;
                                  options.Authority = ConfigurationReader.GetValue("SecurityConfiguration", "Authority");
                                  options.Audience = ConfigurationReader.GetValue("SecurityConfiguration", "ApiName");
                                  options.RequireHttpsMetadata = false;
                                  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                                  {
                                      ValidateIssuer = true,
                                      ValidateAudience = true,
                                      ValidAudience = ConfigurationReader.GetValue("SecurityConfiguration", "ApiName"),
                                      ValidIssuer = ConfigurationReader.GetValue("SecurityConfiguration", "Authority"),
                                  };
                                  options.IncludeErrorDetails = true;
                              });

            services.AddControllers().AddNewtonsoftJson(options =>
                                                        {
                                                            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                                            options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                                                            options.SerializerSettings.Formatting = Formatting.Indented;
                                                            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                                                            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                                        });

            Assembly assembly = this.GetType().GetTypeInfo().Assembly;
            services.AddMvcCore().AddApplicationPart(assembly).AddControllersAsServices();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="provider">The provider.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory,
                              IApiVersionDescriptionProvider provider)
        {
            String nlogConfigFilename = "nlog.config";

            if (env.IsDevelopment())
            {
                nlogConfigFilename = $"nlog.{env.EnvironmentName}.config";
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.ConfigureNLog(Path.Combine(env.ContentRootPath, nlogConfigFilename));
            loggerFactory.AddNLog();

            ILogger logger = loggerFactory.CreateLogger("EstateManagement");

            Logger.Initialise(logger);

            ConfigurationReader.Initialise(Startup.Configuration);

            app.AddRequestLogging();
            app.AddResponseLogging();
            app.AddExceptionHandler();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapControllers();
                                 endpoints.MapHealthChecks("health", new HealthCheckOptions()
                                                                      {
                                                                          Predicate = _ => true,
                                                                          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                                                                      });
                             });
            app.UseSwagger();

            app.UseSwaggerUI(
                             options =>
                             {
                                 // build a swagger endpoint for each discovered API version
                                 foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                                 {
                                     options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                                 }
                             });

            if (String.Compare(ConfigurationReader.GetValue("EventStoreSettings", "START_PROJECTIONS"),
                               Boolean.TrueString,
                               StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                app.PreWarm(true).Wait();
            }
            else
            {
                app.PreWarm();
            }
        }

    }
    
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// 
    /// </summary>
    public static class StartupExtensions
    {
        #region Methods

        /// <summary>
        /// Pres the warm.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="startProjections">if set to <c>true</c> [start projections].</param>
        public static async Task PreWarm(this IApplicationBuilder applicationBuilder,
                                         Boolean startProjections = false)
        {
            try
            {
                if (startProjections)
                {
                    await StartupExtensions.StartEventStoreProjections();
                }

                //ConnectionStringConfigurationContext context = new ConnectionStringConfigurationContext("server=localhost;database=ConnectionStringConfiguration;user id=sa;password=sp1ttal");
                //await context.Database.EnsureCreatedAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Starts the event store projections.
        /// </summary>
        private static async Task StartEventStoreProjections()
        {
            // TODO: Refactor
            // This method is a brutal way of getting projections to be run when the API starts up
            // This needs refactored into a more pleasant function
            String connString = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionString");
            String connectionName = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionName");
            Int32 httpPort = Startup.Configuration.GetValue<Int32>("EventStoreSettings:HttpPort");

            EventStoreConnectionSettings settings = EventStoreConnectionSettings.Create(connString, connectionName, httpPort);

            // Do continuous first
            String continuousProjectionsFolder = ConfigurationReader.GetValue("EventStoreSettings", "ContinuousProjectionsFolder");
            String[] files = Directory.GetFiles(continuousProjectionsFolder, "*.js");

            // TODO: Improve this later to get status of projection before trying to create
            //foreach (String file in files)
            //{
            //    String withoutExtension = Path.GetFileNameWithoutExtension(file);
            //    String projectionBody = File.ReadAllText(file);
            //    Boolean emitEnabled = continuousProjectionsFolder.ToLower().Contains("emitenabled");

            //    await Retry.For(async () =>
            //                    {
            //                        DnsEndPoint d = new DnsEndPoint(settings.IpAddress, settings.HttpPort);

            //                        ProjectionsManager projectionsManager = new ProjectionsManager(new ConsoleLogger(), d, TimeSpan.FromSeconds(5));

            //                        UserCredentials userCredentials = new UserCredentials(settings.UserName, settings.Password);

            //                        String projectionStatus = await projectionsManager.GetStatusAsync(withoutExtension, userCredentials);

            //                        await projectionsManager.CreateContinuousAsync(withoutExtension, projectionBody, userCredentials);
            //                        Logger.LogInformation("After CreateContinuousAsync");
            //                        if (emitEnabled)
            //                        {
            //                            await projectionsManager.AbortAsync(withoutExtension, userCredentials);
            //                            await projectionsManager.UpdateQueryAsync(withoutExtension, projectionBody, emitEnabled, userCredentials);
            //                            await projectionsManager.EnableAsync(withoutExtension, userCredentials);
            //                        }
            //                    });
            //}
        }

        #endregion
    }
}
