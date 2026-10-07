using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalogo.Models;

namespace DoceriaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PedidosController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] PedidoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ClienteNome) || string.IsNullOrWhiteSpace(dto.ClienteTelefone))
            {
                return BadRequest(new { mensagem = "Nome e Telefone são obrigatórios." });
            }

            var codigo = "DOCE-" + new Random().Next(1000, 9999);
            var pedido = new Pedido
            {
                PedidoCodigo = codigo,
                ClienteNome = dto.ClienteNome,
                ClienteTelefone = dto.ClienteTelefone,
                EnderecoEntrega = dto.EnderecoEntrega,
                ItensJson = System.Text.Json.JsonSerializer.Serialize(dto.Itens),
                ValorTotal = dto.ValorTotal,
                Status = "Recebido",
                StatusPagamento = "Aguardando"
            };

            _db.Pedidos.Add(pedido);
            await _db.SaveChangesAsync();

            return Ok(new { pedidoId = codigo, status = pedido.Status, valorTotal = pedido.ValorTotal });
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> ObterStatus(string codigo)
        {
            var pedido = await _db.Pedidos.FirstOrDefaultAsync(p => p.PedidoCodigo == codigo.ToUpper());
            if (pedido == null) return NotFound(new { mensagem = "Pedido não encontrado." });

            return Ok(pedido);
        }

        // --- NOVAS ROTAS PARA O PAINEL DA SUA CLIENTE ---

        // GET: api/pedidos/faturamento
        [HttpGet("faturamento")]
        public async Task<IActionResult> ObterFaturamento()
        {
            var hoje = DateTime.Now;

            // Faturamento da Semana Atual
            var inicioSemana = hoje.AddDays(-(int)hoje.DayOfWeek);
            var totalSemana = await _db.Pedidos
                .Where(p => p.StatusPagamento == "Pago" || p.StatusPagamento == "pago" || p.StatusPagamento == "Recebido")
                .Where(p => p.DataCriacao >= inicioSemana)
                .SumAsync(p => (decimal?)p.ValorTotal) ?? 0;

            // Faturamento do Mês Atual
            var totalMes = await _db.Pedidos
                .Where(p => p.StatusPagamento == "Pago" || p.StatusPagamento == "pago" || p.StatusPagamento == "Recebido")
                .Where(p => p.DataCriacao.Month == hoje.Month && p.DataCriacao.Year == hoje.Year)
                .SumAsync(p => (decimal?)p.ValorTotal) ?? 0;

            // Faturamento do Ano Atual
            var totalAno = await _db.Pedidos
                .Where(p => p.StatusPagamento == "Pago" || p.StatusPagamento == "pago" || p.StatusPagamento == "Recebido")
                .Where(p => p.DataCriacao.Year == hoje.Year)
                .SumAsync(p => (decimal?)p.ValorTotal) ?? 0;

            return Ok(new { totalSemana, totalMes, totalAno });
        }

        // GET: api/pedidos/todos
        [HttpGet("todos")]
        public async Task<IActionResult> ObterTodosPedidos()
        {
            var lista = await _db.Pedidos
                .OrderByDescending(p => p.DataCriacao)
                .ToListAsync();

            return Ok(lista);
        }

        // PUT: api/pedidos/status/5
        [HttpPut("status/{id}")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] StatusDTO dto)
        {
            var pedido = await _db.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound("Pedido não encontrado.");

            if (!string.IsNullOrEmpty(dto.StatusPedido))
                pedido.Status = dto.StatusPedido;

            if (!string.IsNullOrEmpty(dto.StatusPagamento))
                pedido.StatusPagamento = dto.StatusPagamento;

            await _db.SaveChangesAsync();
            return Ok(new { mensagem = "Status atualizado com sucesso!" });
        }
    }

    public class PedidoDTO
    {
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public string EnderecoEntrega { get; set; } = string.Empty;
        public List<ItemDTO> Itens { get; set; } = new();
        public decimal ValorTotal { get; set; }
    }

    public class ItemDTO
    {
        public string DoceNome { get; set; } = string.Empty;
        public string VariacaoNome { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
    }

    public class StatusDTO
    {
        public string? StatusPedido { get; set; }
        public string? StatusPagamento { get; set; }
    }
}