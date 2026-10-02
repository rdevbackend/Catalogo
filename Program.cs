using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=doceria.db"));

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers();

var app = builder.Build();

// Garante que o banco de dados e as tabelas sejam criados ao iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors("AllowAll");
app.MapControllers();
app.Run();

// Database Context
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Pedido> Pedidos { get; set; }
}

public class Pedido
{
    public int Id { get; set; }
    public string PedidoCodigo { get; set; } = string.Empty;
    public string ClienteNome { get; set; } = string.Empty;
    public string ClienteTelefone { get; set; } = string.Empty;
    public string EnderecoEntrega { get; set; } = string.Empty;
    public string ItensJson { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "Recebido"; // Recebido, Em Preparo, Saiu para Entrega, Entregue
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public string StatusPagamento { get; set; } = "Aguardando";
}