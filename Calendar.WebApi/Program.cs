using Microsoft.EntityFrameworkCore;
using Calendar.Infrastructure;
using Calendar.Application.Interfaces;
using Calendar.Application.Services;
using Microsoft.AspNetCore.StaticFiles; // Здесь должна быть реализация ParticipantService

var builder = WebApplication.CreateBuilder(args);

// Подключение EF Core с SQLite
builder.Services.AddDbContext<CalendarDbContext>(options =>
    options.UseSqlite("Data Source=calendar.db"));

// Регистрация сервисов
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();

// Добавление контроллеров и Swagger
builder.Services.AddControllers()
.AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".yaml"] = "application/x-yaml";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

// Подключение Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Промежуточное ПО
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
