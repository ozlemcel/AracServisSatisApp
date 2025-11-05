using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Controller servisini ekle
builder.Services.AddControllers();

// Swagger (OpenAPI) servisini ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Arac Servis Satis API",
        Version = "v1",
        Description = "Bu API, araç servis ve satýþ iþlemleri için geliþtirilmiþtir."
    });
});

var app = builder.Build();

// Geliþtirme ortamýnda Swagger ve arayüzünü aktif et
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();        // Swagger JSON
    app.UseSwaggerUI(c =>    // Swagger UI arayüzü
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arac Servis Satis API v1");
        c.RoutePrefix = string.Empty; // Swagger'ý ana sayfada aç (örn. https://localhost:5001/)
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
