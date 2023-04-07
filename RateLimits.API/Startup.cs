using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateLimits.API
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
            services.AddOptions();//appsettings içindeki key value deðerlerini , bir class üzerinden okuma iþlemi gerçekleþtirebilecek
            services.AddMemoryCache();//bu requestleri host sistemin rem'inde tutmasý için ekliyoruz, sayesinde sayaçlarýmýzý ayarlayacaðýz.
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));//appsetting deki key valuyi okuyacak
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicy"));//limit ayarlarýný belirleyeceðiz
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>(); //api adresindeki datalarý ve policy içindeki datalarý tutacaðý memory cache yi belirteceðiz.Yani IIPolicyStore ile karþýlaþýrsa MemC ver diyoruz.
            //services.AddSingleton<IIpPolicyStore, DistributedCacheClientPolicyStore>(); //Distrubuted versiyon için kullanýlýr
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>(); //IRateLimitCounterStore ile kaç request yapýldýðýnýn datasýný okuyor
            //RateLimit kütüphanesi gelen requesti okuduðundan dolayý , yani bir middleware olarak uygulamamýza bir katman ekledðinden dolayý,ilk olarak RateLimit'in belirlemiþ olduðu katmana gelecek.Bu katman içinden rquestin headerýný okuyabilmesi için aþaðýdaki servisi ekliyoruz :
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();//yeni versiyonda gömülü deðilmiþ


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RateLimits.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RateLimits.API v1"));
            }

            app.UseIpRateLimiting();

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
