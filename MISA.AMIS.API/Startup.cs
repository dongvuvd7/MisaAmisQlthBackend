using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MISA.AMIS.API.MiddleWare;
using MISA.AMIS.CORE.Exceptions;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MISA.AMIS.CORE.Interfaces.Services;
using MISA.AMIS.CORE.Services;
using MISA.AMIS.INFRASTRUCTURE.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MISA.AMIS.API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MISA.AMIS.API", Version = "v1" });
            });
            services.AddCors(options => options.AddPolicy("MyPolicy", builder =>
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                )
            );
            //Tiêm vào start-up để xác định xem cái nào làm việc với cái nào
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeServices, EmployeeServices>();
            services.AddScoped<INccRepository, NccRepository>();
            services.AddScoped<INccServices, NccServices>();
            services.AddScoped<IBankNccRepository, BankNccRepository>();
            services.AddScoped<IBankNccServices, BankNccServices>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseServices<>), typeof(BaseServices<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MISA.AMIS.API v1"));
            }
            //Xử lý global exception 
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            //Xử lý exception
            app.UseExceptionHandler(c => c.Run(async context =>
            {

                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                if (exception is EmployeeException)
                {

                    var response = new
                    {
                        devMsg = exception.Message,
                        userMsg = Properties.Resources.User_msg,
                        MISACode = Properties.Resources.MISACode,
                        Data = "CustomerCode",
                    };
                    var result = JsonConvert.SerializeObject(response);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    //var jsonObject = JsonConvert.SerializeObject(My Custom Model);
                    await context.Response.WriteAsJsonAsync(response);
                }
                else
                {
                    var response = new
                    {
                        devMsg = exception.Message,
                        userMsg = Properties.Resources.User_msg,
                        MISACode = Properties.Resources.MISACode,
                        Data = exception
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }));
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("MyPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
