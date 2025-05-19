using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/reserve", () =>
{
    Console.WriteLine("inventory-api received call");
    // Simulate some processing
    System.Threading.Thread.Sleep(350);
    // Return a response
    // In a real-world scenario, you would likely check the inventory and reserve items here
    return Results.Ok(new { status = "reserve ok" });
});

app.Run("http://0.0.0.0:80");