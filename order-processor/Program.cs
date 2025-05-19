using System;
using System.Net.Http;
using System.Net.Http.Json;
using Dapr.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddDaprClient();

var app = builder.Build();
var http = app.Services.GetRequiredService<IHttpClientFactory>().CreateClient();

// subscriber endpoint
app.MapPost("/orders", async (Order order) =>
{
    Console.WriteLine($">>> Processing reservation for order {order.OrderId}.");
    await http.PostAsJsonAsync("http://localhost:6001/reserve", order);

    Console.WriteLine($">>> Processing payment for order {order.OrderId}.");
    await http.PostAsJsonAsync("http://localhost:6002/charge", order);

    Console.WriteLine($">>> Processing shipping for order {order.OrderId}.");
    await http.PostAsJsonAsync("http://localhost:6003/ship", order);

    Console.WriteLine($">>> processed order {order.OrderId} amount={order.Amount}");

    return Results.Ok();
});

// Dapr subscription declaration
app.MapGet("/dapr/subscribe", () => new[]
{
    new {
        pubsubname = "orders-pubsub",
        topic = "orders",
        route = "orders"
    }
});

app.Run("http://0.0.0.0:6000");

record Order(int OrderId, decimal Amount);