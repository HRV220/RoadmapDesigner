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
builder.Services.AddScoped<GitHubOAuthService>();

builder.Services.AddHttpClient<GitHubOAuthService>();
builder.Services.AddControllers();



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "GitHub";
})
.AddCookie()
.AddOAuth("GitHub", options =>
{
    options.ClientId = "ваш_client_id"; // Укажите Client ID GitHub
    options.ClientSecret = "ваш_client_secret"; // Укажите Client Secret GitHub
    options.CallbackPath = new PathString("/auth/callback");

    options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
    options.TokenEndpoint = "https://github.com/login/oauth/access_token";
    options.UserInformationEndpoint = "https://api.github.com/user";

    options.Scope.Add("user:email");

    options.ClaimActions.MapJsonKey("urn:github:login", "login");
    options.ClaimActions.MapJsonKey("urn:github:id", "id");
    options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

    options.SaveTokens = true;

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.AccessToken);

            var response = await context.Backchannel.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            context.RunClaimActions(user.RootElement);
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

// Роуты для входа и выхода
app.MapGet("/auth/login", async context =>
{
    await context.ChallengeAsync("GitHub");
});

app.MapGet("/auth/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

app.MapGet("/", async context =>
{
    if (context.User.Identity?.IsAuthenticated ?? false)
    {
        var login = context.User.FindFirst("urn:github:login")?.Value;
        var avatar = context.User.FindFirst("urn:github:avatar")?.Value;

        await context.Response.WriteAsync($"<h1>Welcome {login}</h1><img src='{avatar}' />");
    }
    else
    {
        await context.Response.WriteAsync("<a href='/auth/login'>Login with GitHub</a>");
    }
});
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
