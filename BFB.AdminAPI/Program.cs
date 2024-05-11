using Autofac;
using Autofac.Extensions.DependencyInjection;
using BFB.AdminAPI.Extensions;
using BFB.AdminAPI.Modules;
using BFB.AuthApi.CustomFilterAttributes;
using BFB.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLib.Configuration;
using SharedLib.Conventions;
using SharedLib.CustomExtensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
	opt.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers(opt =>
{
	opt.ModelValidatorProviders.Clear();
	opt.Filters.Add(new ValidateFilterAttribute());
	opt.Conventions.Add(new CustomRoutingConvention());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(x =>
{
	x.UseSqlServer(builder.Configuration.GetConnectionString("CS1"), option =>
	{
		option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
	});
});
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
var tokenOpt = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepositoryServiceModule()));
builder.Services.AddIdentityViaExtension();
builder.Services.AddCustomTokenAuth(tokenOpt);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCustomExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
