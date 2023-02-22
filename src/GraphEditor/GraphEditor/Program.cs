using GraphEditor;
using GraphEditor.Hubs;
using GraphEditor.Models;
using GraphEditor.Models.Auth.Handlers;
using GraphEditor.Models.Auth.User;
using GraphEditor.Models.CRUD;
using GraphEditor.Models.Graph;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

Thread.Sleep(5 * 1000);

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

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
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        options.UseNpgsql(connectionString);
    });

services.AddAuthorization(options =>
{
    options.AddPolicy(StringConstants.GraphCRUDPolicy, policy =>
        policy.Requirements.Add(new OperationAuthorizationRequirement()));
});

services.AddTransient<UserRecordValidator>();

services.AddTransient<IAuthorizationHandler, GraphAuthorizationCRUDHandler>();

services.AddTransient<IRepository<GraphRecord>, GraphRepository>();

services.AddTransient<IRepository<UserRecord>, UserRepository>();
services.AddTransient<IUserStore<UserRecord>, UserStore>();

services.AddSignalR().AddJsonProtocol();

services.AddIdentityCore<UserRecord>();

services.AddSwaggerGen();

services.AddSpaStaticFiles(c =>
{
    c.RootPath = "react-app";
});

services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = StringConstants.TokenAuthenticationDefaultScheme;

    // you can also skip this to make the challenge scheme handle the forbid as well
    options.DefaultForbidScheme = StringConstants.TokenAuthenticationDefaultScheme;

    // of course you also need to register that scheme, e.g. using
    options.AddScheme<TokenAuthentificationHandler>(StringConstants.TokenAuthenticationDefaultScheme, StringConstants.TokenAuthenticationDefaultScheme);
});

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
       {
           options.Events.OnRedirectToAccessDenied = ReplaceRedirector(HttpStatusCode.Forbidden, options.Events.OnRedirectToAccessDenied);
           options.Events.OnRedirectToLogin = ReplaceRedirector(HttpStatusCode.Unauthorized, options.Events.OnRedirectToLogin);
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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/isdev", async context =>
    {
        await context.Response.WriteAsync(app.Environment.IsDevelopment().ToString());
        await context.Response.CompleteAsync();
    });
    endpoints.MapHub<GraphHub>("/api/signalr/graph");
});


using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<GraphDBContext>().Database.Migrate();
}

app.UseSpaStaticFiles();
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "react-app";
});

app.MapControllers();
app.Run();

static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirector(HttpStatusCode statusCode,
    Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector) =>
    context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = (int)statusCode;
            return Task.CompletedTask;
        }
        return existingRedirector(context);
    };