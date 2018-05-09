using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BetterMeApi.Models;
using BetterMeApi.Repositories;

namespace BetterMeApi
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private Dictionary<string, RsaSecurityKey> googleSigningKeys;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            googleSigningKeys = GetGoogleSigningKeys().Result;

            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        static async Task<String> GetGoogleJWKSUri()
        {
            string discoveryUri = "https://accounts.google.com/.well-known/openid-configuration";
            
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(discoveryUri))
                {
                    using (var content = response.Content)
                    {
                        var data = await content.ReadAsStringAsync();

                        return data != null ? (string)JToken.Parse(data)["jwks_uri"] : null;
                    }
                }
            }
        }

        static async Task<JToken> GetGoogleJWKS(string jwksUri)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(jwksUri))
                {
                    using (var content = response.Content)
                    {
                        var data = await content.ReadAsStringAsync();
                        
                        return data != null ? JToken.Parse(data)["keys"] : null;
                    }
                }
            }
        }


        static async Task<Dictionary<string, RsaSecurityKey>> GetGoogleSigningKeys()
        {
            var jwksUri = await GetGoogleJWKSUri();
        
            var jwks = await GetGoogleJWKS(jwksUri);
            
            return jwks
                .Select(key => new
                {
                    kid = (string)key["kid"],
                    n = WebEncoders.Base64UrlDecode((string)key["n"]),
                    e = WebEncoders.Base64UrlDecode((string)key["e"])
                })
                .ToDictionary(key => key.kid, key => new RsaSecurityKey(new RSAParameters
                {
                    Exponent = key.e,
                    Modulus = key.n
                }));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) =>
                    {
                        return new List<SecurityKey> { googleSigningKeys[kid] };
                    }
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                .Build());
            });

            services.AddDbContext<BetterMeContext>(options =>
            {
                //options.UseMySql(Configuration.GetConnectionString("BetterMeDatabase"));
                options.UseInMemoryDatabase("BetterMeDatabase");
            });
            
            services.AddSingleton<SortedSet<string>, SortedSet<string>>();
            
            services.AddScoped<UserRepository, UserRepository>();
            services.AddScoped<GoalRepository, GoalRepository>();
            services.AddScoped<AchievementRepository, AchievementRepository>();
            services.AddScoped<AssignmentRepository, AssignmentRepository>();
            services.AddScoped<ProgressRepository, ProgressRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, BetterMeContext dbContext)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors("CorsPolicy");
            app.UseAuthentication();

            //dbContext.Database.Migrate();
            //dbContext.Database.EnsureCreated();

            app.UseMiddleware<RegistrationMiddleware>();

            app.UseMvc();
        }
    }
}
