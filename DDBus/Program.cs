﻿using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using DDBus.Entity;
using DDBus.Services;
using System.Diagnostics;
using DDBus.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  
              .AllowAnyMethod()  
              .AllowAnyHeader();
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DDBusDatabase"));
builder.Services.AddSingleton<CRUD_Service<Account>>();
builder.Services.AddSingleton<CRUD_Service<Stops>>();
builder.Services.AddSingleton<CRUD_Service<Routes>>();







var app = builder.Build();

app.UseStaticFiles();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
