using GraphEditor;
using GraphEditor.Hubs;
using GraphEditor.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

if (configuration.GetValue<bool>("ENVIRONMENT_HEROKU"))
{
    var port = configuration.GetValue<int>("PORT");
    builder.WebHost.UseUrls("http://*:" + port);
}

services.AddCors(o =>
{
    o.AddPolicy(Environments.Development, p =>
    {
        p.AllowAnyHeader()
         .AllowAnyMethod()
         .SetIsOriginAllowed(_ => true)
         .AllowCredentials();
    });
});

services.AddControllers();

services.AddDbContext<GraphDBContext>(
    options =>
    {
        var connectionString = "";
        if (configuration.GetValue<bool>("ENVIRONMENT_HEROKU"))
        {
            var connectionUrl = configuration.GetValue<string>("DATABASE_URL");
            connectionString = DbConnectionStringConverter.FromUrlToKeyValue(connectionUrl);
        }
        else
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        options.UseNpgsql(connectionString);
    });

services.AddTransient<IGraphRepository, GraphRepository>();

services.AddSignalR().AddJsonProtocol();
// TODO switch back to AddSignalRCore
// AddSignalRCore does not provide endpoints.MapHub

services.AddSwaggerGen();

services.AddSpaStaticFiles(c =>
{
    c.RootPath = "react-app";
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseCors(Environments.Development);
else
    app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    if (app.Environment.IsDevelopment())
    {
        endpoints.MapGet("/test", async context =>
        {
            await context.Response.WriteAsync(File.ReadAllText("TestUi.html"));
            await context.Response.CompleteAsync();
        });
    }

    endpoints.MapHub<GraphHub>("/api/signalr/graph");
});


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GraphDBContext>();
    db.Database.Migrate();
}

app.UseSpaStaticFiles();
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "react-app";
});

app.MapControllers();
app.Run();