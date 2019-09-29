using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VsLiveSanDiego.Services;

namespace VsLiveSanDiego
{
    public class Startup
    {
        private const string CORS_POLICY = "default";
        private const string HUB_ROUTE = "/hubs/hello";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // setup cors so javascript clients can reach back in
            services.AddCors(options =>
            {
                options.AddPolicy(CORS_POLICY,
                builder =>
                {
                    builder.WithOrigins("http://localhost:5000",
                                        "https://localhost:5001")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });

            services.AddRazorPages();
            services.AddControllers();

            // -------------------------------------------
            // SHOW: we need to add the SignalR middleware
            services.AddSignalR();
            // -------------------------------------------

            // wire up the IUserIdProvider we'll use for this app
            services.AddSingleton<IUserIdProvider, UsernameUserIdProvider>();

            // wire up the service that will hand back JWT tokens for us
            services.AddSingleton<JwtBearerTokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(CORS_POLICY);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();

                // -------------------------------------------
                // SHOW: we need to setup routes for each hub
                endpoints.MapHub<HelloHub>(HUB_ROUTE);
                // -------------------------------------------
            });
        }

        private void WireUpAuth(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration["AuthKey"]);
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments(HUB_ROUTE)))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
