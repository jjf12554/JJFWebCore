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
        // 使用此方法向容器添加服务
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddControllersAsServices(); //后面这一段用于autofac，不然不行

            //有问题，这样不行，不知道哪里配置不对
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
        // 使用此方法配置HTTP请求管道
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //是否开发环境
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error"); /出现错误跳转到 /Error 页面
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection(); //把 HTTP 重定向到 HTTPS

            app.UseWhen(
                c => c.Request.Path.Value.Contains("upload"),
                _ => _.UseMiddleware<AuthorizeStaticFilesMiddleware>()); //进行访问权限控制
            app.UseStaticFiles();      //使用静态文件

            app.UseCookiePolicy();　　 //与 Cookie 有关

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
            //注册controller里面的service
            var controllerBaseType = typeof(Microsoft.AspNetCore.Mvc.ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .InstancePerLifetimeScope().PropertiesAutowired();

            // 在这里添加服务注册
            string webService = "WebService";// ConfigurationManager.AppSettings["DllName"];
            Assembly asmwebService = Assembly.Load(webService);
            builder.RegisterAssemblyTypes(asmwebService).InstancePerLifetimeScope().PropertiesAutowired();
            string webDAO = "WebDao";// ConfigurationManager.AppSettings["DllName"];
            Assembly asmwebDAO = Assembly.Load(webDAO);
            builder.RegisterAssemblyTypes(asmwebDAO).InstancePerLifetimeScope().PropertiesAutowired();
        }
    }
}
