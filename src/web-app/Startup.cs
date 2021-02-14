using Mesi.Notify.ApplicationLayer.Executions;
using Mesi.Notify.ApplicationLayer.Visuals;
using Mesi.Notify.Core;
using Mesi.Notify.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddRazorPages();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}