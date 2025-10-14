using UrlShortener.Infrastructure;
using UrlShortener.Services.Interfaces;
using UrlShortener.Services;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog((services, ls) => ls
            .ReadFrom.Services(services)
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console());

    SqlConnectionStringBuilder sqlConnectionStringBuilder =
        new(builder.Configuration.GetConnectionString("UrlShortenerContext"))
        {
            Password = builder.Configuration["UrlShortenerContext:Password"]
        };

    builder.Services.AddDbContext<UrlShortenerContext>(opts =>
            opts.UseSqlServer(sqlConnectionStringBuilder.ConnectionString));

    builder.Services.AddScoped<IUrlShortenerRepository, UrlShortenerRepository>();
    builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();

    builder.Services.AddRazorPages();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();
    app.MapRazorPages()
       .WithStaticAssets();

    app.Run();

}
catch (Exception ex)
{
    Log.Error(ex, "Terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}