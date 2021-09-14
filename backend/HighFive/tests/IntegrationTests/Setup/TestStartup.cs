using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
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
using src.AnalysisTools.VideoDecoder;
using src.Storage;
using src.Subsystems.Admin;
using src.Subsystems.Analysis;
using src.Subsystems.FileDownloads;
using src.Subsystems.Livestreaming;
using src.Subsystems.MediaStorage;
using src.Subsystems.Pipelines;
using src.Subsystems.Tools;
using src.Subsystems.User;

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
                .AddApplicationPart(typeof(Org.OpenAPITools.Controllers.AnalysisApiController).Assembly)
                .AddApplicationPart(typeof(Org.OpenAPITools.Controllers.UserApiController).Assembly)
                .AddApplicationPart(typeof(Org.OpenAPITools.Controllers.LivestreamApiController).Assembly)
                .AddApplicationPart(typeof(Org.OpenAPITools.Controllers.ToolsApiController).Assembly)
                .AddApplicationPart(typeof(Org.OpenAPITools.Controllers.DownloadsApiController).Assembly);

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
                        },
                        OnTokenValidated = async ctx =>
                        {
                            var adminValidator = ctx.HttpContext.RequestServices.GetRequiredService<IAdminValidator>();
                            var userId = ((JwtSecurityToken)ctx.SecurityToken).Subject;
                            if (adminValidator.IsAdmin(userId))
                            {
                                var claims = new List<Claim>
                                {
                                    new Claim("Admin", "true")
                                };
                                var appIdentity = new ClaimsIdentity(claims);
                                ctx.Principal.AddIdentity(appIdentity);
                            }
                        }
                    };
                });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
            });
            
            //singletons
            var mockAdminValidator = new MockAdminValidator();
            services.Add(new ServiceDescriptor(typeof(IAdminValidator), mockAdminValidator));
            services.Add(new ServiceDescriptor(typeof(IStorageManager), new MockStorageManager(mockAdminValidator)));//singleton
            // Dependency Injections
            services.AddScoped<IMediaStorageService, MediaStorageService>();
            services.AddScoped<IPipelineService, PipelineService>();
            services.AddScoped<IAnalysisService, AnalysisService>();
            services.AddScoped<IVideoDecoder, MockVideoDecoder>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IToolService, ToolService>();
            services.AddScoped<IDownloadsService, DownloadsService>();
            services.AddScoped<ILivestreamingService, LivestreamingService>();
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