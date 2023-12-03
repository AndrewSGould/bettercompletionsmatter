using TavisApi.Context;
using TavisApi.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tavis.Models;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// DotEnv.Load(options: new DotEnvOptions(probeForEnv: true, ignoreExceptions: false));

if (builder.Environment.EnvironmentName.Trim() == string.Empty)
  configurationBuilder.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);

builder.Configuration.AddConfiguration(configurationBuilder.Build());

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TavisContext>(x => x.UseNpgsql(connectionString));
builder.Services.AddHttpContextAccessor();
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

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
  options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor
                             | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
  options.KnownNetworks.Clear();
  options.KnownProxies.Clear();
});

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(10000);
});

builder.Services.AddCors(options =>
{
  options.AddPolicy("CorsPolicy", build => build
      .WithOrigins(builder.Configuration["ServerConfigs:AudienceServer"])
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());
});

builder.Services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

// TODO: Let's pull out the interface hookups
builder.Services.AddScoped<IParser, Parser>();
builder.Services.AddScoped<IDataSync, DataSync>();
builder.Services.AddScoped<ITA_GameCollection, TA_GameCollection>();
builder.Services.AddScoped<IBcmService, BcmService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IOpenXblService, OpenXblService>();
builder.Services.AddScoped<IDiscordService, DiscordService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseSession();

// var port = "4300";// Environment.GetEnvironmentVariable("PORT");
// if (!string.IsNullOrWhiteSpace(port))
// {
//   app.Urls.Add("http://*:" + port);
// }

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  var context = services.GetRequiredService<TavisContext>();
  context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseDeveloperExceptionPage();
}
else
{
  app.UseHsts();
  app.UseHttpsRedirection();
}

app.UseForwardedHeaders();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<SyncSignal>("/datasync");

app.MapGet("/", () => "Healthy");

app.Run();
