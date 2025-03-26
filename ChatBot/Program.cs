using ChatBot.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ajouter Entity Framework Core et SQL Server
builder.Services.AddDbContext<ChatBotDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.Run();
