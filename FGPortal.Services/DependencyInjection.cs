using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public static class DependencyInjection
    {
        public static void AddRepository(this IServiceCollection services,
         IConfiguration config)
        {

            //Donot forgot to add ConnectionStrings as "dbConnection" to the appsetting.json file
            var constring = config.GetConnectionString("Default");
            services.AddDbContext<AppDbContext>(opt => opt
                .UseSqlServer(constring));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IInternetUserRepository, InternetUserRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // services.AddSingleton<IAuthRepository>(
            //    new AuthRepository(
            //        config.GetSection() < string > ("JWTSecretKey"),
            //        config.GetValue<int>("JWTLifespan")
            //    )
            //);


            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = false;
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidAudience = config["Jwt:Audience"],
            //        ValidIssuer = config["Jwt:Issuer"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
            //    };
            //});

        }
    }
}
