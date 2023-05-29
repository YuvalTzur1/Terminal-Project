using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using TerminalApi.DataBase;
using TerminalApi.Hubs;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    builder.Services.AddTransient<TerminalHub>();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();
    // NLog: catch setup errors
    logger.Error(e, "Stopped program because of exception");
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();