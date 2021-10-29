using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Helpers.ExceptionHandler;
using FireSaverApi.Profiles;
using FireSaverApi.Services;
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
using Newtonsoft.Json;

namespace FireSaverApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            AddAdminIfItNotExists();
        }

        void AddAdminIfItNotExists()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("FireSaverDbConnectionString"));

            using (DatabaseContext dbContext
                        = new DatabaseContext(optionsBuilder.Options))
            {
                var userWithAdminRights = dbContext.Users.FirstOrDefault(u => u.RolesList.Contains(UserRole.ADMIN));
                if (userWithAdminRights == null)
                {
                    var userAdmin = new User()
                    {
                        Mail = "root@gmail.com",
                        Name = "Admin",
                        Password = HashHelper.ComputeSha256Hash("admin"),
                        RolesList = UserRole.ADMIN,
                        TelephoneNumber = "000000000",
                        Surname = "Admin",
                        Patronymic = "Admin",
                        DOB = DateTime.MinValue
                    };
                    dbContext.Users.Add(userAdmin);
                    dbContext.SaveChanges();
                }
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
                    builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
            });

            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FireSaverDbConnectionString")));

            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthUserService, UserService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IUserHelper, UserService>();
            services.AddScoped<IBuildingService, BuildingService>();

            services.AddScoped<ICompartmentService<FloorDto, Floor>, FloorService>();
            services.AddScoped<ICompartmentService<RoomDto, Room>, RoomService>();

            services.AddScoped<ICompartmentHelper, CompartmentHelper>();
            services.AddScoped<IPlanImageUploadService, PlanImageUploadService>();

            services.AddScoped<IEvacuationService, EvacuationService>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PointProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new BuildingProfile());
                mc.AddProfile(new CompartmentProfile());
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
            });
        }
    }
}
