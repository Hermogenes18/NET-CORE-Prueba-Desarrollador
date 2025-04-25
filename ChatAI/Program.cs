using ChatAI.Services.HuggingFaceAIService.Services;
using ChatAI.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<HuggingFaceSettings>(
    builder.Configuration.GetSection("HuggingFace"));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IAIService, HuggingFaceService>();
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAll");
app.Run();