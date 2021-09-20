using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Consul;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.Web.Profile;
using iread_assignment_ms.Web.Service;
using iread_assignment_ms.Web.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace iread_assignment_ms
{
    public class Startup
    {
        public static readonly Microsoft.Extensions.Logging.LoggerFactory _myLoggerFactory =
           new LoggerFactory(new[] {
        new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
           });

        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(Directory.GetCurrentDirectory() + "/Properties/launchSettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
        public static IConfiguration Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "_myAllowSpecificOrigins", builder =>
                     builder
                         .AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());
            });


            // for routing
            services.AddControllers();

            // for stop looping of json result
            services.AddMvc()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // for connection of DB
            services.AddDbContext<AppDbContext>(
                options =>
                {
                    options.UseLoggerFactory(_myLoggerFactory).UseMySQL(Configuration.GetConnectionString("DefaultConnection"));
                });


            // for consul
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = Configuration.GetValue<string>("ConsulConfig:Host");
                consulConfig.Address = new Uri(address);
            }));
            services.AddConsulConfig(Configuration);
            services.AddHttpClient<IConsulHttpClientService, ConsulHttpClientService>();


            // return only msg of errors as a list when get invalid ModelState in background
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage));
                    return new BadRequestObjectResult(errors);
                };
            });

            // for swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "iread_assignment_ms", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            // Inject the public repository
            services.AddScoped<IPublicRepository, PublicRepository>();


            IMapper mapper = new MapperConfiguration(config =>
           {
               config.AddProfile<AutoMapperProfile>();
           }).CreateMapper();
            services.AddSingleton(mapper);

            // for protected APIs
            var provider = services.BuildServiceProvider();
            var consulHttpClientService = provider.GetRequiredService<IConsulHttpClientService>();
            var identityService = consulHttpClientService.GetAgentService("identity_ms");
            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication("Bearer", options =>
            {
                options.ApiName = "api1";
                options.Authority = "http://" + identityService.Address + ":" + identityService.Port;
                options.RequireHttpsMetadata = false;
            });

            services.AddAuthorization(options =>
            {

                options.AddPolicy(Policies.Administrator, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireScope(Policies.Administrator);
                });
                options.AddPolicy(Policies.Teacher, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireScope(Policies.Teacher);
                });
                options.AddPolicy(Policies.Student, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireScope(Policies.Student);
                });
            });

            //services
            services.AddScoped<AssignmentService>();
            services.AddScoped<MultiChoiceService>();
            services.AddScoped<EssayQuestionService>();
            services.AddScoped<InteractionQuestionService>();
            services.AddScoped<EssayAnswerService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "iread_assignment_ms v1"));
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("_myAllowSpecificOrigins");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseConsul(Configuration);
        }
    }
}
