using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Cubo.Api.Repositories;
using Cubo.Api.Settings;
using Cubo.Core.EF;
//using Cubo.Core.EF;
using Cubo.Core.Mappers;
using Cubo.Core.Repositories;
using Cubo.Core.Services;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cubo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
           services.AddMemoryCache();
            services.AddScoped<IUserRepository, UserRepository>();
            services.Configure<SqlSettings>(Configuration.GetSection("ConnectionStrings"));
            services.AddSingleton<IBucketRepository, InMemoryBucketRepository>();
            services.AddScoped<IBucketService, BucketService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddSingleton<IMapper>(_ => AutoMapperConfig.GetMapper());
           services.AddScoped<IDataInitializer, DataInitializer>();
            services.AddEntityFrameworkSqlServer()
               .AddDbContext<CuboContext>();
           


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cubo.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cubo.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
