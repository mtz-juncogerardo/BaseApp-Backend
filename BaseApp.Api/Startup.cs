using System.Text;
using AspNetCoreRateLimit;
using BaseApp.Data.DataAccess;
using BaseApp.InjectionServices;
using BaseApp.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BaseApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private ConfigurationService Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationService(configuration);
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            InjectServices(services);
            AddAuthentication(services);
            ConfigureIpRateLimit(services);
            services.AddControllers();
            services.AddOptions();
            services.AddDbContext<AuthenticationContext>(options => options.UseNpgsql(Configuration.DbConnectionString,
                r => r.MigrationsAssembly("BaseApp.Data")));
            services.AddCors(c =>
                {
                    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                });
            services.AddMvc()
                .AddNewtonsoftJson(
                    options =>
                        {
                            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        });
        }

        private void ConfigureIpRateLimit(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(_configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(_configuration.GetSection("IpRateLimitPolicies"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(r =>
                {
                    r.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    r.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.JwtSecret)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = "https://mtzjunco.com",
                        ValidAudience = "https://mtzjunco.com",
                        RoleClaimType = "UserRole"
                    };
                });
        }

        private void InjectServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddSingleton<IConfigurationService>(Configuration);
            services.AddScoped<IAuditService, AuditFieldsService>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIpRateLimiting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}