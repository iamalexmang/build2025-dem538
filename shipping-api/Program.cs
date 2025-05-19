using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/ship", () =>
{
    Console.WriteLine("shipping-api received call");
    // Simulate some processing
    System.Threading.Thread.Sleep(1750);
    return Results.Ok(new { status = "ship ok" });
});

app.Run("http://0.0.0.0:80");