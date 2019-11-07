using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JJFWebCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // ʹ�ô˷�����������ӷ���
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ʹ�ô˷�������HTTP����ܵ�
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //�Ƿ񿪷�����
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error"); /���ִ�����ת�� /Error ҳ��
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection(); //�� HTTP �ض��� HTTPS

            //app.UseWhen(
            //    c => c.Request.Path.Value.Contains("assets"),
            //    _ => _.UseMiddleware<AuthorizeStaticFilesMiddleware>()); //���з���Ȩ�޿���
            app.UseStaticFiles();      //ʹ�þ�̬�ļ�

            app.UseCookiePolicy();���� //�� Cookie �й�

            app.UseDeveloperExceptionPage();

            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
