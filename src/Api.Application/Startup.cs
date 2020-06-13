using System;
using System.Collections.Generic;
using System.Linq;
using Api.CrossCutting.DependencyInjection;
using Api.CrossCutting.Mappings;
using Api.Domain.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Application {
  public class Startup {
    public Startup (IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices (IServiceCollection services) {
      ConfigureService.ConfigureDependenciesService (services);
      ConfigureRepository.ConfigureDependenciesRepository (services);

      var config = new AutoMapper.MapperConfiguration(cfg => {
        cfg.AddProfile(new DtoToModelProfile());
        cfg.AddProfile(new EntityToDtoProfile());
        cfg.AddProfile(new ModelToEntityProfile());
      });

      IMapper mapper = config.CreateMapper();
      services.AddSingleton(mapper);

      var signingConfigurations = new SigningConfigurations ();
      services.AddSingleton (signingConfigurations);

      var tokenConfigurations = new TokenConfigurations ();
      new ConfigureFromConfigurationOptions<TokenConfigurations> (
          Configuration.GetSection ("TokenConfigurations"))
        .Configure (tokenConfigurations);
      services.AddSingleton (tokenConfigurations);

      services.AddAuthentication (authOptions => {
        authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer (bearerOptions => {
        var paramsValidation = bearerOptions.TokenValidationParameters;
        paramsValidation.IssuerSigningKey = signingConfigurations.Key;
        paramsValidation.ValidAudience = tokenConfigurations.Audience;
        paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
        paramsValidation.ValidateIssuerSigningKey = true;
        paramsValidation.ValidateLifetime = true;
        paramsValidation.ClockSkew = TimeSpan.Zero;
      });

      services.AddAuthorization (auth => {
        auth.AddPolicy ("Bearer", new AuthorizationPolicyBuilder ()
          .AddAuthenticationSchemes (JwtBearerDefaults.AuthenticationScheme)
          .RequireAuthenticatedUser ().Build ());
      });

      services.AddSwaggerGen (c => {
        c.SwaggerDoc ("v1", new OpenApiInfo {
          Version = "v1",
            Title = "API Application",
            Description = "API REST with AspNetCore 3.1",
            TermsOfService = new Uri ("https://github.com/rabbittrix/Readme.md"),
            Contact = new OpenApiContact {
              Name = "Roberto de Souza",
                Url = new Uri ("https://github.com/rabbittrix"),
                Email = "rabbittrix@hotmail.com"
            },
            License = new OpenApiLicense {
              Name = "Use under JRSF",
                Url = new Uri ("https://example.com/license"),
            }
        });
        c.AddSecurityDefinition ("Bearer", new OpenApiSecurityScheme {
          In = ParameterLocation.Header,
            Description = "Enter with a token JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        c.AddSecurityRequirement (new OpenApiSecurityRequirement {
          {
            new OpenApiSecurityScheme {
              Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            },
            new List<string> ()
          }
        });
      });

      services.AddControllers ();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
      }

      //Ativate middleware Swagger
      app.UseSwagger ();
      app.UseSwaggerUI (c => {
        c.RoutePrefix = string.Empty;
        c.SwaggerEndpoint ("/swagger/v1/swagger.json",
          "API Application ASP.NET Core 3.1");
      });

      var option = new RewriteOptions ();
      option.AddRedirect ("^$", "swagger");
      app.UseRewriter (option);

      app.UseRouting ();

      app.UseAuthorization ();

      app.UseEndpoints (endpoints => {
        endpoints.MapControllers ();
      });

    }
  }
}