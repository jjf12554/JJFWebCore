using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using JJFWebCore.Middleware;
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
            services.AddControllersWithViews().AddControllersAsServices(); //������һ������autofac����Ȼ����

            //�����⣬�������У���֪���������ò���
            //services.AddAutofac((container) =>
            //{
            //    var controllerBaseType = typeof(Microsoft.AspNetCore.Mvc.ControllerBase);
            //    container.RegisterAssemblyTypes(typeof(Program).Assembly)
            //        .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            //        .InstancePerLifetimeScope().PropertiesAutowired();

            //    string webService = "WebService";// ConfigurationManager.AppSettings["DllName"];
            //    Assembly asmwebService = Assembly.Load(webService);
            //    container.RegisterAssemblyTypes(asmwebService).InstancePerRequest().PropertiesAutowired();
            //    string webDAO = "WebDao";// ConfigurationManager.AppSettings["DllName"];
            //    Assembly asmwebDAO = Assembly.Load(webDAO);
            //    container.RegisterAssemblyTypes(asmwebDAO).InstancePerRequest().PropertiesAutowired();
            //});
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

            app.UseWhen(
                c => c.Request.Path.Value.Contains("upload"),
                _ => _.UseMiddleware<AuthorizeStaticFilesMiddleware>()); //���з���Ȩ�޿���
            app.UseStaticFiles();      //ʹ�þ�̬�ļ�

            app.UseCookiePolicy();���� //�� Cookie �й�

            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });


        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            //ע��controller�����service
            var controllerBaseType = typeof(Microsoft.AspNetCore.Mvc.ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .InstancePerLifetimeScope().PropertiesAutowired();

            // ��������ӷ���ע��
            string webService = "WebService";// ConfigurationManager.AppSettings["DllName"];
            Assembly asmwebService = Assembly.Load(webService);
            builder.RegisterAssemblyTypes(asmwebService).InstancePerLifetimeScope().PropertiesAutowired();
            string webDAO = "WebDao";// ConfigurationManager.AppSettings["DllName"];
            Assembly asmwebDAO = Assembly.Load(webDAO);
            builder.RegisterAssemblyTypes(asmwebDAO).InstancePerLifetimeScope().PropertiesAutowired();
        }
    }
}
