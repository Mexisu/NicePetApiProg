using System;
using System.Linq;
using System.Text;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Nicepet_API.Models;
using Nicepet_API.Helpers;
using Wkhtmltopdf.NetCore;

namespace Nicepet_API
{
    public class Startup
    {
         public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object options { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EmailAppSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();

            //PDF
            //services.AddWkhtmltopdf("wkhtmltopdf");

            var tokenKey = Configuration.GetValue<string>("TokenKey") ;
            var key = Encoding.ASCII.GetBytes(tokenKey);
            services.AddScoped<PathFilter>();
            //---------------------------------------------Nice Pet Data Base---------------------------------------------
            services.AddDbContext<ApiNicepetContext>(item => item.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //----------------------------------------------Local Data Base-----------------------------------------------
            //services.AddDbContext<ApiNicepetContext>(item => 
            //item.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=NicepetLocalDataBase;integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
            
            services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);
            services.AddOData();
            services.AddSignalR();

            //services.AddCors(options => options.AddPolicy("MyPolicy", ApplicationBuilder => {
            //    ApplicationBuilder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //}));

            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                    builder =>
                    {
                        builder.WithOrigins(
                                "https://localhost:5001",
                                "http://localhost:8080",
                                "http://localhost:8081",
                                "https://localhost:44307",
                                "https://localhost:5000",
                                "https://preprod-spa.nicepet.fr"
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            services.AddSingleton<ITokenRefresher>(x =>new TokenRefresher(key, x.GetService<IJWTAuthenticationManager>()));
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddSingleton<IJWTAuthenticationManager>(x =>new JWTAuthenticationManager(tokenKey, x.GetService<IRefreshTokenGenerator>()));
            services.AddMvc(options => options.OutputFormatters.Add(new Helpers.HtmlOutputFormatter()));
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
                app.UseHsts();
            }
            //Warning:UseCors("MyPolicy") => Http request by browser "URL" 
            app.UseCors("MyPolicy");
          


            app.UseRouting();

            //Warning:UseHttpsRedirection() => Never Activate
            //UseHttpsRedirection():

            //warning:UseStaticFiles() => Userd in FileUploadingController
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.EnableDependencyInjection();
                routeBuilder.Select().Filter().Expand();
                routeBuilder.MapODataServiceRoute("OData", "odata", GetEdmModel());
            });

            //Rotativa PDF
            //Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/signalr");
            });
        }
        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();

            //------------------------------------------User---------------------------------------
            builder.EntitySet<User>("User").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<UserProfile>("UserProfile").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<UserType>("UserType").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<UserAddress>("UserAddress").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            //------------------------------------------Animal---------------------------------------
            builder.EntitySet<Animal>("Animal").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<AnimalProfile>("AnimalProfile").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<AnimalSpecies>("AnimalSpecies").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<AnimalBreed>("AnimalBreed").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<AnimalHairType>("AnimalHairType").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<HistoricalOwner>("HistoricalOwner").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            //------------------------------------------Announcement---------------------------------------
            builder.EntitySet<Announcement>("Announcement").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<AnnouncementType>("AnnouncementType").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            //------------------------------------------Breeding---------------------------------------
            builder.EntitySet<BreedingProfile>("BreedingProfile").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<BreedingProfile>("BreedingType").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<FranceBreeding>("FranceBreeding").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            //---------------------------------------------Message--------------------------------------------------------
            builder.EntitySet<Message>("Message").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            builder.EntitySet<Contacts>("Contacts").EntityType.Filter().Count().Expand(8).OrderBy().Page().Select();
            return builder.GetEdmModel();
        }
    }
}
