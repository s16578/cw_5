using cw_5.DTOs.Responses;
using cw_5.Middleware;
using cw_5.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace cw_5
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
            services.AddTransient<IStudentService, SqlServerStudentService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentService service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseMiddleware<ExceptionMiddleware>();

            app.UseMiddleware<LoggingMiddleware>();

            app.Use(async (contex, next) =>
            {
                if (!contex.Request.Headers.ContainsKey("Index"))
                {
                    contex.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await contex.Response.WriteAsync("There is missing index in header");
                    return;
                }
                string index = contex.Request.Headers["Index"].ToString();

                IndexStudentResponse response = service.GetStudent(index);

                await next();

            }
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}