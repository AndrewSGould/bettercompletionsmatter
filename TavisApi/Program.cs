using TavisApi.Context;
using TavisApi.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder()
                            .SetBasePath(builder.Environment.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                            .AddEnvironmentVariables();

if (builder.Environment.EnvironmentName.Trim() == string.Empty)
  configurationBuilder.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);

builder.Configuration.AddConfiguration(configurationBuilder.Build());

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TavisContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddAuthentication(opt =>
{
  opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["ServerConfigs:IssuerServer"],
    ValidAudience = builder.Configuration["ServerConfigs:AudienceServer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
  };
});

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
  options.AddPolicy("CorsPolicy", build => build
    //.WithOrigins(builder.Configuration["ServerConfigs:AudienceServer"])
    //.WithOrigins()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

builder.Services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

//TODO: lets pull out the interface hookups
builder.Services.AddScoped<IParser, Parser>();
builder.Services.AddScoped<IDataSync, DataSync>();
builder.Services.AddScoped<ITA_GameCollection, TA_GameCollection>();
builder.Services.AddScoped<IBcmService, BcmService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();
var port = Environment.GetEnvironmentVariable("PORT"); 

if (!string.IsNullOrWhiteSpace(port)) { 	
  app.Urls.Add("http://*:" + port); 
} 

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<TavisContext>();    
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapHub<SyncSignal>("/datasync");
app.Run();
