using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//first middleware
app.Use(async (context, next) =>
{
    Console.WriteLine("Hello from the first middleware at start");
    context.Items["message"] = "Hello from the first middleware at start";
    context.Items["payload"] = "payload::123";
    await next();
    context.Items["message"] = "Hello from the first middleware at end";

});


//second middleware
app.Use(async (context, next) =>
{
    Console.WriteLine("Hello from the second middleware at start");
    Console.WriteLine($"Message: {context.Items["message"]}");
    Console.WriteLine($"Payload: {context.Items["payload"]}");
    context.Items["message"] = "Hello from the second middleware at start";

    await next();
    // context.Items["message"] = "Hello from the second middleware at end";
    Console.WriteLine("Hello from the second middleware at end");
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
