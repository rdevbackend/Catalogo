using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Models // <-- Nome do namespace ajustado para "Catalogo.Models"
{
    // Contexto da Base de Dados SQLite
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Doce> Doces { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
    }

    // Entidade principal de Pedido
    public class Pedido
    {
        public int Id { get; set; }
        public string PedidoCodigo { get; set; } = string.Empty;
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public string EnderecoEntrega { get; set; } = string.Empty;
        public string ItensJson { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = "Recebido";
        public string StatusPagamento { get; set; } = "Aguardando";
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    // Entidade de Doce
    public class Doce
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public string Imagem { get; set; } = string.Empty;
        public string VariacoesJson { get; set; } = string.Empty; // Guardado como JSON para simplificar no SQLite
    }

    // DTOs para receber e responder na API
    public class PedidoRequest
    {
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public string EnderecoEntrega { get; set; } = string.Empty;
        public List<ItemPedido> Itens { get; set; } = new();
        public decimal ValorTotal { get; set; }
    }

    public class ItemPedido
    {
        public string DoceNome { get; set; } = string.Empty;
        public string VariacaoNome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }

    public class PedidoResponse
    {
        public string PedidoId { get; set; } = string.Empty;
        public string ClienteNome { get; set; } = string.Empty;
        public string Status { get; set; } = "Recebido";
        public string StatusPagamento { get; set; } = "Aguardando";
        public decimal ValorTotal { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}