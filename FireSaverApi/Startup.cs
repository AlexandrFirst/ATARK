using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Helpers.ExceptionHandler;
using FireSaverApi.hub;
using FireSaverApi.Models;
using FireSaverApi.Profiles;
using FireSaverApi.Services;
using FireSaverApi.Services.PointServices;
using FireSaverApi.Services.shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.Extensions;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;

namespace FireSaverApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Task.Run(AddRolesIfEmpty).Wait();
            Task.Run(AddAdminIfItNotExists).Wait();
        }

        async Task AddAdminIfItNotExists()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("FireSaverDbConnectionString"));

            using (DatabaseContext dbContext
                        = new DatabaseContext(optionsBuilder.Options))
            {
                var roleHelper = new UserRoleHelper(dbContext);

                var userWithAdminRights = dbContext.Users.Include(r => r.RolesList)
                                                        .FirstOrDefault(u => u.RolesList.Any(r => r.Name == UserRoleName.ADMIN));

                var adminRole = await roleHelper.GetRoleByName(UserRoleName.ADMIN);



                if (userWithAdminRights == null)
                {
                    var userAdmin = new User()
                    {
                        Mail = "root@gmail.com",
                        Name = "Admin",
                        Password = CalcHelper.ComputeSha256Hash("admin"),
                        TelephoneNumber = "000000000",
                        Surname = "Admin",
                        Patronymic = "Admin",
                        DOB = DateTime.MinValue
                    };

                    userAdmin.RolesList.Add(adminRole);

                    dbContext.Users.Add(userAdmin);
                    dbContext.SaveChanges();
                }
            }
        }

        async Task AddRolesIfEmpty()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("FireSaverDbConnectionString"));

            using (DatabaseContext dbContext
                        = new DatabaseContext(optionsBuilder.Options))
            {
                var allRoles = await dbContext.UserRoles.ToListAsync();
                if (allRoles.Count() > 0)
                    return;

                var roleHelper = new UserRoleHelper(dbContext);
                await roleHelper.AddUserRoles(UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER, UserRoleName.GUEST);
            }
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("AnyHeadersAllowed", builder =>
                {
                    builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                });
            });

            string username = Configuration["MqttOption:UserName"];
            string password = Configuration["MqttOption:Password"];

            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionValidator(c =>
                {
                    if (c.Username != username)
                    {
                        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        return;
                    }

                    if (c.Password != password)
                    {
                        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        return;
                    }

                    c.ReasonCode = MqttConnectReasonCode.Success;
                });
            var option = optionsBuilder.Build();

            services
                .AddHostedMqttServer(option) //mqttServer => mqttServer.WithoutDefaultEndpoint() | option
                .AddMqttConnectionHandler()
                .AddConnections();

            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FireSaverDbConnectionString")));
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });


            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthUserService, UserService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IUserHelper, UserService>();

            services.AddScoped<IBuildingService, BuildingService>();
            services.AddScoped<IBuildingHelper, BuildingService>();

            services.AddScoped<ICompartmentService<FloorDto, Floor>, FloorService>();
            services.AddScoped<ICompartmentService<RoomDto, Room>, RoomService>();

            services.AddScoped<ICompartmentHelper, CompartmentHelper>();

            services.AddScoped<IPlanImageUploadService, PlanImageCloudinaryService>();
            services.AddScoped<ICompartmentDataCloudinaryService, CompartmentDataCloudinaryService>();

            services.AddScoped<IEvacuationService, EvacuationService>();
            services.AddScoped<IEvacuationServiceHelper, EvacuationServiceHelper>();

            services.AddScoped<IScalePointService, ScalePointService>();

            services.AddScoped<IIoTService, IoTService>();
            services.AddScoped<IIoTHelper, IoTService>();

            services.AddScoped<ITestService, TestService>();

            services.AddScoped<ISocketService, SocketService>();

            services.AddScoped<IUserRoleHelper, UserRoleHelper>();

            services.AddScoped<IMessageService, MessageService>();

            services.AddScoped<IIotControllerService, IotControllerService>();
            

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<ITimerService, TimerService>();
            services.AddSingleton<CompartmentDataStorage>();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.Configure<BackupModel>(Configuration.GetSection("BackupSettings"));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PointProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new BuildingProfile());
                mc.AddProfile(new CompartmentProfile());
                mc.AddProfile(new EvacuationPlanProfile());
                mc.AddProfile(new ScaleModelProfile());
                mc.AddProfile(new IoTProfile());
                mc.AddProfile(new TestProfile());
                mc.AddProfile(new MessageProfile());

            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FireSaverApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FireSaverApi v1"));
            }
            app.ConfigureCustomExceptionMiddleware();

            app.UseRouting();

            app.UseMiddleware<JwtMiddleware>();

            app.UseCors("AnyHeadersAllowed");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SocketHub>("/socket");
                endpoints.MapMqtt("/mqtt");
            });


            app.UseMqttServer(server =>
            {
                server.StopAsync();
                server.StartedHandler = new MqttServerStartedHandlerDelegate(async args =>
                {
                    System.Console.WriteLine("MQTT server is running");
                });

                server.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(async args =>
                  {
                      System.Console.WriteLine("New client connected");
                  });
                server.StartAsync(server.Options);
            });

            ServiceLocator.Instance = app.ApplicationServices;

        }
    }
}
