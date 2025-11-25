using Practice5;
using Practice5.Services;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// 1. РЕЄСТРАЦІЯ СЕРВІСІВ (Аналог ConfigureServices)
// =========================================================

// Додаємо підтримку Razor Pages (як у відео)
builder.Services.AddRazorPages();

// --- Реєстрація основних операцій (Operation) ---
// Кожен раз новий
builder.Services.AddTransient<IOperationTransient, Operation>();

// Один на HTTP-запит
builder.Services.AddScoped<IOperationScoped, Operation>();

// Один на весь час життя програми
builder.Services.AddSingleton<IOperationSingleton, Operation>();

// --- Реєстрація сервісів-обгорток (Service Wrappers) ---
// Це потрібно, щоб побачити, як поводять себе вкладені сервіси
builder.Services.AddTransient<MyTransientService>();
builder.Services.AddScoped<MyScopedService>();
builder.Services.AddSingleton<MySingletonService>();

var app = builder.Build();

// =========================================================
// 2. КОНФІГУРАЦІЯ PIPELINE (Аналог Configure)
// =========================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// --- Додаємо наше кастомне Middleware ---
// Воно має бути додане перед MapRazorPages, щоб перехопити запит
app.UseMiddleware<MyMiddleware>();

app.MapRazorPages();

app.Run();