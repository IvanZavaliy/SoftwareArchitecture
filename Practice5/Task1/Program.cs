var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 1. Підтримка статичних файлів (wwwroot)
app.UseStaticFiles();

// 2. Middleware для логування (приклад custom middleware)
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Path}");
    await next.Invoke();
});

// 3. Маршрутизація
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context => 
        await context.Response.WriteAsync("Home Page"));

    endpoints.MapGet("/hello", async context => 
    {
        // Читання Query String
        if(context.Request.Query.ContainsKey("name"))
        {
            await context.Response.WriteAsync($"Hello, {context.Request.Query["name"]}!");
        }
        else
        {
            await context.Response.WriteAsync("Hello, Stranger!");
        }
    });

    // Параметри маршруту
    endpoints.MapGet("/user/{id:int}", async context =>
        await context.Response.WriteAsync($"User ID: {context.Request.RouteValues["id"]}"));
});

app.Run();