using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using src.Storage;
using src.Subsystems.MediaStorage;
using src.Subsystems.Pipelines;

namespace src
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            // Dependency Injections
            services.Add(new ServiceDescriptor(typeof(IStorageManager), new StorageManager(Configuration)));//singleton
            services.AddScoped<IMediaStorageService, MediaStorageService>();
            services.AddScoped<IPipelineService, PipelineService>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "High Five");
                    c.RoutePrefix = String.Empty;
                });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowOrigin");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}