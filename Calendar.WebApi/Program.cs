using Microsoft.EntityFrameworkCore;
using Calendar.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CalendarDbContext>(options =>
    options.UseSqlite("Data Source=calendar.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();