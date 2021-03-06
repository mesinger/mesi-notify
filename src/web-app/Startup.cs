using Mesi.Notify.ApplicationLayer.Executions;
using Mesi.Notify.ApplicationLayer.Visuals;
using Mesi.Notify.Core;
using Mesi.Notify.Infra;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using web_app.Infrastructure;

namespace web_app
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
            services.AddScoped<AccessTokenAuthenticationEvents>();
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.EventsType = typeof(AccessTokenAuthenticationEvents);
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://is4.raspi";
                    options.ClientId = "mesi-notify-dev";
                    options.ResponseType = "code";

                    options.ClientSecret = "secret";
                    options.SaveTokens = true;

                    options.SignedOutCallbackPath = "/logout-redirect";

                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.GetClaimsFromUserInfoEndpoint = true;
                });
            
            services.AddRazorPages(options => options.Conventions.AuthorizePage("/Command"));
            services.AddControllers();

            services.Configure<EmailOptions>(Configuration.GetSection("Email"));

            services.AddScoped<ICommandFactory, ReflectionCommandProvider>();
            services.AddScoped<ICommandRepository, ReflectionCommandProvider>();
            services.AddScoped<IFileSystem, FileSystem>();
            services.AddScoped<ICommandResponseSender, DefaultCommandResponseSender>();
            services.AddScoped<IEmailCommandResponseSender, EmailCommandResponseSender>();
            services.AddScoped<ISmtpClient, DefaultSmtpClient>();

            services.AddScoped<IGetAvailableCommandNames, VisualsApplicationService>();
            services.AddScoped<IGetCommandByName, VisualsApplicationService>();
            services.AddScoped<IExecuteCommand, ExecutionsApplicationService>();
            services.AddScoped<IExecuteCommandWithPropertiesAsJson, ExecutionsApplicationService>();
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
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}