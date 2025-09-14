using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ToDoApp.Auth;
using ToDoApp.DTOs.Auth;

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
            setup.IncludeXmlComments(@"obj\Debug\net8.0\ToDoApp.xml");
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

        var jwtConfig = configuration.GetSection("Jwt").Get<JwtConfig>();
        if (jwtConfig is null)
            throw new Exception("Jwt settings not found in configuration!");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));
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
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("ElektrikleshdirebildiklerimizdensinizmiElektrikleshdirebildiklerimizdensinizmi"))
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
                });
                options.AddPolicy("CanCreate", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    //policy.RequireClaim("CanTest");
                    policy.Requirements.Add(new CanCreateRequirment());
                });
                options.AddPolicy("SomeRequirment", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    //policy.RequireClaim("CanTest");
                    policy.Requirements.Add(new CanTestRequirment());
                    policy.Requirements.Add(new CanCreateRequirment());
                });
            });

        services.AddSingleton<IAuthorizationHandler, CanTestRequirment>();
        services.AddSingleton<IAuthorizationHandler, CanCreateRequirment>();
        return services;
    }


}
