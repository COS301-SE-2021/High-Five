using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using src.Storage;
using src.Subsystems.Analysis;
using src.Subsystems.MediaStorage;
using src.Subsystems.Pipelines;

namespace tests.IntegrationTests
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            
            services.AddMvc().AddApplicationPart(typeof(Org.OpenAPITools.Controllers.TestApiController).Assembly)
                .AddApplicationPart(typeof(Org.OpenAPITools.Controllers.MediaStorageApiController).Assembly)
                .AddApplicationPart(typeof(Org.OpenAPITools.Controllers.PipelinesApiController).Assembly)
                .AddApplicationPart(typeof(Org.OpenAPITools.Controllers.AnalysisApiController).Assembly);

            // Configuring of Azure AD B2C Authentication
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority = $"https://highfiveactivedirectory.b2clogin.com/tfp/{Configuration["AzureAdB2C:Tenant"]}/{Configuration["AzureAdB2C:SignUpSignInPolicyId"]}/v2.0/";
                    jwtOptions.Audience = Configuration["AzureAdB2C:ClientId"];
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

            // Dependency Injections
            services.Add(new ServiceDescriptor(typeof(IStorageManager), new MockStorageManager()));//singleton
            services.AddScoped<IMediaStorageService, MediaStorageService>();
            services.AddScoped<IPipelineService, PipelineService>();
            services.AddScoped<IAnalysisService, AnalysisService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseAuthentication();
            app.UseAuthorization();
            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });*/
            
            //Bypass authentication for testing purposes
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().WithMetadata(new AllowAnonymousAttribute());
            });
        }
    }
}