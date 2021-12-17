using AuthenticationService.Extensions;
using AuthenticationService.Helper;
using AuthenticationService.Model;
using AuthenticationService.Stores;
using CSRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace AuthenticationService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration _configuration { get; }
        public IWebHostEnvironment  _iwebHostEnvironment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment  webHostEnvironment)
        {
            _configuration = configuration;
            _iwebHostEnvironment = webHostEnvironment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
           
            
            services.AddMvc();
            services.AddSingleton(_configuration as IConfigurationRoot);
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            #region 配置策略
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });
            //登录失败五次后禁用5分钟
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            #endregion

            #region Identity配置
            services.AddDbContextPool<ApplicationDbContext>(options => {
                options.UseSqlServer(connectionString);
#if DEBUG
                options.UseLoggerFactory(new EFLoggerFactory());
               // options.EnableSqlRecorder();
#endif
            }
               );
            #endregion

            #region 认证配置

            var assembly = Assembly.GetExecutingAssembly();
            var migrationsAssembly = assembly.GetName().Name;
            var filePath = Path.Combine(AppContext.BaseDirectory, "idsrv4.pfx");
            var x509Cert = new X509Certificate2(filePath, _configuration["CertPassword"]);
            var credential = new SigningCredentials(new X509SecurityKey(x509Cert), "RS256");
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.IssuerUri = _configuration["IssUrl"];
              
            })
                .AddDeveloperSigningCredential()//开发环境使用的证书
                //添加证书
                 //.AddSigningCredential(x509Cert)
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
               .AddExtensionGrantValidator<AdminAuthGrantValidator>()//管理员登录
               .AddExtensionGrantValidator<PhoneOneClickLoginGrantValidator>()//手机一键登录
                .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()//普通密码登录
            .AddProfileService<CustomProfileService>();//必须放后面 获取用户信息

            IdentityModelEventSource.ShowPII = true;
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = _configuration["AuthenticationUrl"];
                    options.RequireHttpsMetadata = false;
                    //options.Audience = _configuration["Audience"];
                    options.TokenValidationParameters.ValidAudiences = new List<string> { "OnlineSchool_Api", "Wisdom_ClassRoom_Api" };
                });

            #endregion
            services.AddUserDefined();
            var section = _configuration.GetSection("Redis:Default");
            string redisConnectionString = $@"{section["Connection"]},password={section["Password"]}";
            //初始化 RedisHelper
            RedisHelper.Initialization(new CSRedisClient(redisConnectionString + $",defaultDatabase ={ section["DefaultDB"] }"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (_iwebHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseRouting();
            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("CorsPolicy");
            });
     
        }
    }
}
