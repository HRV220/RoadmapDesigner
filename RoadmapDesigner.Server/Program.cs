using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Models.Entity;
using RoadmapDesigner.Server.Repositories;
using RoadmapDesigner.Server.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Text.Json;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем остальные зависимости
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProgramVersionsService, ProgramVersionsService>();
builder.Services.AddScoped<IProgramVersionsRepository, ProgramVersionsRepository>();

builder.Services.AddHttpClient<OAuthService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "TPU";
})
.AddCookie()
.AddOAuth("TPU", options =>
{
    options.ClientId = "ваш_client_id";
    options.ClientSecret = "ваш_client_secret";
    options.CallbackPath = new PathString("/auth/callback");

    options.AuthorizationEndpoint = "https://oauth.tpu.ru/authorize";
    options.TokenEndpoint = "https://oauth.tpu.ru/access_token";
    options.UserInformationEndpoint = "https://api.tpu.ru/v2/auth/user";

    options.SaveTokens = true;

    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    options.ClaimActions.MapJsonSubKey(ClaimTypes.Name, "lichnost", "imya");
    options.ClaimActions.MapJsonSubKey("last_name", "lichnost", "familiya");

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            // Получаем данные пользователя с UserInformationEndpoint
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            request.Headers.Add("apiKey", "ваш_api_key");

            var response = await context.Backchannel.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            context.RunClaimActions(user);
        }
    };
});

// Получаем строку подключения из конфигурации
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Настройка службы базы данных
builder.Services.AddDbContext<RoadmapDesignerContext>(options =>
    options.UseNpgsql(connectionString));

// Настройка контроллеров
builder.Services.AddControllers();

// Настройка Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Добавляем поддержку CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors("AllowAll"); // Используем CORS

// Используем статические файлы
app.UseDefaultFiles();
app.UseStaticFiles();

// Конфигурация HTTP запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();

app.MapGet("/", async context =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        var name = context.User.FindFirst(ClaimTypes.Name)?.Value;
        var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
        await context.Response.WriteAsync($"Привет, {name}! Ваш email: {email}");
    }
    else
    {
        await context.Response.WriteAsync("Вы не вошли в систему. <a href=\"/auth/login\">Войти</a>");
    }
});

app.MapGet("/auth/login", async context =>
{
    await context.ChallengeAsync(
        "TPU",
        new AuthenticationProperties { RedirectUri = "/" });
});



app.MapGet("/auth/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("https://oauth.tpu.ru/auth/logout?redirect=https://ваш_домен");
});

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
