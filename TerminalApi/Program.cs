using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;using NLog.Web;using TerminalApi.Controllers;
using TerminalApi.DataBase;
using TerminalApi.Hubs;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();logger.Debug("init main");try{    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();    builder.Logging.ClearProviders();    builder.Host.UseNLog();
    builder.Services.AddSwaggerGen();    builder.Services.AddDbContext<FlightsDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")), ServiceLifetime.Singleton);    builder.Services.AddSingleton<ControlTower>();
    builder.Services.AddTransient<TerminalHub>();    builder.Services.AddSignalR();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();    app.UseRouting();    app.UseCors("CorsPolicy");    app.UseAuthorization();    app.MapControllers();    app.MapHub<TerminalHub>("/terminalhub");    app.Run();}catch (Exception e){
    // NLog: catch setup errors
    logger.Error(e, "Stopped program because of exception");    throw;}finally{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();}