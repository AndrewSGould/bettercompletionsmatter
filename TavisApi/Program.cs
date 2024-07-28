using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using Tavis.Models;
using TavisApi.Context;
using TavisApi.Services;
using TavisApi.V2.Authentication;
using TavisApi.V2.Discord;
using TavisApi.V2.OXbl;
using TavisApi.V2.Users;
using TavisApi.V2.Utils;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder()
		.SetBasePath(builder.Environment.ContentRootPath)
		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
		.AddEnvironmentVariables();

string encryptionKey;
string discordOAuthClientId;
string discordOAuthSecret;

if (builder.Environment.EnvironmentName == "Development") {
	DotEnv.Load(options: new DotEnvOptions(probeForEnv: true, ignoreExceptions: false));
	var envVars = DotEnv.Read();
	encryptionKey = envVars["ENCRYPTION_KEY"];
	discordOAuthClientId = envVars["DISCORD_CLIENTID"];
	discordOAuthSecret = envVars["DISCORD_OAUTH_SECRET"];
}
else {
	encryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY")!;
	discordOAuthClientId = Environment.GetEnvironmentVariable("DISCORD_CLIENTID")!;
	discordOAuthSecret = Environment.GetEnvironmentVariable("DISCORD_OAUTH_SECRET")!;
}

builder.Configuration.AddConfiguration(configurationBuilder.Build());

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TavisContext>(x => {
	x.UseNpgsql(connectionString);
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(opt => {
	opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
	options.TokenValidationParameters = new TokenValidationParameters {
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["ServerConfigs:IssuerServer"],
		ValidAudience = builder.Configuration["ServerConfigs:AudienceServer"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey))
	};
});

builder.Services.Configure<ForwardedHeadersOptions>(options => {
	options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor
														 | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
	options.KnownNetworks.Clear();
	options.KnownProxies.Clear();
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
	options.IdleTimeout = TimeSpan.FromMinutes(10000);
});

builder.Services.AddCors(options => {
	options.AddPolicy("CorsPolicy", build => build
			.WithOrigins(builder.Configuration["ServerConfigs:AudienceServer"], builder.Configuration["ServerConfigs:IssuerServer"])
			.WithOrigins(builder.Configuration["ServerConfigs:AudienceMetaServer"], builder.Configuration["ServerConfigs:IssuerMetaServer"])
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
builder.Services.AddScoped<IRgscService, RgscService>();
builder.Services.AddScoped<IStatsService, StatsService>();
builder.Services.AddScoped<IYearlyService, YearlyService>();
builder.Services.AddScoped<IDiscordServiceV2, DiscordServiceV2>();
builder.Services.AddScoped<ITokenServiceV2, TokenServiceV2>();
builder.Services.AddScoped<IUserServiceV2, UserServiceV2>();
builder.Services.AddScoped<IOpenXblServiceV2, OpenXblServiceV2>();
builder.Services.AddScoped<IUtils, Utils>();

var app = builder.Build();

app.UseSession();

using (var scope = app.Services.CreateScope()) {
	var services = scope.ServiceProvider;

	var context = services.GetRequiredService<TavisContext>();
	context.Database.Migrate();
}

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}
else {
	app.UseHsts();
	app.UseHttpsRedirection();
}

app.UseForwardedHeaders();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<SyncSignal>("/datasync");

app.MapGet("/", () => "Healthy!!");

app.Run();
