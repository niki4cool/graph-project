using GraphEditor;
using GraphEditor.Auth;
using GraphEditor.CRUD;
using GraphEditor.Hubs;
using GraphEditor.Model;
using GraphEditor.Model.GraphModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddCors(o =>
{
    o.AddPolicy(Environments.Development, c =>
    {
        c.AllowAnyHeader()
         .AllowAnyMethod()
         .SetIsOriginAllowed(_ => true)
         .AllowCredentials();
    });
});

services.AddControllers();

services.AddDbContext<GraphDBContext>(
    c =>
    {
        var isContainered = Environment.OSVersion.Platform == PlatformID.Unix;
        var connectionString = isContainered 
        ? configuration["ConnectionStrings:DefaultConnection"]
        : configuration["ConnectionStrings:LocalDefaultConnection"];
        c.UseNpgsql(connectionString);
    });


services.AddScoped<UserRecordValidator>();

services.AddScoped<IRepository<Graph>, GraphRepository>();
services.AddScoped<IUserStore<User>, UserStore>();

services.AddSignalR()
        .AddJsonProtocol();

services.AddIdentityCore<User>();

services.AddSwaggerGen();

services.AddSpaStaticFiles(c =>
{
    c.RootPath = "react-app";
});

var keySecret = configuration["JwtSigningKey"];
var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keySecret!));

services.AddSingleton(_ => new JwtGenerator(symmetricKey));

services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters.ValidateIssuerSigningKey = true;
    options.TokenValidationParameters.IssuerSigningKey = symmetricKey;
    options.TokenValidationParameters.ValidAudience = JwtGenerator.TokenAudience;
    options.TokenValidationParameters.ValidIssuer = JwtGenerator.TokenIssuer;
});

services.AddSingleton<IAuthorizationHandler, GraphAuthorizationCRUDHandler>();

services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
       .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
       .RequireAuthenticatedUser().Build());
    //options.AddPolicy(StringConstants.GraphCRUDPolicy, policy =>
    //    policy.Requirements.Add(Operations.Create));
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