using Catalogo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura a ligação ao SQLite a partir do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Adiciona suporte para Controllers API
builder.Services.AddControllers();

var app = builder.Build();

// 3. Cria automaticamente a base de dados SQLite e tabelas se ainda não existirem
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// 4. Configura o servidor para entregar os ficheiros da pasta wwwroot (index.html, carrinho.html, etc.)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();