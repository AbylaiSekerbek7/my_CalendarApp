using Microsoft.EntityFrameworkCore;
using Calendar.Infrastructure;
using Calendar.Application.Interfaces;
using Calendar.Application.Services; // Здесь должна быть реализация ParticipantService

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

var app = builder.Build();

// Подключение Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Промежуточное ПО
app.UseAuthorization();
app.MapControllers();

app.Run();
