using System;
using System.Collections.Generic;

namespace CatalogoDoces.Models
{
    // Entidade principal salva no Banco de Dados
    public class Pedido
    {
        public int Id { get; set; }
        public string PedidoCodigo { get; set; } = string.Empty;
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public string EnderecoEntrega { get; set; } = string.Empty;
        public string ItensJson { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = "Recebido"; // Recebido, Em Preparo, Saiu para Entrega, Concluído
        public string StatusPagamento { get; set; } = "Aguardando"; // Aguardando, Pago, Rejeitado
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    public class Doce
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;
        public List<Variacao> Variacoes { get; set; } = new();
    }

    public class Variacao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty; // Ex: "Caixa c/ 6", "Caixa c/ 12", "Cento"
        public decimal Preco { get; set; }
    }

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