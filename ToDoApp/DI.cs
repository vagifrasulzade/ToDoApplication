using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ToDoApp.Auth;
using ToDoApp.Data;
using ToDoApp.DTOs.Auth;
using ToDoApp.Models;
using ToDoApp.Providers;
using ToDoApp.Services.Auth;

namespace ToDoApp;

public  static class DI
{
    //extension method
    // AddSwagger
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            setup =>
            {
            setup.SwaggerDoc(
                "v1",
                new OpenApiInfo()
                {
                    Title = "ToDo",
                    Version = "v 2.0",
                });
           
                var filePath = Path.Combine(AppContext.BaseDirectory, "Documentation.xml");
                setup.IncludeXmlComments(filePath);
            
            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",

            });
            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {

                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }, new string[]{ }

                }
            });
            }
        );
        return services;

    }


 



    public static IServiceCollection AuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        //Authentication
        services.AddIdentity<AppUser, IdentityRole>(setup =>
        {
            setup.Password.RequiredLength = 8;
            setup.Password.RequireDigit = false;
            setup.Password.RequireLowercase = false;
            setup.Password.RequireUppercase = false;
            setup.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<AppDbContext>();
        
        
        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IRequestUserProvider, RequestUserProvider>();

        var jwtConfig = new JwtConfig();
        configuration.Bind("JWT", jwtConfig);
        services.AddSingleton(jwtConfig);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-secure-and-long-secret-key-of-32-bytes-or-more"))
                };
            });

        //Authorization
        services.AddAuthorization(
            options =>
            {
                options.AddPolicy("CanTest", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    //policy.RequireClaim("CanTest");
                    policy.Requirements.Add(new CanTestRequirment());
                    policy.Requirements.Add(new CanCreateRequirment());

                });
               
            });

       
        return services;
    }


}
