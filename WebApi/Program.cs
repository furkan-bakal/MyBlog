using Core;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using WebApi.Extensions;
using WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>());

builder.Services.AddRepository(builder.Configuration);
builder.Services.AddService();

builder.Services.AddValidatorsFromAssemblyContaining<CoreAssembly>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.AddMiddleware();

app.Run();
