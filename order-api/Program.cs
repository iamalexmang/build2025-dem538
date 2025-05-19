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
var dapr = app.Services.GetRequiredService<DaprClient>();

app.MapPost("/orders", async (Order order) =>
{
    // baseline synchronous fan‑out (3 HTTP calls)
    await http.PostAsJsonAsync("http://localhost:6001/reserve", order);
    await http.PostAsJsonAsync("http://localhost:6002/charge", order);
    await http.PostAsJsonAsync("http://localhost:6003/ship", order);

    // await dapr.PublishEventAsync("orders-pubsub", "orders", order);

    return Results.Accepted();
});

app.Run("http://0.0.0.0:80");

record Order(int OrderId, decimal Amount);