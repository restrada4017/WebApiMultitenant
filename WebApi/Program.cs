using Api.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Application;
using Api.Utilities;
using Application.Interfaces;
using WebApi.Services;
using WebApi.Extensions;
using Finbuckle.MultiTenant;
using WebApi.Models;

using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure();



builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    ApiAdminDbContext context = scope.ServiceProvider.GetRequiredService<ApiAdminDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerExtension();
app.UseErrorHandlingMiddleware();

app.MapControllers();

app.Run();
