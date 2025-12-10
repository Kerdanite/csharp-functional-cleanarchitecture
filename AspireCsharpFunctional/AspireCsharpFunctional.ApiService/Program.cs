using AspireCsharpFunctional.ApiService.Endpoints;
using Microsoft.EntityFrameworkCore;
using VetCalendar.Application;
using VetCalendar.Infrastructure;
using VetCalendar.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);



var app = builder.Build();




// DEV ONLY : drop + recreate le schéma à chaque démarrage
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<VetCalendarDbContext>();

    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapDefaultEndpoints();

app.MapClientEndpoints();



app.Run();


