using System.Text;
using AutoMapper;
using EaseDeskMaster_API.Container;
using EaseDeskMaster_API.Helper;
using EaseDeskMaster_API.Model;
using EaseDeskMaster_API.Repos;
using EaseDeskMaster_API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IRefreshHandler, RefreshService>();


builder.Services.AddDbContext<DevMasterDataContext>(o =>
o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection")));

//builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
var _authkey = builder.Configuration.GetValue<string>("JwtSettings:securityKey");
if (string.IsNullOrEmpty(_authkey))
{
	throw new InvalidOperationException("JWT security key is not configured properly.");
}
builder.Services.AddAuthentication(item =>
{
	item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
	item.RequireHttpsMetadata = true;
	item.SaveToken = true;
	item.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authkey)),
		ValidateIssuer = false,
		ValidateAudience = false,
		ClockSkew = TimeSpan.Zero
	};

});

var autoMapper = new MapperConfiguration(item => item.AddProfile(new AutoMapperHandler()));
IMapper mapper = autoMapper.CreateMapper();
builder.Services.AddSingleton(mapper);

string LogPath = builder.Configuration.GetSection("Logging:LogPath").Value;
var _logger = new LoggerConfiguration()
	.MinimumLevel.Debug()
	.MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
	.Enrich.FromLogContext()
	.WriteTo.File(LogPath)
	.CreateLogger();
builder.Logging.AddSerilog(_logger);

var _jwtSetting = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(_jwtSetting);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
