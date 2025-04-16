// Program.cs
using chatbot.Services;
using Microsoft.OpenApi.Models;
using Polly;
using Microsoft.Extensions.Http;
using chatbot.Configuration;
using DotNetEnv;

Env.Load();



var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<AzureOpenAISettings>(options =>
{
    options.Endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? "";
    options.ApiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY") ?? "";
    options.DeploymentId = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_ID") ?? "";
    options.ApiVersion = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_VERSION") ?? "2024-06-01";
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Copilot E-commerce API", Version = "v1" });
});

builder.Services.AddHttpClient<IAiService, AiService>()
    .AddPolicyHandler(Polly.Extensions.Http.HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

builder.Services.AddSingleton<IExcelService, ExcelService>();
builder.Services.AddSingleton<IGenerationService, GenerationService>();
builder.Services.AddSingleton<QueueService>();

builder.Services.AddScoped<ChatOrchestratorService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();

app.Run();
