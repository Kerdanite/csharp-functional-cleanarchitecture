using AspireCsharpFunctional.ApiService.Data;
using AspireCsharpFunctional.ApiService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString("apidb");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();


// Applique les migrations EF Core au démarrage
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapDefaultEndpoints();

app.MapGet("/todos", async (AppDbContext db) =>
    await db.Todos.ToListAsync());


app.MapPost("/todos", async (AppDbContext db, TodoCreateRequest request) =>
{
    var todo = new TodoItem
    {
        Title = request.Title,
        IsDone = request.IsDone
    };

    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todos/{todo.Id}", todo);
});

app.Run();


public record TodoCreateRequest(string Title, bool IsDone);
//// Add services to the container.
//builder.Services.AddProblemDetails();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//app.UseExceptionHandler();

//string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

//app.MapDefaultEndpoints();

//app.Run();

