using Api.EFModels;
using Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Api.Middlewares;
using Microsoft.Extensions.Caching.Memory;

namespace Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
      });

      //TODO: Get data from secure place.
      var dbUser = "completeHere";
      var dbPass = "completeHere";
      var dbName = "completeHere";

      var dbConnectionString = Configuration.GetConnectionString("dbConnection");
      dbConnectionString = dbConnectionString.Replace("{dbUser}", dbUser);
      dbConnectionString = dbConnectionString.Replace("{dbPass}", dbPass);
      dbConnectionString = dbConnectionString.Replace("{dbName}", dbName);

      services.AddDbContext<MyDbContext>(opt =>
      {
        opt.UseMySQL(dbConnectionString);
      });

      services.AddHttpClient();
      services.AddSingleton<IMemoryCache, MemoryCache>();
      services.AddSingleton<IntegrationExternal>();

      services.AddScoped<IUserRepository, UserRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
      }

      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseToken();

      app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
