using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using src.AnalysisTools.VideoDecoder;
using src.Storage;
using src.Subsystems.Analysis;
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
            
            // Configuring of Azure AD B2C Authentication
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority = $"https://highfiveactivedirectory.b2clogin.com/tfp/{Configuration["AzureAdB2C:Tenant"]}/{Configuration["AzureAdB2C:SignUpSignInPolicyId"]}/v2.0/";
                    jwtOptions.Audience = Configuration["AzureAdB2C:ClientId"];
                    jwtOptions.SaveToken = true;
                    jwtOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = async c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 401;
                            c.Response.ContentType = "application/json";
                            await c.Response.WriteAsync("{\"error\":\"Invalid token provided. (Developer's note, this error might also mean something else went wrong with the back-end)\"}");
                        }
                    };
                });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
            });
            
            // Singletons
            services.Add(new ServiceDescriptor(typeof(IConfiguration), Configuration));
            services.Add(new ServiceDescriptor(typeof(IAnalysisModels), new AnalysisModels()));
            // Dependency Injections
            services.AddScoped<IStorageManager, StorageManager>();
            services.AddScoped<IMediaStorageService, MediaStorageService>();
            services.AddScoped<IPipelineService, PipelineService>();
            services.AddScoped<IAnalysisService, AnalysisService>();
            services.AddScoped<IVideoDecoder, VideoDecoder>();
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
                    c.RoutePrefix = string.Empty;
                });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowOrigin");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}