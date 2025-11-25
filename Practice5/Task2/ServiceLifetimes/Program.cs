using Practice5;
using Practice5.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddTransient<IOperationTransient, Operation>();

builder.Services.AddScoped<IOperationScoped, Operation>();

builder.Services.AddSingleton<IOperationSingleton, Operation>();

builder.Services.AddTransient<MyTransientService>();
builder.Services.AddScoped<MyScopedService>();
builder.Services.AddSingleton<MySingletonService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<MyMiddleware>();

app.MapRazorPages();

app.Run();