using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiParquimetros.Contexts;
using WebApiParquimetros.Models;
using System.Reflection;
using WebApiParquimetros.Services;
using IEmailSender = WebApiParquimetros.Services.IEmailSender;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;
using Cronos;

namespace WebApiParquimetros
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCronJob<CronJob1MultaAutomatica>(c =>
            //{
            //    //
            //    c.TimeZoneInfo = TimeZoneInfo.Local;
            //    //c.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City"); ;
            //    c.CronExpression = @"* * * * * MON-SAT";
            //});
            services.AddCronJob<CronJob2MultaDP10>(c =>
            {
             c.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City"); ;
            //     c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @" 0 22  *  * MON-SAT";
            });
            services.AddCronJob<CronJob3ResumenDiario>(c =>
            {
              c.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City"); ;
           //   c.TimeZoneInfo = TimeZoneInfo.Local;
              // c.CronExpression = @" 0 23  *  * MON-SAT";
                c.CronExpression = @" 0 23  *  * MON-SAT";
                // c.CronExpression = @" 9 11  *  * MON-SAT";
            });

            services.AddCronJob<CronJob4ResumenSemanal>(c =>
            {
             c.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City"); ;
             //  c.TimeZoneInfo = TimeZoneInfo.Local;
                // c.CronExpression = @" 30 23  *  * SAT";
                // c.CronExpression = @" 30 23  *  * SAT";
                c.CronExpression = @" 30 23  *  * 6";
            });

            services.AddCronJob<CronJob5ResumenMensual>(c =>

            {
                c.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City"); ;
                //c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @" 35 23  L  * ? ";
                //c.CronExpression = @" 47 11  *  * *";
            });


            services.AddSingleton<IJobFactory, CustomQuartzJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<MultaJob>();
          //  services.AddSingleton<ResumenMensualJob>();
            //Aqui se debe cambiar a cada segundo cuando ya esté en produccion
            //Linea funcional falta modificar
            // services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(MultaJob), "Multa Automatica", " 0 5/1 8-22 ? *  MON-SAT", TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City")));
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(MultaJob), "Multa Automatica", " 0 1 8-22 ? *  MON-SAT", TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City")));
            services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(MultaJob), "Multa Automatica", " 0 1 8-22 ? *  MON-SAT", TimeZoneInfo.Local));
           // services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(ResumenMensualJob), "Resumen Mensual", " 0 35 23  L  * ? ", TimeZoneInfo.Local));
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(MultaJob), "Multa Automatica", " 0 5/1 8-22 ? *  MON-SAT"));
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(MultaDP10Job), "Multa Despues de las 10", "59 * * * * ?"));
            services.AddHostedService<QuartzMultaHostedService>();
           // services.AddHostedService<QuartzResMensualHostedService>();
            //services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, MultaHostedService>();


            services.AddCors();


            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseNpgsql(Configuration.GetConnectionString("DefaultConnectionString")));
            //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnectionString"), opts => opts.EnableRetryOnFailure()));



            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })

               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = false,
                 ValidateAudience = false,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                 ClockSkew = TimeSpan.Zero
             });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOptions();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Parquimetros", Version = "V1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);

            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config => {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Parquimetros");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors(builder => builder.WithOrigins("*").WithMethods("*").WithHeaders("*"));
            app.UseMvc();
        }
    }
}
