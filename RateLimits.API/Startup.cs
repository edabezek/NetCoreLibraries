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
            services.AddOptions();//appsettings i�indeki key value de�erlerini , bir class �zerinden okuma i�lemi ger�ekle�tirebilecek
            services.AddMemoryCache();//bu requestleri host sistemin rem'inde tutmas� i�in ekliyoruz, sayesinde saya�lar�m�z� ayarlayaca��z.
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));//appsetting deki key valuyi okuyacak
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicy"));//limit ayarlar�n� belirleyece�iz
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>(); //api adresindeki datalar� ve policy i�indeki datalar� tutaca�� memory cache yi belirtece�iz.Yani IIPolicyStore ile kar��la��rsa MemC ver diyoruz.
            //services.AddSingleton<IIpPolicyStore, DistributedCacheClientPolicyStore>(); //Distrubuted versiyon i�in kullan�l�r
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>(); //IRateLimitCounterStore ile ka� request yap�ld���n�n datas�n� okuyor
            //RateLimit k�t�phanesi gelen requesti okudu�undan dolay� , yani bir middleware olarak uygulamam�za bir katman ekled�inden dolay�,ilk olarak RateLimit'in belirlemi� oldu�u katmana gelecek.Bu katman i�inden rquestin header�n� okuyabilmesi i�in a�a��daki servisi ekliyoruz :
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();//yeni versiyonda g�m�l� de�ilmi�


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
