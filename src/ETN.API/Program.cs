using ETN.Infrastructure;
using ETN.Infrastructure.Contracts;
using ETN.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace ETN.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddOpenApi()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddLogging()
            .AddScoped<IDataService, DataService>()
            .AddDbContextFactory<EtnDbContext>(options => { options.UseInMemoryDatabase("ETN_DB"); });

        builder.Services.AddControllers();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapSwagger();

        app
            .UseHttpsRedirection()
            .UseSwagger()
            .UseSwaggerUI();

        app.MapControllers();

        app.Run();
    }
}
