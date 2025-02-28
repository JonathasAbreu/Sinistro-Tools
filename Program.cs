using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ✅ Adiciona serviços para o controlador
builder.Services.AddControllers();

// ✅ Configuração do Swagger (Documentação da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Registra o serviço LojaService para ser injetado nos controllers
builder.Services.AddSingleton<LojaAPI.Services.LojaService>();

// ✅ Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ✅ Ativa Swagger para testes na versão de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ Aplica a política de CORS antes dos controladores
app.UseCors("PermitirTudo");

// ✅ Configura o uso de Controllers
app.UseAuthorization();
app.MapControllers();

app.Run();
